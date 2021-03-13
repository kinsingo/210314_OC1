#pragma once

extern "C" __declspec (dllexport) double* CreateMonotoneCubicSpline(int mX_size, int mY_size,double* mX, double* mY);
extern "C" __declspec (dllexport) double interpolate(int mX_size, int mY_size, double* mX, double* mY, double* mM,double x);

