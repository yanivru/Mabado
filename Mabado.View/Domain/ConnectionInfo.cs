using System;
using System.Data.SqlClient;

namespace Mabado.View.Domain
{
    public class ConnectionInfo
    {
        public string Host { get; set; }

        public string Instance { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string DataSource
        {
            get { return Host + "\\" + Instance; }
            set
            {
                string[] token = value.Split(new[] { "\\" }, StringSplitOptions.None);

                if (token.Length > 0)
                {
                    Host = token[0];
                }

                if (token.Length > 1)
                {
                    Instance = token[1];
                }
            }
        }

        public string CreateConnectionString(string initialCatalog)
        {
            return UpdateConnectionString(new SqlConnectionStringBuilder { InitialCatalog = initialCatalog });
        }

        public string UpdateConnectionString(string connectionString)
        {
            return UpdateConnectionString(new SqlConnectionStringBuilder(connectionString));
        }

        public string UpdateConnectionString(SqlConnectionStringBuilder builder)
        {
            builder.DataSource = DataSource;
            builder.UserID = UserName;
            builder.Password = Password;

            return builder.ConnectionString;
        }

        public static ConnectionInfo FromConnectionString(string connectionString)
        {
            SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder(connectionString);

            return new ConnectionInfo
                {
                    DataSource = csb.DataSource,
                    UserName = csb.UserID,
                    Password = csb.Password
                };
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((ConnectionInfo) obj);
        }

        protected bool Equals(ConnectionInfo other)
        {
            return string.Equals(Host, other.Host) && string.Equals(Instance, other.Instance) && string.Equals(UserName, other.UserName) && string.Equals(Password, other.Password);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (Host != null ? Host.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Instance != null ? Instance.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (UserName != null ? UserName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Password != null ? Password.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}