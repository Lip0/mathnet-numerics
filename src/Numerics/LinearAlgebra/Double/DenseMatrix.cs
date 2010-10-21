﻿// <copyright file="DenseMatrix.cs" company="Math.NET">
// Math.NET Numerics, part of the Math.NET Project
// http://numerics.mathdotnet.com
// http://github.com/mathnet/mathnet-numerics
// http://mathnetnumerics.codeplex.com
// Copyright (c) 2009-2010 Math.NET
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
// </copyright>

namespace MathNet.Numerics.LinearAlgebra.Double
{
    using System;
    using Distributions;
    using Generic;
    using Properties;
    using Threading;

    /// <summary>
    /// A Matrix class with dense storage. The underlying storage is a one dimensional array in column-major order.
    /// </summary>
    public class DenseMatrix : Matrix
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DenseMatrix"/> class. This matrix is square with a given size.
        /// </summary>
        /// <param name="order">the size of the square matrix.</param>
        /// <exception cref="ArgumentException">
        /// If <paramref name="order"/> is less than one.
        /// </exception>
        public DenseMatrix(int order)
            : base(order)
        {
            Data = new double[order * order];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DenseMatrix"/> class.
        /// </summary>
        /// <param name="rows">
        /// The number of rows.
        /// </param>
        /// <param name="columns">
        /// The number of columns.
        /// </param>
        public DenseMatrix(int rows, int columns)
            : base(rows, columns)
        {
            Data = new double[rows * columns];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DenseMatrix"/> class with all entries set to a particular value.
        /// </summary>
        /// <param name="rows">
        /// The number of rows.
        /// </param>
        /// <param name="columns">
        /// The number of columns.
        /// </param>
        /// <param name="value">The value which we assign to each element of the matrix.</param>
        public DenseMatrix(int rows, int columns, double value)
            : base(rows, columns)
        {
            Data = new double[rows * columns];
            for (var i = 0; i < Data.Length; i++)
            {
                Data[i] = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DenseMatrix"/> class from a one dimensional array. This constructor
        /// will reference the one dimensional array and not copy it.
        /// </summary>
        /// <param name="rows">The number of rows.</param>
        /// <param name="columns">The number of columns.</param>
        /// <param name="array">The one dimensional array to create this matrix from. This array should store the matrix in column-major order. <seealso cref="http://en.wikipedia.org/wiki/Row-major_order"/></param>
        public DenseMatrix(int rows, int columns, double[] array)
            : base(rows, columns)
        {
            Data = array;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DenseMatrix"/> class from a 2D array. This constructor
        /// will allocate a completely new memory block for storing the dense matrix.
        /// </summary>
        /// <param name="array">The 2D array to create this matrix from.</param>
        public DenseMatrix(double[,] array)
            : base(array.GetLength(0), array.GetLength(1))
        {
            var rows = array.GetLength(0);
            var columns = array.GetLength(1);
            Data = new double[rows * columns];
            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < columns; j++)
                {
                    Data[(j * rows) + i] = array[i, j];
                }
            }
        }

        /// <summary>
        /// Gets the matrix's data.
        /// </summary>
        /// <value>The matrix's data.</value>
        internal double[] Data
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates a <c>DenseMatrix</c> for the given number of rows and columns.
        /// </summary>
        /// <param name="numberOfRows">
        /// The number of rows.
        /// </param>
        /// <param name="numberOfColumns">
        /// The number of columns.
        /// </param>
        /// <returns>
        /// A <c>DenseMatrix</c> with the given dimensions.
        /// </returns>
        public override Matrix<double> CreateMatrix(int numberOfRows, int numberOfColumns)
        {
            return new DenseMatrix(numberOfRows, numberOfColumns);
        }

        /// <summary>
        /// Creates a <see cref="Vector{T}"/> with a the given dimension.
        /// </summary>
        /// <param name="size">The size of the vector.</param>
        /// <returns>
        /// A <see cref="Vector{T}"/> with the given dimension.
        /// </returns>
        public override Vector<double> CreateVector(int size)
        {
            return new DenseVector(size);
        }

        /// <summary>
        /// Retrieves the requested element without range checking.
        /// </summary>
        /// <param name="row">
        /// The row of the element.
        /// </param>
        /// <param name="column">
        /// The column of the element.
        /// </param>
        /// <returns>
        /// The requested element.
        /// </returns>
        public override double At(int row, int column)
        {
            return Data[(column * RowCount) + row];
        }

        /// <summary>
        /// Sets the value of the given element.
        /// </summary>
        /// <param name="row">
        /// The row of the element.
        /// </param>
        /// <param name="column">
        /// The column of the element.
        /// </param>
        /// <param name="value">
        /// The value to set the element to.
        /// </param>
        public override void At(int row, int column, double value)
        {
            Data[(column * RowCount) + row] = value;
        }

        /// <summary>
        /// Sets all values to zero.
        /// </summary>
        public override void Clear()
        {
            Array.Clear(Data, 0, Data.Length);
        }

        /// <summary>
        /// Returns the transpose of this matrix.
        /// </summary>        
        /// <returns>The transpose of this matrix.</returns>
        public override Matrix<double> Transpose()
        {
            var ret = new DenseMatrix(ColumnCount, RowCount);
            for (var j = 0; j < ColumnCount; j++)
            {
                var index = j * RowCount;
                for (var i = 0; i < RowCount; i++)
                {
                    ret.Data[(i * ColumnCount) + j] = Data[index + i];
                }
            }

            return ret;
        }

        /// <summary>Calculates the L1 norm.</summary>
        /// <returns>The L1 norm of the matrix.</returns>
        public override double L1Norm()
        {
            var norm = 0.0;
            for (var j = 0; j < ColumnCount; j++)
            {
                var s = 0.0;
                for (var i = 0; i < RowCount; i++)
                {
                    s += Math.Abs(Data[(j * RowCount) + i]);
                }

                norm = Math.Max(norm, s);
            }

            return norm;
        }

        /// <summary>Calculates the Frobenius norm of this matrix.</summary>
        /// <returns>The Frobenius norm of this matrix.</returns>
        public override double FrobeniusNorm()
        {
            var transpose = (DenseMatrix)Transpose();
            var aat = (DenseMatrix) (this * transpose);

            var norm = 0.0;
            for (var i = 0; i < RowCount; i++)
            {
                norm += Math.Abs(aat.Data[(i * RowCount) + i]);
            }

            norm = Math.Sqrt(norm);
            return norm;
        }

        /// <summary>Calculates the infinity norm of this matrix.</summary>
        /// <returns>The infinity norm of this matrix.</returns>  
        public override double InfinityNorm()
        {
            var norm = 0.0;
            for (var i = 0; i < RowCount; i++)
            {
                var s = 0.0;
                for (var j = 0; j < ColumnCount; j++)
                {
                    s += Math.Abs(Data[(j * RowCount) + i]);
                }

                norm = Math.Max(norm, s);
            }

            return norm;
        }

        #region Elementary operations

        /// <summary>
        /// Adds another matrix to this matrix.
        /// </summary>
        /// <param name="other">The matrix to add to this matrix.</param>
        /// <param name="result">The matrix to store the result of add</param>
        /// <exception cref="ArgumentNullException">If the other matrix is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If the two matrices don't have the same dimensions.</exception>
        protected override void DoAdd(Matrix<double> other, Matrix<double> result)
        {
            var denseOther = other as DenseMatrix;
            var denseResult = result as DenseMatrix;
            if (denseOther == null || denseResult == null)
            {
                base.DoAdd(other, result);
            }
            else
            {
                Control.LinearAlgebraProvider.AddArrays(Data, denseOther.Data, denseResult.Data);
            }
        }

        /// <summary>
        /// Subtracts another matrix from this matrix.
        /// </summary>
        /// <param name="other">The matrix to subtract.</param>
        /// <param name="result">The matrix to store the result of the subtraction.</param>
        protected override void DoSubtract(Matrix<double> other, Matrix<double> result)
        {
            var denseOther = other as DenseMatrix;
            var denseResult = result as DenseMatrix;
            if (denseOther == null || denseResult == null)
            {
                base.DoSubtract(other, result);
            }
            else
            {
                Control.LinearAlgebraProvider.SubtractArrays(Data, denseOther.Data, denseResult.Data);
            }
        }
    
        #endregion

        #region Static constructors for special matrices.

        /// <summary>
        /// Initializes a square <see cref="DenseMatrix"/> with all zero's except for ones on the diagonal.
        /// </summary>
        /// <param name="order">the size of the square matrix.</param>
        /// <returns>A dense identity matrix.</returns>
        /// <exception cref="ArgumentException">
        /// If <paramref name="order"/> is less than one.
        /// </exception>
        public static DenseMatrix Identity(int order)
        {
            var m = new DenseMatrix(order);
            for (var i = 0; i < order; i++)
            {
                m[i, i] = 1.0;
            }

            return m;
        }

        #endregion

        /// <summary>
        /// Generates matrix with random elements.
        /// </summary>
        /// <param name="numberOfRows">Number of rows.</param>
        /// <param name="numberOfColumns">Number of columns.</param>
        /// <param name="distribution">Continuous Random Distribution or Source</param>
        /// <returns>
        /// An <c>numberOfRows</c>-by-<c>numberOfColumns</c> matrix with elements distributed according to the provided distribution.
        /// </returns>
        /// <exception cref="ArgumentException">If the parameter <paramref name="numberOfRows"/> is not positive.</exception>
        /// <exception cref="ArgumentException">If the parameter <paramref name="numberOfColumns"/> is not positive.</exception>
        public override Matrix<double> Random(int numberOfRows, int numberOfColumns, IContinuousDistribution distribution)
        {
            if (numberOfRows < 1)
            {
                throw new ArgumentException(Resources.ArgumentMustBePositive, "numberOfRows");
            }

            if (numberOfColumns < 1)
            {
                throw new ArgumentException(Resources.ArgumentMustBePositive, "numberOfColumns");
            }

            var matrix = CreateMatrix(numberOfRows, numberOfColumns);
            CommonParallel.For(
                0,
                ColumnCount,
                j =>
                {
                    for (var i = 0; i < matrix.RowCount; i++)
                    {
                        matrix[i, j] = distribution.Sample();
                    }
                });

            return matrix;
        }

        /// <summary>
        /// Generates matrix with random elements.
        /// </summary>
        /// <param name="numberOfRows">Number of rows.</param>
        /// <param name="numberOfColumns">Number of columns.</param>
        /// <param name="distribution">Continuous Random Distribution or Source</param>
        /// <returns>
        /// An <c>numberOfRows</c>-by-<c>numberOfColumns</c> matrix with elements distributed according to the provided distribution.
        /// </returns>
        /// <exception cref="ArgumentException">If the parameter <paramref name="numberOfRows"/> is not positive.</exception>
        /// <exception cref="ArgumentException">If the parameter <paramref name="numberOfColumns"/> is not positive.</exception>
        public override Matrix<double> Random(int numberOfRows, int numberOfColumns, IDiscreteDistribution distribution)
        {
            if (numberOfRows < 1)
            {
                throw new ArgumentException(Resources.ArgumentMustBePositive, "numberOfRows");
            }

            if (numberOfColumns < 1)
            {
                throw new ArgumentException(Resources.ArgumentMustBePositive, "numberOfColumns");
            }

            var matrix = CreateMatrix(numberOfRows, numberOfColumns);
            CommonParallel.For(
                0,
                ColumnCount,
                j =>
                {
                    for (var i = 0; i < matrix.RowCount; i++)
                    {
                        matrix[i, j] = distribution.Sample();
                    }
                });

            return matrix;
        }

        /// <summary>
        /// Returns the conjugate transpose of this matrix.
        /// </summary>        
        /// <returns>The conjugate transpose of this matrix.</returns>
        public override Matrix<double> ConjugateTranspose()
        {
            return Transpose();
        }

      /*      Control.LinearAlgebraProvider.MatrixMultiplyWithUpdate(
                Algorithms.LinearAlgebra.Transpose.DontTranspose,
                Algorithms.LinearAlgebra.Transpose.Transpose,
                1.0,
                Data,
                RowCount,
                ColumnCount,
                otherDense.Data,
                otherDense.RowCount,
                otherDense.ColumnCount,
                1.0,
                resultDense.Data);
        */

        /// <summary>
        /// Multiplies each element of the matrix by a scalar and places results into the result matrix.
        /// </summary>
        /// <param name="scalar">The scalar to multiply the matrix with.</param>
        /// <param name="result">The matrix to store the result of the multiplication.</param>
        protected override void DoMultiply(double scalar, Matrix<double> result)
        {
            var denseResult = result as DenseMatrix;
            if (denseResult == null)
            {
                base.DoMultiply(scalar, result);
            }
            else
            {
                Control.LinearAlgebraProvider.ScaleArray(scalar, Data);
            }
        }

        /// <summary>
        /// Multiplies this matrix with a vector and places the results into the result vector.
        /// </summary>
        /// <param name="rightSide">The vector to multiply with.</param>
        /// <param name="result">The result of the multiplication.</param>
        protected override void DoMultiply(Vector<double> rightSide, Vector<double> result)
        {
            CommonParallel.For(
                0,
                RowCount,
                i =>
                {
                    var s = 0.0;
                    for (var j = 0; j != ColumnCount; j++)
                    {
                        s += At(i, j) * rightSide[j];
                    }

                    result[i] = s;
                });
        }

        /// <summary>
        /// Left multiply a matrix with a vector ( = vector * matrix ) and place the result in the result vector.
        /// </summary>
        /// <param name="leftSide">The vector to multiply with.</param>
        /// <param name="result">The result of the multiplication.</param>
        protected override void DoLeftMultiply(Vector<double> leftSide, Vector<double> result)
        {
            CommonParallel.For(
                0,
                RowCount,
                j =>
                {
                    var s = 0.0;
                    for (var i = 0; i != leftSide.Count; i++)
                    {
                        s += leftSide[i] * At(i, j);
                    }

                    result[j] = s;
                });
        }

        /// <summary>
        /// Multiplies this matrix with another matrix and places the results into the result matrix.
        /// </summary>
        /// <param name="other">The matrix to multiply with.</param>
        /// <param name="result">The result of the multiplication.</param>
        protected override void DoMultiply(Matrix<double> other, Matrix<double> result)
        {
            var denseOther = other as DenseMatrix;
            var denseResult = result as DenseMatrix;

            if (denseOther == null || denseResult == null)
            {
                base.DoMultiply(other, result);
            }
            else
            {
                CommonParallel.For(
                0,
                RowCount,
                j =>
                {
                    for (var i = 0; i != other.ColumnCount; i++)
                    {
                        var s = 0.0;
                        for (var l = 0; l < ColumnCount; l++)
                        {
                            s += Data[(j * RowCount) + l] * denseOther.Data[(i * RowCount) + l];
                        }

                        result.At(j, i, s);
                    }
                });

                CommonParallel.For(
                    0,
                    RowCount,
                    j =>
                    {
                        for (var i = 0; i < RowCount; i++)
                        {
                            var s = 0.0;
                            for (var l = 0; l < ColumnCount; l++)
                            {
                                s += Data[(j * RowCount) + l] * denseOther.Data[(l * RowCount) + j];
                            }

                            denseResult.Data[(j * RowCount) + i] *= s;
                        }
                    });
            }
            }

        /// <summary>
        /// Multiplies this matrix with transpose of another matrix and places the results into the result matrix.
        /// </summary>
        /// <param name="other">The matrix to multiply with.</param>
        /// <param name="result">The result of the multiplication.</param>
        protected override void DoTransposeAndMultiply(Matrix<double> other, Matrix<double> result)
        {
             var denseOther = other as DenseMatrix;
            var denseResult = result as DenseMatrix;

            if (denseOther == null || denseResult == null)
            {
                base.DoTransposeAndMultiply(other, result);
            }
            else
            {
                CommonParallel.For(
                    0,
                    RowCount,
                    j =>
                    {
                        for (var i = 0; i < RowCount; i++)
                        {
                            var s = 0.0;
                            for (var l = 0; l < ColumnCount; l++)
                            {
                                s += Data[(j * RowCount) + l] * denseOther.Data[(l * RowCount) + j];
                            }

                            denseResult.Data[(j * RowCount) + i] *= s;
                        }
                    });
            }
        }

        /// <summary>
        /// Negate each element of this matrix and place the results into the result matrix.
        /// </summary>
        /// <param name="result">The result of the negation.</param>
        protected override void DoNegate(Matrix<double> result)
        {
            var denseResult = result as DenseMatrix;

            if (denseResult == null)
            {
                base.DoNegate(result);
            }
            else
            {
                CommonParallel.For(
                    0,
                    RowCount,
                    i =>
                    {
                        for (var j = 0; j != ColumnCount; j++)
                        {
                            var index = (j * RowCount) + i;
                            denseResult.Data[index] =- Data[index];
                        }
                    });
            }
        }

        /// <summary>
        /// Pointwise multiplies this matrix with another matrix and stores the result into the result matrix.
        /// </summary>
        /// <param name="other">The matrix to pointwise multiply with this one.</param>
        /// <param name="result">The matrix to store the result of the pointwise multiplication.</param>
        protected override void DoPointwiseMultiply(Matrix<double> other, Matrix<double> result)
        {
            var denseOther = other as DenseMatrix;
            var denseResult = result as DenseMatrix;

            if (denseOther == null || denseResult == null)
            {
                base.DoPointwiseMultiply(other, result);
            }
            else
            {
                CommonParallel.For(
                    0,
                    ColumnCount,
                    j =>
                    {
                        for (var i = 0; i < RowCount; i++)
                        {
                            var index = (j * RowCount) + i;
                            denseResult.Data[index] = Data[index] * denseOther.Data[index];

                        }
                    });
            }
        }

        /// <summary>
        /// Pointwise divide this matrix by another matrix and stores the result into the result matrix.
        /// </summary>
        /// <param name="other">The matrix to pointwise divide this one by.</param>
        /// <param name="result">The matrix to store the result of the pointwise division.</param>
        protected override void DoPointwiseDivide(Matrix<double> other, Matrix<double> result)
        {
            var denseOther = other as DenseMatrix;
            var denseResult = result as DenseMatrix;

            if (denseOther == null || denseResult == null)
            {
                base.DoPointwiseDivide(other, result);
            }
            else
            {
                CommonParallel.For(
                    0,
                    ColumnCount,
                    j =>
                    {
                        for (var i = 0; i < RowCount; i++)
                        {
                            var index = (j * RowCount) + i;
                            denseResult.Data[index] = Data[index] / denseOther.Data[index];
                        }
                    });
            }
        }

        /// <summary>
        /// Computes the trace of this matrix.
        /// </summary>
        /// <returns>The trace of this matrix</returns>
        /// <exception cref="ArgumentException">If the matrix is not square</exception>
        public override double Trace()
        {
            if (RowCount != ColumnCount)
            {
                throw new ArgumentException(Resources.ArgumentMatrixSquare);
            }

            return CommonParallel.Aggregate(0, RowCount, i => Data[(i * RowCount) + i]);
        }
    }
}
