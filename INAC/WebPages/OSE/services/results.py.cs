using AT;
using AT.WebDriver;

namespace INAC.WebPages.OSE.services
{
    public class results__py : PageBase
    {
        public WebElementTable ResultTable;

        public void InitResultTable()
        {
            ResultTable = new WebElementTable();
            ResultTable.InitByXPath("//table[@class='results_table']");
        }
    }
}