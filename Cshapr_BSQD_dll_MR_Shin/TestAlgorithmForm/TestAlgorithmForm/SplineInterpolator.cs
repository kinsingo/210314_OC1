using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAlgorithmForm
{
	public class SplineInterpolator
	{
        private double[] mX;
		private double[] mY;
		private double[] mM;

		private SplineInterpolator(double[] x, double[] y, double[] m)
		{
			mX = x;
			mY = y;
			mM = m;
		}

		public static SplineInterpolator createMonotoneCubicSpline(double[] x, double[] y)
		{

			if (x == null || y == null || x.Count() != y.Count() || x.Count() < 2)
				throw new Exception("There must be at least two control points and the arrays must be of equal length.");

			int n = x.Count();
			double[] d = new double[n - 1]; // could optimize this out 
			double[] m = new double[n];

			// Compute slopes of secant lines between successive points.
			for (int i = 0; i < n - 1; i++)
			{
				if ((x[i + 1] - x[i]) <= 0)
					throw new Exception("The control points must all have strictly increasing X values.");

				d[i] = (y[i + 1] - y[i]) / (x[i + 1] - x[i]);
			}

			// Initialize the tangents as the average of the secants.
			m[0] = d[0];
			m[n - 1] = d[n - 2];
			for (int i = 1; i < n - 1; i++)
				m[i] = (d[i - 1] + d[i]) * 0.5f;

		
			// Update the tangents to preserve monotonicity.
			for (int i = 0; i < n - 1; i++)
			{
				// successive Y values are equal
				if (d[i] == 0)
				{ 
					m[i] = 0;
					m[i + 1] = 0;
				}
				else
				{
					double a = m[i] / d[i];
					double b = m[i + 1] / d[i];
					double h = Math.Sqrt(Math.Pow(a,2) + Math.Pow(b, 2)); 
					
					if (h > 9)
					{
						double t = 3 / h;
						m[i] = t * a * d[i];
						m[i + 1] = t * b * d[i];
					}

				}

			}
			return new SplineInterpolator(x, y, m);
		}



		/**

		 * Interpolates the value of Y = f(X) for given X. Clamps X to the domain of the spline.


		 * @param x

		 *            The X value.

		 * @return The interpolated Y = f(X) value.

		 */

		public double interpolate(double x)
		{

			// Handle the boundary cases.

			int n = mX.Count();

			if (double.IsNaN(x))
				return x;

			if (x <= mX[0])
				return mY[0];

			if (x >= mX[n - 1])
				return mY[n - 1];

			// Find the index 'i' of the last point with smaller X.
			// We know this will be within the spline due to the boundary tests.
			int i = 0;
			while (x >= mX[i + 1])
			{
				i += 1;
				if (x == mX[i])
					return mY[i];
			}

			// Perform cubic Hermite spline interpolation
			double h = mX[i + 1] - mX[i];
			double t = (x - mX[i]) / h;

			return (mY[i] * (1 + 2 * t) + h * mM[i] * t) * (1 - t) * (1 - t)

					+ (mY[i + 1] * (3 - 2 * t) + h * mM[i + 1] * (t - 1)) * t * t;
		}
	}
}
