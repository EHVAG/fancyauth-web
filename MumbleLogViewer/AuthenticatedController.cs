using System;
using System.Threading.Tasks;
using StatsHelix.Charizard;
using System.Linq;
using System.Data.Entity;
using Fancyauth.Model;

namespace MumbleLogViewer
{
	public class AuthenticatedController
	{
		public string UserFingerprint { get; private set; }

		[Middleware]
		public Task<HttpResponse> ValidateAuth(HttpRequest request)
		{
			UserFingerprint = request.Headers.Single(a => a.Name == "ehvag-user").Value.ToUpperInvariant();

			return null;
		}

		public async Task<User> GetCurrentUser()
		{
			using (var context = new FancyContext())
			{
				var query = context.Users
				                   .Include(a => a.CertCredentials)
				                   .Include(a => a.Membership)
				                   .Where(a => a.CertCredentials.Fingerprint.ToUpper() == UserFingerprint);
				var user = await query.SingleAsync();
				return user;
			}
		}


	}
}