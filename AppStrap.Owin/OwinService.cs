using AppStrap.Owin.WebAPI;
using AppStrap.Utils;
using Microsoft.Owin.Hosting;
using System;
using System.Linq;

namespace AppStrap.Owin
{
    internal class OwinService
    {
        private IDisposable _webApi = null;
        public void Start()
        {
            

            var baseAddress = String.Format("http://*:{0}/", "3845");
            _webApi = WebApp.Start<WebAPIStartup>(url: baseAddress);

            Console.WriteLine("AppStrap.Owin Start : {0}", baseAddress);

            CoreService.Start();

            Scheduler scheduler = new Scheduler();

            scheduler.Start(TimeSpan.FromSeconds(10), () => Lookup());

        }

        public void Stop()
        {

            CoreService.Stop();
            if (_webApi != null)
            {
                _webApi.Dispose();
            }
        }


        void Lookup()
        {
            Console.Clear();
            Console.WriteLine("--------" + DateTime.Now + "--------");
            AppStrapLog.LogList.ToList().ForEach(t => Console.WriteLine(t));
        }
    }
}
