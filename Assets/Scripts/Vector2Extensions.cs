using UnityEngine;

namespace DefaultNamespace
{
	public static class Vector2Extensions
	{
		public static (float a, float b) LinearFunctionBetween(Vector2 from, Vector2 to)
		{
			var a = (to.y - from.y) / (to.x - from.x);
			var b = to.y - a * to.x;
			return (a, b);
		}
	}
}