using AppStrap.Utils;
using System;
using System.Linq;
using System.Reflection;

namespace AppStrap
{
    public class AppStrapProxy : MarshalByRefObject
    {

        private readonly object _padLock = new object();
        private IAppStrapBoot _bootInstance = null;
        private string _appName = null;

        public AppStrapProxy()
        {

        }


        public bool Start(string appName, string appFilePath)
        {
            try
            {
                Preconditions.CheckNotBlankOrWhiteSpace(appName, "appName");

                this._appName = appName;

                AppStrapLog.Info(this._appName, "Start");

                var assembly = Assembly.LoadFile(appFilePath);
                var bootType = assembly.GetTypes().First(i => i.GetInterface(typeof(IAppStrapBoot).FullName) == typeof(IAppStrapBoot));
                this._bootInstance = (IAppStrapBoot)Activator.CreateInstance(bootType);

                if (_bootInstance != null)
                    _bootInstance.Start();

                AppStrapLog.Info(this._appName, "Start succeed");

                return true;
            }
            catch (Exception ex)
            {
                AppStrapLog.Error(this._appName, ex.GetBaseException().Message);
                AppStrapLog.Info(this._appName, "Start failed");
                return false;
            }
        }



        public bool Stop()
        {
            try
            {
                AppStrapLog.Info(this._appName, "Stop");
                if (_bootInstance != null)
                    _bootInstance.Stop();
                AppStrapLog.Info(this._appName, "Stop succeed");
                return true;
            }
            catch (Exception ex)
            {
                AppStrapLog.Error(this._appName, ex.GetBaseException().Message);
                AppStrapLog.Info(this._appName, "Stop failed");
                return false;
            }
        }
    }
}
