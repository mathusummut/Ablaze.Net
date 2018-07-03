using System;
using System.Windows.Forms;
using System.IO;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.Reflection;
using System.Collections.Generic;
using System.Graphics;
using System.Graphics.Models;
using System.Graphics.OGL;
using System.Drawing;
using System.Numerics;
using System.Text;

namespace Designer {
	public partial class MainWindow : GraphicsForm {
		private Scene currentScene;
		private AssemblyName[] currentlyReferencedAssemblies;

		private List<string> toReference = new List<string>();

		public MainWindow() {
			AppDomain currentDomain = AppDomain.CurrentDomain;
			InitializeComponent();
			Shown += Form1_Shown;
			InitializeGL(splitContainer1.Panel1, true, new GraphicsMode(), new MajorMinorVersion(1, 0));
			EnableFullscreenOnAltEnter = true;
		}

		/// <summary>
		/// Called when the OpenGL context is initialized.
		/// </summary>
		protected override void OnGLInitialized() {
			base.OnGLInitialized();
			MeshComponent.SetupGLEnvironment();
			GL.Enable(EnableCap.Light0);
			GL.ClearColor(Color.MidnightBlue);
			GL.Light(LightName.Light0, LightParameter.Ambient, Color4.White);
		}

		/// <summary>
		/// Called whenever a GL render takes place.
		/// </summary>
		protected override void OnPaintGL() {
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			Scene scene = currentScene;
			if (scene == null)
				return;
			//Enter drawing code here
			Matrix4 camera = scene.Camera;
			MeshComponent.Setup3D(ViewSize, ref camera, 10f);
			scene.Render();
			scene.OnRendered();
		}

		private void Form1_Shown(object sender, EventArgs e) {
			//When designer loads, get base assemblies
			currentlyReferencedAssemblies = Assembly.GetExecutingAssembly().GetReferencedAssemblies();

			for (int i = 0; i < currentlyReferencedAssemblies.Length; i++)
				toReference.Add(Assembly.Load(currentlyReferencedAssemblies[i]).Location);
		}


		private void saveToolStripMenuItem_Click(object sender, EventArgs e) {
			//Save scene - needs engine support first
		}

		private void viewCodeToolStripMenuItem_Click(object sender, EventArgs e) {
			try {
				System.Diagnostics.Process.Start(openFileDialog1.FileName);
			} catch {
			}
		}

		private void openToolStripMenuItem_Click(object sender, EventArgs e) { //loads the scene
			string oldFileName = openFileDialog1.FileName;
			if (openFileDialog1.ShowDialog() == DialogResult.OK) {
				try {
					if (oldFileName.Length != 0)
						FileUtils.Directories.Remove(Path.GetDirectoryName(oldFileName));
				} catch {
				}
				try {
					FileUtils.Directories.Add(Path.GetDirectoryName(openFileDialog1.FileName));
				} catch {
				}

				OpenScene(openFileDialog1.FileName);
			}
		}

		private void OpenScene(string filename) {
			try {
				string code;
				FileStream myStream;
				errorLog.Items.Clear();
				using (myStream = FileUtils.LoadFile(filename, FileMode.Open, FileAccess.ReadWrite, FileShare.None)) {
					StreamReader reader = new StreamReader(myStream);
					code = reader.ReadToEnd();

					CompilerParameters parameters = getSceneReferences(code);

					var provider = new CSharpCodeProvider();

					CompilerResults results = provider.CompileAssemblyFromSource(parameters, code);

					if (results.Errors.HasErrors)
						foreach (var error in results.Errors) {
							errorLog.Items.Add(error.ToString());
						}


					Assembly assembly = results.CompiledAssembly;
					var types = assembly.GetTypes();
					for (int i = 0; i < types.Length; i++) {
						if (types[i].IsSubclassOf(typeof(Scene))) {
							//foreach (Type type in types) { //go through compiled assembly types
							try {
								currentScene = (Scene)Activator.CreateInstance(types[0], true);
								if (currentScene != null) {
									propertyGrid1.SelectedObject = currentScene;
									currentScene.PrepareScene();
									currentScene.LoadScene();
									currentScene.OnSceneLoaded();
									break;
								}
							} 
							catch (Exception ex) {
								errorLog.Items.Add(System.Diagnostics.ErrorHandler.ExceptionToDetailedString(null, ex));
							}
						}
					}

					code = insertReferenceComments(code, parameters); //Save references for future use inside code
					byte[] byteInfo = new UTF8Encoding(true).GetBytes(code);
					myStream.Position = 0;
					myStream.Write(byteInfo, 0, code.Length);
				}
			}
			catch (Exception ex) {
				errorLog.Items.Add(ex.Message);
			}
		}

		private void referencesToolStripMenuItem_Click(object sender, EventArgs e) {
			ReferencesWindow referenceWindow = new ReferencesWindow(this, toReference);
			MessageLoop.ShowDialog(referenceWindow);
		}

		private void reloadToolStripMenuItem_Click(object sender, System.EventArgs e) {
			OpenScene(openFileDialog1.FileName);
		}

		protected override void OnUnload() {
			if (currentScene != null) {
				currentScene.Dispose();
				currentScene = null;
			}
		}

		private string insertReferenceComments(string code, CompilerParameters parameters) {
			List<string> lines = new List<string>();
			lines.AddRange(code.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None)); //Split code into lines

			//Convetion :
			//Code
			//...
			//line1 //REF #pathToReference
			//line2	//REF #....

			int i = 1;
			do { //Go through last lines and remove all previous references
				if (lines[lines.Count - i].Split(' ')[0] == "//REF")
					lines.RemoveAt(lines.Count - i);
				else i++;
			} while (lines[lines.Count - i].Contains("}") == false);

			for (i = 0; i < parameters.ReferencedAssemblies.Count; i++) //Insert all current referenced assemblies for the Scene at top of code
					lines.Insert(lines.Count, "//REF " + parameters.ReferencedAssemblies[i]);

			code = string.Join(Environment.NewLine, lines); //Join back lines

			return code;
		}

		private CompilerParameters getSceneReferences(string code) {
			//Method assumes that if //REF exists in cs file, they are the only required references
			//If no //REF exists, the user specified references are used instead
			List<string> lines = new List<string>();
			lines.AddRange(code.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None)); //Split code into lines
			CompilerParameters parameters = new CompilerParameters() {
				GenerateExecutable = false,
				GenerateInMemory = true,
				CompilerOptions = "/optimize"
			};

			int i = 0;
			do { //Search from last lines and add all references found up till first }
				i++;
				if (lines[lines.Count - i].Split(' ')[0] == "//REF")
					parameters.ReferencedAssemblies.Add(@lines[lines.Count - i].Substring(6).Replace(Environment.NewLine, ""));
			} while (!lines[lines.Count - i].Contains("}"));

			if (i == 1)	parameters.ReferencedAssemblies.AddRange(toReference.ToArray()); //Set references to user defined references

			return parameters;
		}
	}
}
