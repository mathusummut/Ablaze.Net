#pragma warning disable 1591

using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Windows.Forms;

namespace System.Platforms.Windows {
	public static class Shell {
		[CLSCompliant(false)]
		public const uint STGM_READ = 0;
		public const int MAX_PATH = 260;

		private static string[] knownFolderGuids = new string[] {
			"{B4BFCC3A-DB2C-424C-B029-7FE99A87C641}", // Desktop
			"{FDD39AD0-238F-46AF-ADB4-6C85480369C7}", // Documents
			"{374DE290-123F-4565-9164-39C4925E467B}", // Downloads
			"{33E28130-4E1E-4676-835A-98395C3BC3BB}", // Pictures
			"{18989B1D-99B5-455B-841C-AB7C74E4DDFC}", // Videos
			"{4BD8D571-6D19-48D3-BE97-422220080E43}", // Music
			"{56784854-C6CB-462B-8169-88E350ACB882}", // Contacts
			"{1777F761-68AD-4D8A-87BD-30B759FA33DD}", // Favorites
			"{BFB9D5E0-C6A9-404C-B2B2-AE6DB6AF4968}", // Links
			"{4C5C32FF-BB9D-43B0-B5B4-2D72E54EAAA4}", // SavedGames
			"{7D1D3A04-DEBB-4115-95CF-2F29DA2920DA}", // SavedSearches
		};

		[SuppressUnmanagedCodeSecurity]
		[DllImport("shell32.dll", EntryPoint = "SHGetKnownFolderPath", ExactSpelling = true)]
		[CLSCompliant(false)]
		public static extern int GetKnownFolderPath([MarshalAs(UnmanagedType.LPStruct)]Guid rfid, uint dwFlags, IntPtr hToken, out IntPtr ppszPath);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("shell32.dll", EntryPoint = "SHGetFileInfo", ExactSpelling = true)]
		[CLSCompliant(false)]
		public static extern IntPtr GetFileInfo(String pszPath, System.IO.FileAttributes dwFileAttributes, ref SHFILEINFO psfi, UInt32 cbFileInfo, ShGetFileIconFlags uFlags);

		[Flags]
		public enum SLGP_FLAGS {
			/// <summary>Retrieves the standard short (8.3 format) file name</summary>
			SLGP_SHORTPATH = 0x1,
			/// <summary>Retrieves the Universal Naming Convention (UNC) path name of the file</summary>
			SLGP_UNCPRIORITY = 0x2,
			/// <summary>Retrieves the raw path name. A raw path is something that might not exist and may include environment variables that need to be expanded</summary>
			SLGP_RAWPATH = 0x4
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public struct WIN32_FIND_DATAW {
			[CLSCompliant(false)]
			public uint dwFileAttributes;
			public long ftCreationTime;
			public long ftLastAccessTime;
			public long ftLastWriteTime;
			[CLSCompliant(false)]
			public uint nFileSizeHigh;
			[CLSCompliant(false)]
			public uint nFileSizeLow;
			[CLSCompliant(false)]
			public uint dwReserved0;
			[CLSCompliant(false)]
			public uint dwReserved1;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
			public string cFileName;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
			public string cAlternateFileName;
		}

		[Flags]
		public enum SLR_FLAGS {
			/// <summary>
			/// Do not display a dialog box if the link cannot be resolved. When SLR_NO_UI is set,
			/// the high-order word of fFlags can be set to a time-out value that specifies the
			/// maximum amount of time to be spent resolving the link. The function returns if the
			/// link cannot be resolved within the time-out duration. If the high-order word is set
			/// to zero, the time-out duration will be set to the default value of 3,000 milliseconds
			/// (3 seconds). To specify a value, set the high word of fFlags to the desired time-out
			/// duration, in milliseconds.
			/// </summary>
			SLR_NO_UI = 0x1,
			/// <summary>Obsolete and no longer used</summary>
			SLR_ANY_MATCH = 0x2,
			/// <summary>If the link object has changed, update its path and list of identifiers.
			/// If SLR_UPDATE is set, you do not need to call IPersistFile::IsDirty to determine
			/// whether or not the link object has changed.</summary>
			SLR_UPDATE = 0x4,
			/// <summary>Do not update the link information</summary>
			SLR_NOUPDATE = 0x8,
			/// <summary>Do not execute the search heuristics</summary>
			SLR_NOSEARCH = 0x10,
			/// <summary>Do not use distributed link tracking</summary>
			SLR_NOTRACK = 0x20,
			/// <summary>Disable distributed link tracking. By default, distributed link tracking tracks
			/// removable media across multiple devices based on the volume name. It also uses the
			/// Universal Naming Convention (UNC) path to track remote file systems whose drive letter
			/// has changed. Setting SLR_NOLINKINFO disables both types of tracking.</summary>
			SLR_NOLINKINFO = 0x40,
			/// <summary>Call the Microsoft Windows Installer</summary>
			SLR_INVOKE_MSI = 0x80
		}


