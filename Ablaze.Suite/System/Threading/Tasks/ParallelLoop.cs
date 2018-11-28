using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace System.Threading.Tasks {
	/// <summary>
	/// Contains methods that aid in range parallellization.
	/// </summary>
	public static class ParallelLoop {
		/// <summary>
		/// Gets the number of threads that parallel loops will use from the thread pool when called.
		/// </summary>
		public static readonly int ThreadCount;
		/// <summary>
		/// The partitioner used for while loops.
		/// </summary>
		public static readonly Partitioner<bool> WhilePartitioner = new InfinitePartitioner();
		private static WaitCallback callbackInt = CallbackInt,
			callbackIntInterruptible = CallbackIntInterruptible,
			callbackLong = CallbackLong,
			callbackLongInterruptible = CallbackLongInterruptible,
			callbackByte = CallbackByte,
			callbackByteInterruptible = CallbackByteInterruptible;

		static ParallelLoop() {
			int processors = Environment.ProcessorCount;
			ThreadCount = processors == 1 ? 1 : processors + processors;
		}

#if !NET35
		/// <summary>
		/// Executes a while-loop concurrently.
		/// </summary>
		/// <param name="parallelOptions">Configures some parallelization options.</param>
		/// <param name="condition">The condition to check whether to stop or continue (true to continue, false to stop).</param>
		/// <param name="body">The method to call for each iteration.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void While(ParallelOptions parallelOptions, Func<bool> condition, Action body) {
			Parallel.ForEach(InfinitePartitioner.IterateUntilFalse(condition), parallelOptions, ignored => body());
		}

		/// <summary>
		/// Executes a while-loop concurrently.
		/// </summary>
		/// <param name="parallelOptions">Configures some parallelization options.</param>
		/// <param name="condition">The condition to check whether to stop or continue (true to continue, false to stop).</param>
		/// <param name="body">The method to call for each iteration.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void While(ParallelOptions parallelOptions, Func<bool> condition, Action<ParallelLoopState> body) {
			Parallel.ForEach(WhilePartitioner, parallelOptions, (ignored, loopState) => {
				if (condition())
					body(loopState);
				else
					loopState.Stop();
			});
		}

		/// <summary>
		/// Executes a while-loop concurrently.
		/// </summary>
		/// <param name="parallelOptions">Configures some parallelization options.</param>
		/// <param name="body">The method to call for each iteration.
		/// Ignore the boolean parameter, and if you want to interrupt the loop, simply invoke Stop() on the ParallelLoopState parameter.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void While(ParallelOptions parallelOptions, Action<bool, ParallelLoopState> body) {
			Parallel.ForEach(WhilePartitioner, parallelOptions, body);
		}
#endif

		/// <summary>
		/// Executes the specified delegates concurrently.
		/// </summary>
		/// <param name="parallellizationCutoff">The invocations are parallellized only if the number of items is be greater or equal to the cutoff.</param>
		/// <param name="delegates">The delegates to invoke.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void Invoke(int parallellizationCutoff, params Action[] delegates) {
			if (delegates == null || delegates.Length == 0)
				return;
			else if (delegates.Length == 1)
				delegates[0]();
			else
				For(0, delegates.Length, i => delegates[i](), parallellizationCutoff);
		}

		/// <summary>
		/// Executes a for-loop concurrently.
		/// </summary>
		/// <param name="start">The start index.</param>
		/// <param name="endExclusive">The end index, which is not included in the iteration (ex. if it is 50, the loop stops at 49).</param>
		/// <param name="func">The callback of each iteration, where int holds the index.</param>
		/// <param name="parallellizationCutoff">The loop is parallellized only if the number of iterations will be greater or equal to the cutoff.</param>
		/// <param name="maxThreads">The maximum number of threads to allocate to this task.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void For(int start, int endExclusive, Action<int> func, int parallellizationCutoff = 2, int maxThreads = int.MaxValue) {
#if NET35
			For(start, endExclusive, 1, func, parallellizationCutoff, maxThreads);
#else
			if (func == null)
				return;
			int count = endExclusive - start;
			if (count <= 0)
				return;
			else if (maxThreads <= 0)
				maxThreads = int.MaxValue;
			int cores = Math.Min(count, Math.Min(maxThreads, ThreadCount));
			if (parallellizationCutoff < 2)
				parallellizationCutoff = 2;
			if (cores <= 1 || count < parallellizationCutoff) {
				for (; start < endExclusive; start++)
					func(start);
			} else {
				Parallel.For(start, endExclusive,/* new ParallelOptions() {
					MaxDegreeOfParallelism = maxThreads //if uncommented, the slowdown is real
				},*/ func);
			}
