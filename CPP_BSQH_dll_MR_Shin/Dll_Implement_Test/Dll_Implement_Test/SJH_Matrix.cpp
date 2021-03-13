#include "stdafx.h"
#include "SJH_Matrix.h"

using namespace std;

	SJH_Matrix::SJH_Matrix()
	{
	}
	SJH_Matrix::~SJH_Matrix()
	{
	}

	double** SJH_Matrix::MatrixCreate(int rows, int cols)
	{
        double** result = new double*[rows];
        for (int i = 0; i < rows; ++i)
        result[i] = new double[cols];
        return result;
	}
	

    double* SJH_Matrix::MatrixCreate(int rows)
	{
	    double* result = new double[rows];
        return result;
	}

    double** SJH_Matrix::MatrixIdentity(int n)
	{
        // return an n x n Identity matrix
        double** result = MatrixCreate(n, n);
        for (int i = 0; i < n; ++i) result[i][i] = 1.0;
        return result;
	}

	double** SJH_Matrix::MatrixProduct(double **matrixA,int aRows,int aCols,double**matrixB, int bRows,int bCols)
	{
            //int aRows = sizeof matrixA / sizeof matrixA[0];   
			//int aCols = sizeof matrixA[0] / sizeof(double); 
            
			//int bRows = sizeof matrixB / sizeof matrixB[0];
			//int bCols = sizeof matrixB[0] / sizeof(double); 

            if (aCols != bRows)
                throw new exception("Non-conformable matrices in MatrixProduct");

            double** result = MatrixCreate(aRows, bCols);

            for (int i = 0; i < aRows; ++i) // each row of A
                for (int j = 0; j < bCols; ++j) // each col of B
                    for (int k = 0; k < aCols; ++k) // could use k less-than bRows
                        result[i][j] += matrixA[i][k] * matrixB[k][j];

            return result;
	}

	double** SJH_Matrix::MatrixInverse(double **matrix,int Rows,int Cols)
	{
            int n = Rows;
            double** result = MatrixDuplicate(matrix,Rows,Cols);
            //int* perm;
			int* perm = new int[n]; // set up row permutation result
            for (int i = 0; i < n; ++i) { perm[i] = i; }
			int toggle;

            double** lum = MatrixDecompose(matrix,Rows,Cols,perm,toggle);
            if (lum == nullptr)
                throw new exception("Unable to compute inverse");

            double* b = new double[n];
            for (int i = 0; i < n; ++i)
            {
                for (int j = 0; j < n; ++j)
                {
                    if (i == perm[j])
                        b[j] = 1.0;
                    else
                        b[j] = 0.0;
                }

                double* x = HelperSolve(lum,n, b);

                for (int j = 0; j < n; ++j)
                    result[j][i] = x[j];
            }
            return result;
	}

	double** SJH_Matrix::MatrixDuplicate(double **matrix,int aRows,int aCols)
	{
            // allocates/creates a duplicate of a matrix.
		    //int aRows = sizeof matrix / sizeof matrix[0];   //length
			//int aCols = sizeof matrix[0] / sizeof(double);   

            double** result = MatrixCreate(aRows, aCols);
            for (int i = 0; i < aRows; ++i) // copy the values
                for (int j = 0; j < aCols; ++j)
                    result[i][j] = matrix[i][j];
            return result;
	}


	double* SJH_Matrix::MatrixDuplicate(double *matrix,int aRows)
	{
		    //int aRows = sizeof matrix / sizeof matrix[0];   //length
            // allocates/creates a duplicate of a matrix.
            double* result = MatrixCreate(aRows);
            for (int i = 0; i < aRows; ++i) // copy the value
                    result[i] = matrix[i];
            return result;
	}

	
	double* SJH_Matrix::HelperSolve(double **luMatrix,int n, double *b)
	{
            // before calling this helper, permute b using the perm array
            // from MatrixDecompose that generated luMatrix
            //int n = sizeof luMatrix / sizeof luMatrix[0];   //length
            double* x = new double[n];
            for(int i=0;i<n;i++)x[i] = b[i];//b.CopyTo(x, 0);
			
            for (int i = 1; i < n; ++i)
            {
                double sum = x[i];
                for (int j = 0; j < i; ++j)
                    sum -= luMatrix[i][j] * x[j];
                x[i] = sum;
            }

            x[n - 1] /= luMatrix[n - 1][n - 1];
            for (int i = n - 2; i >= 0; --i)
            {
                double sum = x[i];
                for (int j = i + 1; j < n; ++j)
                    sum -= luMatrix[i][j] * x[j];
                x[i] = sum / luMatrix[i][i];
            }
            return x;
	}


    double** SJH_Matrix::MatrixDecompose(double **matrix,int rows,int cols, int *perm, int& toggle)
	{
            // Doolittle LUP decomposition with partial pivoting.
            // rerturns: result is L (with 1s on diagonal) and U;
            // perm holds row permutations; toggle is +1 or -1 (even or odd)
            //int rows = sizeof matrix / sizeof matrix[0];   //length
            //int cols = sizeof matrix[0] / sizeof(double);   
		    int n = rows;

            if (rows != cols)
                throw new exception("Attempt to decompose a non-square m");

            double** result = MatrixDuplicate(matrix,rows,cols);

            //perm = new int[n]; // set up row permutation result
            //for (int i = 0; i < n; ++i) { perm[i] = i; }

            toggle = 1; // toggle tracks row swaps.
            // +1 -greater-than even, -1 -greater-than odd. used by MatrixDeterminant

            for (int j = 0; j < n - 1; ++j) // each column
            {
                double colMax = abs(result[j][j]); // find largest val in col
                int pRow = j;
                //for (int i = j + 1; i less-than n; ++i)
                //{
                //  if (result[i][j] greater-than colMax)
                //  {
                //    colMax = result[i][j];
                //    pRow = i;
                //  }
                //}

                // reader Matt V needed this:
                for (int i = j + 1; i < n; ++i)
                {
                    if (abs(result[i][j]) > colMax)
                    {
                        colMax = abs(result[i][j]);
                        pRow = i;
                    }
                }
                // Not sure if this approach is needed always, or not.

                if (pRow != j) // if largest value not on pivot, swap rows
                {
                    double* rowPtr = result[pRow];
                    result[pRow] = result[j];
                    result[j] = rowPtr;

                    int tmp = perm[pRow]; // and swap perm info
                    perm[pRow] = perm[j];
                    perm[j] = tmp;

                    toggle = -toggle; // adjust the row-swap toggle
                }

                // --------------------------------------------------
                // This part added later (not in original)
                // and replaces the 'return null' below.
                // if there is a 0 on the diagonal, find a good row
                // from i = j+1 down that doesn't have
                // a 0 in column j, and swap that good row with row j
                // --------------------------------------------------

                if (result[j][j] == 0.0)
                {
                    // find a good row to swap
                    int goodRow = -1;
                    for (int row = j + 1; row < n; ++row)
                    {
                        if (result[row][j] != 0.0)
                            goodRow = row;
                    }

                    if (goodRow == -1)
                        throw new exception("Cannot use Doolittle's method");

                    // swap rows so 0.0 no longer on diagonal
                    double* rowPtr = result[goodRow];
                    result[goodRow] = result[j];
                    result[j] = rowPtr;

                    int tmp = perm[goodRow]; // and swap perm info
                    perm[goodRow] = perm[j];
                    perm[j] = tmp;

                    toggle = -toggle; // adjust the row-swap toggle
                }
                // --------------------------------------------------
                // if diagonal after swap is zero . .
                //if (Math.Abs(result[j][j]) less-than 1.0E-20) 
                //  return null; // consider a throw

                for (int i = j + 1; i < n; ++i)
                {
                    result[i][j] /= result[j][j];
                    for (int k = j + 1; k < n; ++k)
                    {
                        result[i][k] -= result[i][j] * result[j][k];
                    }
                }
            } // main j column loop
            return result;
	}

	double* SJH_Matrix::Matrix_Multiply(double **Matrix_A,int rows,int cols, double *Matrix_B)
	{
            // C = A*B
		    //int rows = sizeof Matrix_A / sizeof Matrix_A[0];   //length
            //int cols = sizeof Matrix_A[0] / sizeof(double);   

            double** A = MatrixDuplicate(Matrix_A,rows,cols);
            double* B = MatrixDuplicate(Matrix_B,rows);
            double* C = MatrixCreate(rows);
            
            for (int i = 0; i < rows; i++)
            {
                C[i] = 0;
                for (int j = 0; j < cols; j++)
                {
                    C[i] += A[i][j] * B[j];
                }
            }
            return C;
	}
