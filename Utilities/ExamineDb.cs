using ImpListApp.Controllers;
using ImpListApp.Data;
using ImpListApp.Models;

namespace ImpListApp.Utilities
{
    public class ExamineDb
    {
        private readonly ApplicationDbContext _db;
        private readonly EmailSender _emailSender;

        public ExamineDb(ApplicationDbContext db, EmailSender emailSender)
        {
            _db = db;
            _emailSender = emailSender;
        }

        public void ExamineDbAndSendMail()
        {
            //var items = new List<Item>();
            //items = _db.Items.ToList();
            //foreach (var item in items)
            //{
            //    if
            //}
        }
    }
}
