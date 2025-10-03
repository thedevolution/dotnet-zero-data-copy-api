using Microsoft.EntityFrameworkCore;

namespace user_finder.API.Repositories.EF.DataPlatforms
{
	public class DatabricksFoundUserRepository : FoundUserSqlRepository
	{
		public DatabricksFoundUserRepository(DatabaseContext databaseContext) : base(databaseContext)
		{
		}

		protected override string GetUsersByZipCodeSqlQuery(string zip)
		{
			return $@"
                SELECT
                    UL.FIRST_NAME AS FirstName,
                    UL.LAST_NAME AS LastName,
                    UL.EMAIL AS Email,
                    ZMD.ZIP,
                    ZMD.CITY,
                    ZMD.STATE AS StateAbbr
                FROM
                    BC_USERS.PUBLIC.USER_LOCATION AS UL
                JOIN
                    U_S__ZIP_CODE_METADATA.DEFAULT.ZIP_CODE_METADATA AS ZMD
                ON
                    UL.ZIP = ZMD.ZIP
                WHERE UL.ZIP = {zip}";
		}

		protected override string GetUsersCloseToZipcodeSqlQuery(string zipCode, double proximityInMiles)
		{
			return $@"
               WITH CalculatedDistance AS (
                SELECT
                    UL.FIRST_NAME AS FirstName,
                    UL.LAST_NAME AS LastName,
                    UL.EMAIL AS Email,
                    ZMD.ZIP,
                    ZMD.CITY,
                    ZMD.STATE AS StateAbbr,
                    BC_USERS.PUBLIC.HAVERSINE(
                        (SELECT CAST(LATITUDE AS FLOAT) FROM U_S__ZIP_CODE_METADATA.DEFAULT.ZIP_CODE_METADATA WHERE ZIP = {zipCode}),
                        (SELECT CAST(LONGITUDE AS FLOAT) FROM U_S__ZIP_CODE_METADATA.DEFAULT.ZIP_CODE_METADATA WHERE ZIP = {zipCode}),
                        CAST(ZMD.LATITUDE AS FLOAT),
                        CAST(ZMD.LONGITUDE AS FLOAT)
                    ) * 0.621371 AS DistanceInMiles -- The column is calculated here
                FROM
                    BC_USERS.PUBLIC.USER_LOCATION AS UL
                JOIN
                    U_S__ZIP_CODE_METADATA.DEFAULT.ZIP_CODE_METADATA AS ZMD
                ON
                    UL.ZIP = ZMD.ZIP
            )
            SELECT
                *
            FROM
                CalculatedDistance
            WHERE -- Use WHERE, not HAVING, for the filtering on a non-aggregated column
                DistanceInMiles < {proximityInMiles}
            ORDER BY
                DistanceInMiles ASC";
		}
	}

    internal static class DatabricksDependencyInjectionExtensions
    {
		public static void ConfigureDatabricks(this WebApplicationBuilder builder)
		{
			var connectionString = builder.Configuration.GetConnectionString("DatabricksConnection");
			builder.Services.AddDbContext<DatabaseContext>(dbContextOptions =>
			{
				dbContextOptions.UseDatabricks(connectionString);
				dbContextOptions.LogTo(Console.WriteLine, LogLevel.Information); // or LogLevel.Debug for more detail
				dbContextOptions.EnableSensitiveDataLogging(); // WARNING: Only use in development!
			});
			builder.Services.AddTransient<IFoundUserRepository, DatabricksFoundUserRepository>();
		}
	}
}
