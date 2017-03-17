using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PixelNormalMap.Utility.Type
{
	public class Vector4
	{
		public float x;
		public float y;
		public float z;
		public float w;

		public Vector4()
		{
			x = 0;
			y = 0;
			z = 0;
			w = 0;
		}
		public Vector4(float x, float y, float z, float w)
		{
			this.x = x;
			this.y = y;
			this.z = z;
			this.w = w;
		}

		public static Vector4 Zero() {
			return new Vector4(0, 0, 0, 0);
		}

		public static Vector4 One() {
			return new Vector4(1, 1, 1, 1);
		}
	}
}
