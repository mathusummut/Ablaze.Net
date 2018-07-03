﻿using System.Numerics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace System.Graphics.Models {
	/// <summary>
	/// A collection of useful extension methods for mesh manipulation.
	/// </summary>
	public static class MeshExtensions {
		/// <summary>
		/// Converts a list of ModelComponent to a list of ModelStructure.
		/// </summary>
		/// <param name="components">The list to convert.</param>
		/// <returns>A list of ModelStructure.</returns>
		public static IModel[] ToModelStructureArray(this List<MeshComponent> components) {
			IModel[] structures = new IModel[components.Count];
			for (int i = 0; i < structures.Length; i++)
				structures[i] = components[i];
			return structures;
		}

		/// <summary>
		/// Converts a list of Model to a list of ModelStructure.
		/// </summary>
		/// <param name="models">The list to convert.</param>
#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public static IModel[] ToModelStructureArray(this List<Model> models) {
			IModel[] arr = new IModel[models.Count];
			for (int i = 0; i < arr.Length; i++)
				arr[i] = models[i];
			return arr;
		}

		/// <summary>
		/// Merges the specified vertex information into a single vertex array.
		/// </summary>
		/// <param name="vertices">The vertex coordinates.</param>
		/// <param name="texCoords">The corresponding texture coordinates [0 to 1].</param>
		/// <param name="normals">The corresponding normals.</param>
		public static Vertex[] ToVertexArray(this Vector3[] vertices, Vector2[] texCoords, Vector3[] normals) {
			Vertex[] array = new Vertex[vertices.Length];
			for (int i = 0; i < vertices.Length; i++)
				array[i] = new Vertex(vertices[i], texCoords[i], normals[i]);
			return array;
		}

		/// <summary>
		/// Converts the specified vertex information into a sequential list of vertices.
		/// </summary>
		/// <param name="vertices">The vertex coordinates that are ordered using the index array.</param>
		/// <param name="texCoords">The texture coordinates that are ordered using the index array.</param>
		/// <param name="normals">The texture coordinates that are ordered using the index array.</param>
		/// <param name="indices">The indices that represent the vertex order</param>
		public static Vertex[] ToVertexArray(this Vector3[] vertices, Vector2[] texCoords, Vector3[] normals, VertexIndex[] indices) {
			Vertex[] array = new Vertex[indices.Length];
			VertexIndex temp;
			for (int i = 0; i < array.Length; i++) {
				temp = indices[i];
				if (temp.Pos < vertices.Length)
					array[i].Pos = vertices[temp.Pos];
				if (temp.TexPos < texCoords.Length)
					array[i].TexPos = texCoords[temp.TexPos];
				if (temp.Normal < normals.Length)
					array[i].Normal = normals[temp.Normal];
				else
					array[i].Normal = Vector3.UnitY;
			}
			return array;
		}

		/// <summary>
		/// Converts a list of ModelStructure to a list of ModelComponent.
		/// </summary>
		/// <param name="components">The list to convert.</param>
		/// <returns>A list of ModelComponent.</returns>
		public static List<MeshComponent> ToModelComponentList(this List<IModel> components) {
			List<MeshComponent> structures = new List<MeshComponent>(components.Count);
			foreach (IModel temp in components)
				structures.Add(temp as MeshComponent);
			return structures;
		}

		/// <summary>
		/// Converts a list of ModelStructure to a list of Model.
		/// </summary>
		/// <param name="components">The list to convert.</param>
		/// <returns>A list of Model.</returns>
		public static List<Model> ToModelList(this List<IModel> components) {
			List<Model> structures = new List<Model>(components.Count);
			foreach (IModel temp in components)
				structures.Add(temp as Model);
			return structures;
		}

		/// <summary>
		/// Triangulates the specified mesh and returns the sequential vertices that make up the resultant triangles.
		/// </summary>
		/// <param name="quads">An array of sequential vertices that make up the quads to triangulate.</param>
		public static Vertex[] TriangulateQuads(this Vertex[] quads) {
			if (quads == null)
				return null;
			Vertex[] triangles = new Vertex[(quads.Length * 3) / 2];
			ParallelLoop.For(0, quads.Length / 4, delegate (int x) {
				int i = x * 4;
				int triangleOffset = x * 6;
				triangles[triangleOffset] = quads[i];
				triangles[triangleOffset + 1] = quads[i + 1];
				triangles[triangleOffset + 2] = quads[i + 2];
				triangles[triangleOffset + 3] = quads[i];
				triangles[triangleOffset + 4] = quads[i + 2];
				triangles[triangleOffset + 5] = quads[i + 3];
			});
			return triangles;
		}

		/// <summary>
		/// Triangulates the specified mesh and returns the sequential vertices that make up the resultant triangles.
		/// </summary>
		/// <param name="polygon">An array of polygons to triangulate.</param>
		public static Vertex[] TriangulatePolygons(this Vertex[][] polygon) {
			if (polygon == null)
				return null;
			List<Vertex> triangles = new List<Vertex>();
			for (int i = 0; i < polygon.Length; i++)
				triangles.AddRange(TriangulatePolygon(polygon[i]));
			return triangles.ToArray();
		}

		/// <summary>
		///Triangulates the specified mesh and returns the sequential vertices that make up the resultant triangles.
		/// </summary>
		/// <param name="polygon">An array of polygons to triangulate.</param>
		public static Vertex[] TriangulatePolygons(this List<Vertex[]> polygon) {
			if (polygon == null)
				return null;
			List<Vertex> triangles = new List<Vertex>();
			for (int i = 0; i < polygon.Count; i++)
				triangles.AddRange(TriangulatePolygon(polygon[i]));
			return triangles.ToArray();
		}

		/// <summary>
		/// Triangulates the specified mesh and returns the sequential vertices that make up the resultant triangles.
		/// </summary>
		/// <param name="polygon">An array of polygons to triangulate.</param>
		public static Vertex[] TriangulatePolygons(this List<List<Vertex>> polygon) {
			if (polygon == null)
				return null;
			List<Vertex> triangles = new List<Vertex>();
			for (int i = 0; i < polygon.Count; i++)
				triangles.AddRange(TriangulatePolygon(polygon[i]));
			return triangles.ToArray();
		}

		/// <summary>
		/// Triangulates the specified polygon and returns the sequential vertices that make up the resultant triangles.
		/// </summary>
		/// <param name="polygon">The polygon to triangulate.</param>
		public static Vertex[] TriangulatePolygon(this IEnumerable<Vertex> polygon) {
			if (polygon == null)
				return null;
			List<Vertex> result = new List<Vertex>();
			List<Vertex> tempPolygon = new List<Vertex>(polygon);
			int range, initialValue, start1, start2, start = 0, count = tempPolygon.Count;
			while (count >= 3) {
				initialValue = start;
				start1 = (start + 1) % count;
				start2 = (start + 2) % count;
				while (tempPolygon[start].Pos.TripleProduct(tempPolygon[start1].Pos, tempPolygon[start2].Pos) < 0
					|| Intersect(tempPolygon, start, start1, start2)) {
					start = start1;
					start1 = (start + 1) % count;
					start2 = (start + 2) % count;
					if (start == initialValue)
						break;
				}
				result.Add(tempPolygon[start]);
				result.Add(tempPolygon[start1]);
				result.Add(tempPolygon[start2]);
				range = start1 - start;
				start++;
				if (range > 0)
					tempPolygon.RemoveRange(start, range);
				else {
					tempPolygon.RemoveRange(start, count - start);
					tempPolygon.RemoveRange(0, start1 + 1);
				}
				count = tempPolygon.Count;
				start %= count;
			}
			return result.ToArray();
		}

		/// <summary>
		/// Triangulates the specified polygon and returns the sequential vertices that make up the resultant triangles.
		/// </summary>
		/// <param name="polygon">The polygon to triangulate. BEWARE: THE LIST IS MODIFIED IN PLACE.</param>
		internal static VertexIndex[] TriangulatePolygon(this List<KeyValuePair<VertexIndex, Vertex>> polygon) {
			if (polygon == null)
				return null;
			List<VertexIndex> result = new List<VertexIndex>();
			int range, initialValue, start1, start2, start = 0, count = polygon.Count;
			while (count >= 3) {
				initialValue = start;
				start1 = (start + 1) % count;
				start2 = (start + 2) % count;
				while (polygon[start].Value.Pos.TripleProduct(polygon[start1].Value.Pos, polygon[start2].Value.Pos) < 0
					|| Intersect(polygon, start, start1, start2)) {
					start = start1;
					start1 = (start + 1) % count;
					start2 = (start + 2) % count;
					if (start == initialValue)
						break;
				}
				result.Add(polygon[start].Key);
				result.Add(polygon[start1].Key);
				result.Add(polygon[start2].Key);
				range = start1 - start;
				start++;
				if (range > 0)
					polygon.RemoveRange(start, range);
				else {
					polygon.RemoveRange(start, count - start);
					polygon.RemoveRange(0, start1 + 1);
				}
				count = polygon.Count;
				start %= count;
			}
			return result.ToArray();
		}

		private static bool Intersect(this List<Vertex> polygon, int vertex1Ind, int vertex2Ind, int vertex3Ind) {
			float s1, s2, s3;
			for (int i = 0; i < polygon.Count; i++) {
				if (i == vertex1Ind || i == vertex2Ind || i == vertex3Ind)
					continue;
				s1 = polygon[vertex1Ind].Pos.TripleProduct(polygon[vertex2Ind].Pos, polygon[i].Pos);
				s2 = polygon[vertex2Ind].Pos.TripleProduct(polygon[vertex3Ind].Pos, polygon[i].Pos);
				if ((s1 < 0f && s2 > 0f) || (s1 > 0f && s2 < 0f))
					continue;
				s3 = polygon[vertex3Ind].Pos.TripleProduct(polygon[vertex1Ind].Pos, polygon[i].Pos);
				if ((s3 >= 0f && s2 >= 0f) || (s3 <= 0f && s2 <= 0f))
					return true;
			}
			return false;
		}

		private static bool Intersect(this List<KeyValuePair<VertexIndex, Vertex>> polygon, int vertex1Ind, int vertex2Ind, int vertex3Ind) {
			float s1, s2, s3;
			for (int i = 0; i < polygon.Count; i++) {
				if (i == vertex1Ind || i == vertex2Ind || i == vertex3Ind)
					continue;
				s1 = polygon[vertex1Ind].Value.Pos.TripleProduct(polygon[vertex2Ind].Value.Pos, polygon[i].Value.Pos);
				s2 = polygon[vertex2Ind].Value.Pos.TripleProduct(polygon[vertex3Ind].Value.Pos, polygon[i].Value.Pos);
				if ((s1 < 0f && s2 > 0f) || (s1 > 0f && s2 < 0f))
					continue;
				s3 = polygon[vertex3Ind].Value.Pos.TripleProduct(polygon[vertex1Ind].Value.Pos, polygon[i].Value.Pos);
				if ((s3 >= 0f && s2 >= 0f) || (s3 <= 0f && s2 <= 0f))
					return true;
			}
			return false;
		}

		/// <summary>
		/// Converts the specified vertices into a sequential list of vertices.
		/// </summary>
		/// <param name="vertices">The vertices that are ordered using the index array.</param>
		/// <param name="indices">The indices that represent the vertex order (can be byte[], ushort[], uint[] or int[]).</param>
		public static Vertex[] ToVertexArray(this Vertex[] vertices, Array indices) {
			Vertex[] array = new Vertex[indices.Length];
			int i, index, length = indices.Length;
			if (indices is byte[]) {
				byte[] temp = (byte[]) indices;
				for (i = 0; i < length; i++) {
					index = temp[i];
					if (vertices.Length == 0 && index == 0)
						array[i] = new Vertex();
					else
						array[i] = vertices[index];
				}
			} else if (indices is ushort[]) {
				ushort[] temp = (ushort[]) indices;
				for (i = 0; i < length; i++) {
					index = temp[i];
					if (vertices.Length == 0 && index == 0)
						array[i] = new Vertex();
					else
						array[i] = vertices[index];
				}
			} else if (indices is uint[]) {
				uint[] temp = (uint[]) indices;
				uint ind;
				for (i = 0; i < length; i++) {
					ind = temp[i];
					if (vertices.Length == 0 && ind == 0)
						array[i] = new Vertex();
					else
						array[i] = vertices[ind];
				}
			} else if (indices is int[]) {
				int[] temp = (int[]) indices;
				for (i = 0; i < length; i++) {
					index = temp[i];
					if (vertices.Length == 0 && index == 0)
						array[i] = new Vertex();
					else
						array[i] = vertices[index];
				}
			}
			return array;
		}

		/// <summary>
		/// Generates an array that holds the index order of the vertices. The returned array can be byte[], ushort[] or uint[].
		/// </summary>
		/// <param name="bufferData">The triangulated vertices in order.</param>
		/// <param name="optimizeDuplicates">Whether to remove duplicates from the array (may take a long time to perform).</param>
		public static Tuple<Vertex[], Array> GenerateIndices(this Vertex[] bufferData, bool optimizeDuplicates) {
			if (bufferData == null)
				return null;
			if (optimizeDuplicates) {
				int length = bufferData.Length;
				Vertex temp;
				bool found = false;
				if (length <= byte.MaxValue) {
					byte[] array = new byte[length];
					Vertex[] optimizedBuffer = new Vertex[length];
					byte j, actualLength = 0;
					for (int i = 0; i < length; i++) {
						temp = bufferData[i];
						for (j = 0; j < actualLength; j++) {
							if (temp == optimizedBuffer[j]) {
								array[i] = j;
								found = true;
								break;
							}
						}
						if (found)
							found = false;
						else {
							optimizedBuffer[actualLength] = temp;
							array[i] = actualLength;
							actualLength++;
						}
					}
					bufferData = new Vertex[actualLength];
					Array.Copy(optimizedBuffer, bufferData, actualLength);
					return new Tuple<Vertex[], Array>(bufferData, array);
				} else if (length <= ushort.MaxValue) {
					ushort[] array = new ushort[length];
					Vertex[] optimizedBuffer = new Vertex[length];
					ushort j, actualLength = 0;
					for (int i = 0; i < length; i++) {
						temp = bufferData[i];
						for (j = 0; j < actualLength; j++) {
							if (temp == optimizedBuffer[j]) {
								array[i] = j;
								found = true;
								break;
							}
						}
						if (found)
							found = false;
						else {
							optimizedBuffer[actualLength] = temp;
							array[i] = actualLength;
							actualLength++;
						}
					}
					bufferData = new Vertex[actualLength];
					Array.Copy(optimizedBuffer, bufferData, actualLength);
					return new Tuple<Vertex[], Array>(bufferData, array);
				} else {
					uint[] array = new uint[length];
					Vertex[] optimizedBuffer = new Vertex[length];
					uint j, actualLength = 0u;
					for (int i = 0; i < length; i++) {
						temp = bufferData[i];
						for (j = 0; j < actualLength; j++) {
							if (temp == optimizedBuffer[j]) {
								array[i] = j;
								found = true;
								break;
							}
						}
						if (found)
							found = false;
						else {
							optimizedBuffer[actualLength] = temp;
							array[i] = actualLength;
							actualLength++;
						}
					}
					bufferData = new Vertex[actualLength];
					Array.Copy(optimizedBuffer, bufferData, actualLength);
					return new Tuple<Vertex[], Array>(bufferData, array);
				}
			} else {
				if (bufferData.Length <= byte.MaxValue) {
					byte[] array = new byte[bufferData.Length];
					for (byte i = 0; i < array.Length; i++)
						array[i] = i;
					return new Tuple<Vertex[], Array>(bufferData, array);
				} else if (bufferData.Length <= ushort.MaxValue) {
					ushort[] array = new ushort[bufferData.Length];
					for (ushort i = 0; i < array.Length; i++)
						array[i] = i;
					return new Tuple<Vertex[], Array>(bufferData, array);
				} else {
					uint[] array = new uint[bufferData.Length];
					for (uint i = 0u; i < array.Length; i++)
						array[i] = i;
					return new Tuple<Vertex[], Array>(bufferData, array);
				}
			}
		}
	}
}