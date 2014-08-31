using AppStrap.Owin.WebAPI;
using AppStrap.Utils;
using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
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
            var baseAddress = String.Format("http://*:{0}/", "5422");
            WebApp.Start<WebAPIStartup>(url: baseAddress);

            CoreService.Start();
            Scheduler scheduler = new Scheduler();
            scheduler.Start(TimeSpan.FromSeconds(10), () => Lookup());

            CoreService.Stop();
            Console.ReadLine();
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
