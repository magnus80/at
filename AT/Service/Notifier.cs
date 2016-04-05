using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using AT.Global;
using System.Linq;

namespace AT.Service
{
    public static class Notifier
    {
        private static string msg = string.Empty;
        private static string project_name = Config.GetStringParam("project_name") ?? string.Empty;

        private static void FormMessage()
        {
            int pass = GlobalVariables.Tests.Count(test => test.Value == TestStatuses.Passed);
            int fail = GlobalVariables.Tests.Count(test => test.Value == TestStatuses.Failed);

            var message = project_name + @" Autotest Report. <br/>";

            message += @"
            <br/> <b>Summary:</b> <br/> Pass Rate = ";

            if (pass == 0)
                message += "0 %.";
            else if (fail == 0)
                message += "100 %.";
            else message += (100 - fail/pass*100) + "%.";

            message += @"<br/>
                        <table width='90%' border='1' style=
                          'table-layout:fixed;word-wrap:break-word;border-collapse:collapse;font-family:Times New Roman;'>
                          <tr align='center' bgcolor='#BBBBBB'>
                              <th>Test case ID</th>
 
                              <th>Run status</th>
 
                              <th>Error log</th>
                            </tr>";

            foreach (var test in GlobalVariables.Tests)
            {
                var errors =
                    from e in GlobalVariables.Errors
                    where e.Value.Test.Equals(test.Key)
                    select new {e};


                message += @"</tr>
                             <tr>
                                <td align = 'center'>" + test.Key + @"</td>";
                switch (test.Value)
                {
                    case TestStatuses.Passed:
                        message += "<td align = 'center' style='color:#177245'> <b>" + test.Value + "</b></td>";
                        break;
                    default:
                        message += "<td align = 'center' style='color:#e32636'> <b>" + test.Value + "</b></td>";
                        break;
                }

                message += "<td> <br/>&nbsp;&nbsp;";
                message = errors.Aggregate(message,
                                           (current, error) =>
                                           current +
                                           (error.e.Value.Message.Replace(test.Key + " : ", "") + "<br/> &nbsp;&nbsp;"));

                message += @" </td>
                            </tr>";
            }

            message += "</table>";

            msg = message;
        }

        public static void FormXmlReport()
        {
            var strings = new List<string>();
            strings.Add("<?xml version='1.0' encoding='WINDOWS-1251'?>");
            strings.Add("<Test_cases>");
            foreach (var test in GlobalVariables.Tests)
            {
                strings.Add("<test_" + test.Key + ">");
                strings.Add("<status>" + test.Value + "</status>");

                var errors =
                    from e in GlobalVariables.Errors
                    where e.Value.Test.Equals(test.Key)
                    select new {e};

                if (errors.Count() > 0)
                    strings.Add("<errors>");

                strings.AddRange(from error in GlobalVariables.Errors
                                 where error.Value.Test == test.Key
                                 select "<error>" + error.Value.Message.Replace(test.Key + " : ", "") + "</error>");

                if (errors.Count() > 0)
                    strings.Add("</errors>");

                strings.Add("</test_" + test.Key + ">");
            }

            strings.Add("</Test_cases>");

            var sw = new StreamWriter("report.xml");
            sw.AutoFlush = true;
            foreach (var s in strings)
            {
                sw.WriteLine(s);
            }
            sw.Close();
        }

        public static void SendNotif()
        {
            FormMessage();

            var smtp_server = Config.GetStringParam("smtp_server");
            var smtp_port = Config.GetIntParam("smtp_port");
            var smtp_login = Config.GetStringParam("smtp_login");
            var smtp_password = Config.GetStringParam("smtp_password");
            var mail_from = Config.GetStringParam("mail_from");
            var mail_to = Config.GetStringParam("mail_to");

            SmtpClient Smtp = new SmtpClient(smtp_server, smtp_port);
            Smtp.Credentials = new NetworkCredential(smtp_login, smtp_password);
            Smtp.EnableSsl = false;

            //Формирование письма
            MailMessage Message = new MailMessage();
            Message.From = new MailAddress(mail_from);
            Message.To.Add(mail_to);
            Message.Subject = project_name + ".Autotest Report " + DateTime.Now.Date.ToShortDateString();
            Message.Body = msg;
            Message.SubjectEncoding = Encoding.GetEncoding(65001);
            Message.BodyEncoding = Encoding.GetEncoding(65001);
            Message.IsBodyHtml = true;

            //отправка письма
            Smtp.Send(Message);
        }
    }
}
