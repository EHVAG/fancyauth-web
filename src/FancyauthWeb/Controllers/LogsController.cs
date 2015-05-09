using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using FancyauthWeb.ViewModel;
using Fancyauth;
using Fancyauth.Model;
using System.Data.Entity;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace FancyauthWeb.Controllers
{
    public class LogsController : Controller
    {
        // GET: /Logs/
        public async Task<IActionResult> Index(LogsModel model)
        {
            using (var context = await FancyContext.Connect())
            {
                var query = context.Logs.Where(x => x.When > model.From && x.When < model.To);
                if (!String.IsNullOrWhiteSpace(model.Filter))
                    query = query.OfType<LogEntry.ChatMessage>().Where(x => x.Message.Contains(model.Filter));
                model.Logs = await query.ToArrayAsync();
            }

            return View(model);
        }
    }
}
