
namespace SQL_Profiler
{
    partial class Program
    {
        //***********************************************************************************************************************************
        static string ConnectionString(string strServerName, string strDatabaseName)
        //***********************************************************************************************************************************
        {
            // see http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpref/html/frlrfSystemDataSqlClientSqlConnectionClassConnectionStringTopic.asp
            //
            // there is some debate as to whether the Oledb provider is indeed faster than the native client!
            //  
            return "Workstation ID=HA_SQL_Profiler;" +
                   "packet size=8192;" +
                   "Persist Security Info=false;" +
                   "Server=" + strServerName + ";" +
                   "Database=" + strDatabaseName + ";" +
                   "Trusted_Connection=true; " +
                   "Network Library=dbmssocn;" +
                   "Pooling=True; " +
                   "Enlist=True; " +
                   "Connection Lifetime=14400; " +
                   "Max Pool Size=1; Min Pool Size=1";
        }
    }
}
