using Tamir.SharpSsh;

namespace AT.Tools
{
    public class SSH
    {
        private SshShell ssh;

        public SSH(string host, string user, string password)
        {
            ssh = new SshShell(host, user, password);
        }
        
        public void Connect(int port)
        {
            ssh.Connect(port);
        }

        public void Write(string comand)
        {
            ssh.WriteLine(comand);
            System.Threading.Thread.Sleep(5000);
        }

        public string Read()
        {
            return ssh.Expect(); 
        }

        public void Close()
        {
            if (ssh.Connected)
                ssh.Close();
        }
    }
}
