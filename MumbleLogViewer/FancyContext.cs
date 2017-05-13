using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Fancyauth.Model;
using Fancyauth.Model.MusiG;

namespace MumbleLogViewer
{
	public class FancyContext : FancyContextBase
	{
		public FancyContext()
	: base("name=FancyContext")
		{
			this.Configuration.LazyLoadingEnabled = false;

		}

		public static async Task<FancyContext> Connect()
		{
			var context = new FancyContext();
			await context.Database.Connection.OpenAsync();
			return context;
		}
	}
}
