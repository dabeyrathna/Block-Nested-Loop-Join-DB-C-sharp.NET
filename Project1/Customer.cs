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
        public static int numberOfBlock = 8;

        public string getCustName() {
            return this.customer_name;
        }

        public string getStreet()
        {
            return this.customer_street;
        }

        public string getCity()
        {
            return this.customer_city;
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
