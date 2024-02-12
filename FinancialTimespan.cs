using SharpControls.Financial.Models.Contacts;
using SharpControls.Financial.Models.Financial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SharpControls.Financial
{
    public class FinancialTimespan
    {
        public DateTime From { get; private set; }
        public DateTime To {  get; private set; }
        public List<Profit> Profits { get; private set; } = [];
        public List<Expense> Expenses { get; private set; } = [];
        public bool Planned {  get; private set; }

        public FinancialTimespan(Database database, DateTime from, DateTime to, bool planned)
        {
            From = from;
            To = to;
            Planned = planned;
            LoadData(database.Profits, database.Expenses);
        }

        public FinancialTimespan(List<Profit> profits, List<Expense> expenses, DateTime from, DateTime to, bool planned)
        {
            From = from;
            To = to;
            Planned = planned;
            LoadData(profits, expenses);
        }

        private void LoadData(List<Profit> profits, List<Expense> expenses)
        {
            //Clear lists just in case
            Profits.Clear();
            Expenses.Clear();

            if (Planned)
            {
                Profits = profits.Where(p => p.PayDay == null ? p.DueDate >= From && p.DueDate <= To : p.PayDay >= From && p.PayDay <= To).ToList();
                Expenses = expenses.Where(e => e.PayDay == null ? e.DueDate >= From && e.DueDate <= To : e.PayDay >= From && e.PayDay <= To).ToList();
            }
            else
            {
                Profits = profits.Where(p => p.PayDay >= From && p.PayDay <= To).ToList();
                Expenses = expenses.Where(e => e.PayDay >= From && e.PayDay <= To).ToList();
            }
        }

        /// <summary>
        /// Returns the saldo
        /// </summary>
        /// <returns></returns>
        public int SaldoInCents()
        {
            return Utils.SaldoInCents(Profits, Expenses, Planned);
        }

        /// <summary>
        /// Returns the saldo of all profits
        /// </summary>
        /// <returns></returns>
        public int SaldoProfitsInCents()
        {
            return Utils.SaldoInCents(Profits, [], Planned);
        }

        /// <summary>
        /// Returns the saldo of all expenses (Will be negative!)
        /// </summary>
        /// <returns></returns>
        public int SaldoExpensesInCents()
        {
            return Utils.SaldoInCents([], Expenses, Planned);
        }

        /// <summary>
        /// Returns a list of all overdue profits
        /// </summary>
        /// <returns></returns>
        public List<Profit> OverdueProfits()
        {
            List<Profit> overdues = [];
            foreach(Profit profit in Profits)
            {
                if(profit.DueDate <= From && profit.DueDate >= To && profit.PayDay == null)
                {
                    overdues.Add(profit);
                }
            }
            return overdues;
        }

        /// <summary>
        /// Returns a list of all overdue expenses
        /// </summary>
        /// <returns></returns>
        public List<Expense> OverdueExpenses()
        {
            List<Expense> overdues = [];
            foreach (Expense expense in Expenses)
            {
                if (expense.DueDate <= From && expense.DueDate >= To && expense.PayDay == null)
                {
                    overdues.Add(expense);
                }
            }
            return overdues;
        }

        /// <summary>
        /// Returns a list of all clients with overdue profits
        /// </summary>
        /// <returns></returns>
        public List<Client> OverdueClients()
        {
            List<Client> overdues = [];
            foreach(Profit profit in OverdueProfits())
            {
                if (profit.Client != null && overdues.Contains(profit.Client))
                    overdues.Add(profit.Client);
            }
            return overdues;
        }

        /// <summary>
        /// Returns a list of all suppliers with overdue expenses
        /// </summary>
        /// <returns></returns>
        public List<Supplier> OverdueSuppliers()
        {
            List<Supplier> overdues = [];
            foreach (Expense expense in OverdueExpenses())
            {
                if (expense.Supplier != null && overdues.Contains(expense.Supplier))
                    overdues.Add(expense.Supplier);
            }
            return overdues;
        }
    }
}
