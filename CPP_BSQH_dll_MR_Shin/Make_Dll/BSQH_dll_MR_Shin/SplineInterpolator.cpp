#include "stdafx.h"
#include "SplineInterpolator.h"
#include <stdexcept>

using namespace std;


extern "C" __declspec (dllexport) double* CreateMonotoneCubicSpline(int mX_size, int mY_size, double* mX, double* mY)
{
	if (mX == NULL || mY == NULL || mX_size != mY_size || mX_size < 2)
		throw exception("There must be at least two control points and the arrays must be of equal length.");

	int n = mX_size;
	double* d = new double[n - 1]; // could optimize this out 
	double* m = new double[n];


	// Compute slopes of secant lines between successive points.
	for (int i = 0; i < n - 1; i++)
	{
		if ((mX[i + 1] - mX[i]) <= 0)
			throw exception("The control points must all have strictly increasing X values.");

		d[i] = (mY[i + 1] - mY[i]) / (mX[i + 1] - mX[i]);
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
			double h = sqrt(pow(a, 2) + pow(b, 2));

			if (h > 9)
			{
				double t = 3 / h;
				m[i] = t * a * d[i];
				m[i + 1] = t * b * d[i];
			}

		}

	}

	delete[] d;
	return m;
}

extern "C" __declspec (dllexport) double interpolate(int mX_size, int mY_size, double* mX, double* mY, double* mM, double x)
{
	// Handle the boundary cases.
	int n = mX_size;

	//if (isnan(x))
    //	return x;

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
