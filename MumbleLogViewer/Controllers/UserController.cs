using System;
using StatsHelix.Charizard;
using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;

using static StatsHelix.Charizard.HttpResponse;

namespace MumbleLogViewer
{
	[Controller]
	public class UserController : AuthenticatedController
	{
		public async Task<HttpResponse> Me()
		{
			var user = await GetCurrentUser();
			user.Membership.Texture = null;
			return Json(user);
		}
	}
}
