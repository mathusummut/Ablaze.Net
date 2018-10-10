# Ablaze.Net

An open-source C# framework for multithreaded 3D graphics and game development

## Manual Plaform Retarget:

- To target .Net Framework 4.7, declare compiler constants NET45 and NET47.
- To target .Net Framework 4.5 or 4.6, declare compiler constant NET45.
- To target .Net Framework 4.0 (Client or Non-Client profile), declare no compiler constant and add references to System.Numerics.Vectors.
- To target .Net Framework 3.5 (Client or Non-Client or Unity Full-Base Class profile), declare compiler constant NET35 and add references to System.Numerics.Vectors and System.Threading.Tasks.
- To target .Net Framework 2.0 or 3.0, declare compiler constants NET20 and NET35 and add references to System.Numerics.Vectors, System.Threading.Tasks and System.Core.

## Automatic Plaform Retarget:

Use the Ablaze.Convert tool to change the target platform.

## WinForms to Ablaze.Net Replacements:

```Form -> StyledForm (a customisable form) or GraphicsForm (if going to use OpenGL or a game loop)
OpenFileFialog/SaveFileDialog -> FilePrompt (set Open property to true/false)
Application.Run -> MessageLoop.Run
Application.EnableVisualStyles() -> Remove
Application.SetCompatibleTextRenderingDefault -> Remove
Form.ShowDialog() -> MessageLoop.ShowDialog()
Control.PointToClient/Form.PointToClient -> Extensions.PointToClient (more reliable) //StyledForm.PointToClient is already modified
	(same for PointToScreen, RectangleToClient and RectangleToScreen)
MessageBox -> StyledMessageBox
RichTextBox -> NewRichTextBox
Button -> StyledButton
CheckBox -> StyledCheckBox
ComboBox -> StyledComboBox
ContextMenu -> StyledContextMenu
ToolStripItem -> StyledItem
Label -> StyledLabel
MenuStrip -> StyledMenuStrip
TrackBar -> StyledSlider
LinkLabel -> LinkLabelCustomCursor
SplitContainer -> BufferedSplitContainer
ToolStripProfessionalRenderer -> StyleRenderer```

## Additional Replacements:

```new Bitmap("file string") -> Extensions.ImageFromFile("file string")
Bitmap.Dispose() -> Bitmap.DisposeSafe() (extension method in System.Extensions)
Pen.Dispose() -> Pen.DisposeSafe() (extension method in System.Extensions)
Brush.Dispose() -> Brush.DisposeSafe() (extension method in System.Extensions)
Stopwatch -> PreciseStopwatch
new FileStream -> FileUtils.LoadFile
File.Exists -> FileUtils.FileExists
Path.GetFileNameWithoutExtension -> FileUtils.GetFileNameWithoutExtension
Path.Combine -> FileUtils.CombinePath
Directory.Exists -> FileUtils.FolderExists
Matrix4x4 -> Matrix4
Parallel.For -> ParallelLoop.For
Random -> UniformRandom (unless using a particular seed)
Buffer.BlockCopy -> Extensions.MemoryCopy```

```
		   CCCCCCCCCCCCC        ######    ######
		 CCC::::::::::::C       #::::#    #::::#
	   CC:::::::::::::::C       #::::#    #::::#
	  C:::::CCCCCCCC::::C  ######::::######::::######
	 C:::::C       CCCCC   #::::::::::::::::::::::::#
	C:::::C                ######::::######::::######
	C:::::C                     #::::#    #::::#
	C:::::C                     #::::#    #::::#
	C:::::C                ######::::######::::######
	 C:::::C       CCCCC   #::::::::::::::::::::::::#
	  C:::::CCCCCCCC::::C  ######::::######::::######
	   CC:::::::::::::::C       #::::#    #::::#
		 CCC::::::::::::C       #::::#    #::::#
		   CCCCCCCCCCCCC        ######    ######
```