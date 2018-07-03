using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

namespace System.Windows.Forms {
	/// <summary>
	/// Scales UI upon resize automatically.
	/// </summary>
	public static class UIScaler {
		private static SizeF One = new SizeF(1f, 1f);
		private static ControlEventHandler ControlAdded = Control_ControlAdded, ControlRemoved = Control_ControlRemoved;
		private static EventHandler ControlResized = Control_Resize;
		private static ConcurrentDictionary<Control, ControlInfo> PreviousBounds = new ConcurrentDictionary<Control, ControlInfo>();
		private static HashSet<Control> Included = new HashSet<Control>();
		private static HashSet<Control> ExcludeFont = new HashSet<Control>();
		private static HashSet<Control> ExcludeSize = new HashSet<Control>();
		private static HashSet<Control> Excluded = new HashSet<Control>();
		private static object SyncRoot = new object();
		private static ReaderWriterLockSlim LockHandler = new ReaderWriterLockSlim();

		/// <summary>
		/// Adds the control to the UIScaler. If the control was previously excluded, it will be re-included.
		/// </summary>
		/// <param name="control">The control whose children to scale.</param>
		public static void AddToScaler(Control control) {
			if (control == null || StyledForm.DesignMode)
				return;
			LockHandler.EnterWriteLock();
			if (Excluded.Remove(control))
				return;
			LockHandler.ExitWriteLock();
			lock (SyncRoot) {
				if (!Included.Add(control))
					return;
			}
			SetAutoScaleModeToDpi(control);
			ControlInfo info = new ControlInfo(control.ClientRectangle, control.Font.Size);
			PreviousBounds.AddOrUpdate(control, info, (ctrl, sz) => info);
			control.Resize += ControlResized;
			control.ControlAdded += ControlAdded;
			control.ControlRemoved += ControlRemoved;
			control.Disposed += Control_Disposed;
		}

		/// <summary>
		/// Gets whether the specified control is currently being handled by the scaler..
		/// </summary>
		/// <param name="control">The control to check for scaling.</param>
		public static bool IsScaled(Control control) {
			return !(control == null || IsExcluded(control, true)) && PreviousBounds.ContainsKey(control);
		}

		/// <summary>
		/// Resets the control to its original attributes before scaling
		/// </summary>
		/// <param name="control">The contorl whose attributes to reset.</param>
		/// <param name="resetChildren">Whether to reset the attributes of its children as well.</param>
		public static void Reset(Control control, bool resetChildren = true) {
			ControlInfo info;
			if (control == null || IsExcluded(control, true) || !PreviousBounds.TryGetValue(control, out info))
				return;
			control.Bounds = Rectangle.Truncate(info.OriginalBounds);
			control.Font = new Font(control.Font.Name, info.OriginalFontSize);
			if (resetChildren) {
				foreach (Control ctrl in control.Controls)
					Reset(ctrl, true);
			}
		}

		/// <summary>
		/// Returns the original font size of the control, and the original bounds since the first time added to the scaler.
		/// </summary>
		/// <param name="control">The contorl whose attributes to return.</param>
		/// <param name="originalBounds">The original bounds of the control.</param>
		public static float GetOriginalAttributes(Control control, out RectangleF originalBounds) {
			ControlInfo info;
			if (PreviousBounds.TryGetValue(control, out info)) {
				originalBounds = info.OriginalBounds;
				return info.OriginalFontSize;
			} else {
				originalBounds = control.Bounds;
				return control.Font.Size;
			}
		}

		/// <summary>
		/// Excludes the specified control from font scaling, meaning that it will be scaled but font sizes will be left intact.
		/// </summary>
		/// <param name="control">The control to exclude from font scaling.</param>
		public static void ExcludeFontScaling(Control control) {
			lock (SyncRoot)
				ExcludeFont.Add(control);
		}

		/// <summary>
		/// If the control is excluded from font scaling, it wiil be re-included.
		/// </summary>
		/// <param name="control">The control whose fonts to scale.</param>
		public static void ReincludeFontScaling(Control control) {
			lock (SyncRoot)
				ExcludeFont.Remove(control);
		}

		/// <summary>
		/// Excludes the specified control from size scaling, meaning that the location will be moved appropriately, but size will be left intact.
		/// </summary>
		/// <param name="control">The control to exclude from size scaling.</param>
		public static void ExcludeSizeScaling(Control control) {
			lock (SyncRoot)
				ExcludeSize.Add(control);
		}

