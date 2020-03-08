using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebLeague.Common
{
    public static class Extensions
    {
        public static void Shuffle<T>(this IList<T> list)
        {
            Random random = new Random();
            for (var i = list.Count; i > 0; i--)
                list.Swap(0, random.Next(0, i));
        }

        public static void Swap<T>(this IList<T> list, int i, int j)
        {
            var temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }
}
