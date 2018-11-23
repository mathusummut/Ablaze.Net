using System.Collections.Generic;
using System.Platforms.Windows;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Security.Cryptography;

namespace System.IO {
	/// <summary>
	/// Enumerates the what a path can represent.
	/// </summary>
	public enum FileOrFolder {
		/// <summary>
		/// The file or folder was not found. The path is invalid.
		/// </summary>
		NotFound,
		/// <summary>
		/// The path represents a file.
		/// </summary>
		File,
		/// <summary>
		/// The path represents a directory
		/// </summary>
		Folder
	}

	/// <summary>
	/// A collection of tools for managing and loading files.
	/// </summary>
	public static class FileUtils {
		/// <summary>
		/// The directories to add to relative path searching.
		/// </summary>
		public static readonly SyncedList<string> Directories = new SyncedList<string>();
		private static char[] PathSeparator = new char[] { Path.DirectorySeparatorChar };
		private static char[] PathSeparators = new char[] { '/', '\\' };
		private static HashSet<char> PathSeparatorSet = new HashSet<char>(PathSeparators);
		private static char[] DriveMarker = new char[] { ':' };
		private static string parentDir = ".." + Path.DirectorySeparatorChar;
		private static MD5 md5Calculator = MD5.Create();

		/// <summary>
		/// Returns the MD5 checksum of the specified file in hexadecimal. The letters are uppercase.
		/// </summary>
		/// <param name="stream">The stream whose checksum to calculate</param>
		public static string CalculateMD5(Stream stream) {
			using (BufferedStream buffer = new BufferedStream(stream, 1200000))
				return BitConverter.ToString(md5Calculator.ComputeHash(buffer)).Replace("-", string.Empty);
		}

		/// <summary>
		/// Recursively calculates the directory size in bytes
		/// </summary>
		/// <param name="dir">The directory whose size to calculate.</param>
		[CLSCompliant(false)]
		public static ulong CalcutateDirectorySize(DirectoryInfo dir) {
			ulong size = 0ul;
			foreach (FileInfo file in dir.GetFiles()) //add file sizes
				size += (ulong) file.Length;
			foreach (DirectoryInfo d in dir.GetDirectories()) //add subdirectory sizes
				size += CalcutateDirectorySize(d);
			return size;
		}

		/// <summary>
		/// Deletes the specified file if it exists
		/// </summary>
		/// <param name="path">The path to the file to delete</param>
		/// <param name="timeout">The approximate timeout in milliseconds</param>
		public static void DeleteFileIfExists(string path, int timeout = int.MaxValue) {
			if (File.Exists(path)) {
				File.Delete(path);
				int counter = 0;
				while (File.Exists(path)) {
					Thread.Sleep(50);
					counter += 50;
					if (counter >= timeout)
						break;
				}
			}
		}

		private static void DeleteFolderInner(string path) {
			foreach (string directory in Directory.GetDirectories(path))
				DeleteFolderInner(directory);
			try {
				Directory.Delete(path, true);
			} catch (DirectoryNotFoundException) {
			} catch (IOException) { //quantum weirdness
				Thread.Sleep(50);
				Directory.Delete(path, true);
				Thread.Sleep(50);
			} catch (UnauthorizedAccessException) { //quantum weirdness
				Thread.Sleep(50);
				Directory.Delete(path, true);
				Thread.Sleep(50);
			}
		}

		/// <summary>
		/// Deletes the specified folder if it exists
		/// </summary>
		/// <param name="path">The path to the folder to delete</param>
		/// <param name="timeout">The approximate timeout in milliseconds</param>
		public static void DeleteFolderIfExists(string path, int timeout = int.MaxValue) {
			if (Directory.Exists(path)) {
				DeleteFolderInner(path);
				int counter = 0;
				while (Directory.Exists(path)) {
					Thread.Sleep(50);
					counter += 50;
					if (counter >= timeout)
						break;
				}
			}
		}

