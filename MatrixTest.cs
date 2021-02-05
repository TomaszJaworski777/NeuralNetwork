using System;
using System.Collections.Generic;
using System.Text;

namespace MatrixLib
{
    class MatrixTest
    {
        Matrix a;
        Matrix b;

        public bool Test()
        {
            var result = true;

            result = TransposeTest() && ElementWiseMultiplyTest() && ScaleTest();

            return result;
        }

        bool TransposeTest()
        {
            a = new Matrix(2, 3);
            b = new Matrix(3, 2);
            a.Add(2);
            b.Add(2);

            var result = a.Transpose();

            return result.Rows == b.Rows && result.Values[2][1] == b.Values[2][1];
        }
        
        bool ElementWiseMultiplyTest()
        {
            a = new Matrix(2, 2);
            b = new Matrix(2, 2);
            a.Add(2);
            b.Add(3);

            a.Multiply(b);

            return a.Values[1][1] == 6;
        }

        bool ScaleTest()
        {
            a = new Matrix(2, 2);
            a.Add(2);

            a.Multiply(3);

            return a.Values[1][1] == 6;
        }
    }
}
