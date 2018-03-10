using System;
using Microsoft.ApplicationBlocks.Data;
using System.Data;

namespace SQL_Profiler
{
    partial class Program
    {
        static void recordHeartBeat()
        {
            if ((DateTime.Now - lastHeartbeat).TotalMilliseconds > 1000) {
                lastHeartbeat = DateTime.Now;
                int thisSPID = -1;
                string strSQL = "UPDATE dbo.heartbeat SET timestamp = getdate(); select @@SPID";

                try
                {
                    bool result = int.TryParse(SqlHelper.ExecuteScalar(ConnectionString(ServerName, DatabaseName), CommandType.Text, strSQL).ToString(), out thisSPID);
                    if (result)
                    {
                        mySPID = thisSPID;
                    }
                    else
                    {
                        mySPID = -1;
                    }
                }
                catch
                {
                    mySPID = -1;
                }
            }
        }
    }
}
