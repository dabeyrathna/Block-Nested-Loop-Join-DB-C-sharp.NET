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
using System.Text.RegularExpressions;

namespace Project1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            cmbCurrServer.SelectedIndex = 0;
            rdo1.Checked = true;
            lstSite2.HorizontalScrollbar = true;
            txtSelect.Text = "customer_name, street, city";
            txtxFrom.Text = "customer, depositor";
            txtWhere.Text = "customer.customer_name = depositor.customer_name";
        }

        List<Account> accountTable;
        List<Branch> branchTable;
        List<Depositor> depositorTable;
        List<Customer> customerTable;

        List<RelationInfo> tableSet = new List<RelationInfo>();

        public string[] tables = {"branch", "account","depositor", "customer"};
        List<string> twoWaySite1 = new List<string>();
        List<string> twoWaySite2 = new List<string>();

        public List<string> outBuffer = new List<string>();
        
        public void readTable(string tabel, string site)
        {
            
            if (tabel.ToLower().Equals("customer"))
            {
                try
                {
                    //MessageBox.Show("customer_" + site.ToUpper() + ".txt");
                    using (StreamReader sr = new StreamReader("customer_HOU.txt"))
                    {
                        string line;
                        customerTable = new List<Customer>();
                        while ((line = sr.ReadLine()) != null)
                        {
                            string[] s = line.Split(',');
                            customerTable.Add(new Customer(s[0].ToString(), s[1].ToString(), s[2].ToString())); 
                        }
                    }                    

                }
                catch (Exception eee)
                {
                    MessageBox.Show("File not found customer  \n\n"+eee.ToString());
                }
            }
            if (tabel.ToLower().Equals("branch"))
            {
                string fileName = "";
                try
                {
                    fileName = "branch_" + site.ToUpper() + ".txt";

                    //MessageBox.Show(fileName);
                    using (StreamReader sr = new StreamReader(fileName))
                    {
                        string line;
                        branchTable = new List<Branch>();
                        while ((line = sr.ReadLine()) != null)
                        {
                            string[] s = line.Split(',');
                            branchTable.Add(new Branch(s[0].ToString(), s[1].ToString(), s[2].ToString()));
                        }
                    }
                }
                catch (Exception eee)
                {
                    MessageBox.Show("File not found branch  \n\n" + eee.ToString());
                }
            }
            if (tabel.ToLower().Equals("account"))
            {
                try
                {
                    using (StreamReader sr = new StreamReader("account_" + site.ToUpper() + ".txt"))
                    {
                        string line;
                        accountTable = new List<Account>();
                        while ((line = sr.ReadLine()) != null)
                        {
                            string[] s = line.Split(',');
                            accountTable.Add(new Account(s[0].ToString(), s[1].ToString(), s[2].ToString()));
                        }
                    }

                }
                catch (Exception eee)
                {
                    MessageBox.Show("File not found account  \n\n" + eee.ToString());
                }
            }
            if (tabel.ToLower().Equals("depositor"))
            {
                try
                {
                    MessageBox.Show("depositor_" + site.ToUpper() + ".txt");
                    using (StreamReader sr = new StreamReader("depositor_" + site.ToUpper() + ".txt"))
                    {
                        string line;
                        depositorTable = new List<Depositor>();
                        while ((line = sr.ReadLine()) != null)
                        {
                            string[] s = line.Split(',');
                            depositorTable.Add(new Depositor(s[0].ToString(), s[1].ToString()));
                        }
                    }

                }
                catch (Exception eee)
                {
                    MessageBox.Show("File not found depositor  \n\n" + eee.ToString());
                }
            }

            tableSet.Add(new RelationInfo(tabel, site.ToUpper()));

/*
            if (tabel.Equals("customer")) {
                foreach (Customer c in customerTable) {
                    lstInner.Items.Add(c.getCustName());
                }
            }
            else if (tabel.Equals("account")) {
                foreach (Account c in accountTable)
                {
                    lstOuter.Items.Add(c.getAccountName());
                }
            }
            else if (tabel.Equals("branch"))
            {
                foreach (Branch c in branchTable)
                {
                    lstOuter.Items.Add(c.getBranchName());
                }
            }
            else if (tabel.Equals("depositor"))
            {
                foreach (Depositor c in depositorTable)
                {
                    lstOuter.Items.Add(c.getKey1()+","+c.getKey2());
                }
            }

    */
        }

        public List<string> loadTables(string[] arr, List<string> conditions)
        {

            List<string> fileArray = new List<string>();

            bool cust = true;
            bool branch = true;
            bool account = true;
            bool deposite = true;

            

            foreach (string i in conditions)
            {
                if (i.Contains("customer") && cust)
                {
                    cust = false;
                    //txtLog.Text = txtLog.Text + Environment.NewLine+"Customer table in HOU site";
                    fileArray.Add("customer_hou");
                }
                if (i.Contains("OMA") && i.Contains("depositor") && deposite)
                {
                    deposite = false;
                    //txtLog.Text = txtLog.Text + Environment.NewLine+"Depositor table in Omaha site";
                    fileArray.Add("depositor_oma");
                }
                else if (!i.Contains("OMA") && i.Contains("depositor") && deposite)
                {
                    deposite = false;
                    //txtLog.Text = txtLog.Text + Environment.NewLine+"Depositor table in NYC";
                    fileArray.Add("depositor_nyc");
                }
                if (cmbCurrServer.SelectedIndex == 0 && i.Contains("branch") && branch)
                {
                    branch = false;
                    //txtLog.Text = txtLog.Text + Environment.NewLine + "Branch table in NYC site";
                    fileArray.Add("branch_nyc");
                }
                else if (cmbCurrServer.SelectedIndex == 3 && i.Contains("branch") && branch)
                {
                    branch = false;
                    //txtLog.Text = txtLog.Text + Environment.NewLine+"Branch table in SFO site";
                    fileArray.Add("branch_sfo");
                }
                if (cmbCurrServer.SelectedIndex == 3 && i.Contains("account") && account && !getQueryRadiaButtonName().Equals("Query 3"))
                {
                    account = false;
                    //txtLog.Text = txtLog.Text + Environment.NewLine+"Account table in SFO site";
                    fileArray.Add("account_sfo");
                }
                if (cmbCurrServer.SelectedIndex == 3 && i.Contains("account") && account && getQueryRadiaButtonName().Equals("Query 3"))
                {
                    account = false;
                    //txtLog.Text = txtLog.Text + Environment.NewLine+"Account table in SFO site";
                    fileArray.Add("account_nyc");
                }
                else if (i.Contains("OMA") && i.Contains("account") && account)
                {
                    account = false;
                    //txtLog.Text = txtLog.Text + Environment.NewLine+"Account table in OMA site";
                    fileArray.Add("account_oma");
                }
                else if (i.Contains("account") && account)
                {
                    account = false;
                    //txtLog.Text = txtLog.Text + Environment.NewLine+ "Account table in NYC site";
                    fileArray.Add("account_nyc");
                }
            }

            return fileArray;
        }
        
        public void twoWaySemijoin(RelationInfo table1, RelationInfo table2, string currentSite) {
            txtLog.Text = txtLog.Text + Environment.NewLine;
            txtLog.Text = txtLog.Text + "Ship table 1 key to merge, get table 2 key to min of not or ....... and return to"+currentSite;

            if (table1.table.Equals("depositor") && table2.table.Equals("account"))
            {
                txtLog.Text = txtLog.Text + Environment.NewLine;
                txtLog.Text = txtLog.Text + "----------------------------------" + currentSite;

                List<Depositor> t1 = new List<Depositor>(depositorTable);
                List<Account> t2 = new List<Account>(accountTable);
                
                string FileName = "depositor_NYC_HOU_Copy.txt";

                try
                {
                    System.IO.File.Delete(FileName);
                }
                catch (System.IO.IOException eee)
                { }

                blockNestedJoinSameSite(FileName);

                txtLog.Text = txtLog.Text + Environment.NewLine;
                txtLog.Text = txtLog.Text + "First join session finished" + currentSite;
                txtLog.Text = txtLog.Text + Environment.NewLine;
                txtLog.Text = txtLog.Text + "Results write to file " + Environment.NewLine;
                txtLog.Text = txtLog.Text + FileName;

                using (System.IO.StreamWriter file = new System.IO.StreamWriter(FileName, true))
                {
                    foreach (string i in twoWaySite1)
                    {
                        file.WriteLine(i);
                    }
                }

                using (StreamReader sr = new StreamReader(FileName))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        lstSite2.Items.Add(line);
                    }
                }

                blockNestedJoin("customer_HOU.txt",FileName);

                txtLog.Text = txtLog.Text + Environment.NewLine;
                txtLog.Text = txtLog.Text + Environment.NewLine;
                txtLog.Text = txtLog.Text + "File transferd to the site :"+cmbCurrServer.Text;
            }
        }

        public void blockNestedJoinSameSite(string fileName)
        {

            //MessageBox.Show("reciewd :  " + table1 + " and " + table2);
            List<string> tempTable1 = new List<string>();
            List<string> tempTable2 = new List<string>();

            List<string> inner = new List<string>();
            List<string> outer = new List<string>();
            int mainMemorySize = 5;

            string line;

            try
            {

                foreach (Depositor d in depositorTable) {
                    tempTable1.Add(d.getAccNo()+","+d.getCustName());
                }

                foreach (Account a in accountTable) {
                    tempTable2.Add(a.getAccountNumber() + "," + a.getBranchName()+","+a.getBalance());
                }


                int innerBlockSize = blockSize("depositor");
                int outerBlockSize = blockSize("account");

                int blocksInTable1 = (int)Math.Ceiling((double)tempTable1.Count() / innerBlockSize);
                int blocksInTable2 = (int)Math.Ceiling((double)tempTable2.Count() / outerBlockSize);

                if (blocksInTable1 < mainMemorySize)
                {
                    inner = tempTable1;
                    outer = tempTable2;
                }
                else if (blocksInTable2 < mainMemorySize)
                {
                    inner = tempTable2;
                    outer = tempTable1;
                    int temp = innerBlockSize;
                    innerBlockSize = outerBlockSize;
                    outerBlockSize = temp;
                }
                else
                {
                    if (blocksInTable1 < blocksInTable2)
                    {
                        inner = tempTable2;
                        outer = tempTable1;
                        int temp = innerBlockSize;
                        innerBlockSize = outerBlockSize;
                        outerBlockSize = temp;
                    }
                    else
                    {
                        inner = tempTable1;
                        outer = tempTable2;
                    }
                }


                using (System.IO.StreamWriter file = new System.IO.StreamWriter("outer.txt", true))
                {
                    foreach (string i in outer)
                    {
                        file.WriteLine(i);
                    }
                }

                using (System.IO.StreamWriter file = new System.IO.StreamWriter("inner.txt", true))
                {
                    foreach (string i in inner)
                    {
                        file.WriteLine(i);
                    }
                }

                List<string> tempBlock1 = new List<string>();
                List<string> tempBlock2 = new List<string>();

                DataTable table = new DataTable();
                table.Columns.Add("Account_number", typeof(string));
                table.Columns.Add("Customer_name", typeof(string));

                string fromSite1 = "";

                for (int i = 0; i < outer.Count(); i = i + (outerBlockSize * (mainMemorySize - 1)))
                {
                    tempBlock1.Clear();
                    lstOuter.Items.Add("-------Block " + "= " + i + " to " + (i + outerBlockSize * (mainMemorySize - 1)) + "------");
                    foreach (var ss1 in outer.Skip(i).Take(outerBlockSize * (mainMemorySize - 1)))
                    {
                        lstOuter.Items.Add("" + ss1);
                        tempBlock1.Add("" + ss1);
                    }
                    lstOuter.Items.Add("___________________________");

                    for (int j = 0; j < inner.Count(); j = j + innerBlockSize)
                    {
                        tempBlock2.Clear();
                        lstInner.Items.Add("-------Block " + "= " + j + " to " + (j + innerBlockSize) + "------");
                        foreach (var ss2 in inner.Skip(j).Take(innerBlockSize))
                        {
                            lstInner.Items.Add("" + ss2);
                            tempBlock2.Add("" + ss2);
                        }
                        lstInner.Items.Add("___________________________");

                        HashSet<string> duplMap = new HashSet<string>();
                        foreach (string ii in tempBlock1)
                        {
                            foreach (string jj in tempBlock2)
                            {
                                if (ii.Split(',')[0].Equals(jj.Split(',')[0]))
                                {
                                    if (!duplMap.Contains(ii.Split(',')[0]))
                                    {
                                        duplMap.Add(ii.Split(',')[0]);
                                        lstOutBuffer.Items.Add(jj);
                                        outBuffer.Add(jj);
                                    }
                                    if (lstOutBuffer.Items.Count > 4)
                                    {
                                        MessageBox.Show("Out Buffer is Full : Flushing...");
                                        for (int kk = 0; kk < 5; kk++)
                                        {
                                            string[] row2 = new string[] { outBuffer[kk].Split(',')[0], outBuffer[kk].Split(',')[1] };
                                            table.Rows.Add(row2);
                                            gridOut.DataSource = table;
                                        }
                                        outBuffer.Clear();
                                        lstOutBuffer.Items.Clear();
                                    }
                                    if (ii.Split(',')[0].Equals("A85486371")) {
                                        lstSite1.Items.Add(jj.Split(',')[1]);
                                        twoWaySite1.Add(jj.Split(',')[1]);
                                    }
                                }
                            }
                        }
                    }
                    if (outBuffer.Count > 0)
                    {
                        foreach (string restBuff in outBuffer)
                        {
                            string[] row3 = new string[] { restBuff.Split(',')[0], restBuff.Split(',')[1] };
                            table.Rows.Add(row3);
                            gridOut.DataSource = table;
                            lstOutBuffer.Items.Clear();
                        }
                    }
                }
            }
            catch (Exception eee)
            {
                MessageBox.Show("File not found \n\n" + eee.ToString());
            }
            finally
            {
                outBuffer.Clear();
                //writeFinalResultsToFile(getQueryRadiaButtonName());
            }
        }

        public int blockSize(string t1) {
            int blkSize = 1;


            if (t1.Equals("customer"))
            {
                blkSize = Customer.numberOfBlock;
            }
            else if (t1.Equals("depositor"))
            {
                blkSize = Depositor.numberOfBlock;
            }
            else if (t1.Equals("branch"))
            {
                blkSize = Branch.numberOfBlock;
            }
            else if (t1.Equals("account"))
            {
                blkSize = Account.numberOfBlock;
            }

            return blkSize;
        }

        public void blockNestedJoin(string table1, string table2) {

            //MessageBox.Show("reciewd :  " + table1 + " and " + table2);

            
            List<string> tempTable1 = new List<string>();
            List<string> tempTable2 = new List<string>();

            List<string> inner = new List<string>();
            List<string> outer = new List<string>();
            int mainMemorySize = 5;

            string line;

            try
            {
                using (StreamReader sr = new StreamReader(table1))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        tempTable1.Add(line);
                    }
                }

                using (StreamReader sr = new StreamReader(table2))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        tempTable2.Add(line);
                    }
                }

                string[] t1 = table1.Split('_');
                string[] t2 = table2.Split('_');

                int innerBlockSize = blockSize(t1[0]);
                int outerBlockSize = blockSize(t2[0]);

                int blocksInTable1 = (int)Math.Ceiling((double)tempTable1.Count() / innerBlockSize);
                int blocksInTable2 = (int)Math.Ceiling((double)tempTable2.Count() / outerBlockSize);

                if (blocksInTable1 < mainMemorySize)
                {
                    inner = tempTable1;
                    outer = tempTable2;
                }
                else if (blocksInTable2 < mainMemorySize)
                {
                    inner = tempTable2;
                    outer = tempTable1;
                    int temp = innerBlockSize;
                    innerBlockSize = outerBlockSize;
                    outerBlockSize = temp;
                }
                else
                {
                    if (blocksInTable1 < blocksInTable2)
                    {
                        inner = tempTable2;
                        outer = tempTable1;
                        int temp = innerBlockSize;
                        innerBlockSize = outerBlockSize;
                        outerBlockSize = temp;
                    }
                    else
                    {
                        inner = tempTable1;
                        outer = tempTable2;
                    }
                }


                using (System.IO.StreamWriter file = new System.IO.StreamWriter("outer.txt", true))
                {
                    foreach (string i in outer)
                    {
                        file.WriteLine(i);
                    }
                }

                using (System.IO.StreamWriter file = new System.IO.StreamWriter("inner.txt", true))
                {
                    foreach (string i in inner)
                    {
                        file.WriteLine(i);
                    }
                }

                List<string> tempBlock1 = new List<string>();
                List<string> tempBlock2 = new List<string>();

                DataTable table = new DataTable();
                table.Columns.Add("Customer_name", typeof(string));
                table.Columns.Add("Street", typeof(string));
                table.Columns.Add("City", typeof(string));

                for (int i = 0; i < outer.Count(); i = i + (outerBlockSize * (mainMemorySize - 1)))
                {
                    tempBlock1.Clear();
                    lstOuter.Items.Add("-------Block " + "= " + i + " to " + (i + outerBlockSize * (mainMemorySize - 1)) + "------");
                    foreach (var ss1 in outer.Skip(i).Take(outerBlockSize * (mainMemorySize - 1)))
                    {
                        lstOuter.Items.Add("" + ss1);
                        tempBlock1.Add("" + ss1);
                    }
                    lstOuter.Items.Add("___________________________");

                    for (int j = 0; j < inner.Count(); j = j + innerBlockSize)
                    {
                        tempBlock2.Clear();
                        lstInner.Items.Add("-------Block " + "= " + j + " to " + (j + innerBlockSize) + "------");
                        foreach (var ss2 in inner.Skip(j).Take(innerBlockSize))
                        {
                            lstInner.Items.Add("" + ss2);
                            tempBlock2.Add("" + ss2);
                        }
                        lstInner.Items.Add("___________________________");

                        HashSet<string> duplMap = new HashSet<string>();
                        foreach (string ii in tempBlock1)
                        {
                            foreach (string jj in tempBlock2)
                            {
                                if (ii.Split(',')[0].Equals(jj.Split(',')[0]))
                                {
                                    if (!duplMap.Contains(ii.Split(',')[0]))
                                    {
                                        duplMap.Add(ii.Split(',')[0]);
                                        lstOutBuffer.Items.Add(ii);
                                        outBuffer.Add(ii);
                                    }
                                    if (lstOutBuffer.Items.Count > 4)
                                    {
                                        MessageBox.Show("Out Buffer is Full : Flushing...");
                                        for (int kk = 0; kk < 5; kk++)
                                        {
                                            string[] row2 = new string[] { outBuffer[kk].Split(',')[0], outBuffer[kk].Split(',')[1], outBuffer[kk].Split(',')[2] };
                                            table.Rows.Add(row2);
                                            gridOut.DataSource = table;
                                        }
                                        outBuffer.Clear();
                                        lstOutBuffer.Items.Clear();
                                    }
                                }
                            }
                        }
                    }
                    if (outBuffer.Count > 0)
                    {
                        foreach (string restBuff in outBuffer)
                        {
                            string[] row3 = new string[] { restBuff.Split(',')[0], restBuff.Split(',')[1], restBuff.Split(',')[2] };
                            table.Rows.Add(row3);
                            gridOut.DataSource = table;
                            lstOutBuffer.Items.Clear();
                        }
                    }
                }
            }
            catch (Exception eee)
            {
                MessageBox.Show("File not found \n\n" + eee.ToString());
            }
            finally
            {
                outBuffer.Clear();
                writeFinalResultsToFile(getQueryRadiaButtonName());
            }
            
        }

        public void blockNestedJoinAccount(string table1, string table2)              
        {

            //MessageBox.Show("reciewd :  " + table1 + " and " + table2);
            List<string> tempTable1 = new List<string>();
            List<string> tempTable2 = new List<string>();

            List<string> inner = new List<string>();
            List<string> outer = new List<string>();
            int mainMemorySize = 5;

            string line;

            try
            {
                MessageBox.Show("line 1 :" + table1);
                using (StreamReader sr = new StreamReader(table1))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        tempTable1.Add(line);
                    }
                }

                MessageBox.Show("line 2 :" + table2);
                using (StreamReader sr = new StreamReader(table2))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        tempTable2.Add(line);
                    }
                }

                string[] t1 = table1.Split('_');
                string[] t2 = table2.Split('_');

                int innerBlockSize = blockSize(t1[0]);
                int outerBlockSize = blockSize(t2[0]);

                int blocksInTable1 = (int)Math.Ceiling((double)tempTable1.Count() / innerBlockSize);
                int blocksInTable2 = (int)Math.Ceiling((double)tempTable2.Count() / outerBlockSize);

                MessageBox.Show("tbl1 = " + blocksInTable1 + "    tbl2 = " + blocksInTable2);

                if (blocksInTable1 < mainMemorySize)    // Smaller table fits to main memory
                {                                       // Then smaller relation to Inner
                    inner = tempTable1;
                    outer = tempTable2;
                }
                else if (blocksInTable2 < mainMemorySize)// Smaller table fits to main memory
                {                                          // Then smaller relation to Inner
                    inner = tempTable2;
                    outer = tempTable1;
                    int temp = innerBlockSize;
                    innerBlockSize = outerBlockSize;
                    outerBlockSize = temp;
                }
                else                                    // Both not fit in to mian memory
                {
                    if (blocksInTable1 < blocksInTable2)// Lager relation to inner
                    {
                        inner = tempTable2;
                        outer = tempTable1;
                        int temp = innerBlockSize;
                        innerBlockSize = outerBlockSize;
                        outerBlockSize = temp;
                    }
                    else
                    {
                        inner = tempTable1;
                        outer = tempTable2;
                    }
                }

                using (System.IO.StreamWriter file = new System.IO.StreamWriter("outer.txt", true))
                {
                    foreach (string i in outer)
                    {
                        file.WriteLine(i);
                    }
                }

                using (System.IO.StreamWriter file = new System.IO.StreamWriter("inner.txt", true))
                {
                    foreach (string i in inner)
                    {
                        file.WriteLine(i);
                    }
                }


                List<string> tempBlock1 = new List<string>();
                List<string> tempBlock2 = new List<string>();

                DataTable tableA = new DataTable();
                tableA.Columns.Add("Customer_name", typeof(string));
                tableA.Columns.Add("balance", typeof(string));

                for (int i = 0; i < outer.Count(); i = i + (outerBlockSize * (mainMemorySize - 1)))
                {
                    tempBlock1.Clear();
                    lstOuter.Items.Add("-------Block " + "= " + i + " to " + (i + outerBlockSize * (mainMemorySize - 1)) + "------");
                    foreach (var ss1 in outer.Skip(i).Take(outerBlockSize * (mainMemorySize - 1)))
                    {
                        lstOuter.Items.Add("" + ss1);
                        tempBlock1.Add("" + ss1);
                    }
                    lstOuter.Items.Add("___________________________");

                    for (int j = 0; j < inner.Count(); j = j + innerBlockSize)
                    {
                        tempBlock2.Clear();
                        lstInner.Items.Add("-------Block " + "= " + j + " to " + (j + innerBlockSize) + "------");
                        foreach (var ss2 in inner.Skip(j).Take(innerBlockSize))
                        {
                            lstInner.Items.Add("" + ss2);
                            tempBlock2.Add("" + ss2);
                        }
                        lstInner.Items.Add("___________________________");

                        HashSet<string> duplMap = new HashSet<string>();
                        foreach (string ii in tempBlock1)
                        {
                            foreach (string jj in tempBlock2)
                            {
                                if (ii.Split(',')[0].Equals(jj.Split(',')[0]) && jj.Split(',')[1].Equals("Chinatown branch"))
                                {

                                    if (!duplMap.Contains(ii.Split(',')[0]))
                                    {
                                        duplMap.Add(ii.Split(',')[0]);
                                        lstOutBuffer.Items.Add(ii.Split(',')[1] + "," + jj.Split(',')[2]);
                                        outBuffer.Add(ii.Split(',')[1] + "," + jj.Split(',')[2]);
                                    }
                                    if (lstOutBuffer.Items.Count > 4)
                                    {
                                        MessageBox.Show("Out Buffer is Full : Flushing...");
                                        for (int kk = 0; kk < 5; kk++)
                                        {
                                            string[] row1 = new string[] { outBuffer[kk].Split(',')[0], outBuffer[kk].Split(',')[1] };
                                            tableA.Rows.Add(row1);
                                            gridOut.DataSource = tableA;
                                        }
                                        outBuffer.Clear();
                                        lstOutBuffer.Items.Clear();
                                    }
                                }
                            }
                        }
                    }
                    if (outBuffer.Count > 0)
                    {
                        MessageBox.Show("Fulsh the rest of the results");
                        foreach (string restBuff in outBuffer)
                        {
                            string[] row1 = new string[] { restBuff.Split(',')[0], restBuff.Split(',')[1] };
                            tableA.Rows.Add(row1);
                            gridOut.DataSource = tableA;
                            lstOutBuffer.Items.Clear();
                        }
                    }
                }
            }
            catch (Exception eee)
            {
                MessageBox.Show("File not found \n\n" + eee.ToString());
            }
            finally
            {
                outBuffer.Clear();
                
                writeFinalResultsToFile(getQueryRadiaButtonName());
            }
        }

        public string getQueryRadiaButtonName() {
            string query = "Query 1";
            foreach (Control c in groupBox4.Controls)
            {
                if (c.GetType() == typeof(RadioButton))
                {
                    RadioButton rb = c as RadioButton;
                    if (rb.Checked)
                    {
                        query = rb.Text;
                    }
                }
            }
            return query;
        }

        public void writeFinalResultsToFile(string query) {

            string outFileName = "Results for "+query+" ("+ DateTime.Now.ToString("MM-dd-yyyy HH-mm-ss") +").txt";
            StringBuilder strBuilder = new StringBuilder();
            MessageBox.Show(outFileName);
            for (int i = 0; i < gridOut.Rows.Count; i++)
            {
                for (int k = 0; k < gridOut.Columns.Count; k++)
                {
                    if (k == gridOut.Columns.Count - 1)
                    {
                        strBuilder.Append(gridOut.Rows[i].Cells[k].Value);
                    }
                    else
                    {
                        strBuilder.Append(gridOut.Rows[i].Cells[k].Value + ",");
                    }
                }
                if (i != gridOut.Rows.Count)
                {
                    strBuilder.Append("\r\n");
                }
            }
            
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(outFileName, true))
            {
                file.WriteLine(strBuilder);
            }

            txtLog.Text = txtLog.Text + Environment.NewLine + Environment.NewLine + Environment.NewLine +
                                        "Final result write to permanent storage"+ Environment.NewLine + 
                                        "File name : " + Environment.NewLine + outFileName;

        }
        
        public void semiJoin(RelationInfo table1, RelationInfo table2) {
            
            string shippingFileName = "";
            string returndFileName = "";
            if (table1.site.Equals(cmbCurrServer.Text))
            {
                
                shippingFileName = table1.table + "_" + table1.site + "_" + table2.site + "_copy.txt";
                returndFileName = table2.table + "_" + table2.site + "_" + table1.site + "_copy.txt";

                try
                {
                    System.IO.File.Delete(shippingFileName);
                    System.IO.File.Delete(returndFileName);
                }
                catch (System.IO.IOException eee)
                {}

                if (table1.table.Equals("customer") && table2.table.Equals("depositor"))
                {
                    txtLog.Text = txtLog.Text + Environment.NewLine + Environment.NewLine;
                    txtLog.Text = txtLog.Text + "Ship " + table1.table + "_key to site " + table2.site;
                    txtLog.Text = txtLog.Text + Environment.NewLine + "temp table = " + shippingFileName;
                    List<string> tempsiteL1 = new List<string>();

                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(shippingFileName, true))
                    {
                        foreach (Customer i in customerTable) {
                            file.WriteLine(i.getCustName());
                            lstSite1.Items.Add(i.getCustName());
                        }
                    }

                    try
                    {
                        using (StreamReader sr = new StreamReader(shippingFileName))
                        {
                            string line;
                            List<string> tempSite1 = new List<string>();

                            while ((line = sr.ReadLine()) != null)
                            {
                                foreach (Depositor i in depositorTable)
                                {
                                    if (line.Equals(i.getKey1()))
                                    {
                                        tempSite1.Add(line);
                                        break;
                                    }
                                }
                            }
                            tempsiteL1 = tempSite1.ToList<string>();
                            lstSite2.DataSource = tempsiteL1;

                        }
                        txtLog.Text = txtLog.Text + Environment.NewLine + Environment.NewLine;
                        txtLog.Text = txtLog.Text + "recieved from site 2 : " + shippingFileName;

                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(returndFileName, true))
                        {
                            foreach (string i in tempsiteL1)
                            {
                                file.WriteLine(i);
                            }
                        }

                        txtLog.Text = txtLog.Text + Environment.NewLine + Environment.NewLine;
                        txtLog.Text = txtLog.Text + "recieved back to site 1 : " + returndFileName;                        
                        blockNestedJoin(table1.table+"_"+table1.site+".txt",returndFileName);
                    }
                    catch (Exception eee)
                    {
                        MessageBox.Show("File not found customer  \n\n" + eee.ToString());
                    }
                }
                if (table1.table.Equals("depositor") && table2.table.Equals("customer"))
                {
                    txtLog.Text = txtLog.Text + Environment.NewLine + Environment.NewLine;
                    txtLog.Text = txtLog.Text + "Ship " + table1.table + "_key to site " + table2.site;
                    txtLog.Text = txtLog.Text + Environment.NewLine + "temp table = " + shippingFileName;
                    List<string> tempsiteL1 = new List<string>();
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(shippingFileName, true))
                    {
                        foreach (Depositor i in depositorTable)
                        {
                            file.WriteLine(i.getKey1());
                            lstSite1.Items.Add(i.getKey1());
                        }
                    }

                    try
                    {
                        HashSet<string> tempSite2 = new HashSet<string>();
                        using (StreamReader sr = new StreamReader(shippingFileName))
                        {
                            string line;
                            

                            while ((line = sr.ReadLine()) != null)
                            {
                                foreach (Customer i in customerTable)
                                {
                                    if (line.Equals(i.getCustName()))
                                    {
                                        tempSite2.Add(i.getCustName()+","+i.getStreet()+","+i.getCity());
                                        break;
                                    }
                                }
                            }
                            tempsiteL1 = tempSite2.ToList<string>();
                            lstSite2.DataSource = tempsiteL1;

                        }
                        txtLog.Text = txtLog.Text + Environment.NewLine + Environment.NewLine;
                        txtLog.Text = txtLog.Text + "shiped to site 2 : " + shippingFileName;

                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(returndFileName, true))
                        {
                            foreach (string i in tempSite2)
                            {
                                file.WriteLine(i);
                            }
                        }

                        txtLog.Text = txtLog.Text + Environment.NewLine + Environment.NewLine;
                        txtLog.Text = txtLog.Text + "recieved back to site 1 : " + returndFileName;
                        blockNestedJoin(table1.table + "_" + table1.site + ".txt", returndFileName);
                    }
                    catch (Exception eee)
                    {
                        MessageBox.Show("File not found customer  \n\n" + eee.ToString());
                    }
                    
                }
                if (table1.table.Equals("depositor") && table2.table.Equals("account"))
                {
                    txtLog.Text = txtLog.Text + Environment.NewLine + Environment.NewLine;
                    txtLog.Text = txtLog.Text + "Ship " + table1.table + "_key to site " + table2.site;
                    txtLog.Text = txtLog.Text + Environment.NewLine + "temp table = " + shippingFileName;
                    List<string> tempsiteL1 = new List<string>();
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(shippingFileName, true))
                    {
                        foreach (Depositor i in depositorTable)
                        {
                            file.WriteLine(i.getKey1());
                            lstSite1.Items.Add(i.getKey1());
                        }
                    }

                    try
                    {
                        HashSet<string> tempSite2 = new HashSet<string>();
                        using (StreamReader sr = new StreamReader(shippingFileName))
                        {
                            string line;


                            while ((line = sr.ReadLine()) != null)
                            {
                                foreach (Customer i in customerTable)
                                {
                                    if (line.Equals(i.getCustName()))
                                    {
                                        tempSite2.Add(i.getCustName() + "," + i.getStreet() + "," + i.getCity());
                                        break;
                                    }
                                }
                            }
                            tempsiteL1 = tempSite2.ToList<string>();
                            lstSite2.DataSource = tempsiteL1;

                        }
                        txtLog.Text = txtLog.Text + Environment.NewLine + Environment.NewLine;
                        txtLog.Text = txtLog.Text + "shiped to site 2 : " + shippingFileName;

                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(returndFileName, true))
                        {
                            foreach (string i in tempSite2)
                            {
                                file.WriteLine(i);
                            }
                        }

                        txtLog.Text = txtLog.Text + Environment.NewLine + Environment.NewLine;
                        txtLog.Text = txtLog.Text + "recieved back to site 1 : " + returndFileName;
                        blockNestedJoin(table1.table + "_" + table1.site + ".txt", returndFileName);
                    }
                    catch (Exception eee)
                    {
                        MessageBox.Show("File not found customer  \n\n" + eee.ToString());
                    }

                }
                if (table2.table.Equals("branch"))
                {
                    txtLog.Text = txtLog.Text + Environment.NewLine;
                    txtLog.Text = txtLog.Text + "Ship " + table2.table + "_key to site " + table1.site;
                }
                if (table2.table.Equals("account"))
                {
                    txtLog.Text = txtLog.Text + Environment.NewLine;
                    txtLog.Text = txtLog.Text + "Ship " + table2.table + "_key to site " + table1.site;
                    MessageBox.Show("accounttttttttttttttttttttt = "+table1.site);
                }
            }

