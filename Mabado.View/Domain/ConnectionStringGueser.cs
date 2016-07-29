using System.Collections.Generic;

namespace Mabado.View.Domain
{
    public class ConnectionStringGueser : IConnectionInfoGuesser
    {
        private readonly string[] _userNames = { "sa", "sa-lab" };
        private readonly string[] _passwords = { "p@ssword1", "p@ssword" };
        private readonly string[] _instances = { "r2", "r12", "mm8", "mmv", "" };

        public IEnumerable<ConnectionInfo> Guess(ConnectionInfo info)
        {
            string[] userNames = string.IsNullOrEmpty(info.UserName) ? _userNames : new[] { info.UserName };
            string[] passwords = string.IsNullOrEmpty(info.Password) ? _passwords : new[] { info.Password };
            string[] instances = string.IsNullOrEmpty(info.Instance) ? _instances : new[] { info.Instance };

            return GetAllConnectionStringsPermutations(info.Host, instances, userNames, passwords);
        }

        private IEnumerable<ConnectionInfo> GetAllConnectionStringsPermutations(string ip, IEnumerable<string> instances, string[] userNames, string[] passwords)
        {
            foreach (string instance in instances)
            {
                foreach (string userName in userNames)
                {
                    foreach (string password in passwords)
                    {
                        ConnectionInfo info = new ConnectionInfo();
                        info.Host = ip;
                        info.Instance = instance;
                        info.UserName = userName;
                        info.Password = password;

                        yield return info;
                    }
                }
            }
        }
    }
}