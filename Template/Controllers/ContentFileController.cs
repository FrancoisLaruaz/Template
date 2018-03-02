﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.IO;
using i18n;
using System.Threading;
using System.Security.Cryptography;
using Website.ContentFileHelper;
using Commons;

namespace Website.Controllers
{

    /*
     *  https://stackoverflow.com/questions/936626/how-can-i-force-a-hard-refresh-ctrlf5/6439351#6439351
     */

    public sealed class ContentFileController : Controller
    {
        #region Hash calculation, caching and invalidation on file change

        private static readonly Dictionary<string, string> _hashByContentUrl = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private static readonly Dictionary<string, ContentData> _dataByHash = new Dictionary<string, ContentData>(StringComparer.Ordinal);
        private static readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
        private static readonly object _watcherLock = new object();
        private static FileSystemWatcher _watcher;

        internal static string ContentHashUrl(string contentUrl, string contentType, HttpContextBase httpContext, UrlHelper urlHelper)
        {
            EnsureWatching(httpContext);

            _lock.EnterUpgradeableReadLock();
            try
            {
                string hash;
                if (!_hashByContentUrl.TryGetValue(contentUrl, out hash))
                {
                    var contentPath = httpContext.Server.MapPath(contentUrl);

                    // Calculate and combine the hash of both file content and path
                    byte[] contentHash;
                    byte[] urlHash;
                    using (var hashAlgorithm = MD5.Create())
                    {
                        using (var fileStream = System.IO.File.Open(contentPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                            contentHash = hashAlgorithm.ComputeHash(fileStream);
                        urlHash = hashAlgorithm.ComputeHash(Encoding.ASCII.GetBytes(contentPath));
                    }
                    hash = contentUrl.Replace("~", "").Replace("/", "_").Replace("\\", "_").Replace("?", "_");

                    _lock.EnterWriteLock();
                    try
                    {
                        _hashByContentUrl[contentUrl] = hash;
                        _dataByHash[hash] = new ContentData { ContentUrl = contentUrl, ContentType = contentType };
                    }
                    finally
                    {
                        _lock.ExitWriteLock();
                    }
                }

                return urlHelper.Action("Get", "ContentFile", new { hash });
            }
            catch (Exception e)
            {
                Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "contentUrl= "
                + contentUrl
                + " and contentType= "
                + contentType
                );
            }
            finally
            {
                _lock.ExitUpgradeableReadLock();
            }
            return null;
        }

        private static void EnsureWatching(HttpContextBase httpContext)
        {
            try
            {
                if (_watcher != null)
                    return;

                lock (_watcherLock)
                {
                    if (_watcher != null)
                        return;

                    var contentRoot = httpContext.Server.MapPath("/");
                    _watcher = new FileSystemWatcher(contentRoot) { IncludeSubdirectories = true, EnableRaisingEvents = true };
                    var handler = (FileSystemEventHandler)delegate (object sender, FileSystemEventArgs e)
                    {
                        // TODO would be nice to have an inverse function to MapPath.  does it exist?
                        var changedContentUrl = "~" + e.FullPath.Substring(contentRoot.Length - 1).Replace("\\", "/");
                        _lock.EnterWriteLock();
                        try
                        {
                            // if there is a stored hash for the file that changed, remove it
                            string oldHash;
                            if (_hashByContentUrl.TryGetValue(changedContentUrl, out oldHash))
                            {
                                _dataByHash.Remove(oldHash);
                                _hashByContentUrl.Remove(changedContentUrl);
                            }
                        }
                        finally
                        {
                            _lock.ExitWriteLock();
                        }
                    };
                    _watcher.Changed += handler;
                    _watcher.Deleted += handler;
                }
            }
            catch (Exception e)
            {
                Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
        }

        private sealed class ContentData
        {
            public string ContentUrl { get; set; }
            public string ContentType { get; set; }
        }

        #endregion

        public ActionResult Get(string hash)
        {
            try
            {
                _lock.EnterReadLock();
                // set a very long expiry time
                Response.Cache.SetExpires(DateTime.Now.AddYears(1));
                Response.Cache.SetCacheability(HttpCacheability.Public);

                // look up the resource that this hash applies to and serve it
                ContentData data;

                if (_dataByHash.TryGetValue(hash, out data))
                {
                    return new FilePathResult(data.ContentUrl, data.ContentType);
                }
                else
                {
                    Logger.GenerateInfo("Resource not found : " + hash);
                }

            }
            catch (Exception e)
            {
                Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType,"hash = " + hash);
            }
            finally
            {
                _lock.ExitReadLock();
            }
            return null;
        }
    }
}