using System;
using StatsHelix.Charizard;
using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;
using Fancyauth.Model;
using System.Globalization;
using System.Collections.Generic;

using static StatsHelix.Charizard.HttpResponse;

namespace MumbleLogViewer
{
	[Controller]
	public class LogController : AuthenticatedController
	{
		public class ResultGroup
		{
			public string Title { get; set; }
			public List<ResultAction> Actions { get; set; }

		}

		public class ResultAction
		{
			public string Type { get; set; }
			public string Content { get; set; }
			public string ActorName { get; set; }
			public int ActorId { get; set; }
			public string Time { get; set; }
		}

		public async Task<HttpResponse> GetMessages(string day)
		{
			DateTime Start = DateTime.ParseExact(day, "yyyy-MM-dd", CultureInfo.InvariantCulture).Date;

			DateTime End = Start.AddDays(1);


			using (var context = new FancyContext())
			{
				var logs = await context.Logs.Include(a => a.Who).Where(a => a.When > Start && a.When < End).OrderByDescending(a => a.When).ToArrayAsync();


				var res = GroupMessages(logs);

				return Json(res);
			}
		}

		public async Task<HttpResponse> Search(string query)
		{
			var queries = query.Split(' ').Where(a => a.Trim().Length > 2).Select(a => a.ToLowerInvariant()).ToArray();

			using (var context = new FancyContext())
			{
				var messages = context.Logs.Include(a => a.Who).OfType<LogEntry.ChatMessage>();

				foreach (var q in queries)
				{
					messages = messages.Where(
						a => a.Who.Name.ToLower().Contains(q) || a.Message.ToLower().Contains(q)
					);
				}

				messages = messages.OrderByDescending(a => a.When).Take(30);

				var logs = await messages.ToArrayAsync();

				var res = GroupMessages(logs);

				return Json(res);
			}
		}

		private List<ResultGroup> GroupMessages(IEnumerable<LogEntry> logs)
		{
			List<ResultGroup> res = new List<ResultGroup>();

			var currentDay = DateTime.MinValue;
			ResultGroup currentGroup = null;

			foreach (var message in logs)
			{
				if (message.When.Date != currentDay) {
					currentDay = message.When.Date;

					currentGroup = new ResultGroup
					{
						Title = "Messages of " + currentDay.ToString("dd.MM.yyyy"),
						Actions = new List<ResultAction>(),
					};
					res.Add(currentGroup);

				}

				if (message is LogEntry.ChatMessage)
				{
					currentGroup.Actions.Add(new ResultAction
					{
						Type = "Chat",
						Content = ((LogEntry.ChatMessage)message).Message,
						ActorName = message.Who.Name,
						ActorId = message.Who.Id,
						Time = message.When.ToString("o")

					});
				}
				else
				{
					string type = (message is LogEntry.Connected) ? "Connected" : "Disconnected";

					currentGroup.Actions.Add(new ResultAction
					{
						Type = type,
						ActorName = message.Who.Name,
						ActorId = message.Who.Id,
						Time = message.When.ToString("o")
					});
				}
			}

			return res;
		}
	}
}
