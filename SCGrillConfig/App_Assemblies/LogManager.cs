using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlServerCe;
using Elmah;

namespace SCGrillConfig
{
    public class LogManager
    {
        public static void Log(string message, string detail, string source)
        {
            // writes the information to the default ErrorLog
            Error myError = new Error();
            myError.Message = message;
            myError.Time = DateTime.UtcNow;
            myError.HostName = HttpContext.Current.Request.UserHostName;
            myError.StatusCode = 0;
            myError.Type = "Log";
            myError.Detail = detail;
            myError.User = HttpContext.Current.User.Identity.Name.Length > 0 ? HttpContext.Current.User.Identity.Name : "None";
            myError.ApplicationName = HttpContext.Current.Request.Url.Host;
            myError.Source = source;
            ErrorLog.GetDefault(null).Log(myError);
        }

        public static void Log(string message, string detail)
        {
            // writes the information to the default ErrorLog
            Error myError = new Error();
            myError.Message = message;
            myError.Time = DateTime.UtcNow;
            myError.HostName = HttpContext.Current.Request.UserHostName;
            myError.StatusCode = 0;
            myError.Type = "Log";
            myError.Detail = detail;
            myError.User = HttpContext.Current.User.Identity.Name.Length > 0 ? HttpContext.Current.User.Identity.Name : "None";
            myError.ApplicationName = HttpContext.Current.Request.Url.Host;
            myError.Source = "";
            ErrorLog.GetDefault(null).Log(myError);
        }

        public static void Log(string message)
        {
            // writes the information in logEntry to the default ErrorLog
            Error myError = new Error();
            myError.Message = message;
            myError.Time = DateTime.UtcNow;
            myError.HostName = HttpContext.Current.Request.UserHostName;
            myError.StatusCode = 0;
            myError.Type = "Log";
            myError.Detail = message;
            myError.User = HttpContext.Current.User.Identity.Name.Length > 0 ? HttpContext.Current.User.Identity.Name : "None";
            myError.ApplicationName = HttpContext.Current.Request.Url.Host;
            myError.Source = "";
            ErrorLog.GetDefault(null).Log(myError);
        }

        public static void CreateLogTable(string ConnectionString)
        {
            // creates the ELMAH_Error table needed for ELMAH logging in the database specified by the ConnectionString
            // check for valid input
            if (ConnectionString.Length > 0)
            {
                //var connectionString = ConfigurationManager.ConnectionStrings["LoggingTest1Context"].ConnectionString;
                var conn = new SqlCeConnection(ConnectionString);
                var cmd = new SqlCeCommand();
                conn.Open();
                // check if the connection is open
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    // check to see if the ELMAH_Error table already exists
                    var transaction = conn.BeginTransaction();
                    cmd.Connection = conn;
                    cmd.Transaction = transaction;
                    cmd.CommandText = @"SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'ELMAH_Error'";
                    var obj = cmd.ExecuteScalar();
                    if (obj == null)
                    {
                        // the ELMAH_Error table does not exist so create it and its index
                        cmd.CommandText = @"CREATE TABLE ELMAH_Error (
                                    [ErrorId] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT newid(),
                                    [Application] NVARCHAR(60) NOT NULL,
                                    [Host] NVARCHAR(50) NOT NULL,
                                    [Type] NVARCHAR(100) NOT NULL,
                                    [Source] NVARCHAR(60) NOT NULL,
                                    [Message] NVARCHAR(500) NOT NULL,
                                    [User] NVARCHAR(50) NOT NULL,
                                    [StatusCode] INT NOT NULL,
                                    [TimeUtc] DATETIME NOT NULL,
                                    [Sequence] INT IDENTITY (1, 1) NOT NULL,
                                    [AllXml] NTEXT NOT NULL
                                    )";
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = @"CREATE NONCLUSTERED INDEX [IX_Error_App_Time_Seq] ON [ELMAH_Error] (
                                    [Application]   ASC,
                                    [TimeUtc]       DESC,
                                    [Sequence]      DESC
                                    )";
                        cmd.ExecuteNonQuery();
                        transaction.Commit(CommitMode.Immediate);
                        // clean up
                        conn.Close();
                        transaction = null;
                        cmd = null;
                        conn = null;
                        obj = null;
                    } // if (obj == null)
                } // if (conn.State == System.Data.ConnectionState.Open)
            } // if (ConnectionString.Length > 0)
        } // public void CreateLogTable(string ConnectionString)

    } // public class LogManager

}
