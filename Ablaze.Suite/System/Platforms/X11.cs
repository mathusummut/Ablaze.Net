#pragma warning disable 1591

using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace System.Platforms.X11 {
	[Flags]
	public enum XVisualInfoMask {
		No = 0x0,
		ID = 0x1,
		Screen = 0x2,
		Depth = 0x4,
		Class = 0x8,
		Red = 0x10,
		Green = 0x20,
		Blue = 0x40,
		ColormapSize = 0x80,
		BitsPerRGB = 0x100,
		All = 0x1FF,
	}

	public enum XVisualClass : int {
		StaticGray = 0,
		GrayScale = 1,
		StaticColor = 2,
		PseudoColor = 3,
		TrueColor = 4,
		DirectColor = 5,
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct XVisualInfo {
		public IntPtr Visual;
		public IntPtr VisualID;
		public int Screen;
		public int Depth;
		public XVisualClass Class;
		public long RedMask;
		public long GreenMask;
		public long blueMask;
		public int ColormapSize;
		public int BitsPerRgb;

		public override string ToString() {
			return String.Format("id ({0}), screen ({1}), depth ({2}), class ({3})",
				VisualID, Screen, Depth, Class);
		}
	}






	[StructLayout(LayoutKind.Sequential)]
	public sealed class SetWindowAttributes {
		/// <summary>
		/// background, None, or ParentRelative
		/// </summary>
		public IntPtr background_pixmap;
		/// <summary>
		/// background pixel
		/// </summary>
		public long background_pixel;
		/// <summary>
		/// border of the window or CopyFromParent
		/// </summary>
		public IntPtr border_pixmap;
		/// <summary>
		/// border pixel value
		/// </summary>
		public long border_pixel;
		/// <summary>
		/// one of bit gravity values
		/// </summary>
		public int bit_gravity;
		/// <summary>
		/// one of the window gravity values
		/// </summary>
		public int win_gravity;
		/// <summary>
		/// NotUseful, WhenMapped, Always
		/// </summary>
		public int backing_store;
		/// <summary>
		/// planes to be preserved if possible
		/// </summary>
		public long backing_planes;
		/// <summary>
		/// value to use in restoring planes
		/// </summary>
		public long backing_pixel;
		/// <summary>
		/// should bits under be saved? (popups)
		/// </summary>
		public bool save_under;
		/// <summary>
		/// set of events that should be saved
		/// </summary>
		public EventMask event_mask;
		/// <summary>
		/// set of events that should not propagate
		/// </summary>
		public long do_not_propagate_mask;
		/// <summary>
		/// boolean value for override_redirect
		/// </summary>
		public bool override_redirect;
		/// <summary>
		/// color map to be associated with window
		/// </summary>
		public IntPtr colormap;
		/// <summary>
		/// cursor to be displayed (or None)
		/// </summary>
		public IntPtr cursor;
	}





	[StructLayout(LayoutKind.Sequential)]
	public struct SizeHints {
		public long flags;         /* marks which fields in this structure are defined */
		public int x, y;
		public int width, height;
		public int min_width, min_height;
		public int max_width, max_height;
		public int width_inc, height_inc;
		public Rectangle min_aspect, max_aspect;
		public int base_width, base_height;
		public int win_gravity;
		public struct Rectangle {
			public int x;       /* numerator */
			public int y;       /* denominator */
		}
		/* this structure may be extended in the future */
	}





	public struct XRRScreenSize {
		public static int Size = Marshal.SizeOf(typeof(XRRScreenSize));
		public int Width, Height;
		public int MWidth, MHeight;
	};





	public struct Screen {
		public XExtData ext_data;    /* hook for extension to hang buffer */
		public IntPtr display;     /* back pointer to display structure */ /* _XDisplay */
		public IntPtr root;        /* Root window id. */
		public int width, height;    /* width and height of screen */
		public int mwidth, mheight;    /* width and height of  in millimeters */
		public int ndepths;        /* number of depths possible */
								   //Depth *depths;        /* list of allowable depths on the screen */
		public int root_depth;        /* bits per pixel */
									  //Visual* root_visual;    /* root visual */
		public IntPtr default_gc;        /* GC for the root root visual */   // GC
		public IntPtr cmap;        /* default color map */
		[CLSCompliant(false)]
		public UIntPtr white_pixel;    // unsigned long
		[CLSCompliant(false)]
		public UIntPtr black_pixel;    /* White and Black pixel values */  // unsigned long
		public int max_maps, min_maps;    /* max and min color maps */
		public int backing_store;    /* Never, WhenMapped, Always */
		public Boolean save_unders;
		public long root_input_mask;    /* initial root input mask */
	}





	public sealed class XExtData {
		public int number;        /* number returned by XRegisterExtension */
		public XExtData next;    /* next item on list of buffer for structure */
		public delegate int FreePrivateDelegate(XExtData extension);
		public FreePrivateDelegate FreePrivate;    /* called to free private storage */
		public IntPtr private_data;    /* buffer private to this extension. */
	};





	[StructLayout(LayoutKind.Sequential)]
	public struct MotifWmHints {
		public IntPtr flags;
		public IntPtr functions;
		public IntPtr decorations;
		public IntPtr input_mode;
		public IntPtr status;

		public override string ToString() {
			return string.Format("MotifWmHints <flags={0}, functions={1}, decorations={2}, input_mode={3}, status={4}", (MotifFlags) flags.ToInt32(), (MotifFunctions) functions.ToInt32(), (MotifDecorations) decorations.ToInt32(), (MotifInputMode) input_mode.ToInt32(), status.ToInt32());
		}
	}

	[Flags]
	public enum MotifFlags {
		Functions = 1,
		Decorations = 2,
		InputMode = 4,
		Status = 8
	}

	[Flags]
	public enum MotifFunctions {
		All = 0x01,
		Resize = 0x02,
		Move = 0x04,
		Minimize = 0x08,
		Maximize = 0x10,
		Close = 0x20
	}

	[Flags]
	public enum MotifDecorations {
		All = 0x01,
		Border = 0x02,
		ResizeH = 0x04,
		Title = 0x08,
		Menu = 0x10,
		Minimize = 0x20,
		Maximize = 0x40,

	}

	[Flags]
	public enum MotifInputMode {
		Modeless = 0,
		ApplicationModal = 1,
		SystemModal = 2,
		FullApplicationModal = 3
	}







	public struct Constants {
		public const int QueuedAlready = 0;
		public const int QueuedAfterReading = 1;
		public const int QueuedAfterFlush = 2;

		public const int CopyFromParent = 0;
		public const int CWX = 1;
		public const int InputOutput = 1;
		public const int InputOnly = 2;

		/* The hints we recognize */
		public const string XA_WIN_PROTOCOLS = "_WIN_PROTOCOLS";
		public const string XA_WIN_ICONS = "_WIN_ICONS";
		public const string XA_WIN_WORKSPACE = "_WIN_WORKSPACE";
		public const string XA_WIN_WORKSPACE_COUNT = "_WIN_WORKSPACE_COUNT";
		public const string XA_WIN_WORKSPACE_NAMES = "_WIN_WORKSPACE_NAMES";
		public const string XA_WIN_LAYER = "_WIN_LAYER";
		public const string XA_WIN_STATE = "_WIN_STATE";
		public const string XA_WIN_HINTS = "_WIN_HINTS";
		public const string XA_WIN_WORKAREA = "_WIN_WORKAREA";
		public const string XA_WIN_CLIENT_LIST = "_WIN_CLIENT_LIST";
		public const string XA_WIN_APP_STATE = "_WIN_APP_STATE";
		public const string XA_WIN_EXPANDED_SIZE = "_WIN_EXPANDED_SIZE";
		public const string XA_WIN_CLIENT_MOVING = "_WIN_CLIENT_MOVING";
		public const string XA_WIN_SUPPORTING_WM_CHECK = "_WIN_SUPPORTING_WM_CHECK";
	}

	public enum WindowLayer {
		Desktop = 0,
		Below = 2,
		Normal = 4,
		OnTop = 6,
		Dock = 8,
		AboveDock = 10,
		Menu = 12,
	}

	public enum WindowState {
		Sticky = (1 << 0), /* everyone knows sticky */
		Minimized = (1 << 1),
		MaximizedVertically = (1 << 2), /* window in maximized V state */
		MaximizedHorizontally = (1 << 3), /* window in maximized H state */
		Hidden = (1 << 4), /* not on taskbar but window visible */
		Shaded = (1 << 5), /* shaded (NeXT style), */
		HID_WORKSPACE = (1 << 6), /* not on current desktop */
		HID_TRANSIENT = (1 << 7), /* owner of transient is hidden */
		FixedPosition = (1 << 8), /* window is fixed in position even */
		ArrangeIgnore = (1 << 9),  /* ignore for auto arranging */
	}

	public enum WindowHints {
		SkipFocus = (1 << 0), /* "alt-tab" skips this win */
		SkipWinlist = (1 << 1), /* not in win list */
		SkipTaskbar = (1 << 2), /* not on taskbar */
		GroupTransient = (1 << 3),
		FocusOnClick = (1 << 4), /* app only accepts focus when clicked */
		DoNotCover = (1 << 5),  /* attempt to not cover this window */
	}

	public enum ErrorCodes : int {
		Success = 0,
		BadRequest = 1,
		BadValue = 2,
		BadWindow = 3,
		BadPixmap = 4,
		BadAtom = 5,
		BadCursor = 6,
		BadFont = 7,
		BadMatch = 8,
		BadDrawable = 9,
		BadAccess = 10,
		BadAlloc = 11,
		BadColor = 12,
		BadGC = 13,
		BadIDChoice = 14,
		BadName = 15,
		BadLength = 16,
		BadImplementation = 17,
	}

	[Flags]
	public enum CreateWindowMask : long//: ulong
	{
		CWBackPixmap = (1L << 0),
		CWBackPixel = (1L << 1),
		CWSaveUnder = (1L << 10),
		CWEventMask = (1L << 11),
		CWDontPropagate = (1L << 12),
		CWColormap = (1L << 13),
		CWCursor = (1L << 14),
		CWBorderPixmap = (1L << 2),
		CWBorderPixel = (1L << 3),
		CWBitGravity = (1L << 4),
		CWWinGravity = (1L << 5),
		CWBackingStore = (1L << 6),
		CWBackingPlanes = (1L << 7),
		CWBackingPixel = (1L << 8),
		CWOverrideRedirect = (1L << 9),

		//CWY    = (1<<1),
		//CWWidth    = (1<<2),
		//CWHeight    = (1<<3),
		//CWBorderWidth    = (1<<4),
		//CWSibling    = (1<<5),
		//CWStackMode    = (1<<6),
	}



	/// <summary>
	/// Defines LATIN-1 and miscellaneous keys.
	/// </summary>
	public enum XKey {
		/*
		 * TTY function keys, cleverly chosen to map to ASCII, for convenience of
		 * programming, but could have been arbitrary (at the cost of lookup
		 * tables in client code).
		 */

		BackSpace = 0xff08,  /* Back space, back char */
		Tab = 0xff09,
		Linefeed = 0xff0a,  /* Linefeed, LF */
		Clear = 0xff0b,
		Return = 0xff0d,  /* Return, enter */
		Pause = 0xff13,  /* Pause, hold */
		Scroll_Lock = 0xff14,
		Sys_Req = 0xff15,
		Escape = 0xff1b,
		Delete = 0xffff,  /* Delete, rubout */



		/* International & multi-key character composition */

		Multi_key = 0xff20,  /* Multi-key character compose */
		Codeinput = 0xff37,
		SingleCandidate = 0xff3c,
		MultipleCandidate = 0xff3d,
		PreviousCandidate = 0xff3e,

		/* Japanese keyboard support */

		Kanji = 0xff21,  /* Kanji, Kanji convert */
		Muhenkan = 0xff22,  /* Cancel Conversion */
		Henkan_Mode = 0xff23,  /* Start/Stop Conversion */
		Henkan = 0xff23,  /* Alias for Henkan_Mode */
		Romaji = 0xff24,  /* to Romaji */
		Hiragana = 0xff25,  /* to Hiragana */
		Katakana = 0xff26,  /* to Katakana */
		Hiragana_Katakana = 0xff27,  /* Hiragana/Katakana toggle */
		Zenkaku = 0xff28,  /* to Zenkaku */
		Hankaku = 0xff29,  /* to Hankaku */
		Zenkaku_Hankaku = 0xff2a,  /* Zenkaku/Hankaku toggle */
		Touroku = 0xff2b,  /* Add to Dictionary */
		Massyo = 0xff2c,  /* Delete from Dictionary */
		Kana_Lock = 0xff2d,  /* Kana Lock */
		Kana_Shift = 0xff2e,  /* Kana Shift */
		Eisu_Shift = 0xff2f,  /* Alphanumeric Shift */
		Eisu_toggle = 0xff30,  /* Alphanumeric toggle */
		Kanji_Bangou = 0xff37,  /* Codeinput */
		Zen_Koho = 0xff3d,  /* Multiple/All Candidate(s) */
		Mae_Koho = 0xff3e,  /* Previous Candidate */

		/* 0xff31 thru 0xff3f are under XK_KOREAN */

		/* Cursor control & motion */

		Home = 0xff50,
		Left = 0xff51,  /* Move left, left arrow */
		Up = 0xff52,  /* Move up, up arrow */
		Right = 0xff53,  /* Move right, right arrow */
		Down = 0xff54,  /* Move down, down arrow */
		Prior = 0xff55,  /* Prior, previous */
		Page_Up = 0xff55,
		Next = 0xff56,  /* Next */
		Page_Down = 0xff56,
		End = 0xff57,  /* EOL */
		Begin = 0xff58,  /* BOL */


		/* Misc functions */

		Select = 0xff60,  /* Select, mark */
		Print = 0xff61,
		Execute = 0xff62,  /* Execute, run, do */
		Insert = 0xff63,  /* Insert, insert here */
		Undo = 0xff65,
		Redo = 0xff66,  /* Redo, again */
		Menu = 0xff67,
		Find = 0xff68,  /* Find, search */
		Cancel = 0xff69,  /* Cancel, stop, abort, exit */
		Help = 0xff6a,  /* Help */
		Break = 0xff6b,
		Mode_switch = 0xff7e,  /* Character set switch */
		script_switch = 0xff7e,  /* Alias for mode_switch */
		Num_Lock = 0xff7f,

		/* Keypad functions, keypad numbers cleverly chosen to map to ASCII */

		KP_Space = 0xff80,  /* Space */
		KP_Tab = 0xff89,
		KP_Enter = 0xff8d,  /* Enter */
		KP_F1 = 0xff91,  /* PF1, KP_A, ... */
		KP_F2 = 0xff92,
		KP_F3 = 0xff93,
		KP_F4 = 0xff94,
		KP_Home = 0xff95,
		KP_Left = 0xff96,
		KP_Up = 0xff97,
		KP_Right = 0xff98,
		KP_Down = 0xff99,
		KP_Prior = 0xff9a,
		KP_Page_Up = 0xff9a,
		KP_Next = 0xff9b,
		KP_Page_Down = 0xff9b,
		KP_End = 0xff9c,
		KP_Begin = 0xff9d,
		KP_Insert = 0xff9e,
		KP_Delete = 0xff9f,
		KP_Equal = 0xffbd,  /* Equals */
		KP_Multiply = 0xffaa,
		KP_Add = 0xffab,
		KP_Separator = 0xffac,  /* Separator, often comma */
		KP_Subtract = 0xffad,
		KP_Decimal = 0xffae,
		KP_Divide = 0xffaf,

		KP_0 = 0xffb0,
		KP_1 = 0xffb1,
		KP_2 = 0xffb2,
		KP_3 = 0xffb3,
		KP_4 = 0xffb4,
		KP_5 = 0xffb5,
		KP_6 = 0xffb6,
		KP_7 = 0xffb7,
		KP_8 = 0xffb8,
		KP_9 = 0xffb9,

		/*
		 * Auxiliary functions; note the duplicate definitions for left and right
		 * function keys;  Sun keyboards and a few other manufacturers have such
		 * function key groups on the left and/or right sides of the keyboard.
		 * We've not found a keyboard with more than 35 function keys total.
		 */

		F1 = 0xffbe,
		F2 = 0xffbf,
		F3 = 0xffc0,
		F4 = 0xffc1,
		F5 = 0xffc2,
		F6 = 0xffc3,
		F7 = 0xffc4,
		F8 = 0xffc5,
		F9 = 0xffc6,
		F10 = 0xffc7,
		F11 = 0xffc8,
		L1 = 0xffc8,
		F12 = 0xffc9,
		L2 = 0xffc9,
		F13 = 0xffca,
		L3 = 0xffca,
		F14 = 0xffcb,
		L4 = 0xffcb,
		F15 = 0xffcc,
		L5 = 0xffcc,
		F16 = 0xffcd,
		L6 = 0xffcd,
		F17 = 0xffce,
		L7 = 0xffce,
		F18 = 0xffcf,
		L8 = 0xffcf,
		F19 = 0xffd0,
		L9 = 0xffd0,
		F20 = 0xffd1,
		L10 = 0xffd1,
		F21 = 0xffd2,
		R1 = 0xffd2,
		F22 = 0xffd3,
		R2 = 0xffd3,
		F23 = 0xffd4,
		R3 = 0xffd4,
		F24 = 0xffd5,
		R4 = 0xffd5,
		F25 = 0xffd6,
		R5 = 0xffd6,
		F26 = 0xffd7,
		R6 = 0xffd7,
		F27 = 0xffd8,
		R7 = 0xffd8,
		F28 = 0xffd9,
		R8 = 0xffd9,
		F29 = 0xffda,
		R9 = 0xffda,
		F30 = 0xffdb,
		R10 = 0xffdb,
		F31 = 0xffdc,
		R11 = 0xffdc,
		F32 = 0xffdd,
		R12 = 0xffdd,
		F33 = 0xffde,
		R13 = 0xffde,
		F34 = 0xffdf,
		R14 = 0xffdf,
		F35 = 0xffe0,
		R15 = 0xffe0,

		/* Modifiers */

		Shift_L = 0xffe1,  /* Left shift */
		Shift_R = 0xffe2,  /* Right shift */
		Control_L = 0xffe3,  /* Left control */
		Control_R = 0xffe4,  /* Right control */
		Caps_Lock = 0xffe5,  /* Caps lock */
		Shift_Lock = 0xffe6,  /* Shift lock */

		Meta_L = 0xffe7,  /* Left meta */
		Meta_R = 0xffe8,  /* Right meta */
		Alt_L = 0xffe9,  /* Left alt */
		Alt_R = 0xffea,  /* Right alt */
		Super_L = 0xffeb,  /* Left super */
		Super_R = 0xffec,  /* Right super */
		Hyper_L = 0xffed,  /* Left hyper */
		Hyper_R = 0xffee,  /* Right hyper */

		ISO_Level3_Shift = 0xfe03,

		/*
		 * Latin 1
		 * (ISO/IEC 8859-1 = Unicode U+0020..U+00FF)
		 * Byte 3 = 0
		 */

		space = 0x0020,  /* U+0020 SPACE */
		exclam = 0x0021,  /* U+0021 EXCLAMATION MARK */
		quotedbl = 0x0022,  /* U+0022 QUOTATION MARK */
		numbersign = 0x0023,  /* U+0023 NUMBER SIGN */
		dollar = 0x0024,  /* U+0024 DOLLAR SIGN */
		percent = 0x0025,  /* U+0025 PERCENT SIGN */
		ampersand = 0x0026,  /* U+0026 AMPERSAND */
		apostrophe = 0x0027,  /* U+0027 APOSTROPHE */
		quoteright = 0x0027,  /* deprecated */
		parenleft = 0x0028,  /* U+0028 LEFT PARENTHESIS */
		parenright = 0x0029,  /* U+0029 RIGHT PARENTHESIS */
		asterisk = 0x002a,  /* U+002A ASTERISK */
		plus = 0x002b,  /* U+002B PLUS SIGN */
		comma = 0x002c,  /* U+002C COMMA */
		minus = 0x002d,  /* U+002D HYPHEN-MINUS */
		period = 0x002e,  /* U+002E FULL STOP */
		slash = 0x002f,  /* U+002F SOLIDUS */
		Number0 = 0x0030,  /* U+0030 DIGIT ZERO */
		Number1 = 0x0031,  /* U+0031 DIGIT ONE */
		Number2 = 0x0032,  /* U+0032 DIGIT TWO */
		Number3 = 0x0033,  /* U+0033 DIGIT THREE */
		Number4 = 0x0034,  /* U+0034 DIGIT FOUR */
		Number5 = 0x0035,  /* U+0035 DIGIT FIVE */
		Number6 = 0x0036,  /* U+0036 DIGIT SIX */
		Number7 = 0x0037,  /* U+0037 DIGIT SEVEN */
		Number8 = 0x0038,  /* U+0038 DIGIT EIGHT */
		Number9 = 0x0039,  /* U+0039 DIGIT NINE */
		colon = 0x003a,  /* U+003A COLON */
		semicolon = 0x003b,  /* U+003B SEMICOLON */
		less = 0x003c,  /* U+003C LESS-THAN SIGN */
		equal = 0x003d,  /* U+003D EQUALS SIGN */
		greater = 0x003e,  /* U+003E GREATER-THAN SIGN */
		question = 0x003f,  /* U+003F QUESTION MARK */
		at = 0x0040,  /* U+0040 COMMERCIAL AT */
		AUpper = 0x0041,  /* U+0041 LATIN CAPITAL LETTER A */
		BUpper = 0x0042,  /* U+0042 LATIN CAPITAL LETTER B */
		CUpper = 0x0043,  /* U+0043 LATIN CAPITAL LETTER C */
		DUpper = 0x0044,  /* U+0044 LATIN CAPITAL LETTER D */
		EUpper = 0x0045,  /* U+0045 LATIN CAPITAL LETTER E */
		FUpper = 0x0046,  /* U+0046 LATIN CAPITAL LETTER F */
		GUpper = 0x0047,  /* U+0047 LATIN CAPITAL LETTER G */
		HUpper = 0x0048,  /* U+0048 LATIN CAPITAL LETTER H */
		IUpper = 0x0049,  /* U+0049 LATIN CAPITAL LETTER I */
		JUpper = 0x004a,  /* U+004A LATIN CAPITAL LETTER J */
		KUpper = 0x004b,  /* U+004B LATIN CAPITAL LETTER K */
		LUpper = 0x004c,  /* U+004C LATIN CAPITAL LETTER L */
		MUpper = 0x004d,  /* U+004D LATIN CAPITAL LETTER M */
		NUpper = 0x004e,  /* U+004E LATIN CAPITAL LETTER N */
		OUpper = 0x004f,  /* U+004F LATIN CAPITAL LETTER O */
		PUpper = 0x0050,  /* U+0050 LATIN CAPITAL LETTER P */
		QUpper = 0x0051,  /* U+0051 LATIN CAPITAL LETTER Q */
		RUpper = 0x0052,  /* U+0052 LATIN CAPITAL LETTER R */
		SUpper = 0x0053,  /* U+0053 LATIN CAPITAL LETTER S */
		TUpper = 0x0054,  /* U+0054 LATIN CAPITAL LETTER T */
		UUpper = 0x0055,  /* U+0055 LATIN CAPITAL LETTER U */
		VUpper = 0x0056,  /* U+0056 LATIN CAPITAL LETTER V */
		WUpper = 0x0057,  /* U+0057 LATIN CAPITAL LETTER W */
		XUpper = 0x0058,  /* U+0058 LATIN CAPITAL LETTER X */
		YUpper = 0x0059,  /* U+0059 LATIN CAPITAL LETTER Y */
		ZUpper = 0x005a,  /* U+005A LATIN CAPITAL LETTER Z */
		bracketleft = 0x005b,  /* U+005B LEFT SQUARE BRACKET */
		backslash = 0x005c,  /* U+005C REVERSE SOLIDUS */
		bracketright = 0x005d,  /* U+005D RIGHT SQUARE BRACKET */
		asciicircum = 0x005e,  /* U+005E CIRCUMFLEX ACCENT */
		underscore = 0x005f,  /* U+005F LOW LINE */
		grave = 0x0060,  /* U+0060 GRAVE ACCENT */
		quoteleft = 0x0060,  /* deprecated */
		aLower = 0x0061,  /* U+0061 LATIN SMALL LETTER A */
		bLower = 0x0062,  /* U+0062 LATIN SMALL LETTER B */
		cLower = 0x0063,  /* U+0063 LATIN SMALL LETTER C */
		dLower = 0x0064,  /* U+0064 LATIN SMALL LETTER D */
		eLower = 0x0065,  /* U+0065 LATIN SMALL LETTER E */
		fLower = 0x0066,  /* U+0066 LATIN SMALL LETTER F */
		gLower = 0x0067,  /* U+0067 LATIN SMALL LETTER G */
		hLower = 0x0068,  /* U+0068 LATIN SMALL LETTER H */
		iLower = 0x0069,  /* U+0069 LATIN SMALL LETTER I */
		jLower = 0x006a,  /* U+006A LATIN SMALL LETTER J */
		kLower = 0x006b,  /* U+006B LATIN SMALL LETTER K */
		lLower = 0x006c,  /* U+006C LATIN SMALL LETTER L */
		mLower = 0x006d,  /* U+006D LATIN SMALL LETTER M */
		nLower = 0x006e,  /* U+006E LATIN SMALL LETTER N */
		oLower = 0x006f,  /* U+006F LATIN SMALL LETTER O */
		pLower = 0x0070,  /* U+0070 LATIN SMALL LETTER P */
		qLower = 0x0071,  /* U+0071 LATIN SMALL LETTER Q */
		rLower = 0x0072,  /* U+0072 LATIN SMALL LETTER R */
		sLower = 0x0073,  /* U+0073 LATIN SMALL LETTER S */
		tLower = 0x0074,  /* U+0074 LATIN SMALL LETTER T */
		uLower = 0x0075,  /* U+0075 LATIN SMALL LETTER U */
		vLower = 0x0076,  /* U+0076 LATIN SMALL LETTER V */
		wLower = 0x0077,  /* U+0077 LATIN SMALL LETTER W */
		xLower = 0x0078,  /* U+0078 LATIN SMALL LETTER X */
		yLower = 0x0079,  /* U+0079 LATIN SMALL LETTER Y */
		zLower = 0x007a,  /* U+007A LATIN SMALL LETTER Z */
		braceleft = 0x007b,  /* U+007B LEFT CURLY BRACKET */
		bar = 0x007c,  /* U+007C VERTICAL LINE */
		braceright = 0x007d,  /* U+007D RIGHT CURLY BRACKET */
		asciitilde = 0x007e,  /* U+007E TILDE */

		// Extra keys

		XF86AudioMute = 0x1008ff12,
		XF86AudioLowerVolume = 0x1008ff11,
		XF86AudioRaiseVolume = 0x1008ff13,
		XF86PowerOff = 0x1008ff2a,
		XF86Suspend = 0x1008ffa7,
		XF86Copy = 0x1008ff57,
		XF86Paste = 0x1008ff6d,
		XF86Cut = 0x1008ff58,
		XF86MenuKB = 0x1008ff65,
		XF86Calculator = 0x1008ff1d,
		XF86Sleep = 0x1008ff2f,
		XF86WakeUp = 0x1008ff2b,
		XF86Explorer = 0x1008ff5d,
		XF86Send = 0x1008ff7b,
		XF86Xfer = 0x1008ff8a,
		XF86Launch1 = 0x1008ff41,
		XF86Launch2 = 0x1008ff42,
		XF86Launch3 = 0x1008ff43,
		XF86Launch4 = 0x1008ff44,
		XF86Launch5 = 0x1008ff45,
		XF86LaunchA = 0x1008ff4a,
		XF86LaunchB = 0x1008ff4b,
		XF86WWW = 0x1008ff2e,
		XF86DOS = 0x1008ff5a,
		XF86ScreenSaver = 0x1008ff2d,
		XF86RotateWindows = 0x1008ff74,
		XF86Mail = 0x1008ff19,
		XF86Favorites = 0x1008ff30,
		XF86MyComputer = 0x1008ff33,
		XF86Back = 0x1008ff26,
		XF86Forward = 0x1008ff27,
		XF86Eject = 0x1008ff2c,
		XF86AudioPlay = 0x1008ff14,
		XF86AudioStop = 0x1008ff15,
		XF86AudioPrev = 0x1008ff16,
		XF86AudioNext = 0x1008ff17,
		XF86AudioRecord = 0x1008ff1c,
		XF86AudioPause = 0x1008ff31,
		XF86AudioRewind = 0x1008ff3e,
		XF86AudioForward = 0x1008ff97,
		XF86Phone = 0x1008ff6e,
		XF86Tools = 0x1008ff81,
		XF86HomePage = 0x1008ff18,
		XF86Close = 0x1008ff56,
		XF86Reload = 0x1008ff73,
		XF86ScrollUp = 0x1008ff78,
		XF86ScrollDown = 0x1008ff79,
		XF86New = 0x1008ff68,
		XF86TouchpadToggle = 0x1008ffa9,
		XF86WebCam = 0x1008ff8f,
		XF86Search = 0x1008ff1b,
		XF86Finance = 0x1008ff3c,
		XF86Shop = 0x1008ff36,
		XF86MonBrightnessDown = 0x1008ff03,
		XF86MonBrightnessUp = 0x1008ff02,
		XF86AudioMedia = 0x1008ff32,
		XF86Display = 0x1008ff59,
		XF86KbdLightOnOff = 0x1008ff04,
		XF86KbdBrightnessDown = 0x1008ff06,
		XF86KbdBrightnessUp = 0x1008ff05,
		XF86Reply = 0x1008ff72,
		XF86MailForward = 0x1008ff90,
		XF86Save = 0x1008ff77,
		XF86Documents = 0x1008ff5b,
		XF86Battery = 0x1008ff93,
		XF86Bluetooth = 0x1008ff94,
		XF86WLAN = 0x1008ff95,

		SunProps = 0x1005ff70,
		SunOpen = 0x1005ff73,
	}





	public enum MouseMask {
		Button1MotionMask = (1 << 8),
		Button2MotionMask = (1 << 9),
		Button3MotionMask = (1 << 10),
		Button4MotionMask = (1 << 11),
		Button5MotionMask = (1 << 12),
		Button1Mask = (1 << 8),
		Button2Mask = (1 << 9),
		Button3Mask = (1 << 10),
		Button4Mask = (1 << 11),
		Button5Mask = (1 << 12),
		Button6Mask = (1 << 13),
		Button7Mask = (1 << 14),
		Button8Mask = (1 << 15),
		ShiftMask = (1 << 0),
		LockMask = (1 << 1),
		ControlMask = (1 << 2),
		Mod1Mask = (1 << 3),
		Mod2Mask = (1 << 4),
		Mod3Mask = (1 << 5),
		Mod4Mask = (1 << 6),
		Mod5Mask = (1 << 7),
	}




	[SuppressUnmanagedCodeSecurity]
	public static class NativeApi {
		public const string Library = "libX11";
		public static readonly object SyncRoot = new object();



		public const string DllNameVid = "libXxf86vm";

		public static readonly IntPtr DefaultDisplay;
		public static readonly int ScreenCount;



		static NativeApi() {
			try {
				XInitThreads();
				DefaultDisplay = XOpenDisplay(IntPtr.Zero);
				if (DefaultDisplay == IntPtr.Zero)
					return;
				XLockDisplay(DefaultDisplay);
				ScreenCount = XScreenCount(DefaultDisplay);
				XUnlockDisplay(DefaultDisplay);
			} catch {
			}
		}



		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XCreateWindow")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static IntPtr CreateWindow(
			IntPtr display,
			IntPtr parent,
			int x, int y,
			//uint width, uint height,
			int width, int height,
			//uint border_width,
			int border_width,
			int depth,
			//uint @class,
			int @class,
			IntPtr visual,
			[MarshalAs(UnmanagedType.SysUInt)] CreateWindowMask valuemask,
			SetWindowAttributes attributes
		);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XCreateSimpleWindow")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static IntPtr CreateSimpleWindow(
			IntPtr display,
			IntPtr parent,
			int x, int y,
			int width, int height,
			int border_width,
			long border,
			long background
		);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XResizeWindow")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static int XResizeWindow(IntPtr display, IntPtr window, int width, int height);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XDestroyWindow")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static void DestroyWindow(IntPtr display, IntPtr window);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XMapWindow")]
		[Security.SuppressUnmanagedCodeSecurity]
		extern public static void MapWindow(IntPtr display, IntPtr window);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XMapRaised")]
		[Security.SuppressUnmanagedCodeSecurity]
		extern public static void MapRaised(IntPtr display, IntPtr window);



		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XDefaultVisual")]
		[Security.SuppressUnmanagedCodeSecurity]
		extern public static IntPtr DefaultVisual(IntPtr display, int screen_number);



		/// <summary>
		/// Frees the memory used by an X structure. Only use on unmanaged structures!
		/// </summary>
		/// <param name="buffer">A pointer to the structure that will be freed.</param>
		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XFree")]
		[Security.SuppressUnmanagedCodeSecurity]
		extern public static void Free(IntPtr buffer);





		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XEventsQueued")]
		extern public static int EventsQueued(IntPtr display, int mode);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XPending")]
		extern public static int Pending(IntPtr display);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XNextEvent")]
		extern public static void NextEvent(
			IntPtr display,
			[MarshalAs(UnmanagedType.AsAny)][In, Out]object e);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XNextEvent")]
		[Security.SuppressUnmanagedCodeSecurity]
		extern public static void NextEvent(IntPtr display, [In, Out] IntPtr e);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XPeekEvent")]
		[Security.SuppressUnmanagedCodeSecurity]
		extern public static void PeekEvent(
			IntPtr display,
			[MarshalAs(UnmanagedType.AsAny)][In, Out]object event_return
		);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XPeekEvent")]
		[Security.SuppressUnmanagedCodeSecurity]
		extern public static void PeekEvent(IntPtr display, [In, Out]XEvent event_return);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XSendEvent")]
		[Security.SuppressUnmanagedCodeSecurity]
		[return: MarshalAs(UnmanagedType.Bool)]
		extern public static bool SendEvent(IntPtr display, IntPtr window, bool propagate,
			[MarshalAs(UnmanagedType.SysInt)]EventMask event_mask, ref XEvent event_send);

		/// <summary>
		/// The XSelectInput() function requests that the X server report the events associated
		/// with the specified event mask.
		/// </summary>
		/// <param name="display">Specifies the connection to the X server.</param>
		/// <param name="w">Specifies the window whose events you are interested in.</param>
		/// <param name="event_mask">Specifies the event mask.</param>
		/// <remarks>
		/// Initially, X will not report any of these events.
		/// Events are reported relative to a window.
		/// If a window is not interested in a device event,
		/// it usually propagates to the closest ancestor that is interested,
		/// unless the do_not_propagate mask prohibits it.
		/// Setting the event-mask attribute of a window overrides any previous call for the same window but not for other clients. Multiple clients can select for the same events on the same window with the following restrictions: 
		/// <para>Multiple clients can select events on the same window because their event masks are disjoint. When the X server generates an event, it reports it to all interested clients. </para>
		/// <para>Only one client at a time can select CirculateRequest, ConfigureRequest, or MapRequest events, which are associated with the event mask SubstructureRedirectMask. </para>
		/// <para>Only one client at a time can select a ResizeRequest event, which is associated with the event mask ResizeRedirectMask. </para>
		/// <para>Only one client at a time can select a ButtonPress event, which is associated with the event mask ButtonPressMask. </para>
		/// <para>The server reports the event to all interested clients. </para>
		/// <para>XSelectInput() can generate a BadWindow error.</para>
		/// </remarks>
		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XSelectInput")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern void SelectInput(IntPtr display, IntPtr w, EventMask event_mask);

		/// <summary>
		/// When the predicate procedure finds a match, XCheckIfEvent() copies the matched event into the client-supplied XEvent structure and returns True. (This event is removed from the queue.) If the predicate procedure finds no match, XCheckIfEvent() returns False, and the output buffer will have been flushed. All earlier events stored in the queue are not discarded.
		/// </summary>
		/// <param name="display">Specifies the connection to the X server.</param>
		/// <param name="event_return">Returns a copy of the matched event's associated structure.</param>
		/// <param name="predicate">Specifies the procedure that is to be called to determine if the next event in the queue matches what you want</param>
		/// <param name="arg">Specifies the user-supplied argument that will be passed to the predicate procedure.</param>
		/// <returns>true if the predicate returns true for some event, false otherwise</returns>
		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XCheckIfEvent")]
		[Security.SuppressUnmanagedCodeSecurity]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool CheckIfEvent(IntPtr display, ref XEvent event_return,
			/*[MarshalAs(UnmanagedType.FunctionPtr)] */ CheckEventPredicate predicate, /*XPointer*/ IntPtr arg);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XIfEvent")]
		[Security.SuppressUnmanagedCodeSecurity]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool IfEvent(IntPtr display, ref XEvent event_return,
			/*[MarshalAs(UnmanagedType.FunctionPtr)] */ CheckEventPredicate predicate, /*XPointer*/ IntPtr arg);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate bool CheckEventPredicate(IntPtr display, ref XEvent @event, IntPtr arg);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XCheckMaskEvent")]
		[Security.SuppressUnmanagedCodeSecurity]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool CheckMaskEvent(IntPtr display, EventMask event_mask, ref XEvent event_return);





		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XGrabPointer")]
		[Security.SuppressUnmanagedCodeSecurity]
		extern public static ErrorCodes GrabPointer(IntPtr display, IntPtr grab_window,
			bool owner_events, int event_mask, GrabMode pointer_mode, GrabMode keyboard_mode,
			IntPtr confine_to, IntPtr cursor, int time);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XUngrabPointer")]
		[Security.SuppressUnmanagedCodeSecurity]
		extern public static ErrorCodes UngrabPointer(IntPtr display, int time);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XGrabKeyboard")]
		[Security.SuppressUnmanagedCodeSecurity]
		extern public static ErrorCodes GrabKeyboard(IntPtr display, IntPtr grab_window,
			bool owner_events, GrabMode pointer_mode, GrabMode keyboard_mode, int time);

		[Security.SuppressUnmanagedCodeSecurity]
		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XUngrabKeyboard")]
		extern public static void UngrabKeyboard(IntPtr display, int time);

		/// <summary>
		/// The XGetKeyboardMapping() function returns the symbols for the specified number of KeyCodes starting with first_keycode.
		/// </summary>
		/// <param name="display">Specifies the connection to the X server.</param>
		/// <param name="first_keycode">Specifies the first KeyCode that is to be returned.</param>
		/// <param name="keycode_count">Specifies the number of KeyCodes that are to be returned</param>
		/// <param name="keysyms_per_keycode_return">Returns the number of KeySyms per KeyCode.</param>
		/// <returns></returns>
		/// <remarks>
		/// <para>The value specified in first_keycode must be greater than or equal to min_keycode as returned by XDisplayKeycodes(), or a BadValue error results. In addition, the following expression must be less than or equal to max_keycode as returned by XDisplayKeycodes(): </para>
		/// <para>first_keycode + keycode_count - 1 </para>
		/// <para>If this is not the case, a BadValue error results. The number of elements in the KeySyms list is: </para>
		/// <para>keycode_count * keysyms_per_keycode_return </para>
		/// <para>KeySym number N, counting from zero, for KeyCode K has the following index in the list, counting from zero: </para>
		/// <para> (K - first_code) * keysyms_per_code_return + N </para>
		/// <para>The X server arbitrarily chooses the keysyms_per_keycode_return value to be large enough to report all requested symbols. A special KeySym value of NoSymbol is used to fill in unused elements for individual KeyCodes. To free the storage returned by XGetKeyboardMapping(), use XFree(). </para>
		/// <para>XGetKeyboardMapping() can generate a BadValue error.</para>
		/// <para>Diagnostics:</para>
		/// <para>BadValue:    Some numeric value falls outside the range of values accepted by the request. Unless a specific range is specified for an argument, the full range defined by the argument's type is accepted. Any argument defined as a set of alternatives can generate this error.</para>
		/// </remarks>
		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XGetKeyboardMapping")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern IntPtr GetKeyboardMapping(IntPtr display, Byte first_keycode, int keycode_count,
			ref int keysyms_per_keycode_return);

		/// <summary>
		/// The XDisplayKeycodes() function returns the min-keycodes and max-keycodes supported by the specified display.
		/// </summary>
		/// <param name="display">Specifies the connection to the X server.</param>
		/// <param name="min_keycodes_return">Returns the minimum number of KeyCodes</param>
		/// <param name="max_keycodes_return">Returns the maximum number of KeyCodes.</param>
		/// <remarks> The minimum number of KeyCodes returned is never less than 8, and the maximum number of KeyCodes returned is never greater than 255. Not all KeyCodes in this range are required to have corresponding keys.</remarks>
		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XDisplayKeycodes")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern void DisplayKeycodes(IntPtr display, ref int min_keycodes_return, ref int max_keycodes_return);





		[StructLayout(LayoutKind.Sequential)]
		public struct XF86VidModeModeLine {
			public short hdisplay;   /* Number of display pixels horizontally */
			public short hsyncstart; /* Horizontal sync start */
			public short hsyncend;   /* Horizontal sync end */
			public short htotal;     /* Total horizontal pixels */
			public short vdisplay;   /* Number of display pixels vertically */
			public short vsyncstart; /* Vertical sync start */
			public short vsyncend;   /* Vertical sync start */
			public short vtotal;     /* Total vertical pixels */
			public int flags;      /* Mode type */
			public int privsize;   /* Size of private */
			public IntPtr @private;   /* Server privates */
		}

		/// <summary>
		/// Specifies an XF86 display mode.
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
		public struct XF86VidModeModeInfo {
			/// <summary>
			/// Pixel clock.
			/// </summary>
			public int dotclock;

			/// <summary>
			/// Number of display pixels horizontally
			/// </summary>
			public short hdisplay;

			/// <summary>
			/// Horizontal sync start
			/// </summary>
			public short hsyncstart;

			/// <summary>
			/// Horizontal sync end
			/// </summary>
			public short hsyncend;

			/// <summary>
			/// Total horizontal pixel
			/// </summary>
			public short htotal;

			/// <summary>
			/// 
			/// </summary>
			public short hskew;

			/// <summary>
			/// Number of display pixels vertically
			/// </summary>
			public short vdisplay;

			/// <summary>
			/// Vertical sync start
			/// </summary>
			public short vsyncstart;

			/// <summary>
			/// Vertical sync end
			/// </summary>
			public short vsyncend;

			/// <summary>
			/// Total vertical pixels
			/// </summary>
			public short vtotal;

			/// <summary>
			/// 
			/// </summary>
			public short vskew;

			/// <summary>
			/// Mode type
			/// </summary>
			public int flags;

			public int privsize;   /* Size of private */
			public IntPtr @private;   /* Server privates */
		}

		//Monitor information:
		[StructLayout(LayoutKind.Sequential)]
		public struct XF86VidModeMonitor {
			[MarshalAs(UnmanagedType.LPStr)]
			string vendor;     /* Name of manufacturer */
			[MarshalAs(UnmanagedType.LPStr)]
			string model;      /* Model name */
			float EMPTY;      /* unused, for backward compatibility */
			byte nhsync;     /* Number of horiz sync ranges */
							 /*XF86VidModeSyncRange* */
			IntPtr hsync;/* Horizontal sync ranges */
			byte nvsync;     /* Number of vert sync ranges */
							 /*XF86VidModeSyncRange* */
			IntPtr vsync;/* Vertical sync ranges */
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct XF86VidModeSyncRange {
			float hi;         /* Top of range */
			float lo;         /* Bottom of range */
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct XF86VidModeNotifyEvent {
			int type;                      /* of event */
			ulong serial;          /* # of last request processed by server */
			bool send_event;               /* true if this came from a SendEvent req */
			IntPtr display;              /* Display the event was read from */
			IntPtr root;                   /* root window of event screen */
			int state;                     /* What happened */
			int kind;                      /* What happened */
			bool forced;                   /* extents of new region */
										   /* Time */
			IntPtr time;                     /* event timestamp */
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct XF86VidModeGamma {
			float red;                     /* Red Gamma value */
			float green;                   /* Green Gamma value */
			float blue;                    /* Blue Gamma value */
		}




		[SuppressUnmanagedCodeSecurity]
		[DllImport(DllNameVid)]
		[Security.SuppressUnmanagedCodeSecurity]
		extern public static bool XF86VidModeQueryExtension(
			IntPtr display,
			out int event_base_return,
			out int error_base_return);
		/*
		[SuppressUnmanagedCodeSecurity][DllImport(DllNameVid)]
		extern public static bool XF86VidModeSwitchMode(
			Display display,
			int screen,
			int zoom);
		*/

		[SuppressUnmanagedCodeSecurity]
		[DllImport(DllNameVid)]
		[Security.SuppressUnmanagedCodeSecurity]
		extern public static bool XF86VidModeSwitchToMode(
			IntPtr display,
			int screen,
			IntPtr
			/*XF86VidModeModeInfo* */ modeline);


		[SuppressUnmanagedCodeSecurity]
		[DllImport(DllNameVid)]
		[Security.SuppressUnmanagedCodeSecurity]
		extern public static bool XF86VidModeQueryVersion(
			IntPtr display,
			out int major_version_return,
			out int minor_version_return);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(DllNameVid)]
		[Security.SuppressUnmanagedCodeSecurity]
		extern public static bool XF86VidModeGetModeLine(
			IntPtr display,
			int screen,
			out int dotclock_return,
			out XF86VidModeModeLine modeline);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(DllNameVid)]
		[Security.SuppressUnmanagedCodeSecurity]
		extern public static bool XF86VidModeGetAllModeLines(
			IntPtr display,
			int screen,
			out int modecount_return,
			/*XF86VidModeModeInfo***  <-- yes, that's three *'s. */
			out IntPtr modesinfo);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(DllNameVid)]
		[Security.SuppressUnmanagedCodeSecurity]
		extern public static bool XF86VidModeGetViewPort(
			IntPtr display,
			int screen,
			out int x_return,
			out int y_return);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(DllNameVid)]
		[Security.SuppressUnmanagedCodeSecurity]
		extern public static bool XF86VidModeSetViewPort(
			IntPtr display,
			int screen,
			int x,
			int y);

		/*
Bool XF86VidModeSetClientVersion(
	Display *display);

Bool XF86VidModeDeleteModeLine(
	Display *display,
	int screen,
	XF86VidModeModeInfo *modeline);

Bool XF86VidModeModModeLine(
	Display *display,
	int screen,
	XF86VidModeModeLine *modeline);

Status XF86VidModeValidateModeLine(
	Display *display,
	int screen,
	XF86VidModeModeLine *modeline);

Bool XF86VidModeLockModeSwitch(
	Display *display,
	int screen,
	int lock);

Bool XF86VidModeGetMonitor(
	Display *display,
	int screen,
	XF86VidModeMonitor *monitor);

XF86VidModeGetDotClocks(
	Display *display,
	int screen,
	int *type return,
	int *number of clocks return,
	int *max dot clock return,
	int **clocks return);

XF86VidModeGetGamma(
	Display *display,
	int screen,
	XF86VidModeGamma *Gamma);

XF86VidModeSetGamma(
	Display *display,
	int screen,
	XF86VidModeGamma *Gamma);

XF86VidModeGetGammaRamp(
	Display *display,
	int screen,
	int size,
	unsigned short *red array,
	unsigned short *green array,
	unsigned short *blue array);

XF86VidModeSetGammaRamp(
	Display *display,
	int screen,
	int size,
	unsigned short *red array,
	unsigned short *green array,
	unsigned short *blue array);

XF86VidModeGetGammaRampSize(
	Display *display,
	int screen,
	int *size);
		 * */



		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XLookupKeysym")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern IntPtr LookupKeysym(ref XKeyEvent key_event, int index);



		/// <summary>
		/// The XCreateWindow function creates an unmapped subwindow for a specified parent window, returns the window ID of the created window, and causes the X server to generate a CreateNotify event. The created window is placed on top in the stacking order with respect to siblings.
		/// </summary>
		/// <param name="display">Specifies the connection to the X server.</param>
		/// <param name="parent">Specifies the parent window.</param>
		/// <param name="x">Specify the x coordinates, which are the top-left outside corner of the window's borders and are relative to the inside of the parent window's borders.</param>
		/// <param name="y">Specify the y coordinates, which are the top-left outside corner of the window's borders and are relative to the inside of the parent window's borders.</param>
		/// <param name="width">Specify the width, which is the created window's inside dimensions and do not include the created window's borders.</param>
		/// <param name="height">Specify the height, which is the created window's inside dimensions and do not include the created window's borders.</param>
		/// <param name="border_width">Specifies the width of the created window's border in pixels.</param>
		/// <param name="depth">Specifies the window's depth. A depth of CopyFromParent means the depth is taken from the parent.</param>
		/// <param name="class">Specifies the created window's class. You can pass InputOutput, InputOnly, or CopyFromParent. A class of CopyFromParent means the class is taken from the parent.</param>
		/// <param name="visual">Specifies the visual type. A visual of CopyFromParent means the visual type is taken from the parent.</param>
		/// <param name="valuemask">Specifies which window attributes are defined in the attributes argument. This mask is the bitwise inclusive OR of the valid attribute mask bits. If valuemask is zero, the attributes are ignored and are not referenced.</param>
		/// <param name="attributes">Specifies the structure from which the values (as specified by the value mask) are to be taken. The value mask should have the appropriate bits set to indicate which attributes have been set in the structure.</param>
		/// <returns>The window ID of the created window.</returns>
		/// <remarks>
		/// The coordinate system has the X axis horizontal and the Y axis vertical with the origin [0, 0] at the upper-left corner. Coordinates are integral, in terms of pixels, and coincide with pixel centers. Each window and pixmap has its own coordinate system. For a window, the origin is inside the border at the inside, upper-left corner. 
		/// <para>The border_width for an InputOnly window must be zero, or a BadMatch error results. For class InputOutput, the visual type and depth must be a combination supported for the screen, or a BadMatch error results. The depth need not be the same as the parent, but the parent must not be a window of class InputOnly, or a BadMatch error results. For an InputOnly window, the depth must be zero, and the visual must be one supported by the screen. If either condition is not met, a BadMatch error results. The parent window, however, may have any depth and class. If you specify any invalid window attribute for a window, a BadMatch error results. </para>
		/// <para>The created window is not yet displayed (mapped) on the user's display. To display the window, call XMapWindow(). The new window initially uses the same cursor as its parent. A new cursor can be defined for the new window by calling XDefineCursor(). The window will not be visible on the screen unless it and all of its ancestors are mapped and it is not obscured by any of its ancestors. </para>
		/// <para>XCreateWindow can generate BadAlloc BadColor, BadCursor, BadMatch, BadPixmap, BadValue, and BadWindow errors. </para>
		/// <para>The XCreateSimpleWindow function creates an unmapped InputOutput subwindow for a specified parent window, returns the window ID of the created window, and causes the X server to generate a CreateNotify event. The created window is placed on top in the stacking order with respect to siblings. Any part of the window that extends outside its parent window is clipped. The border_width for an InputOnly window must be zero, or a BadMatch error results. XCreateSimpleWindow inherits its depth, class, and visual from its parent. All other window attributes, except background and border, have their default values. </para>
		/// <para>XCreateSimpleWindow can generate BadAlloc, BadMatch, BadValue, and BadWindow errors.</para>
		/// </remarks>
		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XCreateWindow")]//, CLSCompliant(false)]
		[CLSCompliant(false)]
		public extern static IntPtr XCreateWindow(IntPtr display, IntPtr parent,
			int x, int y, int width, int height, int border_width, int depth,
			int @class, IntPtr visual, UIntPtr valuemask, ref XSetWindowAttributes attributes);





		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library)]
		[CLSCompliant(false)]
		public static extern void XChangeWindowAttributes(IntPtr display, IntPtr w, UIntPtr valuemask, ref XSetWindowAttributes attributes);

		public static void XChangeWindowAttributes(IntPtr display, IntPtr w, SetWindowValuemask valuemask, ref XSetWindowAttributes attributes) {
			XChangeWindowAttributes(display, w, (UIntPtr) valuemask, ref attributes);
		}





		/*
        /// <summary>
        /// The XQueryKeymap() function returns a bit vector for the logical state of the keyboard, where each bit set to 1 indicates that the corresponding key is currently pressed down. The vector is represented as 32 bytes. Byte N (from 0) contains the bits for keys 8N to 8N + 7 with the least-significant bit in the byte representing key 8N.
        /// </summary>
        /// <param name="display">Specifies the connection to the X server.</param>
        /// <param name="keys">Returns an array of bytes that identifies which keys are pressed down. Each bit represents one key of the keyboard.</param>
        /// <remarks>Note that the logical state of a device (as seen by client applications) may lag the physical state if device event processing is frozen.</remarks>
        [SuppressUnmanagedCodeSecurity][DllImport(Library, EntryPoint = "XQueryKeymap")]
        extern public static void XQueryKeymap(IntPtr display, [MarshalAs(UnmanagedType.LPArray, SizeConst = 32), In, Out] Keymap keys);
        */

		/// <summary>
		/// The XQueryKeymap() function returns a bit vector for the logical state of the keyboard, where each bit set to 1 indicates that the corresponding key is currently pressed down. The vector is represented as 32 bytes. Byte N (from 0) contains the bits for keys 8N to 8N + 7 with the least-significant bit in the byte representing key 8N.
		/// </summary>
		/// <param name="display">Specifies the connection to the X server.</param>
		/// <param name="keys">Returns an array of bytes that identifies which keys are pressed down. Each bit represents one key of the keyboard.</param>
		/// <remarks>Note that the logical state of a device (as seen by client applications) may lag the physical state if device event processing is frozen.</remarks>
		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XQueryKeymap")]
		[Security.SuppressUnmanagedCodeSecurity]
		extern public static void XQueryKeymap(IntPtr display, byte[] keys);





		/// <summary>
		/// The XMaskEvent() function searches the event queue for the events associated with the specified mask. When it finds a match, XMaskEvent() removes that event and copies it into the specified XEvent structure. The other events stored in the queue are not discarded. If the event you requested is not in the queue, XMaskEvent() flushes the output buffer and blocks until one is received.
		/// </summary>
		/// <param name="display">Specifies the connection to the X server.</param>
		/// <param name="event_mask">Specifies the event mask.</param>
		/// <param name="e">Returns the matched event's associated structure.</param>
		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XMaskEvent")]
		[Security.SuppressUnmanagedCodeSecurity]
		extern public static void XMaskEvent(IntPtr display, EventMask event_mask, ref XEvent e);





		/// <summary>
		/// The XPutBackEvent() function pushes an event back onto the head of the display's event queue by copying the event into the queue. This can be useful if you read an event and then decide that you would rather deal with it later. There is no limit to the number of times in succession that you can call XPutBackEvent().
		/// </summary>
		/// <param name="display">Specifies the connection to the X server.</param>
		/// <param name="event">Specifies the event.</param>
		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XPutBackEvent")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern void XPutBackEvent(IntPtr display, ref XEvent @event);





		public const string XrandrLibrary = "libXrandr.so.2";

		[SuppressUnmanagedCodeSecurity]
		[DllImport(XrandrLibrary)]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern Boolean XRRQueryExtension(IntPtr dpy, ref int event_basep, ref int error_basep);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(XrandrLibrary)]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern Int32 XRRQueryVersion(IntPtr dpy, ref int major_versionp, ref int minor_versionp);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(XrandrLibrary)]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern IntPtr XRRGetScreenInfo(IntPtr dpy, IntPtr draw);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(XrandrLibrary)]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern void XRRFreeScreenConfigInfo(IntPtr config);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(XrandrLibrary)]
		[CLSCompliant(false)]
		public static extern Int32 XRRSetScreenConfig(IntPtr dpy, IntPtr config,
			IntPtr draw, int size_index, UInt16 rotation, IntPtr timestamp);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(XrandrLibrary)]
		[CLSCompliant(false)]
		public static extern Int32 XRRSetScreenConfigAndRate(IntPtr dpy, IntPtr config,
			IntPtr draw, int size_index, UInt16 rotation, short rate, IntPtr timestamp);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(XrandrLibrary)]
		[CLSCompliant(false)]
		public static extern UInt16 XRRConfigRotations(IntPtr config, ref UInt16 current_rotation);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(XrandrLibrary)]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern IntPtr XRRConfigTimes(IntPtr config, ref IntPtr config_timestamp);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(XrandrLibrary)]
		[Security.SuppressUnmanagedCodeSecurity]
		[return: MarshalAs(UnmanagedType.LPStruct)]
		public static extern XRRScreenSize XRRConfigSizes(IntPtr config, int[] nsizes);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(XrandrLibrary)]
		[CLSCompliant(false)]
		unsafe public static extern short* XRRConfigRates(IntPtr config, int size_index, int[] nrates);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(XrandrLibrary)]
		[CLSCompliant(false)]
		public static extern UInt16 XRRConfigCurrentConfiguration(IntPtr config, out UInt16 rotation);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(XrandrLibrary)]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern short XRRConfigCurrentRate(IntPtr config);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(XrandrLibrary)]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern int XRRRootToScreen(IntPtr dpy, IntPtr root);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(XrandrLibrary)]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern IntPtr XRRScreenConfig(IntPtr dpy, int screen);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(XrandrLibrary)]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern IntPtr XRRConfig(ref Screen screen);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(XrandrLibrary)]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern void XRRSelectInput(IntPtr dpy, IntPtr window, int mask);

		/*
		 * intended to take RRScreenChangeNotify,  or
		 * ConfigureNotify (on the root window)
		 * returns 1 if it is an event type it understands, 0 if not
		 */
		[SuppressUnmanagedCodeSecurity]
		[DllImport(XrandrLibrary)]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern int XRRUpdateConfiguration(ref XEvent @event);

		/*
		 * the following are always safe to call, even if RandR is
		 * not implemented on a screen
		 */
		[SuppressUnmanagedCodeSecurity]
		[DllImport(XrandrLibrary)]
		[CLSCompliant(false)]
		public static extern UInt16 XRRRotations(IntPtr dpy, int screen, ref UInt16 current_rotation);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(XrandrLibrary)]
		[CLSCompliant(false)]
		public unsafe static extern IntPtr XRRSizes(IntPtr dpy, int screen, int* nsizes);

		public static XRRScreenSize[] XRRSizes(IntPtr dpy, int screen) {
			XRRScreenSize[] sizes;
			//IntPtr ptr;
			int count;
			unsafe
			{
				//ptr = XRRSizes(dpy, screen, &nsizes);

				byte* data = (byte*) XRRSizes(dpy, screen, &count);//(byte*)ptr;
				if (count == 0)
					return null;
				sizes = new XRRScreenSize[count];
				for (int i = 0; i < count; i++) {
					sizes[i] = new XRRScreenSize();
					sizes[i] = (XRRScreenSize) Marshal.PtrToStructure((IntPtr) data, typeof(XRRScreenSize));
					data += XRRScreenSize.Size;
				}
				//XFree(ptr);   // Looks like we must not free this.
				return sizes;
			}
		}

		[SuppressUnmanagedCodeSecurity]
		[DllImport(XrandrLibrary)]
		[CLSCompliant(false)]
		public unsafe static extern short* XRRRates(IntPtr dpy, int screen, int size_index, int* nrates);

		public static short[] XRRRates(IntPtr dpy, int screen, int size_index) {
			short[] rates;
			int count;
			unsafe
			{
				short* data = (short*) XRRRates(dpy, screen, size_index, &count);
				if (count == 0)
					return null;
				rates = new short[count];
				for (int i = 0; i < count; i++)
					rates[i] = *(data + i);
			}
			return rates;
		}

		[SuppressUnmanagedCodeSecurity]
		[DllImport(XrandrLibrary)]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern IntPtr XRRTimes(IntPtr dpy, int screen, out IntPtr config_timestamp);



		public const string Xinerama = "libXinerama";

		[DllImport(Xinerama)]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern bool XineramaQueryExtension(IntPtr dpy, out int event_basep, out int error_basep);

		[DllImport(Xinerama)]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern int XineramaQueryVersion(IntPtr dpy, out int major_versionp, out int minor_versionp);

		[DllImport(Xinerama)]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern bool XineramaIsActive(IntPtr dpy);

		[DllImport(Xinerama)]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern IntPtr XineramaQueryScreens(IntPtr dpy, out int number);

		public static XineramaScreenInfo[] XineramaQueryScreens(IntPtr dpy) {
			int number;
			IntPtr screen_ptr = XineramaQueryScreens(dpy, out number);
			XineramaScreenInfo[] screens = new XineramaScreenInfo[number];
			unsafe
			{
				XineramaScreenInfo* ptr = (XineramaScreenInfo*) screen_ptr;
				for (int i = 0; i < number; i++) {
					screens[i] = *ptr;
					ptr++;
				}
			}
			return screens;
		}





		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library)]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern int XScreenCount(IntPtr display);





		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library)]
		[CLSCompliant(false)]
		public unsafe static extern int* XListDepths(IntPtr display, int screen_number, int* count_return);

		public static int[] XListDepths(IntPtr display, int screen_number) {
			unsafe
			{
				int count;
				int* data = XListDepths(display, screen_number, &count);
				if (count == 0)
					return null;
				int[] depths = new int[count];
				for (int i = 0; i < count; i++)
					depths[i] = *(data + i);

				return depths;
			}
		}







		public static IntPtr XCreateBitmapFromData(IntPtr display, IntPtr d, byte[,] data) {
			unsafe
			{
				fixed (byte* pdata = data) {
					return XCreateBitmapFromData(display, d, pdata, data.GetLength(0), data.GetLength(1));
				}
			}
		}

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library)]
		[CLSCompliant(false)]
		unsafe public static extern IntPtr XCreateBitmapFromData(IntPtr display, IntPtr d, byte* data, int width, int height);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XAllocColor")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern Int32 XAllocNamedColor(IntPtr display, IntPtr colormap, string color_name, out XColor screen_def_return, out XColor exact_def_return);


		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XOpenDisplay")]
		[Security.SuppressUnmanagedCodeSecurity]
		extern public static IntPtr sys_XOpenDisplay(IntPtr display);
		public static IntPtr XOpenDisplay(IntPtr display) {
			lock (SyncRoot) {
				return sys_XOpenDisplay(display);
			}
		}

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XCloseDisplay")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static int XCloseDisplay(IntPtr display);
		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XSynchronize")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static IntPtr XSynchronize(IntPtr display, bool onoff);

		//[SuppressUnmanagedCodeSecurity][DllImport(Library, EntryPoint = "XCreateWindow"), CLSCompliant(false)]
		//public extern static IntPtr XCreateWindow(IntPtr display, IntPtr parent, int x, int y, int width, int height, int border_width, int depth, int xclass, IntPtr visual, UIntPtr valuemask, ref XSetWindowAttributes attributes);
		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XCreateWindow")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static IntPtr XCreateWindow(IntPtr display, IntPtr parent, int x, int y, int width, int height, int border_width, int depth, int xclass, IntPtr visual, IntPtr valuemask, ref XSetWindowAttributes attributes);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XCreateSimpleWindow")]//, CLSCompliant(false)]
		[CLSCompliant(false)]
		public extern static IntPtr XCreateSimpleWindow(IntPtr display, IntPtr parent, int x, int y, int width, int height, int border_width, UIntPtr border, UIntPtr background);
		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XCreateSimpleWindow")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static IntPtr XCreateSimpleWindow(IntPtr display, IntPtr parent, int x, int y, int width, int height, int border_width, IntPtr border, IntPtr background);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XMapWindow")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static int XMapWindow(IntPtr display, IntPtr window);
		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XUnmapWindow")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static int XUnmapWindow(IntPtr display, IntPtr window);
		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XMapSubwindows")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static int XMapSubindows(IntPtr display, IntPtr window);
		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XUnmapSubwindows")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static int XUnmapSubwindows(IntPtr display, IntPtr window);
		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XRootWindow")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static IntPtr XRootWindow(IntPtr display, int screen_number);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XNextEvent")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static IntPtr XNextEvent(IntPtr display, ref XEvent xevent);
		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library)]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static Boolean XWindowEvent(IntPtr display, IntPtr w, EventMask event_mask, ref XEvent event_return);
		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library)]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static Boolean XCheckWindowEvent(IntPtr display, IntPtr w, EventMask event_mask, ref XEvent event_return);
		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library)]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static Boolean XCheckTypedWindowEvent(IntPtr display, IntPtr w, XEventName event_type, ref XEvent event_return);
		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library)]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static Boolean XCheckTypedEvent(IntPtr display, XEventName event_type, out XEvent event_return);


		public delegate Boolean EventPredicate(IntPtr display, ref XEvent e, IntPtr arg);
		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library)]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static Boolean XIfEvent(IntPtr display, ref XEvent e, IntPtr predicate, IntPtr arg);
		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library)]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static Boolean XCheckIfEvent(IntPtr display, ref XEvent e, IntPtr predicate, IntPtr arg);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library)]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static int XConnectionNumber(IntPtr diplay);
		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library)]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static int XPending(IntPtr diplay);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XSelectInput")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static int XSelectInput(IntPtr display, IntPtr window, IntPtr mask);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XDestroyWindow")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static int XDestroyWindow(IntPtr display, IntPtr window);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XReparentWindow")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static int XReparentWindow(IntPtr display, IntPtr window, IntPtr parent, int x, int y);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XMoveResizeWindow")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static int XMoveResizeWindow(IntPtr display, IntPtr window, int x, int y, int width, int height);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XMoveWindow")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static int XMoveWindow(IntPtr display, IntPtr w, int x, int y);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XGetWindowAttributes")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static int XGetWindowAttributes(IntPtr display, IntPtr window, ref XWindowAttributes attributes);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XFlush")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static int XFlush(IntPtr display);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XSetWMName")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static int XSetWMName(IntPtr display, IntPtr window, ref XTextProperty text_prop);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XStoreName")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static int XStoreName(IntPtr display, IntPtr window, string window_name);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XFetchName")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static int XFetchName(IntPtr display, IntPtr window, ref IntPtr window_name);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XSendEvent")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static int XSendEvent(IntPtr display, IntPtr window, bool propagate, IntPtr event_mask, ref XEvent send_event);

		public static int XSendEvent(IntPtr display, IntPtr window, bool propagate, EventMask event_mask, ref XEvent send_event) {
			return XSendEvent(display, window, propagate, new IntPtr((int) event_mask), ref send_event);
		}

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XQueryTree")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static int XQueryTree(IntPtr display, IntPtr window, out IntPtr root_return, out IntPtr parent_return, out IntPtr children_return, out int nchildren_return);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XFree")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static int XFree(IntPtr data);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XRaiseWindow")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static int XRaiseWindow(IntPtr display, IntPtr window);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XLowerWindow")]//, CLSCompliant(false)]
		[CLSCompliant(false)]
		public extern static uint XLowerWindow(IntPtr display, IntPtr window);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XConfigureWindow")]//, CLSCompliant(false)]
		[CLSCompliant(false)]
		public extern static uint XConfigureWindow(IntPtr display, IntPtr window, ChangeWindowAttributes value_mask, ref XWindowChanges values);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XInternAtom")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static IntPtr XInternAtom(IntPtr display, string atom_name, bool only_if_exists);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XInternAtoms")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static int XInternAtoms(IntPtr display, string[] atom_names, int atom_count, bool only_if_exists, IntPtr[] atoms);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XSetWMProtocols")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static int XSetWMProtocols(IntPtr display, IntPtr window, IntPtr[] protocols, int count);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XGrabPointer")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static int XGrabPointer(IntPtr display, IntPtr window, bool owner_events, EventMask event_mask, GrabMode pointer_mode, GrabMode keyboard_mode, IntPtr confine_to, IntPtr cursor, IntPtr timestamp);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XUngrabPointer")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static int XUngrabPointer(IntPtr display, IntPtr timestamp);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XGrabButton")]
		[CLSCompliant(false)]
		public extern static int XGrabButton(IntPtr display,
			int button, uint modifiers, IntPtr grab_window,
			Boolean owner_events, EventMask event_mask,
			GrabMode pointer_mode, GrabMode keyboard_mode,
			IntPtr confine_to, IntPtr cursor);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XUngrabButton")]
		[CLSCompliant(false)]
		public extern static int XUngrabButton(IntPtr display, uint button, uint
			  modifiers, IntPtr grab_window);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XQueryPointer")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static bool XQueryPointer(IntPtr display, IntPtr window, out IntPtr root, out IntPtr child, out int root_x, out int root_y, out int win_x, out int win_y, out int keys_buttons);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XTranslateCoordinates")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static bool XTranslateCoordinates(IntPtr display, IntPtr src_w, IntPtr dest_w, int src_x, int src_y, out int intdest_x_return, out int dest_y_return, out IntPtr child_return);


		[CLSCompliant(false)]
		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library)]
		public extern static int XGrabKey(IntPtr display, int keycode, uint modifiers,
			IntPtr grab_window, bool owner_events, GrabMode pointer_mode, GrabMode keyboard_mode);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library)]
		[CLSCompliant(false)]
		public extern static int XUngrabKey(IntPtr display, int keycode, uint modifiers, IntPtr grab_window);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XGrabKeyboard")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static int XGrabKeyboard(IntPtr display, IntPtr window, bool owner_events,
			GrabMode pointer_mode, GrabMode keyboard_mode, IntPtr timestamp);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XUngrabKeyboard")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static int XUngrabKeyboard(IntPtr display, IntPtr timestamp);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library)]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static int XAllowEvents(IntPtr display, EventMode event_mode, IntPtr time);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XGetGeometry")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static bool XGetGeometry(IntPtr display, IntPtr window, out IntPtr root, out int x, out int y, out int width, out int height, out int border_width, out int depth);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XGetGeometry")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static bool XGetGeometry(IntPtr display, IntPtr window, IntPtr root, out int x, out int y, out int width, out int height, IntPtr border_width, IntPtr depth);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XGetGeometry")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static bool XGetGeometry(IntPtr display, IntPtr window, IntPtr root, out int x, out int y, IntPtr width, IntPtr height, IntPtr border_width, IntPtr depth);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XGetGeometry")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static bool XGetGeometry(IntPtr display, IntPtr window, IntPtr root, IntPtr x, IntPtr y, out int width, out int height, IntPtr border_width, IntPtr depth);

		[CLSCompliant(false)]
		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XWarpPointer")]//, CLSCompliant(false)]
		public extern static uint XWarpPointer(IntPtr display, IntPtr src_w, IntPtr dest_w, int src_x, int src_y, uint src_width, uint src_height, int dest_x, int dest_y);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XClearWindow")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static int XClearWindow(IntPtr display, IntPtr window);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XClearArea")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static int XClearArea(IntPtr display, IntPtr window, int x, int y, int width, int height, bool exposures);

		// Colormaps
		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XDefaultScreenOfDisplay")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static IntPtr XDefaultScreenOfDisplay(IntPtr display);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XScreenNumberOfScreen")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static int XScreenNumberOfScreen(IntPtr display, IntPtr Screen);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XDefaultVisual")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static IntPtr XDefaultVisual(IntPtr display, int screen_number);

		[CLSCompliant(false)]
		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XDefaultDepth")]//, CLSCompliant(false)]
		public extern static uint XDefaultDepth(IntPtr display, int screen_number);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XDefaultScreen")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static int XDefaultScreen(IntPtr display);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XDefaultColormap")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static IntPtr XDefaultColormap(IntPtr display, int screen_number);

		[Security.SuppressUnmanagedCodeSecurity]
		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XLookupColor")]//, CLSCompliant(false)]
		public extern static int XLookupColor(IntPtr display, IntPtr Colormap, string Coloranem, ref XColor exact_def_color, ref XColor screen_def_color);

		[Security.SuppressUnmanagedCodeSecurity]
		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XAllocColor")]//, CLSCompliant(false)]
		public extern static int XAllocColor(IntPtr display, IntPtr Colormap, ref XColor colorcell_def);

		[Security.SuppressUnmanagedCodeSecurity]
		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XSetTransientForHint")]
		public extern static int XSetTransientForHint(IntPtr display, IntPtr window, IntPtr prop_window);

		[Security.SuppressUnmanagedCodeSecurity]
		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XChangeProperty")]
		public extern static int XChangeProperty(IntPtr display, IntPtr window, IntPtr property, IntPtr type, int format, PropertyMode mode, ref MotifWmHints data, int nelements);

		[CLSCompliant(false)]
		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XChangeProperty")]//, CLSCompliant(false)]
		public extern static int XChangeProperty(IntPtr display, IntPtr window, IntPtr property, IntPtr type, int format, PropertyMode mode, ref uint value, int nelements);
		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XChangeProperty")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static int XChangeProperty(IntPtr display, IntPtr window, IntPtr property, IntPtr type, int format, PropertyMode mode, ref int value, int nelements);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XChangeProperty")]//, CLSCompliant(false)]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static int XChangeProperty(IntPtr display, IntPtr window, IntPtr property, IntPtr type, int format, PropertyMode mode, ref IntPtr value, int nelements);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XChangeProperty")]//, CLSCompliant(false)]
		[CLSCompliant(false)]
		public extern static int XChangeProperty(IntPtr display, IntPtr window, IntPtr property, IntPtr type, int format, PropertyMode mode, uint[] data, int nelements);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XChangeProperty")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static int XChangeProperty(IntPtr display, IntPtr window, IntPtr property, IntPtr type, int format, PropertyMode mode, int[] data, int nelements);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XChangeProperty")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static int XChangeProperty(IntPtr display, IntPtr window, IntPtr property, IntPtr type, int format, PropertyMode mode, IntPtr[] data, int nelements);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XChangeProperty")]
		[CLSCompliant(false)]
		public extern static int XChangeProperty(IntPtr display, IntPtr window, IntPtr property, IntPtr type, int format, PropertyMode mode, IntPtr atoms, int nelements);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XChangeProperty", CharSet = CharSet.Ansi)]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static int XChangeProperty(IntPtr display, IntPtr window, IntPtr property, IntPtr type, int format, PropertyMode mode, string text, int text_length);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XDeleteProperty")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static int XDeleteProperty(IntPtr display, IntPtr window, IntPtr property);

		// Drawing
		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XCreateGC")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static IntPtr XCreateGC(IntPtr display, IntPtr window, IntPtr valuemask, XGCValues[] values);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XFreeGC")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static int XFreeGC(IntPtr display, IntPtr gc);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XSetFunction")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static int XSetFunction(IntPtr display, IntPtr gc, GXFunction function);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XSetLineAttributes")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static int XSetLineAttributes(IntPtr display, IntPtr gc, int line_width, GCLineStyle line_style, GCCapStyle cap_style, GCJoinStyle join_style);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XDrawLine")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static int XDrawLine(IntPtr display, IntPtr drawable, IntPtr gc, int x1, int y1, int x2, int y2);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XDrawRectangle")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static int XDrawRectangle(IntPtr display, IntPtr drawable, IntPtr gc, int x1, int y1, int width, int height);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XFillRectangle")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static int XFillRectangle(IntPtr display, IntPtr drawable, IntPtr gc, int x1, int y1, int width, int height);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XSetWindowBackground")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static int XSetWindowBackground(IntPtr display, IntPtr window, IntPtr background);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XCopyArea")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static int XCopyArea(IntPtr display, IntPtr src, IntPtr dest, IntPtr gc, int src_x, int src_y, int width, int height, int dest_x, int dest_y);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XGetWindowProperty")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static int XGetWindowProperty(IntPtr display, IntPtr window, IntPtr atom, IntPtr long_offset, IntPtr long_length, bool delete, IntPtr req_type, out IntPtr actual_type, out int actual_format, out IntPtr nitems, out IntPtr bytes_after, ref IntPtr prop);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XSetInputFocus")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static int XSetInputFocus(IntPtr display, IntPtr window, RevertTo revert_to, IntPtr time);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XIconifyWindow")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static int XIconifyWindow(IntPtr display, IntPtr window, int screen_number);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XDefineCursor")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static int XDefineCursor(IntPtr display, IntPtr window, IntPtr cursor);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XUndefineCursor")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static int XUndefineCursor(IntPtr display, IntPtr window);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XFreeCursor")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static int XFreeCursor(IntPtr display, IntPtr cursor);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XCreateFontCursor")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static IntPtr XCreateFontCursor(IntPtr display, CursorFontShape shape);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XCreatePixmapCursor")]//, CLSCompliant(false)]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static IntPtr XCreatePixmapCursor(IntPtr display, IntPtr source, IntPtr mask, ref XColor foreground_color, ref XColor background_color, int x_hot, int y_hot);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XCreatePixmapFromBitmapData")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static IntPtr XCreatePixmapFromBitmapData(IntPtr display, IntPtr drawable, byte[] data, int width, int height, IntPtr fg, IntPtr bg, int depth);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XCreatePixmap")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static IntPtr XCreatePixmap(IntPtr display, IntPtr d, int width, int height, int depth);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XFreePixmap")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static IntPtr XFreePixmap(IntPtr display, IntPtr pixmap);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XQueryBestCursor")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static int XQueryBestCursor(IntPtr display, IntPtr drawable, int width, int height, out int best_width, out int best_height);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XQueryExtension")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static int XQueryExtension(IntPtr display, string extension_name, out int major, out int first_event, out int first_error);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XWhitePixel")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static IntPtr XWhitePixel(IntPtr display, int screen_no);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XBlackPixel")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static IntPtr XBlackPixel(IntPtr display, int screen_no);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XGrabServer")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static void XGrabServer(IntPtr display);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XUngrabServer")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static void XUngrabServer(IntPtr display);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XGetWMNormalHints")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static int XGetWMNormalHints(IntPtr display, IntPtr window, ref XSizeHints hints, out IntPtr supplied_return);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XSetWMNormalHints")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static void XSetWMNormalHints(IntPtr display, IntPtr window, ref XSizeHints hints);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XSetZoomHints")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static void XSetZoomHints(IntPtr display, IntPtr window, ref XSizeHints hints);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library)]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern IntPtr XGetWMHints(IntPtr display, IntPtr w); // returns XWMHints*

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library)]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern void XSetWMHints(IntPtr display, IntPtr w, ref XWMHints wmhints);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library)]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern IntPtr XAllocWMHints();

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XGetIconSizes")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static int XGetIconSizes(IntPtr display, IntPtr window, out IntPtr size_list, out int count);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XSetErrorHandler")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static IntPtr XSetErrorHandler(XErrorHandler error_handler);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XGetErrorText")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static IntPtr XGetErrorText(IntPtr display, byte code, StringBuilder buffer, int length);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XInitThreads")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static int XInitThreads();

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XConvertSelection")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static int XConvertSelection(IntPtr display, IntPtr selection, IntPtr target, IntPtr property, IntPtr requestor, IntPtr time);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XGetSelectionOwner")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static IntPtr XGetSelectionOwner(IntPtr display, IntPtr selection);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XSetSelectionOwner")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static int XSetSelectionOwner(IntPtr display, IntPtr selection, IntPtr owner, IntPtr time);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XSetPlaneMask")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static int XSetPlaneMask(IntPtr display, IntPtr gc, IntPtr mask);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XSetForeground")]//, CLSCompliant(false)]
		[CLSCompliant(false)]
		public extern static int XSetForeground(IntPtr display, IntPtr gc, UIntPtr foreground);
		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XSetForeground")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static int XSetForeground(IntPtr display, IntPtr gc, IntPtr foreground);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XSetBackground")]//, CLSCompliant(false)]
		[CLSCompliant(false)]
		public extern static int XSetBackground(IntPtr display, IntPtr gc, UIntPtr background);
		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XSetBackground")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static int XSetBackground(IntPtr display, IntPtr gc, IntPtr background);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XBell")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static int XBell(IntPtr display, int percent);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XChangeActivePointerGrab")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static int XChangeActivePointerGrab(IntPtr display, EventMask event_mask, IntPtr cursor, IntPtr time);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XFilterEvent")]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static bool XFilterEvent(ref XEvent xevent, IntPtr window);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library)]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static bool XkbSetDetectableAutoRepeat(IntPtr display, bool detectable, out bool supported);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library)]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static void XPeekEvent(IntPtr display, ref XEvent xevent);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library, EntryPoint = "XGetVisualInfo")]
		public static extern IntPtr XGetVisualInfo(IntPtr display, IntPtr vinfo_mask, ref XVisualInfo template, out int nitems);

		public static IntPtr XGetVisualInfo(IntPtr display, XVisualInfoMask vinfo_mask, ref XVisualInfo template, out int nitems) {
			return XGetVisualInfo(display, (IntPtr) (int) vinfo_mask, ref template, out nitems);
		}

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library)]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern IntPtr XCreateColormap(IntPtr display, IntPtr window, IntPtr visual, int alloc);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library)]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern void XLockDisplay(IntPtr display);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library)]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern void XUnlockDisplay(IntPtr display);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library)]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern Int32 XGetTransientForHint(IntPtr display, IntPtr w, out IntPtr prop_window_return);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library)]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern void XSync(IntPtr display, bool discard);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library)]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern void XAutoRepeatOff(IntPtr display);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library)]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern void XAutoRepeatOn(IntPtr display);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library)]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern IntPtr XDefaultRootWindow(IntPtr display);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library)]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern int XBitmapBitOrder(IntPtr display);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library)]
		[CLSCompliant(false)]
		public static extern IntPtr XCreateImage(IntPtr display, IntPtr visual,
			uint depth, ImageFormat format, int offset, byte[] data, uint width, uint height,
			int bitmap_pad, int bytes_per_line);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library)]
		[CLSCompliant(false)]
		public static extern IntPtr XCreateImage(IntPtr display, IntPtr visual,
			uint depth, ImageFormat format, int offset, IntPtr data, uint width, uint height,
			int bitmap_pad, int bytes_per_line);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library)]
		[CLSCompliant(false)]
		public static extern void XPutImage(IntPtr display, IntPtr drawable,
			IntPtr gc, IntPtr image, int src_x, int src_y, int dest_x, int dest_y, uint width, uint height);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library)]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern int XLookupString(ref XKeyEvent event_struct, [Out] byte[] buffer_return,
			int bytes_buffer, [Out] IntPtr[] keysym_return, IntPtr status_in_out);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library)]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern Byte XKeysymToKeycode(IntPtr display, IntPtr keysym);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library)]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern IntPtr XKeycodeToKeysym(IntPtr display, Byte keycode, int index);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library)]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern int XRefreshKeyboardMapping(ref XMappingEvent event_map);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library)]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern int XGetEventData(IntPtr display, ref XGenericEventCookie cookie);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Library)]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern void XFreeEventData(IntPtr display, ref XGenericEventCookie cookie);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("libXi")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern int XISelectEvents(IntPtr dpy, IntPtr win, [In] XIEventMask[] masks, int num_masks);
		[SuppressUnmanagedCodeSecurity]
		[DllImport("libXi")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern int XISelectEvents(IntPtr dpy, IntPtr win, [In] ref XIEventMask masks, int num_masks);

		public static int XISelectEvents(IntPtr dpy, IntPtr win, XIEventMask[] masks) {
			return XISelectEvents(dpy, win, masks, masks.Length);
		}

		public static int XISelectEvents(IntPtr dpy, IntPtr win, XIEventMask mask) {
			return XISelectEvents(dpy, win, ref mask, 1);
		}

		[SuppressUnmanagedCodeSecurity]
		[DllImport("libXi")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern Int32 XIGrabDevice(IntPtr display, int deviceid, IntPtr grab_window, IntPtr time,
			IntPtr cursor, int grab_mode, int paired_device_mode, Boolean owner_events, XIEventMask[] mask);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("libXi")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern Int32 XIUngrabDevice(IntPtr display, int deviceid, IntPtr time);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("libXi")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern Boolean XIWarpPointer(IntPtr display,
			int deviceid, IntPtr src_w, IntPtr dest_w,
			double src_x, double src_y, int src_width, int src_height,
			double dest_x, double dest_y);

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct Pixel {
			public byte A, R, G, B;
			public Pixel(byte a, byte r, byte g, byte b) {
				A = a;
				R = r;
				G = g;
				B = b;
			}
			public static explicit operator Pixel(int argb) {
				return new Pixel(
					(byte) ((argb >> 24) & 0xFF),
					(byte) ((argb >> 16) & 0xFF),
					(byte) ((argb >> 8) & 0xFF),
					(byte) (argb & 0xFF));
			}
		}
		public static IntPtr CreatePixmapFromImage(IntPtr display, Bitmap image) {
			int width = image.Width;
			int height = image.Height;
			int size = width * height;

			BitmapData data = image.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

			IntPtr ximage = XCreateImage(display, IntPtr.Zero, 24, ImageFormat.ZPixmap,
				0, data.Scan0, (uint) width, (uint) height, 32, 0);
			IntPtr pixmap = XCreatePixmap(display, XDefaultRootWindow(display),
				width, height, 24);
			IntPtr gc = XCreateGC(display, pixmap, IntPtr.Zero, null);

			XPutImage(display, pixmap, gc, ximage, 0, 0, 0, 0, (uint) width, (uint) height);

			XFreeGC(display, gc);
			image.UnlockBits(data);

			return pixmap;
		}
	}

	public enum EventMode {
		AsyncPointer = 0,
		SyncPointer,
		ReplayPointer,
		AsyncKeyboard,
		SyncKeyboard,
		ReplayKeyboard,
		AsyncBoth,
		SyncBoth
	}



	[StructLayout(LayoutKind.Sequential)]
	public struct XAnyEvent {
		public XEventName type;
		public IntPtr serial;
		public bool send_event;
		public IntPtr display;
		public IntPtr window;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct XKeyEvent {
		public XEventName type;
		public IntPtr serial;
		public bool send_event;
		public IntPtr display;
		public IntPtr window;
		public IntPtr root;
		public IntPtr subwindow;
		public IntPtr time;
		public int x;
		public int y;
		public int x_root;
		public int y_root;
		public int state;
		public int keycode;
		public bool same_screen;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct XButtonEvent {
		public XEventName type;
		public IntPtr serial;
		public bool send_event;
		public IntPtr display;
		public IntPtr window;
		public IntPtr root;
		public IntPtr subwindow;
		public IntPtr time;
		public int x;
		public int y;
		public int x_root;
		public int y_root;
		public int state;
		public int button;
		public bool same_screen;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct XMotionEvent {
		public XEventName type;
		public IntPtr serial;
		public bool send_event;
		public IntPtr display;
		public IntPtr window;
		public IntPtr root;
		public IntPtr subwindow;
		public IntPtr time;
		public int x;
		public int y;
		public int x_root;
		public int y_root;
		public int state;
		public byte is_hint;
		public bool same_screen;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct XCrossingEvent {
		public XEventName type;
		public IntPtr serial;
		public bool send_event;
		public IntPtr display;
		public IntPtr window;
		public IntPtr root;
		public IntPtr subwindow;
		public IntPtr time;
		public int x;
		public int y;
		public int x_root;
		public int y_root;
		public NotifyMode mode;
		public NotifyDetail detail;
		public bool same_screen;
		public bool focus;
		public int state;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct XFocusChangeEvent {
		public XEventName type;
		public IntPtr serial;
		public bool send_event;
		public IntPtr display;
		public IntPtr window;
		public int mode;
		public NotifyDetail detail;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct XKeymapEvent {
		public XEventName type;
		public IntPtr serial;
		public bool send_event;
		public IntPtr display;
		public IntPtr window;
		public byte key_vector0;
		public byte key_vector1;
		public byte key_vector2;
		public byte key_vector3;
		public byte key_vector4;
		public byte key_vector5;
		public byte key_vector6;
		public byte key_vector7;
		public byte key_vector8;
		public byte key_vector9;
		public byte key_vector10;
		public byte key_vector11;
		public byte key_vector12;
		public byte key_vector13;
		public byte key_vector14;
		public byte key_vector15;
		public byte key_vector16;
		public byte key_vector17;
		public byte key_vector18;
		public byte key_vector19;
		public byte key_vector20;
		public byte key_vector21;
		public byte key_vector22;
		public byte key_vector23;
		public byte key_vector24;
		public byte key_vector25;
		public byte key_vector26;
		public byte key_vector27;
		public byte key_vector28;
		public byte key_vector29;
		public byte key_vector30;
		public byte key_vector31;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct XExposeEvent {
		public XEventName type;
		public IntPtr serial;
		public bool send_event;
		public IntPtr display;
		public IntPtr window;
		public int x;
		public int y;
		public int width;
		public int height;
		public int count;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct XGraphicsExposeEvent {
		public XEventName type;
		public IntPtr serial;
		public bool send_event;
		public IntPtr display;
		public IntPtr drawable;
		public int x;
		public int y;
		public int width;
		public int height;
		public int count;
		public int major_code;
		public int minor_code;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct XNoExposeEvent {
		public XEventName type;
		public IntPtr serial;
		public bool send_event;
		public IntPtr display;
		public IntPtr drawable;
		public int major_code;
		public int minor_code;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct XVisibilityEvent {
		public XEventName type;
		public IntPtr serial;
		public bool send_event;
		public IntPtr display;
		public IntPtr window;
		public int state;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct XCreateWindowEvent {
		public XEventName type;
		public IntPtr serial;
		public bool send_event;
		public IntPtr display;
		public IntPtr parent;
		public IntPtr window;
		public int x;
		public int y;
		public int width;
		public int height;
		public int border_width;
		public bool override_redirect;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct XDestroyWindowEvent {
		public XEventName type;
		public IntPtr serial;
		public bool send_event;
		public IntPtr display;
		public IntPtr xevent;
		public IntPtr window;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct XUnmapEvent {
		public XEventName type;
		public IntPtr serial;
		public bool send_event;
		public IntPtr display;
		public IntPtr xevent;
		public IntPtr window;
		public bool from_configure;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct XMapEvent {
		public XEventName type;
		public IntPtr serial;
		public bool send_event;
		public IntPtr display;
		public IntPtr xevent;
		public IntPtr window;
		public bool override_redirect;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct XMapRequestEvent {
		public XEventName type;
		public IntPtr serial;
		public bool send_event;
		public IntPtr display;
		public IntPtr parent;
		public IntPtr window;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct XReparentEvent {
		public XEventName type;
		public IntPtr serial;
		public bool send_event;
		public IntPtr display;
		public IntPtr xevent;
		public IntPtr window;
		public IntPtr parent;
		public int x;
		public int y;
		public bool override_redirect;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct XConfigureEvent {
		public XEventName type;
		public IntPtr serial;
		public bool send_event;
		public IntPtr display;
		public IntPtr xevent;
		public IntPtr window;
		public int x;
		public int y;
		public int width;
		public int height;
		public int border_width;
		public IntPtr above;
		public bool override_redirect;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct XGravityEvent {
		public XEventName type;
		public IntPtr serial;
		public bool send_event;
		public IntPtr display;
		public IntPtr xevent;
		public IntPtr window;
		public int x;
		public int y;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct XResizeRequestEvent {
		public XEventName type;
		public IntPtr serial;
		public bool send_event;
		public IntPtr display;
		public IntPtr window;
		public int width;
		public int height;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct XConfigureRequestEvent {
		public XEventName type;
		public IntPtr serial;
		public bool send_event;
		public IntPtr display;
		public IntPtr parent;
		public IntPtr window;
		public int x;
		public int y;
		public int width;
		public int height;
		public int border_width;
		public IntPtr above;
		public int detail;
		public IntPtr value_mask;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct XCirculateEvent {
		public XEventName type;
		public IntPtr serial;
		public bool send_event;
		public IntPtr display;
		public IntPtr xevent;
		public IntPtr window;
		public int place;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct XCirculateRequestEvent {
		public XEventName type;
		public IntPtr serial;
		public bool send_event;
		public IntPtr display;
		public IntPtr parent;
		public IntPtr window;
		public int place;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct XPropertyEvent {
		public XEventName type;
		public IntPtr serial;
		public bool send_event;
		public IntPtr display;
		public IntPtr window;
		public IntPtr atom;
		public IntPtr time;
		public int state;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct XSelectionClearEvent {
		public XEventName type;
		public IntPtr serial;
		public bool send_event;
		public IntPtr display;
		public IntPtr window;
		public IntPtr selection;
		public IntPtr time;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct XSelectionRequestEvent {
		public XEventName type;
		public IntPtr serial;
		public bool send_event;
		public IntPtr display;
		public IntPtr owner;
		public IntPtr requestor;
		public IntPtr selection;
		public IntPtr target;
		public IntPtr property;
		public IntPtr time;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct XSelectionEvent {
		public XEventName type;
		public IntPtr serial;
		public bool send_event;
		public IntPtr display;
		public IntPtr requestor;
		public IntPtr selection;
		public IntPtr target;
		public IntPtr property;
		public IntPtr time;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct XColormapEvent {
		public XEventName type;
		public IntPtr serial;
		public bool send_event;
		public IntPtr display;
		public IntPtr window;
		public IntPtr colormap;
		public bool c_new;
		public int state;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct XClientMessageEvent {
		public XEventName type;
		public IntPtr serial;
		public bool send_event;
		public IntPtr display;
		public IntPtr window;
		public IntPtr message_type;
		public int format;
		public IntPtr ptr1;
		public IntPtr ptr2;
		public IntPtr ptr3;
		public IntPtr ptr4;
		public IntPtr ptr5;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct XMappingEvent {
		public XEventName type;
		public IntPtr serial;
		public bool send_event;
		public IntPtr display;
		public IntPtr window;
		public int request;
		public int first_keycode;
		public int count;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct XErrorEvent {
		public XEventName type;
		public IntPtr display;
		public IntPtr resourceid;
		public IntPtr serial;
		public byte error_code;
		public XRequest request_code;
		public byte minor_code;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct XEventPad {
		public IntPtr pad0;
		public IntPtr pad1;
		public IntPtr pad2;
		public IntPtr pad3;
		public IntPtr pad4;
		public IntPtr pad5;
		public IntPtr pad6;
		public IntPtr pad7;
		public IntPtr pad8;
		public IntPtr pad9;
		public IntPtr pad10;
		public IntPtr pad11;
		public IntPtr pad12;
		public IntPtr pad13;
		public IntPtr pad14;
		public IntPtr pad15;
		public IntPtr pad16;
		public IntPtr pad17;
		public IntPtr pad18;
		public IntPtr pad19;
		public IntPtr pad20;
		public IntPtr pad21;
		public IntPtr pad22;
		public IntPtr pad23;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct XGenericEvent {
		public int type;         // of event. Always GenericEvent
		public IntPtr serial;       // # of last request processed
		public bool send_event;   // true if from SendEvent request
		public IntPtr display;     // Display the event was read from
		public int extension;    // major opcode of extension that caused the event
		public int evtype;       // actual event type.
	}

	public struct XGenericEventCookie {
		public int type;         // of event. Always GenericEvent
		public IntPtr serial;       // # of last request processed
		public bool send_event;   // true if from SendEvent request
		public IntPtr display;     // Display the event was read from
		public int extension;    // major opcode of extension that caused the event
		public int evtype;       // actual event type.
		[CLSCompliant(false)]
		public uint cookie;       // unique event cookie
		public IntPtr data;        // actual event data
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct XEvent {
		[FieldOffset(0)]
		public XEventName type;
		[FieldOffset(0)]
		public XAnyEvent AnyEvent;
		[FieldOffset(0)]
		public XKeyEvent KeyEvent;
		[FieldOffset(0)]
		public XButtonEvent ButtonEvent;
		[FieldOffset(0)]
		public XMotionEvent MotionEvent;
		[FieldOffset(0)]
		public XCrossingEvent CrossingEvent;
		[FieldOffset(0)]
		public XFocusChangeEvent FocusChangeEvent;
		[FieldOffset(0)]
		public XExposeEvent ExposeEvent;
		[FieldOffset(0)]
		public XGraphicsExposeEvent GraphicsExposeEvent;
		[FieldOffset(0)]
		public XNoExposeEvent NoExposeEvent;
		[FieldOffset(0)]
		public XVisibilityEvent VisibilityEvent;
		[FieldOffset(0)]
		public XCreateWindowEvent CreateWindowEvent;
		[FieldOffset(0)]
		public XDestroyWindowEvent DestroyWindowEvent;
		[FieldOffset(0)]
		public XUnmapEvent UnmapEvent;
		[FieldOffset(0)]
		public XMapEvent MapEvent;
		[FieldOffset(0)]
		public XMapRequestEvent MapRequestEvent;
		[FieldOffset(0)]
		public XReparentEvent ReparentEvent;
		[FieldOffset(0)]
		public XConfigureEvent ConfigureEvent;
		[FieldOffset(0)]
		public XGravityEvent GravityEvent;
		[FieldOffset(0)]
		public XResizeRequestEvent ResizeRequestEvent;
		[FieldOffset(0)]
		public XConfigureRequestEvent ConfigureRequestEvent;
		[FieldOffset(0)]
		public XCirculateEvent CirculateEvent;
		[FieldOffset(0)]
		public XCirculateRequestEvent CirculateRequestEvent;
		[FieldOffset(0)]
		public XPropertyEvent PropertyEvent;
		[FieldOffset(0)]
		public XSelectionClearEvent SelectionClearEvent;
		[FieldOffset(0)]
		public XSelectionRequestEvent SelectionRequestEvent;
		[FieldOffset(0)]
		public XSelectionEvent SelectionEvent;
		[FieldOffset(0)]
		public XColormapEvent ColormapEvent;
		[FieldOffset(0)]
		public XClientMessageEvent ClientMessageEvent;
		[FieldOffset(0)]
		public XMappingEvent MappingEvent;
		[FieldOffset(0)]
		public XErrorEvent ErrorEvent;
		[FieldOffset(0)]
		public XKeymapEvent KeymapEvent;
		[FieldOffset(0)]
		public XGenericEvent GenericEvent;
		[FieldOffset(0)]
		public XGenericEventCookie GenericEventCookie;

		//[MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst=24)]
		//[ FieldOffset(0) ] public int[] pad;
		[FieldOffset(0)]
		public XEventPad Pad;
		public override string ToString() {
			switch (type) {
				case XEventName.PropertyNotify:
					return ToString(PropertyEvent);
				case XEventName.ResizeRequest:
					return ToString(ResizeRequestEvent);
				case XEventName.ConfigureNotify:
					return ToString(ConfigureEvent);
				default:
					return type.ToString();
			}
		}

		public static string ToString(object ev) {
			Type type = ev.GetType();
			FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
			StringBuilder builder = new StringBuilder();
			object value;
			for (int i = 0; i < fields.Length; i++) {
				if (builder.Length != 0) {
					builder.Append(", ");
				}
				value = fields[i].GetValue(ev);
				builder.Append(fields[i].Name + "=" + (value == null ? "<null>" : value.ToString()));
			}
			return type.Name + " (" + builder + ")";
		}
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct XineramaScreenInfo {
		public int ScreenNumber;
		public short X;
		public short Y;
		public short Width;
		public short Height;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct XSetWindowAttributes {
		public IntPtr background_pixmap;
		public IntPtr background_pixel;
		public IntPtr border_pixmap;
		public IntPtr border_pixel;
		public Gravity bit_gravity;
		public Gravity win_gravity;
		public int backing_store;
		public IntPtr backing_planes;
		public IntPtr backing_pixel;
		public bool save_under;
		public IntPtr event_mask;
		public IntPtr do_not_propagate_mask;
		public bool override_redirect;
		public IntPtr colormap;
		public IntPtr cursor;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct XWindowAttributes {
		public int x;
		public int y;
		public int width;
		public int height;
		public int border_width;
		public int depth;
		public IntPtr visual;
		public IntPtr root;
		public int c_class;
		public Gravity bit_gravity;
		public Gravity win_gravity;
		public int backing_store;
		public IntPtr backing_planes;
		public IntPtr backing_pixel;
		public bool save_under;
		public IntPtr colormap;
		public bool map_installed;
		public MapState map_state;
		public IntPtr all_event_masks;
		public IntPtr your_event_mask;
		public IntPtr do_not_propagate_mask;
		public bool override_direct;
		public IntPtr screen;

		public override string ToString() {
			return XEvent.ToString(this);
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct XTextProperty {
		public string value;
		public IntPtr encoding;
		public int format;
		public IntPtr nitems;
	}

	public enum XWindowClass {
		InputOutput = 1,
		InputOnly = 2
	}

	public enum XEventName {
		KeyPress = 2,
		KeyRelease = 3,
		ButtonPress = 4,
		ButtonRelease = 5,
		MotionNotify = 6,
		EnterNotify = 7,
		LeaveNotify = 8,
		FocusIn = 9,
		FocusOut = 10,
		KeymapNotify = 11,
		Expose = 12,
		GraphicsExpose = 13,
		NoExpose = 14,
		VisibilityNotify = 15,
		CreateNotify = 16,
		DestroyNotify = 17,
		UnmapNotify = 18,
		MapNotify = 19,
		MapRequest = 20,
		ReparentNotify = 21,
		ConfigureNotify = 22,
		ConfigureRequest = 23,
		GravityNotify = 24,
		ResizeRequest = 25,
		CirculateNotify = 26,
		CirculateRequest = 27,
		PropertyNotify = 28,
		SelectionClear = 29,
		SelectionRequest = 30,
		SelectionNotify = 31,
		ColormapNotify = 32,
		ClientMessage = 33,
		MappingNotify = 34,
		GenericEvent = 35,

		LASTEvent
	}

	[Flags]
	public enum SetWindowValuemask {
		Nothing = 0,
		BackPixmap = 1,
		BackPixel = 2,
		BorderPixmap = 4,
		BorderPixel = 8,
		BitGravity = 16,
		WinGravity = 32,
		BackingStore = 64,
		BackingPlanes = 128,
		BackingPixel = 256,
		OverrideRedirect = 512,
		SaveUnder = 1024,
		EventMask = 2048,
		DontPropagate = 4096,
		ColorMap = 8192,
		Cursor = 16384
	}

	public enum CreateWindowArgs {
		CopyFromParent = 0,
		ParentRelative = 1,
		InputOutput = 1,
		InputOnly = 2
	}

	public enum Gravity {
		ForgetGravity = 0,
		NorthWestGravity = 1,
		NorthGravity = 2,
		NorthEastGravity = 3,
		WestGravity = 4,
		CenterGravity = 5,
		EastGravity = 6,
		SouthWestGravity = 7,
		SouthGravity = 8,
		SouthEastGravity = 9,
		StaticGravity = 10
	}

	[CLSCompliant(false)]
	public enum XKeySym : uint {
		XK_BackSpace = 0xFF08,
		XK_Tab = 0xFF09,
		XK_Clear = 0xFF0B,
		XK_Return = 0xFF0D,
		XK_Home = 0xFF50,
		XK_Left = 0xFF51,
		XK_Up = 0xFF52,
		XK_Right = 0xFF53,
		XK_Down = 0xFF54,
		XK_Page_Up = 0xFF55,
		XK_Page_Down = 0xFF56,
		XK_End = 0xFF57,
		XK_Begin = 0xFF58,
		XK_Menu = 0xFF67,
		XK_Shift_L = 0xFFE1,
		XK_Shift_R = 0xFFE2,
		XK_Control_L = 0xFFE3,
		XK_Control_R = 0xFFE4,
		XK_Caps_Lock = 0xFFE5,
		XK_Shift_Lock = 0xFFE6,
		XK_Meta_L = 0xFFE7,
		XK_Meta_R = 0xFFE8,
		XK_Alt_L = 0xFFE9,
		XK_Alt_R = 0xFFEA,
		XK_Super_L = 0xFFEB,
		XK_Super_R = 0xFFEC,
		XK_Hyper_L = 0xFFED,
		XK_Hyper_R = 0xFFEE,
	}

	[Flags]
	public enum EventMask {
		NoEventMask = 0,
		KeyPressMask = 1 << 0,
		KeyReleaseMask = 1 << 1,
		ButtonPressMask = 1 << 2,
		ButtonReleaseMask = 1 << 3,
		EnterWindowMask = 1 << 4,
		LeaveWindowMask = 1 << 5,
		PointerMotionMask = 1 << 6,
		PointerMotionHintMask = 1 << 7,
		Button1MotionMask = 1 << 8,
		Button2MotionMask = 1 << 9,
		Button3MotionMask = 1 << 10,
		Button4MotionMask = 1 << 11,
		Button5MotionMask = 1 << 12,
		ButtonMotionMask = 1 << 13,
		KeymapStateMask = 1 << 14,
		ExposureMask = 1 << 15,
		VisibilityChangeMask = 1 << 16,
		StructureNotifyMask = 1 << 17,
		ResizeRedirectMask = 1 << 18,
		SubstructureNotifyMask = 1 << 19,
		SubstructureRedirectMask = 1 << 20,
		FocusChangeMask = 1 << 21,
		PropertyChangeMask = 1 << 22,
		ColormapChangeMask = 1 << 23,
		OwnerGrabButtonMask = 1 << 24
	}

	public enum GrabMode {
		GrabModeSync = 0,
		GrabModeAsync = 1
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct XStandardColormap {
		public IntPtr colormap;
		public IntPtr red_max;
		public IntPtr red_mult;
		public IntPtr green_max;
		public IntPtr green_mult;
		public IntPtr blue_max;
		public IntPtr blue_mult;
		public IntPtr base_pixel;
		public IntPtr visualid;
		public IntPtr killid;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	public struct XColor {
		public IntPtr pixel;
		[CLSCompliant(false)]
		public ushort red;
		[CLSCompliant(false)]
		public ushort green;
		[CLSCompliant(false)]
		public ushort blue;
		public byte flags;
		public byte pad;
	}

	public enum Atom {
		AnyPropertyType = 0,
		XA_PRIMARY = 1,
		XA_SECONDARY = 2,
		XA_ARC = 3,
		XA_ATOM = 4,
		XA_BITMAP = 5,
		XA_CARDINAL = 6,
		XA_COLORMAP = 7,
		XA_CURSOR = 8,
		XA_CUT_BUFFER0 = 9,
		XA_CUT_BUFFER1 = 10,
		XA_CUT_BUFFER2 = 11,
		XA_CUT_BUFFER3 = 12,
		XA_CUT_BUFFER4 = 13,
		XA_CUT_BUFFER5 = 14,
		XA_CUT_BUFFER6 = 15,
		XA_CUT_BUFFER7 = 16,
		XA_DRAWABLE = 17,
		XA_FONT = 18,
		XA_INTEGER = 19,
		XA_PIXMAP = 20,
		XA_POINT = 21,
		XA_RECTANGLE = 22,
		XA_RESOURCE_MANAGER = 23,
		XA_RGB_COLOR_MAP = 24,
		XA_RGB_BEST_MAP = 25,
		XA_RGB_BLUE_MAP = 26,
		XA_RGB_DEFAULT_MAP = 27,
		XA_RGB_GRAY_MAP = 28,
		XA_RGB_GREEN_MAP = 29,
		XA_RGB_RED_MAP = 30,
		XA_STRING = 31,
		XA_VISUALID = 32,
		XA_WINDOW = 33,
		XA_WM_COMMAND = 34,
		XA_WM_HINTS = 35,
		XA_WM_CLIENT_MACHINE = 36,
		XA_WM_ICON_NAME = 37,
		XA_WM_ICON_SIZE = 38,
		XA_WM_NAME = 39,
		XA_WM_NORMAL_HINTS = 40,
		XA_WM_SIZE_HINTS = 41,
		XA_WM_ZOOM_HINTS = 42,
		XA_MIN_SPACE = 43,
		XA_NORM_SPACE = 44,
		XA_MAX_SPACE = 45,
		XA_END_SPACE = 46,
		XA_SUPERSCRIPT_X = 47,
		XA_SUPERSCRIPT_Y = 48,
		XA_SUBSCRIPT_X = 49,
		XA_SUBSCRIPT_Y = 50,
		XA_UNDERLINE_POSITION = 51,
		XA_UNDERLINE_THICKNESS = 52,
		XA_STRIKEOUT_ASCENT = 53,
		XA_STRIKEOUT_DESCENT = 54,
		XA_ITALIC_ANGLE = 55,
		XA_X_HEIGHT = 56,
		XA_QUAD_WIDTH = 57,
		XA_WEIGHT = 58,
		XA_POINT_SIZE = 59,
		XA_RESOLUTION = 60,
		XA_COPYRIGHT = 61,
		XA_NOTICE = 62,
		XA_FONT_NAME = 63,
		XA_FAMILY_NAME = 64,
		XA_FULL_NAME = 65,
		XA_CAP_HEIGHT = 66,
		XA_WM_CLASS = 67,
		XA_WM_TRANSIENT_FOR = 68,

		XA_LAST_PREDEFINED = 68
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct XScreen {
		public IntPtr ext_data;
		public IntPtr display;
		public IntPtr root;
		public int width;
		public int height;
		public int mwidth;
		public int mheight;
		public int ndepths;
		public IntPtr depths;
		public int root_depth;
		public IntPtr root_visual;
		public IntPtr default_gc;
		public IntPtr cmap;
		public IntPtr white_pixel;
		public IntPtr black_pixel;
		public int max_maps;
		public int min_maps;
		public int backing_store;
		public bool save_unders;
		public IntPtr root_input_mask;
	}

	[Flags]
	public enum ChangeWindowAttributes {
		X = 1 << 0,
		Y = 1 << 1,
		Width = 1 << 2,
		Height = 1 << 3,
		BorderWidth = 1 << 4,
		Sibling = 1 << 5,
		StackMode = 1 << 6,

		//BackPixmap    (1L<<0)
		//BackPixel    (1L<<1)
		//SaveUnder    (1L<<10)
		//EventMask    (1L<<11)
		//DontPropagate    (1L<<12)
		//Colormap    (1L<<13)
		//Cursor    (1L<<14)
		//BorderPixmap    (1L<<2)
		//BorderPixel    (1L<<3)
		//BitGravity    (1L<<4)
		//WinGravity    (1L<<5)
		//BackingStore    (1L<<6)
		//BackingPlanes    (1L<<7)
		//BackingPixel    (1L<<8)
		OverrideRedirect = 1 << 9,
	}

	public enum StackMode {
		Above = 0,
		Below = 1,
		TopIf = 2,
		BottomIf = 3,
		Opposite = 4
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct XWindowChanges {
		public int x;
		public int y;
		public int width;
		public int height;
		public int border_width;
		public IntPtr sibling;
		public StackMode stack_mode;
	}

	[Flags]
	public enum ColorFlags {
		DoRed = 1 << 0,
		DoGreen = 1 << 1,
		DoBlue = 1 << 2
	}

	public enum NotifyMode {
		NotifyNormal = 0,
		NotifyGrab = 1,
		NotifyUngrab = 2
	}

	public enum NotifyDetail {
		NotifyAncestor = 0,
		NotifyVirtual = 1,
		NotifyInferior = 2,
		NotifyNonlinear = 3,
		NotifyNonlinearVirtual = 4,
		NotifyPointer = 5,
		NotifyPointerRoot = 6,
		NotifyDetailNone = 7
	}

	[Flags]
	public enum KeyMasks {
		ShiftMask = (1 << 0),
		LockMask = (1 << 1),
		ControlMask = (1 << 2),
		Mod1Mask = (1 << 3),
		Mod2Mask = (1 << 4),
		Mod3Mask = (1 << 5),
		Mod4Mask = (1 << 6),
		Mod5Mask = (1 << 7),

		ModMasks = Mod1Mask | Mod2Mask | Mod3Mask | Mod4Mask | Mod5Mask
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct XModifierKeymap {
		public int max_keypermod;
		public IntPtr modifiermap;
	}

	public enum PropertyMode {
		Replace = 0,
		Prepend = 1,
		Append = 2
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct XKeyBoardState {
		public int key_click_percent;
		public int bell_percent;
		[CLSCompliant(false)]
		public uint bell_pitch, bell_duration;
		public IntPtr led_mask;
		public int global_auto_repeat;
		public AutoRepeats auto_repeats;

		[StructLayout(LayoutKind.Explicit)]
		public struct AutoRepeats {
			[FieldOffset(0)]
			public byte first;

			[FieldOffset(31)]
			public byte last;
		}
	}

	[Flags]
	public enum GCFunction {
		GCFunction = 1 << 0,
		GCPlaneMask = 1 << 1,
		GCForeground = 1 << 2,
		GCBackground = 1 << 3,
		GCLineWidth = 1 << 4,
		GCLineStyle = 1 << 5,
		GCCapStyle = 1 << 6,
		GCJoinStyle = 1 << 7,
		GCFillStyle = 1 << 8,
		GCFillRule = 1 << 9,
		GCTile = 1 << 10,
		GCStipple = 1 << 11,
		GCTileStipXOrigin = 1 << 12,
		GCTileStipYOrigin = 1 << 13,
		GCFont = 1 << 14,
		GCSubwindowMode = 1 << 15,
		GCGraphicsExposures = 1 << 16,
		GCClipXOrigin = 1 << 17,
		GCClipYOrigin = 1 << 18,
		GCClipMask = 1 << 19,
		GCDashOffset = 1 << 20,
		GCDashList = 1 << 21,
		GCArcMode = 1 << 22
	}

	public enum GCJoinStyle {
		JoinMiter = 0,
		JoinRound = 1,
		JoinBevel = 2
	}

	public enum GCLineStyle {
		LineSolid = 0,
		LineOnOffDash = 1,
		LineDoubleDash = 2
	}

	public enum GCCapStyle {
		CapNotLast = 0,
		CapButt = 1,
		CapRound = 2,
		CapProjecting = 3
	}

	public enum GCFillStyle {
		FillSolid = 0,
		FillTiled = 1,
		FillStippled = 2,
		FillOpaqueStppled = 3
	}

	public enum GCFillRule {
		EvenOddRule = 0,
		WindingRule = 1
	}

	public enum GCArcMode {
		ArcChord = 0,
		ArcPieSlice = 1
	}

	public enum GCSubwindowMode {
		ClipByChildren = 0,
		IncludeInferiors = 1
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct XGCValues {
		public GXFunction function;
		public IntPtr plane_mask;
		public IntPtr foreground;
		public IntPtr background;
		public int line_width;
		public GCLineStyle line_style;
		public GCCapStyle cap_style;
		public GCJoinStyle join_style;
		public GCFillStyle fill_style;
		public GCFillRule fill_rule;
		public GCArcMode arc_mode;
		public IntPtr tile;
		public IntPtr stipple;
		public int ts_x_origin;
		public int ts_y_origin;
		public IntPtr font;
		public GCSubwindowMode subwindow_mode;
		public bool graphics_exposures;
		public int clip_x_origin;
		public int clib_y_origin;
		public IntPtr clip_mask;
		public int dash_offset;
		public byte dashes;
	}

	public enum GXFunction {
		GXclear = 0x0,        /* 0 */
		GXand = 0x1,        /* src AND dst */
		GXandReverse = 0x2,        /* src AND NOT dst */
		GXcopy = 0x3,        /* src */
		GXandInverted = 0x4,        /* NOT src AND dst */
		GXnoop = 0x5,        /* dst */
		GXxor = 0x6,        /* src XOR dst */
		GXor = 0x7,        /* src OR dst */
		GXnor = 0x8,        /* NOT src AND NOT dst */
		GXequiv = 0x9,        /* NOT src XOR dst */
		GXinvert = 0xa,        /* NOT dst */
		GXorReverse = 0xb,        /* src OR NOT dst */
		GXcopyInverted = 0xc,        /* NOT src */
		GXorInverted = 0xd,        /* NOT src OR dst */
		GXnand = 0xe,        /* NOT src OR NOT dst */
		GXset = 0xf        /* 1 */
	}

	public enum NetWindowManagerState {
		Remove = 0,
		Add = 1,
		Toggle = 2
	}

	public enum RevertTo {
		None = 0,
		PointerRoot = 1,
		Parent = 2
	}

	public enum MapState {
		IsUnmapped = 0,
		IsUnviewable = 1,
		IsViewable = 2
	}

	public enum CursorFontShape {
		XC_X_cursor = 0,
		XC_arrow = 2,
		XC_based_arrow_down = 4,
		XC_based_arrow_up = 6,
		XC_boat = 8,
		XC_bogosity = 10,
		XC_bottom_left_corner = 12,
		XC_bottom_right_corner = 14,
		XC_bottom_side = 16,
		XC_bottom_tee = 18,
		XC_box_spiral = 20,
		XC_center_ptr = 22,

		XC_circle = 24,
		XC_clock = 26,
		XC_coffee_mug = 28,
		XC_cross = 30,
		XC_cross_reverse = 32,
		XC_crosshair = 34,
		XC_diamond_cross = 36,
		XC_dot = 38,
		XC_dotbox = 40,
		XC_double_arrow = 42,
		XC_draft_large = 44,
		XC_draft_small = 46,

		XC_draped_box = 48,
		XC_exchange = 50,
		XC_fleur = 52,
		XC_gobbler = 54,
		XC_gumby = 56,
		XC_hand1 = 58,
		XC_hand2 = 60,
		XC_heart = 62,
		XC_icon = 64,
		XC_iron_cross = 66,
		XC_left_ptr = 68,
		XC_left_side = 70,

		XC_left_tee = 72,
		XC_left_button = 74,
		XC_ll_angle = 76,
		XC_lr_angle = 78,
		XC_man = 80,
		XC_middlebutton = 82,
		XC_mouse = 84,
		XC_pencil = 86,
		XC_pirate = 88,
		XC_plus = 90,
		XC_question_arrow = 92,
		XC_right_ptr = 94,

		XC_right_side = 96,
		XC_right_tee = 98,
		XC_rightbutton = 100,
		XC_rtl_logo = 102,
		XC_sailboat = 104,
		XC_sb_down_arrow = 106,
		XC_sb_h_double_arrow = 108,
		XC_sb_left_arrow = 110,
		XC_sb_right_arrow = 112,
		XC_sb_up_arrow = 114,
		XC_sb_v_double_arrow = 116,
		XC_sb_shuttle = 118,

		XC_sizing = 120,
		XC_spider = 122,
		XC_spraycan = 124,
		XC_star = 126,
		XC_target = 128,
		XC_tcross = 130,
		XC_top_left_arrow = 132,
		XC_top_left_corner = 134,
		XC_top_right_corner = 136,
		XC_top_side = 138,
		XC_top_tee = 140,
		XC_trek = 142,

		XC_ul_angle = 144,
		XC_umbrella = 146,
		XC_ur_angle = 148,
		XC_watch = 150,
		XC_xterm = 152,
		XC_num_glyphs = 154
	}

	public enum SystrayRequest {
		SYSTEM_TRAY_REQUEST_DOCK = 0,
		SYSTEM_TRAY_BEGIN_MESSAGE = 1,
		SYSTEM_TRAY_CANCEL_MESSAGE = 2
	}

	[Flags]
	public enum XSizeHintsFlags {
		USPosition = (1 << 0),
		USSize = (1 << 1),
		PPosition = (1 << 2),
		PSize = (1 << 3),
		PMinSize = (1 << 4),
		PMaxSize = (1 << 5),
		PResizeInc = (1 << 6),
		PAspect = (1 << 7),
		PAllHints = (PPosition | PSize | PMinSize | PMaxSize | PResizeInc | PAspect),
		PBaseSize = (1 << 8),
		PWinGravity = (1 << 9),
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct XSizeHints {
		public IntPtr flags;
		public int x;
		public int y;
		public int width;
		public int height;
		public int min_width;
		public int min_height;
		public int max_width;
		public int max_height;
		public int width_inc;
		public int height_inc;
		public int min_aspect_x;
		public int min_aspect_y;
		public int max_aspect_x;
		public int max_aspect_y;
		public int base_width;
		public int base_height;
		public int win_gravity;
	}

	[Flags]
	public enum XWMHintsFlags {
		InputHint = (1 << 0),
		StateHint = (1 << 1),
		IconPixmapHint = (1 << 2),
		IconWindowHint = (1 << 3),
		IconPositionHint = (1 << 4),
		IconMaskHint = (1 << 5),
		WindowGroupHint = (1 << 6),
		AllHints = (InputHint | StateHint | IconPixmapHint | IconWindowHint | IconPositionHint | IconMaskHint | WindowGroupHint)
	}

	public enum XInitialState {
		DontCareState = 0,
		NormalState = 1,
		ZoomState = 2,
		IconicState = 3,
		InactiveState = 4
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct XWMHints {
		public IntPtr flags;
		public bool input;
		public XInitialState initial_state;
		public IntPtr icon_pixmap;
		public IntPtr icon_window;
		public int icon_x;
		public int icon_y;
		public IntPtr icon_mask;
		public IntPtr window_group;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct XIconSize {
		public int min_width;
		public int min_height;
		public int max_width;
		public int max_height;
		public int width_inc;
		public int height_inc;
	}

	public delegate int XErrorHandler(IntPtr DisplayHandle, ref XErrorEvent error_event);

	public enum XRequest : byte {
		X_CreateWindow = 1,
		X_ChangeWindowAttributes = 2,
		X_GetWindowAttributes = 3,
		X_DestroyWindow = 4,
		X_DestroySubwindows = 5,
		X_ChangeSaveSet = 6,
		X_ReparentWindow = 7,
		X_MapWindow = 8,
		X_MapSubwindows = 9,
		X_UnmapWindow = 10,
		X_UnmapSubwindows = 11,
		X_ConfigureWindow = 12,
		X_CirculateWindow = 13,
		X_GetGeometry = 14,
		X_QueryTree = 15,
		X_InternAtom = 16,
		X_GetAtomName = 17,
		X_ChangeProperty = 18,
		X_DeleteProperty = 19,
		X_GetProperty = 20,
		X_ListProperties = 21,
		X_SetSelectionOwner = 22,
		X_GetSelectionOwner = 23,
		X_ConvertSelection = 24,
		X_SendEvent = 25,
		X_GrabPointer = 26,
		X_UngrabPointer = 27,
		X_GrabButton = 28,
		X_UngrabButton = 29,
		X_ChangeActivePointerGrab = 30,
		X_GrabKeyboard = 31,
		X_UngrabKeyboard = 32,
		X_GrabKey = 33,
		X_UngrabKey = 34,
		X_AllowEvents = 35,
		X_GrabServer = 36,
		X_UngrabServer = 37,
		X_QueryPointer = 38,
		X_GetMotionEvents = 39,
		X_TranslateCoords = 40,
		X_WarpPointer = 41,
		X_SetInputFocus = 42,
		X_GetInputFocus = 43,
		X_QueryKeymap = 44,
		X_OpenFont = 45,
		X_CloseFont = 46,
		X_QueryFont = 47,
		X_QueryTextExtents = 48,
		X_ListFonts = 49,
		X_ListFontsWithInfo = 50,
		X_SetFontPath = 51,
		X_GetFontPath = 52,
		X_CreatePixmap = 53,
		X_FreePixmap = 54,
		X_CreateGC = 55,
		X_ChangeGC = 56,
		X_CopyGC = 57,
		X_SetDashes = 58,
		X_SetClipRectangles = 59,
		X_FreeGC = 60,
		X_ClearArea = 61,
		X_CopyArea = 62,
		X_CopyPlane = 63,
		X_PolyPoint = 64,
		X_PolyLine = 65,
		X_PolySegment = 66,
		X_PolyRectangle = 67,
		X_PolyArc = 68,
		X_FillPoly = 69,
		X_PolyFillRectangle = 70,
		X_PolyFillArc = 71,
		X_PutImage = 72,
		X_GetImage = 73,
		X_PolyText8 = 74,
		X_PolyText16 = 75,
		X_ImageText8 = 76,
		X_ImageText16 = 77,
		X_CreateColormap = 78,
		X_FreeColormap = 79,
		X_CopyColormapAndFree = 80,
		X_InstallColormap = 81,
		X_UninstallColormap = 82,
		X_ListInstalledColormaps = 83,
		X_AllocColor = 84,
		X_AllocNamedColor = 85,
		X_AllocColorCells = 86,
		X_AllocColorPlanes = 87,
		X_FreeColors = 88,
		X_StoreColors = 89,
		X_StoreNamedColor = 90,
		X_QueryColors = 91,
		X_LookupColor = 92,
		X_CreateCursor = 93,
		X_CreateGlyphCursor = 94,
		X_FreeCursor = 95,
		X_RecolorCursor = 96,
		X_QueryBestSize = 97,
		X_QueryExtension = 98,
		X_ListExtensions = 99,
		X_ChangeKeyboardMapping = 100,
		X_GetKeyboardMapping = 101,
		X_ChangeKeyboardControl = 102,
		X_GetKeyboardControl = 103,
		X_Bell = 104,
		X_ChangePointerControl = 105,
		X_GetPointerControl = 106,
		X_SetScreenSaver = 107,
		X_GetScreenSaver = 108,
		X_ChangeHosts = 109,
		X_ListHosts = 110,
		X_SetAccessControl = 111,
		X_SetCloseDownMode = 112,
		X_KillClient = 113,
		X_RotateProperties = 114,
		X_ForceScreenSaver = 115,
		X_SetPointerMapping = 116,
		X_GetPointerMapping = 117,
		X_SetModifierMapping = 118,
		X_GetModifierMapping = 119,
		X_NoOperation = 127
	}

	[Flags]
	public enum XIMProperties {
		XIMPreeditArea = 0x0001,
		XIMPreeditCallbacks = 0x0002,
		XIMPreeditPosition = 0x0004,
		XIMPreeditNothing = 0x0008,
		XIMPreeditNone = 0x0010,
		XIMStatusArea = 0x0100,
		XIMStatusCallbacks = 0x0200,
		XIMStatusNothing = 0x0400,
		XIMStatusNone = 0x0800,
	}

	[Flags]
	public enum WindowType {
		Client = 1,
		Whole = 2,
		Both = 3
	}

	public enum XEmbedMessage {
		EmbeddedNotify = 0,
		WindowActivate = 1,
		WindowDeactivate = 2,
		RequestFocus = 3,
		FocusIn = 4,
		FocusOut = 5,
		FocusNext = 6,
		FocusPrev = 7,
		/* 8-9 were used for XEMBED_GRAB_KEY/XEMBED_UNGRAB_KEY */
		ModalityOn = 10,
		ModalityOff = 11,
		RegisterAccelerator = 12,
		UnregisterAccelerator = 13,
		ActivateAccelerator = 14
	}

	public enum ImageFormat {
		XYPixmap = 1,
		ZPixmap
	}

	// XInput2 structures

	public struct XIDeviceInfo {
		public int deviceid;
		public IntPtr name; // byte*
		public int use;
		public int attachment;
		public Boolean enabled;
		public int num_classes;
		public IntPtr classes; // XIAnyClassInfo **
	}

	public struct XIAnyClassInfo {
		public int type;
		public int sourceid;
	}

	public struct XIDeviceEvent {
		public int type;         /* GenericEvent */
		public IntPtr serial;       /* # of last request processed by server */
		public bool send_event;   /* true if this came from a SendEvent request */
		public IntPtr display;     /* Display the event was read from */
		public int extension;    /* XI extension offset */
		public XIEventType evtype;
		public IntPtr time;
		public int deviceid;
		public int sourceid;
		public int detail;
		public IntPtr root;
		public IntPtr @event;
		public IntPtr child;
		public double root_x;
		public double root_y;
		public double event_x;
		public double event_y;
		public int flags;
		public XIButtonState buttons;
		public XIValuatorState valuators;
		public XIModifierState mods;
		public XIGroupState @group;
	}

	public struct XIRawEvent {
		public int type;         /* GenericEvent */
		public IntPtr serial;       /* # of last request processed by server */
		public Boolean send_event;   /* true if this came from a SendEvent request */
		public IntPtr display;     /* Display the event was read from */
		public int extension;    /* XI extension offset */
		public XIEventType evtype;       /* XI_RawKeyPress, XI_RawKeyRelease, etc. */
		public IntPtr time;
		public int deviceid;
		public int sourceid;
		public int detail;
		public int flags;
		public XIValuatorState valuators;
		public IntPtr raw_values; // double        *
	}

	public struct XIButtonState {
		public int mask_len;
		public IntPtr mask; // byte*
	}

	public struct XIModifierState {
		public int @base;
		public int latched;
		public int locked;
		public int effective;
	}

	public struct XIGroupState {
		public int @base;
		public int latched;
		public int locked;
		public int effective;
	}

	public struct XIValuatorState {
		public int mask_len;
		public IntPtr mask; // byte*
		public IntPtr values; // double*
	}

	public struct XIEventMask : IDisposable {
		public int deviceid; // 0 = XIAllDevices, 1 = XIAllMasterDevices
		int mask_len;
		unsafe XIEventMasks* mask;

		public XIEventMask(int id, XIEventMasks m) {
			deviceid = id;
			mask_len = sizeof(XIEventMasks);
			unsafe
			{
				mask = (XIEventMasks*) Marshal.AllocHGlobal(mask_len);
				*mask = m;
			}
		}

		public void Dispose() {
			unsafe
			{
				Marshal.FreeHGlobal(new IntPtr((void*) mask));
			}
		}
	}

	public enum XIEventType {
		DeviceChanged = 1,
		KeyPress,
		KeyRelease,
		ButtonPress,
		ButtonRelease,
		Motion,
		Enter,
		Leave,
		FocusIn,
		FocusOut,
		HierarchyChanged,
		PropertyEvent,
		RawKeyPress,
		RawKeyRelease,
		RawButtonPress,
		RawButtonRelease,
		RawMotion,
		LastEvent = RawMotion
	}

	public enum XIEventMasks {
		DeviceChangedMask = (1 << (int) XIEventType.DeviceChanged),
		KeyPressMask = (1 << (int) XIEventType.KeyPress),
		KeyReleaseMask = (1 << (int) XIEventType.KeyRelease),
		ButtonPressMask = (1 << (int) XIEventType.ButtonPress),
		ButtonReleaseMask = (1 << (int) XIEventType.ButtonRelease),
		MotionMask = (1 << (int) XIEventType.Motion),
		EnterMask = (1 << (int) XIEventType.Enter),
		LeaveMask = (1 << (int) XIEventType.Leave),
		FocusInMask = (1 << (int) XIEventType.FocusIn),
		FocusOutMask = (1 << (int) XIEventType.FocusOut),
		HierarchyChangedMask = (1 << (int) XIEventType.HierarchyChanged),
		PropertyEventMask = (1 << (int) XIEventType.PropertyEvent),
		RawKeyPressMask = (1 << (int) XIEventType.RawKeyPress),
		RawKeyReleaseMask = (1 << (int) XIEventType.RawKeyRelease),
		RawButtonPressMask = (1 << (int) XIEventType.RawButtonPress),
		RawButtonReleaseMask = (1 << (int) XIEventType.RawButtonRelease),
		RawMotionMask = (1 << (int) XIEventType.RawMotion),
	}
}

#pragma warning restore 1591