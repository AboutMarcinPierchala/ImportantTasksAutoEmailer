using System;
using System.Threading;
using ImpListApp.Data;
using ImpListApp.Models;
using ImpListApp.Utilities;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Scheduler started");

        // Interval set to 2 hours (in milliseconds)
        TimeSpan interval = TimeSpan.FromSeconds(10);

        while (true)
        {
            // Get the current time
            DateTime nextRunTime = DateTime.Now.Add(interval);

            Console.WriteLine($"Next job will be at: {nextRunTime}");

            // Perform the scheduled task
            PerformScheduledTask();

            // Sleep until the next run time
            TimeSpan sleepTime = nextRunTime - DateTime.Now;
            if (sleepTime.TotalMilliseconds > 0)
            {
                Thread.Sleep(sleepTime);
            }
        }
    }

    static void PerformScheduledTask()
    {
        using(var db = new ApplicationDbContext())
        {
            //var _emailSender = new EmailSender();
            DateTime now = DateTime.Now;
            var items = new List<Item>();
            items = db.Items.ToList();
            foreach (Item item in items)
            {
                //using (var _emailSender = new EmailSender())
                //{

                //}
                
                DateTime itemDateTime = new DateTime(item.EndDate.Year, item.EndDate.Month, item.EndDate.Day);
                TimeSpan calculateDate = itemDateTime - now;

                if (calculateDate.Days > 0 && calculateDate.Days <= 7)
                {
                    //_emailSender.SendEmailAsync("macintecmarcin@gmail.com", "Item is close to be deprecated", $"Item name: {item.Name}\n\nEnd Date: {item.EndDate}");
                    Console.WriteLine($"Task {item.Name} będzie przeterminowany do dnia: {item.EndDate}");
                }
                if (calculateDate.Days <= 0)
                {
                    //_emailSender.SendEmailAsync("macintecmarcin@gmail.com", "ITEM IS DEPRECATED", $"Item: {item.Name} has a deprecated date: {item.EndDate}");
                    Console.WriteLine($"Task {item.Name} został przeterminowany z dniem: {item.EndDate}");
                }

            }
        }
        // Write a message to the console
        Console.WriteLine($"Task executed at: {DateTime.Now}. Hello, this is your scheduled task!");
    }
}