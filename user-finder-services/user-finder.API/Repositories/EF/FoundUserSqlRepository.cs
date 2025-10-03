using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using user_finder.API.Domain;

namespace user_finder.API.Repositories.EF
{
	public abstract class FoundUserSqlRepository : IFoundUserRepository
	{
		protected readonly DatabaseContext _databaseContext;

		public FoundUserSqlRepository(DatabaseContext databaseContext)
		{
			_databaseContext = databaseContext;
		}

		/// <summary>
		/// Returns the vendor/database-specific SQL query to find users near a zip code.  Different platforms
		/// likely have vastly different SQL statements to do this calculation.
		/// </summary>
		/// <param name="zipCode">The zipcode to query near</param>
		/// <param name="proximityInMiles">The amount of miles to query near the zipcode</param>
		/// <returns>Returns an interpolated string representing the SQL query.</returns>
		protected abstract string GetUsersCloseToZipcodeSqlQuery(string zipCode, double proximityInMiles);

		public async Task<List<FoundUserDistance>> FindUsersCloseToZipcode(string zipCode, double proximityInMiles)
		{
			var sqlQuery = GetUsersCloseToZipcodeSqlQuery(zipCode, proximityInMiles);
			var results = await _databaseContext.FoundUserDistances
										.FromSqlInterpolated(FormattableStringFactory.Create(sqlQuery, zipCode, proximityInMiles))
										.ToListAsync();

			return results;
		}

		/// <summary>
		/// Returns the vendor/database-specific SQL query to find users in a zip code.  
		/// </summary>
		/// <param name="zipCode">The zipcode to query where users are in.</param>
		/// <returns>Returns an interpolated string representing the SQL query.</returns>
		protected abstract string GetUsersByZipCodeSqlQuery(string zipCode);

		/// <summary>
		/// New, improved method added once ZIP_CODE_METADATA was available, allowing the FoundUser to now contain CITY and STATE
		/// </summary>
		/// <param name="zip">The zipcode to be used to try to find users</param>
		/// <returns>Returns the found user list, or an empty list if none found.</returns>
		public async Task<List<FoundUser>> GetByZipCode(string zip)
        {
			var sqlQuery = GetUsersByZipCodeSqlQuery(zip);
			var results = await _databaseContext.FoundUsers
										.FromSqlInterpolated(FormattableStringFactory.Create(sqlQuery, zip))
										.ToListAsync();

			return results;
		}
	}
}
