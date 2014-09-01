using AppStrap.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppStrap
{
    [Serializable]
    public class AppStrap
    {
        [NonSerialized]
        private AppStrapProxy _proxy = null;
        [NonSerialized]
        private AppDomain _appDomain = null;
        public string AppName { get; private set; }
        public string AppFilePath { get; private set; }
        public string ConfigFile { get; private set; }
        public string Status { get; private set; }
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
