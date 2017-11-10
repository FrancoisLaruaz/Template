using System;
using System.Data;
using Commons;
using Models.BDDObject;
using System.Xml;
using System.Collections.Generic;


namespace DataAccess
{
    public class BaseDAL : IDisposable
    {
        protected DBConnect db;

        public BaseDAL()
        {
            db = new DBConnect();
        }

        public void Dispose()
        {
            db.Dispose();
        }





    }
}
