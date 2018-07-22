using System;

namespace Ablaze.Physics.Physics2D.Exceptions {
	class NoDirection : Exception {
		public NoDirection() : base("Cannot assign no direction to a force") { }
	}
}
