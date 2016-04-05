using System;
using System.Linq;
using System.Threading;
using AT.Global;
using AT.Service;
using AT.WebDriver;
using Microsoft.Office.Interop.Excel;

namespace AT
{
    public abstract class PageBase
    {
        private string _url;

        public string UrlCache
        {
            get
            {
                Application excel = new Application();
                Workbook wb = excel.Workbooks.Open("C:\\ATU\\TestsData2.xls");
                Worksheet excelSheet = wb.ActiveSheet;
                string d = excelSheet.Cells[11, 7].Value.ToString();
                wb.Close();
                return d;
            }
        }

        public string Url
        {
            get
            {
                Application excel = new Application();
                Workbook wb = excel.Workbooks.Open("C:\\ATU\\TestsData2.xls");
                Worksheet excelSheet = wb.ActiveSheet;
                string d = excelSheet.Cells[1, 7].Value.ToString();
                wb.Close();
                return d;
                //try
                //{
                //    var fullName = GetType().FullName;
                //    var split = fullName.Split(new[] {'.'});

                //    var subProject = split[2];
                //    var page = split.Last();
                //    var hierarchyUrl = "";

                //    var hierarchy =
                //        fullName.Substring(fullName.IndexOf("." + subProject) +
                //                           ("." + subProject).Length).Replace("." + page, "");

                //    if(hierarchy.Length > 0)
                //         hierarchyUrl = (hierarchy.Substring(1).Replace(".", "/") + "/").Replace(GlobalVariables.DashPrefix, "-");

                //    var subProjectUrl = Config.GetStringParam(subProject);

                //    if (page.IndexOf(GlobalVariables.ExtensionPrefix) != -1)
                //    {
                //        var pageFileName = page.Substring(0, page.IndexOf(GlobalVariables.ExtensionPrefix));
                //        var pageFileExtension = page.Replace(pageFileName, "")
                //            .Replace(GlobalVariables.ExtensionPrefix, ".");

                //        page = string.Concat(pageFileName + pageFileExtension);
                //    }
                //    else if (page.IndexOf(GlobalVariables.FolderIndexPrefix) != -1)
                //    {
                //        page = page.Replace(GlobalVariables.FolderIndexPrefix, "");
                //        if (page.Length != 0)
                //            page += "/";
                //    }

                //    _url = subProjectUrl + hierarchyUrl;// +page;//ПРАВИТЬ!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

                //    return _url;
                //}
                //catch (Exception ex)
                //{
                //    GlobalEvents.ExeptionFounded(ex);
                //    return null;
                //}

            }
        }

        public void Open()
        {
            Browser.Navigate(Url);            
        }

        public void SwitchToPopupWindow()
        {
            Browser.SwitchToPopupWindow();
        }

        public void Close()
        {
            Browser.Quit();
        }

        public void Open(string param)
        {
            Browser.Navigate(param);
        }
        public void OpenCache()
        {
            Browser.Navigate(UrlCache);
        }
    }
}
