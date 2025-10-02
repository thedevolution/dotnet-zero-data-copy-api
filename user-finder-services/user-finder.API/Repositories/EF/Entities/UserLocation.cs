using System.ComponentModel.DataAnnotations.Schema;

namespace user_finder.API.Repositories.EF.Entities
{
	[Table("USER_LOCATION")]
	internal class UserLocation
    {
        [Column("USER_LOCATION_ID")]
        public int UserLocationId { get; set; }
        [Column("FIRST_NAME")]
        public string FirstName { get; set; }
        [Column("LAST_NAME")]
        public string LastName { get; set; }
        [Column("EMAIL")]
        public string Email { get; set; }
        [Column("ZIP")]
        public string Zip { get; set; }
    }
}
