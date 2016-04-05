using System.Collections.Generic;
using AT;
using AT.WebDriver;

namespace INAC.WebPages.HD
{
    public class promissed_pay__pl : PageBase
    {
        public WebElementTable Pay_reservedTable = new WebElementTable();
        public WebElementTable BlackListTable = new WebElementTable();

        public void Pay_reservedTableInit()
        {
            Pay_reservedTable.InitByXPath("//*[@id='content']/table[2]/tbody/tr/td/table[1]"); 
            
        }

        public void BlackListTableTableInit()
        {
            BlackListTable.InitByXPath("//*[@id='content']/table[2]/tbody/tr/td/table[2]"); 
        }

    }
}
