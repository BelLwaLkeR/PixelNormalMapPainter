using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PixelNormalMap.Utility.Type
{
	class Vector2
	{
		public float x;
		public float y;

		public Vector2() {
			x = 0;
			y = 0;
		}

		public Vector2(float x, float y) {
			this.x = x;
			this.y = y;
		}

		public Vector2 Zero() {
			return new Vector2(0, 0);
		}
		public static Vector2 One() {
			return new Vector2(1, 1);
		}

		public static Vector2 Right() {
			return new Vector2(1, 0);
		}
		public static Vector2 Left() {
			return new Vector2(-1, 0);
		}

		public static Vector2 Up() {
			return new Vector2(0, 1);
		}

		public static Vector2 Down() {
			return new Vector2(0, -1);
		}
	}
}
