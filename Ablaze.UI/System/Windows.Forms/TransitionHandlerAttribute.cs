namespace System.Windows.Forms {
	/// <summary>
	/// Marks a method as a transition handler.
	/// The method header has to be: static AnimationState Transition(&lt;Type&gt; currentValue, &lt;Type&gt; targetValue, double gradient, double linearSpeed, bool stoppable, ref object cachedValue, out &lt;Type&gt; newValue)
	/// </summary>
	[AttributeUsage(AttributeTargets.Method)]
	public sealed class TransitionHandlerAttribute : Attribute {
		/// <summary>
		/// The type of TransitionHandlerAttribute. 
		/// </summary>
		public static readonly Type Type = typeof(TransitionHandlerAttribute);
		/// <summary>
		/// The type of value to transition.
		/// </summary>
		public readonly Type TargetType;

		/// <summary>
		/// Marks a method that handles a transition between two values of a given type.
		/// </summary>
		/// <param name="valueType">The type of value to handle transitions of.</param>
		public TransitionHandlerAttribute(Type valueType) {
			TargetType = valueType;
		}

		/// <summary>
		/// Gets whether TargetType == null.
		/// </summary>
		public override bool IsDefaultAttribute() {
			return TargetType == null;
		}

		/// <summary>
		/// Gets a string representing the type handled by this instance.
		/// </summary>
		public override string ToString() {
			return Type.ToString();
		}
	}
}