using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
#if !NET35
using System.Linq.Expressions;
#endif
using System.IO;
using System.Numerics;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System {
	/// <summary>
	/// A generic multi-purpose extension library.
	/// </summary>
	public static class Extensions {
		/// <summary>
		/// The delegate for memset().
		/// </summary>
		/// <param name="pointer">Points to the memory region to set.</param>
		/// <param name="value">The value to set the region to.</param>
		/// <param name="length">The size of the region.</param>
		private delegate void MemsetDelegate(IntPtr pointer, byte value, int length);
		private delegate void WndProcDelegate(Control ctrl, ref Message m);
		/// <summary>
		/// The type of object.
		/// </summary>
		public static readonly Type TypeOfObject = typeof(object);
		private static ConcurrentDictionary<Type, ConcurrentDictionary<Type, Delegate>> CompiledDelegates = new ConcurrentDictionary<Type, ConcurrentDictionary<Type, Delegate>>();
		private static FieldInfo nativeImageField = typeof(Image).GetField("nativeImage", BindingFlags.Instance | BindingFlags.NonPublic);
		private static FieldInfo penImmutable = typeof(Pen).GetField("immutable", BindingFlags.Instance | BindingFlags.NonPublic);
		private static FieldInfo brushImmutable = typeof(SolidBrush).GetField("immutable", BindingFlags.Instance | BindingFlags.NonPublic);
		private static Action<Control, int, bool> setState = (Action<Control, int, bool>) Delegate.CreateDelegate(typeof(Action<Control, int, bool>), typeof(Control).GetMethod("SetState", BindingFlags.Instance | BindingFlags.NonPublic));
		private static Action<Control, ControlStyles, bool> setStyle = (Action<Control, ControlStyles, bool>) Delegate.CreateDelegate(typeof(Action<Control, ControlStyles, bool>), typeof(Control).GetMethod("SetStyle", BindingFlags.Instance | BindingFlags.NonPublic));
		private static WndProcDelegate WndProcMethod = (WndProcDelegate) Delegate.CreateDelegate(typeof(WndProcDelegate), typeof(Control).GetMethod("WndProc", BindingFlags.Instance | BindingFlags.NonPublic));
		private static MemsetDelegate MemsetNative;
		private static char[] Trim = new char[] { '0', '.' };
		/// <summary>
		/// Gets whether the form is currently running in design mode.
		/// </summary>
		public static readonly bool DesignMode = LicenseManager.UsageMode == LicenseUsageMode.Designtime;
		/// <summary>
		/// A list of image file extensions.
		/// </summary>
		public const string ImageFileExtensions = "*.jpg;*.jpeg;*.png;*.gif;*.bmp;*.ico;*.dib;*.rle;*.jpe;*.jfif;*.emf;*.wmf;*.tif;*.tiff;";

		static Extensions() {
			DynamicMethod dynamicMethod = new DynamicMethod("memset", MethodAttributes.Public | MethodAttributes.Static, CallingConventions.Standard,
				null, new Type[] { typeof(IntPtr), typeof(byte), typeof(int) }, typeof(PixelWorker), true);
			ILGenerator generator = dynamicMethod.GetILGenerator();
			generator.Emit(OpCodes.Ldarg_0);
			generator.Emit(OpCodes.Ldarg_1);
			generator.Emit(OpCodes.Ldarg_2);
			generator.Emit(OpCodes.Initblk);
			generator.Emit(OpCodes.Ret);
			MemsetNative = (MemsetDelegate) dynamicMethod.CreateDelegate(typeof(MemsetDelegate));
		}

		/// <summary>
		/// Gets whether Aero is currently enabled.
		/// </summary>
		public static bool IsAeroEnabled {
			get {
				if (!DesignMode && Platform.IsWindowsVistaOrNewer) {
					int enabled = 0;
					Platforms.Windows.NativeApi.DwmIsCompositionEnabled(ref enabled);
					return enabled == 1;
				} else
					return false;
			}
		}

		/// <summary>
		/// Calles the WndProc method of the specified control, and returns the result.
		/// </summary>
		/// <param name="ctrl">The control to send the specified message to.</param>
		/// <param name="msg">The message to send.</param>
		public static IntPtr CallWndProc(this Control ctrl, ref Message msg) {
			WndProcMethod(ctrl, ref msg);
			return msg.Result;
		}

		/// <summary>
		/// Sets every byte in the specified array to the specified value.
		/// </summary>
		/// <param name="array">The array whose values to set.</param>
		/// <param name="value">The value to set the region to.</param>
		/// <param name="startIndex">The first index to set.</param>
		/// <param name="length">The size of the region.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static unsafe void Memset(this byte[] array, byte value, int startIndex, int length) {
			fixed (byte* ptr = array)
				MemsetNative(new IntPtr(ptr + startIndex), value, length);
		}

		/// <summary>
		/// Sets every byte in the specified memory region to the specified value.
		/// </summary>
		/// <param name="pointer">Points to the memory region to set.</param>
		/// <param name="value">The value to set the region to.</param>
		/// <param name="length">The size of the region.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void Memset(this IntPtr pointer, byte value, int length) {
			MemsetNative(pointer, value, length);
		}

		/// <summary>
		/// Returns whether the specified collections contain the same items are equal ignoring order but not ignoring multiple instances of each item.
		/// Example: {0, 4, 6, 6} is equal to {6, 0, 4, 6}, but not equal to {0, 6, 4, 6, 6} which is not equal to {4, 4, 0, 6, 6}.
		/// </summary>
		/// <typeparam name="T">The type of items in the list.</typeparam>
		/// <param name="list1">The items in the first list to compare.</param>
		/// <param name="list2">The items in the second list to compare with.</param>
		/// <param name="comparer">The item equality comparer to use.</param>
		public static bool EqualsUnordered<T>(this IList<T> list1, IList<T> list2, IEqualityComparer<T> comparer = null) {
			if (list1 == list2)
				return true;
			else if (list1 == null || list2 == null || list1.Count != list2.Count)
				return false;
			else if (list1.Count == 0)
				return true;
			else
				return EqualsUnordered((IEnumerable<T>) list1, (IEnumerable<T>) list2, comparer);
		}

		/// <summary>
		/// Returns whether the specified collections contain the same items are equal ignoring order but not ignoring multiple instances of each item.
		/// Example: {0, 4, 6, 6} is equal to {6, 0, 4, 6}, but not equal to {0, 6, 4, 6, 6} which is not equal to {4, 4, 0, 6, 6}.
		/// </summary>
		/// <typeparam name="T">The type of items in the list.</typeparam>
		/// <param name="list1">The items in the first list to compare.</param>
		/// <param name="list2">The items in the second list to compare with.</param>
		/// <param name="comparer">The item equality comparer to use.</param>
		public static bool EqualsUnordered<T>(this IEnumerable<T> list1, IEnumerable<T> list2, IEqualityComparer<T> comparer = null) {
			if (list1 == list2)
				return true;
			else if (list1 == null || list2 == null)
				return false;
			Dictionary<T, int> counts = comparer == null ? new Dictionary<T, int>() : new Dictionary<T, int>(comparer);
			foreach (T s in list1) {
				if (counts.ContainsKey(s))
					counts[s]++;
				else
					counts.Add(s, 1);
			}
			foreach (T s in list2) {
				if (counts.ContainsKey(s))
					counts[s]--;
				else
					return false;
			}
			foreach (int value in counts.Values) {
				if (value != 0)
					return false;
			}
			return true;
		}

		/// <summary>
		/// Compares the contents of specified byte arrays and returns whether they are equal.
		/// </summary>
		/// <param name="array1">The array to compare.</param>
		/// <param name="array2">The array to compare with.</param>
		public static unsafe bool Equals(this byte[] array1, byte[] array2) {
			if (array1 == array2)
				return true;
			else if (array1 == null || array2 == null || array1.LongLength != array2.LongLength)
				return false;
			else if (array1.LongLength == 0)
				return true;
			else {
				fixed (byte* bytes1 = array1, bytes2 = array2)
					return Equals(bytes1, bytes2, array1.LongLength);
			}
		}

		/// <summary>
		/// Compares the two memory regions are returns whether they are equal.
		/// </summary>
		/// <param name="array1">The memory region to compare.</param>
		/// <param name="array2">The memory region to compare with.</param>
		/// <param name="length">The length of the memory region length in bytes.</param>
		[CLSCompliant(false)]
		public static unsafe bool Equals(byte* array1, byte* array2, long length) {
			if (length == 0 || array1 == array2)
				return true;
			int rem = (int) (length % (sizeof(long) * 16));
			long* b1 = (long*) array1;
			long* b2 = (long*) array2;
			long* e1 = (long*) (array1 + length - rem);
			while (b1 < e1) {
				if (*b1 != *b2 || b1[1] != b2[1] ||	b1[2] != b2[2] || b1[3] != b2[3] ||
					b1[4] != b2[4] || b1[5] != b2[5] ||	b1[6] != b2[6] || b1[7] != b2[7] ||
					b1[8] != b2[8] || b1[9] != b2[9] ||	b1[10] != b2[10] || b1[11] != b2[11] ||
					b1[12] != b2[12] || b1[13] != b2[13] ||	b1[14] != b2[14] || b1[15] != b2[15])
					return false;
				b1 += 16;
				b2 += 16;
			}
			length--;
			for (int i = 0; i < rem; i++)
				if (array1[length - i] != array2[length - i])
					return false;
			return true;
		}

		/// <summary>
		/// Concatenates the specified predicates by OR-ing their results.
		/// </summary>
		/// <typeparam name="T">The type of items to filter.</typeparam>
		/// <param name="predicates">The predicates to concatenate.</param>
		public static Predicate<T> Or<T>(params Predicate<T>[] predicates) {
			if (predicates == null || predicates.Length == 0)
				return item => true;
			else {
				return item => {
					foreach (Predicate<T> predicate in predicates) {
						if (predicate(item))
							return true;
					}
					return false;
				};
			}
		}

		/// <summary>
		/// Concatenates the specified predicates by AND-ing their results.
		/// </summary>
		/// <typeparam name="T">The type of items to filter.</typeparam>
		/// <param name="predicates">The predicates to concatenate.</param>
		public static Predicate<T> And<T>(params Predicate<T>[] predicates) {
			if (predicates == null || predicates.Length == 0)
				return item => true;
			else {
				return item => {
					foreach (Predicate<T> predicate in predicates) {
						if (!predicate(item))
							return false;
					}
					return true;
				};
			}
		}

		/// <summary>
		/// Gets a dictionary that represents the specified object (where keys represent properties).
		/// </summary>
		/// <param name="source">The object to serialize into a dictionary.</param>
		/// <param name="bindingAttr">The attirbutes to use when searching for properties.</param>
		public static Dictionary<string, object> ToDictionary(this object source, BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance) {
			if (source == null)
				return null;
			else
				return source.GetType().GetProperties(bindingAttr).ToDictionary(propInfo => propInfo.Name, propInfo => propInfo.GetValue(source, null));
		}

#if !NET35
		/// <summary>
		/// Gets the size of the specified class type.
		/// </summary>
		/// <typeparam name="T">The type of object whose size to find.</typeparam>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static int SizeOf<T>() where T : class {
			return Runtime.InteropServices.Marshal.ReadInt32(typeof(T).TypeHandle.Value, 4);
		}
