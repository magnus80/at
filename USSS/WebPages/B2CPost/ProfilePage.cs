using System.Threading;
using AT.DataBase;
using AT.WebDriver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USSS.WebPages.B2CPost
{
    class ProfilePage
    {
        public FinansAndDetalizationPage finansAndDetalizationPage;
        public RequestHistoryPage requestHistoryPage;
        public TariffsPage tariffsPage;
        public ServicesPage servicesPage;

        #region constructor


        #region navigation WE
        //ссылка Отменить
        private WebElement cancelPricePlan;
        //кнопка Подтвердить
        private WebElement retChange; 
        //ссылка на профиль в навигации
        private WebElement ProfileWE;
        //ссылка на уведомления в навигации
        private WebElement NoticeWE;
        //ссылка на настройки в навигации
        private WebElement SettingsWE;
        //ссылка на тарифы в навигации
        private WebElement TariffsWE;
        //ссылка на услуги в навигации
        private WebElement ServicesWE;
        //ссылка на историю запросов в навигации
        private WebElement RequestHistoryWE;
        //ссылка на обратную связь в навигации
        private WebElement FeedbackWE;
        //ссылка на способы оплаты в навигации
        private WebElement FinanceWE;

        #endregion



        public string ConstructionPage()
        {
         //   retChange = new WebElement().ByXPath("//button[contains(@id,subscriberDetailsForm:j_idt2247:j_idt2255')]");
            ProfileWE = new WebElement().ByXPath("//a[@id = 'postProfile']");
            NoticeWE = new WebElement().ByXPath("//a[contains(@onclick,'notice')]");
            SettingsWE = new WebElement().ByXPath("//a[contains(@onclick,'settings')]");
            TariffsWE = new WebElement().ByXPath("//a[contains(@onclick,'tariffs')]");
            ServicesWE = new WebElement().ByXPath("//a[contains(@onclick,'services')]");
            FinanceWE = new WebElement().ByXPath("//a[contains(@onclick,'finance')]");

            RequestHistoryWE = new WebElement().ByXPath("//a[contains(@onclick,'operationsHistory')]");
            FeedbackWE = new WebElement().ByXPath("//a[contains(@onclick,'feedback')]");
          

            if (!ProfileWE.Displayed) { return "Не отображены элементы интерфейса: ссылка на профиль"; }
            if (!NoticeWE.Displayed) { return "Не отображены элементы интерфейса: ссылка на уведомления"; }
            if (!SettingsWE.Displayed) { return "Не отображены элементы интерфейса: ссылка на настройки"; }
            if (!TariffsWE.Displayed) { return "Не отображены элементы интерфейса: ссылка на тарифы"; }
            if (!ServicesWE.Displayed) { return "Не отображены элементы интерфейса: ссылка на услуги"; }
            if (!FinanceWE.Displayed) { return "Не отображены элементы интерфейса: ссылка на финансы и информацию"; }
          
            if (!RequestHistoryWE.Displayed) { return "Не отображены элементы интерфейса: ссылка на историю запросов"; }
            if (!FeedbackWE.Displayed) { return "Не отображены элементы интерфейса: ссылка на обратную связь"; }
           

            return "success";
        }

        #endregion


        #region managerPage


        #endregion

        public string CheckSubscription()
        {

            WebElement subs = new WebElement().ByXPath("//a[text()='Мои информационно-развлекательные услуги']");
            if (!subs.Displayed) return "Отсутствует ссылка Подписки";
            subs.Click();
            WebElement subsList;
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(10000);
                subsList = new WebElement().ByXPath("//div[contains(@class,'subscriptions-list')]");
                if (subsList.Displayed) break;
            }
            subsList = new WebElement().ByXPath("//div[contains(@class,'subscriptions-list')]");
            if (!subsList.Displayed) return "Не отображается список подписок";
            int j = 1;
            WebElement sub = new WebElement().ByXPath("//div[contains(@class,'subscriptions-list')]/div["+j+"]");
            while (sub.Displayed)
            {
                if (sub.ByXPath("//div[@class='info']/div[1]/h2").Text == "")
                    return "Не отображается наименование подписки";
                if (sub.ByXPath("//div[@class='info']//span[contains(@class,'price')]").Text == "")
                    return "Не отображается стоимость подписки";
                if (!sub.ByXPath("//div[@class='info']//div[contains(@class,'ui-outputpanel ui-widget not-free')]").Text.Contains("в"))
                    return "Не отображается период списания";
                if (!sub.ByXPath("//a[@class='full-descr-title dynamic']").Displayed)
                    return "Не отображается ссылка на подробное описание";
                if (sub.ByXPath("//div[contains(@class,'CDP')]").Displayed)
                {
                    if (!sub.ByXPath("//img[@class='from-beeline']").Displayed)
                        return "Не отображается картинка Подписка от Билайн";
                }
                j++;
                sub = new WebElement().ByXPath("//div[contains(@class,'subscriptions-list')]/div[" + j + "]");
            }
            return "success";
        }

       public string CheckSettingSubscription()
       {
           WebElement ssub = new WebElement().ByXPath("//a[contains(@class,'ui-commandlink ui-widget settings false')]");
           if (!ssub.Displayed){ return "Отсутствуют параметризованные подписки";}
           ssub.Click();
           Thread.Sleep(20000);
           if (!new WebElement().ByXPath("//label[contains(text(),'Запрет доступа ко всем платным и бесплатным SMS-сервисам Провайдеров, а также получения сообщений от банков и торговых точек')]").Displayed)
           {
               return "Не отображается настройка Запрет доступа ко всем платным и бесплатным SMS-сервисам Провайдеров...";
           }
           if (!new WebElement().ByXPath("//label[text()='Запрет доступа к SMS-сервисам Провайдеров стоимостью более 50-ти руб. с НДС']").Displayed)
           {
               return "Не отображается настройка Запрет доступа к SMS-сервисам Провайдеров стоимостью более 50-ти руб. с НДС";
           }
           if (!new WebElement().ByXPath("//label[text()='Запрет доступа ко всем платным SMS-сервисам Провайдеров, но остается возможность получать сообщения от банков и торговых сетей']").Displayed)
           {
               return "Не отображается настройка Запрет доступа ко всем платным SMS-сервисам Провайдеров, но остается возможность получать сообщения от банков и торговых сетей";
           }

           WebElement rb = new WebElement().ByXPath("//div[@class='ui-radiobutton-box ui-widget ui-corner-all ui-state-default']//span");
           if (!rb.Displayed){ return "Отсутствует радиобаттон"; }
           rb.Click();
           Thread.Sleep(10000);
           WebElement btnSuccess = new WebElement().ByXPath("//button[contains(@id,'connectorBtn')]");
           if (!btnSuccess.Displayed) { return "Отсутствует кнопка Изменить"; }
           btnSuccess.Click();
           Thread.Sleep(20000);
           string mb = new WebElement().ByXPath("//div[@class='ui-outputpanel ui-widget connectorResults message']").Text;
           if (!mb.Contains("Запрос") | !mb.Contains("на подключение услуги") | !mb.Contains(" успешно отправлен. За статусом запроса следите в")) { return "Текст сообщения неверен"; }
           return "success";
       }

        public string number;
        public  string CheckCPAUnconnect()
        {
            WebElement sub = new WebElement().ByXPath("//div[contains(@class,'subscriptions-list')]//div[contains(@class,'CPA')][1]");
            if (!sub.Displayed){ return "Отсутсвуют CPA подписки"; } 
            sub.ByXPath("//div[@class='switch']/button").Click();
            Thread.Sleep(10000);
            //new WebElement().ByXPath("//div[@class='ui-outputpanel ui-widget service-item subscription-item']//div[@class='buttons']/button").Click();
            //Thread.Sleep(10000);
            //string message = new WebElement().ByXPath(
            //    "//div[@class='ui-outputpanel ui-widget editorResults dashed-block message-single']//div[@class='fmr']").Text;
            //number = message.Remove(0, 8);
            //number = number.Remove(10, number.Length - 10);

            return "success";
        }

         public  string CheckCDPUnconnect()
        {
            WebElement sub = new WebElement().ByXPath("//div[contains(@class,'subscriptions-list')]//div[contains(@class,'CDP')][1]");
            if (!sub.Displayed){ return "Отсутсвуют CDP подписки"; } 
            sub.ByXPath("//div[@class='switch']/button").Click();
             Thread.Sleep(10000);
            //new WebElement().ByXPath("//div[@class='ui-outputpanel ui-widget service-item subscription-item']//div[@class='buttons']/button").Click();
            //Thread.Sleep(10000);
            //string message = new WebElement().ByXPath(
            //    "//div[@class='ui-outputpanel ui-widget editorResults dashed-block message-single']//div[@class='fmr']").Text;
            //number = message.Remove(0, 8);
            //number = number.Remove(10, number.Length - 10);

            return "success";
        }

        public string GoToFinanceAndDetalization()
        {
            finansAndDetalizationPage = new FinansAndDetalizationPage();
            FinanceWE = new WebElement().ByXPath("//a[contains(@onclick,'finance')]");
            if (FinanceWE.Displayed) FinanceWE.Click();
            else { return "Не отображены элементы интерфейса: ссылка на Финансы и информацию"; }
            
            return finansAndDetalizationPage.ConstructionPage();
        }
       
        public string GoToRequestHistoryPage()
        {
            RequestHistoryWE = new WebElement().ById("requestsHistory");//ByXPath("//a(@id='requestsHistory')");
            if (RequestHistoryWE.Displayed) RequestHistoryWE.Click();
            else { return "Не отображены элементы интерфейса: ссылка на историю заявок"; }
            requestHistoryPage = new RequestHistoryPage();
            return requestHistoryPage.ConstructionPage();
        }

        public string GoToTariff()
        {
            TariffsWE = new WebElement().ByXPath("//a[contains(@onclick,'tariffs')]");
            if (!TariffsWE.Displayed)
            {
                return "Не отображены элементы интерфейса: ссылка на тарифы";
            }
            TariffsWE.Click();
            tariffsPage = new TariffsPage();
            return tariffsPage.ConstructionPage();
        }

        public string GoToProfile()
        {
            ProfileWE = new WebElement().ByXPath("//a[@id='postProfile']");
            if (!ProfileWE.Displayed)
            {
                return "Не отображены элементы интерфейса: ссылка на профиль";
            }
            ProfileWE.Click();
            return "success";
        }


        public string GoToService()
        {
            ServicesWE = new WebElement().ByXPath("//a[contains(@onclick,'services')]");
            if (!ServicesWE.Displayed)
            {
                return "Не отображены элементы интерфейса: ссылка на услуги";
            }
            ServicesWE.Click();
            servicesPage = new ServicesPage();
            return servicesPage.ConstructionPage();
        }

        public string GetCurrentTariff(string db_Ans, string number)
        {
            var t = "select logical_date from logical_date@" + db_Ans + @" where rownum<2";
            var tm = Executor.ExecuteSelect(t);
            string dateB = Convert.ToDateTime(tm[0, 0]).ToShortDateString();
            var d = dateB.Split('.');
            string date = d[2] + d[1] + d[0];

            var q = @"select soc from ecr9_service_agreement where subscriber_no = '" + number + @"'
                        and service_type = 'P'
                        and (TO_CHAR(TRUNC(EXPIRATION_DATE),'YYYYMMDD')>=" +
                    date + @" or EXPIRATION_DATE is null) 
                        and TO_CHAR(TRUNC(EFFECTIVE_DATE),'YYYYMMDD')<=" + date;
                var socB = Executor.ExecuteSelect(q);
                if (socB.Count != 0)
                {
                    string soc = socB[0, 0];
                    return soc;
                }

                return "Тариф отсутствует";
        }

        public string GoToDetalizationHistory()
        {

            new WebElement().ByXPath("//a[contains(text(),'Финансы и детализация')]").Click();
            finansAndDetalizationPage = new FinansAndDetalizationPage();
            return finansAndDetalizationPage.ConstructionPageDiagramm();

        }
        public string GoToDetalizationHistory(string AT)
        {
            if (AT != "")
            {
                new WebElement().ByXPath("//a[contains(text(),'Финансы и детализация')]").Click();
                finansAndDetalizationPage = new FinansAndDetalizationPage();
                return finansAndDetalizationPage.ConstructionPage();
            }
            else
            {
                new WebElement().ByXPath("//a[contains(text(),'Финансы и детализация')]").Click();
                finansAndDetalizationPage = new FinansAndDetalizationPage();
                return finansAndDetalizationPage.ConstructionPageDiagramm();
            }
        }
        //Отмена будущей смены ТП
        public string CancelFuturePricePlan()
        {
            cancelPricePlan = new WebElement().ByXPath("//div[@class='cancel-change-link']/a");
            if (!cancelPricePlan.Displayed)
                {
                return "Не отображена ссылка 'Отменить'";
                }
            cancelPricePlan.Click();
            retChange = new WebElement().ByXPath("//button[contains(@id,'subscriberDetailsForm:j_id')]");
            if (retChange.Displayed)
                {
                retChange.Click();
                return "success";
                }
            return "Не отображена кнопка 'Подтвердить'";            
        }

    }
}
