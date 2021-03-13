#pragma once
#include "stdafx.h"
#include "math.h"
#include <time.h>
#include <iostream>
#include <ctime>
#include <vector>
#include <algorithm>
#include <cctype>
#include <iomanip>
#include <sstream>


class SJH_Matrix
{
public:
	SJH_Matrix();
	~SJH_Matrix();
    double** MatrixCreate(int rows, int cols);
    double* MatrixCreate(int rows);
    double** MatrixIdentity(int dn);
    double** MatrixProduct(double **matrixA,int aRows,int aCols,double**matrixB, int bRows,int bCols);
    double** MatrixInverse(double **matrix,int Rows,int Cols);
    double** MatrixDuplicate(double **matrix,int aRows,int aCols);
    double* MatrixDuplicate(double *matrix,int aRows);
    double* HelperSolve(double **luMatrix,int n, double *b);
    double** MatrixDecompose(double **matrix,int rows,int cols, int *perm , int& toggle);
    double* Matrix_Multiply(double **Matrix_A,int rows,int cols, double *Matrix_B);
};
