using System;
using UnityEngine;

namespace DefaultNamespace
{
	public static class TriangleMath
	{
		public static float Area(Vector2 v1, Vector2 v2, Vector2 v3)
		{
			return Math.Abs((v1.x * (v2.y - v3.y) + v2.x * (v3.y - v1.y) + v3.x * (v1.y - v2.y)) / 2.0f);
		}

		public static float Height(Vector2 vh, Vector2 v1, Vector2 v2)
		{
			return 2 * Area(vh, v1, v2) / Vector2.Distance(v1, v2);
		}
	}
}