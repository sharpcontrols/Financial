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
        /// <summary>
        /// Returns a list of profits that would be overdue within X days
        /// </summary>
        /// <param name="database">The database the profits should be fetched from</param>
        /// <param name="days">The days from today where the profits should be counted</param>
        /// <returns>A list of profits that will be overdue</returns>
        public static List<Profit> OverdueProfitsWithinDays(Database database, int days)
        {
            return OverdueProfitsWithinDays(database.Profits, days);
        }

        /// <summary>
        /// Returns a list of profits that would be overdue within X days
        /// </summary>
        /// <param name="profits">A list of profits that should be fetched</param>
        /// <param name="days">The days from today where the profits should be counted</param>
        /// <returns>A list of profits that will be overdue</returns>
        public static List<Profit> OverdueProfitsWithinDays(List<Profit> profits, int days)
        {
            DateTime dueDate = DateTime.Now.AddDays(days);
            return profits.Where(p => p.DueDate >= DateTime.Now && p.DueDate <= dueDate && p.PayDay == null).ToList();
        }

        /// <summary>
        /// Returns a list of expenses that would be overdue within X days
        /// </summary>
        /// <param name="database">The database the expenses should be fetched from</param>
        /// <param name="days">The days from today where the expenses should be counted</param>
        /// <returns>A list of expenses that will be overdue</returns>
        public static List<Expense> OverdueExpensesWithinDays(Database database, int days)
        {
            return OverdueExpensesWithinDays(database.Expenses, days);
        }

        /// <summary>
        /// Returns a list of expenses that would be overdue within X days
        /// </summary>
        /// <param name="expenses">A list of expenses that should be fetched</param>
        /// <param name="days">The days from today where the expenses should be counted</param>
        /// <returns>A list of expenses that will be overdue</returns>
        public static List<Expense> OverdueExpensesWithinDays(List<Expense> expenses, int days)
        {
            DateTime dueDate = DateTime.Now.AddDays(days);
            return expenses.Where(e => e.DueDate >= DateTime.Now && e.DueDate <= dueDate && e.PayDay == null).ToList();
        }

        /// <summary>
        /// Returns the saldo in cents from a specific database
        /// </summary>
        /// <param name="database">The database the informations should be fetched from</param>
        /// <param name="planned">Should planned/unpaid models be included?</param>
        /// <returns>The saldo in cents</returns>
        public static int SaldoInCents(Database database, bool planned = false)
        {
            return SaldoInCents(database.Profits, database.Expenses, planned);
        }

        /// <summary>
        /// Returns the saldo in cents from a list of profits and expenses
        /// </summary>
        /// <param name="profits">A list of profits</param>
        /// <param name="expenses">A list of expenses</param>
        /// <param name="planned">Should planned/unpaid models be included?</param>
        /// <returns>The saldo in cents</returns>
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

        /// <summary>
        /// Returns the saldo in cents from a specific database and within a certain month of a certain year
        /// </summary>
        /// <param name="database">The database the informations should be fetched from</param>
        /// <param name="month">The month the informations should be fetched from</param>
        /// <param name="year">The year the informations should be fetched from</param>
        /// <param name="planned">Should planned/unpaid models be included?</param>
        /// <returns>The saldo in cents</returns>
        public static int SaldoInCents(Database database, int month, int year, bool planned = false)
        {
            return SaldoInCents(database.Profits, database.Expenses, month, year, planned);
        }

        /// <summary>
        /// Returns the saldo in cents from a list of profits and expenses and within a certain month of a certain year
        /// </summary>
        /// <param name="profits">The list of profits</param>
        /// <param name="expenses">The list of expenses</param>
        /// <param name="month">The month the content should be fetched from</param>
        /// <param name="year">The year the content should be fetched from</param>
        /// <param name="planned">Should planned/unpaid models be included?</param>
        /// <returns>The saldo in cents</returns>
        public static int SaldoInCents(List<Profit> profits, List<Expense> expenses, int month, int year, bool planned = false)
        {
            int saldo = 0;
            foreach (Profit profit in profits)
            {
                if (profit.PayDay == null && !planned)
                    continue;

                if (profit.PayDay != null && profit.PayDay.Value.Year == year && profit.PayDay.Value.Month == month)
                {
                    saldo += (int)profit.TotalValue;
                }
                else if (profit.PayDay == null && profit.DueDate.Year == year && profit.DueDate.Month == month)
                {
                    saldo += (int)profit.TotalValue;
                }
            }
            foreach (Expense expense in expenses)
            {
                if (expense.PayDay == null && !planned)
                    continue;
                if (expense.PayDay != null && expense.PayDay.Value.Year == year && expense.PayDay.Value.Month == month)
                {
                    saldo -= (int)expense.TotalValue;
                }
                else if (expense.PayDay == null && expense.DueDate.Year == year && expense.DueDate.Month == month)
                {
                    saldo -= (int)expense.TotalValue;
                }
            }
            return saldo;
        }

        /// <summary>
        /// Returns the daily saldo in cents (Day 1 will be index 0!)
        /// </summary>
        /// <param name="database">The database to fetch the data from</param>
        /// <param name="month">The month to fetch from</param>
        /// <param name="year">The year to fetch from</param>
        /// <param name="planned">Should planned/unpaid models be included?</param>
        /// <returns>The daily saldo as integer array</returns>
        public static int[] DailySaldoInCents(Database database, int month, int year, bool planned = false)
        {
            return DailySaldoInCents(database.Profits, database.Expenses, month, year, planned);
        }

        /// <summary>
        /// Returns the daily saldo in cents (Day 1 will be index 0!)
        /// </summary>
        /// <param name="profits">The list of profits to fetch from</param>
        /// <param name="expenses">The list of expenses to fetch from</param>
        /// <param name="month">The month to fetch from</param>
        /// <param name="year">The year to fetch from</param>
        /// <param name="planned">Should planned/unpaid models be included?</param>
        /// <returns>The daily saldo as integer array</returns>
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