#endif
		}

		/// <summary>
		/// Executes a for-loop concurrently.
		/// </summary>
		/// <param name="start">The start index.</param>
		/// <param name="endExclusive">The end index, which is not included in the iteration (ex. if it is 50, the loop stops at 49).</param>
		/// <param name="step">The increment step (can be negative).</param>
		/// <param name="func">The callback of each iteration, where int holds the index.</param>
		/// <param name="parallellizationCutoff">The loop is parallellized only if the number of iterations will be greater or equal to the cutoff.</param>
		/// <param name="maxThreads">The maximum number of threads to allocate to this task.</param>
		public static void For(int start, int endExclusive, int step, Action<int> func, int parallellizationCutoff = 2, int maxThreads = int.MaxValue) {
			if (func == null)
				return;
			else if (step == 0) {
				if (start == endExclusive)
					func(start);
				return;
			}
			int count;
			bool reverse = step < 0;
			if (reverse) {
				count = start - endExclusive;
				step = -step;
			} else
				count = endExclusive - start;
			int countOverStep = count / step;
			if (count <= 0)
				return;
			else if (maxThreads <= 0)
				maxThreads = int.MaxValue;
			int cores = Math.Min(countOverStep, Math.Min(maxThreads, ThreadCount));
			if (parallellizationCutoff < 2)
				parallellizationCutoff = 2;
			if (cores <= 1 || countOverStep < parallellizationCutoff) {
				if (reverse) {
					for (; start > endExclusive; start -= step)
						func(start);
				} else {
					for (; start < endExclusive; start += step)
						func(start);
				}
			} else {
				count = step * (int) ((count / (float) cores) / step);
				ManualResetEventSlim resetEvent = new ManualResetEventSlim();
				cores--;
				int[] finishCount = new int[] { cores };
				int ending = reverse ? start - count * cores : start + count * cores;
				if (reverse) {
					while (start > ending)
						ThreadPool.UnsafeQueueUserWorkItem(callbackInt, new DataInt(start, start -= count, step, func, null, null, resetEvent, finishCount));
					for (; start > endExclusive; start -= step)
						func(start);
				} else {
					while (start < ending)
						ThreadPool.UnsafeQueueUserWorkItem(callbackInt, new DataInt(start, start += count, step, func, null, null, resetEvent, finishCount));
					for (; start < endExclusive; start += step)
						func(start);
				}
				resetEvent.Wait();
				resetEvent.Dispose();
			}
		}

		/// <summary>
		/// Executes a for-loop concurrently.
		/// </summary>
		/// <param name="start">The start index.</param>
		/// <param name="endExclusive">The end index, which is not included in the iteration (ex. if it is 50, the loop stops at 49).</param>
		/// <param name="step">The increment step (can be negative).</param>
		/// <param name="func">The callback of each iteration, where int holds the index, and object is the "parameter" variable.</param>
		/// <param name="parameter">The parameter to pass to the function.</param>
		/// <param name="parallellizationCutoff">The loop is parallellized only if the number of iterations will be greater or equal to the cutoff.</param>
		/// <param name="maxThreads">The maximum number of threads to allocate to this task.</param>
		public static void For(int start, int endExclusive, int step, object parameter, Action<int, object> func, int parallellizationCutoff = 2, int maxThreads = int.MaxValue) {
			if (func == null)
				return;
			else if (step == 0) {
				if (start == endExclusive)
					func(start, parameter);
				return;
			}
			int count;
			bool reverse = step < 0;
			if (reverse) {
				count = start - endExclusive;
				step = -step;
			} else
				count = endExclusive - start;
			int countOverStep = count / step;
			if (count <= 0)
				return;
			else if (maxThreads <= 0)
				maxThreads = int.MaxValue;
			int cores = Math.Min(countOverStep, Math.Min(maxThreads, ThreadCount));
			if (parallellizationCutoff < 2)
				parallellizationCutoff = 2;
			if (cores <= 1 || countOverStep < parallellizationCutoff) {
				if (reverse) {
					for (; start > endExclusive; start -= step)
						func(start, parameter);
				} else {
					for (; start < endExclusive; start += step)
						func(start, parameter);
				}
			} else {
				count = step * (int) ((count / (float) cores) / step);
				ManualResetEventSlim resetEvent = new ManualResetEventSlim();
				cores--;
				int[] finishCount = new int[] { cores };
				int ending = reverse ? start - count * cores : start + count * cores;
				if (reverse) {
					while (start > ending)
						ThreadPool.UnsafeQueueUserWorkItem(callbackInt, new DataInt(start, start -= count, step, null, func, parameter, resetEvent, finishCount));
					for (; start > endExclusive; start -= step)
						func(start, parameter);
				} else {
					while (start < ending)
						ThreadPool.UnsafeQueueUserWorkItem(callbackInt, new DataInt(start, start += count, step, null, func, parameter, resetEvent, finishCount));
					for (; start < endExclusive; start += step)
						func(start, parameter);
				}
				resetEvent.Wait();
				resetEvent.Dispose();
			}
		}

		/// <summary>
		/// Executes a for-loop concurrently.
		/// </summary>
		/// <param name="start">The start index.</param>
		/// <param name="endExclusive">The end index, which is not included in the iteration (ex. if it is 50, the loop stops at 49).</param>
		/// <param name="func">The callback of each iteration, where int holds the index. The return value is whether the loop should stop (true to stop, false to continue).</param>
		/// <param name="parallellizationCutoff">The loop is parallellized only if the number of iterations will be greater or equal to the cutoff.</param>
		/// <param name="maxThreads">The maximum number of threads to allocate to this task.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void For(int start, int endExclusive, Func<int, bool> func, int parallellizationCutoff = 2, int maxThreads = int.MaxValue) {
			For(start, endExclusive, 1, func, parallellizationCutoff, maxThreads);
		}

		/// <summary>
		/// Executes a for-loop concurrently.
		/// </summary>
		/// <param name="start">The start index.</param>
		/// <param name="endExclusive">The end index, which is not included in the iteration (ex. if it is 50, the loop stops at 49).</param>
		/// <param name="step">The increment step (can be negative).</param>
		/// <param name="func">The callback of each iteration, where int holds the index. The return value is whether the loop should stop (true to stop, false to continue).</param>
		/// <param name="parallellizationCutoff">The loop is parallellized only if the number of iterations will be greater or equal to the cutoff.</param>
		/// <param name="maxThreads">The maximum number of threads to allocate to this task.</param>
		public static void For(int start, int endExclusive, int step, Func<int, bool> func, int parallellizationCutoff = 2, int maxThreads = int.MaxValue) {
			if (func == null)
				return;
			else if (step == 0) {
				if (start == endExclusive)
					func(start);
				return;
			}
			int count;
			bool reverse = step < 0;
			if (reverse) {
				count = start - endExclusive;
				step = -step;
			} else
				count = endExclusive - start;
			int countOverStep = count / step;
			if (count <= 0)
				return;
			else if (maxThreads <= 0)
				maxThreads = int.MaxValue;
			int cores = Math.Min(countOverStep, Math.Min(maxThreads, ThreadCount));
			if (parallellizationCutoff < 2)
				parallellizationCutoff = 2;
			if (cores <= 1 || countOverStep < parallellizationCutoff) {
				if (reverse) {
					while (start > endExclusive && !func(start))
						start -= step;
				} else {
					while (start < endExclusive && !func(start))
						start += step;
				}
			} else {
				count = step * (int) ((count / (float) cores) / step);
				ManualResetEventSlim resetEvent = new ManualResetEventSlim();
				cores--;
				int[] finishCount = new int[] { cores };
				int ending = reverse ? start - count * cores : start + count * cores;
				byte[] flags = new byte[2];
				if (reverse) {
					while (start > ending)
						ThreadPool.UnsafeQueueUserWorkItem(callbackIntInterruptible, new DataIntInterruptible(start, start -= count, step, func, null, null, resetEvent, flags, finishCount));
					for (; start > endExclusive && Volatile.Read(ref flags[0]) == 0; start -= step) {
						if (func(start)) {
							Volatile.Write(ref flags[1], 1);
							Volatile.Write(ref flags[0], 1);
							resetEvent.Dispose();
							return;
						}
					}
				} else {
					while (start < ending)
						ThreadPool.UnsafeQueueUserWorkItem(callbackIntInterruptible, new DataIntInterruptible(start, start += count, step, func, null, null, resetEvent, flags, finishCount));
					for (; start < endExclusive && Volatile.Read(ref flags[0]) == 0; start += step) {
						if (func(start)) {
							Volatile.Write(ref flags[1], 1);
							Volatile.Write(ref flags[0], 1);
							resetEvent.Dispose();
							return;
						}
					}
				}
				resetEvent.Wait();
				resetEvent.Dispose();
			}
		}

		/// <summary>
		/// Executes a for-loop concurrently.
		/// </summary>
		/// <param name="start">The start index.</param>
		/// <param name="endExclusive">The end index, which is not included in the iteration (ex. if it is 50, the loop stops at 49).</param>
		/// <param name="step">The increment step (can be negative).</param>
		/// <param name="func">The callback of each iteration, where int holds the index, and object is the "parameter" variable. The return value is whether the loop should stop (true to stop, false to continue).</param>
		/// <param name="parameter">The parameter to pass to the function.</param>
		/// <param name="parallellizationCutoff">The loop is parallellized only if the number of iterations will be greater or equal to the cutoff.</param>
		/// <param name="maxThreads">The maximum number of threads to allocate to this task.</param>
		public static void For(int start, int endExclusive, int step, object parameter, Func<int, object, bool> func, int parallellizationCutoff = 2, int maxThreads = int.MaxValue) {
			if (func == null)
				return;
			else if (step == 0) {
				if (start == endExclusive)
					func(start, parameter);
				return;
			}
			int count;
			bool reverse = step < 0;
			if (reverse) {
				count = start - endExclusive;
				step = -step;
			} else
				count = endExclusive - start;
			int countOverStep = count / step;
			if (count <= 0)
				return;
			else if (maxThreads <= 0)
				maxThreads = int.MaxValue;
			int cores = Math.Min(countOverStep, Math.Min(maxThreads, ThreadCount));
			if (parallellizationCutoff < 2)
				parallellizationCutoff = 2;
			if (cores <= 1 || countOverStep < parallellizationCutoff) {
				if (reverse) {
					while (start > endExclusive && !func(start, parameter))
						start -= step;
				} else {
					while (start < endExclusive && !func(start, parameter))
						start += step;
				}
			} else {
				count = step * (int) ((count / (float) cores) / step);
				ManualResetEventSlim resetEvent = new ManualResetEventSlim();
				cores--;
				int[] finishCount = new int[] { cores };
				int ending = reverse ? start - count * cores : start + count * cores;
				byte[] flags = new byte[2];
				if (reverse) {
					while (start > ending)
						ThreadPool.UnsafeQueueUserWorkItem(callbackIntInterruptible, new DataIntInterruptible(start, start -= count, step, null, func, parameter, resetEvent, flags, finishCount));
					for (; start > endExclusive && Volatile.Read(ref flags[0]) == 0; start -= step) {
						if (func(start, parameter)) {
							Volatile.Write(ref flags[1], 1);
							Volatile.Write(ref flags[0], 1);
							resetEvent.Dispose();
							return;
						}
					}
				} else {
					while (start < ending)
						ThreadPool.UnsafeQueueUserWorkItem(callbackIntInterruptible, new DataIntInterruptible(start, start += count, step, null, func, parameter, resetEvent, flags, finishCount));
					for (; start < endExclusive && Volatile.Read(ref flags[0]) == 0; start += step) {
						if (func(start, parameter)) {
							Volatile.Write(ref flags[1], 1);
							Volatile.Write(ref flags[0], 1);
							resetEvent.Dispose();
							return;
						}
					}
				}
				resetEvent.Wait();
				resetEvent.Dispose();
			}
		}

		/// <summary>
		/// Executes a for-loop concurrently.
		/// </summary>
		/// <param name="start">The start index.</param>
		/// <param name="endExclusive">The end index, which is not included in the iteration (ex. if it is 50, the loop stops at 49).</param>
		/// <param name="func">The callback of each iteration, where int holds the index.</param>
		/// <param name="parallellizationCutoff">The loop is parallellized only if the number of iterations will be greater or equal to the cutoff.</param>
		/// <param name="maxThreads">The maximum number of threads to allocate to this task.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void For(long start, long endExclusive, Action<long> func, long parallellizationCutoff = 2L, int maxThreads = int.MaxValue) {
