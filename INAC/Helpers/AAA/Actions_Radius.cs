using AT.Tools;

namespace INAC.Helpers.AAA
{
    public static partial class Actions
    {
        public static class Radius
        {
            public static string CheckAuthInRadiusLog(string login, string service, string brasType, string session_id)
            {
                var ssh = new SSH(Environment.RadiusIp, Environment.RadiusUser, Environment.RadiusPassword);
                ssh.Connect(Environment.RadiusPort);

                bool auth = false, svc = false;

                ssh.Write("cd /var/log/ && tail -n 3 radius.log");

                var res = ssh.Read().Split(new[] {'\r', '\n'}, 200);

                for (int index = 0; index < res.Length; index++)
                {
                    var line = res[index];

                    if (brasType.Equals("CISCO"))
                    {
                        if (line.IndexOf(login) != -1 && line.IndexOf("Accept") != -1 && line.IndexOf("Login OK") != -1 &&
                            line.IndexOf(session_id) != -1)
                        {
                            auth = true;
                            line = res[index + 2];
                            if (line.IndexOf(service) != -1 && line.IndexOf("Login OK") != -1 &&
                                line.IndexOf(session_id) != -1)
                            {
                                svc = true;
                                line = res[index + 4];
                                if (line.IndexOf(login) != -1 && line.IndexOf("Login OK") != -1 &&
                                    line.IndexOf(session_id) != -1)
                                {
                                    ssh.Close();
                                    return "auth:true;svc:true;login:true";
                                }
                            }
                        }
                    }
                }
                ssh.Close();
                return "auth:" + auth + ";svc:" + svc + ";login:false";
            }

            public static string CheckRdrInRadiusLog(string login, string service, string brasType, string session_id)
            {
                var ssh = new SSH(Environment.RadiusIp, Environment.RadiusUser, Environment.RadiusPassword);
                ssh.Connect(Environment.RadiusPort);

                bool auth = false, svc = false;

                ssh.Write("cd /var/log/ && tail -n 3 radius.log");

                var res = ssh.Read().Split(new[] {'\r', '\n'}, 200);

                for (int index = 0; index < res.Length; index++)
                {
                    var line = res[index];

                    if (brasType.Equals("CISCO"))
                    {
                        if (line.IndexOf(login) != -1 && line.IndexOf("Accept") != -1 && line.IndexOf("Login OK") != -1 &&
                            line.IndexOf(session_id) != -1)
                        {
                            auth = true;
                            line = res[index + 2];
                            if (line.IndexOf(service) != -1 && line.IndexOf("Login OK") != -1 &&
                                line.IndexOf(session_id) != -1)
                            {
                                svc = true;
                                var block_type = line.Substring(line.IndexOf('#') + 1, 1);
                                line = res[index + 4];
                                if (line.IndexOf(login) != -1 && line.IndexOf("Login OK") != -1 &&
                                    line.IndexOf(session_id) != -1)
                                {
                                    ssh.Close();
                                    return "auth:true;svc:true;login:true;block_type:" + block_type;
                                }
                            }
                        }
                    }
                }
                ssh.Close();
                return "auth:" + auth + ";svc:" + svc + ";login:false";
            }

        }
    }
}
