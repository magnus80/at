using System.Collections.Generic;
using AT;
using AT.WebDriver;

namespace INAC.WebPages.HD
{
    public class pay_reserved__pl : PageBase
    {
        public WebElementTable Pay_reservedTable;
        
        public void PayReservedTableInit()
        {
            Pay_reservedTable = new WebElementTable();
            Pay_reservedTable.InitByXPath("//*[@id='content']/table[2]/tbody/tr/td/table");
        }
    }
}
