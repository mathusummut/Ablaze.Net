using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace ConsoleUnitTest {
	public static class UnitTest {
		private static PreciseStopwatch watch = new PreciseStopwatch();

		[STAThread]
		public static void Main() {
			System.Windows.Forms.Form form = new System.Windows.Forms.Form();
			System.Windows.Forms.MessageLoop.ShowDialog(form, false);
			System.Windows.Forms.MessageLoop.ShowDialog(form);
			AsyncTimer timer = new AsyncTimer(6);
			timer.Tick += Timer_Tick;
			timer.Running = true;
			Console.ReadLine();
		}

		private static void Timer_Tick(AsyncTimer obj) {
			Console.WriteLine(watch.RestartGet());
		}

		/*private static int errorCount;

		public static void Main() {
			PriorityQueue<int> queue = new PriorityQueue<int>();
			Random random = new Random();
			int lastQueued;
			int[] snapshot;
			PriorityQueue<int> queueSnapshot;
			for (int lol = 0; lol < 100; lol++) {
				queue.Clear();
				for (int i = 0; i < 5000; i++) { //Enqueue items
					queue.Enqueue(random.Next(0, 1000));
					if (!CheckIsInOrder(queue)) {
						if (errorCount >= 6) {
							Console.WriteLine("Max error limit reached.");
							Console.ReadLine();
						}
						Console.WriteLine("Bug in Enqueue()");
						errorCount++;
					}
					queueSnapshot = queue.Copy();
					snapshot = TakeSnapshot(queue);
					lastQueued = random.Next(0, 100);
					queue.Enqueue(lastQueued);
					queue.Remove(lastQueued);
					if (!Equals(snapshot, TakeSnapshot(queue))) {
						if (errorCount >= 6) {
							Console.WriteLine("Max error limit reached.");
							Console.ReadLine();
						}
						Console.WriteLine(errorCount + ": Bug in Remove()");
						Console.WriteLine("\nBefore:");
						PrintHeap(queueSnapshot);
						Console.WriteLine("After:");
						PrintHeap(queue);
						Console.WriteLine("Should be equal boii!!");
						Console.WriteLine();
						errorCount++;
					}
				}
				Console.WriteLine("Test passed");
			}
			Console.WriteLine("Test complete");
			Console.ReadLine();
		}

		private static bool CheckIsInOrder(PriorityQueue<int> queue) {
			int count = queue.Count;
			int[] items = new int[count];
			PriorityQueue<int> queueCopy = queue.Copy();
			for (int i = 0; i < count; i++)
				items[i] = queueCopy.Dequeue();
			return Equals(items, queue.ToArray());
		}

		private static int[] TakeSnapshot(PriorityQueue<int> queue) {
			int count = queue.Count;
			int[] items = new int[count];
			PriorityQueue<int> queueCopy = queue.Copy();
			for (int i = 0; i < count; i++)
				items[i] = queueCopy.Dequeue();
			if (!Equals(items, queue.ToArray())) {
				if (errorCount >= 6) {
					Console.WriteLine("Max error limit reached.");
					Console.ReadLine();
				} else {
					Console.WriteLine("Bug in rebalancing");
					errorCount++;
				}
			}
			return items;
		}

		private static bool Equals(int[] x, int[] y) {
			if (x.Length == y.Length) {
				for (int i = 0; i < x.Length; i++) {
					if (x[i] != y[i])
						return false;
				}
				return true;
			} else
				return false;
		}

		private static unsafe void PrintHeap(PriorityQueue<int> queue) {
			int[] arr = ((List<int>) typeof(PriorityQueue<int>).GetField("heap", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(queue)).ToArray();
			fixed (int* heap = arr) {
				Print(new Node(heap, 0, arr.Length - 1));
			}
		}

		private unsafe struct Node {
			public int index, maxIndex;
			public int* ptr;

			public bool IsNull {
				get {
					return ptr == null;
				}
			}

			public int Value {
				get {
					return ptr[index];
				}
			}

			public Node LeftChild {
				get {
					int index = this.index * 2 + 1;
					if (index > maxIndex)
						return new Node(); //empty node
					else
						return new Node(ptr, index, maxIndex);
				}
			}

			public Node RightChild {
				get {
					int index = this.index * 2 + 2;
					if (index > maxIndex)
						return new Node(); //empty node
					else
						return new Node(ptr, index, maxIndex);
				}
			}

			public Node(int* ptr, int index, int maxIndex) {
				this.ptr = ptr;
				this.index = index;
				this.maxIndex = maxIndex;
			}

			public override bool Equals(object obj) {
				return obj is Node ? index == ((Node) obj).index : false;
			}

			public static bool operator ==(Node left, Node right) {
				return left.Equals(right);
			}
			public static bool operator !=(Node left, Node right) {
				return !left.Equals(right);
			}

			public override int GetHashCode() {
				return index;
			}
		}

		//Code below adapted from stackoverflow
		private class NodeInfo {
			public Node Node;
			public string Text;
			public int StartPos;
			public int Size {
				get {
					return Text.Length;
				}
			}
			public int EndPos {
				get {
					return StartPos + Size;
				}
				set {
					StartPos = value - Size;
				}
			}
			public NodeInfo Parent, Left, Right;
		}

		private static void Print(this Node root) {
			if (root.IsNull)
				return;
			int rootTop = Console.CursorTop;
			var last = new List<NodeInfo>();
			var next = root;
			Node leftChild;
			for (int level = 0; !next.IsNull; level++) {
				var item = new NodeInfo { Node = next, Text = next.Value.ToString(" 0 ") };
				if (level < last.Count) {
					item.StartPos = last[level].EndPos + 1;
					last[level] = item;
				} else {
					item.StartPos = 0;
					last.Add(item);
				}
				if (level > 0) {
					item.Parent = last[level - 1];
					if (next == item.Parent.Node.LeftChild) {
						item.Parent.Left = item;
						item.EndPos = Math.Max(item.EndPos, item.Parent.StartPos);
					} else {
						item.Parent.Right = item;
						item.StartPos = Math.Max(item.StartPos, item.Parent.EndPos);
					}
				}
				leftChild = next.LeftChild;
				if (leftChild.IsNull)
					next = next.RightChild;
				else
					next = leftChild;
				for (; next.IsNull; item = item.Parent) {
					Print(item, rootTop + 2 * level);
					if (--level < 0)
						break;
					if (item == item.Parent.Left) {
						item.Parent.StartPos = item.EndPos;
						next = item.Parent.Node.RightChild;
					} else {
						if (item.Parent.Left == null)
							item.Parent.EndPos = item.StartPos;
						else
							item.Parent.StartPos += (item.StartPos - item.Parent.EndPos) / 2;
					}
				}
			}
			Console.SetCursorPosition(0, rootTop + 2 * last.Count - 1);
			Console.WriteLine();
		}

		private static void Print(NodeInfo item, int top) {
			Print(item.Text, top, item.StartPos);
			if (item.Left != null)
				PrintLink(top + 1, "┌", "┘", item.Left.StartPos + item.Left.Size / 2, item.StartPos);
			if (item.Right != null)
				PrintLink(top + 1, "└", "┐", item.EndPos - 1, item.Right.StartPos + item.Right.Size / 2);
		}

		private static void PrintLink(int top, string start, string end, int startPos, int endPos) {
			Print(start, top, startPos);
			Print("─", top, startPos + 1, endPos);
			Print(end, top, endPos);
		}

		private static void Print(string s, int top, int left, int right = -1) {
			Console.SetCursorPosition(left, top);
			if (right < 0)
				right = left + s.Length;
			while (Console.CursorLeft < right)
				Console.Write(s);
		}*/
	}
}