using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OMIS.DBA
{
    public class DataSort
    {

        #region  QuickSort
        private static void Swap(List<int> data, int i, int j)
        {
            int t = data[i];
            data[i] = data[j];
            data[j] = t;
        }

        public static void QuickSort(List<int> data)
        {
            QuickSort(data, 0, data.Count - 1);
        }

        public static void QuickSort(List<int> data, int low, int high)
        {
            if (low >= high) return;
            int temp = data[low];
            int i = low + 1, j = high;
            while (true)
            {
                while (data[j] > temp) j--;
                while (data[i] < temp && i < j) i++;
                if (i >= j) break;
                Swap(data, i, j);
                i++;
                j--;
            }
            if (j != low)
            {
                Swap(data, low, j);
            }
            QuickSort(data, j + 1, high);
            QuickSort(data, low, j - 1);
        }
        #endregion

    }
}