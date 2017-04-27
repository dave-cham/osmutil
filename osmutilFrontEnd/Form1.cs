using Microsoft.Win32;
using osmutil;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace osmutilFrontEnd
{
    public partial class Form1 : Form
    {
        Service _service;
        List<CheckBox> _sections = new List<CheckBox>();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            go.Enabled = false;
            userName.Text = (string)Registry.GetValue("HKEY_CURRENT_USER\\osmutil\\settings", "username", "");
            password.Text = (string)Registry.GetValue("HKEY_CURRENT_USER\\osmutil\\settings", "password", "");
            apiKey.Text = (string)Registry.GetValue("HKEY_CURRENT_USER\\osmutil\\settings", "apiKey", "");
        }

        private void loggedIn()
        {
            var sections = _service.Sections.OrderBy(_ => _.section).ThenBy(_ => _.sectionname).ToArray();
            var leftHandEdge = 300;
            var ypos = 0;
            for (int i = 0; i < sections.Length; i++, ypos++)
            {
                if (i == 6)
                {
                    leftHandEdge += 170;
                    ypos = 0;
                }
                var section = sections[i];
                var box = new CheckBox();
                box.Tag = section.sectionname;
                box.Text = section.sectionname;
                box.AutoSize = true;
                box.Location = new Point(leftHandEdge, 25+ypos*20);
                box.Checked = section.sectionname != "Leavers";

                this.Controls.Add(box);
                _sections.Add(box);
            }
            login.Enabled = false;
            go.Enabled = true;
        }

        private async void go_Click(object sender, EventArgs e)
        {
            go.Enabled = false;
            results.Text = "";
            working.Visible = true;
            var sectionFilter = _sections.Where(_ => _.Checked).Select(_ => _.Text).ToList();

            IOperation operation = null;
            if(listEmailAddresses.Checked)
            {
                operation = new ReportEmailAddresses(_service, sectionFilter);
            }
            else if (findMissingData.Checked)
            {
                operation = new FindMembersWithMisingData(_service, sectionFilter);
            }
            else if (checkContactCheckboxes.Checked)
            {
                operation = new TickboxTicker(_service, sectionFilter);
            }
            else if (findMovers.Checked)
            {
                operation = new FindMovers(_service, sectionFilter);
            }

            if(operation != null)
            {
                await Task.Factory.StartNew(() =>
                {
                    operation.DoIt((str, newline) =>
                    {
                        var control = results; 
                        control.Invoke(new Action(() => control.AppendText(str + (newline?"\r\n":""))));
                    }, dryRun.Checked);
                });
            }
            working.Visible = false;
            go.Enabled = true;
        }

        private void login_Click(object sender, EventArgs e)
        {
            try
            {
                QueryHelpers.ApiKey = apiKey.Text;
                _service = new Service(userName.Text, password.Text);

                Registry.SetValue("HKEY_CURRENT_USER\\osmutil\\settings", "username", userName.Text);
                Registry.SetValue("HKEY_CURRENT_USER\\osmutil\\settings", "password", password.Text);
                Registry.SetValue("HKEY_CURRENT_USER\\osmutil\\settings", "apiKey", apiKey.Text);
            }
            catch
            {
                return;
            }

            loggedIn();
        }
    }
}