		/// <summary>The IShellLink interface allows Shell links to be created, modified, and resolved</summary>
		[ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("000214F9-0000-0000-C000-000000000046")]
		public interface IShellLinkW {
			/// <summary>Retrieves the path and file name of a Shell link object</summary>
			void GetPath([Out(), MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszFile, int cchMaxPath, out WIN32_FIND_DATAW pfd, SLGP_FLAGS fFlags);
			/// <summary>Retrieves the list of item identifiers for a Shell link object</summary>
			void GetIDList(out IntPtr ppidl);
			/// <summary>Sets the pointer to an item identifier list (PIDL) for a Shell link object.</summary>
			void SetIDList(IntPtr pidl);
			/// <summary>Retrieves the description string for a Shell link object</summary>
			void GetDescription([Out(), MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszName, int cchMaxName);
			/// <summary>Sets the description for a Shell link object. The description can be any application-defined string</summary>
			void SetDescription([MarshalAs(UnmanagedType.LPWStr)] string pszName);
			/// <summary>Retrieves the name of the working directory for a Shell link object</summary>
			void GetWorkingDirectory([Out(), MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszDir, int cchMaxPath);
			/// <summary>Sets the name of the working directory for a Shell link object</summary>
			void SetWorkingDirectory([MarshalAs(UnmanagedType.LPWStr)] string pszDir);
			/// <summary>Retrieves the command-line arguments associated with a Shell link object</summary>
			void GetArguments([Out(), MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszArgs, int cchMaxPath);
			/// <summary>Sets the command-line arguments for a Shell link object</summary>
			void SetArguments([MarshalAs(UnmanagedType.LPWStr)] string pszArgs);
			/// <summary>Retrieves the hot key for a Shell link object</summary>
			void GetHotkey(out short pwHotkey);
			/// <summary>Sets a hot key for a Shell link object</summary>
			void SetHotkey(short wHotkey);
			/// <summary>Retrieves the show command for a Shell link object</summary>
			void GetShowCmd(out int piShowCmd);
			/// <summary>Sets the show command for a Shell link object. The show command sets the initial show state of the window.</summary>
			void SetShowCmd(int iShowCmd);
			/// <summary>Retrieves the location (path and index) of the icon for a Shell link object</summary>
			void GetIconLocation([Out(), MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszIconPath,
				int cchIconPath, out int piIcon);
			/// <summary>Sets the location (path and index) of the icon for a Shell link object</summary>
			void SetIconLocation([MarshalAs(UnmanagedType.LPWStr)] string pszIconPath, int iIcon);
			/// <summary>Sets the relative path to the Shell link object</summary>
			void SetRelativePath([MarshalAs(UnmanagedType.LPWStr)] string pszPathRel, int dwReserved);
			/// <summary>Attempts to find the target of a Shell link, even if it has been moved or renamed</summary>
			void Resolve(IntPtr hwnd, SLR_FLAGS fFlags);
			/// <summary>Sets the path and file name of a Shell link object</summary>
			void SetPath([MarshalAs(UnmanagedType.LPWStr)] string pszFile);

		}

		[ComImport, Guid("0000010c-0000-0000-c000-000000000046"),
		InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IPersist {
			[PreserveSig]
			void GetClassID(out Guid pClassID);
		}

		[CLSCompliant(false)]
		[ComImport, Guid("0000010b-0000-0000-C000-000000000046"),
		InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IPersistFile : IPersist {
			new void GetClassID(out Guid pClassID);
			[PreserveSig]
			int IsDirty();

			[PreserveSig]
			void Load([In, MarshalAs(UnmanagedType.LPWStr)] string pszFileName, uint dwMode);

			[PreserveSig]
			void Save([In, MarshalAs(UnmanagedType.LPWStr)] string pszFileName,
				[In, MarshalAs(UnmanagedType.Bool)] bool fRemember);

			[PreserveSig]
			void SaveCompleted([In, MarshalAs(UnmanagedType.LPWStr)] string pszFileName);

			[PreserveSig]
			void GetCurFile([In, MarshalAs(UnmanagedType.LPWStr)] string ppszFileName);
		}

		// CLSID_ShellLink from ShlGuid.h 
		[ComImport, Guid("00021401-0000-0000-C000-000000000046")]
		public class ShellLink {
		}

		/// <summary>
		/// Gets the current path to the specified known folder as currently configured.
		/// Does not require the folder to be existent. The return value can be null.
		/// </summary>
		/// <param name="knownFolder">The known folder which current path will be returned.</param>
		/// <param name="defaultUser">Specifies if the paths of the default user (user profile template) will be used. This requires administrative rights.</param>
		public static string GetPath(KnownFolder knownFolder, bool defaultUser = false) {
			IntPtr outPath;
			if (Platform.IsWindowsVistaOrNewer && GetKnownFolderPath(new Guid(knownFolderGuids[(int) knownFolder]), (uint) KnownFolderFlags.DontVerify, new IntPtr(defaultUser ? -1 : 0), out outPath) >= 0)
				return Marshal.PtrToStringUni(outPath);
			else
				return null;
		}

		[CLSCompliant(false)]
		[Flags]
		public enum KnownFolderFlags : uint {
			SimpleIDList = 0x00000100,
			NotParentRelative = 0x00000200,
			DefaultPath = 0x00000400,
			Init = 0x00000800,
			NoAlias = 0x00001000,
			DontUnexpand = 0x00002000,
			DontVerify = 0x00004000,
			Create = 0x00008000,
			NoAppcontainerRedirection = 0x00010000,
			AliasOnly = 0x80000000
		}

		/// <summary>
		/// Standard folders registered with the system.
		/// </summary>
		public enum KnownFolder {
			Desktop,
			Documents,
			Downloads,
			Pictures,
			Videos,
			Music,
			Contacts,
			Favorites,
			Links,
			SavedGames,
			SavedSearches
		}
	}

	[Flags]
	[CLSCompliant(false)]
	public enum EXECUTION_STATE : uint {
		ES_SYSTEM_REQUIRED = 0x00000001,
		ES_DISPLAY_REQUIRED = 0x00000002,
		// Legacy flag, should not be used.
		// ES_USER_PRESENT   = 0x00000004,
		ES_AWAYMODE_REQUIRED = 0x00000040,
		ES_CONTINUOUS = 0x80000000,
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct SIZE {
		public static readonly int Size = Marshal.SizeOf(typeof(SIZE));
		public int cx;
		public int cy;

		public SIZE(int width, int height) {
			cx = width;
			cy = height;
		}
	}

	[CLSCompliant(false)]
	public enum SysCommandState : uint {
		MF_ENABLED = 0,
		MF_GRAYED = 1
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct BLENDFUNCTION {
		public static readonly int Size = Marshal.SizeOf(typeof(BLENDFUNCTION));
		public byte BlendOp;
		public byte BlendFlags;
		public byte SourceConstantAlpha;
		public byte AlphaFormat;
	}

	public enum DWMNCRENDERINGPOLICY {
		DWMNCRP_USEWINDOWSTYLE,
		DWMNCRP_DISABLED,
		DWMNCRP_ENABLED,
		DWMNCRP_LAST
	}

	public enum ClassLongFlags : int {
		GCLP_MENUNAME = -8,
		GCLP_HBRBACKGROUND = -10,
		GCLP_HCURSOR = -12,
		GCLP_HICON = -14,
		GCLP_HMODULE = -16,
		GCL_CBWNDEXTRA = -18,
		GCL_CBCLSEXTRA = -20,
		GCLP_WNDPROC = -24,
		GCL_STYLE = -26,
		GCLP_HICONSM = -34,
		GCW_ATOM = -32
	}

	public enum BlendOps : byte {
		AC_SRC_OVER = 0x00,
		AC_SRC_ALPHA = 0x01,
		AC_SRC_NO_PREMULT_ALPHA = 0x01,
		AC_SRC_NO_ALPHA = 0x02,
		AC_DST_NO_PREMULT_ALPHA = 0x10,
		AC_DST_NO_ALPHA = 0x20
	}

	[CLSCompliant(false)]
	public enum BlendFlags : uint {
		None = 0x00,
		ULW_COLORKEY = 0x01,
		ULW_ALPHA = 0x02,
		ULW_OPAQUE = 0x04
	}

	public enum PrintFunctionParameters : int {
		CHECKVISIBLE = 1,
		NONCLIENT = 2,
		CLIENT = 4,
		ERASEBKGND = 8,
		CHILDREN = 16,
		OWNED = 32
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct UPDATELAYEREDWINDOWINFO {
		public static readonly int Size = Marshal.SizeOf(typeof(UPDATELAYEREDWINDOWINFO));
		public int cbSize;
		public IntPtr hdcDst;
		public IntPtr pptDst;
		public IntPtr psize;
		public IntPtr hdcSrc;
		public IntPtr pptSrc;
		public int crKey;
		public IntPtr pblend;
		[CLSCompliant(false)]
		public BlendFlags dwFlags;
		public IntPtr prcDirty;
	}

	[Flags]
	public enum DWM_BB {
		Enable = 1,
		BlurRegion = 2,
		TransitionMaximized = 4
	}

	[Flags]
	public enum Gesture {
		Begin = 1,
		End,
		Zoom,
		Pan,
		Rotate,
		TwoFingerTap,
		PressAndTap
	}

	/// <summary>
	/// Gesture flags - GestureInfo.flags
	/// </summary>
	[Flags]
	public enum GestureFlags {
		/// <summary>
		/// GF_BEGIN
		/// </summary>
		Begin = 0x1,
		/// <summary>
		/// GF_INERTIA
		/// </summary>
		Inertia = 0x2,
		/// <summary>
		/// GF_END
		/// </summary>
		End = 0x4
	}

	/// <summary>
	/// Gesture configuration structure
	/// </summary>
	/// <remarks>
	/// Used in SetGestureConfig and GetGestureConfig
	/// http://msdn.microsoft.com/en-us/library/windows/desktop/dd353231%28v=vs.85%29.aspx
	/// </remarks>
	[StructLayout(LayoutKind.Sequential)]
	public struct GestureConfig {
		public static int Size = Marshal.SizeOf(typeof(GestureConfig));

		[CLSCompliant(false)]
		public GestureConfig(uint id, uint want, uint block) {
			Id = id;
			Want = want;
			Block = block;
		}

		/// <summary>
		/// The identifier for the type of configuration that will have messages enabled or disabled.
		/// </summary>
		[CLSCompliant(false)]
		public uint Id;

		/// <summary>
		/// The messages to enable.
		/// </summary>
		[CLSCompliant(false)]
		public uint Want;

		/// <summary>
		/// The messages to disable.
		/// </summary>
		[CLSCompliant(false)]
		public uint Block;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct PointShort {
		public short X;
		public short Y;
	}

	/// <summary>
	/// Stores information about a gesture.
	/// </summary>
	/// <remarks>
	/// - Pass the HGESTUREINFO received in the WM_GESTURE message lParam into the
	///   GetGestureInfo function to retrieve this information.
	/// - If cbExtraArgs is non-zero, pass the HGESTUREINFO received in the WM_GESTURE
	///   message lParam into the GetGestureExtraArgs function to retrieve extended
	///   argument information. 
	/// </remarks>
	[StructLayout(LayoutKind.Sequential)]
	public struct GestureInfo {
		/// <summary>
		/// The size of the structure, in bytes. The caller must set this.
		/// </summary>
		public int Size;
		/// <summary>
		/// The state of the gesture. Contains GestureFlags.
		/// </summary>
		public int Flags;
		/// <summary>
		/// The identifier of the gesture command. Contains GestureId.
		/// </summary>
		public int ID;
		/// <summary>
		/// A handle to the window that is targeted by this gesture.
		/// </summary>
		public IntPtr Hwnd;
		/// <summary>
		/// A Point structure containing the coordinates associated with the gesture. These coordinates are always relative to the origin of the screen.
		/// </summary>
		public PointShort Location;
		/// <summary>
		/// An internally used identifier for the structure.
		/// </summary>
		public int InstanceID;
		/// <summary>
		/// An internally used identifier for the sequence.
		/// </summary>
		public int SequenceID;
		/// <summary>
		/// A 64-bit unsigned integer that contains the arguments for gestures that fit into 8 bytes. 
		/// </summary>
		[CLSCompliant(false)]
		public ulong Arguments;
		/// <summary>
		/// The size, in bytes, of extra arguments that accompany this gesture.
		/// </summary>
		public int ExtraArguments;

		/// <summary>
		/// Gets a value indicating whether the <see cref="GestureFlags"/> Begin is set.
		/// </summary>
		public bool Begin {
			get {
				return ((GestureFlags) Flags & GestureFlags.Begin) == GestureFlags.Begin;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the <see cref="GestureFlags"/> End is set.
		/// </summary>
		public bool End {
			get {
				return ((GestureFlags) Flags & GestureFlags.End) == GestureFlags.End;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the <see cref="GestureFlags"/> Inertia is set
		/// </summary>
		public bool Inertia {
			get {
				return ((GestureFlags) Flags & GestureFlags.Inertia) == GestureFlags.Inertia;
			}
		}
	}

	[CLSCompliant(false)]
	public enum TouchInputMask : uint {
		None = 0,
		TimeFromSystem = 1,
		ExtraInfo = 2,
		ContactArea = 4
	}

	[CLSCompliant(false)]
	public enum TouchEvent : uint {
		None = 0,
		Move = 1,
		Down = 2,
		Up = 4,
		InRange = 8,
		Primary = 16,
		NoCoalesce = 32,
		Palm = 128
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct TouchInput {
		public static int Size = Marshal.SizeOf(typeof(TouchInput));
		public int X;
		public int Y;
		public IntPtr Source;
		[CLSCompliant(false)]
		public uint ID;
		[CLSCompliant(false)]
		public TouchEvent Flags;
		[CLSCompliant(false)]
		public TouchInputMask Mask;
		[CLSCompliant(false)]
		public uint Time;
		[CLSCompliant(false)]
		public IntPtr ExtraInfo;
		[CLSCompliant(false)]
		public uint ContactWidth;
		[CLSCompliant(false)]
		public uint ContactHeight;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct DWM_BLURBEHIND {
		public DWM_BB dwFlags;
		public bool fEnable;
		public IntPtr hRgnBlur;
		public bool fTransitionOnMaximized;
	}

	[Flags]
	public enum PixelFormatDescriptorFlags : int {
		DOUBLEBUFFER = 0x01,
		STEREO = 0x02,
		DRAW_TO_WINDOW = 0x04,
		DRAW_TO_BITMAP = 0x08,
		SUPPORT_GDI = 0x10,
		SUPPORT_OPENGL = 0x20,
		GENERIC_FORMAT = 0x40,
		NEED_PALETTE = 0x80,
		NEED_SYSTEM_PALETTE = 0x100,
		SWAP_EXCHANGE = 0x200,
		SWAP_COPY = 0x400,
		SWAP_LAYER_BUFFERS = 0x800,
		GENERIC_ACCELERATED = 0x1000,
		SUPPORT_DIRECTDRAW = 0x2000,
		SUPPORT_COMPOSITION = 0x8000,
		DEPTH_DONTCARE = unchecked((int) 0x20000000),
		DOUBLEBUFFER_DONTCARE = unchecked((int) 0x40000000),
		STEREO_DONTCARE = unchecked((int) 0x80000000)
	}

	/// <summary>
	/// Describes a pixel format. It is used when interfacing with the WINAPI to create a new Context.
	/// Found in WinGDI.h
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct PixelFormatDescriptor {
		public static readonly short StructSize = (short) Marshal.SizeOf(typeof(PixelFormatDescriptor));
		public short Size;
		public short Version;
		public PixelFormatDescriptorFlags Flags;
		public PixelType PixelType;
		public byte ColorBits;
		public byte RedBits;
		public byte RedShift;
		public byte GreenBits;
		public byte GreenShift;
		public byte BlueBits;
		public byte BlueShift;
		public byte AlphaBits;
		public byte AlphaShift;
		public byte AccumBits;
		public byte AccumRedBits;
		public byte AccumGreenBits;
		public byte AccumBlueBits;
		public byte AccumAlphaBits;
		public byte DepthBits;
		public byte StencilBits;
		public byte AuxBuffers;
		public byte LayerType;
		public byte Reserved;
		public int LayerMask;
		public int VisibleMask;
		public int DamageMask;
	}

	[Serializable, StructLayout(LayoutKind.Sequential)]
	public struct SCROLLINFO {
		[CLSCompliant(false)]
		public uint cbSize;
		[CLSCompliant(false)]
		public uint fMask;
		public int nMin;
		public int nMax;
		[CLSCompliant(false)]
		public uint nPage;
		public int nPos;
		public int nTrackPos;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct COLORREF {
		public static readonly COLORREF Empty = new COLORREF();
		[CLSCompliant(false)]
		public uint crKey;

		public COLORREF(Color color) {
			crKey = color.R + (((uint) color.G) << 8) + (((uint) color.B) << 16);
		}

		public Color GetColor() {
			return Color.FromArgb((int) (0x000000FFu & crKey), (int) (0x0000FF00u & crKey) >> 8, (int) (0x00FF0000u & crKey) >> 16);
		}

		public void SetColor(Color color) {
			crKey = color.R + (((uint) color.G) << 8) + (((uint) color.B) << 16);
		}
	}

	public enum DeviceCap {
		/// <summary>
		/// Device driver version
		/// </summary>
		DRIVERVERSION = 0,
		/// <summary>
		/// Device classification
		/// </summary>
		TECHNOLOGY = 2,
		/// <summary>
		/// Horizontal size in millimeters
		/// </summary>
		HORZSIZE = 4,
		/// <summary>
		/// Vertical size in millimeters
		/// </summary>
		VERTSIZE = 6,
		/// <summary>
		/// Horizontal width in pixels
		/// </summary>
		HORZRES = 8,
		/// <summary>
		/// Vertical height in pixels
		/// </summary>
		VERTRES = 10,
		/// <summary>
		/// Number of bits per pixel
		/// </summary>
		BITSPIXEL = 12,
		/// <summary>
		/// Number of planes
		/// </summary>
		PLANES = 14,
		/// <summary>
		/// Number of brushes the device has
		/// </summary>
		NUMBRUSHES = 16,
		/// <summary>
		/// Number of pens the device has
		/// </summary>
		NUMPENS = 18,
		/// <summary>
		/// Number of markers the device has
		/// </summary>
		NUMMARKERS = 20,
		/// <summary>
		/// Number of fonts the device has
		/// </summary>
		NUMFONTS = 22,
		/// <summary>
		/// Number of colors the device supports
		/// </summary>
		NUMCOLORS = 24,
		/// <summary>
		/// Size required for device descriptor
		/// </summary>
		PDEVICESIZE = 26,
		/// <summary>
		/// Curve capabilities
		/// </summary>
		CURVECAPS = 28,
		/// <summary>
		/// Line capabilities
		/// </summary>
		LINECAPS = 30,
		/// <summary>
		/// Polygonal capabilities
		/// </summary>
		POLYGONALCAPS = 32,
		/// <summary>
		/// Text capabilities
		/// </summary>
		TEXTCAPS = 34,
		/// <summary>
		/// Clipping capabilities
		/// </summary>
		CLIPCAPS = 36,
		/// <summary>
		/// Bitblt capabilities
		/// </summary>
		RASTERCAPS = 38,
		/// <summary>
		/// Length of the X leg
		/// </summary>
		ASPECTX = 40,
		/// <summary>
		/// Length of the Y leg
		/// </summary>
		ASPECTY = 42,
		/// <summary>
		/// Length of the hypotenuse
		/// </summary>
		ASPECTXY = 44,
		/// <summary>
		/// Shading and Blending caps
		/// </summary>
		SHADEBLENDCAPS = 45,

		/// <summary>
		/// Logical pixels inch in X
		/// </summary>
		LOGPIXELSX = 88,
		/// <summary>
		/// Logical pixels inch in Y
		/// </summary>
		LOGPIXELSY = 90,

		/// <summary>
		/// Number of entries in physical palette
		/// </summary>
		SIZEPALETTE = 104,
		/// <summary>
		/// Number of reserved entries in palette
		/// </summary>
		NUMRESERVED = 106,
		/// <summary>
		/// Actual color resolution
		/// </summary>
		COLORRES = 108,

		// Printing related DeviceCaps. These replace the appropriate Escapes
		/// <summary>
		/// Physical Width in device units
		/// </summary>
		PHYSICALWIDTH = 110,
		/// <summary>
		/// Physical Height in device units
		/// </summary>
		PHYSICALHEIGHT = 111,
		/// <summary>
		/// Physical Printable Area x margin
		/// </summary>
		PHYSICALOFFSETX = 112,
		/// <summary>
		/// Physical Printable Area y margin
		/// </summary>
		PHYSICALOFFSETY = 113,
		/// <summary>
		/// Scaling factor x
		/// </summary>
		SCALINGFACTORX = 114,
		/// <summary>
		/// Scaling factor y
		/// </summary>
		SCALINGFACTORY = 115,

		/// <summary>
		/// Current vertical refresh rate of the display device (for displays only) in Hz
		/// </summary>
		VREFRESH = 116,
		/// <summary>
		/// Vertical height of entire desktop in pixels
		/// </summary>
		DESKTOPVERTRES = 117,
		/// <summary>
		/// Horizontal width of entire desktop in pixels
		/// </summary>
		DESKTOPHORZRES = 118,
		/// <summary>
		/// Preferred blt alignment
		/// </summary>
		BLTALIGNMENT = 119
	}

	[CLSCompliant(false)]
	[Flags]
	public enum SendMessageTimeoutFlags : uint {
		NORMAL = 0x0,
		BLOCK = 0x1,
		ABORTIFHUNG = 0x2,
		NOTIMEOUTIFNOTHUNG = 0x8,
		ERRORONEXIT = 0x20
	}


	[Guid("00000122-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IOleDropTarget {
		[MethodImpl(MethodImplOptions.PreserveSig)]
		int OleDragEnter([In] object pDataObj, [In] int grfKeyState, [In] POINT pt, [In][Out] ref int pdwEffect);

		[MethodImpl(MethodImplOptions.PreserveSig)]
		int OleDragLeave();

		[MethodImpl(MethodImplOptions.PreserveSig)]
		int OleDragOver([In] int grfKeyState, [In] POINT pt, [In][Out] ref int pdwEffect);

		[MethodImpl(MethodImplOptions.PreserveSig)]
		int OleDrop([In] object pDataObj, [In] int grfKeyState, [In] POINT pt, [In][Out] ref int pdwEffect);
	}

	public enum AccentState {
		DISABLED = 0,
		ENABLE_GRADIENT = 1,
		ENABLE_TRANSPARENTGRADIENT = 2,
		ENABLE_BLURBEHIND = 3,
		INVALID_STATE = 4
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct AccentPolicy {
		public static readonly int Size = Marshal.SizeOf(typeof(AccentPolicy));
		public AccentState AccentState;
		public int AccentFlags;
		public int GradientColor;
		public int AnimationId;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct WindowCompositionAttributeData {
		public DwmWindowAttribute Attribute;
		public IntPtr Data;
		public int SizeOfData;
	}

	public enum DwmWindowAttribute {
		NCRENDERING_ENABLED = 1,
		NCRENDERING_POLICY,
		TRANSITIONS_FORCEDISABLED,
		ALLOW_NCPAINT,
		CAPTION_BUTTON_BOUNDS,
		NONCLIENT_RTL_LAYOUT,
		FORCE_ICONIC_REPRESENTATION,
		FLIP3D_POLICY,
		EXTENDED_FRAME_BOUNDS,
		HAS_ICONIC_BITMAP,
		DISALLOW_PEEK,
		EXCLUDED_FROM_PEEK,
		LAST,
		ACCENT_POLICY = 19
	}

	[CLSCompliant(false)]
	public enum MessageFilter : uint {
		Reset,
		Allow,
		Disallow
	}

	[SuppressUnmanagedCodeSecurity]
	public static class NativeApi {
		public delegate void WaitOrTimerDelegate(IntPtr lpParameter, bool timerOrWaitFired);

		[DllImport("user32.dll")]
		[SuppressUnmanagedCodeSecurity]
		[CLSCompliant(false)]
		public static extern bool ChangeWindowMessageFilter(WindowMessage message, MessageFilter dwFlag);

		[DllImport("user32.dll")]
		[SuppressUnmanagedCodeSecurity]
		[return: MarshalAs(UnmanagedType.Bool)]
		[CLSCompliant(false)]
		public static extern bool RegisterTouchWindow(IntPtr hWnd, uint ulFlags);

		[DllImport("user32.dll")]
		[SuppressUnmanagedCodeSecurity]
		[return: MarshalAs(UnmanagedType.Bool)]
		[CLSCompliant(false)]
		public static extern bool GetTouchInputInfo(IntPtr hTouchInput, int cInputs, TouchInput[] pInputs, int cbSize);

		[DllImport("user32.dll")]
		[SuppressUnmanagedCodeSecurity]
		[return: MarshalAs(UnmanagedType.Bool)]
		[CLSCompliant(false)]
		public static extern bool CloseTouchInputHandle(IntPtr hTouchInput);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		[CLSCompliant(false)]
		public static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool GetGestureInfo(IntPtr hGesture, out GestureInfo lGesture);

		public static void SetScreenSaverState(bool enable) {
			if (enable)
				SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS);
			else if (SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS | EXECUTION_STATE.ES_DISPLAY_REQUIRED | EXECUTION_STATE.ES_SYSTEM_REQUIRED | EXECUTION_STATE.ES_AWAYMODE_REQUIRED) == 0) //Away mode for Windows >= Vista
				SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS | EXECUTION_STATE.ES_DISPLAY_REQUIRED | EXECUTION_STATE.ES_SYSTEM_REQUIRED); //Windows < Vista, forget away mode
		}

		[DllImport("user32.dll")]
		[SuppressUnmanagedCodeSecurity]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool SetGestureConfig(IntPtr hWnd, int dwReserved, int cIDs, GestureConfig[] pGestureConfig, int cbSize);

		[DllImport("user32.dll")]
		[SuppressUnmanagedCodeSecurity]
		public static extern bool CloseGestureInfoHandle(IntPtr pGestureInfo);

		[DllImport("gdi32.dll")]
		[SuppressUnmanagedCodeSecurity]
		public static extern int SetBkMode(IntPtr hdc, int iBkMode);

		[DllImport("shell32.dll")]
		[SuppressUnmanagedCodeSecurity]
		public static extern void DragAcceptFiles(IntPtr hWnd, bool acceptFiles);

		[DllImport("shell32.dll")]
		[SuppressUnmanagedCodeSecurity]
		[CLSCompliant(false)]
		public static extern uint DragQueryFile(IntPtr hDrop, uint iFile, [Out] StringBuilder lpszFile, uint cch);

		[DllImport("shell32.dll")]
		[SuppressUnmanagedCodeSecurity]
		public static extern void DragFinish(IntPtr hDrop);

		[DllImport("user32.dll")]
		[SuppressUnmanagedCodeSecurity]
		public static extern bool SetWindowCompositionAttribute(IntPtr hwnd, ref WindowCompositionAttributeData data);

		/// <summary>
		/// Sets the current process as dots per inch (dpi) aware.
		/// Note: SetProcessDPIAware is subject to a possible race condition
		/// if a DLL caches dpi settings during initialization.
		/// For this reason, it is recommended that dpi-aware be set through
		/// the application (.exe) manifest rather than by calling SetProcessDPIAware.
		/// </summary>
		/// <returns>
		/// If the function succeeds, the return value is true.
		/// Otherwise, the return value is false.
		/// </returns>
		/// <remarks>
		/// DLLs should accept the dpi setting of the host process
		/// rather than call SetProcessDPIAware themselves.
		/// To be set properly, dpiAware should be specified as part
		/// of the application (.exe) manifest.
		/// </remarks>
		[DllImport("user32.dll")]
		[SuppressUnmanagedCodeSecurity]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool SetProcessDPIAware();

		[SuppressUnmanagedCodeSecurity]
		[DllImport("ole32.dll", CharSet = CharSet.None, ExactSpelling = true, SetLastError = true)]
		public static extern int OleInitialize(int val);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("ole32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int RegisterDragDrop(IntPtr hwnd, IOleDropTarget target);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("ole32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int RevokeDragDrop(IntPtr hwnd);

		[return: MarshalAs(UnmanagedType.Bool)]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", SetLastError = true)]
		[CLSCompliant(false)]
		public static extern bool PostThreadMessage(uint threadId, WindowMessage msg, IntPtr wParam, IntPtr lParam);

		[SuppressUnmanagedCodeSecurity]
		[CLSCompliant(false)]
		[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessageTimeout(IntPtr windowHandle, WindowMessage Msg, IntPtr wParam, IntPtr lParam, SendMessageTimeoutFlags flags, uint timeout, out IntPtr result);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("gdi32.dll")]
		public static extern int GetDeviceCaps(IntPtr hdc, DeviceCap nIndex);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool CloseHandle(IntPtr hObject);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("kernel32.dll")]
		public static extern IntPtr CreateWaitableTimer(IntPtr lpTimerAttributes, bool bManualReset, string lpTimerName);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("kernel32.dll")]
		public static extern bool SetWaitableTimer(IntPtr hTimer, [In] ref long pDueTime, int lPeriod, IntPtr pfnCompletionRoutine,
						IntPtr lpArgToCompletionRoutine, bool fResume);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
		public static extern Int32 WaitForSingleObject(IntPtr handle, int milliseconds);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("kernel32.dll")]
		[CLSCompliant(false)]
		public static extern bool TerminateThread(IntPtr thread, uint exitCode);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("kernel32.dll")]
		[CLSCompliant(false)]
		public static extern uint GetCurrentThreadId();

		[SuppressUnmanagedCodeSecurity]
		[DllImport("kernel32.dll")]
		public static extern IntPtr GetCurrentThread();

		[SuppressUnmanagedCodeSecurity]
		[DllImport("kernel32.dll")]
		[CLSCompliant(false)]
		public static extern bool CreateTimerQueueTimer(out IntPtr phNewTimer, IntPtr TimerQueue, WaitOrTimerDelegate Callback, IntPtr Parameter, uint DueTime, uint Period, uint Flags);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("kernel32.dll")]
		[CLSCompliant(false)]
		public static extern bool ChangeTimerQueueTimer(IntPtr timerQueue, IntPtr timer, uint dueTime, uint period);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("kernel32.dll")]
		public static extern bool DeleteTimerQueueTimer(IntPtr timerQueue, IntPtr timer, IntPtr completionEvent);

		/// <summary>TimeBeginPeriod(). See the Windows API documentation for details.</summary>
		[SuppressUnmanagedCodeSecurity]
		[DllImport("winmm.dll", EntryPoint = "timeBeginPeriod", SetLastError = true)]
		[CLSCompliant(false)]
		public static extern uint TimeBeginPeriod(uint uMilliseconds);

		/// <summary>TimeEndPeriod(). See the Windows API documentation for details.</summary>
		[SuppressUnmanagedCodeSecurity]
		[DllImport("winmm.dll", EntryPoint = "timeEndPeriod", SetLastError = true)]
		[CLSCompliant(false)]
		public static extern uint TimeEndPeriod(uint uMilliseconds);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = false)]
		public static extern int RegisterWindowMessage(string msg);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr SetActiveWindow(IntPtr hWnd);

		[CLSCompliant(false)]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll")]
		public static extern bool SetLayeredWindowAttributes(IntPtr hwnd, COLORREF crKey, byte bAlpha, BlendFlags dwFlags);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetScrollInfo(IntPtr hwnd, int fnBar, ref SCROLLINFO lpsi);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll")]
		public static extern int SetScrollPos(IntPtr hWnd, int nBar, int nPos, bool bRedraw);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("gdi32.dll")]
		public static extern bool SwapBuffers(IntPtr hdc);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("gdi32.dll")]
		[CLSCompliant(false)]
		public static extern int DescribePixelFormat(IntPtr hdc, int ipfd, uint nBytes, [In] ref PixelFormatDescriptor pfd);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("gdi32.dll")]
		public static extern int ChoosePixelFormat(IntPtr dc, [In] ref PixelFormatDescriptor pfd);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("gdi32.dll")]
		public static extern bool SetPixelFormat(IntPtr dc, int format, [In] ref PixelFormatDescriptor pfd);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("kernel32.dll")]
		public static extern IntPtr GetProcAddress(IntPtr handle, string funcname);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll")]
		public static extern bool EnableWindow(IntPtr hWnd, bool enable);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("dwmapi.dll")]
		public static extern IntPtr DwmEnableBlurBehindWindow(IntPtr hwnd, ref DWM_BLURBEHIND blurBehind);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll")]
		public static extern IntPtr SetCursor(IntPtr hcur);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll")]
		public static extern IntPtr LoadCursor(IntPtr hInstcance, SystemCursor hcur);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll")]
		public static extern IntPtr GetForegroundWindow();

		[SuppressUnmanagedCodeSecurity]
		[DllImport("gdi32.dll")]
		public static extern bool SetWindowOrgEx(IntPtr hdc, int x, int y, IntPtr lppt);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("gdi32.dll")]
		public static extern bool SetViewportOrgEx(IntPtr hdc, int x, int y, IntPtr lppt);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll")]
		[Security.SuppressUnmanagedCodeSecurity]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool SetWindowPos(
			IntPtr handle,
			IntPtr insertAfter,
			int x, int y, int cx, int cy,
			SetWindowPosFlags flags
		);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("msvcrt.dll", EntryPoint = "memcpy", CallingConvention = CallingConvention.Cdecl, SetLastError = false, ExactSpelling = true)]
		[CLSCompliant(false)]
		public static extern IntPtr MemoryCopy(IntPtr dst, IntPtr src, UIntPtr count);

		/// <summary>
		/// Calculates the required size of the window rectangle, based on the desired client-rectangle size. The window rectangle can then be passed to the CreateWindow function to create a window whose client area is the desired size.
		/// </summary>
		/// <param name="lpRect">[in, out] Pointer to a RECT structure that contains the coordinates of the top-left and bottom-right corners of the desired client area. When the function returns, the structure contains the coordinates of the top-left and bottom-right corners of the window to accommodate the desired client area.</param>
		/// <param name="dwStyle">[in] Specifies the window style of the window whose required size is to be calculated. Note that you cannot specify the WS_OVERLAPPED style.</param>
		/// <param name="bMenu">[in] Specifies whether the window has a menu.</param>
		/// <returns>
		/// If the function succeeds, the return value is nonzero.
		/// If the function fails, the return value is zero. To get extended error information, call GetLastError.
		/// </returns>
		/// <remarks>
		/// A client rectangle is the smallest rectangle that completely encloses a client area. A window rectangle is the smallest rectangle that completely encloses the window, which includes the client area and the nonclient area. 
		/// The AdjustWindowRect function does not add extra space when a menu bar wraps to two or more rows. 
		/// The AdjustWindowRect function does not take the WS_VSCROLL or WS_HSCROLL styles into account. To account for the scroll bars, call the GetSystemMetrics function with SM_CXVSCROLL or SM_CYHSCROLL.
		/// Found Winuser.h, user32.dll
		/// </remarks>
		[SuppressUnmanagedCodeSecurity]
		[CLSCompliant(false)]
		[DllImport("user32.dll", SetLastError = true)]
		public static extern Boolean AdjustWindowRect([In, Out] ref Win32Rectangle lpRect, WindowStyle dwStyle, Boolean bMenu);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", EntryPoint = "AdjustWindowRectEx", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
		[CLSCompliant(false)]
		public static extern bool AdjustWindowRectEx(ref Win32Rectangle lpRect, WindowStyle dwStyle, bool bMenu, ExtendedWindowStyle dwExStyle);

		[CLSCompliant(false)]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool UnregisterHotKey(IntPtr hWnd, int id);





		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		[CLSCompliant(false)]
		public static extern IntPtr CreateWindowEx(
			ExtendedWindowStyle ExStyle,
			[MarshalAs(UnmanagedType.LPTStr)] string className,
			[MarshalAs(UnmanagedType.LPTStr)] string windowName,
			WindowStyle Style,
			int X, int Y,
			int Width, int Height,
			IntPtr HandleToParentWindow,
			IntPtr Menu,
			IntPtr Instance,
			IntPtr Param);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		[CLSCompliant(false)]
		public static extern IntPtr CreateWindowEx(
			ExtendedWindowStyle ExStyle,
			IntPtr ClassAtom,
			IntPtr WindowName,
			WindowStyle Style,
			int X, int Y,
			int Width, int Height,
			IntPtr HandleToParentWindow,
			IntPtr Menu,
			IntPtr Instance,
			IntPtr Param);



		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", SetLastError = true)]
		[Security.SuppressUnmanagedCodeSecurity]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool DestroyWindow(IntPtr windowHandle);





		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		[CLSCompliant(false)]
		public static extern ushort RegisterClass(ref WindowClass window_class);





		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		[CLSCompliant(false)]
		public static extern ushort RegisterClassEx(ref ExtendedWindowClass window_class);





		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern short UnregisterClass([MarshalAs(UnmanagedType.LPTStr)] String className, IntPtr instance);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern short UnregisterClass(IntPtr className, IntPtr instance);





		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern Boolean GetClassInfoEx(IntPtr hinst,
			[MarshalAs(UnmanagedType.LPTStr)] String lpszClass, ref ExtendedWindowClass lpwcx);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		[CLSCompliant(false)]
		public static extern Boolean GetClassInfoEx(IntPtr hinst, UIntPtr lpszClass, ref ExtendedWindowClass lpwcx);





		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", SetLastError = true)]
		[CLSCompliant(false)]
		public static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, WindowMessage Msg,
			IntPtr wParam, IntPtr lParam);



		[SuppressUnmanagedCodeSecurity]
		[DllImport("kernel32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool QueryPerformanceFrequency(out long PerformanceFrequency);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("kernel32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool QueryPerformanceCounter(out long PerformanceCount);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("gdi32.dll")]
		[SuppressUnmanagedCodeSecurity]
		public static extern IntPtr CreateCompatibleDC(IntPtr hDC);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("gdi32.dll")]
		public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("gdi32.dll")]
		public static extern bool DeleteObject(IntPtr hObject);

