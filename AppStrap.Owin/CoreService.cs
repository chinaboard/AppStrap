using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppStrap.Owin
{
    public class CoreService
    {
        static  AppStrap sb;

        public CoreService()
        {

        }

        public static void Start()
        {
            //sb = new AppStrap("AppStrap.Demo", AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\AppStrap.Demo\bin\Debug\AppStrap.Demo.exe");
           // sb.Start();
        }
        public static void Stop()
        {
            //sb.Stop();
        }

    }
}
