using Hangfire;
using Hangfire.Server;
using ImpListApp.Data;
using ImpListApp.Models;
using ImpListApp.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Diagnostics;

namespace ImpListApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly EmailSender _emailSender;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ApplicationDbContext db, EmailSender emailSender, ILogger<HomeController> logger)
        {
            _db = db;
            _emailSender = emailSender;
            _logger = logger;
        }

        public IActionResult Index()
        {
            //BackgroundJob.Schedule(() => Index(), TimeSpan.FromMinutes(1));
            // Calculate the time remaining until the next day
            var now = DateTime.Now;            
            var tomorrow = now.AddDays(1).Date; // Set the time to 00:00:00 of the next day
            var timeUntilTomorrow = tomorrow - now;

            ViewBag.RefreshNow = now.ToString();
            ViewBag.RefreshNext = now.AddMilliseconds(43200000).ToString();

            var items = new List<Item>();
            items = _db.Items.ToList();
            foreach(Item item in items)
            {
                DateTime itemDateTime = new DateTime(item.EndDate.Year, item.EndDate.Month, item.EndDate.Day);
                 TimeSpan calculateDate = itemDateTime - now;

                if(calculateDate.Days > 0 && calculateDate.Days <= 7)
                {
                    _emailSender.SendEmailAsync("macintecmarcin@gmail.com", "Item is close to be deprecated", $"Item name: {item.Name}\n\nEnd Date: {item.EndDate}");
                }
                if(calculateDate.Days <= 0)
                {
                    _emailSender.SendEmailAsync("macintecmarcin@gmail.com", "ITEM IS DEPRECATED", $"Item: {item.Name} has a deprecated date: {item.EndDate}");
                }

            }
            ViewBag.items = items;
            //_emailSender.SendEmailAsync("macintecmarcin@gmail.com", "Index page was refreshed", $"PAGE WAS REFRESHED {now.ToString()}");
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult AddItem()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddItem(Item item)
        {
            _db.Add(item);
            _db.SaveChanges();
            _emailSender.SendEmailAsync("macintecmarcin@gmail.com", "NEW ITEM", $"Added new item:\n\nItem Name: {item.Name}\n\nItem End Date: {item.EndDate}");
            return Redirect("Index");
        }

        public IActionResult UpdateItem(int id)
        {
            Item item = new Item();
            item = _db.Items.First(x => x.Id == id);
            return View(item);
        }

        [HttpPost]
        public IActionResult UpdateItem(Item item)
        {
            _db.Items.Update(item);
            _db.SaveChanges();
            _emailSender.SendEmailAsync("macintecmarcin@gmail.com", "ITEM UPDATED", $"Updated item:\n\nItem Name: {item.Name}\n\nItem End Date: {item.EndDate}");
            return RedirectToAction("Index");
        }

        public IActionResult DeleteItem(int id)
        {
            Item item = new Item();
            item = _db.Items.First(x => x.Id == id);
            return View(item);
        }

        [HttpPost, ActionName("DeleteItem")]
        public IActionResult DeleteItemPOST(int id)
        {
            var itemToDel = _db.Items.Where(x => x.Id == id).First();
            _db.Items.Remove(itemToDel);
            _db.SaveChanges();
            _emailSender.SendEmailAsync("macintecmarcin@gmail.com", "ITEM DELETED", $"Deleted Item:\n\nItem Name: {itemToDel.Name}\n\nItem End Date: {itemToDel.EndDate}");
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
