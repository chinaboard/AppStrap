using AppStrap.Utils;
using System;
using System.IO;
using System.Security.Policy;

namespace AppStrap
{
    public class AppStrapDomain
    {
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
            if (_appDomain != null)
            {
                AppStrapLog.Info(AppName, $"AppDomain:{AppName} already exists");
                return _appDomain;
            }

            AppStrapLog.Info(AppName, "Create AppDomain");
            if (info == null)
                info = new AppDomainSetup();

            info.ApplicationBase = Path.GetDirectoryName(AppFilePath);

            if (!string.IsNullOrWhiteSpace(ConfigFile))
            {
                info.ConfigurationFile = ConfigFile;
                AppStrapLog.Info(AppName, "Load Config File");
            }

            _appDomain = AppDomain.CreateDomain(AppName, securityInfo, info);
            _appDomain.DoCallBack(() =>
            {
                AppDomain.CurrentDomain.UnhandledException += (object sender, UnhandledExceptionEventArgs e) =>
                {
                    AppStrapLog.Error(AppName, "UnhandledException");
                    AppDomain.Unload(AppDomain.CurrentDomain);
                };
            });

            AppStrapLog.Info(AppName, "Create AppDomain succeed");
            return _appDomain;
        }
        public void Unload()
        {
            AppDomain.Unload(_appDomain);
        }
    }
}
