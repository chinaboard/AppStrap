using AppStrap.Utils;
using System;

namespace AppStrap
{
    public class AppStrap
    {
        private AppStrapProxy _proxy = null;
        private AppDomain _appDomain = null;
        public string AppName { get; set; }
        public string AppFilePath { get; set; }
        public string ConfigFile { get; set; }
        public string Status { get; set; }
        public AppStrap() { }
        public AppStrap(string appName, string appFilePath, string configFile = null)
        {
            Preconditions.CheckNotBlankOrWhiteSpace(appName, "appName");
            Preconditions.CheckNotBlankOrWhiteSpace(appFilePath, "appFilePath");

            AppName = appName;
            AppFilePath = appFilePath;
            ConfigFile = configFile;

            this._appDomain = new AppStrapDomain(appName, appFilePath, configFile).GetAppDomain(); ;

            this._proxy = (AppStrapProxy)this._appDomain.CreateInstanceAndUnwrap(typeof(AppStrapProxy).Assembly.FullName, typeof(AppStrapProxy).FullName);
            this.Status = "Loaded";

            AppStrapList.Instance.GetOrAdd(AppName, this);
        }

        public void Start()
        {
            if (this._proxy != null && this._proxy.Start(this.AppName, this.AppFilePath))
                this.Status = "Running";
        }

        public void Stop()
        {
            if (this._proxy != null && this._proxy.Stop())
                this.Status = "Loaded";
        }

    }
}
