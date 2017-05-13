using System;
using StatsHelix.Charizard;
using System.Net;
using System.Reflection;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MumbleLogViewer
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			IPAddress bindIp;
			ushort port;

			if (args.Length != 2)
			{
				Console.WriteLine("MumbleLogViewer.exe <bindIp> <port>");
				return;
			}

			if (!IPAddress.TryParse(args[0], out bindIp))
			{
				Console.WriteLine("Cannot parse IP");
				return;
			}

			if (!ushort.TryParse(args[1], out port))
			{
				Console.WriteLine("Cannot parse port");
				return;
			}

			Console.WriteLine("Initializing Model...");
			using(var context = new FancyContext())
				Console.WriteLine("Number of Users: {0}", context.Users.Count());
			Console.WriteLine("Finished!");

			Newtonsoft.Json.JsonConvert.DefaultSettings = () => new JsonSerializerSettings { Converters = new List<JsonConverter> { new LongStringJsonConverter() } };


			var server = new HttpServer(new IPEndPoint(bindIp, port), Assembly.GetAssembly(typeof(MainClass)));
			server.Run().Wait();
		}
	}
}