#endif

		/// <summary>
		/// Combines the specified hash codes into a suitable hash code.
		/// </summary>
		/// <param name="h1">The hash code to combine.</param>
		/// <param name="h2">The hash code to combine with.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static int Combine(int h1, int h2) {
			return unchecked((int) ((uint) (h1 << 5) | (uint) h1 >> 27) + h1 ^ h2);
		}

		/// <summary>
		/// Returns a comparer object that wraps the specified comparison delegate.
		/// </summary>
		/// <typeparam name="T">The type of item to compare.</typeparam>
		/// <param name="comparison">The comparison delegate.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static IComparer<T> Create<T>(this Comparison<T> comparison) {
			return new ComparisonComparer<T>(comparison);
		}

		/// <summary>
		/// Returns a comparer object that wraps the specified equality comparison delegate.
		/// </summary>
		/// <typeparam name="T">The type of item to compare.</typeparam>
		/// <param name="comparison">The comparison delegate.</param>
		/// <param name="hashCode">The hash code generator to use. The rule is: equal items MUST have the save hash-code,
		/// and for the same item, the gerenated hash-code must always be the same.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static IEqualityComparer<T> Create<T>(this EqualityComparison<T> comparison, Func<T, int> hashCode) {
			return new EqualityComparisonComparer<T>(comparison, hashCode);
		}

		/// <summary>
		/// Removes all consecutive duplicate characters within a string (ex. 'kaabbbc' will become 'kabc').
		/// </summary>
		/// <param name="str">The string whose duplicate consecutive characters to remove.</param>
		public static string RemoveConsecutiveDuplicates(this string str) {
			if (string.IsNullOrEmpty(str))
				return str;
			StringBuilder builder = new StringBuilder(str.Length);
			char current = str[0];
			builder.Append(current);
			for (int i = 1; i < str.Length; i++) {
				if (str[i] != current) {
					current = str[i];
					builder.Append(current);
				}
			}
			return builder.ToString();
		}

		/// <summary>
		/// Removes consecutive duplicates of the specified character from the string.
		/// </summary>
		/// <param name="str">The string whose duplicate consecutive characters to remove.</param>
		/// <param name="character">The character whose duplicates to remove.</param>
		public static string RemoveConsecutiveDuplicates(this string str, char character) {
			if (string.IsNullOrEmpty(str))
				return str;
			StringBuilder builder = new StringBuilder(str.Length);
			char current = str[0];
			builder.Append(current);
			for (int i = 1; i < str.Length; i++) {
				if (str[i] == current) {
					if (character != current)
						builder.Append(current);
				} else {
					current = str[i];
					builder.Append(current);
				}
			}
			return builder.ToString();
		}

		/// <summary>
		/// Removes consecutive duplicate characters within a string if they match any of the specified characters.
		/// </summary>
		/// <param name="str">The string whose duplicate consecutive characters to remove.</param>
		/// <param name="chars">The characters whose duplicates to remove.</param>
		public static string RemoveConsecutiveDuplicates(this string str, params char[] chars) {
			if (string.IsNullOrEmpty(str) || chars == null || chars.Length == 0)
				return str;
			else
				return RemoveConsecutiveDuplicates(str, new HashSet<char>(chars));
		}

		/// <summary>
		/// Removes consecutive duplicate characters within a string if they match any of the specified characters.
		/// </summary>
		/// <param name="str">The string whose duplicate consecutive characters to remove.</param>
		/// <param name="chars">The characters whose duplicates to remove.</param>
		public static string RemoveConsecutiveDuplicates(this string str, HashSet<char> chars) {
			if (string.IsNullOrEmpty(str) || chars == null || chars.Count == 0)
				return str;
			StringBuilder builder = new StringBuilder(str.Length);
			char current = str[0];
			builder.Append(current);
			bool contains = chars.Contains(current);
			for (int i = 1; i < str.Length; i++) {
				if (str[i] == current) {
					if (!contains)
						builder.Append(current);
				} else {
					current = str[i];
					contains = chars.Contains(current);
					builder.Append(current);
				}
			}
			return builder.ToString();
		}

		/// <summary>
		/// Checks whether the specified string is considered a valid email.
		/// </summary>
		/// <param name="email">The string to check for validity as an email.</param>
		public static bool IsValidEmail(this string email) {
			if (string.IsNullOrEmpty(email))
				return false;
			for (int i = 0; i < email.Length; i++) {
				if (char.IsWhiteSpace(email[i]))
					return false;
			}
			try {
				return new Net.Mail.MailAddress(email).Address == email;
			} catch {
				return false;
			}
		}

		/// <summary>
		/// Removes leading and trailing characters that are not in the specified array of characters to keep.
		/// </summary>
		/// <param name="str">The string to trim.</param>
		/// <param name="toKeep">The characters that are not trimmed (if empty, and empty string is returned).</param>
		public static string TrimExcept(this string str, params char[] toKeep) {
			if (string.IsNullOrEmpty(str) || toKeep == null || toKeep.Length == 0)
				return string.Empty;
			int temp, start;
			char current;
			for (start = 0; start < str.Length; start++) {
				current = str[start];
				for (temp = 0; temp < toKeep.Length; temp++) {
					if (toKeep[temp] == current)
						goto Branch1;
				}
			}
			Branch1:
			int end;
			for (end = str.Length - 1; end >= start; end--) {
				current = str[end];
				for (temp = 0; temp < toKeep.Length; temp++) {
					if (toKeep[temp] == current)
						goto Branch2;
				}
			}
			Branch2:
			return end < start ? string.Empty : str.Substring(start, end + 1 - start);
		}

		/// <summary>
		/// Removes leading characters that are not in the specified array of characters to keep.
		/// </summary>
		/// <param name="str">The string to trim.</param>
		/// <param name="toKeep">The characters that are not trimmed (if empty, and empty string is returned).</param>
		public static string TrimStartExcept(this string str, params char[] toKeep) {
			if (string.IsNullOrEmpty(str) || toKeep == null || toKeep.Length == 0)
				return string.Empty;
			int temp, start;
			char current;
			for (start = 0; start < str.Length; start++) {
				current = str[start];
				for (temp = 0; temp < toKeep.Length; temp++) {
					if (toKeep[temp] == current)
						goto Found;
				}
			}
			Found:
			return str.Substring(start);
		}

		/// <summary>
		/// Removes trailing characters that are not in the specified array of characters to keep.
		/// </summary>
		/// <param name="str">The string to trim.</param>
		/// <param name="toKeep">The characters that are not trimmed (if empty, and empty string is returned).</param>
		public static string TrimEndExcept(this string str, params char[] toKeep) {
			if (string.IsNullOrEmpty(str) || toKeep == null || toKeep.Length == 0)
				return string.Empty;
			int temp, end;
			char current;
			for (end = str.Length - 1; end >= 0; end--) {
				current = str[end];
				for (temp = 0; temp < toKeep.Length; temp++) {
					if (toKeep[temp] == current)
						goto Found;
				}
			}
			Found:
			return end < 0 ? string.Empty : str.Substring(0, end + 1);
		}

		/// <summary>
		/// Removes all the elements that match the conditions defined by the specified predicate.
		/// </summary>
		/// <typeparam name="T">The type of items in the list.</typeparam>
		/// <param name="list">The list to filter.</param>
		/// <param name="match">The predicate delegate that defines the conditions of the elements to remove.</param>
		public static int RemoveAll<T>(this LinkedList<T> list, Predicate<T> match) {
			int count = 0;
			LinkedListNode<T> node = list.First;
			LinkedListNode<T> next;
			while (node != null) {
				next = node.Next;
				if (match(node.Value)) {
					list.Remove(node);
					count++;
				}
				node = next;
			}
			return count;
		}

		/// <summary>
		/// Sorts the specified IList in-place using QuickSort.
		/// </summary>
		/// <typeparam name="T">The type of elements in the list.</typeparam>
		/// <param name="list">The list to sort using the default comparer.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void Sort<T>(this IList<T> list) {
			Sort(list, 0, list.Count, null);
		}

		/// <summary>
		/// Sorts the specified IList in-place using QuickSort.
		/// </summary>
		/// <typeparam name="T">The type of elements in the list.</typeparam>
		/// <param name="list">The list to sort.</param>
		/// <param name="comparer">The comparer to use for sorting.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void Sort<T>(this IList<T> list, Comparison<T> comparer) {
			Sort(list, 0, list.Count, comparer == null ? null : Create(comparer));
		}

		/// <summary>
		/// Sorts the specified IList in-place using QuickSort.
		/// </summary>
		/// <typeparam name="T">The type of elements in the list.</typeparam>
		/// <param name="list">The list to sort.</param>
		/// <param name="comparer">The comparer to use for sorting.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void Sort<T>(this IList<T> list, IComparer<T> comparer) {
			Sort(list, 0, list.Count, comparer);
		}

		/// <summary>
		/// Sorts the specified IList in-place using QuickSort.
		/// </summary>
		/// <typeparam name="T">The type of elements in the list.</typeparam>
		/// <param name="list">The list to sort.</param>
		/// <param name="index">The index of the first element to sort.</param>
		/// <param name="count">The number of elements to sort.</param>
		/// <param name="comparer">The comparer to use for sorting.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void Sort<T>(this IList<T> list, int index, int count, IComparer<T> comparer) {
			QuickSort(list, index, index + count - 1, comparer == null ? Comparer<T>.Default : comparer);
		}

		private static IList<T> QuickSort<T>(IList<T> set, int lBound, int rBound, IComparer<T> comparer) {
			if (lBound >= rBound)
				return set;
			T pivot = set[rBound];
			int left = lBound - 1;
			int right = rBound;
			T temp;
			do {
				while (comparer.Compare(set[++left], pivot) < 0)
					;
				while (comparer.Compare(set[--right], pivot) > 0 && left != right)
					;
				if (left >= right)
					break;
				temp = set[left];
				set[left] = set[right];
				set[right] = temp;
			} while (true);
			temp = set[left];
			set[left] = set[rBound];
			set[rBound] = temp;
			int newPivot = left;
			QuickSort(set, lBound, newPivot - 1, comparer);
			QuickSort(set, newPivot + 1, rBound, comparer);
			return set;
		}

		/// <summary>
		/// Sets the specified state for the given control.
		/// </summary>
		/// <param name="contol">The control whose state to modify.</param>
		/// <param name="style">The style flags to modify.</param>
		/// <param name="flag">The new flag value.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void SetStyle(this Control contol, ControlStyles style, bool flag) {
			setStyle(contol, style, flag);
		}

		/// <summary>
		/// Sets the specified state for the given control.
		/// </summary>
		/// <param name="contol">The control whose state to modify.</param>
		/// <param name="key">The flag of the state to modify.</param>
		/// <param name="value">The new state value.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void SetState(this Control contol, int key, bool value) {
			setState(contol, key, value);
		}

		/// <summary>
		/// Translates the specified screen coordinates to the corresponding client coordinate relative to the specified control.
		/// </summary>
		/// <param name="ctrl">The control that the return value coordinate is respect with.</param>
		/// <param name="screenPoint">The point on the screen to translate to client coordinates.</param>
		public static Point PointToClient(this Control ctrl, Point screenPoint) {
			if (ctrl == null)
				return screenPoint;
			Form form = ctrl as Form;
			if (form == null) {
				do {
					screenPoint -= (Size) ctrl.Location;
					ctrl = ctrl.Parent;
				} while (ctrl != null);
			} else {
				do {
					screenPoint -= (Size) form.Location;
					form = form.MdiParent;
				} while (form != null);
			}
			return screenPoint;
		}

		/// <summary>
		/// WARNING: DO NOT USE! Replaces the specified method with another method dynamically at runtime (dirty hack).
		/// </summary>
		/// <param name="methodToReplace">The method to replace.</param>
		/// <param name="methodToInject">The method to replace with.</param>
		public static void Inject(MethodInfo methodToReplace, MethodInfo methodToInject) {
			RuntimeHelpers.PrepareMethod(methodToReplace.MethodHandle);
			RuntimeHelpers.PrepareMethod(methodToInject.MethodHandle);
			unsafe
			{
				if (IntPtr.Size == 4) {
					int* inj = (int*) methodToInject.MethodHandle.Value.ToPointer() + 2;
					int* tar = (int*) methodToReplace.MethodHandle.Value.ToPointer() + 2;
					if (Diagnostics.Debugger.IsAttached) {
						byte* injInst = (byte*) *inj;
						byte* tarInst = (byte*) *tar;
						int* injSrc = (int*) (injInst + 1);
						int* tarSrc = (int*) (tarInst + 1);
						*tarSrc = (((int) injInst + 5) + *injSrc) - ((int) tarInst + 5);
					} else
						*tar = *inj;
				} else {
					long* inj = (long*) methodToInject.MethodHandle.Value.ToPointer() + 1;
					long* tar = (long*) methodToReplace.MethodHandle.Value.ToPointer() + 1;
					if (Diagnostics.Debugger.IsAttached) {
						byte* injInst = (byte*) *inj;
						byte* tarInst = (byte*) *tar;
						int* injSrc = (int*) (injInst + 1);
						int* tarSrc = (int*) (tarInst + 1);
						*tarSrc = (((int) injInst + 5) + *injSrc) - ((int) tarInst + 5);
					} else
						*tar = *inj;
				}
			}
		}

		/// <summary>
		/// Draws the controls on the given canvas at the specified location relative to the origin (0, 0) of the graphics canvas
		/// </summary>
		/// <param name="g">The graphics canvas to draw onto</param>
		/// <param name="controls">The controls to draw</param>
		/// <param name="location">The location of origin (0, 0) relative to which the controls will be drawn</param>
		/// <param name="drawChildren">Whether to draw the child controls of the specified children as well</param>
		public static void DrawControls(this Graphics g, IEnumerable controls, Point location, bool drawChildren = true) {
			Rectangle clippingRect = Rectangle.Ceiling(g.ClipBounds);
			foreach (Control control in controls) {
				Rectangle ctrlRect = new Rectangle(location + (Size) control.Location, control.Size);
				if ((control.Visible || control is Form) && ctrlRect.IntersectsWith(clippingRect)) {
					IDrawable drawable = control as IDrawable;
					if (drawable == null) {
						/*using (Bitmap bitmap = new Bitmap(control.Width, control.Height, Drawing.Imaging.PixelFormat.Format32bppPArgb)) {
							using (Graphics graphics = Graphics.FromImage(bitmap)) {
								IntPtr hdc = graphics.GetHdc();
								Platforms.Windows.PrintFunctionParameters parameters = Platforms.Windows.PrintFunctionParameters.NONCLIENT | Platforms.Windows.PrintFunctionParameters.CLIENT; //| Platforms.Windows.PrintFunctionParameters.ERASEBKGND;
								if (drawChildren)
									parameters |= Platforms.Windows.PrintFunctionParameters.CHILDREN;
								Message message = new Message() {
									Msg = (int) Platforms.Windows.WindowMessage.PRINT,
									WParam = hdc,
									LParam = new IntPtr((int) parameters),
									HWnd = control.Handle
								};
								control.CallWndProc(ref message);
								IntPtr hdc2 = g.GetHdc();
								Platforms.Windows.NativeApi.BitBlt(hdc2, ctrlRect.X, ctrlRect.Y, ctrlRect.Width, ctrlRect.Height, hdc, 0, 0, Platforms.Windows.TernaryRasterOperations.SRCCOPY);
								g.ReleaseHdcInternal(hdc2);
								graphics.ReleaseHdcInternal(hdc);
							}
						}*/
						float[] matrix = g.Transform.Elements;
						IntPtr hdc = g.GetHdc();
						Platforms.Windows.NativeApi.SetViewportOrgEx(hdc, ctrlRect.X + (int) Math.Round(matrix[4]), ctrlRect.Y + (int) Math.Round(matrix[5]), IntPtr.Zero);
						Platforms.Windows.PrintFunctionParameters parameters = Platforms.Windows.PrintFunctionParameters.NONCLIENT | Platforms.Windows.PrintFunctionParameters.CLIENT; //| Platforms.Windows.PrintFunctionParameters.ERASEBKGND;
						if (drawChildren)
							parameters |= Platforms.Windows.PrintFunctionParameters.CHILDREN;
						Message message = new Message() {
							Msg = (int) Platforms.Windows.WindowMessage.PRINT,
							WParam = hdc,
							LParam = new IntPtr((int) parameters),
							HWnd = control.Handle
						};
						control.CallWndProc(ref message);
						g.ReleaseHdcInternal(hdc);
					} else
						drawable.DrawGdi(g, ctrlRect.Location, drawChildren);
					DrawControls(g, control.Controls, ctrlRect.Location);
				}
			}
		}

		/// <summary>
		/// Translates the specified client coordinates to the corresponding screen location.
		/// </summary>
		/// <param name="ctrl">The control that the given point is respect with.</param>
		/// <param name="clientPoint">The point in client coordinates to translate to screen coordinates.</param>
		public static Point PointToScreen(this Control ctrl, Point clientPoint) {
			if (ctrl == null)
				return clientPoint;
			Form form = ctrl as Form;
			if (form == null) {
				do {
					clientPoint += (Size) ctrl.Location;
					ctrl = ctrl.Parent;
				} while (ctrl != null);
			} else {
				do {
					clientPoint += (Size) form.Location;
					form = form.MdiParent;
				} while (form != null);
			}
			return clientPoint;
		}

		/// <summary>
		/// Translates the specified screen coordinates to the corresponding client coordinate relative to the specified control.
		/// </summary>
		/// <param name="ctrl">The control that the return value rectangle is respect with.</param>
		/// <param name="screenRect">The rectangle on the screen to translate to client coordinates.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Rectangle RectangleToClient(this Control ctrl, Rectangle screenRect) {
			screenRect.Location = PointToClient(ctrl, screenRect.Location);
			return screenRect;
		}

		/// <summary>
		/// Translates the specified client coordinates to the corresponding screen location.
		/// </summary>
		/// <param name="ctrl">The control that the given rectangle is respect with.</param>
		/// <param name="clientRect">The rectangle in client coordinates to translate to screen coordinates.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Rectangle RectangleToScreen(this Control ctrl, Rectangle clientRect) {
			clientRect.Location = PointToScreen(ctrl, clientRect.Location);
			return clientRect;
		}

		/// <summary>
		/// Returns a function that gets the value of the specified field.
		/// </summary>
		/// <typeparam name="S">The type of the containing class.</typeparam>
		/// <typeparam name="T">The type of field value.</typeparam>
		/// <param name="field">The field to wrap with a getter.</param>
		public static Func<S, T> CreateGetter<S, T>(this FieldInfo field) {
			string methodName = field.ReflectedType.FullName + ".get_" + field.Name;
			Type typeOfS = typeof(S);
			DynamicMethod setterMethod = new DynamicMethod(methodName, typeof(T), new Type[] { typeOfS }, typeOfS, true);
			ILGenerator gen = setterMethod.GetILGenerator();
			if (field.IsStatic)
				gen.Emit(OpCodes.Ldsfld, field);
			else {
				gen.Emit(OpCodes.Ldarg_0);
				gen.Emit(OpCodes.Ldfld, field);
			}
			gen.Emit(OpCodes.Ret);
			return (Func<S, T>) setterMethod.CreateDelegate(typeof(Func<S, T>));
		}

		/// <summary>
		/// Returns a function that sets the value of the specified field.
		/// </summary>
		/// <typeparam name="S">The type of the containing class.</typeparam>
		/// <typeparam name="T">The type of field value.</typeparam>
		/// <param name="field">The field to wrap with a setter.</param>
		public static Action<S, T> CreateSetter<S, T>(this FieldInfo field) {
			string methodName = field.ReflectedType.FullName + ".set_" + field.Name;
			Type typeOfS = typeof(S);
			DynamicMethod setterMethod = new DynamicMethod(methodName, null, new Type[] { typeOfS, typeof(T) }, typeOfS, true);
			ILGenerator gen = setterMethod.GetILGenerator();
			if (field.IsStatic) {
				gen.Emit(OpCodes.Ldarg_1);
				gen.Emit(OpCodes.Stsfld, field);
			} else {
				gen.Emit(OpCodes.Ldarg_0);
				gen.Emit(OpCodes.Ldarg_1);
				gen.Emit(OpCodes.Stfld, field);
			}
			gen.Emit(OpCodes.Ret);
			return (Action<S, T>) setterMethod.CreateDelegate(typeof(Action<S, T>));
		}

		/// <summary>
		/// Gets whether the specified key is a modifier key.
		/// </summary>
		/// <param name="key">The key to check if it is a modifier key.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool IsModifier(this Keys key) {
			switch (key) {
				case Keys.None:
				case Keys.Shift:
				case Keys.ShiftKey:
				case Keys.LShiftKey:
				case Keys.RShiftKey:
				case Keys.Alt:
				case Keys.Menu:
				case Keys.Control:
				case Keys.ControlKey:
				case Keys.LControlKey:
				case Keys.RControlKey:
				case Keys.CapsLock:
				case Keys.NumLock:
				case Keys.LWin:
				case Keys.RWin:
					return true;
				default:
					return false;
			}
		}

		/// <summary>
		/// Gets the characters associated with the specified keys (takes keyboard layout into account).
		/// </summary>
		/// <param name="key">The keys that are pressed.</param>
		/// <param name="shift">Whether the shift modifier key is also pressed.</param>
		/// <param name="alt">Whether the alt modifier key is also pressed.</param>
		/// <param name="control">Whether the control modifier key is also pressed.</param>
		public static string ToString(this Keys key, bool shift, bool alt, bool control) {
			StringBuilder buf = new StringBuilder(256);
			unsafe
			{
				byte* keyboardState = stackalloc byte[256];
				if (shift)
					keyboardState[(int) Keys.ShiftKey] = 255;
				if (alt)
					keyboardState[(int) Keys.Menu] = 255;
				if (control)
					keyboardState[(int) Keys.ControlKey] = 255;
				Platforms.Windows.NativeApi.ToUnicode((uint) key, 0, keyboardState, buf, 256, 0);
			}
			return buf.ToString();
		}

		/// <summary>
		/// Gets the characters associated with the specified keys (takes keyboard layout into account).
		/// </summary>
		/// <param name="keys">The keys that are pressed. Modifier keys are assumed to be assumed to be included.</param>
		public static string ToString(this Keys keys) {
			StringBuilder buf = new StringBuilder(256);
			unsafe
			{
				byte* keyboardState = stackalloc byte[256];
				if ((keys & Keys.Shift) == Keys.Shift)
					keyboardState[(int) Keys.ShiftKey] = 255;
				if ((keys & Keys.Alt) == Keys.Alt)
					keyboardState[(int) Keys.Menu] = 255;
				if ((keys & Keys.Control) == Keys.Control)
					keyboardState[(int) Keys.ControlKey] = 255;
				Platforms.Windows.NativeApi.ToUnicode((uint) keys, 0, keyboardState, buf, 256, 0);
			}
			return buf.ToString();
		}

		/// <summary>
		/// Copies the specified number of bytes from the source to the destination memory region.
		/// </summary>
		/// <param name="src">The source to copy from.</param>
		/// <param name="dest">The destination to copy to.</param>
		/// <param name="bytesToCopy">The number of bytes to copy.</param>
		[CLSCompliant(false)]
		public static unsafe void MemoryCopy(byte* src, byte* dest, uint bytesToCopy) {
			if (sizeof(IntPtr) == 8) {
				switch (bytesToCopy) {
					case 0:
						return;
					case 1:
						*dest = *src;
						return;
					case 2:
						*(short*) dest = *(short*) src;
						return;
					case 3:
						*(short*) dest = *(short*) src;
						*(dest + 2) = *(src + 2);
						return;
					case 4:
						*(int*) dest = *(int*) src;
						return;
					case 5:
						*(int*) dest = *(int*) src;
						*(dest + 4) = *(src + 4);
						return;
					case 6:
						*(int*) dest = *(int*) src;
						*(short*) (dest + 4) = *(short*) (src + 4);
						return;
					case 7:
						*(int*) dest = *(int*) src;
						*(short*) (dest + 4) = *(short*) (src + 4);
						*(dest + 6) = *(src + 6);
						return;
					case 8:
						*(long*) dest = *(long*) src;
						return;
					case 9:
						*(long*) dest = *(long*) src;
						*(dest + 8) = *(src + 8);
						return;
					case 10:
						*(long*) dest = *(long*) src;
						*(short*) (dest + 8) = *(short*) (src + 8);
						return;
					case 11:
						*(long*) dest = *(long*) src;
						*(short*) (dest + 8) = *(short*) (src + 8);
						*(dest + 10) = *(src + 10);
						return;
					case 12:
						*(long*) dest = *(long*) src;
						*(int*) (dest + 8) = *(int*) (src + 8);
						return;
					case 13:
						*(long*) dest = *(long*) src;
						*(int*) (dest + 8) = *(int*) (src + 8);
						*(dest + 12) = *(src + 12);
						return;
					case 14:
						*(long*) dest = *(long*) src;
						*(int*) (dest + 8) = *(int*) (src + 8);
						*(short*) (dest + 12) = *(short*) (src + 12);
						return;
					case 15:
						*(long*) dest = *(long*) src;
						*(int*) (dest + 8) = *(int*) (src + 8);
						*(short*) (dest + 12) = *(short*) (src + 12);
						*(dest + 14) = *(src + 14);
						return;
					case 16:
						*(long*) dest = *(long*) src;
						*(long*) (dest + 8) = *(long*) (src + 8);
						return;
					default:
						break;
				}
				if (((int) dest & 3) != 0) {
					if (((int) dest & 1) != 0) {
						*dest = *src;
						src++;
						dest++;
						bytesToCopy--;
						if (((int) dest & 2) == 0)
							goto Aligned;
					}
					*(short*) dest = *(short*) src;
					src += 2;
					dest += 2;
					bytesToCopy -= 2;
					Aligned:
					;
				}

				if (((int) dest & 4) != 0) {
					*(int*) dest = *(int*) src;
					src += 4;
					dest += 4;
					bytesToCopy -= 4;
				}
				uint count = bytesToCopy / 16;
				while (count > 0) {
					((long*) dest)[0] = ((long*) src)[0];
					((long*) dest)[1] = ((long*) src)[1];
					dest += 16;
					src += 16;
					count--;
				}

				if ((bytesToCopy & 8) != 0) {
					((long*) dest)[0] = ((long*) src)[0];
					dest += 8;
					src += 8;
				}
				if ((bytesToCopy & 4) != 0) {
					((int*) dest)[0] = ((int*) src)[0];
					dest += 4;
					src += 4;
				}
				if ((bytesToCopy & 2) != 0) {
					((short*) dest)[0] = ((short*) src)[0];
					dest += 2;
					src += 2;
				}
				if ((bytesToCopy & 1) != 0)
					*dest = *src;

			} else {
				switch (bytesToCopy) {
					case 0:
						return;
					case 1:
						*dest = *src;
						return;
					case 2:
						*(short*) dest = *(short*) src;
						return;
					case 3:
						*(short*) dest = *(short*) src;
						*(dest + 2) = *(src + 2);
						return;
					case 4:
						*(int*) dest = *(int*) src;
						return;
					case 5:
						*(int*) dest = *(int*) src;
						*(dest + 4) = *(src + 4);
						return;
					case 6:
						*(int*) dest = *(int*) src;
						*(short*) (dest + 4) = *(short*) (src + 4);
						return;
					case 7:
						*(int*) dest = *(int*) src;
						*(short*) (dest + 4) = *(short*) (src + 4);
						*(dest + 6) = *(src + 6);
						return;
					case 8:
						*(int*) dest = *(int*) src;
						*(int*) (dest + 4) = *(int*) (src + 4);
						return;
					case 9:
						*(int*) dest = *(int*) src;
						*(int*) (dest + 4) = *(int*) (src + 4);
						*(dest + 8) = *(src + 8);
						return;
					case 10:
						*(int*) dest = *(int*) src;
						*(int*) (dest + 4) = *(int*) (src + 4);
						*(short*) (dest + 8) = *(short*) (src + 8);
						return;
					case 11:
						*(int*) dest = *(int*) src;
						*(int*) (dest + 4) = *(int*) (src + 4);
						*(short*) (dest + 8) = *(short*) (src + 8);
						*(dest + 10) = *(src + 10);
						return;
					case 12:
						*(int*) dest = *(int*) src;
						*(int*) (dest + 4) = *(int*) (src + 4);
						*(int*) (dest + 8) = *(int*) (src + 8);
						return;
					case 13:
						*(int*) dest = *(int*) src;
						*(int*) (dest + 4) = *(int*) (src + 4);
						*(int*) (dest + 8) = *(int*) (src + 8);
						*(dest + 12) = *(src + 12);
						return;
					case 14:
						*(int*) dest = *(int*) src;
						*(int*) (dest + 4) = *(int*) (src + 4);
						*(int*) (dest + 8) = *(int*) (src + 8);
						*(short*) (dest + 12) = *(short*) (src + 12);
						return;
					case 15:
						*(int*) dest = *(int*) src;
						*(int*) (dest + 4) = *(int*) (src + 4);
						*(int*) (dest + 8) = *(int*) (src + 8);
						*(short*) (dest + 12) = *(short*) (src + 12);
						*(dest + 14) = *(src + 14);
						return;
					case 16:
						*(int*) dest = *(int*) src;
						*(int*) (dest + 4) = *(int*) (src + 4);
						*(int*) (dest + 8) = *(int*) (src + 8);
						*(int*) (dest + 12) = *(int*) (src + 12);
						return;
					default:
						break;
				}
				if (((int) dest & 3) != 0) {
					if (((int) dest & 1) != 0) {
						*dest = *src;
						src++;
						dest++;
						bytesToCopy--;
						if (((int) dest & 2) == 0)
							goto Aligned;
					}
					*(short*) dest = *(short*) src;
					src += 2;
					dest += 2;
					bytesToCopy -= 2;
					Aligned:
					;
				}
				uint count = bytesToCopy / 16;
				while (count > 0) {
					((int*) dest)[0] = ((int*) src)[0];
					((int*) dest)[1] = ((int*) src)[1];
					((int*) dest)[2] = ((int*) src)[2];
					((int*) dest)[3] = ((int*) src)[3];
					dest += 16;
					src += 16;
					count--;
				}

				if ((bytesToCopy & 8) != 0) {
					((int*) dest)[0] = ((int*) src)[0];
					((int*) dest)[1] = ((int*) src)[1];
					dest += 8;
					src += 8;
				}
				if ((bytesToCopy & 4) != 0) {
					((int*) dest)[0] = ((int*) src)[0];
					dest += 4;
					src += 4;
				}
				if ((bytesToCopy & 2) != 0) {
					((short*) dest)[0] = ((short*) src)[0];
					dest += 2;
					src += 2;
				}
				if ((bytesToCopy & 1) != 0)
					*dest = *src;
			}
		}

		/// <summary>
		/// Calculates the Levenshtein distance between the two strings.
		/// </summary>
		/// <param name="a">The string to compare.</param>
		/// <param name="b">The string to compare with.</param>
		public static int LevenshteinDistance(this string a, string b) {
			if (string.IsNullOrEmpty(a) || string.IsNullOrEmpty(b))
				return 0;
			int lengthA = a.Length + 1;
			int lengthB = b.Length + 1;
			int[][] distances = new int[lengthA][];
			int i, j;
			for (i = 0; i < lengthA; i++)
				distances[i] = new int[lengthB];
			for (i = 0; i < lengthA; distances[i][0] = i++)
				;
			for (j = 0; j < lengthB; distances[0][j] = j++)
				;
			int cost;
			for (i = 1; i < lengthA; i++) {
				for (j = 1; j < lengthB; j++) {
					cost = b[j - 1] == a[i - 1] ? 0 : 1;
					distances[i][j] = Math.Min(Math.Min(distances[i - 1][j] + 1, distances[i][j - 1] + 1), distances[i - 1][j - 1] + cost);
				}
			}
			return distances[lengthA - 1][lengthB - 1];
		}

		/// <summary>
		/// Initializes a new bitmap from the specified file
		/// </summary>
		/// <param name="path">The path of the bitmap to open.</param>
		public static Bitmap ImageFromFile(string path) {
			using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
				using (Bitmap bitmap = new Bitmap(stream))
					return bitmap.FastCopy();
			}
		}

		/// <summary>
		/// Gets a string that represents the specified float value trimmed and proper.
		/// </summary>
		/// <param name="val">The value to display.</param>
		/// <param name="decimalPlaces">The number of decimal places to keep.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static string ToString(this float val, int decimalPlaces) {
			if (decimalPlaces > 99)
				decimalPlaces = 99;
			return val == 0f ? "0" : val.ToString("F" + decimalPlaces).TrimEnd(Trim);
		}

		/// <summary>
		/// Gets the first non-null element in the specified collection.
		/// The return value is null if 'items' is null or all elements are null.
		/// </summary>
		/// <typeparam name="T">The type of the elements.</typeparam>
		/// <param name="items"></param>
		public static T GetFirstNonNull<T>(this IEnumerable<T> items) where T : class {
			if (items != null) {
				foreach (T item in items) {
					if (item != null)
						return item;
				}
			}
			return null;
		}

		/// <summary>
		/// Returns a Func&lt;object, object&gt; from the given Action.
		/// </summary>
		/// <param name="action">The action to include.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Func<object, object> ToFunc(this Action action) {
			if (action == null)
				return null;
			else
				return delegate (object param) {
					action();
					return null;
				};
		}

		/// <summary>
		/// Returns a Func&lt;object, object&gt; from the given Action&lt;object&gt;.
		/// </summary>
		/// <param name="action">The action to include.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Func<object, object> ToFunc(this Action<object> action) {
			if (action == null)
				return null;
			else {
				return delegate (object param) {
					action(param);
					return null;
				};
			}
		}

		/// <summary>
		/// Checks whether the specified bounds contain the specified point if the area was originated at (0, 0).
		/// </summary>
		/// <param name="bounds">The size of the area to check whether the point is contained inside.</param>
		/// <param name="point">The point to check.</param>
		public static bool Contains(this Size bounds, Point point) {
			return point.X >= 0 && point.X < bounds.Width && point.Y >= 0 && point.Y < bounds.Height;
		}

		/// <summary>
		/// Returns a Func&lt;object, object&gt; from the given Func&lt;object&gt;.
		/// </summary>
		/// <param name="function">The function to include.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Func<object, object> ToFunc(this Func<object> function) {
			if (function == null)
				return null;
			else
				return delegate (object param) {
					return function();
				};
		}

		/// <summary>
		/// Gets whether the string ends with the specified character
		/// </summary>
		/// <param name="str">The string to use.</param>
		/// <param name="lastChar">The character to check.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool EndsWith(this string str, char lastChar) {
			return str.Length != 0 && str[str.Length - 1] == lastChar;
		}

		/// <summary>
		/// Adds the specified elements to the list.
		/// </summary>
		/// <param name="list">The list to add to.</param>
		/// <param name="items">The items to add.</param>
		public static void AddRange(this IList list, IEnumerable items) {
			ArrayList regularList = list as ArrayList;
			if (regularList == null) {
				foreach (object item in items)
					list.Add(item);
			} else
				regularList.AddRange(items);
		}

		/// <summary>
		/// Adds the specified elements to the list.
		/// </summary>
		/// <typeparam name="T">The type of the elements.</typeparam>
		/// <param name="list">The list to add to.</param>
		/// <param name="items">The items to add.</param>
		public static void AddRange<T>(this IList<T> list, IEnumerable<T> items) {
			List<T> regularList = list as List<T>;
			if (regularList == null) {
				foreach (T item in items)
					list.Add(item);
			} else
				regularList.AddRange(items);
		}

