using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Scripts.Environments
{
    public static class NumberFormatter
    {
        private static readonly List<string> Suffixes = new List<string>
     {
         "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M",
         "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z",
         "AA", "AB", "AC", "AD", "AE", "AF", "AG", "AH", "AI", "AJ", "AK", "AL", "AM",
         "AN", "AO", "AP", "AQ", "AR", "AS", "AT", "AU", "AV", "AW", "AX", "AY", "AZ"
     };
        public static string Format(float number,int pointCount = 0)
        {
            if (number < 0)
            {
                return "-" + Format(-number,pointCount);
            }
            if (number == 0)
            {
                return "0";
            }

            if (number < 10000)
            {
                return number.ToString($"F{pointCount}");
            }                                         
                                                                                           
            int suffixIndex = (int)(Mathf.Log10(number) / 4) - 1;

            if (suffixIndex >= Suffixes.Count)
            {
                return number.ToString("E2");
            }

            float shortNumber = number / Mathf.Pow(10000, suffixIndex + 1);

            return shortNumber.ToString("F2") + Suffixes[suffixIndex];
        }
    }
}
