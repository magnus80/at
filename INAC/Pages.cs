using INAC.WebPages.HD.ptn.add_speed;
using INAC.WebPages.OSE;
using INAC.WebPages.OSE.services;
using INAC.WebPages.HD;
using INAC.WebPages.HD.ptn.iptv;

namespace INAC
{
    public static class Pages
    {
        public static class OSE
        {
            public static inaclogin__pl Inaclogin = new inaclogin__pl();
            public static inacusers__pl InacUsers = new inacusers__pl();

            public static class Services
            {
                public static form__py Form = new form__py();
                public static rfc__py Rfc = new rfc__py();
                public static results__py Results = new results__py();
            }
        }

        public static class HD
        {
            public static login__pl Login = new login__pl();
            public static global_problems__pl GlobalProblems = new global_problems__pl();
            public static globalcomments__pl GlobalComments = new globalcomments__pl();
            public static logout__pl Logout = new logout__pl();
            public static queues__pl Queues = new queues__pl();
            public static find_name__pl Find_name = new find_name__pl();
            public static address__pl Address = new address__pl();
            public static comments__pl Comments = new comments__pl();
            public static stop_bc__pl Stop_bc = new stop_bc__pl();
            public static cus_admin__pl Cus_admin = new cus_admin__pl();
            public static cus_campaign__pl Cus_campaign = new cus_campaign__pl();
            public static adv_adr_info__pl Adv_adr_info = new adv_adr_info__pl();
            public static prolongation_bc__pl Prolongation_bc = new prolongation_bc__pl();
            public static new_logs__pl New_logs = new new_logs__pl();
            public static stop_billing__pl Stop_billing = new stop_billing__pl();
            public static move_bill__pl Move_bill = new move_bill__pl();
            public static antivir__pl Antivir = new antivir__pl();
            public static changepassword__pl Changepassword = new changepassword__pl();
            public static avpc_statuses__pl Avpc_statuses = new avpc_statuses__pl();
            public static tickets2__pl Tickets2 = new tickets2__pl();
            public static newglobal__pl New_global = new newglobal__pl();
            public static notification_settings__pl Notification_settings = new notification_settings__pl();
            public static scalechange__pl Scalechange = new scalechange__pl();
            public static shedule_nconnect__pl Shedule_nconnect = new shedule_nconnect__pl();
            public static showcomments__pl Showcomments = new showcomments__pl();
            public static payments__pl Payments = new payments__pl();
            public static pay_reserved__pl Pay_reserved = new pay_reserved__pl();
            public static promissed_pay__pl Promissed_pay = new promissed_pay__pl();
            public static changeadress__pl Changeadress = new changeadress__pl();
            public static changeadress2__pl Changeadress2 = new changeadress2__pl();
            public static changeadress3__pl Changeadress3 = new changeadress3__pl();
            public static startstop__pl Startstop = new startstop__pl();
            public static addiptv__pl Addiptv = new addiptv__pl();
            public static deliptv__pl Deliptv = new deliptv__pl();
            public static rentiptv2__pl Rentiptv2 = new rentiptv2__pl();
            public static changeiptv__pl Changeiptv = new changeiptv__pl();
            public static services__pl Services = new services__pl();
            public static netfone_add__pl Netfone_add = new netfone_add__pl();
            public static users__pl Users = new users__pl();
            public static Bundle_services2__pl Bundle_Services2 = new Bundle_services2__pl();

            public static class PTN
            {
                public static class IPTV
                {
                    public static archive_rent_fld Archive_Rent = new archive_rent_fld();
                }

                public class Add_Speed
                {
                    public static change_speed_fld Change_Speed = new change_speed_fld();
                }
            }
        }
    }
}
