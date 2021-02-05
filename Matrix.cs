using System;
using UnityEngine;

namespace MatrixLib
{
    public struct Matrix
    {
        public int Rows { get; private set; }
        public int Columns { get; private set; }
        public float[][] Values { get; set; }

        public Matrix(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;

            Values = new float[rows][];

            for (int i = 0; i < Rows; i++)
            {
                Values[i] = new float[columns];

                for (int j = 0; j < Columns; j++)
                {
                    Values[i][j] = 0;
                }
            }
        }

        public static Matrix FromArray(float[] x)
        {
            var result = new Matrix(x.Length, 1);

            for (int i = 0; i < x.Length; i++)
            {
                result.Values[i][0] = x[i];
            }

            return result;
        }

        public static float[] ToArray(Matrix x)
        {
            var result = new float[x.Rows];

            for (int i = 0; i < x.Rows; i++)
            {
                for (int j = 0; j < x.Columns; j++)
                {
                    result[i] = x.Values[i][j];
                }
            }

            return result;
        }

        public float[] ToArray()
        {
            var result = new float[Rows];

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    result[i] = Values[i][j];
                }
            }

            return result;
        }

        public void Randomize()
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    //var random = new Random(i * j);
                    Values[i][j] = UnityEngine.Random.Range(-1f, 1f);//((float)random.NextDouble() * 2) - 1;
                }
            }
        }

        public void Multiply(float x)
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    Values[i][j] *= x;
                }
            }
        }

        public void Multiply(Matrix x)
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    Values[i][j] *= x.Values[i][j];
                }
            }
        }

        public static Matrix Multiply(Matrix a, Matrix b)
        {
            if (a.Columns != b.Rows)
                return Empty;

            var result = new Matrix(a.Rows, b.Columns);

            for (int i = 0; i < result.Rows; i++)
            {             
                for (int j = 0; j < result.Columns; j++)
                {
                    var value = 0f;

                    for (int k = 0; k < a.Columns; k++)
                    {
                        value += a.Values[i][k] * b.Values[k][j];
                    }

                    result.Values[i][j] = value;
                }
            }

            return result;
        }

        public void Add(float x)
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    Values[i][j] += x;
                }
            }
        }

        public void Add(Matrix x)
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    Values[i][j] += x.Values[i][j];
                }
            }
        }

        public static Matrix Add(Matrix a, Matrix b)
        {
            var result = new Matrix(a.Rows, a.Columns);

            for (int i = 0; i < a.Rows; i++)
            {
                for (int j = 0; j < a.Columns; j++)
                {
                    result.Values[i][j] = a.Values[i][j] + b.Values[i][j];
                }
            }

            return result;
        }

        public void Subtract(float x)
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    Values[i][j] -= x;
                }
            }
        }

        public void Subtract(Matrix x)
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    Values[i][j] -= x.Values[i][j];
                }
            }
        }

        public static Matrix Subtract(Matrix a, Matrix b)
        {
            var result = new Matrix(a.Rows, a.Columns);

            for (int i = 0; i < a.Rows; i++)
            {
                for (int j = 0; j < a.Columns; j++)
                {
                    result.Values[i][j] = a.Values[i][j] - b.Values[i][j];
                }
            }

            return result;
        }

        public static Matrix Transpose(Matrix x)
        {
            var result = new Matrix(x.Columns, x.Rows);

            for (int i = 0; i < x.Rows; i++)
            {
                for (int j = 0; j < x.Columns; j++)
                {
                    result.Values[j][i] += x.Values[i][j];
                }
            }

            return result;
        }

        public Matrix Transpose()
        {
            var result = new Matrix(Columns, Rows);

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    result.Values[j][i] += Values[i][j];
                }
            }

            return result;
        }

        public void Map(Func<float, float> function)
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    Values[i][j] = function(Values[i][j]);
                }
            }
        }

        public static Matrix Map(Matrix x, Func<float, float> function)
        {
            var result = new Matrix(x.Rows, x.Columns);

            for (int i = 0; i < x.Rows; i++)
            {
                for (int j = 0; j < x.Columns; j++)
                {
                    result.Values[i][j] = function(x.Values[i][j]);
                }
            }

            return result;
        }

        public void Map(Func<int, int, float> function)
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    Values[i][j] = function(i, j);
                }
            }
        }

        public void Map(Func<float, int, int, float> function)
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    Values[i][j] = function(Values[i][j], i, j);
                }
            }
        }

        public void Map(Func<float> function)
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    Values[i][j] = function();
                }
            }
        }

        public void Print()
        {
            for (int i = 0; i < Columns; i++)
            {
                Console.Write("================");
            }
            Console.WriteLine();

            for (int i = 0; i < Rows; i++)
            {
                Console.Write("|");
                for (int j = 0; j < Columns; j++)
                {
                    Console.Write("{0, 14}|", Values[i][j]);
                }
                Console.WriteLine();
            }

            for (int i = 0; i < Columns; i++)
            {
                Console.Write("================");
            }
            Console.WriteLine();
        }

        public static Matrix Empty { get => new Matrix(2, 3); }
    }
}
