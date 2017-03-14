using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    class Customer
    {

        public Customer(string customer_name, string customer_street,string customer_city) {
            this.customer_city = customer_city;
            this.customer_name = customer_name;
            this.customer_street = customer_street;
        }

        string customer_name;
        string customer_street;
        string customer_city;
        int numberOfBlock = 8;

        public string getCustName() {
            return this.customer_name;
        }

        public string getKey()
        {
            return this.customer_name;
        }

        public int numberOfBlocks()
        {
            return numberOfBlock;
        }
    }
}
