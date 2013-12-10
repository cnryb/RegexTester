using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace RegexTester
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }


        public void Check(string strReg)
        {
            if (string.IsNullOrEmpty(strReg) || string.IsNullOrEmpty(rtb.Text)) return;

            // ChangeRichTextBox(rtb, 0, rtb.Text.Length, Color.White, Color.Black);

            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("DisplayMember");
                dt.Columns.Add("ValueMember");
                var reg = new Regex(strReg);
                int startindex = 0;
                foreach (Match item in reg.Matches(rtb.Text))
                {
                    var res = item.Value;
                    int startLocation = rtb.Text.IndexOf(res, startindex);
                    startindex = startLocation + res.Length;
                    DataRow dr = dt.NewRow();
                    dr["DisplayMember"] = res;
                    dr["ValueMember"] = startLocation + "#" + res.Length;
                    dt.Rows.Add(dr);

                    ChangeRichTextBox(rtb, startLocation, res.Length, Color.Yellow, Color.Red);
                }

                lb.DataSource = dt;
                lb.DisplayMember = "DisplayMember";
                lb.ValueMember = "ValueMember";
            }
            catch (Exception ex)
            {
                Text = "正则表达式异常 " + ex;
            }

        }

        private void ChangeRichTextBox(RichTextBox rt, int start, int length, Color bgc, Color c)
        {
            int currentNum = rtb.SelectionStart;
            rt.Select(start, length);

            rtb.SelectionStart = currentNum;
            rt.SelectionColor = c;
            rt.SelectionBackColor = bgc;
        }
        private void txtReg_TextChanged(object sender, EventArgs e)
        {
            Text = "";
            Check(txtReg.Text);
        }

        private void lb_DoubleClick(object sender, EventArgs e)
        {
            DataTable dt = lb.DataSource as DataTable;
            if (dt != null)
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string[] strs = dt.Rows[i]["ValueMember"].ToString().Split('#');
                    if (strs.Length != 2) continue;
                    int start, length;
                    int.TryParse(strs[0], out start);
                    int.TryParse(strs[1], out length);
                    rtb.SelectionStart = start;
                    rtb.Select(start, length);
                    rtb.SelectionBackColor = Color.Yellow;
                }
            {
                if (lb.SelectedValue == null) return;
                string[] strs = lb.SelectedValue.ToString().Split('#');
                if (strs.Length != 2) return;
                int start, length;
                int.TryParse(strs[0], out start);
                int.TryParse(strs[1], out length);
                rtb.SelectionStart = start;
                rtb.Select(start, length);
                rtb.SelectionBackColor = Color.Blue;
                rtb.ScrollToCaret();
            }
        }

        private void rtb_Leave(object sender, EventArgs e)
        {
            //rtb.ScrollToCaret();
            //int currentColumnNumber = rtb.SelectionStart - rtb.GetFirstCharIndexOfCurrentLine()+1;//光标所在本行的位置
            //int lineNum = 1 + rtb.GetLineFromCharIndex(rtb.SelectionStart);//光标所在的行
            // int currentNum = rtb.SelectionStart;
            Check(txtReg.Text);
            //rtb.SelectionStart = currentNum;
            //rtb.SelectionLength = 0;
        }
    }
}