#if !NET35
		/// <summary>
		/// Casts an object to the specified type including implicit casts. The generic overload of this method is faster.
		/// </summary>
		/// <param name="value">The value to cast.</param>
		/// <param name="type">The target type of the object.</param>
		public static object CastTo(this object value, Type type) {
			Type typeOfValue = value.GetType();
			if (typeOfValue.Equals(type))
				return value;
			ConcurrentDictionary<Type, Delegate> innerDict;
			Delegate del;
			if (CompiledDelegates.TryGetValue(typeOfValue, out innerDict)) {
				if (innerDict.TryGetValue(type, out del))
					return del.DynamicInvoke(value);
				else {
					ParameterExpression parameter = Expression.Parameter(TypeOfObject, "data");
					del = Expression.Lambda(Expression.Block(Expression.Convert(Expression.Convert(parameter, value.GetType()), type)), parameter).Compile();
					innerDict.TryAdd(type, del);
					return del.DynamicInvoke(value);
				}
			} else {
				ParameterExpression dataParam = Expression.Parameter(TypeOfObject, "data");
				del = Expression.Lambda(Expression.Block(Expression.Convert(Expression.Convert(dataParam, value.GetType()), type)), dataParam).Compile();
				innerDict = new ConcurrentDictionary<Type, Delegate>();
				innerDict.TryAdd(type, del);
				CompiledDelegates.TryAdd(typeOfValue, innerDict);
				return del.DynamicInvoke(value);
			}
		}

		/// <summary>
		/// Casts an object to the specified type including implicit casts.
		/// </summary>
		/// <typeparam name="T">The return type.</typeparam>
		/// <param name="value">The value to cast.</param>
		public static T CastTo<T>(this object value) {
			Type typeOfValue = value.GetType();
			Type typeOfT = typeof(T);
			if (typeOfValue.Equals(typeOfT))
				return (T) value;
			ConcurrentDictionary<Type, Delegate> innerDict;
			if (CompiledDelegates.TryGetValue(typeOfValue, out innerDict)) {
				Delegate del;
				if (innerDict.TryGetValue(typeOfT, out del))
					return ((Func<object, T>) del)(value);
				else {
					ParameterExpression parameter = Expression.Parameter(TypeOfObject, "data");
					Func<object, T> returnFunc = (Func<object, T>) Expression.Lambda(Expression.Block(Expression.Convert(Expression.Convert(parameter, value.GetType()), typeOfT)), parameter).Compile();
					innerDict.TryAdd(typeOfT, returnFunc);
					return returnFunc(value);
				}
			} else {
				ParameterExpression dataParam = Expression.Parameter(TypeOfObject, "data");
				Func<object, T> returnFunc = (Func<object, T>) Expression.Lambda(Expression.Block(Expression.Convert(Expression.Convert(dataParam, value.GetType()), typeOfT)), dataParam).Compile();
				innerDict = new ConcurrentDictionary<Type, Delegate>();
				innerDict.TryAdd(typeOfT, returnFunc);
				CompiledDelegates.TryAdd(typeOfValue, innerDict);
				return returnFunc(value);
			}
		}
