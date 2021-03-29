using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2.util
{
    public static class SwapRemove
    {
        public static void SwapRemoveList<T>(List<T> list, int index)
        {
            list[index] = list[list.Count - 1];
            list.RemoveAt(list.Count - 1);
        }
    }
}
