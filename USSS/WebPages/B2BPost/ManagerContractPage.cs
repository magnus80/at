using System.Threading;
using AT.DataBase;
using AT.WebDriver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USSS.WebPages.B2BPost
{
    class ManagerContractPage
    {

        public NumberProfilePage numberProfilePage;
        public TariffChangePage tariffChange;

        #region constructor

        #region navigation WE

        //таблица абонентов
        private WebElement AbonentsNumberWE;
   

        #endregion

        private WebElement CheckBox = new WebElement().ByXPath("//div[@id = 'mobileDataForm:abonents']//tbody/tr[1]/td[1]");

        private WebElement CurTariff;
        private WebElement ActiveCheckBox;
        private WebElement tariffChangeButton;
        private WebElement waitingTariffs;

        public string ConstructionPage()
        {
            AbonentsNumberWE = new WebElement().ByXPath("//div[@id = 'mobileDataForm:abonents']//table");

            if (!AbonentsNumberWE.Displayed) { return "Не отображены элементы интерфейса: список абонентов"; }

            return "success";
        }
        public WebElement getWaitingTariffs
        {
            get
            {
                waitingTariffs = new WebElement().ByXPath("//div[@id='mobileDataForm:waitingTariffsKrutilka']");
                return waitingTariffs;
            }
        }
        public WebElement getActiveCheckBox
        {
            get { ActiveCheckBox = new WebElement().ByXPath("//div[@id = 'mobileDataForm:abonents']//tbody/tr[@aria-selected='true']");
                return ActiveCheckBox;
            }
        }

        public WebElement getTariffChangeButton
        {
            get
            {
                tariffChangeButton = new WebElement().ByXPath("//a[contains(text(),'Изменить тариф')]");
                return tariffChangeButton;
            }
        }

        #endregion

        #region managerPage

        public string GoToNumberProfile(string number)
        {
            string numberStr = "(" + number[0] + number[1] + number[2] + ") " + number[3] + number[4] + number[5] + "-" + number[6] + number[7] + "-" + number[8] + number[9];
            WebElement NumberWE = AbonentsNumberWE.ByXPath("//a[text()='" + numberStr + "']");//WebElement NumberWE = AbonentsNumberWE.ByXPath("//a[contains(@href,'/b/info/subscriberDetail.xhtml?objId')][" + number + "]");
            if (NumberWE.Displayed) NumberWE.Click();
            else { return "Не отображены элементы интерфейса: номер абонента"; }
            numberProfilePage = new NumberProfilePage();

            return numberProfilePage.ConstructionPage();
        }

        public string GoToNumberProfile(string number, string Flag)
        {
            string numberStr = "(" + number[0] + number[1] + number[2] + ") " + number[3] + number[4] + number[5] + "-" + number[6] + number[7] + "-" + number[8] + number[9];
            WebElement NumberWE = AbonentsNumberWE.ByXPath("//a[text()='" + numberStr + "']");//WebElement NumberWE = AbonentsNumberWE.ByXPath("//a[contains(@href,'/b/info/subscriberDetail.xhtml?objId')][" + number + "]");
            if (NumberWE.Displayed) NumberWE.Click();
            else { return "Не отображены элементы интерфейса: номер абонента"; }
            numberProfilePage = new NumberProfilePage();

            return numberProfilePage.ConstructionPage(Flag);
        }
        public string ClickCheckBox()
        {
            if(CheckBox.Displayed)
            {
                 CheckBox.Click();
                                
                return CheckBoxActive();
                //return "success";
            }
            else
            {
                return "Не отображен элемент чекбокс";
            }
        }

        public string CheckTariffChange(string newtariff)
        {
            CurTariff = new WebElement().ByXPath("//tbody[@id='mobileDataForm:abonents_data']/tr/td[12]");
            string curtariff = CurTariff.Text.Replace(" ", "");
            if(curtariff.Contains(newtariff))
            {
                return "success";
            }
            else
            {
                return "Не отображена запланированная смена тарифа";
            }
        }
        public string GetCurrentTariffSoc(string ban, string db_Ans, string db_Ms)
        {
            string tariff_soc = "select soc from ecr9_service_agreement where ban ="+ban+" and service_type = 'P'";
            var qTar_soc = Executor.ExecuteSelect(tariff_soc);
            return qTar_soc[0, 0];
        }
        public string GetNewTariffSoc(string ban, string db_Ans, string db_Ms,string oldsoc)
        {
            string tariff_soc = "select soc from ecr9_service_agreement where ban =" + ban + " and service_type = 'P' and soc <> '"+oldsoc+"'";
            var qTar_soc = Executor.ExecuteSelect(tariff_soc);
            return qTar_soc[0, 0];
        }
        public string CheckTariffDBChange(string old_tariff,string ban,string db_Ans,string db_Ms)
        {
            string expiration_date = "select expiration_date from ecr9_service_agreement where soc = '"+old_tariff+"' and ban =" + ban + " and service_type = 'P'";
            var q = Executor.ExecuteSelect(expiration_date);
            expiration_date = q[0, 0];

            string effective_date = "select effective_date from ecr9_service_agreement where soc <> '" + old_tariff + "' and ban =" + ban + " and service_type = 'P'";
            q = Executor.ExecuteSelect(effective_date);
            effective_date = q[0, 0];

            string expiration_date_ans = "select expiration_date from service_agreement@"+db_Ans+" where soc = '" + old_tariff + "' and ban =" + ban + " and service_type = 'P'";
            q = Executor.ExecuteSelect(expiration_date_ans);
            expiration_date_ans = q[0, 0];

            string effective_date_ans = "select effective_date from service_agreement@"+db_Ans+" where soc <> '" + old_tariff + "' and ban =" + ban + " and service_type = 'P'";
            q = Executor.ExecuteSelect(effective_date_ans);
            effective_date_ans = q[0, 0];

            if(expiration_date == effective_date && expiration_date_ans == effective_date_ans)
            {
                return "success";
            }
            return "expiration_date не равна effective_date";
        }

        public string CheckBoxActive()
        {
            if (getActiveCheckBox.Displayed)
            {
                return "success";
            }
            else
            {
                return "Чекбокс не изменил свое значение";
            }
        }
        //public string CurrentTariff(int i)
        //{
        //    CurTariff = new WebElement().ByXPath("//tbody[@id='mobileDataForm:abonents_data']/tr[1]/td[12]");
        //    if(CurTariff.Displayed)
        //    {
        //        return CurTariff.Text;
        //    }
        //    return "";
        //}
        public string GoToTariffChange()
        {
            Thread.Sleep(3000);
            tariffChange = new TariffChangePage();
            if (getTariffChangeButton.Displayed)
            {
                tariffChangeButton.Click();
                while(getWaitingTariffs.Displayed)
                {
                    Thread.Sleep(3000);
                }
                tariffChange = new TariffChangePage();
                Thread.Sleep(5000);
                return tariffChange.Construct();
            }
            else
            {
                return "Не отображен Изменить тарифный план";
            }
        }
        #endregion


        
        
    }
}
