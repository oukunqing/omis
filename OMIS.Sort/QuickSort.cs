using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OMIS.Sort
{
    public class QuickSort
    {

        #region  Sort
        public static void Sort(ref List<int> nums, int left, int right)
        {
            if (0 == nums.Count)
            {
                return;
            }

            if (left < right)
            {
                int i = left, j = right, pivot = nums[(left + right) / 2];
                while (true)
                {
                    while (i < right && nums[i] < pivot)
                    {
                        i++;
                    }
                    while (j > 0 && nums[j] > pivot)
                    {
                        j--;
                    }
                    if (i == j)
                    {
                        break;
                    }
                    nums[i] = nums[i] + nums[j];
                    nums[j] = nums[i] - nums[j];
                    nums[i] = nums[i] - nums[j];

                    if (nums[i] == nums[j])
                    {
                        j--;
                    }
                }
                Sort(ref nums, left, i - 1);
                Sort(ref nums, i + 1, right);
            }
        }

        public static void Sort(ref List<int> nums)
        {
            Sort(ref nums, 0, nums.Count - 1);
        }
        #endregion

    }
}