using AT;
using AT.WebDriver;

namespace INAC.WebPages.OSE.services
{
    public class rfc__py : PageBase
    {
        public WebElementTable Table;

        public void InitTable()
        {
            Table = new WebElementTable();
            Table.InitByXPath("//table[@class='tablesorter results_table']");
        }
    }
}
