using System.Data.SqlClient;
using Mabado.View.Domain;

namespace Mabado.View.Infrastructure
{
    internal class LabInformationProvider
    {
        public InstallationInfo GetLabInfo(string domainDbConnectionString)
        {
            InstallationInfo info = new InstallationInfo();

            using(SqlConnection connection = new SqlConnection(domainDbConnectionString))
            {
                connection.Open();

                GetKeyValueInfo(info, connection);

                GetUserRolesInfo(info, connection);

                connection.Close();
            }

            return info;
        }

        private static void GetUserRolesInfo(InstallationInfo info, SqlConnection connection)
        {
            SqlCommand command = new SqlCommand("Select * from UserRoles Where [RoleID] = 1 Or [RoleID] = 100", connection);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    info.UserSid = (int)reader["UserID"];
                }
            }
        }

        private static void GetKeyValueInfo(InstallationInfo info, SqlConnection connection)
        {
            SqlCommand command = new SqlCommand("Select * from KeyValue Where [key] = 'UI_VERSION'", connection);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    info.Version = (string)reader["Value"];
                }
            }
        }
    }
}