using AppStrap.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace AppStrap.Owin.WebAPI
{
    public class AppStrapController : ApiController
    {
        public JsonResult<List<AppStrap>> GetAppStrapList()
        {
            return Json(AppStrapList.Instance.List.Values.ToList());
        }

        public IEnumerable<string> GetAppStrapLog()
        {
            return AppStrapLog.LogList;
        }

        [HttpGet]
        public bool LoadAppStrap(string appName, string appFilePath)
        {
            try
            {
                Preconditions.CheckNotBlankOrWhiteSpace(appName, "AppName");
                Preconditions.CheckNotBlankOrWhiteSpace(appFilePath, "AppFilePath");
                var app = new AppStrap(appName, appFilePath);
                app.Start();
                return true;
            }
            catch (Exception ex)
            {
                AppStrapLog.Error("AppStrap", ex.Message);
                return false;
            }
        }
    }
}