		[SuppressUnmanagedCodeSecurity]
		[CLSCompliant(false)]
		[DllImport("gdi32.dll")]
		public static extern bool BitBlt(
	  IntPtr hdcDest,     // handle to destination DC (device context)
	  int nXDest,         // x-coord of destination upper-left corner
	  int nYDest,         // y-coord of destination upper-left corner
	  int nWidth,         // width of destination rectangle
	  int nHeight,        // height of destination rectangle
	  IntPtr hdcSrc,      // handle to source DC
	  int nXSrc,          // x-coordinate of source upper-left corner
	  int nYSrc,          // y-coordinate of source upper-left corner
	  TernaryRasterOperations dwRop  // raster operation code
	  );

		[SuppressUnmanagedCodeSecurity]
		[DllImport("gdi32.dll")]
		public static extern bool DeleteDC(IntPtr hdc);

		[SuppressUnmanagedCodeSecurity]
		[CLSCompliant(false)]
		[DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
		public static extern bool UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst, ref Point pptDst,
			ref Size psize, IntPtr hdcSrc, ref Point pprSrc, int crKey, ref BLENDFUNCTION pblend, BlendFlags dwFlags);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
		public static extern bool UpdateLayeredWindowIndirect(IntPtr hwnd, ref UPDATELAYEREDWINDOWINFO updateInfo);

		/// <summary>
		/// Compatible with both 32-bit and 64-bit.
		/// </summary>
		[CLSCompliant(false)]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static UIntPtr SetWindowLong(IntPtr handle, GetWindowLongOffsets item, UIntPtr newValue) {
			if (UIntPtr.Size == 4)
				return new UIntPtr(SetWindowLong(handle, item, newValue.ToUInt32()));
			else
				return SetWindowLongPtr(handle, item, newValue);
		}

		/// <summary>
		/// Compatible with both 32-bit and 64-bit.
		/// </summary>
		[CLSCompliant(false)]
		public static unsafe UIntPtr SetWindowLong(IntPtr handle, WindowProcedure newValue) {
			return SetWindowLong(handle, GetWindowLongOffsets.WNDPROC, new UIntPtr(Marshal.GetFunctionPointerForDelegate(newValue).ToPointer()));
		}

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", SetLastError = true)]
		private static extern UInt32 SetWindowLong(IntPtr hWnd, GetWindowLongOffsets nIndex, UInt32 dwNewLong);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", SetLastError = true)]
		private static extern UIntPtr SetWindowLongPtr(IntPtr hWnd, GetWindowLongOffsets nIndex, UIntPtr dwNewLong);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", SetLastError = true)]
		private static extern UIntPtr SetWindowLongPtr(IntPtr hWnd, GetWindowLongOffsets nIndex, IntPtr dwNewLong);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", SetLastError = true)]
		private static extern Int32 SetWindowLong(IntPtr hWnd, GetWindowLongOffsets nIndex, [MarshalAs(UnmanagedType.FunctionPtr)]WindowProcedure dwNewLong);


		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", SetLastError = true)]
		private static extern IntPtr SetWindowLongPtr(IntPtr hWnd, GetWindowLongOffsets nIndex,[MarshalAs(UnmanagedType.FunctionPtr)]WindowProcedure dwNewLong);

		/// <summary>
		/// Compatible with both 32-bit and 64-bit.
		/// </summary>
		[CLSCompliant(false)]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static UIntPtr SetClassLong(IntPtr hWnd, ClassLongFlags nIndex, IntPtr dwNewLong) {
			if (UIntPtr.Size == 4)
				return new UIntPtr(SetClassLong(hWnd, (int) nIndex, dwNewLong.ToInt32()));
			else
				return SetClassLongPtr(hWnd, (int) nIndex, dwNewLong);
		}

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", SetLastError = true)]
		private static extern uint SetClassLong(IntPtr hWnd, int nIndex, int dwNewLong);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", SetLastError = true)]
		private static extern UIntPtr SetClassLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

		/// <summary>
		/// Compatible with both 32-bit and 64-bit.
		/// </summary>
		[CLSCompliant(false)]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static UIntPtr GetClassLong(IntPtr hWnd, ClassLongFlags nIndex) {
			if (UIntPtr.Size == 4)
				return new UIntPtr(GetClassLong(hWnd, (int) nIndex));
			else
				return GetClassLongPtr(hWnd, (int) nIndex);
		}

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", SetLastError = true)]
		private static extern uint GetClassLong(IntPtr hWnd, int nIndex);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", SetLastError = true)]
		private static extern UIntPtr GetClassLongPtr(IntPtr hWnd, int nIndex);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll")]
		public static extern IntPtr WindowFromPoint(Point pnt);

		[DllImport("user32.dll")]
		[SuppressUnmanagedCodeSecurity]
		[CLSCompliant(false)]
		public static extern int ToUnicode(uint virtualKeyCode, uint scanCode, byte[] keyboardState,
[Out, MarshalAs(UnmanagedType.LPWStr, SizeConst = 64)] StringBuilder receivingBuffer, int bufferSize, uint flags);

		[DllImport("user32.dll")]
		[SuppressUnmanagedCodeSecurity]
		[CLSCompliant(false)]
		public static extern unsafe int ToUnicode(uint virtualKeyCode, uint scanCode, byte* keyboardState,
[Out, MarshalAs(UnmanagedType.LPWStr, SizeConst = 64)] StringBuilder receivingBuffer, int bufferSize, uint flags);





		[CLSCompliant(false)]
		public static UIntPtr GetWindowLong(IntPtr handle, GetWindowLongOffsets index) {
			if (IntPtr.Size == 4)
				return (UIntPtr) GetWindowLongInternal(handle, index);

			return GetWindowLongPtrInternal(handle, index);
		}

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", SetLastError = true, EntryPoint = "GetWindowLong")]
		[CLSCompliant(false)]
		public static extern UInt32 GetWindowLongInternal(IntPtr hWnd, GetWindowLongOffsets nIndex);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", SetLastError = true, EntryPoint = "GetWindowLongPtr")]
		[CLSCompliant(false)]
		public static extern UIntPtr GetWindowLongPtrInternal(IntPtr hWnd, GetWindowLongOffsets nIndex);