////////////////////////////////////////////////////////////////

            else if (table2.site.Equals(cmbCurrServer.Text))
            {
                
                shippingFileName = table2.table + "_" + table2.site + "_" + table1.site + "_copy.txt";
                returndFileName = table1.table + "_" + table1.site + "_" + table2.site + "_copy.txt";
                try
                {
                    System.IO.File.Delete(shippingFileName);
                    System.IO.File.Delete(returndFileName);
                }
                catch (System.IO.IOException eee)
                {
                    MessageBox.Show("File not found depositor  \n\n" + eee.ToString());
                }

                if (table2.table.Equals("customer") && table1.table.Equals("depositor"))
                {
                    txtLog.Text = txtLog.Text + Environment.NewLine + Environment.NewLine;
                    txtLog.Text = txtLog.Text + "Ship " + table2.table + "_key to site " + table1.site;
                    txtLog.Text = txtLog.Text + Environment.NewLine + "temp table = " + shippingFileName;
                    List<string> tempsiteL1 = new List<string>();

                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(shippingFileName, true))
                    {
                        foreach (Customer i in customerTable)
                        {
                            file.WriteLine(i.getCustName());
                            lstSite1.Items.Add(i.getCustName());
                        }
                    }

                    try
                    {
                        using (StreamReader sr = new StreamReader(shippingFileName))
                        {
                            string line;
                            HashSet<string> tempSite1 = new HashSet<string>();


                            while ((line = sr.ReadLine()) != null)
                            {
                                foreach (Depositor i in depositorTable)
                                {
                                    if (line.Equals(i.getKey1()))
                                    {
                                        tempSite1.Add(line);
                                        break;
                                    }
                                }
                            }
                            tempsiteL1 = tempSite1.ToList<string>();
                            lstSite2.DataSource = tempsiteL1;

                        }
                        txtLog.Text = txtLog.Text + Environment.NewLine + Environment.NewLine;
                        txtLog.Text = txtLog.Text + "recieved from site 2 : " + shippingFileName;

                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(returndFileName, true))
                        {
                            foreach (string i in tempsiteL1)
                            {
                                file.WriteLine(i);
                            }
                        }

                        txtLog.Text = txtLog.Text + Environment.NewLine + Environment.NewLine;
                        txtLog.Text = txtLog.Text + "recieved back to site 1 : " + returndFileName;
                        blockNestedJoin(table2.table + "_" + table2.site + ".txt", returndFileName);
                    }

                    catch (Exception eee)
                    {
                        MessageBox.Show("File not found customer  \n\n" + eee.ToString());
                    }

                }
                if (table1.table.Equals("customer") && table2.table.Equals("depositor"))
                {
                    //MessageBox.Show("starting depos site 2 ----------------------------");
                    txtLog.Text = txtLog.Text + Environment.NewLine + Environment.NewLine;
                    txtLog.Text = txtLog.Text + " Ship " + table2.table + " key to site " + table1.site;
                    txtLog.Text = txtLog.Text + Environment.NewLine + "temp table = " + shippingFileName;
                    List<string> tempsiteL1 = new List<string>();


                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(shippingFileName, true))
                    {
                        foreach (Depositor i in depositorTable)
                        {
                            file.WriteLine(i.getKey1());
                            lstSite1.Items.Add(i.getKey1());
                        }
                    }

                    try
                    {
                        HashSet<string> tempSite2 = new HashSet<string>();
                        using (StreamReader sr = new StreamReader(shippingFileName))
                        {
                            string line;
                            
                            while ((line = sr.ReadLine()) != null)
                            {                                
                                foreach (Customer i in customerTable)
                                {
                                    if (line.Equals(i.getCustName()))
                                    {
                                        tempSite2.Add(i.getCustName() + "," + i.getStreet() + "," + i.getCity());
                                        break;
                                    }                           
                                }
                            }
                            
                            tempsiteL1 = tempSite2.ToList<string>();
                            lstSite2.DataSource = tempsiteL1;
                        }
                        txtLog.Text = txtLog.Text + Environment.NewLine + Environment.NewLine;
                        txtLog.Text = txtLog.Text + "shiped to site 2 : "+ shippingFileName;

                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(returndFileName, true))
                        {
                            foreach (string i in tempSite2)
                            {
                                file.WriteLine(i);
                            }
                        }

                        txtLog.Text = txtLog.Text + Environment.NewLine + Environment.NewLine;
                        txtLog.Text = txtLog.Text + "recieved back to site 1 : " + returndFileName;
                        
                        blockNestedJoin(table2.table + "_" + table2.site + ".txt", returndFileName);
                    }
                    catch (Exception eee)
                    {
                        MessageBox.Show("File not found customer  \n\n" + eee.ToString());
                    }

                }
                if (table1.table.Equals("depositor") && table2.table.Equals("account")) // working
                {
                    MessageBox.Show("Dep acc semi");
                    MessageBox.Show("table 2 :"+table2.table+"  site 2:"+table2.site);
                    MessageBox.Show("table 1 :" + table1.table + "  site 1:" + table1.site);
                    txtLog.Text = txtLog.Text + Environment.NewLine + Environment.NewLine;
                    txtLog.Text = txtLog.Text + "Ship " + table2.table + "_key to site " + table1.site;
                    txtLog.Text = txtLog.Text + Environment.NewLine + "temp table = " + shippingFileName;
                    List<string> tempsiteL1 = new List<string>();
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(shippingFileName, true))
                    {
                        foreach (Account i in accountTable)
                        {
                            file.WriteLine(i.getKey());
                            lstSite1.Items.Add(i.getKey());
                        }
                    }

                    try
                    {
                        HashSet<string> tempSite2 = new HashSet<string>();
                        using (StreamReader sr = new StreamReader(shippingFileName))
                        {
                            string line;


                            while ((line = sr.ReadLine()) != null)
                            {
                                foreach (Depositor i in depositorTable)
                                {
                                    if (line.Equals(i.getAccNo()))
                                    {
                                        tempSite2.Add(i.getAccNo() + "," + i.getCustName());
                                        break;
                                    }
                                }
                            }
                            tempsiteL1 = tempSite2.ToList<string>();
                            lstSite2.DataSource = tempsiteL1;

                        }
                        txtLog.Text = txtLog.Text + Environment.NewLine + Environment.NewLine;
                        txtLog.Text = txtLog.Text + "shiped to site 2 : " + shippingFileName;

                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(returndFileName, true))
                        {
                            foreach (string i in tempSite2)
                            {
                                file.WriteLine(i);
                            }
                        }

                        txtLog.Text = txtLog.Text + Environment.NewLine + Environment.NewLine;
                        txtLog.Text = txtLog.Text + "recieved back to site 1 : " + returndFileName;
                        blockNestedJoinAccount(table2.table + "_" + table2.site + ".txt", returndFileName);
                    }
                    catch (Exception eee)
                    {
                        MessageBox.Show("File not found customer  \n\n" + eee.ToString());
                    }

                }
                if (table1.table.Equals("branch"))
                {
                    txtLog.Text = txtLog.Text + Environment.NewLine + Environment.NewLine;
                    txtLog.Text = txtLog.Text + " Ship " + table1.table + " key to site" + table2.site;
                }
                if (table1.table.Equals("account"))
                {
                    txtLog.Text = txtLog.Text + Environment.NewLine + Environment.NewLine;
                    txtLog.Text = txtLog.Text + " Ship " + table1.table + " key to site" + table2.site;
                }
            }
            else
            {
                MessageBox.Show("tbl 1 :"+table1.table+"   tbl 2 :"+table2.table);
                twoWaySemijoin(table1,table2,cmbCurrServer.Text);
            }
        }

        public void joinProcess(List<string> fileList) {

            string[] f_info;
            foreach (string files in fileList)
            {
                f_info = files.Split('_');                
                readTable(f_info[0], f_info[1]);
            }            
        }

        public List<string> conditionCheck(string condition) {

            string[] condList;
            List<string> list = new List<string>();

            Regex rgx = new Regex("AND");
            string result = rgx.Replace(condition, "~");                
            condList = result.Split('~');

            foreach (string i in condList)
            {
                txtLog.Text = txtLog.Text + Environment.NewLine + i +Environment.NewLine;
                list.Add(i);
            }

            return list;
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                clearSites();
                gridOut.DataSource = null;

                string[] fromList = txtxFrom.Text.ToString().Split(',');
                bool fileFlag = true;
                foreach (string i in fromList) {
                    if (!tables.Contains(i.Trim()) || txtWhere.Text == "") {
                        txtLog.Text = txtLog.Text + Environment.NewLine + "Query error. No such a table found = " + i+"\n\nPlease try again  ";
                        txtSelect.Text = "";
                        fileFlag = false;
                    }
                }                

                if (fileFlag) {
                    txtLog.Text = txtLog.Text + Environment.NewLine + "query ok = "+getQueryRadiaButtonName() + Environment.NewLine;
                    joinProcess(loadTables(fromList, conditionCheck(txtWhere.Text)));

                    foreach (RelationInfo i in tableSet)
                    {
                        txtLog.Text = txtLog.Text + Environment.NewLine + "Site is = " + i.site + "  and Table is = " + i.table;
                    }

                    semiJoin(tableSet[0], tableSet[1]);
                    tableSet.Clear();

                    txtLog.Text = txtLog.Text + Environment.NewLine + Environment.NewLine;
                }              
            }
            catch (Exception err) {
                txtSelect.Text = err.ToString();
            }
        }

        public void clearSites()
        {
            try
            {
                lstSite1.Items.Clear();
                
            }
            catch (Exception ee1)
            {
                lstSite1.DataSource = null;
            }
            try
            {
                lstSite2.Items.Clear();
            }
            catch (Exception ee2)
            {
                lstSite2.DataSource = null;
            }
            
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            try { 
                lstInner.Items.Clear();               
            }
            catch (Exception ee1) {
                lstInner.DataSource = null;                
            }
            try {
                lstOuter.Items.Clear();
            }
            catch (Exception ee2) {
                lstOuter.DataSource = null;
            }            
            tableSet.Clear();
            gridOut.DataSource = null;
            lstOutBuffer.Items.Clear();
        }

        private void btnClearLog_Click(object sender, EventArgs e)
        {
            txtLog.Text = "";
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void cmbCurrServer_SelectedIndexChanged(object sender, EventArgs e)
        {
            lstOutBuffer.Items.Clear();
            lstInner.Items.Clear();
            lstOuter.Items.Clear();
            gridOut.DataSource = null;
        }

        private void rdo1_CheckedChanged(object sender, EventArgs e)
        {
            if(rdo1.Checked == true){
                cmbCurrServer.SelectedIndex = 0;
                txtSelect.Text = "cust_name,street,city";
                txtxFrom.Text = "customer,depositor";
                txtWhere.Text = "customer.cust_name = depositor.cust_name";
            }
        }

        private void rdo2_CheckedChanged(object sender, EventArgs e)
        {
            cmbCurrServer.SelectedIndex = 3;
            txtSelect.Text = "cust_name,balance";
            txtxFrom.Text = "depositor,account";
            txtWhere.Text = "depositor.acc_number = account.acc_number AND account.bran_name = 'Chinatown branch'";
        }

        private void rdo3_CheckedChanged(object sender, EventArgs e)
        {
            cmbCurrServer.SelectedIndex = 3;
            txtSelect.Text = "cust_name,street,city";
            txtxFrom.Text = "depositor,account,customer";
            txtWhere.Text = "depositor.acc_number = account.acc_number AND depositor.cust_name = customer.cust_name AND account.acc_number = 'A85486371'";
        }
    }
}
