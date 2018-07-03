//Ported from OpenTK, and excellent library.

#pragma warning disable 1591

namespace System.Graphics.AGL {
	public enum PixelFormatAttribute {
		NONE = 0,
		/// <summary>
		/// Choose from all available renderers.
		/// </summary>
		ALL_RENDERERS = 1,
		/// <summary>
		/// Depth of index buffer.
		/// </summary>
		BUFFER_SIZE = 2,
		/// <summary>
		/// Level in plane stacking.
		/// </summary>
		LEVEL = 3,
		/// <summary>
		/// Rgba format.
		/// </summary>
		RGBA = 4,
		/// <summary>
		/// Double buffer support.
		/// </summary>
		DOUBLEBUFFER = 5,
		/// <summary>
		/// Stereo buffer support.
		/// </summary>
		STEREO = 6,
		/// <summary>
		/// Number of aux buffers.
		/// </summary>
		AUX_BUFFERS = 7,
		/// <summary>
		/// Number of red component bits.
		/// </summary>
		RED_SIZE = 8,
		/// <summary>
		/// Number of green component bits.
		/// </summary>
		GREEN_SIZE = 9,
		/// <summary>
		/// Number of blue component bits.
		/// </summary>
		BLUE_SIZE = 10,
		/// <summary>
		/// Number of alpha component bits.
		/// </summary>
		ALPHA_SIZE = 11,
		/// <summary>
		/// Number of depth bits.
		/// </summary>
		DEPTH_SIZE = 12,
		/// <summary>
		/// Number of stencil bits.
		/// </summary>
		STENCIL_SIZE = 13,
		/// <summary>
		/// Number of red accum bits.
		/// </summary>
		ACCUM_RED_SIZE = 14,
		/// <summary>
		/// Number of green accum bits.
		/// </summary>
		ACCUM_GREEN_SIZE = 15,
		/// <summary>
		/// Number of blue accum bits.
		/// </summary>
		ACCUM_BLUE_SIZE = 16,
		/// <summary>
		/// Number of alpha accum bits.
		/// </summary>
		ACCUM_ALPHA_SIZE = 17,
		PIXEL_SIZE = 50,
		MINIMUM_POLICY = 51,
		MAXIMUM_POLICY = 52,
		OFFSCREEN = 53,
		FULLSCREEN = 54,
		SAMPLE_BUFFERS_ARB = 55,
		SAMPLES_ARB = 56,
		AUX_DEPTH_STENCIL = 57,
		COLOR_FLOAT = 58,
		MULTISAMPLE = 59,
		SUPERSAMPLE = 60,
		SAMPLE_ALPHA = 61,
	}

	public enum ExtendedAttribute {
		/// <summary>
		/// Frame buffer bits per pixel.
		/// </summary>
		PIXEL_SIZE = 50,
		/// <summary>
		/// Smallest requested buffers.
		/// </summary>
		MINIMUM_POLICY = 51,
		/// <summary>
		/// Largest requested buffers.
		/// </summary>
		MAXIMUM_POLICY = 52,
		/// <summary>
		/// Off-screen support.
		/// </summary>
		OFFSCREEN = 53,
		/// <summary>
		/// Full-screen support.
		/// </summary>
		FULLSCREEN = 54,
		/// <summary>
		/// Number of multi-sample buffers.
		/// </summary>
		SAMPLE_BUFFERS_ARB = 55,
		/// <summary>
		/// Number of samples per multi-sample buffer.
		/// </summary>
		SAMPLES_ARB = 56,
		/// <summary>
		/// Independent depth and/or stencil buffers for the aux buffer.
		/// </summary>
		AUX_DEPTH_STENCIL = 57,
		/// <summary>
		/// Color buffer floating-point pixels.
		/// </summary>
		COLOR_FLOAT = 58,
		/// <summary>
		/// Multi-sample support.
		/// </summary>
		MULTISAMPLE = 59,
		/// <summary>
		/// Supersample support.
		/// </summary>
		SUPERSAMPLE = 60,
		/// <summary>
		/// Alpha filtering support.
		/// </summary>
		SAMPLE_ALPHA = 61,
	}

