using SharpControls.Financial.Models.Financial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpControls.Financial
{
    public static class Utils
    {
        public static List<Profit> OverdueProfitsWithinDays(Database database, int days)
        {
            return OverdueProfitsWithinDays(database.Profits, days);
        }

        public static List<Profit> OverdueProfitsWithinDays(List<Profit> profits, int days)
        {
            DateTime dueDate = DateTime.Now.AddDays(days);
            return profits.Where(p => p.DueDate >= DateTime.Now && p.DueDate <= dueDate).ToList();
        }

        public static List<Expense> OverdueExpensesWithinDays(Database database, int days)
        {
            return OverdueExpensesWIthinDays(database.Expenses, days);
        }

        public static List<Expense> OverdueExpensesWIthinDays(List<Expense> expenses, int days)
        {
            DateTime dueDate = DateTime.Now.AddDays(days);
            return expenses.Where(e => e.DueDate >= DateTime.Now && e.DueDate <= dueDate).ToList();
        }

        public static int SaldoInCents(Database database)
        {
            return SaldoInCents(database.Profits, database.Expenses);
        }

        public static int SaldoInCents(List<Profit> profits, List<Expense> expenses)
        {
            int saldo = 0;
            foreach(Profit profit in profits)
            {
                saldo += (int)profit.TotalValue;
            }
            foreach(Expense expense in expenses)
            {
                saldo -= (int)expense.TotalValue;
            }
            return saldo;
        }
    }
}