		/// <summary>
		/// Low-level WINAPI function that checks the next message in the queue.
		/// </summary>
		/// <param name="msg">The pending message (if any) is stored here.</param>
		/// <param name="hWnd">Not used</param>
		/// <param name="messageFilterMin">Not used</param>
		/// <param name="messageFilterMax">Not used</param>
		/// <param name="flags">Not used</param>
		/// <returns>True if there is a message pending.</returns>
		[SuppressUnmanagedCodeSecurity]
		[DllImport("User32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool PeekMessage(ref MSG msg, IntPtr hWnd, int messageFilterMin, int messageFilterMax, int flags);





		/// <summary>
		/// Low-level WINAPI function that retrieves the next message in the queue.
		/// </summary>
		/// <param name="msg">The pending message (if any) is stored here.</param>
		/// <param name="windowHandle">Not used</param>
		/// <param name="messageFilterMin">Not used</param>
		/// <param name="messageFilterMax">Not used</param>
		/// <returns>
		/// Nonzero indicates that the function retrieves a message other than WM_QUIT.
		/// Zero indicates that the function retrieves the WM_QUIT message, or that lpMsg is an invalid pointer.
		/// 1 indicates that an error occurred  for example, the function fails if hWnd is an invalid window handle.
		/// To get extended error information, call GetLastError.
		/// </returns>
		[SuppressUnmanagedCodeSecurity]
		[DllImport("User32.dll")]
		//[return: MarshalAs(UnmanagedType.Bool)]
		public static extern Int32 GetMessage(ref MSG msg,
			IntPtr windowHandle, int messageFilterMin, int messageFilterMax);





		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		[CLSCompliant(false)]
		public static extern IntPtr SendMessage(IntPtr hWnd, WindowMessage Msg, IntPtr wParam, IntPtr lParam);

		[return: MarshalAs(UnmanagedType.Bool)]
		[DllImport("user32.dll", SetLastError = true)]
		[CLSCompliant(false)]
		public static extern bool SendNotifyMessage(IntPtr hWnd, WindowMessage Msg, IntPtr wParam, IntPtr lParam);





		[SuppressUnmanagedCodeSecurity]
		[DllImport("User32.dll", CharSet = CharSet.Auto)]
		[return: MarshalAs(UnmanagedType.Bool)]
		[CLSCompliant(false)]
		public static extern Boolean PostMessage(
			IntPtr hWnd,
			WindowMessage Msg,
			IntPtr wParam,
			IntPtr lParam
		);





		[SuppressUnmanagedCodeSecurity]
		[DllImport("User32.dll", CharSet = CharSet.Auto)]
		public static extern void PostQuitMessage(int exitCode);





		[SuppressUnmanagedCodeSecurity]
		[DllImport("User32.dll")]
		public static extern IntPtr DispatchMessage(ref MSG msg);





		[SuppressUnmanagedCodeSecurity]
		[DllImport("User32.dll")]
		public static extern Boolean TranslateMessage(ref MSG lpMsg);





		/// <summary>
		/// Indicates the type of messages found in the calling thread's message queue.
		/// </summary>
		/// <param name="flags"></param>
		/// <returns>
		/// The high-order word of the return value indicates the types of messages currently in the queue.
		/// The low-order word indicates the types of messages that have been added to the queue and that are still
		/// in the queue since the last call to the GetQueueStatus, GetMessage, or PeekMessage function.
		/// </returns>
		/// <remarks>
		/// The presence of a QS_ flag in the return value does not guarantee that
		/// a subsequent call to the GetMessage or PeekMessage function will return a message.
		/// GetMessage and PeekMessage perform some public filtering that may cause the message
		/// to be processed internally. For this reason, the return value from GetQueueStatus
		/// should be considered only a hint as to whether GetMessage or PeekMessage should be called. 
		/// <para>
		/// The QS_ALLPOSTMESSAGE and QS_POSTMESSAGE type differ in when they are cleared.
		/// QS_POSTMESSAGE is cleared when you call GetMessage or PeekMessage, whether or not you are filtering messages.
		/// QS_ALLPOSTMESSAGE is cleared when you call GetMessage or PeekMessage without filtering messages
		/// (wMsgFilterMin and wMsgFilterMax are 0). This can be useful when you call PeekMessage multiple times
		/// to get messages in different ranges.
		/// </para>
		/// </remarks>
		[SuppressUnmanagedCodeSecurity]
		[DllImport("User32.dll", CharSet = CharSet.Auto)]
		public static extern Int32 GetQueueStatus([MarshalAs(UnmanagedType.U4)] QueueStatusFlags flags);





		[SuppressUnmanagedCodeSecurity]
		[DllImport("User32.dll", CharSet = CharSet.Auto)]
		[CLSCompliant(false)]
		public extern static IntPtr DefWindowProc(IntPtr hWnd, WindowMessage msg, IntPtr wParam, IntPtr lParam);










		/// <summary>
		/// 
		/// </summary>
		/// <param name="hwnd"></param>
		/// <returns></returns>
		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll")]
		public static extern IntPtr GetDC(IntPtr hwnd);





		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll")]
		public static extern IntPtr GetWindowDC(IntPtr hwnd);





		/// <summary>
		/// 
		/// </summary>
		/// <param name="hwnd"></param>
		/// <param name="DC"></param>
		/// <returns></returns>
		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool ReleaseDC(IntPtr hwnd, IntPtr DC);



		[DllImport("gdi32.dll")]
		[SuppressUnmanagedCodeSecurity]
		[CLSCompliant(false)]
		public static extern uint GetGlyphOutline(IntPtr hdc, uint uChar, uint uFormat, out GLYPHMETRICS lpgm, uint cbBuffer, IntPtr lpvBuffer, ref MAT2 lpmat2);







		[SuppressUnmanagedCodeSecurity]
		[DllImport("kernel32.dll")]
		[CLSCompliant(false)]
		public static extern void SetLastError(uint dwErrCode);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("kernel32.dll")]
		[CLSCompliant(false)]
		public static extern uint GetLastError();

		[SuppressUnmanagedCodeSecurity]
		[DllImport("kernel32.dll")]
		public static extern IntPtr GetModuleHandle([MarshalAs(UnmanagedType.LPTStr)]string module_name);





		[SuppressUnmanagedCodeSecurity]
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern IntPtr LoadLibrary(string dllName);





		/// <summary>
		/// 
		/// </summary>
		/// <param name="handle"></param>
		/// <returns></returns>
		[SuppressUnmanagedCodeSecurity]
		[DllImport("kernel32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool FreeLibrary(IntPtr handle);







		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", SetLastError = true)]
		public static extern Int16 GetAsyncKeyState(Keys vKey);





		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", SetLastError = true)]
		public static extern Int16 GetKeyState(Keys vKey);





		[CLSCompliant(false)]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", SetLastError = true)]
		public static extern UInt32 MapVirtualKey(UInt32 uCode, MapVirtualKeyType uMapType);

		[CLSCompliant(false)]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", SetLastError = true)]
		public static extern UInt32 MapVirtualKey(Keys vkey, MapVirtualKeyType uMapType);





		/// <summary>
		/// The ShowWindow function sets the specified window's show state.
		/// </summary>
		/// <param name="hWnd">[in] Handle to the window.</param>
		/// <param name="nCmdShow">[in] Specifies how the window is to be shown. This parameter is ignored the first time an application calls ShowWindow, if the program that launched the application provides a STARTUPINFO structure. Otherwise, the first time ShowWindow is called, the value should be the value obtained by the WinMain function in its nCmdShow parameter. In subsequent calls, this parameter can be one of the ShowWindowEnum values.</param>
		/// <returns>If the window was previously visible, the return value is true. Otherwise false.</returns>
		/// <remarks>
		/// <para>To perform certain special effects when showing or hiding a window, use AnimateWindow.</para>
		/// <para>The first time an application calls ShowWindow, it should use the WinMain function's nCmdShow parameter as its nCmdShow parameter. Subsequent calls to ShowWindow must use one of the values in the given list, instead of the one specified by the WinMain function's nCmdShow parameter.</para>
		/// <para>As noted in the discussion of the nCmdShow parameter, the nCmdShow value is ignored in the first call to ShowWindow if the program that launched the application specifies startup information in the structure. In this case, ShowWindow uses the information specified in the STARTUPINFO structure to show the window. On subsequent calls, the application must call ShowWindow with nCmdShow set to SW_SHOWDEFAULT to use the startup information provided by the program that launched the application. This behavior is designed for the following situations:</para>
		/// <list type="">
		/// <item>Applications create their main window by calling CreateWindow with the WS_VISIBLE flag set.</item>
		/// <item>Applications create their main window by calling CreateWindow with the WS_VISIBLE flag cleared, and later call ShowWindow with the SW_SHOW flag set to make it visible.</item>
		/// </list>
		/// </remarks>
		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", SetLastError = true), SuppressUnmanagedCodeSecurity]
		public static extern Boolean ShowWindow(IntPtr hWnd, ShowWindowCommand nCmdShow);





		/// <summary>
		/// The SetWindowText function changes the text of the specified window's title bar (if it has one). If the specified window is a control, the text of the control is changed. However, SetWindowText cannot change the text of a control in another application.
		/// </summary>
		/// <param name="hWnd">[in] Handle to the window or control whose text is to be changed.</param>
		/// <param name="lpString">[in] Pointer to a null-terminated string to be used as the new title or control text.</param>
		/// <returns>
		/// <para>If the function succeeds, the return value is nonzero.</para>
		/// <para>If the function fails, the return value is zero. To get extended error information, call GetLastError.</para>
		/// </returns>
		/// <remarks>
		/// <para>If the target window is owned by the current process, SetWindowText causes a WM_SETTEXT message to be sent to the specified window or control. If the control is a list box control created with the WS_CAPTION style, however, SetWindowText sets the text for the control, not for the list box entries. </para>
		/// <para>To set the text of a control in another process, send the WM_SETTEXT message directly instead of calling SetWindowText. </para>
		/// <para>The SetWindowText function does not expand tab characters (ASCII code 0x09). Tab characters are displayed as vertical bar (|) characters. </para>
		/// <para>Windows 95/98/Me: SetWindowTextW is supported by the Microsoft Layer for Unicode (MSLU). To use this, you must add certain files to your application, as outlined in Microsoft Layer for Unicode on Windows 95/98/Me Systems .</para>
		/// </remarks>
		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		public static extern Boolean SetWindowText(IntPtr hWnd, [MarshalAs(UnmanagedType.LPTStr)] string lpString);





		/// <summary>
		/// The GetWindowText function copies the text of the specified window's title bar (if it has one) into a buffer. If the specified window is a control, the text of the control is copied. However, GetWindowText cannot retrieve the text of a control in another application.
		/// </summary>
		/// <param name="hWnd">[in] Handle to the window or control containing the text.</param>
		/// <param name="lpString">[out] Pointer to the buffer that will receive the text. If the string is as long or longer than the buffer, the string is truncated and terminated with a NULL character.</param>
		/// <param name="nMaxCount">[in] Specifies the maximum number of characters to copy to the buffer, including the NULL character. If the text exceeds this limit, it is truncated.</param>
		/// <returns>
		/// If the function succeeds, the return value is the length, in characters, of the copied string, not including the terminating NULL character. If the window has no title bar or text, if the title bar is empty, or if the window or control handle is invalid, the return value is zero. To get extended error information, call GetLastError.
		/// <para>This function cannot retrieve the text of an edit control in another application.</para>
		/// </returns>
		/// <remarks>
		/// <para>If the target window is owned by the current process, GetWindowText causes a WM_GETTEXT message to be sent to the specified window or control. If the target window is owned by another process and has a caption, GetWindowText retrieves the window caption text. If the window does not have a caption, the return value is a null string. This behavior is by design. It allows applications to call GetWindowText without becoming unresponsive if the process that owns the target window is not responding. However, if the target window is not responding and it belongs to the calling application, GetWindowText will cause the calling application to become unresponsive.</para>
		/// <para>To retrieve the text of a control in another process, send a WM_GETTEXT message directly instead of calling GetWindowText.</para>
		/// <para>Windows 95/98/Me: GetWindowTextW is supported by the Microsoft Layer for Unicode (MSLU). To use this, you must add certain files to your application, as outlined in Microsoft Layer for Unicode on Windows 95/98/Me</para>
		/// </remarks>
		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		public static extern int GetWindowText(IntPtr hWnd, [MarshalAs(UnmanagedType.LPTStr), In, Out] StringBuilder lpString, int nMaxCount);





		/// <summary>
		/// Converts the screen coordinates of a specified point on the screen to client-area coordinates.
		/// </summary>
		/// <param name="hWnd">Handle to the window whose client area will be used for the conversion.</param>
		/// <param name="point">Pointer to a POINT structure that specifies the screen coordinates to be converted.</param>
		/// <returns>If the function succeeds, the return value is nonzero. If the function fails, the return value is zero. Windows NT/2000/XP: To get extended error information, call GetLastError.</returns>
		/// <remarks>
		/// <para>The function uses the window identified by the hWnd parameter and the screen coordinates given in the POINT structure to compute client coordinates. It then replaces the screen coordinates with the client coordinates. The new coordinates are relative to the upper-left corner of the specified window's client area. </para>
		/// <para>The ScreenToClient function assumes the specified point is in screen coordinates. </para>
		/// <para>All coordinates are in device units.</para>
		/// <para>Do not use ScreenToClient when in a mirroring situation, that is, when changing from left-to-right layout to right-to-left layout. Instead, use MapWindowPoints. For more information, see "Window Layout and Mirroring" in Window Features.</para>
		/// </remarks>
		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", SetLastError = true)]
		//public static extern BOOL ScreenToClient(HWND hWnd, ref POINT point);
		public static extern Boolean ScreenToClient(IntPtr hWnd, ref Point point);





		/// <summary>
		/// Converts the client-area coordinates of a specified point to screen coordinates.
		/// </summary>
		/// <param name="hWnd">Handle to the window whose client area will be used for the conversion.</param>
		/// <param name="point">Pointer to a POINT structure that contains the client coordinates to be converted. The new screen coordinates are copied into this structure if the function succeeds.</param>
		/// <returns>If the function succeeds, the return value is nonzero. If the function fails, the return value is zero. Windows NT/2000/XP: To get extended error information, call GetLastError.</returns>
		/// <remarks>
		/// <para>The ClientToScreen function replaces the client-area coordinates in the POINT structure with the screen coordinates. The screen coordinates are relative to the upper-left corner of the screen. Note, a screen-coordinate point that is above the window's client area has a negative y-coordinate. Similarly, a screen coordinate to the left of a client area has a negative x-coordinate.</para>
		/// <para>All coordinates are device coordinates.</para>
		/// </remarks>
		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", SetLastError = true)]
		public static extern Boolean ClientToScreen(IntPtr hWnd, ref Point point);





		/// <summary>
		/// The GetClientRect function retrieves the coordinates of a window's client area. The client coordinates specify the upper-left and lower-right corners of the client area. Because client coordinates are relative to the upper-left corner of a window's client area, the coordinates of the upper-left corner are (0,0).
		/// </summary>
		/// <param name="windowHandle">Handle to the window whose client coordinates are to be retrieved.</param>
		/// <param name="clientRectangle">Pointer to a RECT structure that receives the client coordinates. The left and top members are zero. The right and bottom members contain the width and height of the window.</param>
		/// <returns>
		/// <para>If the function succeeds, the return value is nonzero.</para>
		/// <para>If the function fails, the return value is zero. To get extended error information, call GetLastError.</para>
		/// </returns>
		/// <remarks>In conformance with conventions for the RECT structure, the bottom-right coordinates of the returned rectangle are exclusive. In other words, the pixel at (right, bottom) lies immediately outside the rectangle.</remarks>
		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", SetLastError = true)]
		public extern static Boolean GetClientRect(IntPtr windowHandle, out Win32Rectangle clientRectangle);





		/// <summary>
		/// The GetWindowRect function retrieves the dimensions of the bounding rectangle of the specified window. The dimensions are given in screen coordinates that are relative to the upper-left corner of the screen.
		/// </summary>
		/// <param name="windowHandle">Handle to the window whose client coordinates are to be retrieved.</param>
		/// <param name="windowRectangle"> Pointer to a structure that receives the screen coordinates of the upper-left and lower-right corners of the window.</param>
		/// <returns>
		/// <para>If the function succeeds, the return value is nonzero.</para>
		/// <para>If the function fails, the return value is zero. To get extended error information, call GetLastError.</para>
		/// </returns>
		/// <remarks>In conformance with conventions for the RECT structure, the bottom-right coordinates of the returned rectangle are exclusive. In other words, the pixel at (right, bottom) lies immediately outside the rectangle.</remarks>
		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", SetLastError = true)]
		public extern static Boolean GetWindowRect(IntPtr windowHandle, out Win32Rectangle windowRectangle);





		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll")]
		public static extern Boolean GetWindowInfo(IntPtr hwnd, ref WindowInfo wi);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll")]
		public static extern IntPtr WindowFromDC(IntPtr hDC);





		[CLSCompliant(false)]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("dwmapi.dll")]
		public unsafe static extern IntPtr DwmGetWindowAttribute(IntPtr hwnd, DwmWindowAttribute dwAttribute, void* pvAttribute, Int32 cbAttribute);

		[SuppressUnmanagedCodeSecurity]
		[CLSCompliant(false)]
		[DllImport("gdi32.dll")]
		public static extern uint GetPixel(IntPtr hdc, int nXPos, int nYPos);

		[DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
		[SuppressUnmanagedCodeSecurity]
		public static extern IntPtr CreateRoundRectRgn
	(
		int nLeftRect, // x-coordinate of upper-left corner
		int nTopRect, // y-coordinate of upper-left corner
		int nRightRect, // x-coordinate of lower-right corner
		int nBottomRect, // y-coordinate of lower-right corner
		int nWidthEllipse, // height of ellipse
		int nHeightEllipse // width of ellipse
	 );

		[SuppressUnmanagedCodeSecurity]
		[DllImport("dwmapi.dll")]
		public static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMarInset);

		[DllImport("dwmapi.dll")]
		[SuppressUnmanagedCodeSecurity]
		public static extern int DwmSetWindowAttribute(IntPtr hwnd, DwmWindowAttribute attr, ref int attrValue, int attrSize);

		[DllImport("dwmapi.dll")]
		[SuppressUnmanagedCodeSecurity]
		public static extern int DwmIsCompositionEnabled(ref int pfEnabled);





		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll")]
		public static extern IntPtr GetFocus();





		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll")]
		public static extern bool IsWindowVisible(IntPtr intPtr);





		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll")]
		public static extern IntPtr LoadIcon(IntPtr hInstance, String lpIconName);





		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll")]
		public static extern IntPtr LoadCursor(IntPtr hInstance, String lpCursorName);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll")]
		public static extern IntPtr LoadCursor(IntPtr hInstance, IntPtr lpCursorName);

		public static IntPtr LoadCursor(CursorName lpCursorName) {
			return LoadCursor(IntPtr.Zero, new IntPtr((int) lpCursorName));
		}

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll")]
		[CLSCompliant(false)]
		public static extern int TrackPopupMenuEx(IntPtr hmenu, uint fuFlags, int x, int y, IntPtr hwnd, IntPtr lptpm);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll")]
		[CLSCompliant(false)]
		public static extern bool EnableMenuItem(IntPtr hMenu, SystemCommand uIDEnableItem, SysCommandState uEnable);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll")]
		[CLSCompliant(false)]
		public static extern bool EnableMenuItem(IntPtr hMenu, uint uIDEnableItem, uint uEnable);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		[SuppressUnmanagedCodeSecurity]
		[CLSCompliant(false)]
		public static extern bool AppendMenu(IntPtr hMenu, uint uFlags, uint uIDNewItem, string lpNewItem);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		[SuppressUnmanagedCodeSecurity]
		[CLSCompliant(false)]
		public static extern bool InsertMenu(IntPtr hMenu, uint uPosition, uint uFlags, uint uIDNewItem, string lpNewItem);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		[SuppressUnmanagedCodeSecurity]
		[CLSCompliant(false)]
		public static extern bool DeleteMenu(IntPtr hMenu, uint uPosition, uint uFlags);



		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", SetLastError = true)]
		public static extern Boolean SetForegroundWindow(IntPtr hWnd);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", SetLastError = true)]
		public static extern Boolean BringWindowToTop(IntPtr hWnd);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", SetLastError = true)]
		public static extern Boolean SetParent(IntPtr child, IntPtr newParent);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr RegisterDeviceNotification(IntPtr hRecipient,
			IntPtr NotificationFilter, DeviceNotification Flags);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", SetLastError = true)]
		public static extern Boolean UnregisterDeviceNotification(IntPtr Handle);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", ExactSpelling = true)]
		public static extern IntPtr MonitorFromWindow(IntPtr handle, int flags);







		/// <summary>
		/// The ChangeDisplaySettings function changes the settings of the default display device to the specified graphics mode.
		/// </summary>
		/// <param name="device_mode">[in] Pointer to a DEVMODE structure that describes the new graphics mode. If lpDevMode is NULL, all the values currently in the registry will be used for the display setting. Passing NULL for the lpDevMode parameter and 0 for the dwFlags parameter is the easiest way to return to the default mode after a dynamic mode change.</param>
		/// <param name="flags">[in] Indicates how the graphics mode should be changed.</param>
		/// <returns></returns>
		/// <remarks>To change the settings of a specified display device, use the ChangeDisplaySettingsEx function.
		/// <para>To ensure that the DEVMODE structure passed to ChangeDisplaySettings is valid and contains only values supported by the display driver, use the DEVMODE returned by the EnumDisplaySettings function.</para>
		/// <para>When the display mode is changed dynamically, the WM_DISPLAYCHANGE message is sent to all running applications.</para>
		/// </remarks>
		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", SetLastError = true)]
		public static extern int ChangeDisplaySettings(DeviceMode device_mode, ChangeDisplaySettingsEnum flags);


		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		public static extern Int32 ChangeDisplaySettingsEx([MarshalAs(UnmanagedType.LPTStr)] String lpszDeviceName,
			DeviceMode lpDevMode, IntPtr hwnd, ChangeDisplaySettingsEnum dwflags, IntPtr lParam);





		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern Boolean EnumDisplayDevices([MarshalAs(UnmanagedType.LPTStr)] String lpDevice,
			Int32 iDevNum, [In, Out] WindowsDisplayDevice lpDisplayDevice, Int32 dwFlags);





		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern Boolean EnumDisplaySettings([MarshalAs(UnmanagedType.LPTStr)] string device_name,
			int graphics_mode, [In, Out] DeviceMode device_mode);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern Boolean EnumDisplaySettings([MarshalAs(UnmanagedType.LPTStr)] string device_name,
			 DisplayModeSettingsEnum graphics_mode, [In, Out] DeviceMode device_mode);





		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		public static extern Boolean EnumDisplaySettingsEx([MarshalAs(UnmanagedType.LPTStr)] String lpszDeviceName, DisplayModeSettingsEnum iModeNum,
			[In, Out] DeviceMode lpDevMode, Int32 dwFlags);





		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", SetLastError = true)]
		public static extern Boolean GetMonitorInfo(IntPtr hMonitor, ref MonitorInfo lpmi);





		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr MonitorFromPoint(POINT pt, MonitorFrom dwFlags);





		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr MonitorFromWindow(IntPtr hwnd, MonitorFrom dwFlags);







		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", SetLastError = true)]
		public static extern Boolean TrackMouseEvent(ref TrackMouseEventStructure lpEventTrack);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
		public static extern bool ReleaseCapture();

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr SetCapture(IntPtr hwnd);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr GetCapture();

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr SetFocus(IntPtr hwnd);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int ShowCursor(bool show);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool ClipCursor(ref Win32Rectangle rcClip);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool ClipCursor(IntPtr rcClip);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll")]
		public static extern bool SetCursorPos(int X, int Y);





		/// <summary>
		/// Retrieves the cursor's position, in screen coordinates.
		/// </summary>
		/// <param name="point">Pointer to a POINT structure that receives the screen coordinates of the cursor.</param>
		/// <returns>Returns nonzero if successful or zero otherwise. To get extended error information, call GetLastError.</returns>
		/// <remarks>
		/// <para>The cursor position is always specified in screen coordinates and is not affected by the mapping mode of the window that contains the cursor.</para>
		/// <para>The calling process must have WINSTA_READATTRIBUTES access to the window station.</para>
		/// <para>The input desktop must be the current desktop when you call GetCursorPos. Call OpenInputDesktop to determine whether the current desktop is the input desktop. If it is not, call SetThreadDesktop with the HDESK returned by OpenInputDesktop to switch to that desktop.</para>
		/// </remarks>
		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", SetLastError = true), SuppressUnmanagedCodeSecurity]
		public static extern Boolean GetCursorPos(ref POINT point);









		/// <summary>
		/// calls the default raw input procedure to provide default processing for
		/// any raw input messages that an application does not process.
		/// This function ensures that every message is processed.
		/// DefRawInputProc is called with the same parameters received by the window procedure.
		/// </summary>
		/// <param name="RawInput">Pointer to an array of RawInput structures.</param>
		/// <param name="Input">Number of RawInput structures pointed to by paRawInput.</param>
		/// <param name="SizeHeader">Size, in bytes, of the RawInputHeader structure.</param>
		/// <returns>If successful, the function returns S_OK. Otherwise it returns an error value.</returns>
		[CLSCompliant(false)]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr DefRawInputProc(RawInput[] RawInput, Int32 Input, UInt32 SizeHeader);

		[CLSCompliant(false)]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", SetLastError = true)]
		public unsafe static extern IntPtr DefRawInputProc(ref RawInput RawInput, Int32 Input, UInt32 SizeHeader);

		[CLSCompliant(false)]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", SetLastError = true)]
		public unsafe static extern IntPtr DefRawInputProc(IntPtr RawInput, Int32 Input, UInt32 SizeHeader);





		/// <summary>
		/// Registers the devices that supply the raw input data.
		/// </summary>
		/// <param name="RawInputDevices">
		/// Pointer to an array of RawInputDevice structures that represent the devices that supply the raw input.
		/// </param>
		/// <param name="NumDevices">
		/// Number of RawInputDevice structures pointed to by RawInputDevices.
		/// </param>
		/// <param name="Size">
		/// Size, in bytes, of a RAWINPUTDEVICE structure.
		/// </param>
		/// <returns>
		/// TRUE if the function succeeds; otherwise, FALSE. If the function fails, call GetLastError for more information.
		/// </returns>
		[SuppressUnmanagedCodeSecurity]
		[CLSCompliant(false)]
		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern Boolean RegisterRawInputDevices(
			RawInputDevice[] RawInputDevices,
			UInt32 NumDevices,
			UInt32 Size
		);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern Boolean RegisterRawInputDevices(
			RawInputDevice[] RawInputDevices,
			Int32 NumDevices,
			Int32 Size
		);





		/// <summary>
		/// Does a buffered read of the raw input data.
		/// </summary>
		/// <param name="Data">
		/// Pointer to a buffer of RawInput structures that contain the raw input data.
		/// If NULL, the minimum required buffer, in bytes, is returned in Size.
		/// </param>
		/// <param name="Size">Pointer to a variable that specifies the size, in bytes, of a RawInput structure.</param>
		/// <param name="SizeHeader">Size, in bytes, of RawInputHeader.</param>
		/// <returns>
		/// If Data is NULL and the function is successful, the return value is zero.
		/// If Data is not NULL and the function is successful, the return value is the number
		/// of RawInput structures written to Data.
		/// If an error occurs, the return value is (UINT)-1. Call GetLastError for the error code.
		/// </returns>
		[CLSCompliant(false)]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", SetLastError = true)]
		public static extern UInt32 GetRawInputBuffer(
			[Out] RawInput[] Data,
			[In, Out] ref UInt32 Size,
			[In] UInt32 SizeHeader
		);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", SetLastError = true)]
		public static extern Int32 GetRawInputBuffer(
			[Out] RawInput[] Data,
			[In, Out] ref Int32 Size,
			[In] Int32 SizeHeader
		);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", SetLastError = true)]
		public static extern Int32 GetRawInputBuffer(
			[Out] IntPtr Data,
			[In, Out] ref Int32 Size,
			[In] Int32 SizeHeader
		);





		/// <summary>
		/// Gets the information about the raw input devices for the current application.
		/// </summary>
		/// <param name="RawInputDevices">
		/// Pointer to an array of RawInputDevice structures for the application.
		/// </param>
		/// <param name="NumDevices">
		/// Number of RawInputDevice structures in RawInputDevices.
		/// </param>
		/// <param name="cbSize">
		/// Size, in bytes, of a RawInputDevice structure.
		/// </param>
		/// <returns>
		/// <para>
		/// If successful, the function returns a non-negative number that is
		/// the number of RawInputDevice structures written to the buffer. 
		/// </para>
		/// <para>
		/// If the pRawInputDevices buffer is too small or NULL, the function sets
		/// the last error as ERROR_INSUFFICIENT_BUFFER, returns -1,
		/// and sets NumDevices to the required number of devices.
		/// </para>
		/// <para>
		/// If the function fails for any other reason, it returns -1. For more details, call GetLastError.
		/// </para>
		/// </returns>
		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", SetLastError = true)]
		[CLSCompliant(false)]
		public static extern UInt32 GetRegisteredRawInputDevices(
			[Out] RawInput[] RawInputDevices,
			[In, Out] ref UInt32 NumDevices,
			UInt32 cbSize
		);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", SetLastError = true)]
		public static extern Int32 GetRegisteredRawInputDevices(
			[Out] RawInput[] RawInputDevices,
			[In, Out] ref Int32 NumDevices,
			Int32 cbSize
		);





		/// <summary>
		/// Enumerates the raw input devices attached to the system.
		/// </summary>
		/// <param name="RawInputDeviceList">
		/// ointer to buffer that holds an array of RawInputDeviceList structures
		/// for the devices attached to the system.
		/// If NULL, the number of devices are returned in NumDevices.
		/// </param>
		/// <param name="NumDevices">
		/// Pointer to a variable. If RawInputDeviceList is NULL, it specifies the number
		/// of devices attached to the system. Otherwise, it contains the size, in bytes,
		/// of the preallocated buffer pointed to by pRawInputDeviceList.
		/// However, if NumDevices is smaller than needed to contain RawInputDeviceList structures,
		/// the required buffer size is returned here.
		/// </param>
		/// <param name="Size">
		/// Size of a RawInputDeviceList structure.
		/// </param>
		/// <returns>
		/// If the function is successful, the return value is the number of devices stored in the buffer
		/// pointed to by RawInputDeviceList.
		/// If RawInputDeviceList is NULL, the return value is zero. 
		/// If NumDevices is smaller than needed to contain all the RawInputDeviceList structures,
		/// the return value is (UINT) -1 and the required buffer is returned in NumDevices.
		/// Calling GetLastError returns ERROR_INSUFFICIENT_BUFFER.
		/// On any other error, the function returns (UINT) -1 and GetLastError returns the error indication.
		/// </returns>
		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", SetLastError = true)]
		[CLSCompliant(false)]
		public static extern UInt32 GetRawInputDeviceList(
			[In, Out] RawInputDeviceList[] RawInputDeviceList,
			[In, Out] ref UInt32 NumDevices,
			UInt32 Size
		);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", SetLastError = true)]
		public static extern Int32 GetRawInputDeviceList(
			[In, Out] RawInputDeviceList[] RawInputDeviceList,
			[In, Out] ref Int32 NumDevices,
			Int32 Size
		);

		/// <summary>
		/// Enumerates the raw input devices attached to the system.
		/// </summary>
		/// <param name="RawInputDeviceList">
		/// ointer to buffer that holds an array of RawInputDeviceList structures
		/// for the devices attached to the system.
		/// If NULL, the number of devices are returned in NumDevices.
		/// </param>
		/// <param name="NumDevices">
		/// Pointer to a variable. If RawInputDeviceList is NULL, it specifies the number
		/// of devices attached to the system. Otherwise, it contains the size, in bytes,
		/// of the preallocated buffer pointed to by pRawInputDeviceList.
		/// However, if NumDevices is smaller than needed to contain RawInputDeviceList structures,
		/// the required buffer size is returned here.
		/// </param>
		/// <param name="Size">
		/// Size of a RawInputDeviceList structure.
		/// </param>
		/// <returns>
		/// If the function is successful, the return value is the number of devices stored in the buffer
		/// pointed to by RawInputDeviceList.
		/// If RawInputDeviceList is NULL, the return value is zero. 
		/// If NumDevices is smaller than needed to contain all the RawInputDeviceList structures,
		/// the return value is (UINT) -1 and the required buffer is returned in NumDevices.
		/// Calling GetLastError returns ERROR_INSUFFICIENT_BUFFER.
		/// On any other error, the function returns (UINT) -1 and GetLastError returns the error indication.
		/// </returns>
		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", SetLastError = true)]
		[CLSCompliant(false)]
		public static extern UInt32 GetRawInputDeviceList(
			[In, Out] IntPtr RawInputDeviceList,
			[In, Out] ref UInt32 NumDevices,
			UInt32 Size
		);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", SetLastError = true)]
		public static extern Int32 GetRawInputDeviceList(
			[In, Out] IntPtr RawInputDeviceList,
			[In, Out] ref Int32 NumDevices,
			Int32 Size
		);





		/// <summary>
		/// Gets information about the raw input device.
		/// </summary>
		/// <param name="Device">
		/// Handle to the raw input device. This comes from the lParam of the WM_INPUT message,
		/// from RawInputHeader.Device, or from GetRawInputDeviceList.
		/// It can also be NULL if an application inserts input data, for example, by using SendInput.
		/// </param>
		/// <param name="Command">
		/// Specifies what data will be returned in pData. It can be one of the following values. 
		/// RawInputDeviceInfoEnum.PREPARSEDDATA
		/// Data points to the previously parsed data.
		/// RawInputDeviceInfoEnum.DEVICENAME
		/// Data points to a string that contains the device name. 
		/// For this Command only, the value in Size is the character count (not the byte count).
		/// RawInputDeviceInfoEnum.DEVICEINFO
		/// Data points to an RawInputDeviceInfo structure.
		/// </param>
		/// <param name="Data">
		/// ointer to a buffer that contains the information specified by Command.
		/// If Command is RawInputDeviceInfoEnum.DEVICEINFO, set RawInputDeviceInfo.Size to sizeof(RawInputDeviceInfo)
		/// before calling GetRawInputDeviceInfo. (This is done automatically in System)
		/// </param>
		/// <param name="Size">
		/// Pointer to a variable that contains the size, in bytes, of the data in Data.
		/// </param>
		/// <returns>
		/// <para>If successful, this function returns a non-negative number indicating the number of bytes copied to Data.</para>
		/// <para>If Data is not large enough for the data, the function returns -1. If Data is NULL, the function returns a value of zero. In both of these cases, Size is set to the minimum size required for the Data buffer.</para>
		/// <para>Call GetLastError to identify any other errors.</para>
		/// </returns>
		[CLSCompliant(false)]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", SetLastError = true)]
		public static extern UInt32 GetRawInputDeviceInfo(
			IntPtr Device,
			[MarshalAs(UnmanagedType.U4)] RawInputDeviceInfoEnum Command,
			[In, Out] IntPtr Data,
			[In, Out] ref UInt32 Size
		);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", SetLastError = true)]
		public static extern Int32 GetRawInputDeviceInfo(
			IntPtr Device,
			[MarshalAs(UnmanagedType.U4)] RawInputDeviceInfoEnum Command,
			[In, Out] IntPtr Data,
			[In, Out] ref Int32 Size
		);

		/// <summary>
		/// Gets information about the raw input device.
		/// </summary>
		/// <param name="Device">
		/// Handle to the raw input device. This comes from the lParam of the WM_INPUT message,
		/// from RawInputHeader.Device, or from GetRawInputDeviceList.
		/// It can also be NULL if an application inserts input data, for example, by using SendInput.
		/// </param>
		/// <param name="Command">
		/// Specifies what data will be returned in pData. It can be one of the following values. 
		/// RawInputDeviceInfoEnum.PREPARSEDDATA
		/// Data points to the previously parsed data.
		/// RawInputDeviceInfoEnum.DEVICENAME
		/// Data points to a string that contains the device name. 
		/// For this Command only, the value in Size is the character count (not the byte count).
		/// RawInputDeviceInfoEnum.DEVICEINFO
		/// Data points to an RawInputDeviceInfo structure.
		/// </param>
		/// <param name="Data">
		/// ointer to a buffer that contains the information specified by Command.
		/// If Command is RawInputDeviceInfoEnum.DEVICEINFO, set RawInputDeviceInfo.Size to sizeof(RawInputDeviceInfo)
		/// before calling GetRawInputDeviceInfo. (This is done automatically in System)
		/// </param>
		/// <param name="Size">
		/// Pointer to a variable that contains the size, in bytes, of the data in Data.
		/// </param>
		/// <returns>
		/// <para>If successful, this function returns a non-negative number indicating the number of bytes copied to Data.</para>
		/// <para>If Data is not large enough for the data, the function returns -1. If Data is NULL, the function returns a value of zero. In both of these cases, Size is set to the minimum size required for the Data buffer.</para>
		/// <para>Call GetLastError to identify any other errors.</para>
		/// </returns>
		[CLSCompliant(false)]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", SetLastError = true)]
		public static extern UInt32 GetRawInputDeviceInfo(
			IntPtr Device,
			[MarshalAs(UnmanagedType.U4)] RawInputDeviceInfoEnum Command,
			[In, Out] RawInputDeviceInfo Data,
			[In, Out] ref UInt32 Size
		);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", SetLastError = true)]
		public static extern Int32 GetRawInputDeviceInfo(
			IntPtr Device,
			[MarshalAs(UnmanagedType.U4)] RawInputDeviceInfoEnum Command,
			[In, Out] RawInputDeviceInfo Data,
			[In, Out] ref Int32 Size
		);






		/// <summary>
		/// Gets the raw input from the specified device.
		/// </summary>
		/// <param name="RawInput">Handle to the RawInput structure. This comes from the lParam in WM_INPUT.</param>
		/// <param name="Command">
		/// Command flag. This parameter can be one of the following values. 
		/// RawInputDateEnum.INPUT
		/// Get the raw data from the RawInput structure.
		/// RawInputDateEnum.HEADER
		/// Get the header information from the RawInput structure.
		/// </param>
		/// <param name="Data">Pointer to the data that comes from the RawInput structure. This depends on the value of uiCommand. If Data is NULL, the required size of the buffer is returned in Size.</param>
		/// <param name="Size">Pointer to a variable that specifies the size, in bytes, of the data in Data.</param>
		/// <param name="SizeHeader">Size, in bytes, of RawInputHeader.</param>
		/// <returns>
		/// <para>If Data is NULL and the function is successful, the return value is 0. If Data is not NULL and the function is successful, the return value is the number of bytes copied into Data.</para>
		/// <para>If there is an error, the return value is (UINT)-1.</para>
		/// </returns>
		/// <remarks>
		/// GetRawInputData gets the raw input one RawInput structure at a time. In contrast, GetRawInputBuffer gets an array of RawInput structures.
		/// </remarks>
		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", SetLastError = true)]
		public static extern Int32 GetRawInputData(
			IntPtr RawInput,
			GetRawInputDataEnum Command,
			[Out] IntPtr Data,
			[In, Out] ref Int32 Size,
			Int32 SizeHeader
		);

		/// <summary>
		/// Gets the raw input from the specified device.
		/// </summary>
		/// <param name="RawInput">Handle to the RawInput structure. This comes from the lParam in WM_INPUT.</param>
		/// <param name="Command">
		/// Command flag. This parameter can be one of the following values. 
		/// RawInputDateEnum.INPUT
		/// Get the raw data from the RawInput structure.
		/// RawInputDateEnum.HEADER
		/// Get the header information from the RawInput structure.
		/// </param>
		/// <param name="Data">Pointer to the data that comes from the RawInput structure. This depends on the value of uiCommand. If Data is NULL, the required size of the buffer is returned in Size.</param>
		/// <param name="Size">Pointer to a variable that specifies the size, in bytes, of the data in Data.</param>
		/// <param name="SizeHeader">Size, in bytes, of RawInputHeader.</param>
		/// <returns>
		/// <para>If Data is NULL and the function is successful, the return value is 0. If Data is not NULL and the function is successful, the return value is the number of bytes copied into Data.</para>
		/// <para>If there is an error, the return value is (UINT)-1.</para>
		/// </returns>
		/// <remarks>
		/// GetRawInputData gets the raw input one RawInput structure at a time. In contrast, GetRawInputBuffer gets an array of RawInput structures.
		/// </remarks>
		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", SetLastError = true)]
		public static extern Int32 GetRawInputData(
			IntPtr RawInput,
			GetRawInputDataEnum Command,
			/*[MarshalAs(UnmanagedType.LPStruct)]*/ [Out] out RawInput Data,
			[In, Out] ref Int32 Size,
			Int32 SizeHeader
		);

		[CLSCompliant(false)]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", SetLastError = true)]
		public unsafe static extern Int32 GetRawInputData(
			IntPtr RawInput,
			GetRawInputDataEnum Command,
			RawInput* Data,
			[In, Out] ref Int32 Size,
			Int32 SizeHeader
		);





		/* From winuser.h
        
        #define RAWINPUT_ALIGN(x)   (((x) + sizeof(QWORD) - 1) & ~(sizeof(QWORD) - 1))
        #else   // _WIN64
        #define RAWINPUT_ALIGN(x)   (((x) + sizeof(DWORD) - 1) & ~(sizeof(DWORD) - 1))
          // _WIN64

        #define NEXTRAWINPUTBLOCK(ptr) ((PRAWINPUT)RAWINPUT_ALIGN((ULONG_PTR)((PBYTE)(ptr) + (ptr)->header.dwSize)))
        */

		public static IntPtr NextRawInputStructure(IntPtr data) {
			unsafe
			{
				return RawInputAlign((IntPtr) (((byte*) data) + RawInputHeader.StructSize));
			}
		}

		public static IntPtr RawInputAlign(IntPtr data) {
			unsafe
			{
				return (IntPtr) (((byte*) data) + ((IntPtr.Size - 1) & ~(IntPtr.Size - 1)));
			}
		}












		[SuppressUnmanagedCodeSecurity]
		[DllImport("gdi32.dll", SetLastError = true)]
		public static extern IntPtr GetStockObject(int index);







		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", SetLastError = true)]
		[CLSCompliant(false)]
		public static extern UIntPtr SetTimer(IntPtr hWnd, UIntPtr nIDEvent, UInt32 uElapse, IntPtr lpTimerFunc);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", SetLastError = true)]
		[CLSCompliant(false)]
		public static extern Boolean KillTimer(IntPtr hWnd, UIntPtr uIDEvent);

		[CLSCompliant(false)]
		[UnmanagedFunctionPointer(CallingConvention.Winapi)]
		public delegate void TimerProc(IntPtr hwnd, WindowMessage uMsg, UIntPtr idEvent, Int32 dwTime);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll")]
		public static extern IntPtr SetClipboardViewer(IntPtr hWnd);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll")]
		public static extern bool ChangeClipboardChain(IntPtr hWndRemove, IntPtr hWndNewNext);

		[SuppressUnmanagedCodeSecurity]
		[CLSCompliant(false)]
		[DllImport("user32.dll")]
		public static extern int ToAscii(uint uVirtKey, uint uScanCode, byte[] lpKeyState, [Out] StringBuilder lpChar, uint uFlags);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		[CLSCompliant(false)]
		public static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		[CLSCompliant(false)]
		public static extern IntPtr SetWindowsHookEx(int idHook, LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool UnhookWindowsHookEx(IntPtr hhk);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, ref KBDLLHOOKSTRUCT lParam);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		[CLSCompliant(false)]
		public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, ref MSLLHOOKSTRUCT lParam);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "mouse_event", CallingConvention = CallingConvention.StdCall)]
		[CLSCompliant(false)]
		public static extern void MouseEvent(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);
		public delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, ref KBDLLHOOKSTRUCT lParam);

