using Boardgames.Shared;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Boardgames.DataProcessor.ImportDto
{
    public class ImportSellerDto
    {
        [JsonProperty("Name")]
        [Required]
        [MaxLength(GlobalConstants.SellerNameMaxLength)]
        [MinLength(GlobalConstants.SellerNameMinLength)]
        public string Name { get; set; } = null!;

        [JsonProperty("Address")]
        [Required]
        [MaxLength(GlobalConstants.SellerAddressMaxLength)]
        [MinLength(GlobalConstants.SellerAddressMinLength)]
        public string Address { get; set; } = null!;

        [JsonProperty("Country")]
        [Required]
        public string Country { get; set; } = null!;

        [JsonProperty("Website")]
        [Required]
        [RegularExpression(GlobalConstants.SellerWebsiteRegex)]
        public string Website { get; set; } = null!;

        [JsonProperty("Boardgames")]
        public int[] BoardgamesIds { get; set; } = null!;
    }
}
