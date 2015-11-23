using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
        HttpClient client = new HttpClient();

        public Main()
        {
            InitializeComponent();

        }

        private async void GetListButton_Click(object sender, EventArgs e)
        {
            var response = await client.GetAsync(@"http://localhost:3845/api/AppStrap/GetAppStrapList");
            var content = await response.Content.ReadAsStringAsync();
            var list = JsonConvert.DeserializeObject<List<AppStrap>>(content);


            list.ForEach(p =>
            {
                var tmp = new ListViewItem();
                tmp.SubItems[0].Text = p.AppName;
                tmp.SubItems.Add(p.Status);
                listView.Items.Add(tmp);
            });
        }

        private async void GetLogButton_Click(object sender, EventArgs e)
        {
            var response = await client.GetAsync(@"http://localhost:3845/api/AppStrap/GetAppStrapLog");
            var content = await response.Content.ReadAsStringAsync();
            var list = JsonConvert.DeserializeObject<IEnumerable<string>>(content);

            textBoxLog.Lines = list.ToArray();
        }

        private void AddAPPButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Application(*.exe)|*.exe";
            if (ofd.ShowDialog(this) == DialogResult.OK)
            {
                string name = Path.GetFileNameWithoutExtension(ofd.FileName);
                string getUrl = string.Format("http://localhost:3845/api/AppStrap/LoadAppStrap/?AppName={0}&AppFilePath={1}", name, ofd.FileName);
                MessageBox.Show(client.GetAsync(getUrl).Result.Content.ReadAsStringAsync().Result);
            }
        }
    }
}
