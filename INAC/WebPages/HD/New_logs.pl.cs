using System.Collections.Generic;
using AT;
using AT.WebDriver;

namespace INAC.WebPages.HD
{
    public class new_logs__pl: PageBase
    {
        #region elements 
        
        
        #endregion


        #region element_value

        public string Balance
        {
            get
            {
                var source = Browser.Source;
                source = source.Substring(source.IndexOf("Баланс:"));
                source = source.Remove(source.IndexOf("Зарезервированно:"));
                source = source.Remove(source.IndexOf("</b>"));

                source = source.Substring(source.IndexOf("<b>") + "<b>".Length);

                if (source.IndexOf('.') != -1)
                    source = source.Substring(0, source.IndexOf('.'));
                return source;
            }
        }

        #endregion


        #region actions

        #endregion
    }
}
