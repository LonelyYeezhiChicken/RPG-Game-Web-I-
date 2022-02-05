using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;

namespace Web.Helper
{
    public class SQLConnectionHelper
    {
        public static string GetSQLConnectionString<T>()
        {
            var dbType = typeof(T);
            //string DBName = "Entity.AdIdentity";
            string DBName = $"{dbType.Namespace}.{dbType.Name}";

            SqlConnectionStringBuilder providerCs = new SqlConnectionStringBuilder();

            providerCs.DataSource = @"NLMD0\VA7POS";
            providerCs.InitialCatalog = @"AD";
            providerCs.IntegratedSecurity = false;
            //providerCs.UserInstance = true;
            providerCs.UserID = @"enorobot";
            providerCs.Password = @"eP1291277";

            var csBuilder = new EntityConnectionStringBuilder();

            csBuilder.Provider = "System.Data.SqlClient";
            csBuilder.ProviderConnectionString = providerCs.ToString();

            csBuilder.Metadata = string.Format("res://{0}/{1}.csdl|res://{0}/{1}.ssdl|res://{0}/{1}.msl",
                "Models", DBName);

            return csBuilder.ToString();
        }
    }
}