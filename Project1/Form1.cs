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
            txtMemorySize.Text = 5 + "";
            this.query4 = false;
        }


        // table data structures
        List<Account> accountTable;
        List<Branch> branchTable;
        List<Depositor> depositorTable;
        List<Customer> customerTable;

        // Relation list according to the joining process
        List<RelationInfo> tableSet = new List<RelationInfo>();

        // Default main memory size (blocks)
        int mainMemorySize = 5;
        double balanceCondition = 0;
        bool query4 = false;
        bool possitiveQ4 = true;

        public string[] tables = {"branch", "account","depositor", "customer"};
        List<string> twoWaySite1 = new List<string>();
        List<string> twoWaySite2 = new List<string>();

        public List<string> outBuffer = new List<string>();
        

        /********** Function list **************/

        public void writeQuery4ToFile()
        {

            string fileName = "depositor_NYC_HOU_Copy.txt";

            try
            {
                System.IO.File.Delete(fileName);
            }
            catch (System.IO.IOException eee)
            { }
            try
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileName, true))
                {
                    foreach (string i in lstSite2.Items)
                    {
                        file.WriteLine(i);
                    }
                }

            }
            catch (Exception eee) { }
        }

        public void twoWaySemijoin(RelationInfo table1, RelationInfo table2, string currentSite) {
                      

            if (table1.table.Equals("depositor") && table2.table.Equals("account"))
            {

                txtLog.Text = txtLog.Text + Environment.NewLine + Environment.NewLine;
                txtLog.Text = txtLog.Text + "Ship " + table1.table + "_key to " + table2.site;
                txtLog.Text = txtLog.Text + Environment.NewLine;
               

                List<Depositor> t1 = new List<Depositor>(depositorTable);
                List<Account> t2 = new List<Account>(accountTable);

                string FileName = "depositor_NYC_HOU_Copy.txt";
                txtLog.Text = txtLog.Text + Environment.NewLine + "temp table = "+ FileName;
                txtLog.Text = txtLog.Text + Environment.NewLine;
                txtLog.Text = txtLog.Text + Environment.NewLine;
                txtLog.Text = txtLog.Text + "----------------------------------";
                try
                {
                    System.IO.File.Delete(FileName);
                }
                catch (System.IO.IOException eee)
                { }

                blockNestedJoinSameSite(FileName);

                
                txtLog.Text = txtLog.Text + Environment.NewLine;
                txtLog.Text = txtLog.Text + "First join session finished";
                txtLog.Text = txtLog.Text + Environment.NewLine;
                txtLog.Text = txtLog.Text + "Results write to file " + Environment.NewLine;
                txtLog.Text = txtLog.Text + Environment.NewLine;
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

                txtLog.Text = txtLog.Text + Environment.NewLine + Environment.NewLine;
                txtLog.Text = txtLog.Text + "Ship " + FileName + " to site HOU";
                blockNestedJoin("customer_HOU.txt", FileName);

                txtLog.Text = txtLog.Text + Environment.NewLine;
                txtLog.Text = txtLog.Text + Environment.NewLine;
                //txtLog.Text = txtLog.Text + "File shiped to the site : "+cmbCurrServer.Text;
                

            }
            else if (table1.table.Equals("customer") && table2.table.Equals("depositor")) {
                
                List<Depositor> t1 = new List<Depositor>(depositorTable);
                List<Account> t2 = new List<Account>(accountTable);

                string FileName = "customer_HOU_NYC_Copy.txt";

                try
                {
                    System.IO.File.Delete(FileName);
                }
                catch (System.IO.IOException eee)
                { }
                
                semiJoinCustWithDepositor();

            }
        }

        public void blockNestedJoinBalance(string table1, string table2)
        {

            List<string> tempTable1 = new List<string>();
            List<string> tempTable2 = new List<string>();

            List<string> inner = new List<string>();
            List<string> outer = new List<string>();
            int mainMemorySize = this.mainMemorySize;

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

                int t1BlockSize = blockSize(t1[0],tempTable1.Count);
                int t2BlockSize = blockSize(t2[0],tempTable2.Count);

                int innerPerBlock = perBlock(t1[0]);
                int outerPerBlock = perBlock(t2[0]);

                MessageBox.Show("Relation 1 has " + t1BlockSize + " blocks   AND   Relation 2 has " + t2BlockSize+" blocks");

                bool changed = false;

                if (t1BlockSize < mainMemorySize && t2BlockSize >= mainMemorySize)
                {
                    // then table1 is Inner relation and Table2 is Outer
                    inner = tempTable1;
                    outer = tempTable2;
                }
                else if (t2BlockSize < mainMemorySize && t1BlockSize >= mainMemorySize)
                {
                    // then table2 is Inner relation and Table1 is Outer
                    changed = true;
                    inner = tempTable2;
                    outer = tempTable1;

                    int temp = t1BlockSize;
                    t1BlockSize = t2BlockSize;
                    t2BlockSize = temp;

                    temp = innerPerBlock;
                    innerPerBlock = outerPerBlock;
                    outerPerBlock = temp;

                }
                else if (t1BlockSize < mainMemorySize && t2BlockSize < mainMemorySize)
                {
                    // of both the relations are fit in main memory
                    if (t1BlockSize <= t2BlockSize)
                    {
                        inner = tempTable1;
                        outer = tempTable2;
                    }
                    else
                    {
                        changed = true;

                        inner = tempTable2;
                        outer = tempTable1;
                        int temp = t1BlockSize;
                        t1BlockSize = t2BlockSize;
                        t2BlockSize = temp;

                        temp = innerPerBlock;
                        innerPerBlock = outerPerBlock;
                        outerPerBlock = temp;
                    }
                }
                else
                {
                    /* if both relations are contain blocks more than main memory
                       then smaller relation is used as Outer relation
                       I/O complexity is O(m/kB * (n/B + k))  */

                    if (t1BlockSize < t2BlockSize)
                    {
                        changed = true;

                        inner = tempTable2;
                        outer = tempTable1;
                        int temp = t1BlockSize;
                        t1BlockSize = t2BlockSize;
                        t2BlockSize = temp;

                        temp = innerPerBlock;
                        innerPerBlock = outerPerBlock;
                        outerPerBlock = temp;
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

                for (int i = 0; i < outer.Count(); i = i + (outerPerBlock * (mainMemorySize - 1)))
                {
                    tempBlock1.Clear();
                    lstOuter.Items.Add("-------Records " + "from " + i + " to " + (i + outerPerBlock * (mainMemorySize - 1)) + "------");
                    foreach (var ss1 in outer.Skip(i).Take(outerPerBlock * (mainMemorySize - 1)))
                    {
                        lstOuter.Items.Add("" + ss1);
                        tempBlock1.Add("" + ss1);
                    }
                    lstOuter.Items.Add("___________________________");

                    for (int j = 0; j < inner.Count(); j = j + innerPerBlock)
                    {
                        tempBlock2.Clear();
                        lstInner.Items.Add("-------Records " + "from " + j + " to " + (j + innerPerBlock) + "------");
                        foreach (var ss2 in inner.Skip(j).Take(innerPerBlock))
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
                                        if (jj.Split(',').Length == 3)
                                        {
                                            duplMap.Add(jj.Split(',')[0]);
                                            lstOutBuffer.Items.Add(jj);
                                            outBuffer.Add(jj);
                                        }
                                        else
                                        {
                                            duplMap.Add(ii.Split(',')[0]);
                                            lstOutBuffer.Items.Add(ii);
                                            outBuffer.Add(ii);
                                        }

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
        
        


        // Query 4 (Third join) block nested loop join with semi join results (At HOU)
        public void blockNestedJoinSemiJoinPossitiveNegative(string table1, string table2)
        {
            txtLog.Text = txtLog.Text + Environment.NewLine;

            if (possitiveQ4)
            {
                txtLog.Text = txtLog.Text + "Two way semi join returns with matching set...";
            }
            else
            {
                txtLog.Text = txtLog.Text + "Two way semi join returns with non matching set...";
            }


            clearGrid();
            try
            {
                lstInner.Items.Clear();
            }
            catch (Exception ee1)
            {
                lstInner.DataSource = null;
            }
            try
            {
                lstOuter.Items.Clear();
            }
            catch (Exception ee2)
            {
                lstOuter.DataSource = null;
            }

            List<string> tempTable1 = new List<string>();
            List<string> tempTable2 = new List<string>();

            List<string> inner = new List<string>();
            List<string> outer = new List<string>();
            int mainMemorySize = this.mainMemorySize;

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

                int t1BlockSize = blockSize(t1[0], tempTable1.Count);
                int t2BlockSize = blockSize(t2[0], tempTable2.Count);

                int innerPerBlock = perBlock(t1[0]);
                int outerPerBlock = perBlock(t2[0]);

                MessageBox.Show("Relation 1 has " + t1BlockSize + " blocks AND  Relation 2 has " + t2BlockSize + " blocks");

                bool changed = false;

                if (t1BlockSize < mainMemorySize && t2BlockSize >= mainMemorySize)
                {
                    // then table1 is Inner relation and Table2 is Outer
                    inner = tempTable1;
                    outer = tempTable2;
                }
                else if (t2BlockSize < mainMemorySize && t1BlockSize >= mainMemorySize)
                {
                    // then table2 is Inner relation and Table1 is Outer
                    changed = true;
                    inner = tempTable2;
                    outer = tempTable1;

                    int temp = t1BlockSize;
                    t1BlockSize = t2BlockSize;
                    t2BlockSize = temp;

                    temp = innerPerBlock;
                    innerPerBlock = outerPerBlock;
                    outerPerBlock = temp;

                }
                else if (t1BlockSize < mainMemorySize && t2BlockSize < mainMemorySize)
                {
                    // of both the relations are fit in main memory
                    if (t1BlockSize <= t2BlockSize)
                    {
                        inner = tempTable1;
                        outer = tempTable2;
                    }
                    else
                    {
                        changed = true;

                        inner = tempTable2;
                        outer = tempTable1;
                        int temp = t1BlockSize;
                        t1BlockSize = t2BlockSize;
                        t2BlockSize = temp;

                        temp = innerPerBlock;
                        innerPerBlock = outerPerBlock;
                        outerPerBlock = temp;
                    }
                }
                else
                {
                    /* if both relations are contain blocks more than main memory
                       then smaller relation is used as Outer relation
                       I/O complexity is O(m/kB * (n/B + k))  */

                    if (t1BlockSize < t2BlockSize)
                    {
                        changed = true;

                        inner = tempTable2;
                        outer = tempTable1;
                        int temp = t1BlockSize;
                        t1BlockSize = t2BlockSize;
                        t2BlockSize = temp;

                        temp = innerPerBlock;
                        innerPerBlock = outerPerBlock;
                        outerPerBlock = temp;
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


                for (int i = 0; i < outer.Count(); i = i + (outerPerBlock * (mainMemorySize - 1)))
                {
                    tempBlock1.Clear();
                    lstOuter.Items.Add("-------Records " + "from " + i + " to " + (i + outerPerBlock * (mainMemorySize - 1)) + "------");
                    lstOuter.Items.Add(" ");
                    foreach (var ss1 in outer.Skip(i).Take(outerPerBlock * (mainMemorySize - 1)))
                    {
                        lstOuter.Items.Add("" + ss1);
                        tempBlock1.Add("" + ss1);
                    }
                    lstOuter.Items.Add("___________________________");

                    for (int j = 0; j < inner.Count(); j = j + innerPerBlock)
                    {
                        tempBlock2.Clear();
                        lstInner.Items.Add("-------Records " + "from " + j + " to " + (j + innerPerBlock) + "------");
                        lstInner.Items.Add(" ");
                        foreach (var ss2 in inner.Skip(j).Take(innerPerBlock))
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
                                if (possitiveQ4)                                    // if it is possitive set from two way semi join result
                                {
                                    if (ii.Split(',')[0].Equals(jj.Split(',')[0]))  // Join matching tuples
                                    {
                                        if (!duplMap.Contains(ii.Split(',')[0]))
                                        {
                                            if (jj.Split(',').Length == 3)
                                            {
                                                duplMap.Add(jj.Split(',')[0]);
                                                lstOutBuffer.Items.Add(jj);
                                                outBuffer.Add(jj);
                                            }
                                            else
                                            {
                                                duplMap.Add(ii.Split(',')[0]);
                                                lstOutBuffer.Items.Add(ii);
                                                outBuffer.Add(ii);
                                            }

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
                                else                                                            // if it is negative set from two way semi join result
                                {
                                    if (!ii.Split(',')[0].Equals(jj.Split(',')[0]))             // Join non-matching tuples
                                    {
                                        if (!duplMap.Contains(ii.Split(',')[0]))
                                        {
                                            if (jj.Split(',').Length == 3)
                                            {
                                                duplMap.Add(jj.Split(',')[0]);
                                                lstOutBuffer.Items.Add(jj);
                                                outBuffer.Add(jj);
                                            }
                                            else
                                            {
                                                duplMap.Add(ii.Split(',')[0]);
                                                lstOutBuffer.Items.Add(ii);
                                                outBuffer.Add(ii);
                                            }

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

        // Query 4 (Second join) block nested loop join with semi join results (At NYC)
        public void blockNestedJoinSameSiteBalance(List<string> dipList, string table2)
        {

            //MessageBox.Show("reciewd :  " + table1 + " and " + table2);
            List<string> tempTable1 = new List<string>();
            List<string> tempTable2 = new List<string>();

            List<string> inner = new List<string>();
            List<string> outer = new List<string>();
            int mainMemorySize = this.mainMemorySize;

            string line;

            try
            {

                foreach (string d in dipList)
                {
                    tempTable1.Add(d);
                }

                foreach (Account a in accountTable)
                {
                    tempTable2.Add(a.getAccountNumber() + "," + a.getBranchName() + "," + a.getBalance());
                }


                int t1BlockSize = blockSize("depositor", tempTable1.Count);
                int t2BlockSize = blockSize("account", tempTable2.Count);

                int innerPerBlock = perBlock("depositor");
                int outerPerBlock = perBlock("account");

                MessageBox.Show("Relation 1 has " + t1BlockSize + " blocks AND  Relation 2 has " + t2BlockSize + " blocks");



                bool changed = false;

                if (t1BlockSize < mainMemorySize && t2BlockSize >= mainMemorySize)
                {
                    // then table1 is Inner relation and Table2 is Outer
                    inner = tempTable1;
                    outer = tempTable2;
                }
                else if (t2BlockSize < mainMemorySize && t1BlockSize >= mainMemorySize)
                {
                    // then table2 is Inner relation and Table1 is Outer
                    changed = true;
                    inner = tempTable2;
                    outer = tempTable1;

                    int temp = t1BlockSize;
                    t1BlockSize = t2BlockSize;
                    t2BlockSize = temp;

                    temp = innerPerBlock;
                    innerPerBlock = outerPerBlock;
                    outerPerBlock = temp;

                }
                else if (t1BlockSize < mainMemorySize && t2BlockSize < mainMemorySize)
                {
                    // of both the relations are fit in main memory
                    if (t1BlockSize <= t2BlockSize)
                    {
                        inner = tempTable1;
                        outer = tempTable2;
                    }
                    else
                    {
                        changed = true;

                        inner = tempTable2;
                        outer = tempTable1;
                        int temp = t1BlockSize;
                        t1BlockSize = t2BlockSize;
                        t2BlockSize = temp;

                        temp = innerPerBlock;
                        innerPerBlock = outerPerBlock;
                        outerPerBlock = temp;
                    }
                }
                else
                {
                    /* if both relations are contain blocks more than main memory
                       then smaller relation is used as Outer relation
                       I/O complexity is O(m/kB * (n/B + k))  */

                    if (t1BlockSize < t2BlockSize)
                    {
                        changed = true;

                        inner = tempTable2;
                        outer = tempTable1;
                        int temp = t1BlockSize;
                        t1BlockSize = t2BlockSize;
                        t2BlockSize = temp;

                        temp = innerPerBlock;
                        innerPerBlock = outerPerBlock;
                        outerPerBlock = temp;
                    }
                    else
                    {
                        inner = tempTable1;
                        outer = tempTable2;
                    }
                }
                try
                {
                    System.IO.File.Delete("outer.txt");
                    System.IO.File.Delete("inner.txt");
                }
                catch (System.IO.IOException eee)
                { }

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
                HashSet<string> custPossitive = new HashSet<string>();
                HashSet<string> custNegative = new HashSet<string>();
                DataTable table = new DataTable();
                table.Columns.Add("Customer_name", typeof(string));
                table.Columns.Add("Accunt_Number", typeof(string));
                table.Columns.Add("Balance", typeof(string));
                string fromSite1 = "";

                for (int i = 0; i < outer.Count(); i = i + (outerPerBlock * (mainMemorySize - 1)))
                {
                    tempBlock1.Clear();
                    lstOuter.Items.Add("-------Records " + "from " + i + " to " + (i + outerPerBlock * (mainMemorySize - 1)) + "------");
                    lstOuter.Items.Add(" ");
                    foreach (var ss1 in outer.Skip(i).Take(outerPerBlock * (mainMemorySize - 1)))
                    {
                        lstOuter.Items.Add("" + ss1);
                        tempBlock1.Add("" + ss1);
                    }
                    lstOuter.Items.Add("___________________________");
                    HashSet<string> duplMap = new HashSet<string>();

                    for (int j = 0; j < inner.Count(); j = j + innerPerBlock)
                    {
                        tempBlock2.Clear();
                        lstInner.Items.Add("-------Records " + "from " + j + " to " + (j + innerPerBlock) + "------");
                        lstInner.Items.Add(" ");
                        foreach (var ss2 in inner.Skip(j).Take(innerPerBlock))
                        {
                            lstInner.Items.Add("" + ss2);
                            tempBlock2.Add("" + ss2);
                        }
                        lstInner.Items.Add("___________________________");


                        foreach (string ii in tempBlock1)
                        {
                            foreach (string jj in tempBlock2)
                            {
                                if (ii.Split(',')[0].Equals(jj.Split(',')[0]))
                                {
                                    if (!duplMap.Contains(ii.Split(',')[0]))
                                    {
                                        if (changed)
                                        {
                                            double amount;
                                            if (Double.TryParse(jj.Split(',')[2], out amount))
                                            {
                                                if (amount > balanceCondition)
                                                {
                                                    duplMap.Add(jj.Split(',')[0]);
                                                    lstOutBuffer.Items.Add(jj.Split(',')[0] + "," + ii.Split(',')[1] + "," + jj.Split(',')[2]);
                                                    outBuffer.Add(jj.Split(',')[0] + "," + ii.Split(',')[1] + "," + jj.Split(',')[2]);
                                                    custPossitive.Add(ii.Split(',')[1]);
                                                }
                                                else
                                                {
                                                    custNegative.Add(ii.Split(',')[1]);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            double amount;
                                            if (Double.TryParse(ii.Split(',')[2], out amount))
                                            {
                                                if (amount > balanceCondition)
                                                {
                                                    duplMap.Add(ii.Split(',')[0]);
                                                    lstOutBuffer.Items.Add(ii.Split(',')[0] + "," + jj.Split(',')[1] + "," + ii.Split(',')[2]);
                                                    outBuffer.Add(ii.Split(',')[0] + "," + jj.Split(',')[1] + "," + ii.Split(',')[2]);
                                                    custPossitive.Add(jj.Split(',')[1]);
                                                }
                                                else
                                                {
                                                    custNegative.Add(jj.Split(',')[1]);
                                                }
                                            }
                                        }
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

                try
                { lstSite1.Items.Clear(); }
                catch (Exception ee1) { lstSite1.DataSource = null; }
                try { lstSite2.Items.Clear(); }
                catch (Exception ee2) { lstSite2.DataSource = null; }
                try { lstInner.Items.Clear(); }
                catch (Exception ee1) { lstInner.DataSource = null; }
                try { lstOuter.Items.Clear(); }
                catch (Exception ee2) { lstOuter.DataSource = null; }

                lstSite1.DataSource = custPossitive.ToList<string>();

                if (custPossitive.Count > custNegative.Count)
                {
                    possitiveQ4 = false;
                    MessageBox.Show("Non matching set is exporting...");
                    foreach (string ss1 in custNegative)
                    {
                        lstSite2.Items.Add(ss1);
                        twoWaySite1.Add(ss1);
                    }
                }
                else
                {
                    possitiveQ4 = true;
                    MessageBox.Show("Matching set is exporting...");
                    foreach (string ss1 in custPossitive)
                    {
                        lstSite2.Items.Add(ss1);
                        twoWaySite1.Add(ss1);
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
                writeQuery4ToFile();
            }
        }

        // Query 4 (First join) block nested loop join with semi join results (At NYC)
        public void semiJoinCustWithDepositor()
        {
            string shippingFileName = "Customer_HOU_NYC_Copy.txt";
            txtLog.Text = txtLog.Text + Environment.NewLine + Environment.NewLine;
            txtLog.Text = txtLog.Text + "Ship Customer_key to site NYC";
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

            txtLog.Text = txtLog.Text + Environment.NewLine + Environment.NewLine;
            txtLog.Text = txtLog.Text + "Customer copy recieved to NYC site";

            txtLog.Text = txtLog.Text + Environment.NewLine + Environment.NewLine;
            txtLog.Text = txtLog.Text + "recieved from site 2 : " + shippingFileName;

            txtLog.Text = txtLog.Text + Environment.NewLine + Environment.NewLine;
            txtLog.Text = txtLog.Text + "Intersect customer keys with depositor keys";


            List<string> tempSite1 = new List<string>();
            try
            {
                HashSet<Depositor> depList = new HashSet<Depositor>();
                using (StreamReader sr = new StreamReader(shippingFileName))
                {
                    string line;


                    while ((line = sr.ReadLine()) != null)
                    {
                        foreach (Depositor i in depositorTable)
                        {
                            if (line.Equals(i.getKey1()))
                            {
                                tempSite1.Add(i.getKey2() + "," + line);
                                depList.Add(i);
                                break;
                            }
                        }
                    }
                    tempsiteL1 = tempSite1.ToList<string>();
                    lstSite2.DataSource = tempsiteL1;
                }

                string returndFileName = "depositor_NYC_NYC_copy.txt";

                txtLog.Text = txtLog.Text + Environment.NewLine + Environment.NewLine;
                txtLog.Text = txtLog.Text + "Intermediate Result saved in File :";
                txtLog.Text = txtLog.Text + returndFileName;

                try
                {
                    System.IO.File.Delete(returndFileName);
                }
                catch (System.IO.IOException eee)
                { }

                using (System.IO.StreamWriter file = new System.IO.StreamWriter(returndFileName, true))
                {
                    foreach (string i in tempsiteL1)
                    {
                        file.WriteLine(i);
                    }
                }

                txtLog.Text = txtLog.Text + Environment.NewLine + Environment.NewLine;
                txtLog.Text = txtLog.Text + "Joining..." + Environment.NewLine + "depositor_NYC_NYC_copy " + Environment.NewLine + " With Account_NYC";

                blockNestedJoinSameSiteBalance(tempSite1, "account");

                txtLog.Text = txtLog.Text + Environment.NewLine;
                txtLog.Text = txtLog.Text + "Ship Result FROM NYC to HOU";
                txtLog.Text = txtLog.Text + Environment.NewLine;
                txtLog.Text = txtLog.Text + "Temp file : depositor_NYC_HOU_Copy.txt";


                txtLog.Text = txtLog.Text + Environment.NewLine + Environment.NewLine;
                txtLog.Text = txtLog.Text + "Joining..." + Environment.NewLine + "depositor_NYC_HOU_Copy.txt" + Environment.NewLine + " customer_HOU";


                blockNestedJoinSemiJoinPossitiveNegative("depositor_NYC_HOU_Copy.txt", "customer_HOU.txt");

                //txtLog.Text = txtLog.Text + Environment.NewLine + Environment.NewLine;
                //txtLog.Text = txtLog.Text + "recieved back to site 1 : " + returndFileName;
                //blockNestedJoin(table1.table + "_" + table1.site + ".txt", returndFileName);
            }
            catch (Exception eee)
            {
                MessageBox.Show("File not found customer  \n\n" + eee.ToString());
            }
        }



        // Query 3 block nested loop join with semi join results
        public void blockNestedJoinSameSite(string fileName)
        {

            //MessageBox.Show("reciewd :  " + table1 + " and " + table2);
            List<string> tempTable1 = new List<string>();
            List<string> tempTable2 = new List<string>();

            List<string> inner = new List<string>();
            List<string> outer = new List<string>();
            int mainMemorySize = this.mainMemorySize;

            string line;

            try
            {

                foreach (Depositor d in depositorTable)
                {
                    tempTable1.Add(d.getAccNo() + "," + d.getCustName());
                }

                foreach (Account a in accountTable)
                {
                    tempTable2.Add(a.getAccountNumber() + "," + a.getBranchName() + "," + a.getBalance());
                }


                int t1BlockSize = blockSize("depositor", tempTable1.Count);
                int t2BlockSize = blockSize("account", tempTable2.Count);

                int innerPerBlock = perBlock("depositor");
                int outerPerBlock = perBlock("account");

                MessageBox.Show("Relation 1 has " + t1BlockSize + " blocks AND  Relation 2 has " + t2BlockSize + " blocks");


                bool changed = false;

                if (t1BlockSize < mainMemorySize && t2BlockSize >= mainMemorySize)
                {
                    // then table1 is Inner relation and Table2 is Outer
                    inner = tempTable1;
                    outer = tempTable2;
                }
                else if (t2BlockSize < mainMemorySize && t1BlockSize >= mainMemorySize)
                {
                    // then table2 is Inner relation and Table1 is Outer
                    changed = true;
                    inner = tempTable2;
                    outer = tempTable1;

                    int temp = t1BlockSize;
                    t1BlockSize = t2BlockSize;
                    t2BlockSize = temp;

                    temp = innerPerBlock;
                    innerPerBlock = outerPerBlock;
                    outerPerBlock = temp;

                }
                else if (t1BlockSize < mainMemorySize && t2BlockSize < mainMemorySize)
                {
                    // of both the relations are fit in main memory
                    if (t1BlockSize <= t2BlockSize)
                    {
                        inner = tempTable1;
                        outer = tempTable2;
                    }
                    else
                    {
                        changed = true;

                        inner = tempTable2;
                        outer = tempTable1;
                        int temp = t1BlockSize;
                        t1BlockSize = t2BlockSize;
                        t2BlockSize = temp;

                        temp = innerPerBlock;
                        innerPerBlock = outerPerBlock;
                        outerPerBlock = temp;
                    }
                }
                else
                {
                    /* if both relations are contain blocks more than main memory
                       then smaller relation is used as Outer relation
                       I/O complexity is O(m/kB * (n/B + k))  */

                    if (t1BlockSize < t2BlockSize)
                    {
                        changed = true;

                        inner = tempTable2;
                        outer = tempTable1;
                        int temp = t1BlockSize;
                        t1BlockSize = t2BlockSize;
                        t2BlockSize = temp;

                        temp = innerPerBlock;
                        innerPerBlock = outerPerBlock;
                        outerPerBlock = temp;
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

                for (int i = 0; i < outer.Count(); i = i + (outerPerBlock * (mainMemorySize - 1)))
                {
                    tempBlock1.Clear();
                    lstOuter.Items.Add("-------Records " + "from " + i + " to " + (i + outerPerBlock * (mainMemorySize - 1)) + "------");
                    lstOuter.Items.Add(" ");
                    foreach (var ss1 in outer.Skip(i).Take(outerPerBlock * (mainMemorySize - 1)))
                    {
                        lstOuter.Items.Add("" + ss1);
                        tempBlock1.Add("" + ss1);
                    }
                    lstOuter.Items.Add("___________________________");

                    for (int j = 0; j < inner.Count(); j = j + innerPerBlock)
                    {
                        tempBlock2.Clear();
                        lstInner.Items.Add("-------Records " + "from " + j + " to " + (j + innerPerBlock) + "------");
                        lstInner.Items.Add(" ");
                        foreach (var ss2 in inner.Skip(j).Take(innerPerBlock))
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
                                        if (changed)
                                        {
                                            duplMap.Add(jj.Split(',')[0]);
                                            lstOutBuffer.Items.Add(jj);
                                            outBuffer.Add(jj);
                                        }
                                        else
                                        {
                                            duplMap.Add(ii.Split(',')[0]);
                                            lstOutBuffer.Items.Add(ii);
                                            outBuffer.Add(ii);
                                        }
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
                                    if (ii.Split(',')[0].Equals("A85486371"))
                                    {
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


        // Query 2 block nested loop join (China town branch)
        public void blockNestedJoinAccount(string table1, string table2)
        {

            //MessageBox.Show("reciewd :  " + table1 + " and " + table2);
            List<string> tempTable1 = new List<string>();
            List<string> tempTable2 = new List<string>();

            List<string> inner = new List<string>();
            List<string> outer = new List<string>();
            int mainMemorySize = this.mainMemorySize;

            string line;

            try
            {
                //MessageBox.Show("line 1 :" + table1);
                using (StreamReader sr = new StreamReader(table1))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        tempTable1.Add(line);
                    }
                }

                //MessageBox.Show("line 2 :" + table2);
                using (StreamReader sr = new StreamReader(table2))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        tempTable2.Add(line);
                    }
                }

                string[] t1 = table1.Split('_');
                string[] t2 = table2.Split('_');

                int t1BlockSize = blockSize(t1[0], tempTable1.Count);
                int t2BlockSize = blockSize(t2[0], tempTable2.Count);

                int innerPerBlock = perBlock(t1[0]);
                int outerPerBlock = perBlock(t2[0]);

                MessageBox.Show("Relation 1 has " + t1BlockSize + " blocks AND  Relation 2 has " + t2BlockSize + " blocks");

                bool changed = false;

                if (t1BlockSize < mainMemorySize && t2BlockSize >= mainMemorySize)
                {
                    // then table1 is Inner relation and Table2 is Outer
                    inner = tempTable1;
                    outer = tempTable2;
                }
                else if (t2BlockSize < mainMemorySize && t1BlockSize >= mainMemorySize)
                {
                    // then table2 is Inner relation and Table1 is Outer
                    changed = true;
                    inner = tempTable2;
                    outer = tempTable1;

                    int temp = t1BlockSize;
                    t1BlockSize = t2BlockSize;
                    t2BlockSize = temp;

                    temp = innerPerBlock;
                    innerPerBlock = outerPerBlock;
                    outerPerBlock = temp;

                }
                else if (t1BlockSize < mainMemorySize && t2BlockSize < mainMemorySize)
                {
                    // of both the relations are fit in main memory
                    if (t1BlockSize <= t2BlockSize)
                    {
                        inner = tempTable1;
                        outer = tempTable2;
                    }
                    else
                    {
                        changed = true;

                        inner = tempTable2;
                        outer = tempTable1;
                        int temp = t1BlockSize;
                        t1BlockSize = t2BlockSize;
                        t2BlockSize = temp;

                        temp = innerPerBlock;
                        innerPerBlock = outerPerBlock;
                        outerPerBlock = temp;
                    }
                }
                else
                {
                    /* if both relations are contain blocks more than main memory
                       then smaller relation is used as Outer relation
                       I/O complexity is O(m/kB * (n/B + k))  */

                    if (t1BlockSize < t2BlockSize)
                    {
                        changed = true;

                        inner = tempTable2;
                        outer = tempTable1;
                        int temp = t1BlockSize;
                        t1BlockSize = t2BlockSize;
                        t2BlockSize = temp;

                        temp = innerPerBlock;
                        innerPerBlock = outerPerBlock;
                        outerPerBlock = temp;
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

                for (int i = 0; i < outer.Count(); i = i + (outerPerBlock * (mainMemorySize - 1)))
                {
                    tempBlock1.Clear();
                    lstOuter.Items.Add("-------Records " + "from " + i + " to " + (i + outerPerBlock * (mainMemorySize - 1)) + "------");
                    lstOuter.Items.Add(" ");
                    foreach (var ss1 in outer.Skip(i).Take(outerPerBlock * (mainMemorySize - 1)))
                    {
                        lstOuter.Items.Add("" + ss1);
                        tempBlock1.Add("" + ss1);
                    }
                    lstOuter.Items.Add("___________________________");

                    for (int j = 0; j < inner.Count(); j = j + innerPerBlock)
                    {
                        tempBlock2.Clear();
                        lstInner.Items.Add("-------Records " + "from " + j + " to " + (j + innerPerBlock) + "------");
                        lstInner.Items.Add(" ");
                        foreach (var ss2 in inner.Skip(j).Take(innerPerBlock))
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
                                        //MessageBox.Show("ii ==" + ii.Split(',').Length + "   jj " +jj.Split(','));
                                        if (ii.Split(',').Length == 2)
                                        {
                                            duplMap.Add(jj.Split(',')[0]);
                                            lstOutBuffer.Items.Add(ii.Split(',')[1] + "," + jj.Split(',')[2]);
                                            outBuffer.Add(ii.Split(',')[1] + "," + jj.Split(',')[2]);
                                        }
                                        else
                                        {
                                            duplMap.Add(ii.Split(',')[0]);
                                            lstOutBuffer.Items.Add(jj.Split(',')[1] + "," + ii.Split(',')[2]);
                                            outBuffer.Add(jj.Split(',')[1] + "," + ii.Split(',')[2]);
                                        }
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


        // Query 1 block nested loop join
        public void blockNestedJoin(string table1, string table2) {

            clearGrid();
            try{
                lstInner.Items.Clear();
            }catch (Exception ee1){
                lstInner.DataSource = null;
            }
            try{
                lstOuter.Items.Clear();
            }catch (Exception ee2){
                lstOuter.DataSource = null;
            }

            List<string> tempTable1 = new List<string>();
            List<string> tempTable2 = new List<string>();

            List<string> inner = new List<string>();
            List<string> outer = new List<string>();

            int mainMemorySize = this.mainMemorySize;

            string line;

            try
            {
                // Read semi join results or relations in to a data structure
                using (StreamReader sr = new StreamReader(table1))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        tempTable1.Add(line);
                    }
                }

                // Read semi join results or relations in to a data structure
                using (StreamReader sr = new StreamReader(table2))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        tempTable2.Add(line);
                    }
                }

                string[] t1 = table1.Split('_');    // Ex: Customer_HOU
                string[] t2 = table2.Split('_');    // Ex: Depositor_NYC_HOU_Copy


                int innerPerBlock = perBlock(t1[0]);        // Number of tupples per block in relation 1
                int outerPerBlock = perBlock(t2[0]);        // Number of tupples per block in relation 2

                int t1BlockSize = blockSize(t1[0], tempTable1.Count);   // Number of blocks contains in relation 1
                int t2BlockSize = blockSize(t2[0], tempTable2.Count);   // Number of blocks contains in relation 2

                // Display block details
                MessageBox.Show("Relation 1 has " + t1BlockSize + " blocks AND  Relation 2 has " + t2BlockSize +" blocks");
                
                bool changed = false;


                /* Inner and Outer relations selection process

                 If one of the relation is contain blocks less than main memory size
                 Make the small input the inner
                 I/O complexity is O(m/B + n/B)  */

                if (t1BlockSize < mainMemorySize && t2BlockSize >= mainMemorySize)
                {
                    // then table1 is Inner relation and Table2 is Outer
                    inner = tempTable1;
                    outer = tempTable2;
                }
                else if (t2BlockSize < mainMemorySize && t1BlockSize >= mainMemorySize)
                {
                    // then table2 is Inner relation and Table1 is Outer
                    changed = true;
                    inner = tempTable2;
                    outer = tempTable1;

                    int temp = t1BlockSize;
                    t1BlockSize = t2BlockSize;
                    t2BlockSize = temp;

                    temp = innerPerBlock;
                    innerPerBlock = outerPerBlock;
                    outerPerBlock = temp;

                }
                else if (t1BlockSize < mainMemorySize && t2BlockSize < mainMemorySize) {
                    // of both the relations are fit in main memory
                    if (t1BlockSize <= t2BlockSize){
                        inner = tempTable1;
                        outer = tempTable2;
                    }
                    else {
                        changed = true;

                        inner = tempTable2;
                        outer = tempTable1;
                        int temp = t1BlockSize;
                        t1BlockSize = t2BlockSize;
                        t2BlockSize = temp;

                        temp = innerPerBlock;
                        innerPerBlock = outerPerBlock;
                        outerPerBlock = temp;
                    }
                }
                else
                {
                    /* if both relations are contain blocks more than main memory
                       then smaller relation is used as Outer relation
                       I/O complexity is O(m/kB * (n/B + k))  */

                    if (t1BlockSize < t2BlockSize)
                    {
                        changed = true;

                        inner = tempTable2;
                        outer = tempTable1;
                        int temp = t1BlockSize;
                        t1BlockSize = t2BlockSize;
                        t2BlockSize = temp;

                        temp = innerPerBlock;
                        innerPerBlock = outerPerBlock;
                        outerPerBlock = temp;
                    }
                    else
                    {
                        inner = tempTable1;
                        outer = tempTable2;
                    }
                }

                // for testing write inner and outer relations to a file
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

                // Grig view colimn assignment
                DataTable table = new DataTable();
                table.Columns.Add("Customer_name", typeof(string));
                table.Columns.Add("Street", typeof(string));
                table.Columns.Add("City", typeof(string));


                MessageBox.Show("Inner block size : " + t1BlockSize + "       Outer block :" + t2BlockSize);

                // Load k pages of outer into memory at a time  (k = M - 1)
                for (int i = 0; i < outer.Count(); i = i + (outerPerBlock * (mainMemorySize - 1)))
                {
                    tempBlock1.Clear();
                    lstOuter.Items.Add("-------Records " + "from " + i + " to " + (i + outerPerBlock * (mainMemorySize - 1)) + "------");
                    lstOuter.Items.Add(" ");
                    foreach (var ss1 in outer.Skip(i).Take(outerPerBlock * (mainMemorySize - 1)))
                    {
                        lstOuter.Items.Add("" + ss1);
                        tempBlock1.Add("" + ss1);
                    }
                    lstOuter.Items.Add("___________________________");
                    

                    //  Allocate 1 page for inner
                    for (int j = 0; j < inner.Count(); j = j + innerPerBlock)
                    {
                        tempBlock2.Clear();
                        lstInner.Items.Add("-------Records " + "from " + j + " to " + (j + innerPerBlock) + "------");
                        lstInner.Items.Add(" ");
                        foreach (var ss2 in inner.Skip(j).Take(innerPerBlock))
                        {
                            lstInner.Items.Add("" + ss2);
                            tempBlock2.Add("" + ss2);
                        }
                        lstInner.Items.Add("___________________________");

                        HashSet<string> duplMap = new HashSet<string>();
                        foreach (string ii in tempBlock1)                           // for each outer k blocks
                        {
                            foreach (string jj in tempBlock2)                       // for each inner block
                            {
                                if (ii.Split(',')[0].Equals(jj.Split(',')[0]))      // compare join columns
                                {
                                    if (!duplMap.Contains(ii.Split(',')[0]))
                                    {
                                        if (jj.Split(',').Length == 3)
                                        {
                                            duplMap.Add(jj.Split(',')[0]);          // add to HashSet to remove duplicate results         
                                            lstOutBuffer.Items.Add(jj);             // Add to out buffer View 
                                            outBuffer.Add(jj);                      // Add to out buffer storage (Size 5)
                                        }
                                        else {
                                            duplMap.Add(ii.Split(',')[0]);
                                            lstOutBuffer.Items.Add(ii);
                                            outBuffer.Add(ii);
                                        }
                                        
                                    }
                                    if (lstOutBuffer.Items.Count > 4)                           // if buffer is full
                                    {
                                        MessageBox.Show("Out Buffer is Full : Flushing...");    // Display message

                                        for (int kk = 0; kk < 5 ; kk++)                         // Flush out buffer to output
                                        {
                                            string[] row2 = new string[] { outBuffer[kk].Split(',')[0], outBuffer[kk].Split(',')[1], outBuffer[kk].Split(',')[2] };
                                            table.Rows.Add(row2);
                                            gridOut.DataSource = table;                         // add to out put data grid View
                                            
                                        }
                                        outBuffer.Clear();                                      // Clear the out buffer
                                        lstOutBuffer.Items.Clear();
                                    }
                                }
                            }
                        }
                    }
                    if (outBuffer.Count > 0)                                                    // Rest of the items in out buffer after the final iteration
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
        
        // Write final results to text file (When out buffer overflows)
        public void writeFinalResultsToFile(string query)
        {

            string outFileName = "Results for " + query + " (" + DateTime.Now.ToString("MM-dd-yyyy HH-mm-ss") + ").txt";
            StringBuilder strBuilder = new StringBuilder();
            //MessageBox.Show(outFileName);
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

            txtLog.Text = txtLog.Text + Environment.NewLine + Environment.NewLine +
                                        "Final result write to permanent storage" + Environment.NewLine +
                                        "File name : " + Environment.NewLine + outFileName;
        }

        // Returns the number of tuples per block for given relation
        public int perBlock(string t1)
        {   
            int blkPer = 1;

            if (t1.Equals("customer"))
            {
                blkPer = Customer.tuplesPerBlock;
            }
            else if (t1.Equals("depositor"))
            {
                blkPer = Depositor.tuplesPerBlock;
            }
            else if (t1.Equals("branch"))
            {
                blkPer = Branch.tuplesPerBlock;
            }
            else if (t1.Equals("account"))
            {
                blkPer = Account.tuplesPerBlock;
            }
            return blkPer;
        }

        // returns the number of blocks in current relation 
        public int blockSize(string t1, int tuples)
        {
            int blkSize = 1;
            int blkPer = 1;

            if (t1.Equals("customer"))
            {
                blkSize = Customer.numberOfBlock;
                blkPer = Customer.tuplesPerBlock;
            }
            else if (t1.Equals("depositor"))
            {
                blkSize = Depositor.numberOfBlock;
                blkPer = Depositor.tuplesPerBlock;
            }
            else if (t1.Equals("branch"))
            {
                blkSize = Branch.numberOfBlock;
                blkPer = Branch.tuplesPerBlock;
            }
            else if (t1.Equals("account"))
            {
                blkSize = Account.numberOfBlock;
                blkPer = Account.tuplesPerBlock;
            }
            MessageBox.Show("tuples =" + tuples + "   PerBlock = " + blkPer);
            double numOfBlocks = tuples / blkPer;
            return (int)(numOfBlocks + 1);              //   return (int)Math.Celing(tuples / blkPer);
        }
        



        // semi join process
        public void semiJoin(RelationInfo table1, RelationInfo table2) {

            string shippingFileName = "";
            string returndFileName = "";

            if (query4)
            {               
                List<string> tempsiteL1 = new List<string>();
                twoWaySemijoin(table1, table2, cmbCurrServer.Text);
            }
            else
            {

                // if relation 1 is in same site where the query request is done
                if (table1.site.Equals(cmbCurrServer.Text))
                {
                    // Prepare the file names for semi join transfers
                    shippingFileName = table1.table + "_" + table1.site + "_" + table2.site + "_copy.txt";
                    returndFileName = table2.table + "_" + table2.site + "_" + table1.site + "_copy.txt";

                    try { 
                        System.IO.File.Delete(shippingFileName);
                        System.IO.File.Delete(returndFileName);
                    }
                    catch (System.IO.IOException eee){ }

                    if (table1.table.Equals("customer") && table2.table.Equals("depositor"))
                    {
                        txtLog.Text = txtLog.Text + Environment.NewLine + Environment.NewLine;
                        txtLog.Text = txtLog.Text + "Ship " + table1.table + "_key to site " + table2.site;
                        txtLog.Text = txtLog.Text + Environment.NewLine + "temp table = " + shippingFileName;
                        List<string> tempsiteL1 = new List<string>();


                        // Write customer keys to a file
                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(shippingFileName, true))
                        {
                            foreach (Customer i in customerTable)
                            {
                                file.WriteLine(i.getCustName());
                                lstSite1.Items.Add(i.getCustName());
                            }
                        }

                        MessageBox.Show("Site 1 keys written to a file for ship\n\nFile name ="+ shippingFileName);

                        // Now Site2 recieved the file sent from site1.


                        // updating the log
                        txtLog.Text = txtLog.Text + Environment.NewLine + Environment.NewLine;
                        txtLog.Text = txtLog.Text + "recieved from site 2 : " + shippingFileName;

                        // Read the file with customer keys sent from site1
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
                                        if (line.Equals(i.getKey1()))   // Comparison
                                        {
                                            tempSite1.Add(line);        // Intersection stores in this hash map (No duplicates)
                                            break;
                                        }
                                    }
                                }
                                tempsiteL1 = tempSite1.ToList<string>();
                                lstSite2.DataSource = tempsiteL1;

                            }

                            // Write the intersection set to a file to send back to site1
                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(returndFileName, true))
                            {
                                foreach (string i in tempsiteL1)
                                {
                                    file.WriteLine(i);
                                }
                            }

                            // Now Site1 recieved the file sent from site2.
                            txtLog.Text = txtLog.Text + Environment.NewLine + Environment.NewLine;
                            txtLog.Text = txtLog.Text + "recieved back to site 1 : " + returndFileName;


                            // start the block nested loop join
                            blockNestedJoin(table1.table + "_" + table1.site + ".txt", returndFileName);
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
                        MessageBox.Show("Site 1 keys written to a file for ship\n\nFile name = " + shippingFileName);

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

                        MessageBox.Show("Site 1 keys written to a file for ship\n\nFile name = " + shippingFileName);
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
                        //MessageBox.Show("accounttttttttttttttttttttt = " + table1.site);
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

                        MessageBox.Show("Site 1 keys written to a file for ship\n\nFile name = " + shippingFileName);
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

                        MessageBox.Show("Site 1 keys written to a file for ship\n\nFile name = " + shippingFileName);

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

                            blockNestedJoin(table2.table + "_" + table2.site + ".txt", returndFileName);
                        }
                        catch (Exception eee)
                        {
                            MessageBox.Show("File not found customer  \n\n" + eee.ToString());
                        }

                    }
                    if (table1.table.Equals("depositor") && table2.table.Equals("account")) // working
                    {
                        //MessageBox.Show("Dep acc semi");
                        //MessageBox.Show("table 2 :" + table2.table + "  site 2:" + table2.site);
                        //MessageBox.Show("table 1 :" + table1.table + "  site 1:" + table1.site);
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

                        MessageBox.Show("Site 1 keys written to a file for ship\n\nFile name = " + shippingFileName);

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
                    twoWaySemijoin(table1, table2, cmbCurrServer.Text);
                }
            }
        }

        // Text file reader (Read relations from text files)
        public void readTable(string tabel, string site)
        {

            if (tabel.ToLower().Equals("customer"))
            {
                try
                {
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
                    MessageBox.Show("File not found customer  \n\n" + eee.ToString());
                }
            }
            if (tabel.ToLower().Equals("branch"))
            {
                string fileName = "";
                try
                {
                    fileName = "branch_" + site.ToUpper() + ".txt";

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
        }

        // Read tables in join process order
        public void joinProcess(List<string> fileList) {
            // Identify Relations using text file names
            string[] f_info;
            foreach (string files in fileList)
            {
                f_info = files.Split('_');                
                readTable(f_info[0], f_info[1]);
            }            
        }

        // Identify relations in the query and map them to relation objects
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

        // Where Clause extractor 
        public List<string> conditionCheck(string condition) {

            string[] condList;
            List<string> list = new List<string>();

            // break the Where conditions using AND
            Regex rgx = new Regex("AND");
            string result = rgx.Replace(condition, "~");                
            condList = result.Split('~');
            
            foreach (string i in condList)
            {
                string[] ss = i.Split(' ');
                if (ss[1].Contains("account.balance")) {                  
                    double j;
                    query4 = true;
                    if (Double.TryParse(ss[3].ToString(), out j)) {
                        balanceCondition = j;                       
                    }
                    else {
                        MessageBox.Show("Enterd balance condition is not valid");
                    }                            
                }
                txtLog.Text = txtLog.Text + Environment.NewLine + i +Environment.NewLine;
                list.Add(i);
            }            
            return list;
        }

        // Starting point of the program
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                // retrieve the memory size
                int j=2;
                if (Int32.TryParse(txtMemorySize.Text.ToString(), out j))
                {                     
                    if (j > 3)
                    {
                        mainMemorySize = j;
                        MessageBox.Show("Available Memory size = "+j);
                    }
                    else {
                        MessageBox.Show("Memory size should be Possitive Integer grater than 3");
                        mainMemorySize = 5;
                        txtMemorySize.Text = 5 + "";
                    }
                    
                }
                else {
                    MessageBox.Show("Memory size should be Possitive Integer value");
                    mainMemorySize = 5;
                    txtMemorySize.Text = 5 + "";
                }


                clearSites();
                clearGrid();                
                gridOut.DataSource = null;
                query4 = false;

                // Query check for validation
                string[] fromList = txtxFrom.Text.ToString().Split(',');
                bool fileFlag = true;
                foreach (string i in fromList) {
                    if (!tables.Contains(i.Trim()) || txtWhere.Text == "") {
                        txtLog.Text = txtLog.Text + Environment.NewLine + "Query error. No such a table found = " + i+"\n\nPlease try again  ";
                        txtSelect.Text = "";
                        fileFlag = false;
                    }
                }                

                // if query is valid
                if (fileFlag) {
                    txtLog.Text = txtLog.Text + Environment.NewLine + "query ok = "+getQueryRadiaButtonName() + Environment.NewLine;
                    txtLog.Text = txtLog.Text + Environment.NewLine + "Memory available = " + mainMemorySize +" Blocks"+ Environment.NewLine;

                    // Break the where clouse and retrive the conditions seperately
                    joinProcess(loadTables(fromList, conditionCheck(txtWhere.Text)));

                    // update the log
                    foreach (RelationInfo i in tableSet)
                    {
                        txtLog.Text = txtLog.Text + Environment.NewLine + "Site is = " + i.site + "  and Table is = " + i.table;
                    }

                    // start the join process
                    semiJoin(tableSet[0], tableSet[1]);
                    tableSet.Clear();

                    txtLog.Text = txtLog.Text + Environment.NewLine + Environment.NewLine;
                }              
            }
            catch (Exception err) {
                txtSelect.Text = err.ToString();
            }
        }




        // Clear list boxes which contains semi join results
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
            try
            {
                lstInner.Items.Clear();
            }
            catch (Exception ee1)
            {
                lstInner.DataSource = null;
            }
            try
            {
                lstOuter.Items.Clear();
            }
            catch (Exception ee2)
            {
                lstOuter.DataSource = null;
            }
            tableSet.Clear();            
            lstOutBuffer.Items.Clear();
        }

        // Clear list boxes which contains inner and outer block results
        public void clearGrid() {
            tableSet.Clear();
            gridOut.DataSource = null;
            lstOutBuffer.Items.Clear();
        }

        // Clear button
        private void btnClear_Click(object sender, EventArgs e)
        {
            clearSites();
            clearGrid();
        }

        // Clear the log
        private void btnClearLog_Click(object sender, EventArgs e)
        {
            txtLog.Text = "";
        }

        //Exit button
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // Select the current site
        private void cmbCurrServer_SelectedIndexChanged(object sender, EventArgs e)
        {
            lstOutBuffer.Items.Clear();
            lstInner.Items.Clear();
            lstOuter.Items.Clear();
            gridOut.DataSource = null;
        }

        // Query 1 Radio Button
        private void rdo1_CheckedChanged(object sender, EventArgs e)
        {
            if(rdo1.Checked == true){
                cmbCurrServer.SelectedIndex = 0;
                txtSelect.Text = "cust_name,street,city";
                txtxFrom.Text = "customer,depositor";
                txtWhere.Text = "customer.cust_name = depositor.cust_name";
            }
        }

        // Query 2 Radio Button
        private void rdo2_CheckedChanged(object sender, EventArgs e)
        {
            cmbCurrServer.SelectedIndex = 3;
            txtSelect.Text = "cust_name,balance";
            txtxFrom.Text = "depositor,account";
            txtWhere.Text = "depositor.acc_number = account.acc_number AND account.bran_name = 'Chinatown branch'";
        }

        // Query 3 Radio Button
        private void rdo3_CheckedChanged(object sender, EventArgs e)
        {
            cmbCurrServer.SelectedIndex = 3;
            txtSelect.Text = "cust_name,street,city";
            txtxFrom.Text = "depositor,account,customer";
            txtWhere.Text = "depositor.acc_number = account.acc_number AND depositor.cust_name = customer.cust_name AND account.acc_number = 'A85486371'";
        }

        // Query 4 Radio Button
        private void rdo4_CheckedChanged(object sender, EventArgs e)
        {
            cmbCurrServer.SelectedIndex = 2;
            txtSelect.Text = "cust_name,street,city";
            txtxFrom.Text = "depositor,account,customer";
            txtWhere.Text = "depositor.cust_name = customer.cust_name AND depositor.acc_number = account.acc_number AND account.balance > 500";
        }

        // Retrive current query
        public string getQueryRadiaButtonName()
        {
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

    }
}