#endif

		/// <summary>
		/// Appends StringBuilder content to a StringBuilder without reallocation (hence is faster).
		/// </summary>
		/// <param name="builder">The StringBuilder instance to append to.</param>
		/// <param name="toAppend">The StringBuilder whose content to append.</param>
		/// <returns>The first StringBuilder instance (the one that has been appended to).</returns>
		public static void Append(this StringBuilder builder, StringBuilder toAppend) {
			if (builder == null || toAppend == null)
				return;
			if (builder == toAppend)
				builder.Append(toAppend.ToString());
			else {
				builder.EnsureCapacity(builder.Length + toAppend.Length);
				for (int i = 0; i < toAppend.Length; i++)
					builder.Append(toAppend[i]);
			}
		}

		/// <summary>
		/// Returns the index of the start of the contents in a StringBuilder.
		/// </summary>
		/// <param name="sb">The StringBuilder value to search.</param>
		/// <param name="value">The string to find.</param>
		/// <param name="startIndex">The starting index.</param>
		/// <param name="ignoreCase">Whether to ignore case.</param>
		/// <returns>The index of the specified value, and if not found returns -1.</returns>
		public static int IndexOf(this StringBuilder sb, string value, int startIndex, bool ignoreCase) {
			int length = value.Length;
			int maxSearchLength = (sb.Length - length) + 1;
			int index, i;
			if (ignoreCase) {
				for (i = startIndex; i < maxSearchLength; ++i) {
					if (char.ToLower(sb[i]) == char.ToLower(value[0])) {
						index = 1;
						while ((index < length) && (char.ToLower(sb[i + index]) == char.ToLower(value[index])))
							++index;
						if (index == length)
							return i;
					}
				}
				return -1;
			}
			for (i = startIndex; i < maxSearchLength; ++i) {
				if (sb[i] == value[0]) {
					index = 1;
					while ((index < length) && (sb[i + index] == value[index]))
						++index;
					if (index == length)
						return i;
				}
			}
			return -1;
		}

		/// <summary>
		/// Disposes of the pen, but checks for null and for system pens.
		/// </summary>
		/// <param name="pen">The pen to dispose of.</param>
		public static void DisposeSafe(this Pen pen) {
			if (!(pen == null || (bool) penImmutable.GetValue(pen)))
				pen.Dispose();
		}

		/// <summary>
		/// Disposes of the image, but checks for null and whether it's disposed.
		/// </summary>
		/// <param name="image">The image to dispose of.</param>
		public static void DisposeSafe(this Bitmap image) {
			if (!IsDisposed(image))
				image.Dispose();
		}

		/// <summary>
		/// Disposes of the brush, but checks for null and for system brush.
		/// </summary>
		/// <param name="brush">The brush to dispose of.</param>
		public static void DisposeSafe(this Brush brush) {
			if (brush == null)
				return;
			SolidBrush solid = brush as SolidBrush;
			if (solid == null)
				brush.Dispose();
			else if (!(bool) brushImmutable.GetValue(solid))
				brush.Dispose();
		}

		/// <summary>
		/// Casts the specified 2D array to a jagged array.
		/// </summary>
		/// <typeparam name="T">The type of the array.</typeparam>
		/// <param name="array">The array to convert to jagged.</param>
		public static T[][] ToJaggedArray<T>(this T[,] array) {
			int rowsFirstIndex = array.GetLowerBound(0);
			int numberOfRows = array.GetUpperBound(0) + 1;
			int columnsFirstIndex = array.GetLowerBound(1);
			int numberOfColumns = array.GetUpperBound(1) + 1;
			T[][] jaggedArray = new T[numberOfRows][];
			int j;
			for (int i = rowsFirstIndex; i < numberOfRows; i++) {
				jaggedArray[i] = new T[numberOfColumns];
				for (j = columnsFirstIndex; j < numberOfColumns; j++)
					jaggedArray[i][j] = array[i, j];
			}
			return jaggedArray;
		}

		/// <summary>
		/// Sets all indices of the given array to the specified value. Nulls are tolerated.
		/// </summary>
		/// <param name="array">The array to initialize</param>
		/// <param name="value">The value to initialize it to.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void Initialize<T>(this T[] array, T value) {
			if (array == null)
				return;
			int length = array.Length;
			for (int i = 0; i < length; ++i)
				array[i] = value;
		}

		/// <summary>
		/// Sets all indices of the given array to the specified value. All nulls are tolerated.
		/// </summary>
		/// <param name="array">The array to initialize</param>
		/// <param name="value">The value to initialize it to.</param>
		[CLSCompliant(false)]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void Initialize<T>(this T[][] array, T value) {
			if (array == null)
				return;
			int length = array.Length;
			for (int i = 0; i < length; ++i)
				Initialize(array[i], value);
		}

		/// <summary>
		/// Adds all the values of the array by the specified value.
		/// </summary>
		/// <param name="values">The array of values to add.</param>
		/// <param name="toAdd">The value to add.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void Add(this byte[] values, float toAdd) {
			if (values == null)
				return;
			int length = values.Length;
			for (int i = 0; i < length; ++i)
				values[i] = ImageLib.Clamp(values[i] + toAdd);
		}

		/// <summary>
		/// Adds all the values of the array by the specified value.
		/// </summary>
		/// <param name="values">The array of values to add.</param>
		/// <param name="toAdd">The value to add.</param>
		[CLSCompliant(false)]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void Add(this byte[][] values, float toAdd) {
			if (values == null)
				return;
			int length = values.Length;
			for (int i = 0; i < length; ++i)
				Add(values[i], toAdd);
		}

		/// <summary>
		/// Adds all the values of the array by the specified value.
		/// </summary>
		/// <param name="values">The array of values to add.</param>
		/// <param name="toAdd">The value to add.</param>
		[CLSCompliant(false)]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void Add(this byte[][][] values, float toAdd) {
			if (values == null)
				return;
			int length = values.Length;
			for (int i = 0; i < length; ++i)
				Add(values[i], toAdd);
		}

		/// <summary>
		/// Adds all the values of the array by the specified value.
		/// </summary>
		/// <param name="values">The array of values to add.</param>
		/// <param name="toAdd">The value to add.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void Add(this int[] values, float toAdd) {
			if (values == null)
				return;
			int length = values.Length;
			for (int i = 0; i < length; ++i)
				values[i] = (int) (values[i] + toAdd);
		}

		/// <summary>
		/// Adds all the values of the array by the specified value.
		/// </summary>
		/// <param name="values">The array of values to add.</param>
		/// <param name="toAdd">The value to add.</param>
		[CLSCompliant(false)]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void Add(this int[][] values, float toAdd) {
			if (values == null)
				return;
			int length = values.Length;
			for (int i = 0; i < length; ++i)
				Add(values[i], toAdd);
		}

		/// <summary>
		/// Adds all the values of the array by the specified value.
		/// </summary>
		/// <param name="values">The array of values to add.</param>
		/// <param name="toAdd">The value to add.</param>
		[CLSCompliant(false)]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void Add(this int[][][] values, float toAdd) {
			if (values == null)
				return;
			int length = values.Length;
			for (int i = 0; i < length; ++i)
				Add(values[i], toAdd);
		}

		/// <summary>
		/// Adds all the values of the array by the specified value.
		/// </summary>
		/// <param name="values">The array of values to add.</param>
		/// <param name="toAdd">The value to add.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void Add(this float[] values, float toAdd) {
			if (values == null)
				return;
			int length = values.Length;
			for (int i = 0; i < length; ++i)
				values[i] += toAdd;
		}

		/// <summary>
		/// Adds all the values of the array by the specified value.
		/// </summary>
		/// <param name="values">The array of values to add.</param>
		/// <param name="toAdd">The value to add.</param>
		[CLSCompliant(false)]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void Add(this float[][] values, float toAdd) {
			if (values == null)
				return;
			int length = values.Length;
			for (int i = 0; i < length; ++i)
				Add(values[i], toAdd);
		}

		/// <summary>
		/// Adds all the values of the array by the specified value.
		/// </summary>
		/// <param name="values">The array of values to add.</param>
		/// <param name="toAdd">The value to add.</param>
		[CLSCompliant(false)]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void Add(this float[][][] values, float toAdd) {
			if (values == null)
				return;
			int length = values.Length;
			for (int i = 0; i < length; ++i)
				Add(values[i], toAdd);
		}

		/// <summary>
		/// Multiplies all the values of the array by the specified value.
		/// </summary>
		/// <param name="values">The array of values to multiply.</param>
		/// <param name="multiplier">The multiplier.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void Multiply(this byte[] values, float multiplier) {
			if (values == null)
				return;
			int length = values.Length;
			for (int i = 0; i < length; ++i)
				values[i] = ImageLib.Clamp(values[i] * multiplier);
		}

		/// <summary>
		/// Multiplies all the values of the array by the specified value.
		/// </summary>
		/// <param name="values">The array of values to multiply.</param>
		/// <param name="multiplier">The multiplier.</param>
		[CLSCompliant(false)]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void Multiply(this byte[][] values, float multiplier) {
			if (values == null)
				return;
			int length = values.Length;
			for (int i = 0; i < length; ++i)
				Multiply(values[i], multiplier);
		}

		/// <summary>
		/// Multiplies all the values of the array by the specified value.
		/// </summary>
		/// <param name="values">The array of values to multiply.</param>
		/// <param name="multiplier">The multiplier.</param>
		[CLSCompliant(false)]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void Multiply(this byte[][][] values, float multiplier) {
			if (values == null)
				return;
			int length = values.Length;
			for (int i = 0; i < length; ++i)
				Multiply(values[i], multiplier);
		}

		/// <summary>
		/// Multiplies all the values of the array by the specified value.
		/// </summary>
		/// <param name="values">The array of values to multiply.</param>
		/// <param name="multiplier">The multiplier.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void Multiply(this int[] values, float multiplier) {
			if (values == null)
				return;
			int length = values.Length;
			for (int i = 0; i < length; ++i)
				values[i] = (int) (values[i] * multiplier);
		}

		/// <summary>
		/// Multiplies all the values of the array by the specified value.
		/// </summary>
		/// <param name="values">The array of values to multiply.</param>
		/// <param name="multiplier">The multiplier.</param>
		[CLSCompliant(false)]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void Multiply(this int[][] values, float multiplier) {
			if (values == null)
				return;
			int length = values.Length;
			for (int i = 0; i < length; ++i)
				Multiply(values[i], multiplier);
		}

		/// <summary>
		/// Multiplies all the values of the array by the specified value.
		/// </summary>
		/// <param name="values">The array of values to multiply.</param>
		/// <param name="multiplier">The multiplier.</param>
		[CLSCompliant(false)]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void Multiply(this int[][][] values, float multiplier) {
			if (values == null)
				return;
			int length = values.Length;
			for (int i = 0; i < length; ++i)
				Multiply(values[i], multiplier);
		}

		/// <summary>
		/// Multiplies all the values of the array by the specified value.
		/// </summary>
		/// <param name="values">The array of values to multiply.</param>
		/// <param name="multiplier">The multiplier.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void Multiply(this float[] values, float multiplier) {
			if (values == null)
				return;
			int length = values.Length;
			for (int i = 0; i < length; ++i)
				values[i] *= multiplier;
		}

		/// <summary>
		/// Multiplies all the values of the array by the specified value.
		/// </summary>
		/// <param name="values">The array of values to multiply.</param>
		/// <param name="multiplier">The multiplier.</param>
		[CLSCompliant(false)]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void Multiply(this float[][] values, float multiplier) {
			if (values == null)
				return;
			int length = values.Length;
			for (int i = 0; i < length; ++i)
				Multiply(values[i], multiplier);
		}

		/// <summary>
		/// Multiplies all the values of the array by the specified value.
		/// </summary>
		/// <param name="values">The array of values to multiply.</param>
		/// <param name="multiplier">The multiplier.</param>
		[CLSCompliant(false)]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void Multiply(this float[][][] values, float multiplier) {
			if (values == null)
				return;
			int length = values.Length;
			for (int i = 0; i < length; ++i)
				Multiply(values[i], multiplier);
		}

		/// <summary>
		/// Computes an in-place 1D fourier transform of the specified values.
		/// </summary>
		/// <param name="values">An array of the values to transform.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static ComplexF[] FFT(this ComplexF[] values) {
			return FourierWorker.FFT(values, true);
		}

		/// <summary>
		/// Computes an in-place 1D inverse fourier transform of the specified values.
		/// </summary>
		/// <param name="values">An array of the values to transform.</param>
		/// <param name="truncatePadding">Whether to remove trailing 0s.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static ComplexF[] InverseFFT(this ComplexF[] values, bool truncatePadding = true) {
			return FourierWorker.FFT(values, false, truncatePadding);
		}

		/// <summary>
		/// Performs a fourier shift on the specified values.
		/// </summary>
		/// <param name="values">The values to shift.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static ComplexF[] FFTShift(this ComplexF[] values) {
			return FourierWorker.FFTShift(values, true);
		}

		/// <summary>
		/// Performs an inverse fourier shift on the specified values.
		/// </summary>
		/// <param name="values">The values to shift.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static ComplexF[] InverseFFTShift(this ComplexF[] values) {
			return FourierWorker.FFTShift(values, false);
		}

		/// <summary>
		/// Interpolates the specified values cubically.
		/// </summary>
		/// <param name="values">The values to interpolate</param>
		/// <param name="targetLength">The length of the output array.</param>
		public static ComplexF[] InterpolateCubic(this ComplexF[] values, int targetLength) {
			if (targetLength <= 0)
				return new ComplexF[0];
			else if (values == null)
				return null;
			ComplexF[] resultant = new ComplexF[targetLength];
			int length = values.Length;
			if (length == targetLength)
				Array.Copy(values, resultant, targetLength);
			else if (length != 0) {
				if (length == 1) {
					ComplexF val = values[0];
					for (int i = 0; i < targetLength; i++)
						resultant[i] = val;
				} else {
					float ratio = ((float) length) / targetLength;
					float source;
					int src, x0, x2, x3;
					for (int i = 0; i < targetLength; i++) {
						source = i * ratio;
						src = (int) source;
						x0 = src - 1;
						if (x0 < 0)
							x0 = 0;
						x2 = src + 1;
						x3 = src + 2;
						if (x2 >= length) {
							x2 = src;
							x3 = src;
						} else if (x3 >= length)
							x3 = x2;
						resultant[i] = ImageLib.InterpolateCubic(values[x0], values[src], values[x2], values[x3], source % 1f);
					}
				}
			}
			return resultant;
		}

		/// <summary>
		/// Normalizes the specified set of values in place. WARNING: This assumes only positive values for better performance.
		/// </summary>
		/// <param name="values">The values to normalize.</param>
		/// <param name="max">The maximum to normalize to.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void Normalize(this float[] values, float max = 1f) {
			float currentMax = Max(values);
			if (currentMax == 0f)
				return;
			currentMax = max / currentMax;
			int length = values.Length;
			for (int i = 0; i < length; ++i)
				values[i] *= currentMax;
		}

		/// <summary>
		/// Normalizes the specified set of values in place. WARNING: This assumes only positive values for better performance.
		/// </summary>
		/// <param name="values">The values to normalize.</param>
		/// <param name="max">The maximum to normalize to.</param>
		[CLSCompliant(false)]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void Normalize(this float[][] values, float max = 1f) {
			float currentMax = Max(values);
			if (currentMax == 0f)
				return;
			currentMax = max / currentMax;
			int width = values.Length;
			int height = width == 0 || values[0] == null ? 0 : values[0].Length;
			int y;
			float[] val;
			for (int x = 0; x < width; ++x) {
				val = values[x];
				for (y = 0; y < height; ++y)
					val[y] *= currentMax;
			}
		}

		/// <summary>
		/// Gets the maximum value.
		/// </summary>
		/// <param name="values">An array of values.</param>
		/// <param name="initialValue">The initial value to start with.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static byte Max(this byte[] values, byte initialValue = 0) {
			byte current;
			int length = values.Length;
			for (int i = 0; i < length; ++i) {
				current = values[i];
				if (current > initialValue)
					initialValue = current;
			}
			return initialValue;
		}

		/// <summary>
		/// Gets the minimum value.
		/// </summary>
		/// <param name="values">An array of values.</param>
		/// <param name="initialValue">The initial value to start with.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static byte Min(this byte[] values, byte initialValue = 255) {
			byte current;
			int length = values.Length;
			for (int i = 0; i < length; ++i) {
				current = values[i];
				if (current < initialValue)
					initialValue = current;
			}
			return initialValue;
		}

		/// <summary>
		/// Gets the maximum value.
		/// </summary>
		/// <param name="values">An array of values.</param>
		/// <param name="initialValue">The initial value to start with.</param>
		[CLSCompliant(false)]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static byte Max(this byte[][] values, byte initialValue = 0) {
			int length = values.Length;
			for (int i = 0; i < length; ++i)
				initialValue = Max(values[i], initialValue);
			return initialValue;
		}

		/// <summary>
		/// Gets the minimum value.
		/// </summary>
		/// <param name="values">An array of values.</param>
		/// <param name="initialValue">The initial value to start with.</param>
		[CLSCompliant(false)]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static byte Min(this byte[][] values, byte initialValue = 255) {
			int length = values.Length;
			for (int i = 0; i < length; ++i)
				initialValue = Min(values[i], initialValue);
			return initialValue;
		}

		/// <summary>
		/// Gets the maximum value.
		/// </summary>
		/// <param name="values">An array of values.</param>
		/// <param name="initialValue">The initial value to start with.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static int Max(this int[] values, int initialValue = int.MinValue) {
			int current;
			int length = values.Length;
			for (int i = 0; i < length; ++i) {
				current = values[i];
				if (current > initialValue)
					initialValue = current;
			}
			return initialValue;
		}

		/// <summary>
		/// Gets the minimum value.
		/// </summary>
		/// <param name="values">An array of values.</param>
		/// <param name="initialValue">The initial value to start with.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static int Min(this int[] values, int initialValue = int.MaxValue) {
			int current;
			int length = values.Length;
			for (int i = 0; i < length; ++i) {
				current = values[i];
				if (current < initialValue)
					initialValue = current;
			}
			return initialValue;
		}

		/// <summary>
		/// Gets the maximum value.
		/// </summary>
		/// <param name="values">An array of values.</param>
		/// <param name="initialValue">The initial value to start with.</param>
		[CLSCompliant(false)]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static int Max(this int[][] values, int initialValue = int.MinValue) {
			int length = values.Length;
			for (int i = 0; i < length; ++i)
				initialValue = Max(values[i], initialValue);
			return initialValue;
		}

		/// <summary>
		/// Gets the minimum value.
		/// </summary>
		/// <param name="values">An array of values.</param>
		/// <param name="initialValue">The initial value to start with.</param>
		[CLSCompliant(false)]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static int Min(this int[][] values, int initialValue = int.MaxValue) {
			int length = values.Length;
			for (int i = 0; i < length; ++i)
				initialValue = Min(values[i], initialValue);
			return initialValue;
		}

		/// <summary>
		/// Gets the maximum value.
		/// </summary>
		/// <param name="values">An array of values.</param>
		/// <param name="initialValue">The initial value to start with.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static float Max(this float[] values, float initialValue = float.NegativeInfinity) {
			float current;
			int length = values.Length;
			for (int i = 0; i < length; ++i) {
				current = values[i];
				if (current > initialValue)
					initialValue = current;
			}
			return initialValue;
		}

		/// <summary>
		/// Gets the minimum value.
		/// </summary>
		/// <param name="values">An array of values.</param>
		/// <param name="initialValue">The initial value to start with.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static float Min(this float[] values, float initialValue = float.PositiveInfinity) {
			float current;
			int length = values.Length;
			for (int i = 0; i < length; ++i) {
				current = values[i];
				if (current < initialValue)
					initialValue = current;
			}
			return initialValue;
		}

		/// <summary>
		/// Gets the maximum value.
		/// </summary>
		/// <param name="values">An array of values.</param>
		/// <param name="initialValue">The initial value to start with.</param>
		[CLSCompliant(false)]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static float Max(this float[][] values, float initialValue = float.NegativeInfinity) {
			int length = values.Length;
			for (int i = 0; i < length; ++i)
				initialValue = Max(values[i], initialValue);
			return initialValue;
		}

		/// <summary>
		/// Gets the minimum value.
		/// </summary>
		/// <param name="values">An array of values.</param>
		/// <param name="initialValue">The initial value to start with.</param>
		[CLSCompliant(false)]
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static float Min(this float[][] values, float initialValue = float.PositiveInfinity) {
			int length = values.Length;
			for (int i = 0; i < length; ++i)
				initialValue = Min(values[i], initialValue);
			return initialValue;
		}

		/// <summary>
		/// Reverses the string.
		/// </summary>
		/// <param name="str">The string to reverse.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static string Reverse(this string str) {
			char[] arr = str.ToCharArray();
			Array.Reverse(arr);
			return new string(arr);
		}

		/// <summary>
		/// Reverses the specified comparer. If the comparer is already reversed, it un-reverses it.
		/// </summary>
		/// <typeparam name="T">The type of items the comparer supports.</typeparam>
		/// <param name="comparer">The comparer to reverse.</param>
		public static IComparer<T> Reverse<T>(this IComparer<T> comparer) {
			ReverseComparer<T> reversed = comparer as ReverseComparer<T>;
			if (reversed == null)
				return new ReverseComparer<T>(comparer);
			else
				return reversed.OriginalComparer;
		}

		/// <summary>
		/// Gets a pixel byte array from the specified float values [x][y].
		/// </summary>
		/// <param name="values">The values to load the image from.</param>
		/// <param name="normalize">Whether to normalize the values to byte range. Negative values are truncated.</param>
		public static byte[] ToPixels(this float[][] values, bool normalize) {
			if (values == null)
				return null;
			int width = values.Length;
			int height = width == 0 || values[0] == null ? 0 : values[0].Length;
			byte[] resultant = new byte[width * height];
			if (normalize) {
				float max = values.Max(0f);
				float min = values.Min();
				if (max > min)
					return resultant;
				else if (max == min) {
					Memset(resultant, ImageLib.Clamp(min), 0, resultant.Length);
					return resultant;
				} else
					max = 255f / (max - min);
				ParallelLoop.For(0, width, x => {
					float[] v = values[x];
					for (int y = 0; y < height; y++)
						resultant[y * width + x] = ImageLib.Clamp((v[y] - min) * max);
				}, ImageLib.ParallelCutoff);
			} else {
				ParallelLoop.For(0, width, x => {
					float[] v = values[x];
					for (int y = 0; y < height; y++)
						resultant[y * width + x] = ImageLib.Clamp(v[y]);
				}, ImageLib.ParallelCutoff);
			}
			return resultant;
		}

		/// <summary>
		/// Gets whether the specified image is disposed (returns true if null).
		/// </summary>
		/// <param name="image">The image to check if disposed.</param>
		public static bool IsDisposed(this Image image) {
			return image == null || ((IntPtr) nativeImageField.GetValue(image)) == IntPtr.Zero;
		}

		/// <summary>
		/// Transforms a normalized Plane by a Matrix.
		/// </summary>
		/// <param name="plane"> The normalized Plane to transform. 
		/// This Plane must already be normalized, so that its Normal vector is of unit length, before this method is called.</param>
		/// <param name="matrix">The transformation matrix to apply to the Plane.</param>
		/// <returns>The transformed Plane.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Plane Transform(this Plane plane, ref Matrix4 matrix) {
			Matrix4.Invert(ref matrix, out matrix);
			Vector4 vector = new Vector4(plane.Normal, plane.D);
			return new Plane(vector * matrix.Row0 + vector * matrix.Row1 + vector * matrix.Row2 + vector * matrix.Row3);
		}

		/// <summary>
		/// Convert this instance to an axis-angle representation.
		/// </summary>
		/// <returns>A Quaternion that is the axis-angle representation of this quaternion.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Quaternion ToAxisAngle(this Quaternion quaternion) {
			if (Math.Abs(quaternion.W) > 1f)
				quaternion = Quaternion.Normalize(quaternion);
			Quaternion result = new Quaternion();
			result.W = 2f * (float) Math.Acos(quaternion.W);
			float den = (float) Math.Sqrt(1.0 - quaternion.W * quaternion.W);
			if (den > 0.0001f) {
				den = 1f / den;
				result.X = quaternion.X * den;
				result.Y = quaternion.Y * den;
				result.Z = quaternion.Z * den;
			} else
				result.X = 1f;
			return result;
		}

		/// <summary>
		/// Transforms a vector normal by the given matrix.
		/// </summary>
		/// <param name="normal">The source vector.</param>
		/// <param name="matrix">The transformation matrix.</param>
		/// <returns>The transformed vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector3 TransformNormal(this Vector3 normal, ref Matrix4 matrix) {
			Vector4 vec = new Vector4(normal, 0f);
			return (vec * matrix.Row0 + vec * matrix.Row1 + vec * matrix.Row2 + vec * matrix.Row3).ToVector3();
		}

		/// <summary>
		/// Dot(Cross(v1, v2), v3)
		/// </summary>
		/// <param name="v1">V1</param>
		/// <param name="v2">V2</param>
		/// <param name="v3">V3</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static float TripleProduct(this Vector3 v1, Vector3 v2, Vector3 v3) {
			return Vector3.Dot(Vector3.Cross(v1, v2), v3);
		}

		/// <summary>Transform a Position by the given Matrix</summary>
		/// <param name="pos">The position to transform</param>
		/// <param name="mat">The desired transformation</param>
		/// <returns>The transformed position</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector3 TransformPosition(this Vector3 pos, ref Matrix4 mat) {
			return new Vector3(Vector3.Dot(pos, mat.Column0.ToVector3()) + mat.Row3.X, Vector3.Dot(pos, mat.Column1.ToVector3()) + mat.Row3.Y, Vector3.Dot(pos, mat.Column2.ToVector3()) + mat.Row3.Z);
		}

		/// <summary>
		/// Transforms a vector by the given matrix.
		/// </summary>
		/// <param name="position">The source vector.</param>
		/// <param name="matrix">The transformation matrix.</param>
		/// <returns>The transformed vector.</returns>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector3 Transform(this Vector3 position, ref Matrix4 matrix) {
			return new Vector3(position.X * matrix.M11 + position.Y * matrix.M21 + position.Z * matrix.M31 + matrix.M41, position.X * matrix.M12 + position.Y * matrix.M22 + position.Z * matrix.M32 + matrix.M42, position.X * matrix.M13 + position.Y * matrix.M23 + position.Z * matrix.M33 + matrix.M43);
		}

		/// <summary>
		/// Places the point about the specified axis at the specified angle.
		/// </summary>
		/// <param name="vector">The vector to set the angle with respect to it.</param>
		/// <param name="axis">The center point to use as origin.</param>
		/// <param name="angle">The angle to set in relation to the vector (absolute).</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector2 SetAngleFrom(this Vector2 vector, Vector2 axis, float angle) {
			return axis + ToMoveAt(Vector2.Distance(vector, axis), angle);
		}

		/// <summary>
		/// Rotates the point about the specified axis at the specified angle.
		/// </summary>
		/// <param name="vector">The vector to rotate.</param>
		/// <param name="axis">The center point of rotation.</param>
		/// <param name="angle">The cumulative angle of rotation.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector2 RotateAbout(this Vector2 vector, Vector2 axis, float angle) {
			return axis + ToMoveAt(Vector2.Distance(vector, axis), axis.GetAngleFrom(vector) + angle);
		}

		/// <summary>
		/// Gets the current angle from another point in Radians.
		/// </summary>
		/// <param name="v">The vector.</param>
		/// <param name="axis">The axis vector to get rotation from (consider it as origin).</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static float GetAngleFrom(this Vector2 v, Vector2 axis) {
			v -= axis;
			return (float) Math.Atan2(v.Y, v.X);
		}

		/// <summary>
		/// Moves the point at the specified angle (measured in Radians) at the specified distance.
		/// </summary>
		/// <param name="vector">The point to move.</param>
		/// <param name="distance">The distance to move.</param>
		/// <param name="angle">The angle to move.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector2 MoveAt(this Vector2 vector, float distance, float angle) {
			return vector + ToMoveAt(distance, angle);
		}

		/// <summary>
		/// Returns the movement required for the point to move at the specified distance and angle.
		/// Polar to Eucledean coordinates.
		/// </summary>
		/// <param name="distance">The distance to move at.</param>
		/// <param name="angle">The angle to move at</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector2 ToMoveAt(float distance, float angle) {
			return new Vector2((float) Math.Cos(angle), (float) Math.Sin(angle)) * distance;
		}

		/// <summary>
		/// Moves the point towards the specified point for the specified distance.
		/// </summary>
		/// <param name="vector">The vector to move.</param>
		/// <param name="point">The point to move towards.</param>
		/// <param name="distance">The distance to move.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector2 MoveTowards(this Vector2 vector, Vector2 point, float distance) {
			return vector.MoveAt(distance, vector.GetAngleFrom(point));
		}

		/// <summary>
		/// Returns the movement required to move the point the specified distance towards the specified point.
		/// </summary>
		/// <param name="vector">The vector to move.</param>
		/// <param name="point">The point to move towards.</param>
		/// <param name="distance">The distance to move at.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector2 DirectionTowards(this Vector2 vector, Vector2 point, float distance) {
			return ToMoveAt(distance, vector.GetAngleFrom(point));
		}

		/// <summary>
		/// Moves the point away from the specified point at the specified distance.
		/// </summary>
		/// <param name="vector">The vector to move.</param>
		/// <param name="point">The point to move away from.</param>
		/// <param name="distance">The distance to move.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector2 MoveAwayFrom(this Vector2 vector, Vector2 point, float distance) {
			return vector.MoveAt(distance, point.GetAngleFrom(vector));
		}

		/// <summary>
		/// Returns the movement required to move the point the specified distance away from the specified point.
		/// </summary>
		/// <param name="vector">The vector to move.</param>
		/// <param name="point">The point to move towards.</param>
		/// <param name="distance">The distance to move at.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector2 DirectionAwayFrom(this Vector2 vector, Vector2 point, float distance) {
			return ToMoveAt(distance, point.GetAngleFrom(vector));
		}

		/// <summary>
		/// Gets the relative point on the circumference of an ellipse that is located at the specified angle from the center.
		/// </summary>
		/// <param name="radii">The radii of the ellipse (width and height).</param>
		/// <param name="ellipseRotation">The angle of rotation of the ellipse itself.</param>
		/// <param name="angle">The angle the point to obtain on the circumference makes with the center.</param>
		public static Vector2 GetVectorOnEllipseFromAngle(Vector2 radii, float ellipseRotation, float angle) {
			if (Math.Abs(radii.X - radii.Y) <= float.Epsilon)
				return SetAngleFrom(radii, Vector2.Zero, angle);
			else {
				angle = angle % Maths.TwoPiF;
				if (angle >= Maths.PiF)
					angle -= Maths.TwoPiF;
				double t = Math.Atan(radii.X * Math.Tan(angle) / radii.Y);
				if (angle >= Maths.PiOver2F && angle < Maths.PiF)
					t += Math.PI;
				else if (angle >= -Maths.PiF && angle < -Maths.PiOver2F)
					t -= Math.PI;
				Vector2 resultant = radii * new Vector2((float) Math.Cos(t), (float) Math.Sin(t));
				return ellipseRotation % Maths.TwoPiF == 0f ? resultant : RotateAbout(resultant, Vector2.Zero, ellipseRotation);
			}
		}

		/// <summary>
		/// Gets the point on the circumference of an ellipse that is located at the specified angle from the center.
		/// </summary>
		/// <param name="center">The center point of the ellipse.</param>
		/// <param name="radii">The radii of the ellipse (width and height).</param>
		/// <param name="ellipseRotation">The angle of rotation of the ellipse itself.</param>
		/// <param name="angle">The angle the point to obtain on the circumference makes with the center.</param>
		public static Vector2 GetPointOnEllipseFromAngle(this Vector2 center, Vector2 radii, float ellipseRotation, float angle) {
			return center + GetVectorOnEllipseFromAngle(radii, ellipseRotation, angle);
		}

		/// <summary>
		/// Gets the perpendicular vector on the right side of this vector.
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector2 GetPerpendicularRight(this Vector2 vec) {
			return new Vector2(vec.Y, -vec.X);
		}

		/// <summary>
		/// Gets the perpendicular vector on the left side of this vector.
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector2 GetPerpendicularLeft(this Vector2 vec) {
			return new Vector2(-vec.Y, vec.X);
		}

		/// <summary>
		/// Gets a Vector2 like this one but with X and Y swapped.
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector2 SwapComponents(this Vector2 vec) {
			return new Vector2(vec.Y, vec.X);
		}

		/// <summary>
		/// Casts the Vector4 to Vector3 by discarding the W component.
		/// </summary>
		/// <param name="vec">The source vector.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector3 ToVector3(this Vector4 vec) {
			return new Vector3(vec.X, vec.Y, vec.Z);
		}

		/// <summary>
		/// Casts the Vector4 to Vector2 by discarding the Z and W component.
		/// </summary>
		/// <param name="vec">The source vector.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector2 ToVector2(this Vector4 vec) {
			return new Vector2(vec.X, vec.Y);
		}

		/// <summary>
		/// Casts the Vector3 to Vector2 by discarding the Z component.
		/// </summary>
		/// <param name="vec">The source vector.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector2 ToVector2(this Vector3 vec) {
			return new Vector2(vec.X, vec.Y);
		}

		/// <summary>
		/// Casts a Vector2 to PointF
		/// </summary>
		/// <param name="v">The vector to cast</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static PointF ToPointF(this Vector2 v) {
			return new PointF(v.X, v.Y);
		}

		/// <summary>
		/// Casts a PointF to Vector2
		/// </summary>
		/// <param name="v">The vector to cast</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector2 ToVector2(this PointF v) {
			return new Vector2(v.X, v.Y);
		}

		/// <summary>
		/// Casts a Vector2 to Point
		/// </summary>
		/// <param name="v">The vector to cast</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Point ToPoint(this Vector2 v) {
			return new Point((int) v.X, (int) v.Y);
		}

		/// <summary>
		/// Casts a Point to Vector2
		/// </summary>
		/// <param name="v">The point to cast</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector2 ToVector2(this Point v) {
			return new Vector2(v.X, v.Y);
		}

		/// <summary>
		/// Casts a Vector3 to PointF
		/// </summary>
		/// <param name="v">The vector to cast</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static PointF ToPointF(this Vector3 v) {
			return new PointF(v.X, v.Y);
		}

		/// <summary>
		/// Casts a PointF to Vector3
		/// </summary>
		/// <param name="v">The vector to cast</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector3 ToVector3(this PointF v) {
			return new Vector3(v.X, v.Y, 0f);
		}

		/// <summary>
		/// Casts a Vector3 to Point
		/// </summary>
		/// <param name="v">The vector to cast</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Point ToPoint(this Vector3 v) {
			return new Point((int) v.X, (int) v.Y);
		}

		/// <summary>
		/// Casts a Point to Vector3
		/// </summary>
		/// <param name="v">The point to cast</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector3 ToVector3(this Point v) {
			return new Vector3(v.X, v.Y, 0f);
		}

		/// <summary>
		/// Casts a Vector2 to SizeF
		/// </summary>
		/// <param name="v">The vector to cast</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static SizeF ToSizeF(this Vector2 v) {
			return new SizeF(v.X, v.Y);
		}

		/// <summary>
		/// Casts a SizeF to Vector2
		/// </summary>
		/// <param name="v">The size to cast</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector2 ToVector2(this SizeF v) {
			return new Vector2(v.Width, v.Height);
		}

		/// <summary>
		/// Casts a Vector2 to Size
		/// </summary>
		/// <param name="v">The vector to cast</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Size ToSize(this Vector2 v) {
			return new Size((int) v.X, (int) v.Y);
		}

		/// <summary>
		/// Casts a Size to Vector2
		/// </summary>
		/// <param name="v">The size to cast</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector2 ToVector2(this Size v) {
			return new Vector2(v.Width, v.Height);
		}

		/// <summary>
		/// Casts a Vector3 to SizeF
		/// </summary>
		/// <param name="v">The vector to cast</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static SizeF ToSizeF(this Vector3 v) {
			return new SizeF(v.X, v.Y);
		}

		/// <summary>
		/// Casts a SizeF to Vector3
		/// </summary>
		/// <param name="v">The size to cast</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector3 ToVector3(this SizeF v) {
			return new Vector3(v.Width, v.Height, 0f);
		}

		/// <summary>
		/// Casts a Vector3 to Size
		/// </summary>
		/// <param name="v">The vector to cast</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Size ToSize(this Vector3 v) {
			return new Size((int) v.X, (int) v.Y);
		}

		/// <summary>
		/// Casts a Size to Vector3
		/// </summary>
		/// <param name="v">The size to cast</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static Vector3 ToVector3(this Size v) {
			return new Vector3(v.Width, v.Height, 0f);
		}

		/// <summary>
		/// Gets the biggest from X, Y, and Z.
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static float GetMaxComponent(this Vector3 vector) {
			float ret = Math.Max(vector.X, vector.Y);
			return Math.Max(ret, vector.Z);
		}

		/// <summary>
		/// Gets the smallest from X, Y, and Z.
		/// </summary>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static float GetMinComponent(this Vector3 vector) {
			float ret = Math.Min(vector.X, vector.Y);
			return Math.Min(ret, vector.Z);
		}

		/*public static float Max(this float[] array) {
			float max = array[0];
			int step = Vector<float>.Count;
			int i;
			if (array.Length <= step) {
				for (i = 1; i < array.Length; i++) {
					if (max < array[i])
						max = array[i];
				}
				return max;
			} else {
				Vector<float> maxVector = new Vector<float>(max);
				i = 0;
				int stopAt = array.Length - array.Length % step;
				for (i = 0; i < stopAt; i += step)
					maxVector = Vector.Max(new Vector<float>(array, i), maxVector);
				for (i = 0; i < step; i++) {
					if (max < maxVector[i])
						max = maxVector[i];
				}
				for (; stopAt < array.Length; stopAt++) {
					if (max < array[stopAt])
						max = array[stopAt];
				}
				return max;
			}
		}*/
	}
}