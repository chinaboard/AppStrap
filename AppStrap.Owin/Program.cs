using AppStrap.Owin.WebAPI;
using AppStrap.Utils;
using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AppStrap.Owin
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Clear Log?");
            if (Console.ReadKey(false).Key == ConsoleKey.Y)
            {
                File.Delete(Utils.AppStrapLog.LogFilePath);
                Console.WriteLine("\r\nThe log has been cleared.");
            }
            else
            {
                Console.Clear();
            }
            var baseAddress = String.Format("http://*:{0}/", "5422");
            WebApp.Start<WebAPIStartup>(url: baseAddress);

            CoreService.Start();
            Scheduler scheduler = new Scheduler();
            scheduler.Start(TimeSpan.FromSeconds(10), () => Lookup());

            CoreService.Stop();
            Thread.Sleep(Timeout.Infinite);
        }

        static void Lookup()
        {
            Console.Clear();
            //foreach (var x in AppStrapList.Instance.List)
            //{
            //    AppStrapLog.Info(x.Key, x.Value.Status);
            //}
            Console.WriteLine("--------" + DateTime.Now + "--------");
            AppStrapLog.LogList.ToList().ForEach(t => Console.WriteLine(t));
        }
    }
}
