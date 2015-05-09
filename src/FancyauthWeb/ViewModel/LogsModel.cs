using Fancyauth.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FancyauthWeb.ViewModel
{
    public class LogsModel
    {
        public DateTime From { get; set; } = DateTime.UtcNow.AddDays(-1);
        public DateTime To { get; set; } = DateTime.UtcNow;
        public string Filter { get; set; } = null;

        public IEnumerable<LogEntry> Logs { get; set; }
    }
}
