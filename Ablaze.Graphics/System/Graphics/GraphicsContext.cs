using System.Collections.Concurrent;
using System.Diagnostics;
using System.Platforms;
using System.Threading;
using System.Runtime.CompilerServices;

namespace System.Graphics {
	/// <summary>
	/// Signals the state during which the leak happened.
	/// </summary>
	public enum LeakedWhile {
		/// <summary>
		/// The resource leaked while the finalizer was being executed.
		/// </summary>
		Finalizing = 0,
		/// <summary>
		/// An attempt was made to delete the resource by calling the Dispose() method, but an error occurred.
		/// </summary>
		Disposing
	}

	/// <summary>
	/// Provides methods to manipulate a graphics context.
	/// </summary>
	public abstract class GraphicsContext : IDisposable {
		/// <summary>
		/// Handles a resource leak.
		/// </summary>
		/// <param name="sender">The object that caused the leak.</param>
		/// <param name="state">Specifies when the leak happened.</param>
		/// <param name="handle">The handle of the leaked resource (for debugging purposes).</param>
		public delegate void ResourceLeakedEventHandler(object sender, LeakedWhile state, IntPtr handle);
		private static ConcurrentDictionary<IntPtr, GraphicsContext> Contexts = new ConcurrentDictionary<IntPtr, GraphicsContext>();
		private Thread currentContextThread;
		private static bool isExiting;
		/// <summary>
		/// Gets or sets how to handle logged resource leaks.
		/// </summary>
		public static ExceptionMode LeakHandling = ExceptionMode.Log;
		private IntPtr handle;

		/// <summary>
		/// Fired when an OpenGL resource has leaked out of garbage collection.
		/// </summary>
		public static event ResourceLeakedEventHandler ResourceLeaked = HandleLeak;

		/// <summary>
		/// Gets the thread in which this context is current.
		/// </summary>
		public Thread CurrentContextThread {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return currentContextThread;
			}
		}

		/// <summary>
		/// Gets or sets whether VSync is enabled.
		/// </summary>
		public bool VSync {
			get {
				return SwapInterval > 0;
			}
			set {
				if (value && SwapInterval <= 0)
					SwapInterval = 1;
				else if (!value)
					SwapInterval = 0;
			}
		}

		/// <summary>
		/// Gets whether a graphics context is currently available.
		/// </summary>
		public static bool IsGraphicsContextAvailable {
			get {
				return GraphicsPlatform.Factory.GetCurrentContext() != IntPtr.Zero;
			}
		}

		/// <summary>
		/// Gets the GraphicsContext that is current on this thread.
		/// </summary>
		public static GraphicsContext CurrentContext {
			get {
				IntPtr handle = GraphicsPlatform.Factory.GetCurrentContext();
				if (handle == IntPtr.Zero)
					return null;
				GraphicsContext context;
				if (Contexts.TryGetValue(handle, out context))
					return context;
				else {
					GraphicsContext current = GraphicsPlatform.Factory.CreateGLContext(handle);
					Contexts.TryAdd(handle, current);
					return current;
				}
			}
		}

