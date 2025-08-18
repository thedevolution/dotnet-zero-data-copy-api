using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using user_finder.API.Domain;

namespace user_finder.API.Repositories.EF
{
	public class FoundUserRepository : IFoundUserRepository
	{
		private readonly DatabaseContext _databaseContext;

		public FoundUserRepository(DatabaseContext databaseContext)
		{
			_databaseContext = databaseContext;
		}

		public async Task<List<FoundUserDistance>> FindUsersCloseToZipcode(string zipCode, double proximityInMiles)
		{
			var sqlQuery = $@"
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

			var results = await _databaseContext.FoundUserDistances
										.FromSqlInterpolated(FormattableStringFactory.Create(sqlQuery, zipCode, proximityInMiles))
										.ToListAsync();

			return results;
		}

		/// <summary>
		/// New, improved method added once ZIP_CODE_METADATA was available, allowing the FoundUser to now contain CITY and STATE
		/// </summary>
		/// <param name="zip">The zipcode to be used to try to find users</param>
		/// <returns>Returns the found user list, or an empty list if none found.</returns>
		public async Task<List<FoundUser>> GetByZipCode(string zip)
        {
			var sqlQuery = $@"
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
			var results = await _databaseContext.FoundUsers
										.FromSqlInterpolated(FormattableStringFactory.Create(sqlQuery, zip))
										.ToListAsync();

			return results;
		}

		/*
		 * Previous method where only local data in USER_LOCATION table could be queried
		 *
		public async Task<List<FoundUser>> GetByZipCode(string zip)
		{
			return await _databaseContext.UserLocations.Where(ul => ul.Zip == zip).Select(ul => new FoundUser
			{
				FirstName = ul.FirstName,
				LastName = ul.LastName,
				Email = ul.Email,
				Zip = zip
			}).ToListAsync();
		}*/
		
	}
}
