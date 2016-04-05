using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AT.DataBase;

namespace INAC.Helpers.Queues
{
    public static class Actions
    {
        public static void RunSMSProcessor()
        {
            Executor.ExecuteProcedure("inac.queue_proc.sms_processor", Environment.InacDb);
        }

        public static void RunSMSProcessor(string queue)
        {
            Executor.ProcedureParamList.Add(new ProcedureParam("number", "p_n_queue", queue));
            Executor.ExecuteProcedure("inac.queue_proc.sms_processor", Environment.InacDb);
        }

        public static void RunMailProcessor(string queue)
        {
            Executor.ProcedureParamList.Add(new ProcedureParam("number", "p_n_queue", queue));
            Executor.ExecuteProcedure("inac.queue_proc.mail_processor", Environment.InacDb);
        }
    }
}
