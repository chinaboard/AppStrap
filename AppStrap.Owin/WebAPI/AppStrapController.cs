using AppStrap.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace AppStrap.Owin.WebAPI
{
    public class AppStrapController : ApiController
    {
        // GET api/values 
        public IEnumerable<AppStrap> GetAppStrapList()
        {
            return AppStrapList.Instance.List.Values;
        }

        public IEnumerable<string> GetAppStrapLog()
        {
            return AppStrapLog.LogList.ToArray<string>();
        }

        [HttpGet]
        public bool LoadAppStrap(HttpRequestMessage request)
        {
            string appName = request.RequestUri.ParseQueryString().Get("AppName");
            string appFilePath = request.RequestUri.ParseQueryString().Get("AppFilePath");

            Preconditions.CheckNotBlankOrWhiteSpace(appName, "AppName");
            Preconditions.CheckNotBlankOrWhiteSpace(appFilePath, "AppFilePath");

            try
            {
                var app = new AppStrap(appName, appFilePath);
                app.Start();
                return true;
            }
            catch (Exception ex)
            {
                AppStrapLog.Error(appName, ex.Message);
                return false;
            }
        }
    }
}
