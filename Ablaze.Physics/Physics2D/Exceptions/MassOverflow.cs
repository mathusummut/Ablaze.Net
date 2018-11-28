using System;

namespace Ablaze.Physics.Physics2D.Exceptions {
	class MassOverflow : Exception {
		public MassOverflow() : base("Total mass numerical overflow") {}
	}
}
