using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpControls.Financial.Models.Contacts;

namespace SharpControls.Financial.Models.Financial
{
    public class Expense : FinancialModel
    {
        public Supplier? Supplier { get; set; }
    }
}
