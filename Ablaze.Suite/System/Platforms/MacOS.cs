#pragma warning disable 1591

using System.Drawing;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Platforms.MacOS {
	public struct CFArray {
		public IntPtr Ref;

		public CFArray(IntPtr reference) {
			Ref = reference;
		}

		public int Count {
			get {
				return CF.CFArrayGetCount(Ref);
			}
		}
		public IntPtr this[int index] {
			get {
				if (index >= Count || index < 0)
					throw new IndexOutOfRangeException();

				return CF.CFArrayGetValueAtIndex(Ref, index);
			}
		}
	}

	public struct CFDictionary {
		public CFDictionary(IntPtr reference) {
			Ref = reference;
		}

		public IntPtr Ref;

		public int Count {
			get {
				return CF.CFDictionaryGetCount(Ref);
			}
		}
		public double GetNumberValue(string key) {
			unsafe
			{
				double retval;
				IntPtr cfnum = CF.CFDictionaryGetValue(Ref,
					CF.CFSTR(key));

				CF.CFNumberGetValue(cfnum, CF.CFNumberType.kCFNumberDoubleType, &retval);

				return retval;
			}
		}
	}

	[SuppressUnmanagedCodeSecurity]
	public static class CF {
		public const string appServices = "/System/Library/Frameworks/ApplicationServices.framework/Versions/Current/ApplicationServices";

		[SuppressUnmanagedCodeSecurity]
		[DllImport(appServices)]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern int CFArrayGetCount(IntPtr theArray);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(appServices)]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern IntPtr CFArrayGetValueAtIndex(IntPtr theArray, int idx);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(appServices)]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern int CFDictionaryGetCount(IntPtr theDictionary);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(appServices)]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern IntPtr CFDictionaryGetValue(IntPtr theDictionary, IntPtr theKey);

		// this mirrors the definition in CFString.h.
		// I don't know why, but __CFStringMakeConstantString is marked as "private and should not be used directly"
		// even though the CFSTR macro just calls it.
		[SuppressUnmanagedCodeSecurity]
		[DllImport(appServices, EntryPoint = "__CFStringMakeConstantString")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern IntPtr CFSTR(string cStr);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(appServices)]
		[CLSCompliant(false)]
		public unsafe static extern bool CFNumberGetValue(IntPtr number, CFNumberType theType, int* valuePtr);
		[SuppressUnmanagedCodeSecurity]
		[DllImport(appServices)]
		[CLSCompliant(false)]
		public unsafe static extern bool CFNumberGetValue(IntPtr number, CFNumberType theType, double* valuePtr);

		public enum CFNumberType {
			kCFNumberSInt8Type = 1,
			kCFNumberSInt16Type = 2,
			kCFNumberSInt32Type = 3,
			kCFNumberSInt64Type = 4,
			kCFNumberFloat32Type = 5,
			kCFNumberFloat64Type = 6,
			kCFNumberCharType = 7,
			kCFNumberShortType = 8,
			kCFNumberIntType = 9,
			kCFNumberLongType = 10,
			kCFNumberLongLongType = 11,
			kCFNumberFloatType = 12,
			kCFNumberDoubleType = 13,
			kCFNumberCFIndexType = 14,
			kCFNumberNSIntegerType = 15,
			kCFNumberCGFloatType = 16,
			kCFNumberMaxType = 16
		};

		public enum CFRunLoopExitReason {
			Finished = 1,
			Stopped = 2,
			TimedOut = 3,
			HandledSource = 4
		}

		public static readonly IntPtr RunLoopModeDefault = CF.CFSTR("kCFRunLoopDefaultMode");

		[SuppressUnmanagedCodeSecurity]
		[DllImport(appServices)]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern IntPtr CFRunLoopGetCurrent();

		[SuppressUnmanagedCodeSecurity]
		[DllImport(appServices)]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern IntPtr CFRunLoopGetMain();

		[SuppressUnmanagedCodeSecurity]
		[DllImport(appServices)]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern CFRunLoopExitReason CFRunLoopRunInMode(
			IntPtr cfstrMode, double interval, bool returnAfterSourceHandled);
	}



	public enum MacOSKeyCode {
		A = 0,
		B = 11,
		C = 8,
		D = 2,
		E = 14,
		F = 3,
		G = 5,
		H = 4,
		I = 34,
		J = 38,
		K = 40,
		L = 37,
		M = 46,
		N = 45,
		O = 31,
		P = 35,
		Q = 12,
		R = 15,
		S = 1,
		T = 17,
		U = 32,
		V = 9,
		W = 13,
		X = 7,
		Y = 16,
		Z = 6,

		Key_1 = 18,
		Key_2 = 19,
		Key_3 = 20,
		Key_4 = 21,
		Key_5 = 23,
		Key_6 = 22,
		Key_7 = 26,
		Key_8 = 28,
		Key_9 = 25,
		Key_0 = 29,

		Space = 49,
		Tilde = 50,

		Minus = 27,
		Equals = 24,
		BracketLeft = 33,
		BracketRight = 30,
		Backslash = 42,
		Semicolon = 41,
		Quote = 39,
		Comma = 43,
		Period = 47,
		Slash = 44,

		Enter = 36,
		Tab = 48,
		Backspace = 51,
		Return = 52,
		Esc = 53,
		KeyPad_Decimal = 65,
		KeyPad_Multiply = 67,
		KeyPad_Add = 69,
		KeyPad_Divide = 75,
		KeyPad_Enter = 76,
		KeyPad_Subtract = 78,
		KeyPad_Equal = 81,
		KeyPad_0 = 82,
		KeyPad_1 = 83,
		KeyPad_2 = 84,
		KeyPad_3 = 85,
		KeyPad_4 = 86,
		KeyPad_5 = 87,
		KeyPad_6 = 88,
		KeyPad_7 = 89,
		KeyPad_8 = 91,
		KeyPad_9 = 92,
		F1 = 122,
		F2 = 120,
		F3 = 99,
		F4 = 118,
		F5 = 96,
		F6 = 97,
		F7 = 98,
		F8 = 100,
		F9 = 101,
		F10 = 109,
		F11 = 103,
		F12 = 111,
		F13 = 105,
		F14 = 107,
		F15 = 113,

		Menu = 110,

		Insert = 114,
		Home = 115,
		Pageup = 116,
		Del = 117,
		End = 119,
		Pagedown = 121,
		Up = 126,
		Down = 125,
		Left = 123,
		Right = 124,
	}

	[Flags]
	public enum MacOSKeyModifiers {
		None = 0,
		Shift = 0x0200,
		CapsLock = 0x0400,
		Control = 0x1000,  // 
		Command = 0x0100,  // Open-Apple  - Windows key 
		Option = 0x0800,  // Option key is same position as the alt key on non-mac keyboards.
	}

	public enum CGDisplayErr {
	}

	public enum CGError {
		Success = 0,
		Failure = 1000,
		IllegalArgument = 1001,
		InvalidConnection = 1002,
		InvalidContext = 1003,
		CannotComplete = 1004,
		NotImplemented = 1006,
		RangeCheck = 1007,
		TypeCheck = 1008,
		InvalidOperation = 1010,
		NoneAvailable = 1011,
	}

	[SuppressUnmanagedCodeSecurity]
	public static class CG {
		public const string appServices = "/System/Library/Frameworks/ApplicationServices.framework/Versions/Current/ApplicationServices";

		// CGPoint -> HIPoint
		// CGSize -> HISize
		// CGRect -> HIRect

		[SuppressUnmanagedCodeSecurity]
		[DllImport(appServices, EntryPoint = "CGGetActiveDisplayList")]
		[CLSCompliant(false)]
		public unsafe static extern CGDisplayErr GetActiveDisplayList(int maxDisplays, IntPtr* activeDspys, out int dspyCnt);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(appServices, EntryPoint = "CGMainDisplayID")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern IntPtr MainDisplayID();

		[SuppressUnmanagedCodeSecurity]
		[DllImport(appServices, EntryPoint = "CGDisplayBounds")]
		[Security.SuppressUnmanagedCodeSecurity]
		public unsafe static extern HIRect DisplayBounds(IntPtr display);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(appServices, EntryPoint = "CGDisplayPixelsWide")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern int DisplayPixelsWide(IntPtr display);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(appServices, EntryPoint = "CGDisplayPixelsHigh")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern int DisplayPixelsHigh(IntPtr display);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(appServices, EntryPoint = "CGDisplayCurrentMode")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern IntPtr DisplayCurrentMode(IntPtr display);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(appServices, EntryPoint = "CGDisplayCapture")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern CGDisplayErr DisplayCapture(IntPtr display);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(appServices, EntryPoint = "CGDisplayRelease")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern CGDisplayErr DisplayRelease(IntPtr display);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(appServices, EntryPoint = "CGDisplayAvailableModes")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern IntPtr DisplayAvailableModes(IntPtr display);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(appServices, EntryPoint = "CGDisplaySwitchToMode")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern IntPtr DisplaySwitchToMode(IntPtr display, IntPtr displayMode);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(appServices, EntryPoint = "CGWarpMouseCursorPosition")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern CGError WarpMouseCursorPosition(HIPoint newCursorPosition);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(appServices, EntryPoint = "CGCursorIsVisible")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern bool CursorIsVisible();

		[SuppressUnmanagedCodeSecurity]
		[DllImport(appServices, EntryPoint = "CGDisplayShowCursor")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern CGError DisplayShowCursor(IntPtr display);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(appServices, EntryPoint = "CGDisplayHideCursor")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern CGError DisplayHideCursor(IntPtr display);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(appServices, EntryPoint = "CGAssociateMouseAndMouseCursorPosition")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern CGError AssociateMouseAndMouseCursorPosition(bool connected);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(appServices, EntryPoint = "CGSetLocalEventsSuppressionInterval")]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern CGError SetLocalEventsSuppressionInterval(double seconds);
	}





	[StructLayout(LayoutKind.Sequential)]
	public struct CarbonPoint {
		public short V;
		public short H;

		public CarbonPoint(short x, short y) {
			V = x;
			H = y;
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct Rect {
		public short top;
		public short left;
		public short bottom;
		public short right;

		public Rect(short _left, short _top, short _width, short _height) {
			top = _top;
			left = _left;
			bottom = (short) (_top + _height);
			right = (short) (_left + _width);
		}

		public short X {
			get {
				return left;
			}
			set {
				short width = Width;
				left = value;
				right = (short) (left + width);
			}
		}
		public short Y {
			get {
				return top;
			}
			set {
				short height = Height;
				top = value;
				bottom = (short) (top + height);
			}
		}
		public short Width {
			get {
				return (short) (right - left);
			}
			set {
				right = (short) (left + value);
			}
		}
		public short Height {
			get {
				return (short) (bottom - top);
			}
			set {
				bottom = (short) (top + value);
			}
		}

		public override string ToString() {
			return string.Format(
				"Rect: [{0}, {1}, {2}, {3}]", X, Y, Width, Height);
		}

		public Rectangle ToRectangle() {
			return new Rectangle(X, Y, Width, Height);
		}
	}





	[StructLayout(LayoutKind.Sequential)]
	public struct HIPoint {
		public float X;
		public float Y;
		public HIPoint(float x, float y) {
			X = x;
			Y = y;
		}
	}
	[StructLayout(LayoutKind.Sequential)]
	public struct HISize {
		public float Width;
		public float Height;
	}
	[StructLayout(LayoutKind.Sequential)]
	public struct HIRect {
		public HIPoint Origin;
		public HISize Size;

		public override string ToString() {
			return string.Format(
				"Rect: [{0}, {1}, {2}, {3}]", Origin.X, Origin.Y, Size.Width, Size.Height);
		}
	}





	[CLSCompliant(false)]
	public enum EventAttributes : uint {
		kEventAttributeNone = 0,
		kEventAttributeUserEvent = (1 << 0),
		kEventAttributeMonitored = 1 << 3,
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct EventTypeSpec {
		public EventTypeSpec(EventClass evtClass, AppEventKind evtKind) {
			this.EventClass = evtClass;
			this.EventKind = (uint) evtKind;
		}
		public EventTypeSpec(EventClass evtClass, AppleEventKind appleKind) {
			this.EventClass = evtClass;
			this.EventKind = (uint) appleKind;
		}
		public EventTypeSpec(EventClass evtClass, MouseEventKind evtKind) {
			this.EventClass = evtClass;
			this.EventKind = (uint) evtKind;
		}
		public EventTypeSpec(EventClass evtClass, KeyboardEventKind evtKind) {
			this.EventClass = evtClass;
			this.EventKind = (uint) evtKind;
		}
		public EventTypeSpec(EventClass evtClass, WindowEventKind evtKind) {
			this.EventClass = evtClass;
			this.EventKind = (uint) evtKind;
		}

		public EventClass EventClass;
		[CLSCompliant(false)]
		public uint EventKind;
	}

	public enum EventClass : int {
		/*
		kEventClassMouse            = FOUR_CHAR_CODE('mous'),
		kEventClassKeyboard         = FOUR_CHAR_CODE('keyb'),
		kEventClassTextInput        = FOUR_CHAR_CODE('text'),
		kEventClassApplication      = FOUR_CHAR_CODE('appl'),
		kEventClassAppleEvent       = FOUR_CHAR_CODE('eppc'),
		kEventClassMenu             = FOUR_CHAR_CODE('menu'),
		kEventClassWindow           = FOUR_CHAR_CODE('wind'),
		kEventClassControl          = FOUR_CHAR_CODE('cntl'),
		kEventClassCommand          = FOUR_CHAR_CODE('cmds')
		*/
		Mouse = 0x6d6f7573,
		Keyboard = 0x6b657962,
		Application = 0x6170706c,
		AppleEvent = 0x65707063,
		Menu = 0x6d656e75,
		Window = 0x77696e64,
	}
	public enum WindowEventKind : int {
		// window events
		WindowUpdate = 1,
		WindowDrawContent = 2,
		WindowDrawStructure = 3,
		WindowEraseContent = 4,
		WindowActivate = 5,
		WindowDeactivate = 6,
		WindowSizeChanged = 23,
		WindowBoundsChanging = 26,
		WindowBoundsChanged = 27,
		WindowClickDragRgn = 32,
		WindowClickResizeRgn = 33,
		WindowClickCollapseRgn = 34,
		WindowClickCloseRgn = 35,
		WindowClickZoomRgn = 36,
		WindowClickContentRgn = 37,
		WindowClickProxyIconRgn = 38,
		WindowClose = 72,
		WindowClosed = 73,
	}
	public enum MouseEventKind : int {
		MouseDown = 1,
		MouseUp = 2,
		MouseMoved = 5,
		MouseDragged = 6,
		MouseEntered = 8,
		MouseExited = 9,
		WheelMoved = 10,
	}
	public enum MouseButton : short {
		Primary = 1,
		Secondary = 2,
		Tertiary = 3,
	}

	public enum KeyboardEventKind : int {
		// raw keyboard events
		RawKeyDown = 1,
		RawKeyRepeat = 2,
		RawKeyUp = 3,
		RawKeyModifiersChanged = 4,
	}

	public enum AppEventKind : int {
		// application events
		AppActivated = 1,
		AppDeactivated = 2,
		AppQuit = 3,
		AppLaunchNotification = 4,
	}

	public enum AppleEventKind : int {
		AppleEvent = 1,
	}

	public enum EventParamName : int {
		WindowRef = 0x77696e64,           // typeWindowRef,

		// Mouse Events
		MouseLocation = 0x6d6c6f63,       // typeHIPoint
		WindowMouseLocation = 0x776d6f75, // typeHIPoint
		MouseButton = 0x6d62746e,         // typeMouseButton
		ClickCount = 0x63636e74,          // typeUInt32
		MouseWheelAxis = 0x6d776178,      // typeMouseWheelAxis
		MouseWheelDelta = 0x6d77646c,     // typeSInt32
		MouseDelta = 0x6d647461,          // typeHIPoint

		// Keyboard events
		KeyCode = 0x6b636f64,             // typeUInt32
		KeyMacCharCode = 0x6b636872,      // typechar
		KeyModifiers = 0x6b6d6f64,        // typeUInt32

	}
	public enum EventParamType : int {
		typeWindowRef = 0x77696e64,

		typeMouseButton = 0x6d62746e,
		typeMouseWheelAxis = 0x6d776178,
		typeHIPoint = 0x68697074,
		typeHISize = 0x6869737a,
		typeHIRect = 0x68697263,

		typeChar = 0x54455854,

		typeUInt32 = 0x6d61676e,
		typeSInt32 = 0x6c6f6e67,
		typeSInt16 = 0x73686f72,
		typeSInt64 = 0x636f6d70,
		typeIEEE32BitFloatingPoint = 0x73696e67,
		typeIEEE64BitFloatingPoint = 0x646f7562,
	}

	public enum EventMouseButton : int {
		Primary = 0,
		Secondary = 1,
		Tertiary = 2,
	}

	public enum WindowRegionCode : int {
		TitleBarRegion = 0,
		TitleTextRegion = 1,
		CloseBoxRegion = 2,
		ZoomBoxRegion = 3,
		DragRegion = 5,
		GrowRegion = 6,
		CollapseBoxRegion = 7,
		TitleProxyIconRegion = 8,
		StructureRegion = 32,
		ContentRegion = 33,
		UpdateRegion = 34,
		OpaqueRegion = 35,
		GlobalPortRegion = 40,
		ToolbarButtonRegion = 41
	};





	[CLSCompliant(false)]
	public enum WindowClass : uint {
		Alert = 1,             /* "I need your attention now."*/
		MovableAlert = 2,             /* "I need your attention now, but I'm kind enough to let you switch out of this app to do other things."*/
		Modal = 3,             /* system modal, not draggable*/
		MovableModal = 4,             /* application modal, draggable*/
		Floating = 5,             /* floats above all other application windows*/
		Document = 6,             /* document windows*/
		Desktop = 7,             /* desktop window (usually only one of these exists) - OS X only in CarbonLib 1.0*/
		Utility = 8,             /* Available in CarbonLib 1.1 and later, and in Mac OS X*/
		Help = 10,            /* Available in CarbonLib 1.1 and later, and in Mac OS X*/
		Sheet = 11,            /* Available in CarbonLib 1.3 and later, and in Mac OS X*/
		Toolbar = 12,            /* Available in CarbonLib 1.1 and later, and in Mac OS X*/
		Plain = 13,            /* Available in CarbonLib 1.2.5 and later, and Mac OS X*/
		Overlay = 14,            /* Available in Mac OS X*/
		SheetAlert = 15,            /* Available in CarbonLib 1.3 and later, and in Mac OS X 10.1 and later*/
		AltPlain = 16,            /* Available in CarbonLib 1.3 and later, and in Mac OS X 10.1 and later*/
		Drawer = 20,            /* Available in Mac OS X 10.2 or later*/
		All = 0xFFFFFFFFu    /* for use with GetFrontWindowOfClass, FindWindowOfClass, GetNextWindowOfClass*/
	}

	[Flags]
	[CLSCompliant(false)]
	public enum WindowAttributes : uint {
		NoAttributes = 0u,         /* no attributes*/
		CloseBox = (1u << 0),  /* window has a close box*/
		HorizontalZoom = (1u << 1),  /* window has horizontal zoom box*/
		VerticalZoom = (1u << 2),  /* window has vertical zoom box*/
		FullZoom = (VerticalZoom | HorizontalZoom),
		CollapseBox = (1u << 3),  /* window has a collapse box*/
		Resizable = (1u << 4),  /* window is resizable*/
		SideTitlebar = (1u << 5),  /* window wants a titlebar on the side    (floating window class only)*/
		NoUpdates = (1u << 16), /* this window receives no update events*/
		NoActivates = (1u << 17), /* this window receives no activate events*/
		NoBuffering = (1u << 20), /* this window is not buffered (Mac OS X only)*/
		StandardHandler = (1u << 25),
		InWindowMenu = (1u << 27),
		LiveResize = (1u << 28),
		StandardDocument = (CloseBox | FullZoom | CollapseBox | Resizable),
		StandardFloating = (CloseBox | CollapseBox)
	}

	[CLSCompliant(false)]
	public enum WindowPositionMethod : uint {
		CenterOnMainScreen = 1,
		CenterOnParentWindow = 2,
		CenterOnParentWindowScreen = 3,
		CascadeOnMainScreen = 4,
		CascadeOnParentWindow = 5,
		CascadeOnParentWindowScreen = 6,
		CascadeStartAtParentWindowScreen = 10,
		AlertPositionOnMainScreen = 7,
		AlertPositionOnParentWindow = 8,
		AlertPositionOnParentWindowScreen = 9
	}

	public delegate OSStatus MacOSEventHandler(IntPtr inCaller, IntPtr inEvent, IntPtr userData);

	public enum WindowPartCode : short {
		inDesk = 0,
		inNoWindow = 0,
		inMenuBar = 1,
		inSysWindow = 2,
		inContent = 3,
		inDrag = 4,
		inGrow = 5,
		inGoAway = 6,
		inZoomIn = 7,
		inZoomOut = 8,
		inCollapseBox = 11,
		inProxyIcon = 12,
		inToolbarButton = 13,
		inStructure = 15,
	}





	public enum GestaltSelector {
		SystemVersion = 0x73797376,  // FOUR_CHAR_CODE("sysv"), /* system version*/
		SystemVersionMajor = 0x73797331,  // FOUR_CHAR_CODE("sys1"), /* The major system version number; in 10.4.17 this would be the decimal value 10 */
		SystemVersionMinor = 0x73797332,  // FOUR_CHAR_CODE("sys2"), /* The minor system version number; in 10.4.17 this would be the decimal value 4 */
		SystemVersionBugFix = 0x73797333,  // FOUR_CHAR_CODE("sys3") /* The bug fix system version number; in 10.4.17 this would be the decimal value 17 */
	};





	public enum ProcessApplicationTransformState : int {
		kProcessTransformToForegroundApplication = 1,
	}

	public struct ProcessSerialNumber {
		[CLSCompliant(false)]
		public ulong high;
		[CLSCompliant(false)]
		public ulong low;
	}



	[CLSCompliant(false)]
	public enum HICoordinateSpace {
		_72DPIGlobal = 1,
		ScreenPixel = 2,
		Window = 3,
		View = 4
	};

	public enum OSStatus {
		NoError = 0,

		ParameterError = -50,                          /*error in user parameter list*/
		NoHardwareError = -200,                         /*Sound Manager Error Returns*/
		NotEnoughHardwareError = -201,                         /*Sound Manager Error Returns*/
		UserCanceledError = -128,
		QueueError = -1,                           /*queue element not found during deletion*/
		VTypErr = -2,                           /*invalid queue element*/
		CorErr = -3,                           /*core routine number out of range*/
		UnimpErr = -4,                           /*unimplemented core routine*/
		SlpTypeErr = -5,                           /*invalid queue element*/
		SeNoDB = -8,                           /*no debugger installed to handle debugger command*/
		ControlErr = -17,                          /*I/O System Errors*/
		StatusErr = -18,                          /*I/O System Errors*/
		ReadErr = -19,                          /*I/O System Errors*/
		WritErr = -20,                          /*I/O System Errors*/
		BadUnitErr = -21,                          /*I/O System Errors*/
		UnitEmptyErr = -22,                          /*I/O System Errors*/
		OpenErr = -23,                          /*I/O System Errors*/
		ClosErr = -24,                          /*I/O System Errors*/
		DRemovErr = -25,                          /*tried to remove an open driver*/
		DInstErr = -26,                          /*DrvrInstall couldn't find driver in resources*/

		// Window Manager result codes.
		InvalidWindowPtr = -5600,
		UnsupportedWindowAttributesForClass = -5601,
		WindowDoesNotHaveProxy = -5602,
		WindowPropertyNotFound = -5604,
		UnrecognizedWindowClass = -5605,
		CorruptWindowDescription = -5606,
		UserWantsToDragWindow = -5607,
		WindowsAlreadyInitialized = -5608,
		FloatingWindowsNotInitialized = -5609,
		WindowNotFound = -5610,
		WindowDoesNotFitOnscreen = -5611,
		WindowAttributeImmutable = -5612,
		WindowAttributesConflict = -5613,
		WindowManagerInternalError = -5614,
		WindowWrongState = -5615,
		WindowGroupInvalid = -5616,
		WindowAppModalStateAlreadyExists = -5617,
		WindowNoAppModalState = -5618,
		WindowDoesntSupportFocus = -30583,
		WindowRegionCodeInvalid = -30593,

		// Event Manager result codes
		EventAlreadyPosted = -9860,
		EventTargetBusy = -9861,
		EventDeferAccessibilityEvent = -9865,
		EventInternalError = -9868,
		EventParameterNotFound = -9870,
		EventNotHandled = -9874,
		EventLoopTimedOut = -9875,
		EventLoopQuit = -9876,
		EventNotInQueue = -9877,
		HotKeyExists = -9878,
		EventPassToNextTarget = -9880

	}

	[SuppressUnmanagedCodeSecurity]
	public static class NativeApi {
		/// <summary>
		/// Carbon library
		/// </summary>
		public const string Carbon = "/System/Library/Frameworks/Carbon.framework/Versions/Current/Carbon";

		[DllImport(Carbon)]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern EventClass GetEventClass(IntPtr inEvent);
		[SuppressUnmanagedCodeSecurity]
		[DllImport(Carbon)]
		[CLSCompliant(false)]
		public static extern uint GetEventKind(IntPtr inEvent);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Carbon, EntryPoint = "CreateNewWindow")]
		[CLSCompliant(false)]
		public static extern OSStatus _CreateNewWindow(WindowClass @class, WindowAttributes attributes, ref Rect r, out IntPtr window);

		[CLSCompliant(false)]
		public static IntPtr CreateNewWindow(WindowClass @class, WindowAttributes attributes, Rect r) {
			IntPtr retval;
			OSStatus stat = _CreateNewWindow(@class, attributes, ref r, out retval);
			if (stat != OSStatus.NoError)
				throw new InvalidOperationException(stat.ToString());

			return retval;
		}

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Carbon)]
		public static extern void DisposeWindow(IntPtr window);




		[SuppressUnmanagedCodeSecurity]
		[DllImport(Carbon)]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern void ShowWindow(IntPtr window);
		[SuppressUnmanagedCodeSecurity]
		[DllImport(Carbon)]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern void HideWindow(IntPtr window);
		[SuppressUnmanagedCodeSecurity]
		[DllImport(Carbon)]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern bool IsWindowVisible(IntPtr window);
		[SuppressUnmanagedCodeSecurity]
		[DllImport(Carbon)]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern void SelectWindow(IntPtr window);




		[SuppressUnmanagedCodeSecurity]
		[DllImport(Carbon)]
		[CLSCompliant(false)]
		public static extern OSStatus RepositionWindow(IntPtr window, IntPtr parentWindow, WindowPositionMethod method);
		[SuppressUnmanagedCodeSecurity]
		[DllImport(Carbon)]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern void SizeWindow(IntPtr window, short w, short h, bool fUpdate);
		[SuppressUnmanagedCodeSecurity]
		[DllImport(Carbon)]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern void MoveWindow(IntPtr window, short x, short y, bool fUpdate);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Carbon)]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern OSStatus GetWindowBounds(IntPtr window, WindowRegionCode regionCode, out Rect globalBounds);
		public static Rect GetWindowBounds(IntPtr window, WindowRegionCode regionCode) {
			Rect retval;
			OSStatus result = GetWindowBounds(window, regionCode, out retval);

			if (result != OSStatus.NoError)
				throw new InvalidOperationException(result.ToString());

			return retval;
		}

		//[SuppressUnmanagedCodeSecurity][DllImport(carbon)]
		//public static extern void MoveWindow(IntPtr window, short hGlobal, short vGlobal, bool front);




		[SuppressUnmanagedCodeSecurity]
		[DllImport(Carbon)]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern IntPtr GetEventDispatcherTarget();

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Carbon, EntryPoint = "ReceiveNextEvent")]
		[CLSCompliant(false)]
		public static extern OSStatus ReceiveNextEvent(uint inNumTypes,
			IntPtr inList,
			double inTimeout,
			bool inPullEvent,
			out IntPtr outEvent);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Carbon)]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern void SendEventToEventTarget(IntPtr theEvent, IntPtr theTarget);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Carbon)]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern void ReleaseEvent(IntPtr theEvent);

		public static void SendEvent(IntPtr theEvent) {
			IntPtr theTarget = GetEventDispatcherTarget();
			SendEventToEventTarget(theEvent, theTarget);
		}

		// Processes events in the queue and then returns.
		public static void ProcessEvents() {
			IntPtr theEvent;
			IntPtr theTarget = GetEventDispatcherTarget();

			for (;;) {
				OSStatus status = ReceiveNextEvent(0, IntPtr.Zero, 0.0, true, out theEvent);

				if (status == OSStatus.EventLoopTimedOut)
					break;

				if (status != OSStatus.NoError) {
					break;
				}
				if (theEvent == IntPtr.Zero)
					break;

				try {
					SendEventToEventTarget(theEvent, theTarget);
				} catch {
				}

				ReleaseEvent(theEvent);
			}

		}



		[StructLayout(LayoutKind.Sequential)]

		public struct EventRecord {
			[CLSCompliant(false)]
			public ushort what;
			[CLSCompliant(false)]
			public uint message;
			[CLSCompliant(false)]
			public uint when;
			public CarbonPoint where;
			[CLSCompliant(false)]
			public uint modifiers;
		}

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Carbon)]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern bool ConvertEventRefToEventRecord(IntPtr inEvent, out EventRecord outEvent);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Carbon)]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern OSStatus AEProcessAppleEvent(ref EventRecord theEventRecord);

		public static void ProcessAppleEvent(IntPtr inEvent) {
			EventRecord record;

			ConvertEventRefToEventRecord(inEvent, out record);
			AEProcessAppleEvent(ref record);
		}






		[SuppressUnmanagedCodeSecurity]
		[DllImport(Carbon, EntryPoint = "CreateEvent")]
		[CLSCompliant(false)]
		public static extern OSStatus _CreateEvent(IntPtr inAllocator,
			EventClass inClassID, UInt32 kind, Double when,
			EventAttributes flags, out IntPtr outEvent);

		public static IntPtr CreateWindowEvent(WindowEventKind kind) {
			IntPtr retval;

			OSStatus stat = _CreateEvent(IntPtr.Zero, EventClass.Window, (uint) kind,
				0, EventAttributes.kEventAttributeNone, out retval);

			if (stat != OSStatus.NoError) {
				throw new InvalidOperationException(stat.ToString());
			}

			return retval;
		}

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Carbon)]
		[CLSCompliant(false)]
		public static extern OSStatus GetEventParameter(
			IntPtr inEvent, EventParamName inName, EventParamType inDesiredType,
			IntPtr outActualType, uint inBufferSize, IntPtr outActualSize, IntPtr outData);

		public static MacOSKeyCode GetEventKeyboardKeyCode(IntPtr inEvent) {
			int code;

			unsafe
			{
				int* codeAddr = &code;

				OSStatus result = NativeApi.GetEventParameter(inEvent,
					 EventParamName.KeyCode, EventParamType.typeUInt32, IntPtr.Zero,
					 (uint) System.Runtime.InteropServices.Marshal.SizeOf(typeof(UInt32)), IntPtr.Zero,
					 (IntPtr) codeAddr);

				if (result != OSStatus.NoError) {
					throw new InvalidOperationException(result.ToString());
				}
			}

			return (MacOSKeyCode) code;
		}

		public static char GetEventKeyboardChar(IntPtr inEvent) {
			char code;

			unsafe
			{
				char* codeAddr = &code;

				OSStatus result = NativeApi.GetEventParameter(inEvent,
					 EventParamName.KeyMacCharCode, EventParamType.typeChar, IntPtr.Zero,
					 (uint) System.Runtime.InteropServices.Marshal.SizeOf(typeof(char)), IntPtr.Zero,
					 (IntPtr) codeAddr);

				if (result != OSStatus.NoError) {
					throw new InvalidOperationException(result.ToString());
				}
			}

			return code;
		}

		public static MouseButton GetEventMouseButton(IntPtr inEvent) {
			int button;

			unsafe
			{
				int* btn = &button;

				OSStatus result = NativeApi.GetEventParameter(inEvent,
						EventParamName.MouseButton, EventParamType.typeMouseButton, IntPtr.Zero,
						(uint) System.Runtime.InteropServices.Marshal.SizeOf(typeof(short)), IntPtr.Zero,
						(IntPtr) btn);

				if (result != OSStatus.NoError)
					throw new InvalidOperationException(result.ToString());
			}

			return (MouseButton) button;
		}
		public static int GetEventMouseWheelDelta(IntPtr inEvent) {
			int delta;

			unsafe
			{
				int* d = &delta;

				OSStatus result = NativeApi.GetEventParameter(inEvent,
					 EventParamName.MouseWheelDelta, EventParamType.typeSInt32,
					 IntPtr.Zero, (uint) sizeof(int), IntPtr.Zero, (IntPtr) d);

				if (result != OSStatus.NoError)
					throw new InvalidOperationException(result.ToString());
			}

			return delta;
		}

		public static OSStatus GetEventWindowMouseLocation(IntPtr inEvent, out HIPoint pt) {
			HIPoint point;

			unsafe
			{
				HIPoint* parm = &point;
				OSStatus result = NativeApi.GetEventParameter(inEvent,
						EventParamName.WindowMouseLocation, EventParamType.typeHIPoint, IntPtr.Zero,
						(uint) System.Runtime.InteropServices.Marshal.SizeOf(typeof(HIPoint)), IntPtr.Zero,
						(IntPtr) parm);
				pt = point;

				return result;
			}
		}

		public static OSStatus GetEventMouseDelta(IntPtr inEvent, out HIPoint pt) {
			HIPoint point;

			unsafe
			{
				HIPoint* parm = &point;
				OSStatus result = NativeApi.GetEventParameter(inEvent,
						EventParamName.MouseDelta, EventParamType.typeHIPoint, IntPtr.Zero,
						(uint) System.Runtime.InteropServices.Marshal.SizeOf(typeof(HIPoint)), IntPtr.Zero,
						(IntPtr) parm);
				pt = point;

				return result;
			}
		}

		public static OSStatus GetEventWindowRef(IntPtr inEvent, out IntPtr windowRef) {
			IntPtr retval;

			unsafe
			{
				IntPtr* parm = &retval;
				OSStatus result = NativeApi.GetEventParameter(inEvent,
					EventParamName.WindowRef, EventParamType.typeWindowRef, IntPtr.Zero,
					(uint) sizeof(IntPtr), IntPtr.Zero, (IntPtr) parm);

				windowRef = retval;

				return result;
			}
		}

		public static OSStatus GetEventMouseLocation(IntPtr inEvent, out HIPoint pt) {
			HIPoint point;

			unsafe
			{
				HIPoint* parm = &point;

				OSStatus result = NativeApi.GetEventParameter(inEvent,
						EventParamName.MouseLocation, EventParamType.typeHIPoint, IntPtr.Zero,
						(uint) System.Runtime.InteropServices.Marshal.SizeOf(typeof(HIPoint)), IntPtr.Zero,
						(IntPtr) parm);

				pt = point;

				return result;
			}

		}
		public static MacOSKeyModifiers GetEventKeyModifiers(IntPtr inEvent) {
			uint code;

			unsafe
			{
				uint* codeAddr = &code;

				OSStatus result = NativeApi.GetEventParameter(inEvent,
					 EventParamName.KeyModifiers, EventParamType.typeUInt32, IntPtr.Zero,
					 (uint) System.Runtime.InteropServices.Marshal.SizeOf(typeof(uint)), IntPtr.Zero,
					 (IntPtr) codeAddr);

				if (result != OSStatus.NoError) {
					throw new InvalidOperationException(result.ToString());
				}
			}

			return (MacOSKeyModifiers) code;
		}




		[SuppressUnmanagedCodeSecurity]
		[DllImport(Carbon, EntryPoint = "InstallEventHandler")]
		[CLSCompliant(false)]
		public static extern OSStatus _InstallEventHandler(
								IntPtr eventTargetRef, IntPtr handlerProc,
								int numtypes, EventTypeSpec[] typeList,
								IntPtr userData, IntPtr handlerRef);

		public static void InstallWindowEventHandler(IntPtr windowRef, IntPtr uppHandlerProc,
				EventTypeSpec[] eventTypes, IntPtr userData, IntPtr handlerRef) {
			IntPtr windowTarget = GetWindowEventTarget(windowRef);
			OSStatus error = _InstallEventHandler(windowTarget, uppHandlerProc,
									eventTypes.Length, eventTypes,
									userData, handlerRef);
			if (error != OSStatus.NoError) {
				throw new InvalidOperationException(error.ToString());
			}
		}

		public static void InstallApplicationEventHandler(IntPtr uppHandlerProc,
				EventTypeSpec[] eventTypes, IntPtr userData, IntPtr handlerRef) {

			OSStatus error = _InstallEventHandler(GetApplicationEventTarget(), uppHandlerProc,
									eventTypes.Length, eventTypes,
									userData, handlerRef);

			if (error != OSStatus.NoError) {
				throw new InvalidOperationException(error.ToString());
			}

		}

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Carbon)]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern OSStatus RemoveEventHandler(IntPtr inHandlerRef);




		[SuppressUnmanagedCodeSecurity]
		[DllImport(Carbon)]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern IntPtr GetWindowEventTarget(IntPtr window);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Carbon)]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern IntPtr GetApplicationEventTarget();




		[SuppressUnmanagedCodeSecurity]
		[DllImport(Carbon)]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern IntPtr NewEventHandlerUPP(MacOSEventHandler handler);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Carbon)]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern void DisposeEventHandlerUPP(IntPtr userUPP);




		[SuppressUnmanagedCodeSecurity]
		[DllImport(Carbon)]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern int TransformProcessType(ref ProcessSerialNumber psn, ProcessApplicationTransformState type);
		[SuppressUnmanagedCodeSecurity]
		[DllImport(Carbon)]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern int GetCurrentProcess(ref ProcessSerialNumber psn);
		[SuppressUnmanagedCodeSecurity]
		[DllImport(Carbon)]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern int SetFrontProcess(ref ProcessSerialNumber psn);




		[SuppressUnmanagedCodeSecurity]
		[DllImport(Carbon)]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static IntPtr CGColorSpaceCreateDeviceRGB();
		[SuppressUnmanagedCodeSecurity]
		[DllImport(Carbon)]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static IntPtr CGDataProviderCreateWithData(IntPtr info, IntPtr[] data, int size, IntPtr releasefunc);
		[SuppressUnmanagedCodeSecurity]
		[DllImport(Carbon)]
		[CLSCompliant(false)]
		public extern static IntPtr CGImageCreate(int width, int height, int bitsPerComponent, int bitsPerPixel, int bytesPerRow, IntPtr colorspace, uint bitmapInfo, IntPtr provider, IntPtr decode, int shouldInterpolate, int intent);
		[SuppressUnmanagedCodeSecurity]
		[DllImport(Carbon)]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static void SetApplicationDockTileImage(IntPtr imageRef);
		[SuppressUnmanagedCodeSecurity]
		[DllImport(Carbon)]
		[Security.SuppressUnmanagedCodeSecurity]
		public extern static void RestoreApplicationDockTileImage();



		[SuppressUnmanagedCodeSecurity]
		[DllImport(Carbon)]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern IntPtr GetControlBounds(IntPtr control, out Rect bounds);

		public static Rect GetControlBounds(IntPtr control) {
			Rect retval;
			GetControlBounds(control, out retval);

			return retval;
		}

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Carbon)]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern OSStatus ActivateWindow(IntPtr inWindow, bool inActivate);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Carbon)]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern void RunApplicationEventLoop();

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Carbon)]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern void QuitApplicationEventLoop();

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Carbon)]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern IntPtr GetControlOwner(IntPtr control);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Carbon)]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern IntPtr HIViewGetWindow(IntPtr inView);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Carbon)]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern OSStatus HIViewGetFrame(IntPtr inView, out HIRect outRect);
		public static HIRect HIViewGetFrame(IntPtr inView) {
			HIRect retval;
			OSStatus result = HIViewGetFrame(inView, out retval);

			if (result != OSStatus.NoError)
				throw new InvalidOperationException(result.ToString());

			return retval;
		}


		[SuppressUnmanagedCodeSecurity]
		[DllImport(Carbon)]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern void SetWindowTitleWithCFString(IntPtr windowRef, IntPtr title);

		public static void SetWindowTitle(IntPtr windowRef, string title) {
			IntPtr str = __CFStringMakeConstantString(title);
			SetWindowTitleWithCFString(windowRef, str);

			// Apparently releasing this reference to the CFConstantString here
			// causes the program to crash on the fourth window created.  But I am 
			// afraid that not releasing the string would result in a memory leak, but that would
			// only be a serious issue if the window title is changed a lot.
			//CFRelease(str);
		}



		[SuppressUnmanagedCodeSecurity]
		[DllImport(Carbon, EntryPoint = "ChangeWindowAttributes")]
		[CLSCompliant(false)]
		public static extern OSStatus _ChangeWindowAttributes(IntPtr windowRef, WindowAttributes setTheseAttributes, WindowAttributes clearTheseAttributes);

		[CLSCompliant(false)]
		public static void ChangeWindowAttributes(IntPtr windowRef, WindowAttributes setTheseAttributes, WindowAttributes clearTheseAttributes) {
			OSStatus error = _ChangeWindowAttributes(windowRef, setTheseAttributes, clearTheseAttributes);

			if (error != OSStatus.NoError) {
				throw new InvalidOperationException(error.ToString());
			}
		}

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Carbon)]
		[CLSCompliant(false)]
		public static extern IntPtr __CFStringMakeConstantString(string cStr);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Carbon)]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern void CFRelease(IntPtr cfStr);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Carbon)]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern OSStatus CallNextEventHandler(IntPtr nextHandler, IntPtr theEvent);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Carbon)]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern IntPtr GetWindowPort(IntPtr windowRef);



		[SuppressUnmanagedCodeSecurity]
		[DllImport(Carbon)]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern IntPtr AcquireRootMenu();




		[SuppressUnmanagedCodeSecurity]
		[DllImport(Carbon)]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern bool IsWindowCollapsed(IntPtr windowRef);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Carbon, EntryPoint = "CollapseWindow")]
		[CLSCompliant(false)]
		public static extern OSStatus _CollapseWindow(IntPtr windowRef, bool collapse);

		public static void CollapseWindow(IntPtr windowRef, bool collapse) {
			OSStatus error = _CollapseWindow(windowRef, collapse);

			if (error != OSStatus.NoError) {
				throw new InvalidOperationException(error.ToString());
			}
		}

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Carbon, EntryPoint = "IsWindowInStandardState")]
		[CLSCompliant(false)]
		public static extern bool _IsWindowInStandardState(IntPtr windowRef, IntPtr inIdealSize, IntPtr outIdealStandardState);

		public static bool IsWindowInStandardState(IntPtr windowRef) {
			return _IsWindowInStandardState(windowRef, IntPtr.Zero, IntPtr.Zero);
		}

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Carbon, EntryPoint = "ZoomWindowIdeal")]
		[CLSCompliant(false)]
		public unsafe static extern OSStatus _ZoomWindowIdeal(IntPtr windowRef, short inPartCode, IntPtr toIdealSize);

		public static void ZoomWindowIdeal(IntPtr windowRef, WindowPartCode inPartCode, ref CarbonPoint toIdealSize) {
			CarbonPoint pt = toIdealSize;
			OSStatus error;
			IntPtr handle = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(CarbonPoint)));
			Marshal.StructureToPtr(toIdealSize, handle, false);

			error = _ZoomWindowIdeal(windowRef, (short) inPartCode, handle);

			toIdealSize = (CarbonPoint) Marshal.PtrToStructure(handle, typeof(CarbonPoint));

			Marshal.FreeHGlobal(handle);

			if (error != OSStatus.NoError) {
				throw new InvalidOperationException(error.ToString());
			}
		}

		[SuppressUnmanagedCodeSecurity]
		[DllImport(Carbon)]
		[Security.SuppressUnmanagedCodeSecurity]
		public unsafe static extern OSStatus DMGetGDeviceByDisplayID(
			IntPtr displayID, out IntPtr displayDevice, Boolean failToMain);



		// These seem to crash when called, and I haven't figured out why.
		// Currently a workaround is used to convert from screen to client coordinates.

		//[SuppressUnmanagedCodeSecurity][DllImport(carbon, EntryPoint="HIPointConvert")]
		//extern static OSStatus _HIPointConvert(ref HIPoint ioPoint,
		//    HICoordinateSpace inSourceSpace, IntPtr inSourceObject,
		//    HICoordinateSpace inDestinationSpace, IntPtr inDestinationObject);

		//public static HIPoint HIPointConvert(HIPoint inPoint,
		//    HICoordinateSpace inSourceSpace, IntPtr inSourceObject,
		//    HICoordinateSpace inDestinationSpace, IntPtr inDestinationObject)
		//{
		//    OSStatus error = _HIPointConvert(ref inPoint, inSourceSpace, inSourceObject, inDestinationSpace, inDestinationObject);

		//    if (error != OSStatus.NoError)
		//    {
		//        throw new MacOSException(error);
		//    }

		//    return inPoint;
		//}

		//[SuppressUnmanagedCodeSecurity][DllImport(carbon, EntryPoint = "HIViewConvertPoint")]
		//extern static OSStatus _HIViewConvertPoint(ref HIPoint inPoint, IntPtr inSourceView, IntPtr inDestView);

		//public static HIPoint HIViewConvertPoint( HIPoint point, IntPtr sourceHandle, IntPtr destHandle)
		//{
		//    //Carbon.Rect window_bounds = new Carbon.Rect();
		//    //Carbon.API.GetWindowBounds(handle, WindowRegionCode.StructureRegion /*32*/, out window_bounds);

		//    //point.X -= window_bounds.X;
		//    //point.Y -= window_bounds.Y;

		//    OSStatus error = _HIViewConvertPoint(ref point, sourceHandle, destHandle);

		//    if (error != OSStatus.NoError)
		//    {
		//        throw new MacOSException(error);
		//    }

		//   return point;
		//}



		public const string gestaltlib = "/System/Library/Frameworks/Carbon.framework/Versions/Current/Carbon";


		[SuppressUnmanagedCodeSecurity]
		[DllImport(gestaltlib)]
		[Security.SuppressUnmanagedCodeSecurity]
		public static extern OSStatus Gestalt(GestaltSelector selector, out int response);
	}
}

#pragma warning restore 1591