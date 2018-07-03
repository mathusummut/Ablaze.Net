using System;
using SlimDX;
using SlimDX.Direct3D9;

namespace PhysicsEngine {
	public class GridSignedDistanceMap : SignedDistanceMap {
		private float[,,] grid;
		private bool[,,] gridInitialized;

		private readonly Cuboid bbox;
		private readonly float cellSize;
		private readonly float invCellSize;
		private BruteForceSignedDistanceMap bfsdm;

		public GridSignedDistanceMap(Polyhedron p, float cellSize) {
			this.cellSize = cellSize;
			this.invCellSize = 1 / cellSize;
			bfsdm = new BruteForceSignedDistanceMap(p);

			// compute bounding box
			bbox = new Cuboid();
			foreach (Vertex v in p.verts) {
				bbox.Expand(v.pos);
			}

			// create the grid, populate
			// notice we pad the grid
			Vector3 extents = bbox.p0 - bbox.p1;
			int w = (int) Math.Ceiling(extents.X * invCellSize) + 3;
			int h = (int) Math.Ceiling(extents.Y * invCellSize) + 3;
			int d = (int) Math.Ceiling(extents.Z * invCellSize) + 3;
			grid = new float[w, h, d];
			gridInitialized = new bool[w, h, d];

			//            Console.WriteLine("GridSDM: dim1={0}, dim2={1}, dim3={2}", grid.GetLength(0), grid.GetLength(1), grid.GetLength(2));
			//            Console.WriteLine("populating grid...");

			//            for (int i = 0; i < grid.GetLength(0); i++)
			//            {
			//                for (int j = 0; j < grid.GetLength(1); j++)
			//                {
			//                    for (int k = 0; k < grid.GetLength(2); k++)
			//                    {
			//                        grid[i,j,k] = bfsdm.Evaluate(new Vector3(i-1, j-1, k-1) * cellSize + bbox.p0, true).d;
			//                    }
			//                }
			//            }
		}

		private float GridValue(int x, int y, int z) {
			// add one for padding
			x++;
			y++;
			z++;

			if (!gridInitialized[x, y, z]) {
				grid[x, y, z] = bfsdm.Evaluate(new Vector3(x - 1, y - 1, z - 1) * cellSize + bbox.p1, true).d;
				gridInitialized[x, y, z] = true;
			}
			return grid[x, y, z];
		}