#if NET35
			For(start, endExclusive, 1L, func, parallellizationCutoff, maxThreads);
#else
			if (func == null)
				return;
			long count = endExclusive - start;
			if (count <= 0L)
				return;
			else if (maxThreads <= 0)
				maxThreads = int.MaxValue;
			long cores = Math.Min(count, Math.Min(maxThreads, ThreadCount));
			if (parallellizationCutoff < 2L)
				parallellizationCutoff = 2L;
			if (cores <= 1L || count < parallellizationCutoff) {
				for (; start < endExclusive; start++)
					func(start);
			} else {
				Parallel.For(start, endExclusive, /*new ParallelOptions() {
					MaxDegreeOfParallelism = maxThreads //if uncommented, the slowdown is real
				},*/ func);
			}
#endif
		}

		/// <summary>
		/// Executes a for-loop concurrently.
		/// </summary>
		/// <param name="start">The start index.</param>
		/// <param name="endExclusive">The end index, which is not included in the iteration (ex. if it is 50, the loop stops at 49).</param>
		/// <param name="step">The increment step (can be negative).</param>
		/// <param name="func">The callback of each iteration, where int holds the index.</param>
		/// <param name="parallellizationCutoff">The loop is parallellized only if the number of iterations will be greater or equal to the cutoff.</param>
		/// <param name="maxThreads">The maximum number of threads to allocate to this task.</param>
		public static void For(long start, long endExclusive, long step, Action<long> func, long parallellizationCutoff = 2, int maxThreads = int.MaxValue) {
			if (func == null)
				return;
			else if (step == 0) {
				if (start == endExclusive)
					func(start);
				return;
			}
			long count;
			bool reverse = step < 0;
			if (reverse) {
				count = start - endExclusive;
				step = -step;
			} else
				count = endExclusive - start;
			long countOverStep = count / step;
			if (count <= 0)
				return;
			else if (maxThreads <= 0)
				maxThreads = int.MaxValue;
			long cores = Math.Min(countOverStep, Math.Min(maxThreads, ThreadCount));
			if (parallellizationCutoff < 2)
				parallellizationCutoff = 2;
			if (cores <= 1 || countOverStep < parallellizationCutoff) {
				if (reverse) {
					for (; start > endExclusive; start -= step)
						func(start);
				} else {
					for (; start < endExclusive; start += step)
						func(start);
				}
			} else {
				count = step * (long) ((count / (double) cores) / step);
				ManualResetEventSlim resetEvent = new ManualResetEventSlim();
				cores--;
				int[] finishCount = new int[] { (int) cores };
				long ending = reverse ? start - count * cores : start + count * cores;
				if (reverse) {
					while (start > ending)
						ThreadPool.UnsafeQueueUserWorkItem(callbackLong, new DataLong(start, start -= count, step, func, null, null, resetEvent, finishCount));
					for (; start > endExclusive; start -= step)
						func(start);
				} else {
					while (start < ending)
						ThreadPool.UnsafeQueueUserWorkItem(callbackLong, new DataLong(start, start += count, step, func, null, null, resetEvent, finishCount));
					for (; start < endExclusive; start += step)
						func(start);
				}
				resetEvent.Wait();
				resetEvent.Dispose();
			}
		}

		/// <summary>
		/// Executes a for-loop concurrently.
		/// </summary>
		/// <param name="start">The start index.</param>
		/// <param name="endExclusive">The end index, which is not included in the iteration (ex. if it is 50, the loop stops at 49).</param>
		/// <param name="step">The increment step (can be negative).</param>
		/// <param name="func">The callback of each iteration, where int holds the index, and object is the "parameter" variable.</param>
		/// <param name="parameter">The parameter to pass to the function.</param>
		/// <param name="parallellizationCutoff">The loop is parallellized only if the number of iterations will be greater or equal to the cutoff.</param>
		/// <param name="maxThreads">The maximum number of threads to allocate to this task.</param>
		public static void For(long start, long endExclusive, long step, object parameter, Action<long, object> func, long parallellizationCutoff = 2, int maxThreads = int.MaxValue) {
			if (func == null)
				return;
			else if (step == 0) {
				if (start == endExclusive)
					func(start, parameter);
				return;
			}
			long count;
			bool reverse = step < 0;
			if (reverse) {
				count = start - endExclusive;
				step = -step;
			} else
				count = endExclusive - start;
			long countOverStep = count / step;
			if (count <= 0)
				return;
			else if (maxThreads <= 0)
				maxThreads = int.MaxValue;
			long cores = Math.Min(countOverStep, Math.Min(maxThreads, ThreadCount));
			if (parallellizationCutoff < 2)
				parallellizationCutoff = 2;
			if (cores <= 1 || countOverStep < parallellizationCutoff) {
				if (reverse) {
					for (; start > endExclusive; start -= step)
						func(start, parameter);
				} else {
					for (; start < endExclusive; start += step)
						func(start, parameter);
				}
			} else {
				count = step * (long) ((count / (double) cores) / step);
				ManualResetEventSlim resetEvent = new ManualResetEventSlim();
				cores--;
				int[] finishCount = new int[] { (int) cores };
				long ending = reverse ? start - count * cores : start + count * cores;
				if (reverse) {
					while (start > ending)
						ThreadPool.UnsafeQueueUserWorkItem(callbackLong, new DataLong(start, start -= count, step, null, func, parameter, resetEvent, finishCount));
					for (; start > endExclusive; start -= step)
						func(start, parameter);
				} else {
					while (start < ending)
						ThreadPool.UnsafeQueueUserWorkItem(callbackLong, new DataLong(start, start += count, step, null, func, parameter, resetEvent, finishCount));
					for (; start < endExclusive; start += step)
						func(start, parameter);
				}
				resetEvent.Wait();
				resetEvent.Dispose();
			}
		}

		/// <summary>
		/// Executes a for-loop concurrently.
		/// </summary>
		/// <param name="start">The start index.</param>
		/// <param name="endExclusive">The end index, which is not included in the iteration (ex. if it is 50, the loop stops at 49).</param>
		/// <param name="func">The callback of each iteration, where int holds the index. The return value is whether the loop should stop (true to stop, false to continue).</param>
		/// <param name="parallellizationCutoff">The loop is parallellized only if the number of iterations will be greater or equal to the cutoff.</param>
		/// <param name="maxThreads">The maximum number of threads to allocate to this task.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static void For(long start, long endExclusive, Func<long, bool> func, long parallellizationCutoff = 2, int maxThreads = int.MaxValue) {
			For(start, endExclusive, 1L, func, parallellizationCutoff, maxThreads);
		}

		/// <summary>
		/// Executes a for-loop concurrently.
		/// </summary>
		/// <param name="start">The start index.</param>
		/// <param name="endExclusive">The end index, which is not included in the iteration (ex. if it is 50, the loop stops at 49).</param>
		/// <param name="step">The increment step (can be negative).</param>
		/// <param name="func">The callback of each iteration, where int holds the index. The return value is whether the loop should stop (true to stop, false to continue).</param>
		/// <param name="parallellizationCutoff">The loop is parallellized only if the number of iterations will be greater or equal to the cutoff.</param>
		/// <param name="maxThreads">The maximum number of threads to allocate to this task.</param>
		public static void For(long start, long endExclusive, long step, Func<long, bool> func, long parallellizationCutoff = 2, int maxThreads = int.MaxValue) {
			if (func == null)
				return;
			else if (step == 0) {
				if (start == endExclusive)
					func(start);
				return;
			}
			long count;
			bool reverse = step < 0;
			if (reverse) {
				count = start - endExclusive;
				step = -step;
			} else
				count = endExclusive - start;
			long countOverStep = count / step;
			if (count <= 0)
				return;
			else if (maxThreads <= 0)
				maxThreads = int.MaxValue;
			long cores = Math.Min(countOverStep, Math.Min(maxThreads, ThreadCount));
			if (parallellizationCutoff < 2)
				parallellizationCutoff = 2;
			if (cores <= 1 || countOverStep < parallellizationCutoff) {
				if (reverse) {
					while (start > endExclusive && !func(start))
						start -= step;
				} else {
					while (start < endExclusive && !func(start))
						start += step;
				}
			} else {
				count = step * (long) ((count / (double) cores) / step);
				ManualResetEventSlim resetEvent = new ManualResetEventSlim();
				cores--;
				int[] finishCount = new int[] { (int) cores };
				long ending = reverse ? start - count * cores : start + count * cores;
				byte[] flags = new byte[2];
				if (reverse) {
					while (start > ending)
						ThreadPool.UnsafeQueueUserWorkItem(callbackLongInterruptible, new DataLongInterruptible(start, start -= count, step, func, null, null, resetEvent, flags, finishCount));
					for (; start > endExclusive && Volatile.Read(ref flags[0]) == 0; start -= step) {
						if (func(start)) {
							Volatile.Write(ref flags[1], 1);
							Volatile.Write(ref flags[0], 1);
							resetEvent.Dispose();
							return;
						}
					}
				} else {
					while (start < ending)
						ThreadPool.UnsafeQueueUserWorkItem(callbackLongInterruptible, new DataLongInterruptible(start, start += count, step, func, null, null, resetEvent, flags, finishCount));
					for (; start < endExclusive && Volatile.Read(ref flags[0]) == 0; start += step) {
						if (func(start)) {
							Volatile.Write(ref flags[1], 1);
							Volatile.Write(ref flags[0], 1);
							resetEvent.Dispose();
							return;
						}
					}
				}
				resetEvent.Wait();
				resetEvent.Dispose();
			}
		}

		/// <summary>
		/// Executes a for-loop concurrently.
		/// </summary>
		/// <param name="start">The start index.</param>
		/// <param name="endExclusive">The end index, which is not included in the iteration (ex. if it is 50, the loop stops at 49).</param>
		/// <param name="step">The increment step (can be negative).</param>
		/// <param name="func">The callback of each iteration, where int holds the index, and object is the "parameter" variable. The return value is whether the loop should stop (true to stop, false to continue).</param>
		/// <param name="parameter">The parameter to pass to the function.</param>
		/// <param name="parallellizationCutoff">The loop is parallellized only if the number of iterations will be greater or equal to the cutoff.</param>
		/// <param name="maxThreads">The maximum number of threads to allocate to this task.</param>
		public static void For(long start, long endExclusive, long step, object parameter, Func<long, object, bool> func, long parallellizationCutoff = 2, int maxThreads = int.MaxValue) {
			if (func == null)
				return;
			else if (step == 0) {
				if (start == endExclusive)
					func(start, parameter);
				return;
			}
			long count;
			bool reverse = step < 0;
			if (reverse) {
				count = start - endExclusive;
				step = -step;
			} else
				count = endExclusive - start;
			long countOverStep = count / step;
			if (count <= 0)
				return;
			else if (maxThreads <= 0)
				maxThreads = int.MaxValue;
			long cores = Math.Min(countOverStep, Math.Min(maxThreads, ThreadCount));
			if (parallellizationCutoff < 2)
				parallellizationCutoff = 2;
			if (cores <= 1 || countOverStep < parallellizationCutoff) {
				if (reverse) {
					while (start > endExclusive && !func(start, parameter))
						start -= step;
				} else {
					while (start < endExclusive && !func(start, parameter))
						start += step;
				}
			} else {
				count = step * (long) ((count / (double) cores) / step);
				ManualResetEventSlim resetEvent = new ManualResetEventSlim();
				cores--;
				int[] finishCount = new int[] { (int) cores };
				long ending = reverse ? start - count * cores : start + count * cores;
				byte[] flags = new byte[2];
				if (reverse) {
					while (start > ending)
						ThreadPool.UnsafeQueueUserWorkItem(callbackLongInterruptible, new DataLongInterruptible(start, start -= count, step, null, func, parameter, resetEvent, flags, finishCount));
					for (; start > endExclusive && Volatile.Read(ref flags[0]) == 0; start -= step) {
						if (func(start, parameter)) {
							Volatile.Write(ref flags[1], 1);
							Volatile.Write(ref flags[0], 1);
							resetEvent.Dispose();
							return;
						}
					}
				} else {
					while (start < ending)
						ThreadPool.UnsafeQueueUserWorkItem(callbackLongInterruptible, new DataLongInterruptible(start, start += count, step, null, func, parameter, resetEvent, flags, finishCount));
					for (; start < endExclusive && Volatile.Read(ref flags[0]) == 0; start += step) {
						if (func(start, parameter)) {
							Volatile.Write(ref flags[1], 1);
							Volatile.Write(ref flags[0], 1);
							resetEvent.Dispose();
							return;
						}
					}
				}
				resetEvent.Wait();
				resetEvent.Dispose();
			}
		}

		/// <summary>
		/// Executes a for-loop concurrently.
		/// </summary>
		/// <param name="ptr">The start pointer. Pointer should point to a byte.</param>
		/// <param name="count">The number of steps to make.</param>
		/// <param name="step">The size of each stride (can be negative).</param>
		/// <param name="func">The callback of each iteration, where int holds the index.</param>
		/// <param name="parallellizationCutoff">The loop is parallellized only if the number of iterations will be greater or equal to the cutoff.</param>
		/// <param name="maxThreads">The maximum number of threads to allocate to this task.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static unsafe void For(IntPtr ptr, long count, long step, Action<IntPtr> func, long parallellizationCutoff = 2, int maxThreads = int.MaxValue) {
			For((byte*) ptr.ToPointer(), count, step, func, parallellizationCutoff, maxThreads);
		}

		/// <summary>
		/// Executes a for-loop concurrently.
		/// </summary>
		/// <param name="ptr">The start pointer.</param>
		/// <param name="count">The number of steps to make.</param>
		/// <param name="step">The size of each stride (can be negative).</param>
		/// <param name="func">The callback of each iteration, where int holds the index.</param>
		/// <param name="parallellizationCutoff">The loop is parallellized only if the number of iterations will be greater or equal to the cutoff.</param>
		/// <param name="maxThreads">The maximum number of threads to allocate to this task.</param>
		[CLSCompliant(false)]
		public static unsafe void For(byte* ptr, long count, long step, Action<IntPtr> func, long parallellizationCutoff = 2, int maxThreads = int.MaxValue) {
			if (func == null || step == 0 || count <= 0)
				return;
			byte* endExclusive;
			bool reverse = step < 0;
			if (reverse) {
				step = -step;
				byte* temp = ptr;
				ptr += count * step;
				endExclusive = temp;
			} else
				endExclusive = ptr + count * step;
			if (maxThreads <= 0)
				maxThreads = int.MaxValue;
			long cores = Math.Min(count, Math.Min(maxThreads, ThreadCount));
			if (parallellizationCutoff < 2)
				parallellizationCutoff = 2;
			if (cores == 1 || count < parallellizationCutoff) {
				if (reverse) {
					for (; ptr > endExclusive; ptr -= step)
						func(new IntPtr(ptr));
				} else {
					for (; ptr < endExclusive; ptr += step)
						func(new IntPtr(ptr));
				}
			} else {
				count = step * (long) (((count * step) / (double) cores) / step);
				ManualResetEventSlim resetEvent = new ManualResetEventSlim();
				cores--;
				byte* ending = reverse ? ptr - count * cores : ptr + count * cores;
				int[] finishCount = new int[] { (int) cores };
				if (reverse) {
					while (ptr > ending)
						ThreadPool.UnsafeQueueUserWorkItem(callbackByte, new DataByte(ptr, ptr -= count, step, func, null, null, resetEvent, finishCount));
					for (; ptr > endExclusive; ptr -= step)
						func(new IntPtr(ptr));
				} else {
					while (ptr < ending)
						ThreadPool.UnsafeQueueUserWorkItem(callbackByte, new DataByte(ptr, ptr += count, step, func, null, null, resetEvent, finishCount));
					for (; ptr < endExclusive; ptr += step)
						func(new IntPtr(ptr));
				}
				resetEvent.Wait();
				resetEvent.Dispose();
			}
		}

		/// <summary>
		/// Executes a for-loop concurrently.
		/// </summary>
		/// <param name="ptr">The start pointer. Pointer should point to a byte.</param>
		/// <param name="count">The number of steps to make.</param>
		/// <param name="step">The size of each stride (can be negative).</param>
		/// <param name="func">The callback of each iteration, where int holds the index, and object is the "parameter" variable.</param>
		/// <param name="parameter">The parameter to pass to the function.</param>
		/// <param name="parallellizationCutoff">The loop is parallellized only if the number of iterations will be greater or equal to the cutoff.</param>
		/// <param name="maxThreads">The maximum number of threads to allocate to this task.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static unsafe void For(IntPtr ptr, long count, long step, object parameter, Action<IntPtr, object> func, long parallellizationCutoff = 2, int maxThreads = int.MaxValue) {
			For((byte*) ptr.ToPointer(), count, step, parameter, func, parallellizationCutoff, maxThreads);
		}

		/// <summary>
		/// Executes a for-loop concurrently.
		/// </summary>
		/// <param name="ptr">The start pointer.</param>
		/// <param name="count">The number of steps to make.</param>
		/// <param name="step">The size of each stride (can be negative).</param>
		///	<param name="func">The callback of each iteration, where int holds the index, and object is the "parameter" variable.</param>
		/// <param name="parameter">The parameter to pass to the function.</param>
		/// <param name="parallellizationCutoff">The loop is parallellized only if the number of iterations will be greater or equal to the cutoff.</param>
		/// <param name="maxThreads">The maximum number of threads to allocate to this task.</param>
		[CLSCompliant(false)]
		public static unsafe void For(byte* ptr, long count, long step, object parameter, Action<IntPtr, object> func, long parallellizationCutoff = 2, int maxThreads = int.MaxValue) {
			if (func == null || step == 0 || count <= 0)
				return;
			byte* endExclusive;
			bool reverse = step < 0;
			if (reverse) {
				step = -step;
				byte* temp = ptr;
				ptr += count * step;
				endExclusive = temp;
			} else
				endExclusive = ptr + count * step;
			if (maxThreads <= 0)
				maxThreads = int.MaxValue;
			long cores = Math.Min(count, Math.Min(maxThreads, ThreadCount));
			if (parallellizationCutoff < 2)
				parallellizationCutoff = 2;
			if (cores == 1 || count < parallellizationCutoff) {
				if (reverse) {
					for (; ptr > endExclusive; ptr -= step)
						func(new IntPtr(ptr), parameter);
				} else {
					for (; ptr < endExclusive; ptr += step)
						func(new IntPtr(ptr), parameter);
				}
			} else {
				count = step * (long) (((count * step) / (double) cores) / step);
				ManualResetEventSlim resetEvent = new ManualResetEventSlim();
				cores--;
				byte* ending = reverse ? ptr - count * cores : ptr + count * cores;
				int[] finishCount = new int[] { (int) cores };
				if (reverse) {
					while (ptr > ending)
						ThreadPool.UnsafeQueueUserWorkItem(callbackByte, new DataByte(ptr, ptr -= count, step, null, func, parameter, resetEvent, finishCount));
					for (; ptr > endExclusive; ptr -= step)
						func(new IntPtr(ptr), parameter);
				} else {
					while (ptr < ending)
						ThreadPool.UnsafeQueueUserWorkItem(callbackByte, new DataByte(ptr, ptr += count, step, null, func, parameter, resetEvent, finishCount));
					for (; ptr < endExclusive; ptr += step)
						func(new IntPtr(ptr), parameter);
				}
				resetEvent.Wait();
				resetEvent.Dispose();
			}
		}

		/// <summary>
		/// Executes a for-loop concurrently.
		/// </summary>
		/// <param name="ptr">The start pointer. Pointer should point to a byte.</param>
		/// <param name="count">The number of steps to make.</param>
		/// <param name="step">The size of each stride (can be negative).</param>
		/// <param name="func">The callback of each iteration, where int holds the index. The return value is whether the loop should stop (true to stop, false to continue).</param>
		/// <param name="parallellizationCutoff">The loop is parallellized only if the number of iterations will be greater or equal to the cutoff.</param>
		/// <param name="maxThreads">The maximum number of threads to allocate to this task.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static unsafe void For(IntPtr ptr, long count, long step, Func<IntPtr, bool> func, long parallellizationCutoff = 2, int maxThreads = int.MaxValue) {
			For((byte*) ptr.ToPointer(), count, step, func, parallellizationCutoff, maxThreads);
		}

		/// <summary>
		/// Executes a for-loop concurrently.
		/// </summary>
		/// <param name="ptr">The start pointer.</param>
		/// <param name="count">The number of steps to make.</param>
		/// <param name="step">The size of each stride (can be negative).</param>
		/// <param name="func">The callback of each iteration, where int holds the index. The return value is whether the loop should stop (true to stop, false to continue).</param>
		/// <param name="parallellizationCutoff">The loop is parallellized only if the number of iterations will be greater or equal to the cutoff.</param>
		/// <param name="maxThreads">The maximum number of threads to allocate to this task.</param>
		[CLSCompliant(false)]
		public static unsafe void For(byte* ptr, long count, long step, Func<IntPtr, bool> func, long parallellizationCutoff = 2, int maxThreads = int.MaxValue) {
			if (func == null || step == 0 || count <= 0)
				return;
			byte* endExclusive;
			bool reverse = step < 0;
			if (reverse) {
				step = -step;
				byte* temp = ptr;
				ptr += count * step;
				endExclusive = temp;
			} else
				endExclusive = ptr + count * step;
			if (maxThreads <= 0)
				maxThreads = int.MaxValue;
			long cores = Math.Min(count, Math.Min(maxThreads, ThreadCount));
			if (parallellizationCutoff < 2)
				parallellizationCutoff = 2;
			if (cores == 1 || count < parallellizationCutoff) {
				if (reverse) {
					while (ptr > endExclusive && !func(new IntPtr(ptr)))
						ptr -= step;
				} else {
					while (ptr < endExclusive && !func(new IntPtr(ptr)))
						ptr += step;
				}
			} else {
				count = step * (long) (((count * step) / (double) cores) / step);
				ManualResetEventSlim resetEvent = new ManualResetEventSlim();
				cores--;
				byte* ending = reverse ? ptr - count * cores : ptr + count * cores;
				int[] finishCount = new int[] { (int) cores };
				byte[] flags = new byte[2];
				if (reverse) {
					while (ptr > ending)
						ThreadPool.UnsafeQueueUserWorkItem(callbackByteInterruptible, new DataByteInterruptible(ptr, ptr -= count, step, func, null, null, resetEvent, flags, finishCount));
					for (; ptr > endExclusive && Volatile.Read(ref flags[0]) == 0; ptr -= step) {
						if (func(new IntPtr(ptr))) {
							Volatile.Write(ref flags[1], 1);
							Volatile.Write(ref flags[0], 1);
							resetEvent.Dispose();
							return;
						}
					}
				} else {
					while (ptr < ending)
						ThreadPool.UnsafeQueueUserWorkItem(callbackByteInterruptible, new DataByteInterruptible(ptr, ptr += count, step, func, null, null, resetEvent, flags, finishCount));
					for (; ptr < endExclusive && Volatile.Read(ref flags[0]) == 0; ptr += step) {
						if (func(new IntPtr(ptr))) {
							Volatile.Write(ref flags[1], 1);
							Volatile.Write(ref flags[0], 1);
							resetEvent.Dispose();
							return;
						}
					}
				}
				resetEvent.Wait();
				resetEvent.Dispose();
			}
		}

		/// <summary>
		/// Executes a for-loop concurrently.
		/// </summary>
		/// <param name="ptr">The start pointer. Pointer should point to a byte.</param>
		/// <param name="count">The number of steps to make.</param>
		/// <param name="step">The size of each stride (can be negative).</param>
		/// <param name="parameter">The parameter to pass to the function.</param>
		/// <param name="func">The callback of each iteration, where int holds the index, and object is the "parameter" variable. The return value is whether the loop should stop (true to stop, false to continue).</param>
		/// <param name="parallellizationCutoff">The loop is parallellized only if the number of iterations will be greater or equal to the cutoff.</param>
		/// <param name="maxThreads">The maximum number of threads to allocate to this task.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static unsafe void For(IntPtr ptr, long count, long step, object parameter, Func<IntPtr, object, bool> func, long parallellizationCutoff = 2, int maxThreads = int.MaxValue) {
			For((byte*) ptr.ToPointer(), count, step, parameter, func, parallellizationCutoff, maxThreads);
		}

		/// <summary>
		/// Executes a for-loop concurrently.
		/// </summary>
		/// <param name="ptr">The start pointer.</param>
		/// <param name="count">The number of steps to make.</param>
		/// <param name="step">The size of each stride (can be negative).</param>
		/// <param name="func">The callback of each iteration, where int holds the index, and object is the "parameter" variable. The return value is whether the loop should stop (true to stop, false to continue).</param>
		/// <param name="parameter">The parameter to pass to the function.</param>
		/// <param name="parallellizationCutoff">The loop is parallellized only if the number of iterations will be greater or equal to the cutoff.</param>
		/// <param name="maxThreads">The maximum number of threads to allocate to this task.</param>
		[CLSCompliant(false)]
		public static unsafe void For(byte* ptr, long count, long step, object parameter, Func<IntPtr, object, bool> func, long parallellizationCutoff = 2, int maxThreads = int.MaxValue) {
			if (func == null || step == 0 || count <= 0)
				return;
			byte* endExclusive;
			bool reverse = step < 0;
			if (reverse) {
				step = -step;
				byte* temp = ptr;
				ptr += count * step;
				endExclusive = temp;
			} else
				endExclusive = ptr + count * step;
			if (maxThreads <= 0)
				maxThreads = int.MaxValue;
			long cores = Math.Min(count, Math.Min(maxThreads, ThreadCount));
			if (parallellizationCutoff < 2)
				parallellizationCutoff = 2;
			if (cores == 1 || count < parallellizationCutoff) {
				if (reverse) {
					while (ptr > endExclusive && !func(new IntPtr(ptr), parameter))
						ptr -= step;
				} else {
					while (ptr < endExclusive && !func(new IntPtr(ptr), parameter))
						ptr += step;
				}
			} else {
				count = step * (long) (((count * step) / (double) cores) / step);
				ManualResetEventSlim resetEvent = new ManualResetEventSlim();
				cores--;
				byte* ending = reverse ? ptr - count * cores : ptr + count * cores;
				int[] finishCount = new int[] { (int) cores };
				byte[] flags = new byte[2];
				if (reverse) {
					while (ptr > ending)
						ThreadPool.UnsafeQueueUserWorkItem(callbackByteInterruptible, new DataByteInterruptible(ptr, ptr -= count, step, null, func, parameter, resetEvent, flags, finishCount));
					for (; ptr > endExclusive && Volatile.Read(ref flags[0]) == 0; ptr -= step) {
						if (func(new IntPtr(ptr), parameter)) {
							Volatile.Write(ref flags[1], 1);
							Volatile.Write(ref flags[0], 1);
							resetEvent.Dispose();
							return;
						}
					}
				} else {
					while (ptr < ending)
						ThreadPool.UnsafeQueueUserWorkItem(callbackByteInterruptible, new DataByteInterruptible(ptr, ptr += count, step, null, func, parameter, resetEvent, flags, finishCount));
					for (; ptr < endExclusive && Volatile.Read(ref flags[0]) == 0; ptr += step) {
						if (func(new IntPtr(ptr), parameter)) {
							Volatile.Write(ref flags[1], 1);
							Volatile.Write(ref flags[0], 1);
							resetEvent.Dispose();
							return;
						}
					}
				}
				resetEvent.Wait();
				resetEvent.Dispose();
			}
		}

		private static void CallbackInt(object threadData) {
			DataInt data = (DataInt) threadData;
			int index = data.start;
			int end = data.end;
			int step = data.step;
			if (data.parametricFunc == null) {
				Action<int> func = data.func;
				if (index <= end) {
					for (; index < end; index += step)
						func(index);
				} else {
					for (; index > end; index -= step)
						func(index);
				}
			} else {
				Action<int, object> func = data.parametricFunc;
				object parameter = data.parameter;
				if (index <= end) {
					for (; index < end; index += step)
						func(index, parameter);
				} else {
					for (; index > end; index -= step)
						func(index, parameter);
				}
			}
			if (Interlocked.Decrement(ref data.finishCount[0]) == 0)
				data.resetEvent.Set();
		}

		private static void CallbackIntInterruptible(object threadData) {
			DataIntInterruptible data = (DataIntInterruptible) threadData;
			int index = data.start;
			int end = data.end;
			int step = data.step;
			byte[] flags = data.flags;
			if (data.parametricFunc == null) {
				Func<int, bool> func = data.func;
				if (index <= end) {
					for (; index < end && Volatile.Read(ref flags[0]) == 0; index += step) {
						if (func(index)) {
							Volatile.Write(ref flags[0], 1);
							break;
						}
					}
				} else {
					for (; index > end && Volatile.Read(ref flags[0]) == 0; index -= step) {
						if (func(index)) {
							Volatile.Write(ref flags[0], 1);
							break;
						}
					}
				}
			} else {
				Func<int, object, bool> func = data.parametricFunc;
				object parameter = data.parameter;
				if (index <= end) {
					for (; index < end && Volatile.Read(ref flags[0]) == 0; index += step) {
						if (func(index, parameter)) {
							Volatile.Write(ref flags[0], 1);
							break;
						}
					}
				} else {
					for (; index > end && Volatile.Read(ref flags[0]) == 0; index -= step) {
						if (func(index, parameter)) {
							Volatile.Write(ref flags[0], 1);
							break;
						}
					}
				}
			}
			if (Interlocked.Decrement(ref data.finishCount[0]) == 0 && Volatile.Read(ref flags[1]) == 0)
				data.resetEvent.Set();
		}

		private static void CallbackLong(object threadData) {
			DataLong data = (DataLong) threadData;
			long index = data.start;
			long end = data.end;
			long step = data.step;
			if (data.parametricFunc == null) {
				Action<long> func = data.func;
				if (index <= end) {
					for (; index < end; index += step)
						func(index);
				} else {
					for (; index > end; index -= step)
						func(index);
				}
			} else {
				Action<long, object> func = data.parametricFunc;
				object parameter = data.parameter;
				if (index <= end) {
					for (; index < end; index += step)
						func(index, parameter);
				} else {
					for (; index > end; index -= step)
						func(index, parameter);
				}
			}
			if (Interlocked.Decrement(ref data.finishCount[0]) == 0)
				data.resetEvent.Set();
		}

		private static void CallbackLongInterruptible(object threadData) {
			DataLongInterruptible data = (DataLongInterruptible) threadData;
			long index = data.start;
			long end = data.end;
			long step = data.step;
			byte[] flags = data.flags;
			if (data.parametricFunc == null) {
				Func<long, bool> func = data.func;
				if (index <= end) {
					for (; index < end && Volatile.Read(ref flags[0]) == 0; index += step) {
						if (func(index)) {
							Volatile.Write(ref flags[0], 1);
							break;
						}
					}
				} else {
					for (; index > end && Volatile.Read(ref flags[0]) == 0; index -= step) {
						if (func(index)) {
							Volatile.Write(ref flags[0], 1);
							break;
						}
					}
				}
			} else {
				Func<long, object, bool> func = data.parametricFunc;
				object parameter = data.parameter;
				if (index <= end) {
					for (; index < end && Volatile.Read(ref flags[0]) == 0; index += step) {
						if (func(index, parameter)) {
							Volatile.Write(ref flags[0], 1);
							break;
						}
					}
				} else {
					for (; index > end && Volatile.Read(ref flags[0]) == 0; index -= step) {
						if (func(index, parameter)) {
							Volatile.Write(ref flags[0], 1);
							break;
						}
					}
				}
			}
			if (Interlocked.Decrement(ref data.finishCount[0]) == 0 && Volatile.Read(ref flags[1]) == 0)
				data.resetEvent.Set();
		}

		private static unsafe void CallbackByte(object threadData) {
			DataByte data = (DataByte) threadData;
			byte* index = data.start;
			byte* end = data.end;
			long step = data.step;
			if (data.parametricFunc == null) {
				Action<IntPtr> func = data.func;
				if (index <= end) {
					for (; index < end; index += step)
						func(new IntPtr(index));
				} else {
					for (; index > end; index -= step)
						func(new IntPtr(index));
				}
			} else {
				Action<IntPtr, object> func = data.parametricFunc;
				object parameter = data.parameter;
				if (index <= end) {
					for (; index < end; index += step)
						func(new IntPtr(index), parameter);
				} else {
					for (; index > end; index -= step)
						func(new IntPtr(index), parameter);
				}
			}
			if (Interlocked.Decrement(ref data.finishCount[0]) == 0)
				data.resetEvent.Set();
		}

		private static unsafe void CallbackByteInterruptible(object threadData) {
			DataByteInterruptible data = (DataByteInterruptible) threadData;
			byte* index = data.start;
			byte* end = data.end;
			long step = data.step;
			byte[] flags = data.flags;
			if (data.parametricFunc == null) {
				Func<IntPtr, bool> func = data.func;
				if (index <= end) {
					for (; index < end && Volatile.Read(ref flags[0]) == 0; index += step) {
						if (func(new IntPtr(index))) {
							Volatile.Write(ref flags[0], 1);
							break;
						}
					}
				} else {
					for (; index > end && Volatile.Read(ref flags[0]) == 0; index -= step) {
						if (func(new IntPtr(index))) {
							Volatile.Write(ref flags[0], 1);
							break;
						}
					}
				}
			} else {
				Func<IntPtr, object, bool> func = data.parametricFunc;
				object parameter = data.parameter;
				if (index <= end) {
					for (; index < end && Volatile.Read(ref flags[0]) == 0; index += step) {
						if (func(new IntPtr(index), parameter)) {
							Volatile.Write(ref flags[0], 1);
							break;
						}
					}
				} else {
					for (; index > end && Volatile.Read(ref flags[0]) == 0; index -= step) {
						if (func(new IntPtr(index), parameter)) {
							Volatile.Write(ref flags[0], 1);
							break;
						}
					}
				}
			}
			if (Interlocked.Decrement(ref data.finishCount[0]) == 0 && Volatile.Read(ref flags[1]) == 0)
				data.resetEvent.Set();
		}

		private sealed class DataInt {
			public Action<int> func;
			public Action<int, object> parametricFunc;
			public object parameter;
			public ManualResetEventSlim resetEvent;
			public int[] finishCount;
			public int start, end, step;

			public DataInt(int start, int end, int step, Action<int> func, Action<int, object> parametricFunc, object parameter, ManualResetEventSlim resetEvent, int[] finishCount) {
				this.start = start;
				this.end = end;
				this.step = step;
				this.func = func;
				this.parametricFunc = parametricFunc;
				this.parameter = parameter;
				this.resetEvent = resetEvent;
				this.finishCount = finishCount;
			}
		}

		private sealed class DataIntInterruptible {
			public Func<int, bool> func;
			public Func<int, object, bool> parametricFunc;
			public object parameter;
			public ManualResetEventSlim resetEvent;
			public byte[] flags;
			public int[] finishCount;
			public int start, end, step;

			public DataIntInterruptible(int start, int end, int step, Func<int, bool> func, Func<int, object, bool> parametricFunc, object parameter, ManualResetEventSlim resetEvent, byte[] flags, int[] finishCount) {
				this.start = start;
				this.end = end;
				this.step = step;
				this.func = func;
				this.parametricFunc = parametricFunc;
				this.parameter = parameter;
				this.resetEvent = resetEvent;
				this.flags = flags;
				this.finishCount = finishCount;
			}
		}

		private sealed class DataLong {
			public Action<long> func;
			public Action<long, object> parametricFunc;
			public object parameter;
			public ManualResetEventSlim resetEvent;
			public int[] finishCount;
			public long start, end, step;

			public DataLong(long start, long end, long step, Action<long> func, Action<long, object> parametricFunc, object parameter, ManualResetEventSlim resetEvent, int[] finishCount) {
				this.start = start;
				this.end = end;
				this.step = step;
				this.func = func;
				this.parametricFunc = parametricFunc;
				this.parameter = parameter;
				this.resetEvent = resetEvent;
				this.finishCount = finishCount;
			}
		}

		private sealed class DataLongInterruptible {
			public Func<long, bool> func;
			public Func<long, object, bool> parametricFunc;
			public object parameter;
			public ManualResetEventSlim resetEvent;
			public byte[] flags;
			public int[] finishCount;
			public long start, end, step;

			public DataLongInterruptible(long start, long end, long step, Func<long, bool> func, Func<long, object, bool> parametricFunc, object parameter, ManualResetEventSlim resetEvent, byte[] flags, int[] finishCount) {
				this.start = start;
				this.end = end;
				this.step = step;
				this.func = func;
				this.parametricFunc = parametricFunc;
				this.parameter = parameter;
				this.resetEvent = resetEvent;
				this.flags = flags;
				this.finishCount = finishCount;
			}
		}

		private sealed class DataByte {
			public Action<IntPtr> func;
			public Action<IntPtr, object> parametricFunc;
			public object parameter;
			public ManualResetEventSlim resetEvent;
			public int[] finishCount;
			public unsafe byte* start, end;
			public long step;

			public unsafe DataByte(byte* start, byte* end, long step, Action<IntPtr> func, Action<IntPtr, object> parametricFunc, object parameter, ManualResetEventSlim resetEvent, int[] finishCount) {
				this.start = start;
				this.end = end;
				this.step = step;
				this.func = func;
				this.parametricFunc = parametricFunc;
				this.parameter = parameter;
				this.resetEvent = resetEvent;
				this.finishCount = finishCount;
			}
		}

		private sealed class DataByteInterruptible {
			public Func<IntPtr, bool> func;
			public Func<IntPtr, object, bool> parametricFunc;
			public object parameter;
			public ManualResetEventSlim resetEvent;
			public byte[] flags;
			public int[] finishCount;
			public unsafe byte* start, end;
			public long step;

			public unsafe DataByteInterruptible(byte* start, byte* end, long step, Func<IntPtr, bool> func, Func<IntPtr, object, bool> parametricFunc, object parameter, ManualResetEventSlim resetEvent, byte[] flags, int[] finishCount) {
				this.start = start;
				this.end = end;
				this.step = step;
				this.func = func;
				this.parametricFunc = parametricFunc;
				this.parameter = parameter;
				this.resetEvent = resetEvent;
				this.flags = flags;
				this.finishCount = finishCount;
			}
		}

		private sealed class InfinitePartitioner : Partitioner<bool>, IEnumerable<bool> {
			public override bool SupportsDynamicPartitions {
				get {
					return true;
				}
			}

			public static IEnumerable<bool> IterateUntilFalse(Func<bool> condition) {
				while (condition())
					yield return true;
			}

			public override IList<IEnumerator<bool>> GetPartitions(int partitionCount) {
				IEnumerator<bool>[] arr = new IEnumerator<bool>[partitionCount];
				for (int i = 0; i < arr.Length; i++)
					arr[i] = GetInfiniteEnumerator();
				return arr;
			}

			public override IEnumerable<bool> GetDynamicPartitions() {
				return this;
			}

			public static IEnumerator<bool> GetInfiniteEnumerator() {
				do {
					yield return true;
				} while (true);
			}

			public IEnumerator<bool> GetEnumerator() {
				return GetInfiniteEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator() {
				return GetInfiniteEnumerator();
			}
		}
	}
}