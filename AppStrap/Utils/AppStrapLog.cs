using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppStrap.Utils
{
    public static class AppStrapLog
    {
        private const string file = @"\Log.txt";

        public static IEnumerable<string> LogList { get { return File.ReadAllLines(file); } }


        public static void Info(string appName, string message, Action<string> action = null)
        {
            var str = string.Format("[{0}][{1}]:{2}  {3}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), "Info", appName, message);
            Log(str);
        }
        public static void Error(string appName, string message, Action<string> action = null)
        {
            var str = string.Format("[{0}][{1}]:{2}  {3}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), "Error", appName, message);
            Log(str);
        }

        private static void Log(string message, Action<string> action = null)
        {
            if (action == null)
                action = Console.WriteLine;

            action(message);

            File.AppendAllText(file, message + Environment.NewLine);
        }

    }
}
