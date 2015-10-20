using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gamma
{
    public static class HMath
    {
        /// <summary>
        /// Формула Ланцаша, 15 цифр после запятой
        /// </summary>
        public static double Gamma(double z)
        {
            if (z <= 0.0 && z.IsInt())
                return Double.PositiveInfinity;

            // Lanczos approximation
            // 15 correct decimal places
            int g = 7;
            double[] p = {0.99999999999980993, 676.5203681218851, -1259.1392167224028,
                             771.32342877765313, -176.61502916214059, 12.507343278686905,
                             -0.13857109526572012, 9.9843695780195716e-6, 1.5056327351493116e-7};
            if (z < 0.5)
            {
                // Reflection formula
                return Math.PI / (Math.Sin(Math.PI * z) * Gamma(1 - z));
            }
            else
            {
                z -= 1;
                double x = p[0];
                for (var i = 1; i < g + 2; i++)
                {
                    x += p[i] / (z + i);
                }
                double t = z + g + 0.5;
                return Math.Sqrt(2 * Math.PI) * (Math.Pow(t, z + 0.5)) * Math.Exp(-t) * x;
            }
        }

        /// <summary>
        /// Построить сетку по функции на отрезке [a, b] с точностью eps
        /// </summary>
        public static Function[] ToGrid(Func<double, double> function, double a, double b, double eps)
        {
            if (a >= b)
            {
                throw new ArgumentException("A must be less than B!");
            }

            List<Function> result = new List<Function>();
            for (double x = a; x <= b; x += eps)
            {
                result.Add(new Function { X = x, Fx = function(x) });
            }

            return result.ToArray();
        }

        /// <summary>
        /// Производная первого порядка
        /// </summary>
        public static Function[] Derivative(Function[] function)
        {
            double h = Math.Abs(function[0].X - function[1].X);
            double doubleH = h + h;

            Function[] result = Function.Instance(function.Length);

            for (int i = 1; i < function.Length - 1; i++)
            {
                result[i].X = function[i].X;
                result[i].Fx = (function[i + 1].Fx - function[i - 1].Fx) / (doubleH);
            }

            // TODO тут скрываются большие проблемки...
            // TODO надо что-то с этим будет делать...
            result[0].X = function[0].X;
            result[0].Fx = result[1].Fx;
            result[function.Length - 1].X = function[function.Length - 1].X;
            result[function.Length - 1].Fx = result[function.Length - 2].Fx;

            return result;
        }

        /// <summary>
        /// Производная натурального порядка
        /// </summary>
        public static Function[] Derivative(Function[] function, int exp)
        {
            if (exp <= 0)
            {
                throw new ArgumentException("A must be less than B!");
            }

            Function[] result = Function.Instance(function);
            for (int i = 0; i < exp; i++)
            {
                result = Derivative(result);
            }

            return result;
        }

        /// <summary>
        /// Интеграл определёный
        /// </summary>
        public static double Integrate(Function[] function)
        {
            double h = Math.Abs(function[0].X - function[1].X);

            double sum = 0.0;
            for (int i = 1; i < function.Length - 1; i++)
            {
                sum += function[i].Fx;
            }

            sum += ((function[0].Fx + function[function.Length - 1].Fx) / 2.0);
            sum *= h;

            return sum;
        }

        /// <summary>
        /// Дробная производная
        /// </summary>
        public static Function[] FractionalDerivative(Function[] function, double exp)
        {
            if (exp < 0)
            {
                throw new ArgumentException("Exp can't be less zero!");
            }

            double h = Math.Abs(function[0].X - function[1].X);
            Function[] result = Function.Instance(function);

            if (exp == 0)
            {
                return result;
            }

            int integ = 1;
            if (exp > 1)
            {
                integ += (int) Math.Truncate(exp);
                exp -= (integ - 1);
            }

            for (int i = 0; i < function.Length; i++)
            {
                double x = function[i].X;
                int size = (int) ((x - function[0].X) / h + 1.0);
                Function[] integraFunc = Function.Instance(size);
                if (size == 1)
                {
                    result[0].X = function[0].X;
                    result[0].Fx = 0;
                }
                else
                {
                    for (int j = 0; j < size; j++)
                    {
                        integraFunc[j].X = function[j].X;
                        integraFunc[j].Fx = function[j].Fx / Math.Pow((x - function[j].X), exp);
                    }

                    result[i].X = x;
                    result[i].Fx = Integrate(integraFunc);
                }
            }

            result = Derivative(result, integ);
            double p = (double)integ - exp;
            double k = 1.0 / Gamma(p);

            for (int i = 0; i < result.Length; i++)
            {
                result[i].Fx *= k;
            }

            return result;
        }

    }
}
