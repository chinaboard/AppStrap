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

            _appDomain = new AppStrapDomain(appName, appFilePath, configFile).GetAppDomain();

            _proxy = (AppStrapProxy)_appDomain.CreateInstanceAndUnwrap(typeof(AppStrapProxy).Assembly.FullName, typeof(AppStrapProxy).FullName);
            Status = "Loaded";

            AppStrapList.Instance.GetOrAdd(AppName, this);
        }

        public void Start()
        {
            if (_proxy != null && _proxy.Start(AppName, AppFilePath))
                Status = "Running";
        }

        public void Stop()
        {
            if (_proxy != null && _proxy.Stop())
                Status = "Loaded";
        }

    }
}
