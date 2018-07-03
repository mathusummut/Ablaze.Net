using System.Collections.Generic;

namespace System.Physics2D {
	public class Fluid {
		private double forceMult;
		public double Traction;
		public readonly Particle Component;
		internal List<Force> Forces = new List<Force>();

		public double ForceMultiplier {
			get {
				return forceMult;
			}
			set {
				if (value == forceMult)
					return;
				forceMult = value;
				Force temp;
				for (int i = 0; i < Forces.Count; i++) {
					temp = Forces[i];
					temp.Strength = value;
					Forces[i] = temp;
				}
			}
		}

		public Fluid(Particle component, double forceMultiplier, double traction) {
			ForceMultiplier = forceMultiplier;
			Traction = traction;
			Component = component;
			Component.fluid = this;
		}

		public static implicit operator Particle(Fluid f) {
			return f.Component;
		}

		public void AddFluidCurrent(Force f) {
			Forces.Add(f);
		}

		public void RemoveFluidCurrent(Force f) {
			for (int i = 0; i < Forces.Count; i++) {
				if (Forces[i] == f)
					Forces.RemoveAt(i);
			}
		}

		public void RemoveAllFluidCurrents() {
			Forces.Clear();
		}

		~Fluid() {
			Dispose();
		}

		public void Dispose() {
			Component.Dispose();
			GC.SuppressFinalize(this);
		}
	}
}