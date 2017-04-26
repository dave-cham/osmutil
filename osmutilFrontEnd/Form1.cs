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
        }

        private void loggedIn()
        { 
            var leftHandEdge = 300;
            var ypos = 0;
            for (int i = 0; i < _service.Sections.Length; i++, ypos++)
            {
                if (i == 6)
                {
                    leftHandEdge += 170;
                    ypos = 0;
                }
                var section = _service.Sections[i];
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

        private void go_Click(object sender, EventArgs e)
        {
            results.Text = "";

            // TODO: Put this on a background thread. Try to get the text from the service as it's produced....
            var sectionFilter = _sections.Where(_ => _.Checked).Select(_ => _.Text).ToList();
            if(listEmailAddresses.Checked)
            {
                var query = new ReportEmailAddresses(_service, sectionFilter);
                results.Text = query.DoIt();
            }
            else if (findMissingData.Checked)
            {
                var query = new FindMembersWithMisingData(_service, sectionFilter);
                results.Text = query.DoIt();
            }
            else if (checkContactCheckboxes.Checked)
            {
                var query = new TickboxTicker(_service, sectionFilter);
                results.Text = query.DoIt();
            }
            else if (findMovers.Checked)
            {
                var query = new FindMovers(_service, sectionFilter);
                results.Text = query.DoIt();
            }
        }

        private void login_Click(object sender, EventArgs e)
        {
            try
            {
                // TODO: Get rid of this hardcoded stuff, but try to make it dialog remember the previous entries.
                // TODO: How to handle dry run? Take it out of the service constructor.
                // TODO: Enable and disable the dry run checkbox depending on whether the selected operation is a write operation.

            }
            catch
            {
                return;
            }

            loggedIn();
        }
    }
}
