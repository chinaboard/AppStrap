using System;
using System.IO;
using Topshelf;
using Topshelf.ServiceConfigurators;

namespace AppStrap.Owin
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                x.Service<OwinService>((ServiceConfigurator<OwinService> s) =>
                {
                    s.ConstructUsing(() => new OwinService());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });
                x.StartAutomatically();
                x.RunAsLocalSystem();
                x.SetDescription("一个特别牛X的AppDomain Host");
                x.SetDisplayName("AppStrap.OwinService");
                x.SetServiceName("AppStrap.OwinService");
            });
        }


        static void ClearLog()
        {
            Console.WriteLine("Clear Log?");
            if (Console.ReadKey(false).Key == ConsoleKey.Y)
            {
                File.Delete(Utils.AppStrapLog.LogFilePath);
                Console.WriteLine("\r\nThe log has been cleared.");
            }
            Console.Clear();
        }
    }
}