		/// <summary>
		/// If the control is excluded from size scaling, it wiil be re-included.
		/// </summary>
		/// <param name="control">The control whose size to scale.</param>
		public static void ReincludeSizeScaling(Control control) {
			lock (SyncRoot)
				ExcludeSize.Remove(control);
		}

		/// <summary>
		/// Adds the control to the exclusion list. It and its child controls will not be scaled.
		/// </summary>
		/// <param name="control">The control to add.</param>
		public static void Exclude(Control control) {
			if (control == null)
				return;
			LockHandler.EnterUpgradeableReadLock();
			if (!Excluded.Contains(control)) {
				LockHandler.EnterWriteLock();
				Excluded.Add(control);
				LockHandler.ExitWriteLock();
			}
			LockHandler.ExitUpgradeableReadLock();
		}

		/// <summary>
		/// Removes the specified control from the UIScaler.
		/// </summary>
		/// <param name="control">The control to remove.</param>
		public static void RemoveFromScaler(Control control) {
			if (control == null)
				return;
			LockHandler.EnterWriteLock();
			Excluded.Remove(control);
			LockHandler.ExitWriteLock();
			lock (SyncRoot)
				Included.Remove(control);
			control.Resize -= ControlResized;
			control.ControlAdded -= ControlAdded;
			control.ControlRemoved -= ControlRemoved;
			control.Disposed -= Control_Disposed;
			RemoveFromScalerInner(control);
		}

		private static void RemoveFromScalerInner(Control control) {
			ControlInfo temp;
			PreviousBounds.TryRemove(control, out temp);
			foreach (Control ctrl in control.Controls)
				RemoveFromScalerInner(ctrl);
		}

		private static void SetAutoScaleModeToDpi(Control control) {
			if (control == null)
				return;
			try {
				PropertyInfo property = control.GetType().GetProperty("AutoScaleMode", BindingFlags.Public | BindingFlags.Instance);
				if (property != null)
					property.SetValue(control, AutoScaleMode.Dpi, null);
			} catch {
			}
			foreach (Control child in control.Controls)
				SetAutoScaleModeToDpi(child);
		}


		/// <summary>
		/// Returns whether parentOrEqual is a parent or equal to the childControl.
		/// </summary>
		/// <param name="parentOrEqual">The control to check if it is parent or equal to childControl.</param>
		/// <param name="childControl">The supposed child control.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool IsParentOrEqual(Control parentOrEqual, Control childControl) {
			if (parentOrEqual == null)
				return false;
			while (childControl != null) {
				if (childControl == parentOrEqual)
					return true;
				else
					childControl = childControl.Parent;
			}
			return false;
		}

		/// <summary>
		/// Returns whether the specified control is excluded.
		/// </summary>
		/// <param name="control">The control to check.</param>
		/// <param name="checkParents">Whether to check whether any of its parents are excluded as well.</param>
		public static bool IsExcluded(Control control, bool checkParents) {
			while (control != null) {
				LockHandler.EnterReadLock();
				try {
					if (Excluded.Contains(control))
						return true;
				} finally {
					LockHandler.ExitReadLock();
				}
				if (checkParents)
					control = control.Parent;
				else
					return false;
			}
			return false;
		}

		/*/// <summary>
		/// Gets the current scale multiplier for the specified control.
		/// </summary>
		/// <param name="control">The control whose multiplier to return.</param>
		public static SizeF GetMultiplier(Control control) {
			while (control != null) {
				LockHandler.EnterReadLock();
				try {
					if (Excluded.Contains(control))
						return One;
				} finally {
					LockHandler.ExitReadLock();
				}
				lock (SyncRoot) {
					if (Included.Contains(control)) {
						Rectangle originalBounds = PreviousBounds[control].OriginalBounds;
						return new SizeF(control.Width / (float) originalBounds.Width, control.Height / (float) originalBounds.Height);
					} else
						control = control.Parent;
				}
			}
			return One;
		}*/

