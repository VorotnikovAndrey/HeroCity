using System.Collections.Generic;
using UnityEngine;

namespace Defong.Utils
{
    public static class ArrayUtils
    {
        public static T GetRandom<T>(this IList<T> list)
        {
            return list[Random.Range(0, list.Count)];
        }
    }
}
