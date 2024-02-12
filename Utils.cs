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

        public static int[] DailySaldoInCents(Database database, int month, int year)
        {
            return DailySaldoInCents(database.Profits, database.Expenses, month, year);
        }

        public static int[] DailySaldoInCents(List<Profit> profits, List<Expense> expenses, int month, int year)
        {
            int daysInMonth = DateTime.DaysInMonth(year, month);
            int[] saldo = [daysInMonth];
            foreach(Profit profit in profits)
            {
                if(profit.PayDay != null && profit.PayDay.Value.Year == year && profit.PayDay.Value.Month == month)
                {
                    saldo[profit.PayDay.Value.Day - 1] += (int)profit.TotalValue;
                }
            }
            foreach (Expense expense in expenses)
            {
                if (expense.PayDay != null && expense.PayDay.Value.Year == year && expense.PayDay.Value.Month == month)
                {
                    saldo[expense.PayDay.Value.Day - 1] -= (int)expense.TotalValue;
                }
            }
            return saldo;
        }
    }
}
