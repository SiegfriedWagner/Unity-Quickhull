using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
	public static class Quickhull
	{
		/// <summary>
		/// Solves convex hull problem
		/// </summary>
		/// <param name="points">List of points enclosed by hull, at least 3 points</param>
		/// <returns></returns>
		/// <exception cref="ArgumentException">Thrown when argument is invalid</exception>
		public static int[] Solve(Vector2[] points)
		{
			if (points == null)
				throw new ArgumentException("Points are null");
			if (points.Length < 3)
				throw new ArgumentException("At least three points are required");
			if (points.Length == 3)
				return new[]
				{
					0, 1, 2
				};
			int mostLeftIndex = 0;
			int mostRightIndex = 0;
			float mostLeftValue = points[0].x;
			float mostRightValue = points[0].x;
			for (int i = 1; i < points.Length; i++)
			{
				if (points[i].x < mostLeftValue)
				{
					mostLeftIndex = i;
					mostLeftValue = points[i].x;
				}
				else if (points[i].x > mostRightValue)
				{
					mostRightIndex = i;
					mostRightValue = points[i].x;
				}
			}

			var (a, b) = Vector2Extensions.LinearFunctionBetween(points[mostLeftIndex], points[mostRightIndex]);
			var top = new List<int>();
			var bottom = new List<int>();

			for (int i = 0; i < points.Length; i++)
			{
				if (i == mostRightIndex || i == mostLeftIndex)
					continue;
				var point = points[i];
				if (point.y > a * point.x + b)
					top.Add(i);
				else
					bottom.Add(i);
			}
			top = QuickHull(points, points[mostLeftIndex], points[mostRightIndex], top, true);
			bottom = QuickHull(points, points[mostRightIndex], points[mostLeftIndex], bottom, false);
			var result = new int[top.Count + bottom.Count + 2];
			result[0] = mostLeftIndex;
			for (int i = 0; i < top.Count; i++)
			{
				result[i + 1] = top[i];
			}

			result[1 + top.Count] = mostRightIndex;
			for (int i = 0; i < bottom.Count; i++)
			{
				result[i + 2 + top.Count] = bottom[i];
			}

			return result;
		}

		/// <summary>
		/// Quickhull recursive step
		/// </summary>
		/// <param name="points">All points</param>
		/// <param name="mostLeft">Most left point (x lowest) in current step</param>
		/// <param name="mostRight">Most right point (x highest) in current step</param>
		/// <param name="indexToCheck">List of indexes of points in step considered to be added to solution</param>
		/// <param name="top">Where to find new points</param>
		/// <returns>List of indexes that should be included in solution</returns>
		private static List<int> QuickHull(Vector2[] points, Vector2 mostLeft, Vector2 mostRight,
			List<int> indexToCheck, bool top)
		{
			if (indexToCheck.Count <= 1)
				return indexToCheck;
			var i = 0;
			var furthestIndex = indexToCheck[i];
			float furthestDistance = TriangleMath.Height(points[furthestIndex], mostLeft, mostRight);
			for (i = 1; i < indexToCheck.Count; i++)
			{
				var checkedIndex = indexToCheck[i];
				var distance = TriangleMath.Height(points[checkedIndex], mostLeft,mostRight);
				if (distance > furthestDistance)
				{
					furthestIndex = checkedIndex;
					furthestDistance = distance;
				}
			}

			var furthestPoint = points[furthestIndex];
			var left = new List<int>();
			var right = new List<int>();
			{
				var (a, b) = Vector2Extensions.LinearFunctionBetween(mostLeft, furthestPoint);
				var (ap, bp) = Vector2Extensions.LinearFunctionBetween(furthestPoint, mostRight);
				foreach(var index in indexToCheck)
				{
					if (index == furthestIndex)
						continue;
					var point = points[index];
					if (top)
					{
						if (point.y > point.x * a + b)
						{
							left.Add(index);
						}
						else if (point.y > point.x * ap + bp)
						{
							right.Add(index);
						}
					}
					else
					{
						if (point.y < point.x * a + b)
						{
							left.Add(index);
						}
						else if (point.y < point.x * ap + bp)
						{
							right.Add(index);
						}
					}
				}
			}
			left = QuickHull(points, mostLeft, furthestPoint, left, top);
			right = QuickHull(points, furthestPoint, mostRight, right, top);
			var result = new List<int>(new int[left.Count + right.Count + 1]);
			
			for (int j = 0; j < left.Count; j++)
				result[j] = left[j];
			result[left.Count] = furthestIndex;
			for (int j = 0; j < right.Count; j++)
				result[j + left.Count + 1] = right[j];
			return result;
		}
	}
}