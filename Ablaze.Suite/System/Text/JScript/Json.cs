using System.IO;

namespace System.Text.JScript {
	internal static class Json {
		public static void Serialize(object obj, JavaScriptSerializer jss, StringBuilder output) {
			JsonSerializer js = new JsonSerializer(jss);
			js.Serialize(obj, output);
			js = null;
		}

		public static void Serialize(object obj, JavaScriptSerializer jss, TextWriter output) {
			JsonSerializer js = new JsonSerializer(jss);
			js.Serialize(obj, output);
			js = null;
		}

		public static object Deserialize(string input, JavaScriptSerializer jss) {
			if (jss == null)
				return null;
			using (StringReader reader = new StringReader(input))
				return Deserialize(reader, jss);
		}

		public static object Deserialize(TextReader input, JavaScriptSerializer jss) {
			if (jss == null)
				return null;

			JsonDeserializer ser = new JsonDeserializer(jss);
			return ser.Deserialize(input);
		}
	}
}