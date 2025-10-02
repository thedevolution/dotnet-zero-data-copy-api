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

		protected abstract string GetUsersCloseToZipcodeQuery(string zipCode, double proximityInMiles);

		public async Task<List<FoundUserDistance>> FindUsersCloseToZipcode(string zipCode, double proximityInMiles)
		{
			var sqlQuery = GetUsersCloseToZipcodeQuery(zipCode, proximityInMiles);
			var results = await _databaseContext.FoundUserDistances
										.FromSqlInterpolated(FormattableStringFactory.Create(sqlQuery, zipCode, proximityInMiles))
										.ToListAsync();

			return results;
		}

		protected abstract string GetUsersByZipCode(string zipCode);

		/// <summary>
		/// New, improved method added once ZIP_CODE_METADATA was available, allowing the FoundUser to now contain CITY and STATE
		/// </summary>
		/// <param name="zip">The zipcode to be used to try to find users</param>
		/// <returns>Returns the found user list, or an empty list if none found.</returns>
		public async Task<List<FoundUser>> GetByZipCode(string zip)
        {
			var sqlQuery = GetUsersByZipCode(zip);
			var results = await _databaseContext.FoundUsers
										.FromSqlInterpolated(FormattableStringFactory.Create(sqlQuery, zip))
										.ToListAsync();

			return results;
		}
	}
}
