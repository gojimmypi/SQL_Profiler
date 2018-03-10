using System;
using Microsoft.ApplicationBlocks.Data;
using System.Data.SqlClient;
using System.Data;

namespace SQL_Profiler
{
    partial class Program
    {

        static string readerParam(string fieldName)
        {
            string res;
            try
            {
                if (reader is null)
                {
                    res = null;
                }
                else if (reader[fieldName].Equals(DBNull.Value))
                {
                    res = null;
                }
                else
                {
                    res = reader[fieldName].ToString();
                }
            }
            catch
            {
                res = null;
            }

            return res;
        }

        static void sqlSaveStatementHistory()
        {
            SqlParameter[] paramSQL = new SqlParameter[16];
            string strTextData = "";
            paramSQL[1] = new SqlParameter("@LoginName", SqlDbType.VarChar, 128);
            paramSQL[1].Value = readerParam(@"LoginName");

            paramSQL[2] = new SqlParameter("@HostName", SqlDbType.VarChar, 128);
            paramSQL[2].Value = readerParam("HostName");

            paramSQL[3] = new SqlParameter("@ServerName", SqlDbType.VarChar, 128);
            paramSQL[3].Value = readerParam("ServerName");

            paramSQL[4] = new SqlParameter("@DatabaseName", SqlDbType.VarChar, 128);
            paramSQL[4].Value = readerParam("DatabaseName");

            paramSQL[5] = new SqlParameter("@RowCounts", SqlDbType.Int);
            paramSQL[5].Value = readerParam("RowCounts");

            paramSQL[6] = new SqlParameter("@Error", SqlDbType.Int);
            paramSQL[6].Value = readerParam("Error");

            paramSQL[7] = new SqlParameter("@CPU", SqlDbType.Int);
            paramSQL[7].Value = readerParam("CPU");

            paramSQL[8] = new SqlParameter("@Reads", SqlDbType.Int);
            paramSQL[8].Value = readerParam("Reads");

            paramSQL[9] = new SqlParameter("@Writes", SqlDbType.Int);
            paramSQL[9].Value = readerParam("Writes");

            paramSQL[10] = new SqlParameter("@Duration", SqlDbType.Int);
            paramSQL[10].Value = readerParam("Duration");

            paramSQL[11] = new SqlParameter("@SPID", SqlDbType.Int);
            paramSQL[11].Value = readerParam("SPID");

            paramSQL[12] = new SqlParameter("@StartTime", SqlDbType.DateTime);
            paramSQL[12].Value = readerParam("StartTime");

            paramSQL[13] = new SqlParameter("@EndTime", SqlDbType.DateTime);
            paramSQL[13].Value = readerParam("EndTime");

            paramSQL[14] = new SqlParameter("@DBID", SqlDbType.Int);
            paramSQL[14].Value = readerParam("DBID");

            paramSQL[15] = new SqlParameter("@TextData", SqlDbType.VarChar);
            strTextData = readerParam("TextData");
            if (strTextData == null)
            {
                strTextData = "";
            }
            paramSQL[15].Value = strTextData;

            if (mySPID != (Convert.ToInt32(readerParam("SPID"))))
            {
                String strSQL = "dbo.proc_add_statement_history";
                SqlHelper.ExecuteScalar(ConnectionString(ServerName, DatabaseName), CommandType.StoredProcedure, strSQL, paramSQL);

            }

        }
    }
}
