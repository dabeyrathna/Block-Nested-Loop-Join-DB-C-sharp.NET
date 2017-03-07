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

namespace Project1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            cmbCurrServer.SelectedIndex = 0;
        }

        public string[] files = {"branch_NYC", "branch_NYC", "account_NYC","account_SFO","account_OMA","branch_SFO","customer_HOU","depositor_OMA" };
        int branchBlockSize = 7;
        int accountBlockSize = 10;
        int customerBlockSize = 8;
        int depositorBlockSize = 15;

        public string[] outBuffer = new string[5]; //


        public void getStatistics(string file) {
            string[] relation = file.Split('_');
            if (relation[0].Equals("branch")) {

            }
        }

        public void joinOrder(string[] arr) {

            foreach (string i in arr) {
                getStatistics(i);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {


                string[] arr = txtxFrom.Text.ToString().Split(',');
                bool fileFlag = true;

                foreach (string i in arr) {
                    if (!files.Contains(i)) {
            
                        MessageBox.Show("Query error. No table found\n\nPlease try again  "+i);
                        txtSelect.Text = "";
                        fileFlag = false;
                    }
                }

                if (fileFlag) {

                    MessageBox.Show("query ok........");

                    foreach (string a in arr)
                    {
                        lstInner.Items.Add(a);
                        // string line = File.ReadLines(files[1]).Skip(1).Take(3).First();
                        //txtQuery.Text = line.ToString();
                    }

                    joinOrder(arr);

                }

              
            }
            catch (Exception err) {
                txtSelect.Text = err.ToString();
            }
        }
    }
}
