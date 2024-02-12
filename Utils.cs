using SharpControls.Financial.Models.Financial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
            return profits.Where(p => p.DueDate >= DateTime.Now && p.DueDate <= dueDate && p.PayDay == null).ToList();
        }

        public static List<Expense> OverdueExpensesWithinDays(Database database, int days)
        {
            return OverdueExpensesWithinDays(database.Expenses, days);
        }

        public static List<Expense> OverdueExpensesWithinDays(List<Expense> expenses, int days)
        {
            DateTime dueDate = DateTime.Now.AddDays(days);
            return expenses.Where(e => e.DueDate >= DateTime.Now && e.DueDate <= dueDate && e.PayDay == null).ToList();
        }

        public static int SaldoInCents(Database database, bool planned = false)
        {
            return SaldoInCents(database.Profits, database.Expenses, planned);
        }

        public static int SaldoInCents(List<Profit> profits, List<Expense> expenses, bool planned = false)
        {
            int saldo = 0;
            foreach(Profit profit in profits)
            {
                if (profit.PayDay == null && !planned)
                    continue;
                saldo += (int)profit.TotalValue;
            }
            foreach(Expense expense in expenses)
            {
                if (expense.PayDay == null && !planned)
                    continue;
                saldo -= (int)expense.TotalValue;
            }
            return saldo;
        }

        public static int SaldoInCents(Database database, int month, int year, bool planned = false)
        {
            return SaldoInCents(database.Profits, database.Expenses, month, year);
        }

        public static int SaldoInCents(List<Profit> profits, List<Expense> expenses, int month, int year, bool planned = false)
        {
            int saldo = 0;
            foreach (Profit profit in profits)
            {
                if (profit.PayDay == null && !planned)
                    continue;
                saldo += (int)profit.TotalValue;
            }
            foreach (Expense expense in expenses)
            {
                if (expense.PayDay == null && !planned)
                    continue;
                saldo -= (int)expense.TotalValue;
            }
            return saldo;
        }

        public static int[] DailySaldoInCents(Database database, int month, int year, bool planned = false)
        {
            return DailySaldoInCents(database.Profits, database.Expenses, month, year);
        }

        public static int[] DailySaldoInCents(List<Profit> profits, List<Expense> expenses, int month, int year, bool planned = false)
        {
            int daysInMonth = DateTime.DaysInMonth(year, month);
            int[] saldo = [daysInMonth];
            foreach(Profit profit in profits)
            {
                if(profit.PayDay == null && !planned)
                {
                    continue;
                }
                if(profit.PayDay != null && profit.PayDay.Value.Year == year && profit.PayDay.Value.Month == month)
                {
                    saldo[profit.PayDay.Value.Day - 1] += (int)profit.TotalValue;
                }else if(profit.PayDay == null && profit.DueDate.Year == year && profit.DueDate.Month == month)
                {
                    saldo[profit.DueDate.Day - 1] += (int)profit.TotalValue;
                }
            }
            foreach (Expense expense in expenses)
            {
                if (expense.PayDay == null && !planned)
                {
                    continue;
                }
                if (expense.PayDay != null && expense.PayDay.Value.Year == year && expense.PayDay.Value.Month == month)
                {
                    saldo[expense.PayDay.Value.Day - 1] -= (int)expense.TotalValue;
                }
                else if (expense.PayDay == null && expense.DueDate.Year == year && expense.DueDate.Month == month)
                {
                    saldo[expense.DueDate.Day - 1] -= (int)expense.TotalValue;
                }
            }
            return saldo;
        }
    }
}