		public CollisionPoint Evaluate(Vector3 pt, bool compute_normal) {
			CollisionPoint v = new CollisionPoint(pt, new Vector3(0, 0, 1), float.PositiveInfinity);

			// check against bbox
			if (!bbox.Contains(pt))
				return v;

			Vector3 ptRel = (pt - bbox.p1) * invCellSize;
			int x = (int) Math.Floor(ptRel.X);
			int y = (int) Math.Floor(ptRel.Y);
			int z = (int) Math.Floor(ptRel.Z);
			Vector3 ptOff = ptRel - new Vector3(x, y, z);

			// trilerp to get the value
			//            v.d = 0;
			//            v.d += GridValue(x+0,y+0,z+0) * (1-ptOff.X) * (1-ptOff.Y) * (1-ptOff.Z);
			//            v.d += GridValue(x+1,y+0,z+0) * (0+ptOff.X) * (1-ptOff.Y) * (1-ptOff.Z);
			//            v.d += GridValue(x+0,y+1,z+0) * (1-ptOff.X) * (0+ptOff.Y) * (1-ptOff.Z);
			//            v.d += GridValue(x+1,y+1,z+0) * (0+ptOff.X) * (0+ptOff.Y) * (1-ptOff.Z);
			//            v.d += GridValue(x+0,y+0,z+1) * (1-ptOff.X) * (1-ptOff.Y) * (0+ptOff.Z);
			//            v.d += GridValue(x+1,y+0,z+1) * (0+ptOff.X) * (1-ptOff.Y) * (0+ptOff.Z);
			//            v.d += GridValue(x+0,y+1,z+1) * (1-ptOff.X) * (0+ptOff.Y) * (0+ptOff.Z);
			//            v.d += GridValue(x+1,y+1,z+1) * (0+ptOff.X) * (0+ptOff.Y) * (0+ptOff.Z);

			v.d =
				((GridValue(x + 0, y + 0, z + 0) * (1 - ptOff.X) + GridValue(x + 1, y + 0, z + 0) * (0 + ptOff.X)) * (1 - ptOff.Y) +
				(GridValue(x + 0, y + 1, z + 0) * (1 - ptOff.X) + GridValue(x + 1, y + 1, z + 0) * (0 + ptOff.X)) * (0 + ptOff.Y)) * (1 - ptOff.Z) +
				((GridValue(x + 0, y + 0, z + 1) * (1 - ptOff.X) + GridValue(x + 1, y + 0, z + 1) * (0 + ptOff.X)) * (1 - ptOff.Y) +
				(GridValue(x + 0, y + 1, z + 1) * (1 - ptOff.X) + GridValue(x + 1, y + 1, z + 1) * (0 + ptOff.X)) * (0 + ptOff.Y)) * (0 + ptOff.Z);

			if (compute_normal) {
				// bilerp to get the gradient in each dimension
				float dfdx = 0;
				dfdx += (GridValue(x + 1, y + 0, z + 0) - GridValue(x + 0, y + 0, z + 0)) * (1 - ptOff.Y) * (1 - ptOff.Z);
				dfdx += (GridValue(x + 1, y + 1, z + 0) - GridValue(x + 0, y + 1, z + 0)) * (0 + ptOff.Y) * (1 - ptOff.Z);
				dfdx += (GridValue(x + 1, y + 0, z + 1) - GridValue(x + 0, y + 0, z + 1)) * (1 - ptOff.Y) * (0 + ptOff.Z);
				dfdx += (GridValue(x + 1, y + 1, z + 1) - GridValue(x + 0, y + 1, z + 1)) * (0 + ptOff.Y) * (0 + ptOff.Z);

				float dfdy = 0;
				dfdy += (GridValue(x + 0, y + 1, z + 0) - GridValue(x + 0, y + 0, z + 0)) * (1 - ptOff.Z) * (1 - ptOff.X);
				dfdy += (GridValue(x + 0, y + 1, z + 1) - GridValue(x + 0, y + 0, z + 1)) * (0 + ptOff.Z) * (1 - ptOff.X);
				dfdy += (GridValue(x + 1, y + 1, z + 0) - GridValue(x + 1, y + 0, z + 0)) * (1 - ptOff.Z) * (0 + ptOff.X);
				dfdy += (GridValue(x + 1, y + 1, z + 1) - GridValue(x + 1, y + 0, z + 1)) * (0 + ptOff.Z) * (0 + ptOff.X);

				float dfdz = 0;
				dfdz += (GridValue(x + 0, y + 0, z + 1) - GridValue(x + 0, y + 0, z + 0)) * (1 - ptOff.X) * (1 - ptOff.Y);
				dfdz += (GridValue(x + 1, y + 0, z + 1) - GridValue(x + 1, y + 0, z + 0)) * (0 + ptOff.X) * (1 - ptOff.Y);
				dfdz += (GridValue(x + 0, y + 1, z + 1) - GridValue(x + 0, y + 1, z + 0)) * (1 - ptOff.X) * (0 + ptOff.Y);
				dfdz += (GridValue(x + 1, y + 1, z + 1) - GridValue(x + 1, y + 1, z + 0)) * (0 + ptOff.X) * (0 + ptOff.Y);

				v.n = new Vector3(1, 0, 0) * dfdx + new Vector3(0, 1, 0) * dfdy + new Vector3(0, 0, 1) * dfdz;
				v.n.Normalize();
			}

			return v;
		}
	}
}