		/// <summary>
		/// Gets whether the GraphicsContext can be used on the current thread.
		/// </summary>
		public bool IsCurrent {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return GraphicsPlatform.Factory.GetCurrentContext() == handle;
			}
		}

		/// <summary>
		/// Gets whether this instance has been disposed of.
		/// </summary>
		public bool IsDisposed {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return handle == IntPtr.Zero;
			}
		}

		/// <summary>
		/// Gets the GraphicsMode of the instance.
		/// </summary>
		public GraphicsMode GraphicsMode {
			get;
			protected set;
		}

		/// <summary>
		/// Gets the handle of the GraphicsContext.
		/// </summary>
		public IntPtr Handle {
#if NET45
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
			get {
				return handle;
			}
			protected set {
				if (value == handle)
					return;
				if (handle != IntPtr.Zero) {
					GraphicsContext val;
					Contexts.TryRemove(handle, out val);
				}
				handle = value;
				if (value != IntPtr.Zero)
					Contexts.TryAdd(value, this);
			}
		}

		/// <summary>
		/// Gets or sets a the number of display refreshes between consecutive <see cref="SwapBuffers()"/> calls.
		/// </summary>
		public abstract int SwapInterval {
			get;
			set;
		}

		static GraphicsContext() {
			Windows.Forms.Application.ApplicationExit += Application_ApplicationExit;
		}

		/// <summary>
		/// Initializes a new graphics context.
		/// </summary>
		protected GraphicsContext() {
		}

		/// <summary>
		/// Initializes a new graphics context wrapper..
		/// </summary>
		/// <param name="handle">The handle of the display.</param>
		protected GraphicsContext(IntPtr handle) {
			Handle = handle;
		}

		/// <summary>
		/// Swaps buffers on a context. This presents the rendered scene to the user.
		/// </summary>
		public abstract void SwapBuffers();

		/// <summary>
		/// Makes the GraphicsContext the rendering target on the current thread (can be null).
		/// A context can only be current on a single thread. To transfer a context to another thread,
		/// MakeCurrent(null) must first be called on the thread that currently owns it.
		/// </summary>
		/// <param name="window">The window in which the context is initialized.</param>
		public virtual void MakeCurrent(Window window) {
			currentContextThread = window == null ? null : Thread.CurrentThread;
		}

		/// <summary>
		/// Notifies of a resource leak.
		/// </summary>
		/// <param name="sender">The object that caused the leak.</param>
		/// <param name="state">Specifies when the leak happened.</param>
		/// <param name="handle">The handle of the leaked resource (for debugging purposes).</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void RaiseResourceLeakedEvent(object sender, LeakedWhile state, IntPtr handle) {
			ResourceLeakedEventHandler handler = ResourceLeaked;
			if (handler != null)
				handler(sender, state, handle);
		}

		/// <summary>
		/// Makes the GraphicsContext the rendering target on the current thread (can be null).
		/// A context can only be current on a single thread. To transfer a context to another thread,
		/// MakeCurrent(null) must first be called on the thread that currently owns it.
		/// </summary>
		/// <param name="component">The component to make current onto.</param>
		public void MakeCurrent(GraphicsComponent component) {
			MakeCurrent(component == null ? null : component.Window);
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> representing this instance.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that contains a string representation of this instance.</returns>
		public override string ToString() {
			return handle.ToString();
		}

		/// <summary>
		/// Returns the hash code for this instance.
		/// </summary>
		/// <returns>A <see cref="System.Int32"/> with the hash code of this instance.</returns>
		public override int GetHashCode() {
			return handle.GetHashCode();
		}

		/// <summary>
		/// Returns whether the instance represents the specified instance.
		/// </summary>
		/// <param name="obj">The instance to compare to.</param>
		/// <returns>Whether the instance represents the specified instance.</returns>
		public override bool Equals(object obj) {
			GraphicsContext context = obj as GraphicsContext;
			return context != null && handle == context.handle;
		}

#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		private static void HandleLeak(object sender, LeakedWhile state, IntPtr handle) {
			if (isExiting || LeakHandling == ExceptionMode.Ignore || AppDomain.CurrentDomain.IsFinalizingForUnload() || Environment.HasShutdownStarted)
				return;
			string st8 = state.ToString().ToLower();
			if (LeakHandling == ExceptionMode.Throw) {
				if (sender == null)
					throw new ResourceLeakException(handle, "Resource leaked while " + st8 + ": { Identifier: " + handle.ToString() + " }");
				else
					throw new ResourceLeakException(sender, "Resource leaked while " + st8 + ": { Type: " + sender.GetType().Name + ", identifier: " + sender + ", handle: " + handle.ToString() + " }");
			} else {
				if (sender == null)
					ErrorHandler.LogException(new ResourceLeakException(handle, "Resource leaked while " + st8 + ": { Identifier: " + handle.ToString() + " }"));
				else
					ErrorHandler.LogException(new ResourceLeakException(sender, "Resource leaked while " + st8 + ": { Type: " + sender.GetType().Name + ", identifier: " + sender + ", handle: " + handle.ToString() + " }"));
			}
		}

		private static void Application_ApplicationExit(object sender, EventArgs e) {
			isExiting = true;
		}

		/// <summary>
		/// Disposes of the resources used by the GraphicsContext.
		/// </summary>
		~GraphicsContext() {
			Dispose();
		}

		/// <summary>
		/// Disposes of the resources used by the GraphicsContext.
		/// </summary>
		protected abstract void OnDisposed();

		/// <summary>
		/// Disposes of the resources used by the GraphicsContext.
		/// </summary>
		public void Dispose() {
			if (Handle == IntPtr.Zero)
				return;
			if (IsCurrent)
				MakeCurrent(null as Window);
			OnDisposed();
			Handle = IntPtr.Zero;
			GC.SuppressFinalize(this);
		}
	}
}