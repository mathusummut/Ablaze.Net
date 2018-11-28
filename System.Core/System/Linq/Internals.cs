using System.Collections.Generic;

namespace System.Linq {
	internal sealed class Grouping<K, V> : List<V>, IGrouping<K, V> {
		public K Key {
			get;
			private set;
		}

		public Grouping(K key) {
			Key = key;
		}
	}

	internal struct DelegatingComparer<T> : IComparer<T> {
		private Func<T, T, int> _comparer;

		public DelegatingComparer(Func<T, T, int> comparer) {
			_comparer = comparer;
		}

		public int Compare(T x, T y) {
			return _comparer(x, y);
		}
	}

	internal struct Key<T> {
		public Key(T value) : this() { Value = value; }
		public T Value {
			get; private set;
		}
	}

	internal struct KeyComparer<T> : IEqualityComparer<Key<T>> {
		private IEqualityComparer<T> _innerComparer;

		public KeyComparer(IEqualityComparer<T> innerComparer) {
			_innerComparer = innerComparer == null ? EqualityComparer<T>.Default : innerComparer;
		}

		public bool Equals(Key<T> x, Key<T> y) {
			return _innerComparer.Equals(x.Value, y.Value);
		}

		public int GetHashCode(Key<T> obj) {
			return obj.Value == null ? 0 : _innerComparer.GetHashCode(obj.Value);
		}
	}
}
