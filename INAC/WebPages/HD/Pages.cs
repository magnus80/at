using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AT.WebPages.HD;

namespace AT.WebPages
{
    public static class Pages
    {
        public static class HD
        {
            public static Login__pl Login = new Login__pl();
            public static Global_problems__pl Global_problems = new Global_problems__pl();
            public static Logout__pl Logout = new Logout__pl();
            public static Queues__pl Queues = new Queues__pl();
            public static Find_name__pl Find_name = new Find_name__pl();
            public static Address__pl Address = new Address__pl();
            public static Comments__pl Comments = new Comments__pl();
            public static Stop_bc__pl Stop_bc = new Stop_bc__pl();
            public static Cus_admin__pl Cus_admin = new Cus_admin__pl();
            public static Cus_campaign__pl Cus_campaign = new Cus_campaign__pl();
            public static Adv_adr_info__pl Adv_adr_info = new Adv_adr_info__pl();
            public static Prolongation_bc__pl Prolongation_bc = new Prolongation_bc__pl();
            public static New_logs__pl New_logs = new New_logs__pl();
            public static Stop_billing__pl Stop_billing = new Stop_billing__pl();
            public static Move_bill__pl Move_bill = new Move_bill__pl();
        }
    }
}