		private static void Scale(Control control, float xMult, float yMult, bool isExcludedCheck, bool scaleBounds, bool scaleFont) {
			if (IsExcluded(control, isExcludedCheck))
				return;
			ControlInfo info;
			float fontSize = control.Font.Size;
			Rectangle bounds = control.Bounds;
			if (!PreviousBounds.TryGetValue(control, out info))
				info = new ControlInfo(bounds, fontSize);
			if (!scaleBounds) {
				bool excludeSize;
				lock (SyncRoot)
					excludeSize = ExcludeSize.Contains(control);
				if (bounds != info.LastBounds) {
					if (bounds.X != 0)
						info.OriginalBounds.X *= bounds.X / (float) info.LastBounds.X;
					if (bounds.Y != 0)
						info.OriginalBounds.Y *= bounds.Y / (float) info.LastBounds.Y;
					if (!excludeSize) {
						if (bounds.Width != 0)
							info.OriginalBounds.Width *= bounds.Width / (float) info.LastBounds.Width;
						if (bounds.Height != 0)
							info.OriginalBounds.Height *= bounds.Height / (float) info.LastBounds.Height;
					}
					info.LastBounds = bounds;
				}
				bounds = new Rectangle((int) Math.Round(info.OriginalBounds.X * xMult), (int) Math.Round(info.OriginalBounds.Y * yMult), (int) Math.Round(info.OriginalBounds.Width * xMult), (int) Math.Round(info.OriginalBounds.Height * yMult));
				if (excludeSize)
					control.Location = bounds.Location;
				else
					control.Bounds = bounds;
				info.LastBounds = bounds;
			}
			bool excludeFont;
			lock (SyncRoot)
				excludeFont = ExcludeFont.Contains(control);
			if (!excludeFont) {
				if (Math.Abs(fontSize - info.LastFontSize) >= 0.01f)
					info.OriginalFontSize *= fontSize / info.LastFontSize;
				fontSize = info.OriginalFontSize * Math.Min(xMult, yMult);
				if (fontSize != 0f) {
					control.Font = new Font(control.Font.Name, fontSize);
					info.LastFontSize = fontSize;
				}
			}
			PreviousBounds.AddOrUpdate(control, info, (ctrl, sz) => info);
			foreach (Control child in control.Controls)
				Scale(child, xMult, yMult, false, true, true);
		}

		private static void Control_ControlAdded(object sender, ControlEventArgs e) {
			Control parent = e.Control.Parent;
			ControlInfo info;
			SetAutoScaleModeToDpi(e.Control);
			if (PreviousBounds.TryGetValue(parent, out info)) {
				SizeF scale = info.LastScale;
				Scale(e.Control, scale.Width, scale.Height, true, true, true);
			}
		}

		private static void Control_ControlRemoved(object sender, ControlEventArgs e) {
			Control parent = sender as Control;
			ControlInfo info;
			if (parent != null && PreviousBounds.TryGetValue(parent, out info)) {
				SizeF scale = info.LastScale;
				Scale(e.Control, 1f / scale.Width, 1f / scale.Height, true, true, true);
			}
		}

		private static void Control_Resize(object sender, EventArgs e) {
			Control control = sender as Control;
			ControlInfo info;
			if (control == null || IsExcluded(control, true) || !PreviousBounds.TryGetValue(control, out info))
				return;
			info.LastBounds = control.ClientRectangle;
			SizeF mult = info.LastScale;
			foreach (Control child in control.Controls)
				Scale(child, mult.Width, mult.Height, false, false, true);
		}

		private static void Control_Disposed(object sender, EventArgs e) {
			RemoveFromScaler(sender as Control);
		}

		private sealed class ControlInfo {
			public Rectangle LastBounds;
			public RectangleF OriginalBounds;
			public float OriginalFontSize, LastFontSize;

			public SizeF LastScale {
				get {
					if (OriginalBounds.Width == 0f || OriginalBounds.Height == 0f)
						return One;
					else
						return new SizeF(LastBounds.Width / OriginalBounds.Width, LastBounds.Height / OriginalBounds.Height);
				}
			}

			/*public float LastFontScale {
				get {
					SizeF scale = LastScale;
					return Math.Min(scale.Width, scale.Height);
				}
			}*/

			public ControlInfo(Rectangle originalBounds, float originalFontSize) {
				OriginalBounds = originalBounds;
				LastBounds = originalBounds;
				OriginalFontSize = originalFontSize;
				LastFontSize = originalFontSize;
			}
		}
	}
}