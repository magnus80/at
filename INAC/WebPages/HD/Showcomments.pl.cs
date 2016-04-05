﻿using System.Collections.Generic;
using AT;
using AT.WebDriver;

namespace INAC.WebPages.HD
{
    public class showcomments__pl : PageBase
    {
        /// <summary>
        /// Ищет в таблице со списаниями списания с указаной суммой
        /// </summary>
        /// <param name="sum">сумма</param>
        /// <returns>количество найденных записей</returns>
        public int FindWriteOff(string sum)
        {
            int count = 0;
            
            sum = sum.Replace(',', '.');
            foreach (var el in Browser.FindElementsByTagName("td"))
            {
                if (el.Text.IndexOf(sum) == 0)
                {
                    count++;
                }
            }
            return count;
        }
    }
}
