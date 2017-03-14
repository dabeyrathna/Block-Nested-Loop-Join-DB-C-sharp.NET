using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    public class RelationInfo
    {
        public RelationInfo(string table,string site) {
            this.site = site;
            this.table = table;
        }

        public string table;
        public string site;
    }
}
