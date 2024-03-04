using Boardgames.Shared;
using System.ComponentModel.DataAnnotations;

namespace Boardgames.Data.Models
{
    public class Seller
    {
        public Seller()
        {
            BoardgamesSellers = new HashSet<BoardgameSeller>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(GlobalConstants.SellerNameMaxLength)]
        [MinLength(GlobalConstants.SellerNameMinLength)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(GlobalConstants.SellerAddressMaxLength)]
        [MinLength(GlobalConstants.SellerAddressMinLength)]
        public string Address { get; set; } = null!;

        [Required]
        public string Country { get; set; } = null!;

        [Required]
        public string Website { get; set; } = null!;

        public virtual ICollection<BoardgameSeller> BoardgamesSellers { get; set; }
    }
}
