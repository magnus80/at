using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AT;
using AT.WebDriver;

namespace USSS.Helpers
{
    class ReaderMail 
    {
        public void ReadLastMail()
        {
            Yandex y = new Yandex();
            y.GoToMailList();
            y.GoToLastMail();
            y.DownloadFile();
            Console.WriteLine(y.GetText()); 
        }
    }

    internal class Yandex : PageBase
    {
        public void GoToMailList()
        {
            Browser.Navigate("https://mail.yandex.ru/");
            WebElement login = new WebElement().ByXPath("//input[@name='login']");
            login.SendKeys("usss.pnz");
            WebElement password = new WebElement().ByXPath("//input[@name='passwd']");
            password.SendKeys("Qwerty4$");
            WebElement submit =
                new WebElement().ByXPath("//button[@class=' nb-button _nb-action-button nb-group-start']");
            submit.Click();
            Thread.Sleep(10000);
        }

        public void GoToLastMail()
        {
            WebElement lastMail =
                new WebElement().ByXPath(
                    "//div[@class='b-messages b-messages_threaded']//a[@class='b-messages__message__link daria-action'][1]");
            lastMail.Click();
            Thread.Sleep(10000);
        }

        public void DownloadFile()
        {
            WebElement a =
                new WebElement().ByXPath(
                    "//a[@class='b-link b-link_w b-link_js b-file__download js-attachments-get-btn daria-action']");
            a.Click();
            Thread.Sleep(5000);
            SendKeys.SendWait("{ENTER}");
            Thread.Sleep(10000);
        }

        public string GetText()
        {
            WebElement div = new WebElement().ByXPath("//div[@class='b-message-body__content']");
            return div.Text;
        }
    }
}
