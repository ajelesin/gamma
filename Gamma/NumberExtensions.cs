using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gamma
{
    public static class NumberExtensions
    {
        /// <summary>
        /// Является ли дробное число целым?
        /// </summary>
        public static bool IsInt(this double value)
        {
            return Math.Truncate(value) == value;
        }

        /// <summary>
        /// Является ли это число почти целым? :)
        /// </summary>
        public static bool IsInt(this double value, double eps)
        {
            return Math.Abs(Math.Truncate(value) - value) <= eps
                || Math.Abs(Math.Truncate(value + 1) - value) <= eps;
        }

    }
}