		public delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, ref MSLLHOOKSTRUCT lParam);

		[DllImport("kernel32.dll", CharSet = CharSet.Ansi, EntryPoint = "GetStartupInfoA", SetLastError = true)]
		public static extern bool GetStartupInfo(out STARTUPINFO info);

	}

	/// <summary>
	/// Location of cursor hot spot returnet in WM_NCHITTEST.
	/// </summary>
	public enum NCHITTEST {
		/// <summary>
		/// On the screen background or on a dividing line between windows 
		/// (same as HTNOWHERE, except that the DefWindowProc function produces a system beep to indicate an error).
		/// </summary>
		HTERROR = (-2),
		/// <summary>
		/// In a window currently covered by another window in the same thread 
		/// (the message will be sent to underlying windows in the same thread until one of them returns a code that is not HTTRANSPARENT).
		/// </summary>
		HTTRANSPARENT = (-1),
		/// <summary>
		/// On the screen background or on a dividing line between windows.
		/// </summary>
		HTNOWHERE = 0,
		/// <summary>In a client area.</summary>
		HTCLIENT = 1,
		/// <summary>In a title bar.</summary>
		HTCAPTION = 2,
		/// <summary>In a window menu or in a Close button in a child window.</summary>
		HTSYSMENU = 3,
		/// <summary>In a size box (same as HTSIZE).</summary>
		HTGROWBOX = 4,
		/// <summary>In a menu.</summary>
		HTMENU = 5,
		/// <summary>In a horizontal scroll bar.</summary>
		HTHSCROLL = 6,
		/// <summary>In the vertical scroll bar.</summary>
		HTVSCROLL = 7,
		/// <summary>In a Minimize button.</summary>
		HTMINBUTTON = 8,
		/// <summary>In a Maximize button.</summary>
		HTMAXBUTTON = 9,
		/// <summary>In the left border of a resizable window 
		/// (the user can click the mouse to resize the window horizontally).</summary>
		HTLEFT = 10,
		/// <summary>
		/// In the right border of a resizable window 
		/// (the user can click the mouse to resize the window horizontally).
		/// </summary>
		HTRIGHT = 11,
		/// <summary>In the upper-horizontal border of a window.</summary>
		HTTOP = 12,
		/// <summary>In the upper-left corner of a window border.</summary>
		HTTOPLEFT = 13,
		/// <summary>In the upper-right corner of a window border.</summary>
		HTTOPRIGHT = 14,
		/// <summary>	In the lower-horizontal border of a resizable window 
		/// (the user can click the mouse to resize the window vertically).</summary>
		HTBOTTOM = 15,
		/// <summary>In the lower-left corner of a border of a resizable window 
		/// (the user can click the mouse to resize the window diagonally).</summary>
		HTBOTTOMLEFT = 16,
		/// <summary>	In the lower-right corner of a border of a resizable window 
		/// (the user can click the mouse to resize the window diagonally).</summary>
		HTBOTTOMRIGHT = 17,
		/// <summary>In the border of a window that does not have a sizing border.</summary>
		HTBORDER = 18,

		HTOBJECT = 19,
		/// <summary>In a Close button.</summary>
		HTCLOSE = 20,
		/// <summary>In a Help button.</summary>
		HTHELP = 21,
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct FIXED {
		public short fract;
		public short value;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct MAT2 {
		[MarshalAs(UnmanagedType.Struct)]
		public FIXED eM11;
		[MarshalAs(UnmanagedType.Struct)]
		public FIXED eM12;
		[MarshalAs(UnmanagedType.Struct)]
		public FIXED eM21;
		[MarshalAs(UnmanagedType.Struct)]
		public FIXED eM22;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct POINTFX {
		[MarshalAs(UnmanagedType.Struct)]
		public FIXED x;
		[MarshalAs(UnmanagedType.Struct)]
		public FIXED y;
	}

	public struct HeldButtons {
		public bool Left;
		public bool Middle;
		public bool Right;
		public bool XBtn;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct GLYPHMETRICS {
		public int gmBlackBoxX;
		public int gmBlackBoxY;
		[MarshalAs(UnmanagedType.Struct)]
		public POINT gmptGlyphOrigin;
		[MarshalAs(UnmanagedType.Struct)]
		public POINTFX gmptfxGlyphOrigin;
		public short gmCellIncX;
		public short gmCellIncY;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct MARGINS {
		public int leftWidth;
		public int rightWidth;
		public int topHeight;
		public int bottomHeight;
	}

	public struct KBDLLHOOKSTRUCT {
		public int vkCode;
		public int scanCode;
		public int flags;
		public int time;
		public int dwExtraInfo;
	}



	public static class Constants {
		// Found in winuser.h
		public const int KEYBOARD_OVERRUN_MAKE_CODE = 0xFF;

		// WM_ACTIVATE state values (found in winuser.h)
		public const int WA_INACTIVE = 0;
		public const int WA_ACTIVE = 1;
		public const int WA_CLICKACTIVE = 2;

		// Pixel types (found in wingdi.h)
		public const byte PFD_TYPE_RGBA = 0;
		public const byte PFD_TYPE_COLORINDEX = 1;

		// Layer types (found in wingdi.h)
		public const byte PFD_MAIN_PLANE = 0;
		public const byte PFD_OVERLAY_PLANE = 1;
		public const byte PFD_UNDERLAY_PLANE = unchecked((byte) -1);

		// Device mode types (found in wingdi.h)
		public const int DM_BITSPERPEL = 0x00040000;
		public const int DM_PELSWIDTH = 0x00080000;
		public const int DM_PELSHEIGHT = 0x00100000;
		public const int DM_DISPLAYFLAGS = 0x00200000;
		public const int DM_DISPLAYFREQUENCY = 0x00400000;

		// ChangeDisplaySettings results (found in winuser.h)
		public const int DISP_CHANGE_SUCCESSFUL = 0;
		public const int DISP_CHANGE_RESTART = 1;
		public const int DISP_CHANGE_FAILED = -1;

		// (found in winuser.h)
		public const int ENUM_REGISTRY_SETTINGS = -2;
		public const int ENUM_CURRENT_SETTINGS = -1;

		public static readonly IntPtr MESSAGE_ONLY = new IntPtr(-3);
	}





	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
	public struct STARTUPINFO {
		[CLSCompliant(false)]
		public uint cb;
		public string lpReserved;
		public string lpDesktop;
		public string lpTitle;
		[CLSCompliant(false)]
		public uint dwX;
		[CLSCompliant(false)]
		public uint dwY;
		[CLSCompliant(false)]
		public uint dwXSize;
		[CLSCompliant(false)]
		public uint dwYSize;
		[CLSCompliant(false)]
		public uint dwXCountChars;
		[CLSCompliant(false)]
		public uint dwYCountChars;
		[CLSCompliant(false)]
		public uint dwFillAttribute;
		[CLSCompliant(false)]
		public uint dwFlags;
		/// <summary>
		/// ShowWindowCommand flags
		/// </summary>
		[CLSCompliant(false)]
		public ushort wShowWindow;
		[CLSCompliant(false)]
		public ushort cbReserved2;
		public IntPtr lpReserved2;
		public IntPtr hStdInput;
		public IntPtr hStdOutput;
		public IntPtr hStdError;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct MSLLHOOKSTRUCT {
		public POINT pt;
		[CLSCompliant(false)]
		public uint mouseData;
		[CLSCompliant(false)]
		public uint flags;
		[CLSCompliant(false)]
		public uint time;
		public IntPtr dwExtraInfo;
	}



	public struct CreateStruct {
		/// <summary>
		/// Contains additional data which may be used to create the window.
		/// </summary>
		/// <remarks>
		///  If the window is being created as a result of a call to the CreateWindow
		///  or CreateWindowEx function, this member contains the value of the lpParam 
		///  parameter specified in the function call.
		///  <para>
		/// If the window being created is a multiple-document interface (MDI) client window,
		/// this member contains a pointer to a CLIENTCREATESTRUCT structure. If the window
		/// being created is a MDI child window, this member contains a pointer to an 
		/// MDICREATESTRUCT structure.
		///  </para>
		/// <para>
		/// Windows NT/2000/XP: If the window is being created from a dialog template,
		/// this member is the address of a SHORT value that specifies the size, in bytes,
		/// of the window creation data. The value is immediately followed by the creation data.
		/// </para>
		/// <para>
		/// Windows NT/2000/XP: You should access the data represented by the lpCreateParams member
		/// using a pointer that has been declared using the UNALIGNED type, because the pointer
		/// may not be DWORD aligned.
		/// </para>
		/// </remarks>
		public IntPtr lpCreateParams;
		/// <summary>
		/// Handle to the module that owns the new window.
		/// </summary>
		public IntPtr hInstance;
		/// <summary>
		/// Handle to the menu to be used by the new window.
		/// </summary>
		public IntPtr hMenu;
		/// <summary>
		/// Handle to the parent window, if the window is a child window.
		/// If the window is owned, this member identifies the owner window.
		/// If the window is not a child or owned window, this member is NULL.
		/// </summary>
		public IntPtr hwndParent;
		/// <summary>
		/// Specifies the height of the new window, in pixels.
		/// </summary>
		public int cy;
		/// <summary>
		/// Specifies the width of the new window, in pixels.
		/// </summary>
		public int cx;
		/// <summary>
		/// Specifies the y-coordinate of the upper left corner of the new window.
		/// If the new window is a child window, coordinates are relative to the parent window.
		/// Otherwise, the coordinates are relative to the screen origin.
		/// </summary>
		public int y;
		/// <summary>
		/// Specifies the x-coordinate of the upper left corner of the new window.
		/// If the new window is a child window, coordinates are relative to the parent window.
		/// Otherwise, the coordinates are relative to the screen origin.
		/// </summary>
		public int x;
		/// <summary>
		/// Specifies the style for the new window.
		/// </summary>
		public Int32 style;
		/// <summary>
		/// Pointer to a null-terminated string that specifies the name of the new window.
		/// </summary>
		[MarshalAs(UnmanagedType.LPTStr)]
		public String lpszName;
		/// <summary>
		/// Either a pointer to a null-terminated string or an atom that specifies the public class name
		/// of the new window.
		/// <remarks>
		/// Note  Because the lpszClass member can contain a pointer to a local (and thus inaccessable) atom,
		/// do not obtain the public class name by using this member. Use the GetClassName function instead.
		/// </remarks>
		/// </summary>
		[MarshalAs(UnmanagedType.LPTStr)]
		public String lpszClass;
		/// <summary>
		/// Specifies the extended window style for the new window.
		/// </summary>
		public Int32 dwExStyle;
	}




	/*
    typedef public struct _devicemode { 
      BCHAR  dmDeviceName[CCHDEVICENAME]; 
      WORD   dmSpecVersion; 
      WORD   dmDriverVersion; 
      WORD   dmSize; 
      WORD   dmDriverExtra; 
      DWORD  dmFields; 
      union {
        public struct {
          short dmOrientation;
          short dmPaperSize;
          short dmPaperLength;
          short dmPaperWidth;
          short dmScale; 
          short dmCopies; 
          short dmDefaultSource; 
          short dmPrintQuality; 
        };
        POINTL dmPosition;
        DWORD  dmDisplayOrientation;
        DWORD  dmDisplayFixedOutput;
      };

      short  dmColor; 
      short  dmDuplex; 
      short  dmYResolution; 
      short  dmTTOption; 
      short  dmCollate; 
      BYTE  dmFormName[CCHFORMNAME]; 
      WORD  dmLogPixels; 
      DWORD  dmBitsPerPel; 
      DWORD  dmPelsWidth; 
      DWORD  dmPelsHeight; 
      union {
        DWORD  dmDisplayFlags; 
        DWORD  dmNup;
      }
      DWORD  dmDisplayFrequency; 
    
      DWORD  dmICMMethod;
      DWORD  dmICMIntent;
      DWORD  dmMediaType;
      DWORD  dmDitherType;
      DWORD  dmReserved1;
      DWORD  dmReserved2;
    
      DWORD  dmPanningWidth;
      DWORD  dmPanningHeight;
    
     
    } DEVMODE; 
    */
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	public sealed class DeviceMode {
		private static short SizeOf = (short) Marshal.SizeOf(typeof(DeviceMode));
		public DeviceMode() {
			Size = SizeOf;
		}

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string DeviceName;
		public short SpecVersion;
		public short DriverVersion;
		public short Size;
		public short DriverExtra;
		public int Fields;

		//public short Orientation;
		//public short PaperSize;
		//public short PaperLength;
		//public short PaperWidth;
		//public short Scale;
		//public short Copies;
		//public short DefaultSource;
		//public short PrintQuality;

		public POINT Position;
		public Int32 DisplayOrientation;
		public Int32 DisplayFixedOutput;

		public short Color;
		public short Duplex;
		public short YResolution;
		public short TTOption;
		public short Collate;
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string FormName;
		public short LogPixels;
		public int BitsPerPel;
		public int PelsWidth;
		public int PelsHeight;
		public int DisplayFlags;
		public int DisplayFrequency;
		public int ICMMethod;
		public int ICMIntent;
		public int MediaType;
		public int DitherType;
		public int Reserved1;
		public int Reserved2;
		public int PanningWidth;
		public int PanningHeight;
	}

	/// \public
	/// <summary>
	/// The DISPLAY_DEVICE structure receives information about the display device specified by the iDevNum parameter of the EnumDisplayDevices function.
	/// </summary>
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	public sealed class WindowsDisplayDevice {
		private static short SizeOf = (short) Marshal.SizeOf(typeof(WindowsDisplayDevice));

		public WindowsDisplayDevice() {
			size = SizeOf;
		}
		Int32 size;
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string DeviceName;
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string DeviceString;
		public DisplayDeviceStateFlags StateFlags;    // DWORD
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string DeviceID;
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string DeviceKey;
	}






	[StructLayout(LayoutKind.Sequential)]
	public struct WindowClass {
		public ClassStyle Style;
		[MarshalAs(UnmanagedType.FunctionPtr)]
		[CLSCompliant(false)]
		public WindowProcedure WindowProcedure;
		public int ClassExtraBytes;
		public int WindowExtraBytes;
		//[MarshalAs(UnmanagedType.
		public IntPtr Instance;
		public IntPtr Icon;
		public IntPtr Cursor;
		public IntPtr BackgroundBrush;
		//[MarshalAs(UnmanagedType.LPStr)]
		public IntPtr MenuName;
		[MarshalAs(UnmanagedType.LPTStr)]
		public string ClassName;
		//public string ClassName;

		public static int SizeInBytes = Marshal.SizeOf(typeof(WindowClass));
	}




	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	public struct ExtendedWindowClass {
		[CLSCompliant(false)]
		public UInt32 Size;
		public ClassStyle Style;
		//public WNDPROC WndProc;
		[MarshalAs(UnmanagedType.FunctionPtr)]
		[CLSCompliant(false)]
		public WindowProcedure WndProc;
		public int cbClsExtra;
		public int cbWndExtra;
		public IntPtr Instance;
		public IntPtr Icon;
		public IntPtr Cursor;
		public IntPtr Background;
		public IntPtr MenuName;
		public IntPtr ClassName;
		public IntPtr IconSm;

		[CLSCompliant(false)]
		public static uint SizeInBytes = (uint) Marshal.SizeOf(typeof(ExtendedWindowClass));
	}





	/// \public
	/// <summary>
	/// Struct pointed to by WM_GETMINMAXINFO lParam
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct MINMAXINFO {
		Point Reserved;
		public Size MaxSize;
		public Point MaxPosition;
		public Size MinTrackSize;
		public Size MaxTrackSize;
	}





	/// \public
	/// <summary>
	/// The WindowPosition structure contains information about the size and position of a window.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct WindowPosition {
		/// <summary>
		/// Handle to the window.
		/// </summary>
		public IntPtr hwnd;
		/// <summary>
		/// Specifies the position of the window in Z order (front-to-back position).
		/// This member can be a handle to the window behind which this window is placed,
		/// or can be one of the special values listed with the SetWindowPos function.
		/// </summary>
		public IntPtr hwndInsertAfter;
		/// <summary>
		/// Specifies the position of the left edge of the window.
		/// </summary>
		public int x;
		/// <summary>
		/// Specifies the position of the top edge of the window.
		/// </summary>
		public int y;
		/// <summary>
		/// Specifies the window width, in pixels.
		/// </summary>
		public int cx;
		/// <summary>
		/// Specifies the window height, in pixels.
		/// </summary>
		public int cy;
		/// <summary>
		/// Specifies the window position.
		/// </summary>
		[MarshalAs(UnmanagedType.U4)]
		public SetWindowPosFlags flags;
	}



	[Flags]
	public enum SetWindowPosFlags : int {
		/// <summary>
		/// Retains the current size (ignores the cx and cy parameters).
		/// </summary>
		NOSIZE = 0x0001,
		/// <summary>
		/// Retains the current position (ignores the x and y parameters).
		/// </summary>
		NOMOVE = 0x0002,
		/// <summary>
		/// Retains the current Z order (ignores the hwndInsertAfter parameter).
		/// </summary>
		NOZORDER = 0x0004,
		/// <summary>
		/// Does not redraw changes. If this flag is set, no repainting of any kind occurs.
		/// This applies to the client area, the nonclient area (including the title bar and scroll bars),
		/// and any part of the parent window uncovered as a result of the window being moved.
		/// When this flag is set, the application must explicitly invalidate or redraw any parts
		/// of the window and parent window that need redrawing.
		/// </summary>
		NOREDRAW = 0x0008,
		/// <summary>
		/// Does not activate the window. If this flag is not set,
		/// the window is activated and moved to the top of either the topmost or non-topmost group
		/// (depending on the setting of the hwndInsertAfter member).
		/// </summary>
		NOACTIVATE = 0x0010,
		/// <summary>
		/// Sends a WM_NCCALCSIZE message to the window, even if the window's size is not being changed.
		/// If this flag is not specified, WM_NCCALCSIZE is sent only when the window's size is being changed.
		/// </summary>
		FRAMECHANGED = 0x0020, /* The frame changed: send WM_NCCALCSIZE */
							   /// <summary>
							   /// Displays the window.
							   /// </summary>
		SHOWWINDOW = 0x0040,
		/// <summary>
		/// Hides the window.
		/// </summary>
		HIDEWINDOW = 0x0080,
		/// <summary>
		/// Discards the entire contents of the client area. If this flag is not specified,
		/// the valid contents of the client area are saved and copied back into the client area 
		/// after the window is sized or repositioned.
		/// </summary>
		NOCOPYBITS = 0x0100,
		/// <summary>
		/// Does not change the owner window's position in the Z order.
		/// </summary>
		NOOWNERZORDER = 0x0200, /* Don't do owner Z ordering */
								/// <summary>
								/// Prevents the window from receiving the WM_WINDOWPOSCHANGING message.
								/// </summary>
		NOSENDCHANGING = 0x0400, /* Don't send WM_WINDOWPOSCHANGING */

		/// <summary>
		/// Draws a frame (defined in the window's public class description) around the window.
		/// </summary>
		DRAWFRAME = FRAMECHANGED,
		/// <summary>
		/// Same as the NOOWNERZORDER flag.
		/// </summary>
		NOREPOSITION = NOOWNERZORDER,

		DEFERERASE = 0x2000,
		ASYNCWINDOWPOS = 0x4000
	}











	/// \public
	/// <summary>
	/// Defines information for the raw input devices.
	/// </summary>
	/// <remarks>
	/// If RIDEV_NOLEGACY is set for a mouse or a keyboard, the system does not generate any legacy message for that device for the application. For example, if the mouse TLC is set with RIDEV_NOLEGACY, WM_LBUTTONDOWN and related legacy mouse messages are not generated. Likewise, if the keyboard TLC is set with RIDEV_NOLEGACY, WM_KEYDOWN and related legacy keyboard messages are not generated.
	/// </remarks>
	[StructLayout(LayoutKind.Sequential)]
	public struct RawInputDevice {
		public static readonly int Size = Marshal.SizeOf(typeof(RawInputDevice));
		/// <summary>
		/// Top level collection Usage page for the raw input device.
		/// </summary>
		//public USHORT UsagePage;
		public Int16 UsagePage;
		/// <summary>
		/// Top level collection Usage for the raw input device.
		/// </summary>
		//public USHORT Usage;
		public Int16 Usage;
		/// <summary>
		/// Mode flag that specifies how to interpret the information provided by UsagePage and Usage.
		/// It can be zero (the default) or one of the following values.
		/// By default, the operating system sends raw input from devices with the specified top level collection (TLC)
		/// to the registered application as long as it has the window focus. 
		/// </summary>
		public RawInputDeviceFlags Flags;
		/// <summary>
		/// Handle to the target window. If NULL it follows the keyboard focus.
		/// </summary>
		public IntPtr Target;

		public override string ToString() {
			return String.Format("{0}/{1}, flags: {2}, window: {3}", UsagePage, Usage, Flags, Target);
		}
	}





	/// \public
	/// <summary>
	/// Contains information about a raw input device.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct RawInputDeviceList {
		public static readonly int Size = Marshal.SizeOf(typeof(RawInputDeviceList));
		/// <summary>
		/// Handle to the raw input device.
		/// </summary>
		public IntPtr Device;
		/// <summary>
		/// Type of device.
		/// </summary>
		public RawInputDeviceType Type;

		public override string ToString() {
			return String.Format("{0}, Handle: {1}", Type, Device);
		}
	}





	/// \public
	/// <summary>
	/// Contains the raw input from a device.
	/// </summary>
	/// <remarks>
	/// <para>The handle to this structure is passed in the lParam parameter of WM_INPUT.</para>
	/// <para>To get detailed information -- such as the header and the content of the raw input -- call GetRawInputData.</para>
	/// <para>To get device specific information, call GetRawInputDeviceInfo with the hDevice from RAWINPUTHEADER.</para>
	/// <para>Raw input is available only when the application calls RegisterRawInputDevices with valid device specifications.</para>
	/// </remarks>
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct RawInput {
		public RawInputHeader Header;
		public RawInputData Data;
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct RawInputData {
		[FieldOffset(0)]
		public RawMouse Mouse;
		[FieldOffset(0)]
		public RawKeyboard Keyboard;
		[FieldOffset(0)]
		public RawHID HID;
	}





	/// \public
	/// <summary>
	/// Contains the header information that is part of the raw input data.
	/// </summary>
	/// <remarks>
	/// To get more information on the device, use hDevice in a call to GetRawInputDeviceInfo.
	/// </remarks>
	[StructLayout(LayoutKind.Sequential)]
	public struct RawInputHeader {
		public static readonly int StructSize = Marshal.SizeOf(typeof(RawInputHeader));
		/// <summary>
		/// Type of raw input.
		/// </summary>
		public RawInputDeviceType Type;
		/// <summary>
		/// Size, in bytes, of the entire input packet of data. This includes the RawInput public struct plus possible extra input reports in the RAWHID variable length array.
		/// </summary>
		public Int32 Size;
		/// <summary>
		/// Handle to the device generating the raw input data.
		/// </summary>
		public IntPtr Device;
		/// <summary>
		/// Value passed in the wParam parameter of the WM_INPUT message.
		/// </summary>
		public IntPtr Param;
	}





	/// \public
	/// <summary>
	/// Contains information about the state of the keyboard.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct RawKeyboard {
		/// <summary>
		/// Scan code from the key depression. The scan code for keyboard overrun is KEYBOARD_OVERRUN_MAKE_CODE.
		/// </summary>
		//public USHORT MakeCode;
		public Int16 MakeCode;
		/// <summary>
		/// Flags for scan code information. It can be one or more of the following.
		/// RI_KEY_MAKE
		/// RI_KEY_BREAK
		/// RI_KEY_E0
		/// RI_KEY_E1
		/// RI_KEY_TERMSRV_SET_LED
		/// RI_KEY_TERMSRV_SHADOW
		/// </summary>
		public RawInputKeyboardDataFlags Flags;
		/// <summary>
		/// Reserved; must be zero.
		/// </summary>
		UInt16 Reserved;
		/// <summary>
		/// Microsoft Windows message compatible virtual-key code. For more information, see Virtual-Key Codes.
		/// </summary>
		//public USHORT VKey;
		public Keys VKey;
		/// <summary>
		/// Corresponding window message, for example WM_KEYDOWN, WM_SYSKEYDOWN, and so forth.
		/// </summary>
		//public UINT Message;
		public Int32 Message;
		/// <summary>
		/// Device-specific additional information for the event.
		/// </summary>
		//public ULONG ExtraInformation;
		public Int32 ExtraInformation;
	}





	/// \public
	/// <summary>
	/// Contains information about the state of the mouse.
	/// </summary>
	[StructLayout(LayoutKind.Explicit)]
	public struct RawMouse {
		/// <summary>
		/// Mouse state. This member can be any reasonable combination of the following. 
		/// MOUSE_ATTRIBUTES_CHANGED
		/// Mouse attributes changed; application needs to query the mouse attributes.
		/// MOUSE_MOVE_RELATIVE
		/// Mouse movement data is relative to the last mouse position.
		/// MOUSE_MOVE_ABSOLUTE
		/// Mouse movement data is based on absolute position.
		/// MOUSE_VIRTUAL_DESKTOP
		/// Mouse coordinates are mapped to the virtual desktop (for a multiple monitor system).
		/// </summary>
		[FieldOffset(0)]
		[CLSCompliant(false)]
		public RawMouseFlags Flags;  // USHORT in winuser.h, but only INT works -- USHORT returns 0.

		[FieldOffset(4)]
		[CLSCompliant(false)]
		public RawInputMouseState ButtonFlags;

		/// <summary>
		/// If usButtonFlags is RI_MOUSE_WHEEL, this member is a signed value that specifies the wheel delta.
		/// </summary>
		[FieldOffset(6)]
		[CLSCompliant(false)]
		public UInt16 ButtonData;

		/// <summary>
		/// Raw state of the mouse buttons.
		/// </summary>
		[FieldOffset(8)]
		[CLSCompliant(false)]
		public UInt32 RawButtons;

		/// <summary>
		/// Motion in the X direction. This is signed relative motion or absolute motion, depending on the value of usFlags.
		/// </summary>
		[FieldOffset(12)]
		public Int32 LastX;

		/// <summary>
		/// Motion in the Y direction. This is signed relative motion or absolute motion, depending on the value of usFlags.
		/// </summary>
		[FieldOffset(16)]
		public Int32 LastY;

		/// <summary>
		/// Device-specific additional information for the event.
		/// </summary>
		[FieldOffset(20)]
		[CLSCompliant(false)]
		public UInt32 ExtraInformation;
	}





	/// \public
	/// <summary>
	/// The RawHID structure describes the format of the raw input
	/// from a Human Interface Device (HID).
	/// </summary>
	/// <remarks>
	/// Each WM_INPUT can indicate several inputs, but all of the inputs
	/// come from the same HID. The size of the bRawData array is
	/// dwSizeHid * dwCount.
	/// </remarks>
	[StructLayout(LayoutKind.Sequential)]
	public struct RawHID {
		/// <summary>
		/// Size, in bytes, of each HID input in bRawData.
		/// </summary>
		public Int32 SizeHid;
		/// <summary>
		/// Number of HID inputs in bRawData.
		/// </summary>
		public Int32 Count;
		// The RawData field must be marshalled manually.
		///// <summary>
		///// Raw input data as an array of bytes.
		///// </summary>
		//public IntPtr RawData;
	}





	/// \public
	/// <summary>
	/// Defines the raw input data coming from any device.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public sealed class RawInputDeviceInfo {
		public static readonly int ClassSize = Marshal.SizeOf(typeof(RawInputDeviceInfo));
		/// <summary>
		/// Size, in bytes, of the RawInputDeviceInfo structure.
		/// </summary>
		Int32 Size = ClassSize;
		/// <summary>
		/// Type of raw input data.
		/// </summary>
		public RawInputDeviceType Type;
		public DeviceStruct Device;
		[StructLayout(LayoutKind.Explicit)]
		public struct DeviceStruct {
			[FieldOffset(0)]
			public RawInputMouseDeviceInfo Mouse;
			[FieldOffset(0)]
			public RawInputKeyboardDeviceInfo Keyboard;
			[FieldOffset(0)]
			public RawInputHIDDeviceInfo HID;
		};
	}





	/// \public
	/// <summary>
	/// Defines the raw input data coming from the specified Human Interface Device (HID).
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct RawInputHIDDeviceInfo {
		/// <summary>
		/// Vendor ID for the HID.
		/// </summary>
		public Int32 VendorId;
		/// <summary>
		/// Product ID for the HID.
		/// </summary>
		public Int32 ProductId;
		/// <summary>
		/// Version number for the HID.
		/// </summary>
		public Int32 VersionNumber;
		/// <summary>
		/// Top-level collection Usage Page for the device.
		/// </summary>
		//public USHORT UsagePage;
		public Int16 UsagePage;
		/// <summary>
		/// Top-level collection Usage for the device.
		/// </summary>
		//public USHORT Usage;
		public Int16 Usage;
	}





	/// \public
	/// <summary>
	/// Defines the raw input data coming from the specified keyboard.
	/// </summary>
	/// <remarks>
	/// For the keyboard, the Usage Page is 1 and the Usage is 6.
	/// </remarks>
	[StructLayout(LayoutKind.Sequential)]
	public struct RawInputKeyboardDeviceInfo {
		/// <summary>
		/// Type of the keyboard.
		/// </summary>
		public Int32 Type;
		/// <summary>
		/// Subtype of the keyboard.
		/// </summary>
		public Int32 SubType;
		/// <summary>
		/// Scan code mode.
		/// </summary>
		public Int32 KeyboardMode;
		/// <summary>
		/// Number of function keys on the keyboard.
		/// </summary>
		public Int32 NumberOfFunctionKeys;
		/// <summary>
		/// Number of LED indicators on the keyboard.
		/// </summary>
		public Int32 NumberOfIndicators;
		/// <summary>
		/// Total number of keys on the keyboard.
		/// </summary>
		public Int32 NumberOfKeysTotal;
	}





	/// \public
	/// <summary>
	/// Defines the raw input data coming from the specified mouse.
	/// </summary>
	/// <remarks>
	/// For the keyboard, the Usage Page is 1 and the Usage is 2.
	/// </remarks>
	[StructLayout(LayoutKind.Sequential)]
	public struct RawInputMouseDeviceInfo {
		/// <summary>
		/// ID for the mouse device.
		/// </summary>
		public Int32 Id;
		/// <summary>
		/// Number of buttons for the mouse.
		/// </summary>
		public Int32 NumberOfButtons;
		/// <summary>
		/// Number of data points per second. This information may not be applicable for every mouse device.
		/// </summary>
		public Int32 SampleRate;
		/// <summary>
		/// TRUE if the mouse has a wheel for horizontal scrolling; otherwise, FALSE.
		/// </summary>
		/// <remarks>
		/// This member is only supported under Microsoft Windows Vista and later versions.
		/// </remarks>
		public Boolean HasHorizontalWheel;
	}

	/// \public
	/// <summary>
	/// Defines the coordinates of the upper-left and lower-right corners of a rectangle.
	/// </summary>
	/// <remarks>
	/// By convention, the right and bottom edges of the rectangle are normally considered exclusive. In other words, the pixel whose coordinates are (right, bottom) lies immediately outside of the the rectangle. For example, when RECT is passed to the FillRect function, the rectangle is filled up to, but not including, the right column and bottom row of pixels. This structure is identical to the RECTL structure.
	/// </remarks>
	[StructLayout(LayoutKind.Sequential)]
	public struct Win32Rectangle {
		/// <summary>
		/// Specifies the x-coordinate of the upper-left corner of the rectangle.
		/// </summary>
		public int Left;
		/// <summary>
		/// Specifies the y-coordinate of the upper-left corner of the rectangle.
		/// </summary>
		public int Top;
		/// <summary>
		/// Specifies the x-coordinate of the lower-right corner of the rectangle.
		/// </summary>
		public int Right;
		/// <summary>
		/// Specifies the y-coordinate of the lower-right corner of the rectangle.
		/// </summary>
		public int Bottom;

		public int Width {
			get {
				return Right - Left;
			}
		}
		public int Height {
			get {
				return Bottom - Top;
			}
		}

		public Win32Rectangle(int left, int top, int right, int bottom) {
			this.Left = left;
			this.Top = top;
			this.Right = right;
			this.Bottom = bottom;
		}

		public override string ToString() {
			return String.Format("({0},{1})-({2},{3})", Left, Top, Right, Bottom);
		}

		public Rectangle ToRectangle() {
			return Rectangle.FromLTRB(Left, Top, Right, Bottom);
		}

		public static Win32Rectangle From(Rectangle value) {
			Win32Rectangle rect = new Win32Rectangle() {
				Left = value.Left,
				Right = value.Right,
				Top = value.Top,
				Bottom = value.Bottom
			};
			return rect;
		}

		public static Win32Rectangle From(Size value) {
			Win32Rectangle rect = new Win32Rectangle() {
				Left = 0,
				Right = value.Width,
				Top = 0,
				Bottom = value.Height
			};
			return rect;
		}
	}





	/// \public
	/// <summary>
	/// Contains window information.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct WindowInfo {
		public static readonly int StructSize = Marshal.SizeOf(typeof(WindowInfo));
		/// <summary>
		/// The size of the structure, in bytes.
		/// </summary>
		public Int32 Size;
		/// <summary>
		/// Pointer to a RECT structure that specifies the coordinates of the window. 
		/// </summary>
		public Win32Rectangle Window;
		/// <summary>
		/// Pointer to a RECT structure that specifies the coordinates of the client area. 
		/// </summary>
		public Win32Rectangle Client;
		/// <summary>
		/// The window styles. For a table of window styles, see CreateWindowEx. 
		/// </summary>
		[CLSCompliant(false)]
		public WindowStyle Style;
		/// <summary>
		/// The extended window styles. For a table of extended window styles, see CreateWindowEx.
		/// </summary>
		[CLSCompliant(false)]
		public ExtendedWindowStyle ExStyle;
		/// <summary>
		/// The window status. If this member is WS_ACTIVECAPTION, the window is active. Otherwise, this member is zero.
		/// </summary>
		public Int32 WindowStatus;
		/// <summary>
		/// The width of the window border, in pixels. 
		/// </summary>
		[CLSCompliant(false)]
		public UInt32 WindowBordersX;
		/// <summary>
		/// The height of the window border, in pixels.
		/// </summary>
		[CLSCompliant(false)]
		public UInt32 WindowBordersY;
		/// <summary>
		/// The window public class atom (see RegisterClass). 
		/// </summary>
		public Int32 WindowType;
		/// <summary>
		/// The Microsoft Windows version of the application that created the window. 
		/// </summary>
		public Int16 CreatorVersion;
	}





	public struct MonitorInfo {
		public Int32 Size;
		public Win32Rectangle Monitor;
		public Win32Rectangle Work;
		public Int32 Flags;

		public static readonly int SizeInBytes = Marshal.SizeOf(typeof(MonitorInfo));
	}

	//NCCALCSIZE_PARAMS Structure
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct NcCalculateSize {
		public Win32Rectangle NewBounds;
		public Win32Rectangle OldBounds;
		public Win32Rectangle OldClientRectangle;
		[CLSCompliant(false)]
		public unsafe WindowPosition* Position;
	}


	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	public struct SHFILEINFO {
		[CLSCompliant(false)]
		public static readonly uint Size = (uint) Marshal.SizeOf(typeof(SHFILEINFO));
		public IntPtr hIcon;
		public int iIcon;
		[CLSCompliant(false)]
		public uint dwAttributes;
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		public string szDisplayName;
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
		public string szTypeName;
	};

	public struct TrackMouseEventStructure {
		public Int32 Size;
		[CLSCompliant(false)]
		public TrackMouseEventFlags Flags;
		public IntPtr TrackWindowHandle;
		public Int32 HoverTime;

		public static readonly int SizeInBytes = Marshal.SizeOf(typeof(TrackMouseEventStructure));
	}





	public struct BroadcastHeader {
		public Int32 Size;
		public DeviceBroadcastType DeviceType;
		public Int32 dbch_reserved;
	}





	public struct BroadcastDeviceInterface {
		public Int32 Size;
		public DeviceBroadcastType DeviceType;
		public Int32 dbcc_reserved;
		public Guid ClassGuid;
		public char dbcc_name;
	}







	public enum SystemCursor : int {
		IDC_APPSTARTING = 32650, //Standard arrow and small hourglass
		IDC_NORMAL = 32512, //Standard arrow
		IDC_CROSS = 32515, //Crosshair
		IDC_HAND = 32649, //Hand
		IDC_HELP = 32651, //Arrow and question mark
		IDC_IBEAM = 32513, //I-beam
		IDC_NO = 32648, //Slashed circle
		IDC_SIZEALL = 32646, //Four-pointed arrow pointing north, south, east, and west
		IDC_SIZENESW = 32643, //Double-pointed arrow pointing northeast and southwest
		IDC_SIZENS = 32645, //Double-pointed arrow pointing north and south
		IDC_SIZENWSE = 32642, //Double-pointed arrow pointing northwest and southeast
		IDC_SIZEWE = 32644, //Double-pointed arrow pointing west and east
		IDC_UP = 32516, //Vertical arrow
		IDC_WAIT = 32514  //Hourglass
	}



	public enum SizeMessage {
		MAXHIDE = 4,
		MAXIMIZED = 2,
		MAXSHOW = 3,
		MINIMIZED = 1,
		RESTORED = 0
	}





	public enum NcCalcSizeOptions {
		ALIGNTOP = 0x10,
		ALIGNRIGHT = 0x80,
		ALIGNLEFT = 0x20,
		ALIGNBOTTOM = 0x40,
		HREDRAW = 0x100,
		VREDRAW = 0x200,
		REDRAW = (HREDRAW | VREDRAW),
		VALIDRECTS = 0x400
	}





	public enum DisplayModeSettingsEnum {
		CurrentSettings = -1,
		RegistrySettings = -2
	}





	[Flags]
	public enum DisplayDeviceStateFlags {
		None = 0x00000000,
		AttachedToDesktop = 0x00000001,
		MultiDriver = 0x00000002,
		PrimaryDevice = 0x00000004,
		MirroringDriver = 0x00000008,
		VgaCompatible = 0x00000010,
		Removable = 0x00000020,
		ModesPruned = 0x08000000,
		Remote = 0x04000000,
		Disconnect = 0x02000000,
		// Child device state
		Active = 0x00000001,
		Attached = 0x00000002,
	}





	[Flags]
	public enum ChangeDisplaySettingsEnum {
		// ChangeDisplaySettings types (found in winuser.h)
		UpdateRegistry = 0x00000001,
		Test = 0x00000002,
		Fullscreen = 0x00000004,
	}





	[Flags]
	[CLSCompliant(false)]
	public enum WindowStyle : uint {
		Overlapped = 0x00000000,
		Popup = 0x80000000,
		Child = 0x40000000,
		Minimize = 0x20000000,
		Visible = 0x10000000,
		Disabled = 0x08000000,
		ClipSiblings = 0x04000000,
		ClipChildren = 0x02000000,
		Maximize = 0x01000000,
		Caption = 0x00C00000,    // Border | DialogFrame
		Border = 0x00800000,
		DialogFrame = 0x00400000,
		VScroll = 0x00200000,
		HScreen = 0x00100000,
		SystemMenu = 0x00080000,
		ThickFrame = 0x00040000,
		Group = 0x00020000,
		TabStop = 0x00010000,

		MinimizeBox = 0x00020000,
		MaximizeBox = 0x00010000,

		Tiled = Overlapped,
		Iconic = Minimize,
		SizeBox = ThickFrame,
		TiledWindow = OverlappedWindow,

		// Common window styles:
		OverlappedWindow = Overlapped | Caption | SystemMenu | ThickFrame | MinimizeBox | MaximizeBox,
		PopupWindow = Popup | Border | SystemMenu,
		ChildWindow = Child
	}





	[Flags]
	[CLSCompliant(false)]
	public enum ExtendedWindowStyle : uint {
		DialogModalFrame = 0x00000001,
		NoParentNotify = 0x00000004,
		Topmost = 0x00000008,
		AcceptFiles = 0x00000010,
		Transparent = 0x00000020,

		// 
		MdiChild = 0x00000040,
		ToolWindow = 0x00000080,
		WindowEdge = 0x00000100,
		ClientEdge = 0x00000200,
		ContextHelp = 0x00000400,
		// 

		// 
		Right = 0x00001000,
		Left = 0x00000000,
		RightToLeftReading = 0x00002000,
		LeftToRightReading = 0x00000000,
		LeftScrollbar = 0x00004000,
		RightScrollbar = 0x00000000,

		ControlParent = 0x00010000,
		StaticEdge = 0x00020000,
		ApplicationWindow = 0x00040000,

		OverlappedWindow = WindowEdge | ClientEdge,
		PaletteWindow = WindowEdge | ToolWindow | Topmost,
		// 

		// 
		Layered = 0x00080000,
		// 

		// 
		NoInheritLayout = 0x00100000, // Disable inheritence of mirroring by children
		NoRedirectionBitmap = 0x00200000,
		RightToLeftLayout = 0x00400000, // Right to left mirroring
										//  /* WINVER >= 0x0500 */

		// 
		Composited = 0x02000000,
		//  /* _WIN32_WINNT >= 0x0501 */

		// 
		NoActivate = 0x08000000
		//  /* _WIN32_WINNT >= 0x0500 */
	}





	public enum GetWindowLongOffsets : int {
		WNDPROC = (-4),
		HINSTANCE = (-6),
		HWNDPARENT = (-8),
		STYLE = (-16),
		EXSTYLE = (-20),
		USERDATA = (-21),
		ID = (-12),
	}





	public enum WindowPlacementOptions {
		TOP = 0,
		BOTTOM = 1,
		TOPMOST = -1,
		NOTOPMOST = -2
	}




	[Flags]
	public enum ClassStyle {
		None = 0x0000,
		VRedraw = 0x0001,
		HRedraw = 0x0002,
		DoubleClicks = 0x0008,
		OwnDC = 0x0020,
		ClassDC = 0x0040,
		ParentDC = 0x0080,
		NoClose = 0x0200,
		SaveBits = 0x0800,
		ByteAlignClient = 0x1000,
		ByteAlignWindow = 0x2000,
		GlobalClass = 0x4000,

		Ime = 0x00010000,

		// 
		DropShadow = 0x00020000
		//  /* _WIN32_WINNT >= 0x0501 */
	}




	[Flags]
	public enum RawInputDeviceFlags : int {
		/// <summary>
		/// If set, this removes the top level collection from the inclusion list.
		/// This tells the operating system to stop reading from a device which matches the top level collection.
		/// </summary>
		REMOVE = 0x00000001,
		/// <summary>
		/// If set, this specifies the top level collections to exclude when reading a complete usage page.
		/// This flag only affects a TLC whose usage page is already specified with RawInputDeviceEnum.PAGEONLY. 
		/// </summary>
		EXCLUDE = 0x00000010,
		/// <summary>
		/// If set, this specifies all devices whose top level collection is from the specified UsagePage.
		/// Note that usUsage must be zero. To exclude a particular top level collection, use EXCLUDE.
		/// </summary>
		PAGEONLY = 0x00000020,
		/// <summary>
		/// If set, this prevents any devices specified by UsagePage or Usage from generating legacy messages.
		/// This is only for the mouse and keyboard. See RawInputDevice Remarks.
		/// </summary>
		NOLEGACY = 0x00000030,
		/// <summary>
		/// If set, this enables the caller to receive the input even when the caller is not in the foreground.
		/// Note that Target must be specified in RawInputDevice.
		/// </summary>
		INPUTSINK = 0x00000100,
		/// <summary>
		/// If set, the mouse button click does not activate the other window.
		/// </summary>
		CAPTUREMOUSE = 0x00000200, // effective when mouse nolegacy is specified, otherwise it would be an error
								   /// <summary>
								   /// If set, the application-defined keyboard device hotkeys are not handled.
								   /// However, the system hotkeys; for example, ALT+TAB and CTRL+ALT+DEL, are still handled.
								   /// By default, all keyboard hotkeys are handled.
								   /// NOHOTKEYS can be specified even if NOLEGACY is not specified and Target is NULL in RawInputDevice.
								   /// </summary>
		NOHOTKEYS = 0x00000200, // effective for keyboard.
								/// <summary>
								/// Microsoft Windows XP Service Pack 1 (SP1): If set, the application command keys are handled. APPKEYS can be specified only if NOLEGACY is specified for a keyboard device.
								/// </summary>
		APPKEYS = 0x00000400, // effective for keyboard.
							  /// <summary>
							  /// If set, this enables the caller to receive input in the background only if the foreground application
							  /// does not process it. In other words, if the foreground application is not registered for raw input,
							  /// then the background application that is registered will receive the input.
							  /// </summary>
		EXINPUTSINK = 0x00001000,
		DEVNOTIFY = 0x00002000,
		//EXMODEMASK      = 0x000000F0
	}





	public enum GetRawInputDataEnum {
		INPUT = 0x10000003,
		HEADER = 0x10000005
	}





	public enum RawInputDeviceInfoEnum {
		PREPARSEDDATA = 0x20000005,
		DEVICENAME = 0x20000007,  // the return valus is the character length, not the byte size
		DEVICEINFO = 0x2000000b
	}





	[Flags]
	[CLSCompliant(false)]
	public enum RawInputMouseState : ushort {
		LEFT_BUTTON_DOWN = 0x0001,  // Left Button changed to down.
		LEFT_BUTTON_UP = 0x0002,  // Left Button changed to up.
		RIGHT_BUTTON_DOWN = 0x0004,  // Right Button changed to down.
		RIGHT_BUTTON_UP = 0x0008,  // Right Button changed to up.
		MIDDLE_BUTTON_DOWN = 0x0010,  // Middle Button changed to down.
		MIDDLE_BUTTON_UP = 0x0020,  // Middle Button changed to up.

		BUTTON_1_DOWN = LEFT_BUTTON_DOWN,
		BUTTON_1_UP = LEFT_BUTTON_UP,
		BUTTON_2_DOWN = RIGHT_BUTTON_DOWN,
		BUTTON_2_UP = RIGHT_BUTTON_UP,
		BUTTON_3_DOWN = MIDDLE_BUTTON_DOWN,
		BUTTON_3_UP = MIDDLE_BUTTON_UP,

		BUTTON_4_DOWN = 0x0040,
		BUTTON_4_UP = 0x0080,
		BUTTON_5_DOWN = 0x0100,
		BUTTON_5_UP = 0x0200,

		WHEEL = 0x0400
	}





	public enum RawInputKeyboardDataFlags : short //: ushort
	{
		MAKE = 0,
		BREAK = 1,
		E0 = 2,
		E1 = 4,
		TERMSRV_SET_LED = 8,
		TERMSRV_SHADOW = 0x10
	}





	public enum RawInputDeviceType : int {
		MOUSE = 0,
		KEYBOARD = 1,
		HID = 2
	}





	/// <summary>
	/// Mouse indicator type (found in winuser.h).
	/// </summary>
	[Flags]
	[CLSCompliant(false)]
	public enum RawMouseFlags : ushort {
		/// <summary>
		/// LastX/Y indicate relative motion.
		/// </summary>
		MOUSE_MOVE_RELATIVE = 0x00,
		/// <summary>
		/// LastX/Y indicate absolute motion.
		/// </summary>
		MOUSE_MOVE_ABSOLUTE = 0x01,
		/// <summary>
		/// The coordinates are mapped to the virtual desktop.
		/// </summary>
		MOUSE_VIRTUAL_DESKTOP = 0x02,
		/// <summary>
		/// Requery for mouse attributes.
		/// </summary>
		MOUSE_ATTRIBUTES_CHANGED = 0x04,
	}





	/// \public
	/// <summary>
	/// Queue status type for GetQueueStatus() and MsgWaitForMultipleObjects()
	/// </summary>
	[Flags]
	public enum QueueStatusFlags {
		/// <summary>
		/// A WM_KEYUP, WM_KEYDOWN, WM_SYSKEYUP, or WM_SYSKEYDOWN message is in the queue.
		/// </summary>
		KEY = 0x0001,
		/// <summary>
		/// A WM_MOUSEMOVE message is in the queue.
		/// </summary>
		MOUSEMOVE = 0x0002,
		/// <summary>
		/// A mouse-button message (WM_LBUTTONUP, WM_RBUTTONDOWN, and so on).
		/// </summary>
		MOUSEBUTTON = 0x0004,
		/// <summary>
		/// A posted message (other than those listed here) is in the queue.
		/// </summary>
		POSTMESSAGE = 0x0008,
		/// <summary>
		/// A WM_TIMER message is in the queue.
		/// </summary>
		TIMER = 0x0010,
		/// <summary>
		/// A WM_PAINT message is in the queue.
		/// </summary>
		PAINT = 0x0020,
		/// <summary>
		/// A message sent by another thread or application is in the queue.
		/// </summary>
		SENDMESSAGE = 0x0040,
		/// <summary>
		/// A WM_HOTKEY message is in the queue.
		/// </summary>
		HOTKEY = 0x0080,
		/// <summary>
		/// A posted message (other than those listed here) is in the queue.
		/// </summary>
		ALLPOSTMESSAGE = 0x0100,
		/// <summary>
		/// A raw input message is in the queue. For more information, see Raw Input.
		/// Windows XP and higher only.
		/// </summary>
		RAWINPUT = 0x0400,
		/// <summary>
		/// A WM_MOUSEMOVE message or mouse-button message (WM_LBUTTONUP, WM_RBUTTONDOWN, and so on).
		/// </summary>
		MOUSE = MOUSEMOVE | MOUSEBUTTON,
		/// <summary>
		/// An input message is in the queue. This is composed of KEY, MOUSE and RAWINPUT.
		/// Windows XP and higher only.
		/// </summary>
		INPUT = MOUSE | KEY | RAWINPUT,
		/// <summary>
		/// An input message is in the queue. This is composed of QS_KEY and QS_MOUSE.
		/// Windows 2000 and earlier.
		/// </summary>
		INPUT_LEGACY = MOUSE | KEY,
		/// <summary>
		/// An input, WM_TIMER, WM_PAINT, WM_HOTKEY, or posted message is in the queue.
		/// </summary>
		ALLEVENTS = INPUT | POSTMESSAGE | TIMER | PAINT | HOTKEY,
		/// <summary>
		/// Any message is in the queue.
		/// </summary>
		ALLINPUT = INPUT | POSTMESSAGE | TIMER | PAINT | HOTKEY | SENDMESSAGE
	}

	/// <summary>
	/// Represents a list of all common window messages.
	/// </summary>
	[CLSCompliant(false)]
	public enum WindowMessage : uint {
		NULL = 0x0000,
		CREATE = 0x0001,
		DESTROY = 0x0002,
		MOVE = 0x0003,
		SIZE = 0x0005,
		ACTIVATE = 0x0006,
		SETFOCUS = 0x0007,
		KILLFOCUS = 0x0008,
		SETVISIBLE = 0x0009,
		ENABLE = 0x000A,
		SETREDRAW = 0x000B,
		SETTEXT = 0x000C,
		GETTEXT = 0x000D,
		GETTEXTLENGTH = 0x000E,
		PAINT = 0x000F,
		CLOSE = 0x0010,
		QUERYENDSESSION = 0x0011,
		QUIT = 0x0012,
		QUERYOPEN = 0x0013,
		ERASEBKGND = 0x0014,
		SYSCOLORCHANGE = 0x0015,
		ENDSESSION = 0x0016,
		SYSTEMERROR = 0x0017,
		SHOWWINDOW = 0x0018,
		CTLCOLOR = 0x0019,
		WININICHANGE = 0x001A,
		SETTINGCHANGE = 0x001A,
		DEVMODECHANGE = 0x001B,
		ACTIVATEAPP = 0x001C,
		FONTCHANGE = 0x001D,
		TIMECHANGE = 0x001E,
		CANCELMODE = 0x001F,
		SETCURSOR = 0x0020,
		MOUSEACTIVATE = 0x0021,
		CHILDACTIVATE = 0x0022,
		QUEUESYNC = 0x0023,
		GETMINMAXINFO = 0x0024,
		PAINTICON = 0x0026,
		ICONERASEBKGND = 0x0027,
		NEXTDLGCTL = 0x0028,
		ALTTABACTIVE = 0x0029,
		SPOOLERSTATUS = 0x002A,
		DRAWITEM = 0x002B,
		MEASUREITEM = 0x002C,
		DELETEITEM = 0x002D,
		VKEYTOITEM = 0x002E,
		CHARTOITEM = 0x002F,
		SETFONT = 0x0030,
		GETFONT = 0x0031,
		SETHOTKEY = 0x0032,
		GETHOTKEY = 0x0033,
		FILESYSCHANGE = 0x0034,
		ISACTIVEICON = 0x0035,
		QUERYPARKICON = 0x0036,
		QUERYDRAGICON = 0x0037,
		COMPAREITEM = 0x0039,
		TESTING = 0x003a,
		OTHERWINDOWCREATED = 0x003c,
		GETOBJECT = 0x003D,
		ACTIVATESHELLWINDOW = 0x003e,
		COMPACTING = 0x0041,
		COMMNOTIFY = 0x0044,
		WINDOWPOSCHANGING = 0x0046,
		WINDOWPOSCHANGED = 0x0047,
		POWER = 0x0048,
		COPYDATA = 0x004A,
		CANCELJOURNAL = 0x004B,
		NOTIFY = 0x004E,
		INPUTLANGCHANGEREQUEST = 0x0050,
		INPUTLANGCHANGE = 0x0051,
		TCARD = 0x0052,
		HELP = 0x0053,
		USERCHANGED = 0x0054,
		NOTIFYFORMAT = 0x0055,
		CONTEXTMENU = 0x007B,
		STYLECHANGING = 0x007C,
		STYLECHANGED = 0x007D,
		DISPLAYCHANGE = 0x007E,
		GETICON = 0x007F,

		// Non-Client messages
		SETICON = 0x0080,
		NCCREATE = 0x0081,
		NCDESTROY = 0x0082,
		NCCALCSIZE = 0x0083,
		NCHITTEST = 0x0084,
		NCPAINT = 0x0085,
		NCACTIVATE = 0x0086,
		GETDLGCODE = 0x0087,
		SYNCPAINT = 0x0088,
		SYNCTASK = 0x0089,
		NCMOUSEMOVE = 0x00A0,
		NCLBUTTONDOWN = 0x00A1,
		NCLBUTTONUP = 0x00A2,
		NCLBUTTONDBLCLK = 0x00A3,
		NCRBUTTONDOWN = 0x00A4,
		NCRBUTTONUP = 0x00A5,
		NCRBUTTONDBLCLK = 0x00A6,
		NCMBUTTONDOWN = 0x00A7,
		NCMBUTTONUP = 0x00A8,
		NCMBUTTONDBLCLK = 0x00A9,
		/// <summary>
		/// Windows 2000 and higher only.
		/// </summary>
		NCXBUTTONDOWN = 0x00ab,
		/// <summary>
		/// Windows 2000 and higher only.
		/// </summary>
		NCXBUTTONUP = 0x00ac,
		/// <summary>
		/// Windows 2000 and higher only.
		/// </summary>
		NCXBUTTONDBLCLK = 0x00ad,

		INPUT = 0x00FF,

		KEYDOWN = 0x0100,
		KEYFIRST = 0x0100,
		KEYUP = 0x0101,
		CHAR = 0x0102,
		DEADCHAR = 0x0103,
		SYSKEYDOWN = 0x0104,
		SYSKEYUP = 0x0105,
		SYSCHAR = 0x0106,
		SYSDEADCHAR = 0x0107,
		KEYLAST = 0x0108,
		IME_STARTCOMPOSITION = 0x010D,
		IME_ENDCOMPOSITION = 0x010E,
		IME_COMPOSITION = 0x010F,
		IME_KEYLAST = 0x010F,
		INITDIALOG = 0x0110,
		COMMAND = 0x0111,
		SYSCOMMAND = 0x0112,
		TIMER = 0x0113,
		HSCROLL = 0x0114,
		VSCROLL = 0x0115,
		INITMENU = 0x0116,
		INITMENUPOPUP = 0x0117,
		SYSTIMER = 0x0118,
		GESTURE = 0x0119,
		GESTURENOTIFY = 0x011A,
		MENUSELECT = 0x011F,
		MENUCHAR = 0x0120,
		ENTERIDLE = 0x0121,
		MENURBUTTONUP = 0x0122,
		MENUDRAG = 0x0123,
		MENUGETOBJECT = 0x0124,
		UNINITMENUPOPUP = 0x0125,
		MENUCOMMAND = 0x0126,

		CHANGEUISTATE = 0x0127,
		UPDATEUISTATE = 0x0128,
		QUERYUISTATE = 0x0129,

		LBTRACKPOINT = 0x0131,
		CTLCOLORMSGBOX = 0x0132,
		CTLCOLOREDIT = 0x0133,
		CTLCOLORLISTBOX = 0x0134,
		CTLCOLORBTN = 0x0135,
		CTLCOLORDLG = 0x0136,
		CTLCOLORSCROLLBAR = 0x0137,
		CTLCOLORSTATIC = 0x0138,
		MOUSEMOVE = 0x0200,
		MOUSEFIRST = 0x0200,
		LBUTTONDOWN = 0x0201,
		LBUTTONUP = 0x0202,
		LBUTTONDBLCLK = 0x0203,
		RBUTTONDOWN = 0x0204,
		RBUTTONUP = 0x0205,
		RBUTTONDBLCLK = 0x0206,
		MBUTTONDOWN = 0x0207,
		MBUTTONUP = 0x0208,
		MBUTTONDBLCLK = 0x0209,
		MOUSEWHEEL = 0x020A,
		MOUSELAST = 0x020D,
		/// <summary>
		/// Windows 2000 and higher only.
		/// </summary>
		XBUTTONDOWN = 0x020B,
		/// <summary>
		/// Windows 2000 and higher only.
		/// </summary>
		XBUTTONUP = 0x020C,
		/// <summary>
		/// Windows 2000 and higher only.
		/// </summary>
		XBUTTONDBLCLK = 0x020D,
		PARENTNOTIFY = 0x0210,
		ENTERMENULOOP = 0x0211,
		EXITMENULOOP = 0x0212,
		NEXTMENU = 0x0213,
		SIZING = 0x0214,
		CAPTURECHANGED = 0x0215,
		MOVING = 0x0216,
		POWERBROADCAST = 0x0218,
		DEVICECHANGE = 0x0219,
		MDICREATE = 0x0220,
		MDIDESTROY = 0x0221,
		MDIACTIVATE = 0x0222,
		MDIRESTORE = 0x0223,
		MDINEXT = 0x0224,
		MDIMAXIMIZE = 0x0225,
		MDITILE = 0x0226,
		MDICASCADE = 0x0227,
		MDIICONARRANGE = 0x0228,
		MDIGETACTIVE = 0x0229,
		/* D&D messages */
		DROPOBJECT = 0x022A,
		QUERYDROPOBJECT = 0x022B,
		BEGINDRAG = 0x022C,
		DRAGLOOP = 0x022D,
		DRAGSELECT = 0x022E,
		DRAGMOVE = 0x022F,
		MDISETMENU = 0x0230,
		ENTERSIZEMOVE = 0x0231,
		EXITSIZEMOVE = 0x0232,
		DROPFILES = 0x0233,
		MDIREFRESHMENU = 0x0234,
		TOUCH = 0x0240,
		IME_SETCONTEXT = 0x0281,
		IME_NOTIFY = 0x0282,
		IME_CONTROL = 0x0283,
		IME_COMPOSITIONFULL = 0x0284,
		IME_SELECT = 0x0285,
		IME_CHAR = 0x0286,
		IME_REQUEST = 0x0288,
		IME_KEYDOWN = 0x0290,
		IME_KEYUP = 0x0291,
		NCMOUSEHOVER = 0x02A0,
		MOUSEHOVER = 0x02A1,
		NCMOUSELEAVE = 0x02A2,
		MOUSELEAVE = 0x02A3,
		CUT = 0x0300,
		COPY = 0x0301,
		PASTE = 0x0302,
		CLEAR = 0x0303,
		UNDO = 0x0304,
		RENDERFORMAT = 0x0305,
		RENDERALLFORMATS = 0x0306,
		DESTROYCLIPBOARD = 0x0307,
		DRAWCLIPBOARD = 0x0308,
		PAINTCLIPBOARD = 0x0309,
		VSCROLLCLIPBOARD = 0x030A,
		SIZECLIPBOARD = 0x030B,
		ASKCBFORMATNAME = 0x030C,
		CHANGECBCHAIN = 0x030D,
		HSCROLLCLIPBOARD = 0x030E,
		QUERYNEWPALETTE = 0x030F,
		PALETTEISCHANGING = 0x0310,
		PALETTECHANGED = 0x0311,
		HOTKEY = 0x0312,
		PRINT = 0x0317,
		PRINTCLIENT = 0x0318,
		THEMECHANGED = 0x031A,
		HANDHELDFIRST = 0x0358,
		HANDHELDLAST = 0x35F,
		SIZEPARENT = 0x0361,
		SETMESSAGESTRING = 0x0362,
		IDLEUPDATECMDUI = 0x0363,
		INITIALUPDATE = 0x0364,
		COMMANDHELP = 0x0365,
		HELPHITTEST = 0x0366,
		EXITHELPMODE = 0x0367,
		PENWINFIRST = 0x380,
		PENWINLAST = 0x38F,
		COALESCE_FIRST = 0x390,
		COALESCE_LAST = 0x39F,
		DDE_FIRST = 0x3E0,
		DDE_INITIATE = 0x3E0,
		DDE_TERMINATE = 0x3E1,
		DDE_ADVISE = 0x3E2,
		DDE_UNADVISE = 0x3E3,
		DDE_ACK = 0x3E4,
		DDE_DATA = 0x3E5,
		DDE_REQUEST = 0x3E6,
		DDE_POKE = 0x3E7,
		DDE_EXECUTE = 0x3E8,
		DDE_LAST = 0x3E8,
		AFXFIRST = 0x0360,
		AFXLAST = 0x037F,
		TVM_SETEXTENDEDSTYLE = 0x1100 + 44,
		TVM_GETEXTENDEDSTYLE = 0x1100 + 45,
		APP = 0x8000,
		USER = 0x0400,

		// Our "private" ones
		MOUSEENTER = 0x0401,
		ASYNC_MESSAGE = 0x0403,
		REFLECT = USER + 0x1c00,
		CLOSE_INTERNAL = USER + 0x1c01,

		// NotifyIcon (Systray) Balloon messages 
		BALLOONSHOW = USER + 0x0002,
		BALLOONHIDE = USER + 0x0003,
		BALLOONTIMEOUT = USER + 0x0004,
		BALLOONUSERCLICK = USER + 0x0005,

		GETLIMITTEXT = 0x425,
		POSFROMCHAR = 0x426,
		CHARFROMPOS = 0x427,
		SCROLLCARET = 0x431,
		CANPASTE = 0x432,
		DISPLAYBAND = 0x433,
		EXGETSEL = 0x434,
		EXLIMITTEXT = 0x435,
		EXLINEFROMCHAR = 0x436,
		EXSETSEL = 0x437,
		FINDTEXT = 0x438,
		FORMATRANGE = 0x439,
		GETCHARFORMAT = 0x43A,
		GETEVENTMASK = 0x43B,
		GETOLEINTERFACE = 0x43C,
		GETPARAFORMAT = 0x43D,
		GETSELTEXT = 0x43E,
		HIDESELECTION = 0x43F,
		PASTESPECIAL = 0x440,
		REQUESTRESIZE = 0x441,
		SELECTIONTYPE = 0x442,
		SETBKGNDCOLOR = 0x443,
		SETCHARFORMAT = 0x444,
		SETEVENTMASK = 0x445,
		SETOLECALLBACK = 0x446,
		SETPARAFORMAT = 0x447,
		SETTARGETDEVICE = 0x448,
		STREAMIN = 0x449,
		STREAMOUT = 0x44A,
		GETTEXTRANGE = 0x44B,
		FINDWORDBREAK = 0x44C,
		SETOPTIONS = 0x44D,
		GETOPTIONS = 0x44E,
		FINDTEXTEX = 0x44F,
		GETWORDBREAKPROCEX = 0x450,
		SETWORDBREAKPROCEX = 0x451,
		SETUNDOLIMIT = 0x452,
		REDO = 0x454,
		CANREDO = 0x455,
		GETUNDONAME = 0x456,
		GETREDONAME = 0x457,
		STOPGROUPTYPING = 0x458,
		SETTEXTMODE = 0x459,
		GETTEXTMODE = 0x45A,
		AUTOURLDETECT = 0x45B,
		GETAUTOURLDETECT = 0x45C,
		SETPALETTE = 0x45D,
		GETTEXTEX = 0x45E,
		GETTEXTLENGTHEX = 0x45F,
		SETPUNCTUATION = 0x464,
		GETPUNCTUATION = 0x465,
		SETWORDWRAPMODE = 0x466,
		GETWORDWRAPMODE = 0x467,
		SETIMECOLOR = 0x468,
		GETIMECOLOR = 0x469,
		SETIMEOPTIONS = 0x46A,
		GETIMEOPTIONS = 0x46B,
		CONVPOSITION = 0x46C,
		SETLANGOPTIONS = 0x478,
		GETLANGOPTIONS = 0x479,
		GETIMECOMPMODE = 0x47A,
		FINDTEXTW = 0x47B,
		FINDTEXTEXW = 0x47C,
		RECONVERSION = 0x47D,
		SETIMEMODEBIAS = 0x47E,
		GETIMEMODEBIAS = 0x47F,
		SETBIDIOPTIONS = 0x4C8,
		GETBIDIOPTIONS = 0x4C9,
		SETTYPOGRAPHYOPTIONS = 0x4CA,
		GETTYPOGRAPHYOPTIONS = 0x4CB,
		SETEDITSTYLE = 0x4CC,
		COPYGLOBALDATA = 0x0049,
		UNICHAR = 0x0109,
		CONVERTREQUEST = 0x010a,
		CONVERTRESULT = 0x010b,
		INTERIM = 0x010c,
		IME_REPORT = 0x0280,
		APPCOMMAND = 0x0319,
		RCRESULT = 0x0381,
		HOOKRCRESULT = 0x0382,
		GLOBALRCCHANGE = 0x0383,
		SKB = 0x0384,
		PENCTL = 0x0385,
		PENMISC = 0x0386,
		CTLINIT = 0x0387,
		PENEVENT = 0x0388,
		CAP_UNICODE_START = 0x0464,
		CHOOSEFONT_SETLOGFONT = 0x0465,
		CHOOSEFONT_SETFLAGS = 0x0466,
		CPL_LAUNCH = 0x07e8,
		CPL_LAUNCHED = 0x07e9,
		CLIPBOARD_CHANGED = 0xc227,
		RASDIALEVENT = 0xcccd
	}

	/// <summary>
	/// Represents the key modifiers that can be used to trigger hotkeys.
	/// </summary>
	[Flags]
	public enum KeyModifiers : int {
		/// <summary>
		/// No modifier
		/// </summary>
		None = 0,
		/// <summary>
		/// Alt key
		/// </summary>
		Alt = 1,
		/// <summary>
		/// Control key
		/// </summary>
		Control = 2,
		/// <summary>
		/// Shift key
		/// </summary>
		Shift = 4,
		/// <summary>
		/// Windows Start key
		/// </summary>
		Windows = 8,
		/// <summary>
		/// Disables press and hold to repeat for the hotkey.
		/// </summary>
		NoRepeat = 16384
	}

	public enum PixelType : byte {
		RGBA = 0,
		INDEXED = 1
	}

	[CLSCompliant(false)]
	public enum SystemCommand : uint {
		SC_SIZE = 0xF000,
		SC_MOVE = 0xF010,
		SC_MINIMIZE = 0xF020,
		SC_MAXIMIZE = 0xF030,
		SC_NEXTWINDOW = 0xF040,
		SC_PREVWINDOW = 0xF050,
		SC_CLOSE = 0xF060,
		SC_VSCROLL = 0xF070,
		SC_HSCROLL = 0xF080,
		SC_MOUSEMENU = 0xF090,
		SC_KEYMENU = 0xF100,
		SC_ARRANGE = 0xF110,
		SC_RESTORE = 0xF120,
		SC_TASKLIST = 0xF130,
		SC_SCREENSAVE = 0xF140,
		SC_HOTKEY = 0xF150,
		//#ifdef(WINVER >= 0x0400) //Win95
		SC_DEFAULT = 0xF160,
		SC_MONITORPOWER = 0xF170,
		SC_CONTEXTHELP = 0xF180,
		SC_SEPARATOR = 0xF00F,
		//#endif /* WINVER >= 0x0400 */

		//#ifdef(WINVER >= 0x0600) //Vista
		SCF_ISSECURE = 0x00000001,
		//#endif /* WINVER >= 0x0600 */
		SC_ICON = SC_MINIMIZE,
		SC_ZOOM = SC_MAXIMIZE,
	}

	public enum MouseMessages {
		MOUSEMOVE = 0x0200,
		LBUTTONDOWN = 0x0201,
		LBUTTONUP = 0x0202,
		LBUTTONDBLCLK = 0x0203,
		RBUTTONDOWN = 0x0204,
		RBUTTONUP = 0x0205,
		RBUTTONDBLCLK = 0x0206,
		MBUTTONDOWN = 0x0207,
		MBUTTONUP = 0x0208,
		MBUTTONDBLCLK = 0x0209,
		MOUSEWHEEL = 0x020A,
		/// <summary>
		/// Windows 2000 and higher only.
		/// </summary>
		XBUTTONDOWN = 0x020B,
		/// <summary>
		/// Windows 2000 and higher only.
		/// </summary>
		XBUTTONUP = 0x020C,
		/// <summary>
		/// Windows 2000 and higher only.
		/// </summary>
		XBUTTONDBLCLK = 0x020D,
	}

	[CLSCompliant(false)]
	public enum TernaryRasterOperations : uint {
		SRCCOPY = 0x00CC0020,
		SRCPAINT = 0x00EE0086,
		SRCAND = 0x008800C6,
		SRCINVERT = 0x00660046,
		SRCERASE = 0x00440328,
		NOTSRCCOPY = 0x00330008,
		NOTSRCERASE = 0x001100A6,
		MERGECOPY = 0x00C000CA,
		MERGEPAINT = 0x00BB0226,
		PATCOPY = 0x00F00021,
		PATPAINT = 0x00FB0A09,
		PATINVERT = 0x005A0049,
		DSTINVERT = 0x00550009,
		BLACKNESS = 0x00000042,
		WHITENESS = 0x00FF0062,
		CAPTUREBLT = 0x40000000 //only if WinVer >= 5.0.0 (see wingdi.h)
	}

	/// <summary>
	/// ShowWindow() Commands
	/// </summary>
	public enum ShowWindowCommand {
		/// <summary>
		/// Hides the window and activates another window.
		/// </summary>
		HIDE = 0,
		/// <summary>
		/// Activates and displays a window. If the window is minimized or maximized, the system restores it to its original size and position. An application should specify this flag when displaying the window for the first time.
		/// </summary>
		SHOWNORMAL = 1,
		NORMAL = 1,
		/// <summary>
		/// Activates the window and displays it as a minimized window.
		/// </summary>
		SHOWMINIMIZED = 2,
		/// <summary>
		/// Activates the window and displays it as a maximized window.
		/// </summary>
		SHOWMAXIMIZED = 3,
		MAXIMIZE = 3,
		/// <summary>
		/// Displays a window in its most recent size and position. This value is similar to SW_SHOWNORMAL, except that the window is not activated.
		/// </summary>
		SHOWNOACTIVATE = 4,
		/// <summary>
		/// Activates the window and displays it in its current size and position.
		/// </summary>
		SHOW = 5,
		/// <summary>
		/// Minimizes the specified window and activates the next top-level window in the Z order.
		/// </summary>
		MINIMIZE = 6,
		/// <summary>
		/// Displays the window as a minimized window. This value is similar to SW_SHOWMINIMIZED, except the window is not activated.
		/// </summary>
		SHOWMINNOACTIVE = 7,
		/// <summary>
		/// Displays the window in its current size and position. This value is similar to SW_SHOW, except the window is not activated.
		/// </summary>
		SHOWNA = 8,
		/// <summary>
		/// Activates and displays the window. If the window is minimized or maximized, the system restores it to its original size and position. An application should specify this flag when restoring a minimized window.
		/// </summary>
		RESTORE = 9,
		/// <summary>
		/// Sets the show state based on the SW_ value specified in the STARTUPINFO structure passed to the CreateProcess function by the program that started the application.
		/// </summary>
		SHOWDEFAULT = 10,
		/// <summary>
		/// Windows 2000/XP: Minimizes a window, even if the thread that owns the window is not responding. This flag should only be used when minimizing windows from a different thread.
		/// </summary>
		FORCEMINIMIZE = 11,
		//MAX             = 11,

		// Old ShowWindow() Commands
		//HIDE_WINDOW        = 0,
		//SHOW_OPENWINDOW    = 1,
		//SHOW_ICONWINDOW    = 2,
		//SHOW_FULLSCREEN    = 3,
		//SHOW_OPENNOACTIVATE= 4,
	}

	/// <summary>
	/// Identifiers for the WM_SHOWWINDOW message
	/// </summary>
	public enum ShowWindowMessageIdentifiers {
		PARENTCLOSING = 1,
		OTHERZOOM = 2,
		PARENTOPENING = 3,
		OTHERUNZOOM = 4,
	}

	/// <summary>
	/// Enumerates the available character sets.
	/// </summary>
	public enum GdiCharset {
		Ansi = 0,
		Default = 1,
		Symbol = 2,
		ShiftJIS = 128,
		Hangeul = 129,
		Hangul = 129,
		GB2312 = 134,
		ChineseBig5 = 136,
		OEM = 255,
		//
		Johab = 130,
		Hebrew = 177,
		Arabic = 178,
		Greek = 161,
		Turkish = 162,
		Vietnamese = 163,
		Thai = 222,
		EastEurope = 238,
		Russian = 204,
		Mac = 77,
		Baltic = 186,
	}

	public enum MapVirtualKeyType {
		/// <summary>uCode is a virtual-key code and is translated into a scan code. If it is a virtual-key code that does not distinguish between left- and right-hand keys, the left-hand scan code is returned. If there is no translation, the function returns 0.</summary>
		VirtualKeyToScanCode = 0,
		/// <summary>uCode is a scan code and is translated into a virtual-key code that does not distinguish between left- and right-hand keys. If there is no translation, the function returns 0.</summary>
		ScanCodeToVirtualKey = 1,
		/// <summary>uCode is a virtual-key code and is translated into an unshifted character value in the low-order word of the return value. Dead keys (diacritics) are indicated by setting the top bit of the return value. If there is no translation, the function returns 0.</summary>
		VirtualKeyToCharacter = 2,
		/// <summary>Windows NT/2000/XP: uCode is a scan code and is translated into a virtual-key code that distinguishes between left- and right-hand keys. If there is no translation, the function returns 0.</summary>
		ScanCodeToVirtualKeyExtended = 3,
		VirtualKeyToScanCodeExtended = 4,
	}

	[Flags]
	public enum ShGetFileIconFlags : int {
		/// <summary>get icon</summary>
		Icon = 0x000000100,
		/// <summary>get display name</summary>
		DisplayName = 0x000000200,
		/// <summary>get type name</summary>
		TypeName = 0x000000400,
		/// <summary>get attributes</summary>
		Attributes = 0x000000800,
		/// <summary>get icon location</summary>
		IconLocation = 0x000001000,
		/// <summary>return exe type</summary>
		ExeType = 0x000002000,
		/// <summary>get system icon index</summary>
		SysIconIndex = 0x000004000,
		/// <summary>put a link overlay on icon</summary>
		LinkOverlay = 0x000008000,
		/// <summary>show icon in selected state</summary>
		Selected = 0x000010000,
		/// <summary>get only specified attributes</summary>
		Attr_Specified = 0x000020000,
		/// <summary>get large icon</summary>
		LargeIcon = 0x000000000,
		/// <summary>get small icon</summary>
		SmallIcon = 0x000000001,
		/// <summary>get open icon</summary>
		OpenIcon = 0x000000002,
		/// <summary>get shell size icon</summary>
		ShellIconSize = 0x000000004,
		/// <summary>pszPath is a pidl</summary>
		PIDL = 0x000000008,
		/// <summary>use passed dwFileAttribute</summary>
		UseFileAttributes = 0x000000010,
		/// <summary>apply the appropriate overlays</summary>
		AddOverlays = 0x000000020,
		/// <summary>Get the index of the overlay in the upper 8 bits of the iIcon</summary>
		OverlayIndex = 0x000000040,
	}

	public enum MonitorFrom {
		None = 0,
		Primary = 1,
		Nearest = 2,
	}

	public enum CursorName : int {
		Arrow = 32512
	}

	[Flags]
	[CLSCompliant(false)]
	public enum TrackMouseEventFlags : uint {
		HOVER = 0x00000001,
		LEAVE = 0x00000002,
		NONCLIENT = 0x00000010,
		QUERY = 0x40000000,
		CANCEL = 0x80000000,
	}

	public enum MouseActivate {
		ACTIVATE = 1,
		ACTIVATEANDEAT = 2,
		NOACTIVATE = 3,
		NOACTIVATEANDEAT = 4,
	}

	public enum DeviceNotification {
		WINDOW_HANDLE = 0x00000000,
		SERVICE_HANDLE = 0x00000001,
		ALL_INTERFACE_CLASSES = 0x00000004,
	}

	public enum DeviceBroadcastType {
		OEM = 0,
		VOLUME = 2,
		PORT = 3,
		INTERFACE = 5,
		HANDLE = 6,
	}

	[CLSCompliant(false)]
	public delegate IntPtr WindowProcedure(IntPtr handle, WindowMessage message, IntPtr wParam, IntPtr lParam);

	[StructLayout(LayoutKind.Sequential)]
	[Serializable]
	public struct MSG {
		public static readonly int SizeOfMSG = Marshal.SizeOf(typeof(MSG));
		public IntPtr HWnd;
		[CLSCompliant(false)]
		public WindowMessage Message;
		public IntPtr WParam;
		public IntPtr LParam;
		[CLSCompliant(false)]
		public uint Time;
		public POINT Point;
		//public object RefObject;

		public override string ToString() {
			return string.Format("msg=0x{0:x} ({1}) hwnd=0x{2:x} wparam=0x{3:x} lparam=0x{4:x} pt=0x{5:x}", (int) Message, Message.ToString(), HWnd.ToInt32(), WParam.ToInt32(), LParam.ToInt32(), Point);
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct POINT {
		public static readonly int Size = Marshal.SizeOf(typeof(POINT));
		public int X;
		public int Y;

		public POINT(int x, int y) {
			this.X = x;
			this.Y = y;
		}

		public POINT(Point point) {
			this.X = point.X;
			this.Y = point.Y;
		}

		public Point ToPoint() {
			return new Point(X, Y);
		}

		public override string ToString() {
			return "Point {" + X + ", " + Y + ")";
		}
	}
}

#pragma warning restore 1591