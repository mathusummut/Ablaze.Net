using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System {
	public class Wrapper<T> {
		public T Value;

		public Wrapper(T value) {
			Value = value;
		}
	}
}
