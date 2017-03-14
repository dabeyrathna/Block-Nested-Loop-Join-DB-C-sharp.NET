using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    class Account
    {
        public Account(string account_number,string branch_name,string balance) {
            this.account_number = account_number;
            this.branch_name = branch_name;
            this.balance = balance;
        }

        string account_number;
        string branch_name;
        string balance;
        int numberOfBlock = 10;

        public string getAccountName() {
            return account_number;
        }

        public string getKey()
        {
            return this.branch_name;
        }

        public int numberOfBlocks()
        {
            return numberOfBlock;
        }
    }
}
