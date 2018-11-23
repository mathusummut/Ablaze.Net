using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace Ablaze.Convert {
	public enum Target {
		Net20 = 1,
		Net30,
		Net35,
		Net35Client,
		Net40,
		Net40Client,
		Net45,
		Net451,
		Net452,
		Net46,
		Net461,
		Net462,
		Net47,
		Net471,
		UnityNet35
	}

	public static class Program {
		public static int TargetCount = Enum.GetNames(typeof(Target)).Length;
		private delegate bool ProcessValueDelegate(XmlNode node, string param, object tag);
		private static string SolutionRoot;

		public static void Main() {
			do {
				Console.WriteLine("Please enter path to Ablaze solution directory (the folder that contains Ablaze.sln):");
				SolutionRoot = FileUtils.ResolvePath(Console.ReadLine().Trim().Trim('\"').Trim());
				if (SolutionRoot.Length == 0)
					Console.WriteLine("Path cannot be empty.\n");
				else if (!FileUtils.FolderExists(SolutionRoot))
					Console.WriteLine("Path does not appear to exist.\n");
				else if (!FileUtils.FileExists(FileUtils.CombinePath(SolutionRoot, "Ablaze.sln")))
					Console.WriteLine("The specified folder does not contain Ablaze.sln.\n");
				else
					break;
			} while (true);
			Console.Write(@"
Choose the target platform:

1. Net Framework 2.0
2. Net Framework 3.0
3. Net Framework 3.5
4. Net Framework 3.5 Client
5. Net Framework 4.0
6. Net Framework 4.0 Client
7. Net Framework 4.5
8. Net Framework 4.5.1
9. Net Framework 4.5.2
10. Net Framework 4.6
11. Net Framework 4.6.1
12. Net Framework 4.6.2
13. Net Framework 4.7
14. Net Framework 4.7.1
15. Unity Net 3.5 Full

Choose a target platform (1-" + TargetCount + "): ");
			int selection;
			do {
				if (int.TryParse(Console.ReadLine().Trim(), out selection) && selection >= 1 && selection <= TargetCount)
					break;
				else
					Console.Write("Choose a target platform (1-" + TargetCount + "): ");
			} while (true);
			Console.Write("\nEnter path that contains projects that need to be retargeted (or leave empty to retarget Ablaze itself): ");
			string directory;
			do {
				directory = Console.ReadLine().Trim().Trim('\"').Trim();
				if (directory.Length == 0) {
					directory = SolutionRoot;
					break;
				} else if (FileUtils.FolderExists(directory))
					break;
				else
					Console.Write("Path does not appear to exist. Please choose a valid path (or leave empty to retarget Ablaze itself): ");
			} while (true);
			Console.WriteLine("\nPress enter to convert all projects in that directory and all subdirectories to compile for the specified platform...");
			Console.ReadLine();
			try {
				foreach (string file in Directory.GetFiles(FileUtils.ResolvePath(directory), "*.csproj", SearchOption.AllDirectories)) {
					try {
						XmlDocument doc = new XmlDocument();
						doc.Load(file);
						Target target = (Target) selection;
						string assemblyName = GetAssemblyName(doc.DocumentElement);
						switch (assemblyName) {
							case "System.Core":
							case "DirectXPhysics":
								continue;
							case "System.Threading.Tasks":
								switch (target) {
									case Target.Net20:
									case Target.Net30:
									case Target.Net35:
									case Target.Net35Client:
									case Target.UnityNet35:
										break;
									case Target.Net40Client:
										target = Target.Net35Client;
										break;
									case Target.Net40:
									case Target.Net45:
									case Target.Net451:
									case Target.Net452:
									case Target.Net46:
									case Target.Net461:
									case Target.Net462:
									case Target.Net47:
									case Target.Net471:
										target = Target.Net35;
										break;
								}
								break;
						}
						ProcessXml(doc.DocumentElement, null, null, null, RemoveWhitespace);
						ProcessXml(doc.DocumentElement, null, "System.Numerics.Vectors", null, RemoveReferenceToDll);
						ProcessXml(doc.DocumentElement, null, "System.Numerics.Vectors", null, RemoveReferenceToProject);
						ProcessXml(doc.DocumentElement, null, "System.Core", null, RemoveReferenceToDll);
						ProcessXml(doc.DocumentElement, null, "System.Core", null, RemoveReferenceToProject);
						ProcessXml(doc.DocumentElement, null, "System.Threading.Tasks", null, RemoveReferenceToDll);
						ProcessXml(doc.DocumentElement, null, "System.Threading.Tasks", null, RemoveReferenceToProject);
						ProcessXml(doc.DocumentElement, "TargetFrameworkProfile", string.Empty, null, ReplaceValue);
						switch (target) {
							case Target.Net20:
							case Target.Net30:
								ProcessXml(doc.DocumentElement, "TargetFrameworkVersion", target == Target.Net20 ? "v2.0" : "v3.0", null, ReplaceValue);
								ProcessXml(doc.DocumentElement, "DefineConstants", "NET20;NET35", null, ReplaceCompilerConstants);
								switch (assemblyName) {
									case "System.Numerics.Vectors":
										break;
									case "System.Threading.Tasks":
										AddReferenceToCoreProject(doc.DocumentElement, file);
										break;
									case "Ablaze.Graphics":
									case "Ablaze.ModelViewer":
									case "Ablaze.Suite":
									case "Ablaze.UI":
									case "ConsoleUnitTest":
									case "FormUnitTest":
										AddReferenceToCoreProject(doc.DocumentElement, file);
										AddReferenceToTasksProject(doc.DocumentElement, file);
										AddReferenceToVectorsProject(doc.DocumentElement, file);
										break;
									default:
										AddReferenceToCoreDll(doc.DocumentElement, file);
										AddReferenceToTasksDll(doc.DocumentElement, file);
										AddReferenceToVectorsDll(doc.DocumentElement, file);
										break;
								}
								break;
							case Target.Net35:
							case Target.Net35Client:
							case Target.UnityNet35:
								ProcessXml(doc.DocumentElement, "TargetFrameworkVersion", "v3.5", null, ReplaceValue);
								switch (target) {
									case Target.Net35Client:
										ProcessXml(doc.DocumentElement, "TargetFrameworkProfile", "Client", null, ReplaceValue);
										break;
									case Target.UnityNet35:
										ProcessXml(doc.DocumentElement, "TargetFrameworkProfile", "Unity Full v3.5", null, ReplaceValue);
										break;
								}
								ProcessXml(doc.DocumentElement, "DefineConstants", "NET35", null, ReplaceCompilerConstants);
								switch (assemblyName) {
									case "System.Numerics.Vectors":
									case "System.Threading.Tasks":
										break;
									case "Ablaze.Graphics":
									case "Ablaze.ModelViewer":
									case "Ablaze.Suite":
									case "Ablaze.UI":
									case "ConsoleUnitTest":
									case "FormUnitTest":
										AddReferenceToTasksProject(doc.DocumentElement, file);
										AddReferenceToVectorsProject(doc.DocumentElement, file);
										break;
									default:
										AddReferenceToTasksDll(doc.DocumentElement, file);
										AddReferenceToVectorsDll(doc.DocumentElement, file);
										break;
								}
								break;
							case Target.Net40:
							case Target.Net40Client:
								ProcessXml(doc.DocumentElement, "TargetFrameworkVersion", "v4.0", null, ReplaceValue);
								if (target == Target.Net40Client)
									ProcessXml(doc.DocumentElement, "TargetFrameworkProfile", "Client", null, ReplaceValue);
								ProcessXml(doc.DocumentElement, "DefineConstants", string.Empty, null, ReplaceCompilerConstants);
								switch (assemblyName) {
									case "System.Numerics.Vectors":
									case "System.Threading.Tasks":
										break;
									case "Ablaze.Graphics":
									case "Ablaze.ModelViewer":
									case "Ablaze.Suite":
									case "Ablaze.UI":
									case "ConsoleUnitTest":
									case "FormUnitTest":
										AddReferenceToVectorsProject(doc.DocumentElement, file);
										break;
									default:
										AddReferenceToVectorsDll(doc.DocumentElement, file);
										break;
								}
								break;
							case Target.Net45:
							case Target.Net451:
							case Target.Net452:
							case Target.Net46:
							case Target.Net461:
							case Target.Net462:
							case Target.Net47:
							case Target.Net471:
								string versionString = string.Empty;
								switch (target) {
									case Target.Net45:
										versionString = "v4.5";
										break;
									case Target.Net451:
										versionString = "v4.5.1";
										break;
									case Target.Net452:
										versionString = "v4.5.2";
										break;
									case Target.Net46:
										versionString = "v4.6";
										break;
									case Target.Net461:
										versionString = "v4.6.1";
										break;
									case Target.Net462:
										versionString = "v4.6.2";
										break;
									case Target.Net47:
										versionString = "v4.7";
										break;
									case Target.Net471:
										versionString = "v4.7.1";
										break;
								}
								ProcessXml(doc.DocumentElement, "TargetFrameworkVersion", versionString, null, ReplaceValue);
								string net47string = string.Empty;
								switch (target) {
									case Target.Net47:
									case Target.Net471:
										net47string = ";NET47";
										break;
								}
								ProcessXml(doc.DocumentElement, "DefineConstants", "NET45" + net47string, null, ReplaceCompilerConstants);
								switch (assemblyName) {
									case "System.Numerics.Vectors":
									case "System.Threading.Tasks":
										break;
									default:
										AddReferenceToVectorsDllNet45(doc.DocumentElement, file);
										break;
								}
								break;
						}
						doc.Save(file);
						Console.WriteLine(file);
					}
					catch {
						Console.WriteLine("Could not replace contents of " + file);
					}
				}
				foreach (string file in Directory.GetFiles(FileUtils.ResolvePath(directory), "*.config", SearchOption.AllDirectories)) {
					try {
						XmlDocument doc = new XmlDocument();
						doc.Load(file);
						ProcessXml(doc.DocumentElement, "supportedRuntime", null, null, RemoveAllInstances);
						doc.Save(file);
						Console.WriteLine(file);
					} catch {
					}
				}
			} catch {
			}
		}

		private static bool RemoveAllInstances(XmlNode node, string param, object tag) {
			node.ParentNode.RemoveChild(node);
			return true;
		}

		private static void AddReferenceToVectorsDll(XmlNode root, string csprojPath) {
			AddReferenceToDll(root, "System.Numerics.Vectors", FileUtils.CombinePath(SolutionRoot, "System.Numerics.Vectors", "bin", "Release", "System.Numerics.Vectors.dll"), csprojPath);
		}

		private static void AddReferenceToVectorsDllNet45(XmlNode root, string csprojPath) {
			AddReferenceToDll(root, "System.Numerics.Vectors", FileUtils.CombinePath(SolutionRoot, "Ablaze.Suite", "System.Numerics.Vectors.dll"), csprojPath);
		}

		private static void AddReferenceToCoreDll(XmlNode root, string csprojPath) {
			AddReferenceToDll(root, "System.Core", FileUtils.CombinePath(SolutionRoot, "System.Core", "bin", "Release", "System.Core.dll"), csprojPath);
		}

		private static void AddReferenceToTasksDll(XmlNode root, string csprojPath) {
			AddReferenceToDll(root, "System.Threading.Tasks", FileUtils.CombinePath(SolutionRoot, "System.Threading.Tasks", "bin", "Release", "System.Threading.Tasks.dll"), csprojPath);
		}

		private static void AddReferenceToDll(XmlNode root, string dllAssemblyName, string dllPathFromSolutionRoot, string csprojPath) {
			string param = dllAssemblyName + "|" + FileUtils.ToRelativePath(dllPathFromSolutionRoot, Path.GetDirectoryName(csprojPath));
			if (ProcessXml(root, null, param, null, AddReferenceToDll)) {
				XmlElement newElement = root.OwnerDocument.CreateElement("ItemGroup", root.NamespaceURI);
				root.AppendChild(newElement);
				AddDllReferenceInner(newElement, param);
			}
		}

		private static void AddDllReferenceInner(XmlNode node, string param) {
			string[] parameters = param.Split('|');
			XmlElement newElement = node.OwnerDocument.CreateElement("Reference", node.NamespaceURI);
			XmlAttribute attribute = node.OwnerDocument.CreateAttribute("Include");
			attribute.InnerText = parameters[0].Trim();
			newElement.Attributes.Append(attribute);
			XmlElement childElement = node.OwnerDocument.CreateElement("HintPath", node.NamespaceURI);
			childElement.InnerText = parameters[1].Trim();
			newElement.AppendChild(childElement);
			node.AppendChild(newElement);
		}

		private static bool AddReferenceToDll(XmlNode node, string param, object tag) {
			if (node.LocalName.Equals("Reference", StringComparison.InvariantCultureIgnoreCase)) {
				AddDllReferenceInner(node.ParentNode, param);
				return false;
			} else
				return true;
		}

		private static string GetAssemblyName(XmlNode root) {
			string assemblyName = null;
			ProcessXml(root, "AssemblyName", null, null, delegate (XmlNode node, string param, object tag) {
				assemblyName = node.InnerText.Trim();
				return false;
			});
			return assemblyName;
		}

		private static void AddReferenceToVectorsProject(XmlNode root, string currentCsprojPath) {
			AddReferenceToProject(root, "System.Numerics.Vectors", FileUtils.CombinePath(SolutionRoot, "System.Numerics.Vectors", "System.Numerics.Vectors.csproj"), "9edcf14d-a296-4e86-97bf-43c73db5e889", currentCsprojPath);
		}

		private static void AddReferenceToCoreProject(XmlNode root, string currentCsprojPath) {
			AddReferenceToProject(root, "System.Core", FileUtils.CombinePath(SolutionRoot, "System.Core", "System.Core.csproj"), "66621034-90cc-4d1a-b49e-73a13e3b22ef", currentCsprojPath);
		}

		private static void AddReferenceToTasksProject(XmlNode root, string currentCsprojPath) {
			AddReferenceToProject(root, "System.Threading.Tasks", FileUtils.CombinePath(SolutionRoot, "System.Threading.Tasks", "System.Threading.Tasks.csproj"), "dcb5d745-525c-46a1-bfc0-e12f87ab6165", currentCsprojPath);
		}

		private static void AddReferenceToProject(XmlNode root, string projectName, string pathFromSolutionRoot, string guid, string currentCsprojPath) {
			string param = projectName + "|" + FileUtils.ToRelativePath(pathFromSolutionRoot, Path.GetDirectoryName(currentCsprojPath)) + "|{" + guid + "}";
			if (ProcessXml(root, null, param, null, AddReferenceToProject)) {
				XmlElement newElement = root.OwnerDocument.CreateElement("ItemGroup", root.NamespaceURI);
				root.AppendChild(newElement);
				AddReferenceToProjectInner(newElement, param);
			}
		}

		private static bool AddReferenceToProject(XmlNode node, string param, object tag) {
			if (node.LocalName.Equals("ProjectReference", StringComparison.InvariantCultureIgnoreCase)) {
				AddReferenceToProjectInner(node.ParentNode, param);
				return false;
			} else
				return true;
		}

		private static void AddReferenceToProjectInner(XmlNode node, string param) {
			string[] parameters = param.Split('|');
			XmlElement newElement = node.OwnerDocument.CreateElement("ProjectReference", node.NamespaceURI);
			XmlAttribute attribute = node.OwnerDocument.CreateAttribute("Include");
			attribute.InnerText = parameters[1].Trim();
			newElement.Attributes.Append(attribute);
			XmlElement childElement = node.OwnerDocument.CreateElement("Project", node.NamespaceURI);
			childElement.InnerText = parameters[2].Trim();
			newElement.AppendChild(childElement);
			XmlElement nameElement = node.OwnerDocument.CreateElement("Name", node.NamespaceURI);
			nameElement.InnerText = parameters[0].Trim();
			newElement.AppendChild(nameElement);
			node.AppendChild(newElement);
		}

		public static bool RemoveWhitespace(XmlNode node, string param, object tag) {
			if (node.NodeType == XmlNodeType.Element) {
				param = node.LocalName.Trim();
				XmlElement oldElement = (XmlElement) node;
				XmlElement newElement =	node.OwnerDocument.CreateElement(param, node.NamespaceURI);
				while (oldElement.HasAttributes)
					newElement.SetAttributeNode(oldElement.RemoveAttributeNode(oldElement.Attributes[0]));
				while (oldElement.HasChildNodes)
					newElement.AppendChild(oldElement.FirstChild);
				if (oldElement.ParentNode != null)
					oldElement.ParentNode.ReplaceChild(newElement, oldElement);
			}
			return true;
		}

		private static bool ReplaceValue(XmlNode node, string newValue, object tag) {
			node.InnerText = newValue;
			return true;
		}

		private static bool ReplaceCompilerConstants(XmlNode node, string newValue, object tag) {
			List<string> compilerConstants = new List<string>(node.InnerText.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
			compilerConstants.RemoveAll(IsCompilerConstant);
			StringBuilder newString = new StringBuilder();
			foreach (string cc in compilerConstants) {
				newString.Append(cc);
				newString.Append(";");
			}
			newString.Append(newValue);
			node.InnerText = newString.ToString();
			return true;
		}

		private static bool IsCompilerConstant(string value) {
			return value.Length == 5 && value.StartsWith("NET") && char.IsNumber(value[3]) && char.IsNumber(value[4]);
		}

		private static bool RemoveReferenceToDll(XmlNode node, string param, object tag) {
			if (node.LocalName.Equals("Reference", StringComparison.InvariantCultureIgnoreCase) && node.Attributes.Count != 0 &&
				node.Attributes[0].LocalName.Equals("Include", StringComparison.InvariantCultureIgnoreCase) &&
				node.Attributes[0].InnerText.Equals(param, StringComparison.InvariantCultureIgnoreCase)) {
				node.ParentNode.RemoveChild(node);
			}
			return true;
		}

		private static bool RemoveReferenceToProject(XmlNode node, string param, object tag) {
			if (node.LocalName.Equals("ProjectReference", StringComparison.InvariantCultureIgnoreCase)) {
				foreach (XmlNode child in node) {
					if (child.LocalName.Equals("Name", StringComparison.InvariantCultureIgnoreCase) &&
						child.InnerText.Trim().Equals(param, StringComparison.InvariantCultureIgnoreCase))
						node.ParentNode.RemoveChild(node);
				}
			}
			return true;
		}

		private static bool ProcessXml(XmlNode node, string nodeToSelect, string param, object tag, ProcessValueDelegate target) {
			if (node == null)
				return true;
			else if (string.IsNullOrEmpty(nodeToSelect) || node.LocalName.Equals(nodeToSelect, StringComparison.InvariantCultureIgnoreCase)) {
				if (!target(node, param, tag))
					return false;
			}
			XmlNodeList nodeChildren = node.ChildNodes;
			for (int i = 0; i < nodeChildren.Count; i++) {
				if (!ProcessXml(nodeChildren[i], nodeToSelect, param, tag, target))
					return false;
			}
			return true;
		}
	}
}
