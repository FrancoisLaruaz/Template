using System;
using System.Globalization;
using System.IO;
using System.Web;

namespace Commons
{
    public class FileHelper
    {
		/// <summary>
		/// Deletes the specified document.
		/// </summary>
		/// <returns><c>true</c>, if document was deleted, <c>false</c> otherwise.</returns>
		/// <param name="cheminDocument">Chemin document.</param>
		public static bool DeleteDocument(string FilePath)
		{
			bool result = true;
			try
			{
				if (!String.IsNullOrEmpty(FilePath))
				{
					string FileName = Path.GetFileName(FilePath);
					string urlRelative = FilePath.Replace(FileName, "");
					string urlAbsolue = GetStorageRoot(urlRelative) + FileName;
					System.IO.File.Delete(urlAbsolue);
				}
			}
			catch (Exception e)
			{
				result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "FilePath = " + FilePath);
            }
			return result;
		}


		/// <summary>
		/// Fonction déterminant si un document est une image en fonction de son extension
		/// </summary>
		/// <returns><c>true</c>, if image was ised, <c>false</c> otherwise.</returns>
		/// <param name="urlDocument">URL document.</param>
		public static bool IsImage(string urlDocument)
		{
			bool result = false;
			try
			{

				string extensionFichier = Path.GetExtension(urlDocument);
				// On regarde ensuite si l'extension est dans la liste des extensions "images"
				if (Const.ListeExtensionsImage.Contains(extensionFichier.ToUpper()))
					result = true;

			}
			catch (Exception e)
			{
				result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "urlDocument = " + urlDocument);
            }
			return result;
		}


		/// <summary>
		/// Fonction déterminant si un document est une  video en fonction de son extension
		/// </summary>
		/// <returns><c>true</c>, if image was ised, <c>false</c> otherwise.</returns>
		/// <param name="urlDocument">URL document.</param>
		public static bool IsVideo(string urlDocument)
		{
			bool result = false;
			try
			{

				string extensionFichier = Path.GetExtension(urlDocument);
				// On regarde ensuite si l'extension est dans la liste des extensions "images"
				if (Const.ListeExtensionsVideos.Contains(extensionFichier))
					result = true;

			}
			catch (Exception e)
			{
				result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "urlDocument = " + urlDocument);
            }
			return result;
		}



		/// <summary>
		/// Retourne l'adresse absolue d'un répertoire sur le serveur
		/// </summary>
		/// <returns>The storage root.</returns>
		/// <param name="url">URL.</param>
		public static string GetStorageRoot(string url)
		{
            try
            {
                return Path.Combine(System.Web.HttpContext.Current.Server.MapPath(url));
            }
            catch(Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "url = " + url);
            }
            return "";
		}
    }
}
