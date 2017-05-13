using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MumbleLogViewer
{
	public class LongStringJsonConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(long) || objectType == typeof(long?);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (!CanConvert(objectType))
				return serializer.Deserialize(reader, objectType);

			long buf;
			var str = reader.Value as string;
			if (str != null && long.TryParse(str, out buf))
				return (objectType == typeof(long?)) ? new long?(buf) : buf;
			else if (objectType == typeof(long?))
				return new long?();
			else
				throw new InvalidOperationException();
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			var val = value as long?;
			if (val.HasValue)
				JToken.FromObject(val.Value.ToString()).WriteTo(writer);
			else
				JToken.FromObject(value).WriteTo(writer);
		}

		public override bool CanRead { get { return true; } }
		public override bool CanWrite { get { return true; } }
	}
}
