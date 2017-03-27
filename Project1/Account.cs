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
        public static int numberOfBlock = 10;

        public string getAccountNumber() {
            return account_number;
        }
        public string getBranchName()
        {
            return branch_name;
        }
        public string getBalance()
        {
            return balance;
        }

        public string getKey()
        {
            return this.account_number;
        }

        public int numberOfBlocks()
        {
            return numberOfBlock;
        }
    }
}
