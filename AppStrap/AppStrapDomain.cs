using AppStrap.Utils;
using System;
using System.IO;
using System.Security.Policy;

namespace AppStrap
{
    [Serializable]
    public class AppStrapDomain
    {
        [NonSerialized]
        private AppDomain _appDomain = null;
        public string AppName { get; private set; }
        public string AppFilePath { get; private set; }
        public string ConfigFile { get; private set; }
        public AppStrapDomain(string appName, string appFilePath, string configFile = null)
        {
            Preconditions.CheckNotBlankOrWhiteSpace(appName, "appName");
            Preconditions.CheckNotBlankOrWhiteSpace(appFilePath, "appFilePath");

            AppName = appName;
            AppFilePath = appFilePath;
            ConfigFile = configFile;
        }
        public AppDomain GetAppDomain(Evidence securityInfo = null, AppDomainSetup info = null)
        {
            AppStrapLog.Info(this.AppName, "Create AppDomain");
            if (info == null)
                info = new AppDomainSetup();

            info.ApplicationBase = Path.GetDirectoryName(this.AppFilePath);
            
            if (!string.IsNullOrWhiteSpace(this.ConfigFile))
            {
                info.ConfigurationFile = this.ConfigFile;
                AppStrapLog.Info(this.AppName, "Load Config File");
            }

            this._appDomain = AppDomain.CreateDomain(this.AppName, securityInfo, info);
            this._appDomain.DoCallBack(() =>
            {
                AppDomain.CurrentDomain.UnhandledException += (object sender, UnhandledExceptionEventArgs e) =>
                {
                    AppStrapLog.Error(this.AppName, "UnhandledException");
                    AppDomain.Unload(AppDomain.CurrentDomain);
                };
            });

            AppStrapLog.Info(this.AppName, "Create AppDomain succeed");
            return this._appDomain;
        }
    }
}
