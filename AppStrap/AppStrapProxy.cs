using AppStrap.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AppStrap
{
    public class AppStrapProxy : MarshalByRefObject
    {

        private readonly object _padLock = new object();
        private IAppStrapBoot _bootInstance = null;


        public AppStrapProxy()
        {

        }


        public void Start(string appName, string appFilePath)
        {
            var assembly = Assembly.LoadFile(appFilePath);
            var bootType = assembly.GetTypes().First(i => i.GetInterface(typeof(IAppStrapBoot).FullName) == typeof(IAppStrapBoot));
            this._bootInstance = (IAppStrapBoot)Activator.CreateInstance(bootType);
            try
            {
                AppStrapLog.Info(appName, "Start");
                if (_bootInstance != null)
                    _bootInstance.Start();
                AppStrapLog.Info(appName, "Start succeed");
            }
            catch (Exception ex)
            {
                AppStrapLog.Error(appName, ex.GetBaseException().Message);
                AppStrapLog.Info(appName, "Start failed");
            }
        }




        public void Stop()
        {
            if (_bootInstance != null)
                _bootInstance.Stop();
        }
    }
}
