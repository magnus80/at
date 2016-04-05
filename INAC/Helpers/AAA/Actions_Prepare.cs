using System.Collections.Generic;

namespace INAC.Helpers.AAA
{
    public static partial class Actions
    {
        public static class Prepare
        {
            public static void NewAbonentsPackUnlim(ref List<string> abons, int count)
            {
                for (int i = 0; i < count; i++)
                {
                    var service = Services.FTTB.Queries.GetFttbServiceUnlim("12042");
                    var login = Abonents.Actions.Creation.Create(service, 10000, Environment.NewAbonPassword);
                    abons.Add(login);
                }
            }

            public static void NewAbonentsPackLimit(ref List<string> abons, int count)
            {
                for (int i = 0; i < count; i++)
                {
                    var service = Services.FTTB.Queries.GetFttbServiceLimit("12042");
                    var login = Abonents.Actions.Creation.Create(service, 10000, Environment.NewAbonPassword);
                    abons.Add(login);
                }
            }

            public static void NewAbonentsPackVSU(ref List<string> abons, int count)
            {
                for (int i = 0; i < count; i++)
                {
                    var service = Services.FTTB.Queries.GetFttbServiceForVsu("12042");
                    var login = Abonents.Actions.Creation.Create(service, 10000, Environment.NewAbonPassword);
                    abons.Add(login);
                    var vsu_serv = Services.VSU.Queries.GetMinVsuService("12042");
                    Abonents.Actions.VSU.Connect(login, vsu_serv);
                }
            }

            public static void NewAbonentsPackTB(ref List<string> abons, int count)
            {
                for (int i = 0; i < count; i++)
                {
                    var service = Services.FTTB.Queries.GetFttbServiceUnlim("12042");
                    var login = Abonents.Actions.Creation.Create(service, 10000, Environment.NewAbonPassword);
                    abons.Add(login);
                    var tb_serv = Services.Turbo_Button.Queries.GetTurboSpeedUpService("12042");
                    Abonents.Actions.TurboButton.Connect(login, tb_serv);
                }
            }

            public static void NewAbonentsPackShaped(ref List<string> abons, int count)
            {
                for (int i = 0; i < count; i++)
                {
                    var service = Services.FTTB.Queries.GetFttbServiceShaped("12042");
                    var login = Abonents.Actions.Creation.Create(service, 10000, Environment.NewAbonPassword);
                    abons.Add(login);
                }
            }

            public static void NewAbonentsPackMultiService(ref List<string> abons, int count)
            {

            }
        }
    }
}
