using System.Collections.Generic;
using AT;
using AT.WebDriver;

namespace INAC.WebPages.HD
{
    public class payments__pl : PageBase
    {
        /// <summary>
        /// Ищет в таблице со списаниями списания с указаной суммой
        /// </summary>
        /// <param name="sum">сумма</param>
        /// <returns>количество найденных записей</returns>
        public int FindPayment(string sum)
        {
            int count = 0;
            sum = sum.Replace(',', '.');
            foreach (var el in Browser.FindElementsByTagName("td"))
            {
                if (el.Text.Equals(sum))
                {
                    count++;
                }
            } 
            return count;
        }
    }
}
