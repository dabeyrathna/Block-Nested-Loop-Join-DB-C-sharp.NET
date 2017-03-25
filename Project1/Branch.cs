using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    class Branch
    {
        public Branch(string branch_name, string branch_city, string assets) {
            this.assets = assets;
            this.branch_city = branch_city;
            this.branch_name = branch_name;
        }

        string branch_name;
        string branch_city;
        string assets;
        public static int numberOfBlock = 7;

        public string getBranchName() {
            return branch_name;
        }

        public string getKey() {
            return this.branch_name;
        }

        public int numberOfBlocks() {
            return numberOfBlock;
        }
    }
}
