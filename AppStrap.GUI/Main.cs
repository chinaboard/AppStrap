using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Windows.Forms;

namespace AppStrap.GUI
{
    public partial class Main : Form
    {


        public Main()
        {
            InitializeComponent();

        }

        private void GetListButton_Click(object sender, EventArgs e)
        {
            HttpClient client = new HttpClient();
            var response = client.GetAsync(@"http://localhost:3845/api/AppStrap/GetAppStrapList")
                .Result.Content.ReadAsStringAsync().Result;
            var list = JsonConvert.DeserializeObject<IEnumerable<AppStrap>>(response);


            list.ToList().ForEach(p =>
            {
                var tmp = new ListViewItem();
                tmp.SubItems[0].Text = p.AppName;
                tmp.SubItems.Add(p.Status);
                this.listView.Items.Add(tmp);
            });
        }

        private void GetLogButton_Click(object sender, EventArgs e)
        {
            HttpClient client = new HttpClient();
            var response = client.GetAsync(@"http://localhost:3845/api/AppStrap/GetAppStrapLog")
                .Result.Content.ReadAsStringAsync().Result;
            var list = JsonConvert.DeserializeObject<IEnumerable<string>>(response);

            this.textBoxLog.Lines = list.ToArray();
        }
    }
}
