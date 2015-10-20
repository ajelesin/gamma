using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gamma
{
    public class Function
    {
        public double X { get; set; }
        public double Fx { get; set; }

        public static Function[] Instance(int size)
        {
            Function[] function = new Function[size];
            for (int i = 0; i < size; i++)
            {
                function[i] = new Function();
            }

            return function;
        }

        public static Function[] Instance(Function[] other)
        {
            Function[] function = new Function[other.Length];
            for (int i = 0; i < other.Length; i ++)
            {
                function[i] = new Function { X = other[i].X, Fx = other[i].Fx };
            }

            return function;
        }
    }
}
