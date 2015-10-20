using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace Gamma
{
    class Program
    {
        static void Main(string[] args)
        {
            Func<double, double> fx = x => x * x;
            Function[] grid = HMath.ToGrid(fx, 2, 4, 0.001);

            Function[] result = HMath.FractionalDerivative(grid, 0.5);
            for (int i = 0; i < result.Length; i++)
            {
                Console.WriteLine("({0:0.00};{1:0.00})", result[i].X, result[i].Fx);
            }

        } 
    }
}