	public enum RendererManagement {
		/// <summary>
		/// Render ID management.
		/// </summary>
		RENDERER_ID = 70,
		/// <summary>
		/// Single renderer for all screens.
		/// </summary>
		SINGLE_RENDERER = 71,
		/// <summary>
		/// Disable all failure recovery systems.
		/// </summary>
		NO_RECOVERY = 72,
		/// <summary>
		/// Hardware accelerated rendering.
		/// </summary>
		ACCELERATED = 73,
		/// <summary>
		/// Closest requested color buffer.
		/// </summary>
		CLOSEST_POLICY = 74,
		/// <summary>
		/// Renderer not needing failure recovery.
		/// </summary>
		ROBUST = 75,
		/// <summary>
		/// Back-buffer contents valid after swap.
		/// </summary>
		BACKING_STORE = 76,
		/// <summary>
		/// Renderer is multi-processor safe.
		/// </summary>
		MP_SAFE = 78,
		/// <summary>
		/// Can be used to render to a window.
		/// </summary>
		WINDOW = 80,
		/// <summary>
		/// Dingle window can span multiple screens.
		/// </summary>
		MULTISCREEN = 81,
		/// <summary>
		/// Virtual screen number.
		/// </summary>
		VIRTUAL_SCREEN = 82,
		/// <summary>
		/// Renderer is OpenGL compliant.
		/// </summary>
		COMPLIANT = 83,
		/// <summary>
		/// Can be used to render to a pbuffer.
		/// </summary>
		PBUFFER = 90,
		/// <summary>
		/// Can be used to render offline to a pbuffer.
		/// </summary>
		REMOTE_PBUFFER = 91,
	}

	public enum RendererProperties {
		OFFSCREEN = 53,
		FULLSCREEN = 54,
		RENDERER_ID = 70,
		ACCELERATED = 73,
		ROBUST = 75,
		BACKING_STORE = 76,
		MP_SAFE = 78,
		WINDOW = 80,
		MULTISCREEN = 81,
		COMPLIANT = 83,
		PBUFFER = 90,
		BUFFER_MODES = 100,
		MIN_LEVEL = 101,
		MAX_LEVEL = 102,
		COLOR_MODES = 103,
		ACCUM_MODES = 104,
		DEPTH_MODES = 105,
		STENCIL_MODES = 106,
		MAX_AUX_BUFFERS = 107,
		VIDEO_MEMORY = 120,
		TEXTURE_MEMORY = 121,
		RENDERER_COUNT = 128,
	}

	public enum ParameterNames {
		/// <summary>
		/// Enable or set the swap rectangle.
		/// </summary>
		SWAP_RECT = 200,
		/// <summary>
		/// Enable or set the buffer rectangle.
		/// </summary>
		BUFFER_RECT = 202,
		/// <summary>
		/// Enable or disable the swap async limit.
		/// </summary>
		SWAP_LIMIT = 203,
		/// <summary>
		/// Enable or disable colormap tracking.
		/// </summary>
		COLORMAP_TRACKING = 210,
		/// <summary>
		/// Set a colormap entry to {index, r, g, b}.
		/// </summary>
		COLORMAP_ENTRY = 212,
		/// <summary>
		/// Enable or disable all rasterization.
		/// </summary>
		RASTERIZATION = 220,
		/// <summary>
		/// Set the swap interval.
		/// </summary>
		SWAP_INTERVAL = 222,
		/// <summary>
		/// Validate state for multi-screen functionality.
		/// </summary>
		STATE_VALIDATION = 230,
		/// <summary>
		/// Set the buffer name. Allows for buffer resource sharing.
		/// </summary>
		BUFFER_NAME = 231,
		/// <summary>
		/// Order the current context in front of all the other contexts.
		/// </summary>
		ORDER_CONTEXT_TO_FRONT = 232,
		/// <summary>
		/// The ID of the drawable surface for the context.
		/// </summary>
		CONTEXT_SURFACE_ID = 233,
		/// <summary>
		/// The display Id of all displays affected by the context, up to a maximum of 32 displays.
		/// </summary>
		CONTEXT_DISPLAY_ID = 234,
		/// <summary>
		/// Position of OpenGL surface relative to window.
		/// </summary>
		SURFACE_ORDER = 235,
		/// <summary>
		/// Opacity of OpenGL surface.
		/// </summary>
		SURFACE_OPACITY = 236,
		/// <summary>
		/// Enable or set the drawable clipping region.
		/// </summary>
		CLIP_REGION = 254,
		/// <summary>
		/// Enable the capturing of only a single display for full-screen rendering.
		/// </summary>
		FS_CAPTURE_SINGLE = 255,
		/// <summary>
		/// Width and height of surface backing size.
		/// </summary>
		SURFACE_BACKING_SIZE = 304,
		/// <summary>
		/// Enable or disable surface backing size.
		/// </summary>
		ENABLE_SURFACE_BACKING_SIZE = 305,
		/// <summary>
		/// Flag surface to candidate for deletion.
		/// </summary>
		SURFACE_VOLATILE = 306,
	}

	public enum OptionName {
		/// <summary>
		/// Pixel format cache size.
		/// </summary>
		FORMAT_CACHE_SIZE = 501,
		/// <summary>
		/// Reset pixel format cache.
		/// </summary>
		CLEAR_FORMAT_CACHE = 502,
		/// <summary>
		/// Retain loaded renderers in memory.
		/// </summary>
		RETAIN_RENDERERS = 503,
	}

