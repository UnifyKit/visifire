/*   
    Copyright (C) 2008 Webyog Softworks Private Limited

    This file is a part of Visifire Charts.
 
    Visifire is a free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.
      
    You should have received a copy of the GNU General Public License
    along with Visifire Charts.  If not, see <http://www.gnu.org/licenses/>.
  
    If GPL is not suitable for your products or company, Webyog provides Visifire 
    under a flexible commercial license designed to meet your specific usage and 
    distribution requirements. If you have already obtained a commercial license 
    from Webyog, you can use this file under those license terms.
    
*/

#if WPF

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Diagnostics;

#else
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Diagnostics;
#endif

namespace Visifire.Charts
{
    /// <summary>
    /// Visifire.Charts.AxisLabel class
    /// </summary>
    /// http://en.wikipedia.org/wiki/B%C3%A9zier_curve
    /// http://en.wikipedia.org/wiki/B%C3%A9zier_spline
 	public static class Bezier
	{
		/// <summary>
		/// Get open-ended Bezier Spline Control Points.
		/// </summary>
        /// <param name="knots">Line tension</param>
        /// /// <param name="knots">Input Knot Bezier spline points.</param>
		/// <param name="firstControlPoints">Output First Control points array of knots.Length - 1 length.</param>
		/// <param name="secondControlPoints">Output Second Control points array of knots.Length - 1 length.</param>
		/// <exception cref="ArgumentNullException"><paramref name="knots"/> parameter must be not null.</exception>
		/// <exception cref="ArgumentException"><paramref name="knots"/> array must containg at least two points.</exception>
		public static void GetCurveControlPoints(Double tension, Point[] knots, out Point[] firstControlPoints, out Point[] secondControlPoints)
		{   
			if (knots == null)
				throw new ArgumentNullException("knots");
			int n = knots.Length - 1;
            if (n < 1)
            {
                firstControlPoints = null;
                secondControlPoints = null;
                return;
            }

            // throw new ArgumentException("At least two knot points required", "knots");
			if (n == 1)
			{ // Special case: Bezier curve should be a straight line.
				firstControlPoints = new Point[1];
				// 3P1 = 2P0 + P3
				firstControlPoints[0].X = (2 * knots[0].X + knots[1].X) / 3;
				firstControlPoints[0].Y = (2 * knots[0].Y + knots[1].Y) / 3;

				secondControlPoints = new Point[1];
				// P2 = 2P1 – P0
				secondControlPoints[0].X = 2 * firstControlPoints[0].X - knots[0].X;
				secondControlPoints[0].Y = 2 * firstControlPoints[0].Y - knots[0].Y;
				return;
			}

			// Calculate first Bezier control points
			// Right hand side vector
			double[] rhs = new double[n];

			// Set right hand side X values
			for (int i = 1; i < n - 1; ++i)
				rhs[i] = 4 * knots[i].X + 2 * knots[i + 1].X;
			rhs[0] = knots[0].X + 2 * knots[1].X;
			rhs[n - 1] = (8 * knots[n - 1].X + knots[n].X) / 2.0;
			// Get first control points X-values
            double[] x = GetFirstControlPoints(rhs, tension);

			// Set right hand side Y values
			for (int i = 1; i < n - 1; ++i)
				rhs[i] = 4 * knots[i].Y + 2 * knots[i + 1].Y;
			rhs[0] = knots[0].Y + 2 * knots[1].Y;
			rhs[n - 1] = (8 * knots[n - 1].Y + knots[n].Y) / 2.0;
			// Get first control points Y-values
            double[] y = GetFirstControlPoints(rhs, tension);

			// Fill output arrays.
			firstControlPoints = new Point[n];
			secondControlPoints = new Point[n];
			for (int i = 0; i < n; ++i)
			{
				// First control point
				firstControlPoints[i] = new Point(x[i], y[i]);

				// Second control point
				if (i < n - 1)
                    secondControlPoints[i] = new Point(2 * knots[i + 1].X - x[i + 1], 2 * knots[i + 1].Y - y[i + 1]);
				else
                    secondControlPoints[i] = new Point((knots[n].X + x[n - 1]) / 2, (knots[n].Y + y[n - 1]) / 2);
			}
		}

		/// <summary>
		/// Solves a tridiagonal system for one of coordinates (x or y) of first Bezier control points.
		/// </summary>
		/// <param name="rhs">Right hand side vector.</param>
		/// <returns>Solution vector.</returns>
        private static double[] GetFirstControlPoints(double[] rhs, Double tension)
		{
			int n = rhs.Length;
			double[] x = new double[n]; // Solution vector.
			double[] tmp = new double[n]; // Temp workspace.

            double b = tension;
			x[0] = rhs[0] / b;

			for (int i = 1; i < n; i++) // Decomposition and forward substitution.
			{
				tmp[i] = 1 / b;
                b = (i < n - 1 ? 4.0 : 3.5) - tmp[i];
				x[i] = (rhs[i] - x[i - 1]) / b;
			}

			for (int i = 1; i < n; i++)
				x[n - i - 1] -= tmp[n - i] * x[n - i]; // Backsubstitution.

			return x;
		}
	}
}