		/// <summary>
		/// Returns the file name of the specified path string without the extension.
		/// </summary>
		/// <param name="path">The path of the file.</param>
		public static string GetFileNameWithoutExtension(this string path) {
			int dotIndex = -1;
			char chr;
			for (int i = path.Length - 1; i >= 0; i--) {
				chr = path[i];
				if (chr == '/' || chr == '\\')
					return path;
				else if (chr == '.') {
					dotIndex = i;
					break;
				}
			}
			if (dotIndex == -1)
				return path;
			int startOfFileName = 0;
			for (int i = dotIndex - 1; i >= 0; i--) {
				chr = path[i];
				if (chr == '/' || chr == '\\') {
					startOfFileName = i + 1;
					break;
				}
			}
			return path.Substring(startOfFileName, dotIndex - startOfFileName);
		}

		/// <summary>
		/// Loads the specified file and returns null if the file could not be loaded. If the file is currently locked/in use,
		/// the function will retry loading until the specified timeout expires.
		/// </summary>
		/// <param name="path">>A path to the file (can be relative or absolute).</param>
		/// <param name="mode">The file mode to use.</param>
		/// <param name="accessLevel">The access right level requested by this instance.</param>
		/// <param name="permissionsForOtherStreams">What to allow other filestreams to do while this stream is open.</param>
		/// <param name="timeout">The retry timeout in milliseconds if a file is in use (or 0 to only try once, -1 to retry indefinitely).</param>
		public static FileStream LoadFileTimeout(string path, FileMode mode, FileAccess accessLevel, FileShare permissionsForOtherStreams, int timeout = 5000) {
			int cumulative = 0;
			FileStream fs = null;
			do {
				try {
					fs = LoadFile(path, mode, accessLevel, permissionsForOtherStreams, true);
					return fs;
				} catch (IOException) {
					if (fs != null)
						fs.Dispose();
					Thread.Sleep(50);
				} catch {
					return null;
				}
				cumulative += 50;
			} while (timeout == -1 || cumulative < timeout);
			return null;
		}

		/// <summary>
		/// Loads the specified file using a stream.
		/// </summary>
		/// <param name="path">A path to the file (can be relative or absolute).</param>
		/// <param name="mode">The file mode to use.</param>
		/// <param name="accessLevel">The access right level requested by this instance.</param>
		/// <param name="permissionsForOtherStreams">What to allow other filestreams to do while this stream is open.</param>
		/// <param name="throwOnError">Whether to throw an exception when the file is not found.</param>
		public static FileStream LoadFile(string path, FileMode mode, FileAccess accessLevel, FileShare permissionsForOtherStreams, bool throwOnError) {
			FileStream stream = LoadFile(path, mode, accessLevel, permissionsForOtherStreams);
			if (stream == null && throwOnError)
				throw new FileNotFoundException("The file " + path + " was not found.", path);
			else
				return stream;
		}

		/// <summary>
		/// Loads the specified file using a stream. Returns null if not found.
		/// </summary>
		/// <param name="path">A path to the file (can be relative or absolute).</param>
		/// <param name="mode">The file mode to use.</param>
		/// <param name="accessLevel">The access right level requested by this instance.</param>
		/// <param name="permissionsForOtherStreams">What to allow other filestreams to do while this stream is open.</param>
		public static FileStream LoadFile(string path, FileMode mode, FileAccess accessLevel, FileShare permissionsForOtherStreams) {
			if (path == null)
				return null;
			FileOrFolder exists;
			string absolutePath = ResolvePath(path, true, out exists);
			if (exists != FileOrFolder.File)
				return null;
			else if (absolutePath != null)
				return new FileStream(absolutePath, mode, accessLevel, permissionsForOtherStreams);
			else if (mode == FileMode.Create || mode == FileMode.CreateNew || mode == FileMode.OpenOrCreate)
				return new FileStream(path, mode, accessLevel, permissionsForOtherStreams);
			else
				return null;
		}

		/// <summary>
		/// Loads the specified file as a string. Returns null if the file is not found.
		/// </summary>
		/// <param name="path">A path to the file (can be relative or absolute).</param>
		/// <param name="encoding">The encoding to use (null is UTF-8).</param>
		public static string ReadAllText(string path, Encoding encoding = null) {
			if (path == null)
				return null;
			FileOrFolder exists;
			string absolutePath = ResolvePath(path, true, out exists);
			if (exists != FileOrFolder.File)
				return null;
			else if (encoding == null)
				encoding = Encoding.UTF8;
			return File.ReadAllText(absolutePath, encoding);
		}

