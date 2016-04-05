using System;
using AT.Tools;

namespace INAC.Helpers.TL
{
    static internal class IptvTechlists
    {
        public static string FormIptvServicesInsertTechlist(string cityId, string cityName, string type, string serviceName, string price)
        {
            ExcelWorker ex = new ExcelWorker(Environment.TlIptvServicesInsertPath);

            ex.SetCell("A2", cityId);
            ex.SetCell("B2", cityName);
            ex.SetCell("C2", type);
            ex.SetCell("E2", serviceName);
            ex.SetCell("F2", price);

            return Techlist.SaveAndClose(ref ex, "iptv_serv_ins");
        }

        public static string FormIptvPacketsInsertTechlist(string basePacketName, string visible, string effectivePrice, string cityId, string cityName)
        {
            ExcelWorker ex = new ExcelWorker(Environment.TlIptvPacketsInsertPath);

            string basePacketNumber = "1";
            int line = 2;
            int outId = 1;
            

            ex.SetCell("A" + line, (line-1).ToString());
            ex.SetCell("B" + line, basePacketNumber);
            ex.SetCell("C" + line, visible);
            ex.SetCell("D" + line, basePacketName);
            ex.SetCell("H" + line, "0");
            ex.SetCell("I" + line, effectivePrice);
            ex.SetCell("M" + line, outId.ToString());

            outId++;

            int typesCount = new Random().Next(1, 7);
            int typeIter = 0;
            while(typeIter < typesCount)
            {
                line++;
                ex.SetCell("A" + line, (line - 1).ToString());
                ex.SetCell("B" + line, basePacketNumber);
                ex.SetCell("E" + line, "type_" + typeIter);
                ex.SetCell("J" + line, "1");
                ex.SetCell("K" + line, "N");

                int subtypesCount = new Random().Next(1, 7);
                int subtypeIter = 0;
                while (subtypeIter < subtypesCount)
                {
                    line++;
                    ex.SetCell("A" + line, (line - 1).ToString());
                    ex.SetCell("B" + line, basePacketNumber);
                    ex.SetCell("F" + line, "type_" + typeIter + "_subtype_" + subtypeIter);

                    int elementsCount = new Random().Next(1, 7);
                    int elementIter = 0;

                    var rnd = new Random();
                    while (elementIter < elementsCount)
                    {
                        var price = rnd.Next(100, 999).ToString();
                        line++;
                        ex.SetCell("A" + line, (line - 1).ToString());
                        ex.SetCell("B" + line, basePacketNumber);
                        ex.SetCell("C" + line, visible);
                        ex.SetCell("G" + line, "type_" + typeIter + "_subtype_" + subtypeIter + "_element_" + elementIter);
                        ex.SetCell("H" + line, price);
                        ex.SetCell("M" + line, outId.ToString());

                        outId++;
                        elementIter++;
                    }

                    subtypeIter++;
                }

                typeIter++;
            }

            ex.ChangeSheet(4);
            ex.SetCell("A2", basePacketNumber);
            ex.SetCell("B2", basePacketName);
            ex.SetCell("C2", cityId);
            ex.SetCell("D2", cityName);

            return Techlist.SaveAndClose(ref ex, "iptv_packet_ins");
        }

        public static string FormIptvPacketsUpdateTechlist(string pkt_id, string pkt_name, string externalId, string effectivePrice)
        {
            ExcelWorker ex = new ExcelWorker(Environment.TlIptvPacketsUpdatePath);

            ex.ChangeSheet(3);

            ex.SetCell("A2", pkt_id);
            ex.SetCell("B2", pkt_name);
            ex.SetCell("C2", "Игнорировать");
            ex.SetCell("D2", externalId);
            ex.SetCell("E2", "Игнорировать");
            ex.SetCell("F2", effectivePrice);

            return Techlist.SaveAndClose(ref ex, "iptv_packet_upd");
        }
    }
}
