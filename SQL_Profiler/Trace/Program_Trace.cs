using System.Windows.Forms; // for Application.DoEvents();

namespace SQL_Profiler
{
    partial class Program
    {
        static bool dataReady()
        {
            bool res = false;
            try
            {
                // TODO writetime stamp to database 
                res = reader.Read();
            }
            catch
            {
                res = false;
            }
            Application.DoEvents();
            return res;
        }


    }
}
