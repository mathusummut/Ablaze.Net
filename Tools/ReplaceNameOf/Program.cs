using System;
using System.IO;

namespace ReplaceNameOf {
	public static class Program {
		private static char[] ToIgnore = new char[] { ' ', '\n', '\t', '\r', '(', ')' };

		public static void Main() {
			Console.WriteLine("Press enter to replace all nameof() in all .cs files in this directory and all subdirectories.");
			Console.ReadLine();
			string fileText;
			int index, counter, startIndex;
			bool isString = false;
			foreach (string file in Directory.GetFiles(Environment.CurrentDirectory, "*.cs", SearchOption.AllDirectories)) {
				try {
					fileText = File.ReadAllText(file);
					for (index = 0; index < fileText.Length; index++) {
						if (fileText[index] == '"')
							isString = !isString;
						else if (!isString && index + 7 < fileText.Length && fileText.Substring(index, 7) == "nameof(") {
							index += 7;
							if (index - 8 < 0 || !char.IsLetterOrDigit(fileText[index - 8])) {
								startIndex = index;
								counter = 1;
								do {
									if (fileText[index] == '(')
										counter++;
									else if (fileText[index] == ')') {
										counter--;
										if (counter == 0) {
											fileText = fileText.Substring(0, startIndex - 7) + CalcNameof(fileText.Substring(startIndex, index - startIndex)) + fileText.Substring(index + 1);
											index = startIndex - 8;
											break;
										}
									}
									index++;
								} while (true);
							}
						}
					}
					File.WriteAllText(file, fileText);
					Console.WriteLine(file);
				} catch {
					Console.WriteLine("Could not replace contents of " + file);
				}
			}
		}

		private static string CalcNameof(string expression) {
			expression = expression.Trim(ToIgnore);
			double temp;
			if (!double.TryParse(expression, out temp)) {
				int lastIndex = expression.LastIndexOf('.');
				if (lastIndex != -1)
					expression = expression.Substring(lastIndex + 1);
			}
			return "\"" + expression + "\"";
		}
	}
}
