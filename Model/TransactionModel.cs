using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectCSharp1.Model
{
    class TransactionModel : ProductModel
    {
        public int transID { get; set; }
        public string payment { get; set; }
        public int soldQty { get; set; }
    }
}
