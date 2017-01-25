using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PixelNormalMap.Utility.Type
{
	public class Vector3
	{
		public float x;
		public float y;
		public float z;

		public Vector3() {
			x = 0;
			y = 0;
			z = 0;

		}

		public Vector3(float x, float y, float z) {
			this.x = x;
			this.y = y;
			this.z = z;
		}

		public static Vector3 Zero() {
			return new Vector3(0, 0, 0);
		}

		public static Vector3 One(){
			return new Vector3(1, 1, 1);
		}

		public static Vector3 Right() {
			return new Vector3(1, 0, 0);
		}
		public static Vector3 Left() {
			return new Vector3(-1, 0, 0);
		}

		public static Vector3 Up() {
			return new Vector3(0, 1, 0);
		}

		public static Vector3 Down() {
			return new Vector3(0, -1, 0);
		}

		public static Vector3 Back() {
			return new Vector3(0, 0, 1);
		}

		public static Vector3 Forward() {
			return new Vector3(0, 0, -1);
		}

		public static float Dot(Vector3 value1, Vector3 value2) {
			return value1.x * value2.x + value1.y * value2.y + value1.z * value2.z;
		}

		public static Vector3 Cross(Vector3 value1, Vector3 value2) {
			return new Vector3(
				value1.y * value2.z - value1.z * value2.y,
				value1.z * value2.x - value1.x * value2.z,
				value1.x * value2.y - value1.y * value2.x
				);
		}

		public float LengthSquared() {
			return x * x + y * y + z * z;
		}

		public float Length() {
			return (float)Math.Sqrt(LengthSquared());
		}

		public static Vector3 operator + (Vector3 value1, Vector3 value2) {
			return new Vector3(
					value1.x + value2.x,
					value1.y + value2.y,
					value1.z + value2.z
				);
		}
		public static Vector3 operator - (Vector3 value1, Vector3 value2) {
			return new Vector3(
					value1.x - value2.x,
					value1.y - value2.y,
					value1.z - value2.z
				);
		}



	}
}
