using System.Collections.Generic;
using AT;
using AT.WebDriver;

namespace INAC.WebPages.HD
{
    public class startstop__pl : PageBase
    {
        public WebElementTable StartStopTable = new WebElementTable();
        public WebElementTable BlackListTable;

        public void StartStopTableInit()
        {
            StartStopTable.InitByXPath("//*[@id='content']/table[2]/tbody/tr/td/table");
        }

    }
}
