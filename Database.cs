using SharpControls.Financial.Models.Contacts;
using SharpControls.Financial.Models.Financial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpControls.Financial
{
    public class Database
    {
        public List<Client> Clients { get; set; } = [];
        public List<Supplier> Suppliers { get; set; } = [];
        public List<Profit> Profits { get; set; } = [];
        public List<Expense> Expenses { get; set; } = [];

        public Database() { }
        public Database(Client[] clients, Supplier[] suppliers, Profit[] profits, Expense[] expenses)
        {
            Clients.AddRange(clients);
            Suppliers.AddRange(suppliers);
            Profits.AddRange(profits);
            Expenses.AddRange(expenses);
        }
        
        public List<Profit> OverdueProfits(DateTime dateTime)
        {
            return Profits.Where(p => p.DueDate < dateTime).ToList();
        }

        public List<Expense> OverdueExpenses(DateTime dateTime)
        {
            return Expenses.Where(e => e.DueDate < dateTime).ToList();
        }

        public List<Profit> ProfitsDueWithin(DateTime from, DateTime to)
        {
            return Profits.Where(p => p.DueDate >= from && p.DueDate <= to).ToList();
        }

        public List<Expense> ExpensesDueWithin(DateTime from, DateTime to)
        {
            return Expenses.Where(e => e.DueDate >= from &&  e.DueDate <= to).ToList();
        }

        public List<Profit> ProfitsPaidWithin(DateTime from, DateTime to)
        {
            return Profits.Where(p => p.PayDay >= from && p.PayDay <= to).ToList();
        }

        public List<Expense> ExpensesPaidWithin(DateTime from, DateTime to)
        {
            return Expenses.Where(e => e.PayDay >= from && e.PayDay <= to).ToList();
        }
    }
}
