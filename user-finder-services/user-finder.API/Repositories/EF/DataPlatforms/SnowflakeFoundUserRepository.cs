using Microsoft.EntityFrameworkCore;

namespace user_finder.API.Repositories.EF.DataPlatforms
{
	public class SnowflakeFoundUserRepository : FoundUserSqlRepository
	{
		public SnowflakeFoundUserRepository(DatabaseContext databaseContext) : base(databaseContext)
		{
		}

		protected override string GetUsersByZipCode(string zip)
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
                U_S__ZIP_CODE_METADATA.ZIP_DEMOGRAPHICS.ZIP_CODE_METADATA AS ZMD
            ON
                UL.ZIP = ZMD.ZIP
            WHERE UL.ZIP = {zip}";
		}

		protected override string GetUsersCloseToZipcodeQuery(string zipCode, double proximityInMiles)
		{
			return $@"
            SELECT
                UL.FIRST_NAME AS FirstName,
                UL.LAST_NAME AS LastName,
                UL.EMAIL AS Email,
                ZMD.ZIP,
                ZMD.CITY,
                ZMD.STATE AS StateAbbr,
                HAVERSINE(
                    (SELECT LATITUDE::FLOAT FROM U_S__ZIP_CODE_METADATA.ZIP_DEMOGRAPHICS.ZIP_CODE_METADATA WHERE ZIP = {zipCode}),
                    (SELECT LONGITUDE::FLOAT FROM U_S__ZIP_CODE_METADATA.ZIP_DEMOGRAPHICS.ZIP_CODE_METADATA WHERE ZIP = {zipCode}),
                    ZMD.LATITUDE::FLOAT,
                    ZMD.LONGITUDE::FLOAT
                ) * 0.621371 AS DistanceInMiles
            FROM
                BC_USERS.PUBLIC.USER_LOCATION AS UL
            JOIN
                U_S__ZIP_CODE_METADATA.ZIP_DEMOGRAPHICS.ZIP_CODE_METADATA AS ZMD
            ON
                UL.ZIP = ZMD.ZIP
            HAVING
                DistanceInMiles < {proximityInMiles}
            ORDER BY
                DistanceInMiles ASC";
		}
	}

    internal static class SnowflakeDependencyInjectionExtensions
    {
		public static void ConfigureSnowflake(this WebApplicationBuilder builder)
		{
			var connectionString = builder.Configuration.GetConnectionString("SnowflakeConnection");
			builder.Services.AddDbContext<DatabaseContext>(dbContextOptions =>
			{
				dbContextOptions.UseSnowflake(connectionString);
				dbContextOptions.LogTo(Console.WriteLine, LogLevel.Information); // or LogLevel.Debug for more detail
				dbContextOptions.EnableSensitiveDataLogging(); // WARNING: Only use in development!
			});
			builder.Services.AddTransient<IFoundUserRepository, SnowflakeFoundUserRepository>();
		}
	}
}
