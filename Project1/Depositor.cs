﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    class Depositor
    {
        public Depositor(string customer_name,string account_number) {
            this.customer_name = customer_name;
            this.account_number = account_number;
        }

        string customer_name;
        string account_number;
        int numberOfBlock = 7;



        public string getKey1()
        {
            return this.customer_name;
        }
        public string getKey2()
        {
            return this.account_number;
        }

        public int numberOfBlocks()
        {
            return numberOfBlock;
        }
    }
}