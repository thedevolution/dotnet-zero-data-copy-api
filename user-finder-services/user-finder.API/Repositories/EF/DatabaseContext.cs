using Microsoft.EntityFrameworkCore;
using user_finder.API.Domain;
using user_finder.API.Repositories.EF.Entities;

namespace user_finder.API.Repositories.EF
{
	public class DatabaseContext : DbContext
	{
		internal DbSet<UserLocation> UserLocations { get; set; }
		internal DbSet<FoundUser> FoundUsers { get; set; }
		internal DbSet<FoundUserDistance> FoundUserDistances { get; set; }

		public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// Specify that FoundUser is intended to hold query results, not to be a table with a primary key
			modelBuilder.Entity<FoundUser>().HasNoKey();
			modelBuilder.Entity<FoundUserDistance>().HasNoKey();

		}
	}
}
