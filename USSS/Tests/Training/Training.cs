using AT;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USSS.Helpers;
using USSS.WebPages.CommonPages;

namespace USSS.Tests.Training
{
    class Training : TestBase
    {
        static string testName = "Обучение";//название теста
        string login = ReaderTestData.ReadExel(testName, "Login");// тянем с экселя параметра (путь С:\Atu\TestDate.xls)
        string password = ReaderTestData.ReadExel(testName, "Password");

        bool globalR = true;

        AuthorizationPage ap;

        [Test]
        public void step_01()
        {
            string rezult = "";
            Logger.PrintHeadTest(testName);//пишем в лог(путь по дефолту С:\Atu\Logs\)
            Logger.PrintStepName("Step 1");
            Logger.PrintAction("Открытие стенда", "");
            ap = new AuthorizationPage();
            ap.Open();//открываем браузер на странице авторизации(url c конфига xls тянется)
            //Проверка отображения страницы авторизации
            Logger.PrintAction("Проверка отображения страницы авторизации", "");
            rezult = ap.ConstructionPage();
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
            }
            else
            {
                Logger.PrintRezult(true, "Страница авторизации корректна");
            }
            rezult = "";
            //Авторизация
            Logger.PrintAction("Авторизация", "Логин:" + login + ", Пароль: " + password);
            rezult = ap.Logon(login, password);
            if (rezult != "success")
            {
                globalR = false;
                Logger.PrintRezult(false, rezult);
                ap.Close();
                Logger.PrintRezultTest(globalR);
            }
            else
            {
                Logger.PrintRezult(true, "Авторизация прошла успешно");
            }
            rezult = "";
        }

        //степ

        //степ

        //...

        //степ

       
        [Test]
        public void step_100500()//последний степ
        {
            //код степа
            Logger.PrintRezultTest(globalR);//глобальный результат всего теста
            ap.Close();//закрываем браузер
        }
    }
}