		/// <summary>
		/// Returns whether the specified folder exists.
		/// </summary>
		/// <param name="path">The path to the directory.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool FolderExists(string path) {
			try {
				return Directory.Exists(path);
			} catch {
				return false;
			}
		}

		/// <summary>
		/// Returns whether the specified file exists.
		/// </summary>
		/// <param name="path">The path to the file.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static bool FileExists(string path) {
			try {
				return File.Exists(path);
			} catch {
				return false;
			}
		}

		/// <summary>
		/// Returns whether the specified file or folder exists.
		/// </summary>
		/// <param name="path">The path to the file or folder.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static FileOrFolder Exists(string path) {
			return FileExists(path) ? FileOrFolder.File : (FolderExists(path) ? FileOrFolder.Folder : FileOrFolder.NotFound);
		}

		/// <summary>
		/// Loads the specified file using a buffered stream.
		/// </summary>
		/// <param name="path">A path to the file (can be relative or absolute).</param>
		/// <param name="mode">The file mode to use.</param>
		/// <param name="throwOnError">Whether to throw an exception when the file is not found.</param>
		/// <param name="accessLevel">The access right level requested by this instance.</param>
		/// <param name="permissionsForOtherStreams">What to allow other filestreams to do while this stream is open.</param>
		public static BufferedStream LoadFileBuffered(string path, FileMode mode, FileAccess accessLevel, FileShare permissionsForOtherStreams, bool throwOnError = false) {
			FileStream stream = LoadFile(path, mode, accessLevel, permissionsForOtherStreams);
			if (stream == null && throwOnError)
				throw new FileNotFoundException("The file " + path + " was not found.", path);
			else
				return stream == null ? null : new BufferedStream(stream);
		}

		/// <summary>
		/// Converts an absolute path to path relative to a specified directory which will be used as root.
		/// No directory separator is appended at the end of the returned string, and the directories do not need to exist.
		/// If an invalid paths are given, there is high chance that an invalid path is returned.
		/// </summary>
		/// <param name="absolutePath">The absolute path to a file/folder to convert to relative path.</param>
		/// <param name="absolutePathToCurrentDirectory">An absolute path to the directory which the returned path is relative to.</param>
		public static string ToRelativePath(string absolutePath, string absolutePathToCurrentDirectory) {
			if (absolutePath == null)
				return null;
			char separator = Path.DirectorySeparatorChar;
			absolutePath = SimplifyPath(absolutePath).Replace('\\', separator).Replace('/', separator).TrimEnd(PathSeparator);
			if (absolutePathToCurrentDirectory != null)
				absolutePathToCurrentDirectory = SimplifyPath(absolutePathToCurrentDirectory).Replace('\\', separator).Replace('/', separator).TrimEnd(PathSeparator);
			if (absolutePathToCurrentDirectory.Length == 0)
				return absolutePath;
			else if (absolutePath.Length == 0)
				return string.Empty;
			int drive = absolutePath.IndexOf(':');
			if (drive != -1) {
				int currentDrive = absolutePathToCurrentDirectory.IndexOf(':');
				if (currentDrive != -1) {
					if (!absolutePath.Substring(0, drive).Equals(absolutePathToCurrentDirectory.Substring(0, currentDrive), StringComparison.OrdinalIgnoreCase))
						return absolutePath;
				}
			}
			int i, common = 0, currentCommon = 0;
			int maxIndex = Math.Min(absolutePathToCurrentDirectory.Length, absolutePath.Length) - 1;
			char currentChar;
			for (i = 0; i <= maxIndex; i++) {
				currentChar = absolutePath[i];
				if ((currentChar == separator || currentChar == Path.VolumeSeparatorChar) && currentChar == absolutePathToCurrentDirectory[i]) {
					currentCommon++;
					common = currentCommon;
				} else if (char.ToUpper(currentChar) == char.ToUpper(absolutePathToCurrentDirectory[i]))
					currentCommon++;
				else
					break;
			}
			if (currentCommon > common) {
				if ((i >= absolutePath.Length || (absolutePath[i] == '/' || absolutePath[i] == '\\')) &&
					(i >= absolutePathToCurrentDirectory.Length || (absolutePathToCurrentDirectory[i] == '/' || absolutePathToCurrentDirectory[i] == '\\')))
					common = currentCommon;
			}
			if (common == 0)
				return absolutePath;
			absolutePath = absolutePath.Substring(common).Trim(separator);
			absolutePathToCurrentDirectory = absolutePathToCurrentDirectory.Substring(common).Trim(separator);
			StringBuilder result = new StringBuilder();
			if (absolutePathToCurrentDirectory.Length != 0)
				result.Append(parentDir);
			for (i = 0; i < absolutePathToCurrentDirectory.Length; i++) {
				if (absolutePathToCurrentDirectory[i] == separator)
					result.Append(parentDir);
			}
			result.Append(absolutePath);
			return result.ToString().Trim(DriveMarker);
		}

		/// <summary>
		/// Simplifies the specified absolute path by resolving any ".\", "./", "..\" or "../" found.
		/// Any path separator at the end of the string is removed from the return value.
		/// If the input is invalid, the output will most probably be invalid as well.
		/// </summary>
		/// <param name="absolutePath">The absolute path to resolve (does not need to exist).</param>
		public static string SimplifyPath(string absolutePath) {
			if (string.IsNullOrEmpty(absolutePath))
				return absolutePath;
			Stack<string> components = new Stack<string>();
			char current;
			string component;
			int i, loaned = 0;
			absolutePath = absolutePath.RemoveConsecutiveDuplicates(PathSeparatorSet);
			char lastChar = absolutePath[absolutePath.Length - 1];
			if (!(lastChar == '/' || lastChar == '\\'))
				absolutePath += Path.DirectorySeparatorChar;
			int startIndex = 0, lastIndex = absolutePath.Length - 1;
			for (i = 0; i < absolutePath.Length; i++) {
				current = absolutePath[i];
				if (current == '\\' || current == '/' || i == lastIndex) {
					component = absolutePath.Substring(startIndex, i - startIndex);
					if (component.Length != 0 && component[component.Length - 1] == '.') {
						if (component.Length != 1 && component[component.Length - 2] == '.') {
							if (components.Count == 0)
								loaned++;
							else
								components.Pop();
						}
					} else if (loaned > 0)
						loaned--;
					else
						components.Push(component + current);
					startIndex = i + 1;
				}
			}
			int componentCount = components.Count;
			string[] resultantComponents = new string[componentCount];
			for (i = componentCount - 1; components.Count != 0; i--)
				resultantComponents[i] = components.Pop();
			StringBuilder result = new StringBuilder();
			for (i = 0; i < componentCount; i++)
				result.Append(resultantComponents[i]);
			return result.ToString().TrimEnd(PathSeparators);
		}

		/// <summary>
		/// Concatenates the specified path into a single path. Path.DirectorySeparatorChar is used to combine the paths.
		/// </summary>
		/// <param name="firstPart">The directory part.</param>
		/// <param name="secondPart">The subfolder or file part.</param>
		public static string CombinePath(string firstPart, string secondPart) {
			if (firstPart == null)
				return secondPart;
			else if (secondPart == null)
				return firstPart;
			char separator = Path.DirectorySeparatorChar;
			firstPart = firstPart.Replace('\\', separator).Replace('/', separator).TrimEnd(PathSeparator);
			secondPart = secondPart.Replace('\\', separator).Replace('/', separator).TrimStart(PathSeparator);
			if (secondPart.Length == 0)
				return firstPart;
			else if (firstPart.Length == 0)
				return secondPart;
			else
				return firstPart + separator + secondPart;
		}

		/// <summary>
		/// Concatenates the specified path into a single path. Path.DirectorySeparatorChar is used to combine the paths.
		/// </summary>
		/// <param name="pathComponents">The components that form the path in order. CombinePath("C:", "Users", "User") is
		/// equivalent to CombinePath("C:", CombinePath("Users", "user")).</param>
		public static string CombinePath(params string[] pathComponents) {
			if (pathComponents == null || pathComponents.Length == 0)
				return string.Empty;
			string workingPath = pathComponents[0];
			for (int i = 1; i < pathComponents.Length; i++)
				workingPath = CombinePath(workingPath, pathComponents[i]);
			return workingPath;
		}

		/// <summary>
		/// If the path is relative, it is resolved into absolute, else leave it as is.
		/// </summary>
		/// <param name="path">The path to resolve.</param>
		/// <param name="addDirToPath">Whether to store the directory name in the application's lookup path so that subsequent lookups would not 
		/// require directory path.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static string ResolvePath(string path, bool addDirToPath = true) {
			FileOrFolder exists;
			return ResolvePath(path, addDirToPath, out exists);
		}

		/// <summary>
		/// If the path is relative, it is resolved into absolute, else leave it as is.
		/// </summary>
		/// <param name="path">The path to resolve.</param>
		/// <param name="addDirToPath">Whether to store the directory name in the application's lookup path so that subsequent lookups would not 
		/// require directory path.</param>
		/// <param name="exists">Returns whether the specified file or folder exists.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static string ResolvePath(string path, bool addDirToPath, out FileOrFolder exists) {
			exists = FileOrFolder.NotFound;
			if (path == null)
				return null;
			path = path.Trim();
			if (path.Length == 0)
				return path;
			char separator = Path.DirectorySeparatorChar;
			path = path.Replace('/', separator).Replace('\\', separator).RemoveConsecutiveDuplicates(separator);
			bool fullPathFound;
			string fullPath;
			try {
				fullPath = Path.GetFullPath(path);
				fullPathFound = true;
			} catch {
				fullPathFound = false;
				fullPath = path;
			}
			if (addDirToPath && fullPathFound) {
				try {
					string directory = Path.GetDirectoryName(fullPath);
					lock (Directories.SyncRoot) {
						if (!Directories.Items.Contains(directory))
							Directories.Items.Insert(0, directory);
					}
				} catch {
				}
			}
			exists = Exists(path);
			if (exists != FileOrFolder.NotFound)
				return fullPath;
			string temp;
			bool startsWithSeparator = path[0] == separator;
			string currentDir = Environment.CurrentDirectory;
			if (startsWithSeparator) {
				temp = currentDir.EndsWith(separator) ? currentDir.Substring(0, currentDir.Length - 1) + path : currentDir + path;
				exists = Exists(temp);
				if (exists != FileOrFolder.NotFound)
					return temp;
			} else {
				temp = currentDir.EndsWith(separator) ? currentDir + path : currentDir + separator + path;
				exists = Exists(temp);
				if (exists != FileOrFolder.NotFound)
					return temp;
			}
			foreach (string dir in Directories) {
				if (startsWithSeparator) {
					temp = dir.EndsWith(separator) ? dir.Substring(0, dir.Length - 1) + path : dir + path;
					exists = Exists(temp);
					if (exists != FileOrFolder.NotFound)
						return temp;
				} else {
					temp = dir.EndsWith(separator) ? dir + path : dir + separator + path;
					exists = Exists(temp);
					if (exists != FileOrFolder.NotFound)
						return temp;
				}
			}
			return path;
		}

		/// <summary>
		/// Resolves the target file of the specified shortcut.
		/// </summary>
		/// <param name="file">The path to the shortcut to resolve.</param>
		public static string ResolveShortcut(string file) {
			Shell.ShellLink link = new Shell.ShellLink();
			((Shell.IPersistFile) link).Load(file, Shell.STGM_READ);
			// TODO: if I can get hold of the hwnd call resolve first. This handles moved and renamed files.  
			// ((IShellLinkW)link).Resolve(hwnd, 0) 
			StringBuilder sb = new StringBuilder(Shell.MAX_PATH);
			Shell.WIN32_FIND_DATAW data = new Shell.WIN32_FIND_DATAW();
			((Shell.IShellLinkW) link).GetPath(sb, sb.Capacity, out data, 0);
			string str = sb.ToString();
			return str.EndsWith(".lnk") ? ResolveShortcut(str) : str;
		}
	}
}