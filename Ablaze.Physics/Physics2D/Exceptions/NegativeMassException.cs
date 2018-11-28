using System;

namespace Ablaze.Physics.Physics2D.Exceptions {
	class NegativeMassException : Exception {
		public NegativeMassException() : base("Mass cannot be negative for a Particle2D") {}
	}
}