	public enum BufferModes {
		MONOSCOPIC_BIT = 0x00000001,
		STEREOSCOPIC_BIT = 0x00000002,
		SINGLEBUFFER_BIT = 0x00000004,
		DOUBLEBUFFER_BIT = 0x00000008,
	}

	public enum BitDepths {
		Bit0 = 0x00000001,
		Bit1 = 0x00000002,
		Bit2 = 0x00000004,
		Bit3 = 0x00000008,
		Bit4 = 0x00000010,
		Bit5 = 0x00000020,
		Bit6 = 0x00000040,
		Bit7 = 0x00000080,
		Bit8 = 0x00000100,
		Bit12 = 0x00000200,
		Bit16 = 0x00000400,
		Bit24 = 0x00000800,
		Bit32 = 0x00001000,
		Bit48 = 0x00002000,
		Bit64 = 0x00004000,
		Bit96 = 0x00008000,
		Bit128 = 0x00010000,
	}

	public enum ColorModes {
		RGB8_BIT = 0x00000001,
		RGB8_A8_BIT = 0x00000002,
		BGR233_BIT = 0x00000004,
		BGR233_A8_BIT = 0x00000008,
		RGB332_BIT = 0x00000010,
		RGB332_A8_BIT = 0x00000020,
		RGB444_BIT = 0x00000040,
		ARGB4444_BIT = 0x00000080,
		RGB444_A8_BIT = 0x00000100,
		RGB555_BIT = 0x00000200,
		ARGB1555_BIT = 0x00000400,
		RGB555_A8_BIT = 0x00000800,
		RGB565_BIT = 0x00001000,
		RGB565_A8_BIT = 0x00002000,
		RGB888_BIT = 0x00004000,
		ARGB8888_BIT = 0x00008000,
		RGB888_A8_BIT = 0x00010000,
		RGB101010_BIT = 0x00020000,
		ARGB2101010_BIT = 0x00040000,
		RGB101010_A8_BIT = 0x00080000,
		RGB121212_BIT = 0x00100000,
		ARGB12121212_BIT = 0x00200000,
		RGB161616_BIT = 0x00400000,
		ARGB16161616_BIT = 0x00800000,
		INDEX8_BIT = 0x20000000,
		INDEX16_BIT = 0x40000000,
		RGBFLOAT64_BIT = 0x01000000,
		RGBAFLOAT64_BIT = 0x02000000,
		RGBFLOAT128_BIT = 0x04000000,
		RGBAFLOAT128_BIT = 0x08000000,
		RGBFLOAT256_BIT = 0x10000000,
		RGBAFLOAT256_BIT = 0x20000000,
	}

	public enum AglError {
		/// <summary>
		/// No error.
		/// </summary>
		NoError = 0,
		/// <summary>
		/// Invalid pixel format attribute.
		/// </summary>
		BadAttribute = 10000,
		/// <summary>
		/// Invalid renderer property.
		/// </summary>
		BadProperty = 10001,
		/// <summary>
		/// Invalid pixel format.
		/// </summary>
		BadPixelFormat = 10002,
		/// <summary>
		/// Invalid renderer info.
		/// </summary>
		BadRendererInfo = 10003,
		/// <summary>
		/// Invalid context.
		/// </summary>
		BadContext = 10004,
		/// <summary>
		/// Invalid drawable.
		/// </summary>
		BadDrawable = 10005,
		/// <summary>
		/// Invalid graphics device.
		/// </summary>
		BadGraphicsDevice = 10006,
		/// <summary>
		/// Invalid context state.
		/// </summary>
		BadState = 10007,
		/// <summary>
		/// Invalid numerical value.
		/// </summary>
		BadValue = 10008,
		/// <summary>
		/// Invalid share context.
		/// </summary>
		BadMatch = 10009,
		/// <summary>
		/// Invalid enumerant.
		/// </summary>
		BadEnum = 10010,
		/// <summary>
		/// Invalid off-screen drawable.
		/// </summary>
		BadOffscreen = 10011,
		/// <summary>
		/// Invalid fullscreen drawable.
		/// </summary>
		BadFullscreen = 10012,
		/// <summary>
		/// Invalid window.
		/// </summary>
		BadWindow = 10013,
		/// <summary>
		/// Invalid pointer.
		/// </summary>
		BadPointer = 10014,
		/// <summary>
		/// Invalid code module.
		/// </summary>
		BadModule = 10015,
		/// <summary>
		/// Memory allocation failure.
		/// </summary>
		BadAlloc = 10016,
		/// <summary>
		/// CoreGraphics connection failure.
		/// </summary>
		BadConnection = 10017,
	}
}

#pragma warning restore 1591