
using System;

using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Trace;
using System.Windows.Forms; // for Application.DoEvents();
using System.Runtime.InteropServices; // for DllImport
using System.IO;

// Note 32bit C:\Program Files (x86)\Microsoft SQL Server\110\Tools\Binn\PFCLNT.DLL  (cannot be a 64 bit app)

namespace SQL_Profiler
{
    public class StopAndCloseTraceOnExit
    {
        private TraceServer trace;

        public StopAndCloseTraceOnExit(TraceServer trace)
        {
            this.trace = trace;
            SetConsoleCtrlHandler(new HandlerRoutine(ConsoleCtrlCheck), true);
        }

        [DllImport("Kernel32")]
        public static extern bool SetConsoleCtrlHandler(HandlerRoutine Handler, bool Add);

        public delegate bool HandlerRoutine(CtrlTypes CtrlType);

        public enum CtrlTypes
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT,
            CTRL_CLOSE_EVENT,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT
        };

        private bool ConsoleCtrlCheck(CtrlTypes ctrlType)
        {
            if (trace != null)
            {
                Console.WriteLine("Stopping trace ...\n");
                trace.Stop();

                Console.WriteLine("Closing trace ...\n");
                trace.Close();
            }

            return true;
        }
    }
    partial class Program
    {
        // see https://technet.microsoft.com/en-us/library/ms345134%28v=sql.90%29.aspx?f=255&MSPPError=-2147217396
        //     https://blogs.msdn.microsoft.com/sqlprogrammability/2006/05/25/programmatically-receiving-profiler-events-in-real-time-from-sql-server-2005/
        //     https://www.mssqltips.com/sqlservertutorial/3504/sql-server-profiler-data-columns-explained/
        //     https://docs.microsoft.com/en-us/sql/relational-databases/event-classes/sql-server-event-class-reference

        const string ServerName = "localhost";
        const string DatabaseName = "sql_profile_log";

        static TraceServer reader = new TraceServer();
        static ConnectionInfoBase ci = new SqlConnectionInfo(ServerName);
        static int mySPID = -1;
        // static DateTime thisHeartbeat = DateTime.Now;
        static DateTime lastHeartbeat = DateTime.Parse("1/1/1990");

        static void init()
        {
            try
            {
                // Clean up trace on process exit
                StopAndCloseTraceOnExit traceCleanup = new StopAndCloseTraceOnExit(reader);

                // Start a trace using the trace definition file (passed as first command line parameters)
                Console.WriteLine("Starting trace ...\n");

                //atring strTraceFile = @"..\..\..\doc\Standard.tdf";  // anyCPU path to doc
                string strTraceFile = @"..\..\..\doc\Standard.tdf";  // anyCPU path to doc

                ((SqlConnectionInfo)ci).UseIntegratedSecurity = true;
                // reader.InitializeAsReader(ci, @"..\..\..\doc\Standard.tdf");  // anyCPU path
                if (File.Exists(strTraceFile)) {
                    reader.InitializeAsReader(ci, strTraceFile); // this is a template file created in SQL Profiler! e.g.  \Program Files\Microsoft SQL Server\90\Tools\Profiler\Templates folder
                                                                    // C:\Users\%USERNAME%\AppData\Roaming\Microsoft\SQL Profiler\11.0\Templates\Microsoft SQL Server\110
                                                                    // or \Program Files (x86)\Microsoft SQL Server\110\Tools\Profiler\Templates\Microsoft SQL Server\110
                }
                else
                {
                    Console.WriteLine("Trace file " + Path.GetFullPath(strTraceFile) + " not found in " + Directory.GetCurrentDirectory());
                }
            }
            catch (SqlTraceException ex)
            {
                Console.WriteLine(ex.Message);

                if (ex.InnerException != null)
                    Console.WriteLine(ex.InnerException.Message);
            }
        }

        static void Main(string[] args)
        {
            init();
            // Reading events
            while (reader.Read())
            {
                try
                {
                    recordHeartBeat();
                    if (mySPID != (Convert.ToInt32(readerParam("SPID"))))
                    {
                        sqlSaveStatementHistory();
                        Console.WriteLine("mySPID: " + mySPID.ToString());
                        Console.WriteLine("Event : " + reader["EventClass"]);
                        Console.WriteLine("SPID  : " + reader["SPID"]);
                        Console.WriteLine("Login : " + reader["NTUserName"]);
                        Console.WriteLine("Object: " + reader["LoginName"]);
                        Console.WriteLine("Text  : " + reader["TextData"]);
                        Console.WriteLine();
                    }
                    Console.Write("Skipping " + mySPID.ToString());
                    Application.DoEvents();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        } // main
    } // class
} // namespace

