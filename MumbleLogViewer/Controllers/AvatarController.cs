using System;
using StatsHelix.Charizard;
using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;

using static StatsHelix.Charizard.HttpResponse;
using Fancyauth.Model;
using System.IO;

namespace MumbleLogViewer
{
	[Controller]
	public class AvatarController : AuthenticatedController
	{
		static byte[] NoImageAvailableImage = GetNoUserImage();
		static HttpResponse NoImageAvailable = Data(GetNoUserImage(), HttpStatus.Ok, ContentType.Custom).SetHeader("Content-Type", "image/png");

		static byte[] GetNoUserImage()
		{
			var assembly = System.Reflection.Assembly.GetExecutingAssembly();
			using (var stream = assembly.GetManifestResourceStream("MumbleLogViewer.none.png"))
			{
				byte[] buffer = new byte[stream.Length];
				int read = stream.Read(buffer, 0, buffer.Length);

				if (read != buffer.Length)
				{
					Console.WriteLine("Couldn't read resource.");
					throw new Exception("Couldn't read resource.");
				}

				return buffer;
			}
		}

		public async Task<HttpResponse> Get(int user)
		{
			using (var context = new FancyContext())
			{
				var texture = await context.Users.Where(a => a.Id == user).Select(a => a.Membership.Texture).SingleAsync();

				if (texture == null || texture.Length == 0)
					return NoImageAvailable;


				return Data(texture, HttpStatus.Ok, ContentType.Custom).SetHeader("Content-Type", "image/png");
			}
		}

		public async Task<HttpResponse> GetByName(string name)
		{
			using (var context = new FancyContext())
			{
				var texture = await context.Users.Where(a => a.Name == name).Select(a => a.Membership.Texture).SingleOrDefaultAsync();

				if (texture == null || texture.Length == 0)
					return NoImageAvailable;

				return Data(texture, HttpStatus.Ok, ContentType.Custom).SetHeader("Content-Type", "image/png");
			}
		}

		public async Task<HttpResponse> GetMyAvatar()
		{
			using (var context = new FancyContext())
			{
				var texture = (await GetCurrentUser()).Membership.Texture;

				if (texture == null || texture.Length == 0)
					return NoImageAvailable;

				return Data(texture, HttpStatus.Ok, ContentType.Custom).SetHeader("Content-Type", "image/png");
			}
		}
	}
}