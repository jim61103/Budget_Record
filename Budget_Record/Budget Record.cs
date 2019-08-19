using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Budget_Record
{
    public partial class Budget_Record : Form
    {
        public Dictionary<string, string> dicMonthDate = new Dictionary<string, string>();
        public List<Record> lsRecord = new List<Record>();
        public string path = System.Environment.CurrentDirectory;
        public static string Record = "Record.txt";
        public static string Config = "Config.txt";

        public Budget_Record()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
                 
            string FilePath = path + "\\"+Config;
            string res = getConfig(FilePath);
            if (res != "")
            {
                MessageBox.Show(res);
            }
            else
            {
                FilePath = path + "\\"+Record;
                getRecord(FilePath);
            }            
        }

        private void getRecord(string filePath)
        {
            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
            }

            StreamReader sr = new StreamReader(filePath);
            while (!sr.EndOfStream)
            {
                string msg = sr.ReadLine();
                string[] sRecord = msg.Split(',');
                Record rd = new Record();
                rd.year = sRecord[0];
                rd.month = sRecord[1];
                rd.date = sRecord[2];
                rd.budget = sRecord[3];
                rd.cost = sRecord[4];
                rd.dif = sRecord[5];
                lsRecord.Add(rd);
            }
            sr.Dispose();
        }

        private string getConfig(string filePath)
        {
            if (File.Exists(filePath))
            {            
                StreamReader sr = new StreamReader(filePath);
                bool year = false;
                bool date = false;
                while (!sr.EndOfStream)
                {
                    string msg = sr.ReadLine();
                    if (msg.Contains("YEAR"))
                    {
                        year = true;
                        date = false;
                        continue;
                    }
                    else if (msg.Contains("MONTH_DATE"))
                    {
                        year = false;
                        date = true;
                        continue;
                    }

                    if (year)
                    {
                        cboYear.Items.Add(msg);
                    }
                    else if (date)
                    {
                        string _Month = msg.Split('=')[0];
                        string Date = msg.Split('=')[1];
                        cboMonth.Items.Add(_Month);
                        dicMonthDate.Add(_Month, Date);
                    }
                }
                sr.Dispose();
                return "";
            }
            else
            {
                return "請確認Config是否存在";
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void cboMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            cboDate.Items.Clear();
            string sDate = dicMonthDate[cboMonth.SelectedItem.ToString()];
            string[] aDate = sDate.Split(',');

            for (int i = 0; i < aDate.Length; i++)
            {
                cboDate.Items.Add(aDate[i]);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (StreamWriter sw = new StreamWriter(path+"\\"+Record)) {
                int dif = Int32.Parse(txtBudget.Text.Trim()) - Int32.Parse(txtCost.Text.Trim());
                string msg = cboYear.SelectedItem.ToString() + "," + cboMonth.SelectedItem.ToString() + "," + cboDate.SelectedItem.ToString() + "," + txtBudget.Text.Trim() + "," + txtCost.Text.Trim() + "," + dif;
                Record rd = new Record { year = cboYear.SelectedItem.ToString(), month = cboMonth.SelectedItem.ToString(),date = cboDate.SelectedItem.ToString(),budget = txtBudget.Text,cost = txtCost.Text,dif = dif.ToString() };
                lsRecord.Add(rd);
                sw.WriteLine(msg);
            }
        }
    }
}
