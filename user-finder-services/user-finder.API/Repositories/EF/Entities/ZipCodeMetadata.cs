using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace user_finder.API.Repositories.EF.Entities
{
	/// <summary>
	/// Data/Definition found at Snowflake Marketplace "U.S. ZIP Code Metadata" from DeepSync
	/// </summary>
	internal class ZipCodeMetadata
	{
		/// <summary>
		/// The ZIP code. This is designated as the primary key.
		/// The [Column] attribute ensures the C# property maps to the uppercase Snowflake column.
		/// </summary>
		[Key]
		[Column("ZIP")]
		public string Zip { get; set; } = string.Empty;

		/// <summary>
		/// The primary city name for the Zip code.
		/// </summary>
		[Column("CITY")]
		public string City { get; set; } = string.Empty;

		/// <summary>
		/// The Daylight Savings Indicator.
		/// </summary>
		[Column("DAYLIGHT_SAVINGS")]
		public string DaylightSavings { get; set; } = string.Empty;

		/// <summary>
		/// The Nielsen DMA Identifier.
		/// </summary>
		[Column("DMA_ID")]
		public string DmaId { get; set; } = string.Empty;

		/// <summary>
		/// The Nielsen Designated Marketing Area Name.
		/// </summary>
		[Column("DMA_NAME")]
		public string DmaName { get; set; } = string.Empty;

		/// <summary>
		/// The latitude and longitude of the Zip code centroid.
		/// </summary>
		[Column("GEOPOINT")]
		public string Geopoint { get; set; } = string.Empty;

		/// <summary>
		/// The Zip centroid latitude.
		/// </summary>
		[Column("LATITUDE")]
		public string Latitude { get; set; } = string.Empty;

		/// <summary>
		/// The Zip centroid longitude.
		/// </summary>
		[Column("LONGITUDE")]
		public string Longitude { get; set; } = string.Empty;

		/// <summary>
		/// The median age in this Zip code according to 2020 US census data.
		/// Using double for the Float type for better precision.
		/// </summary>
		[Column("MEDIAN_AGE")]
		public double MedianAge { get; set; }

		/// <summary>
		/// The US State.
		/// </summary>
		[Column("STATE")]
		public string State { get; set; } = string.Empty;

		/// <summary>
		/// The Timezone Offset.
		/// </summary>
		[Column("TIMEZONE")]
		public string Timezone { get; set; } = string.Empty;

		/// <summary>
		/// The total female population according to 2020 US Census data.
		/// Using long for the Number type to prevent potential data overflow.
		/// </summary>
		[Column("TOTAL_FEMALE_POPULATION")]
		public long TotalFemalePopulation { get; set; }

		/// <summary>
		/// The total male population according to 2020 US Census data.
		/// Using long for the Number type to prevent potential data overflow.
		/// </summary>
		[Column("TOTAL_MALE_POPULATION")]
		public long TotalMalePopulation { get; set; }

		/// <summary>
		/// The total population according to 2020 US Census data.
		/// Using long for the Number type to prevent potential data overflow.
		/// </summary>
		[Column("TOTAL_POPULATION")]
		public long TotalPopulation { get; set; }
	}
}