using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Data.Entity.Core.EntityClient;

namespace camdochieuduong.Function
{
    public class SingleConnection
    {
        private SingleConnection() { }
        private static SingleConnection _ConsString = null;
        private String _String = null;
        public static string ConString
        {
            get
            {
                if (_ConsString == null)
                {
                    _ConsString = new SingleConnection { _String = SingleConnection.Connect() };
                    return _ConsString._String;
                }
                else
                    return _ConsString._String;
            }
        }
        public static string Connect()
        {
            //Build an SQL connection string  
            SqlConnectionStringBuilder sqlString = new SqlConnectionStringBuilder()
            {
                //DataSource = "DESKTOP-6K56T6E\\SQLEXPRESS", // Server name 
                DataSource = "localhost\\SQLEXPRESS", // Server name   
                InitialCatalog = "camdochieuduong",  //Database  
                UserID = "sa",         //Username  
                Password = "hathanh",  //Password  
            };
            //Build an Entity Framework connection string  
            EntityConnectionStringBuilder entityString = new EntityConnectionStringBuilder()
            {
                Provider = "System.Data.SqlClient",
                Metadata = "res://*/Model.Model1.csdl|res://*/Model.Model1.ssdl|res://*/Model.Model1.msl",
                ProviderConnectionString = sqlString.ToString()
            };
            return entityString.ConnectionString;
        }
    }
}
