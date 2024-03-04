using Boardgames.Shared;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Boardgames.DataProcessor.ImportDto
{
    [XmlType("Creator")]
    public class ImportCreatorDto
    {
        [XmlElement("FirstName")]
        [Required]
        [MaxLength(GlobalConstants.CreatorFirstNameMaxLength)]
        [MinLength(GlobalConstants.CreatorFirstNameMinLength)]
        public string FirstName { get; set; } = null!;

        [XmlElement("LastName")]
        [Required]
        [MaxLength(GlobalConstants.CreatorLastNameMaxLength)]
        [MinLength(GlobalConstants.CreatorLastNameMinLength)]
        public string LastName { get; set; } = null!;

        [XmlArray("Boardgames")]
        public ImportBoardgameDto[] Boardgames { get; set; } = null!;
    }

    [XmlType("Boardgame")]
    public class ImportBoardgameDto
    {
        [XmlElement("Name")]
        [Required]
        [MaxLength(GlobalConstants.BoardgameNameMaxLength)]
        [MinLength(GlobalConstants.BoardgameNameMinLength)]
        public string Name { get; set; } = null!;

        [XmlElement("Rating")]
        [Required]
        [Range(GlobalConstants.BoardgameRatingMinRange, GlobalConstants.BoardgameRatingMaxRange)]
        public double Rating { get; set; }

        [XmlElement("YearPublished")]
        [Required]
        [Range(GlobalConstants.BoardgameYearPublishedMinRange, GlobalConstants.BoardgameYearPublishedMaxRange)]
        public int YearPublished { get; set; }

        [XmlElement("CategoryType")]
        [Required]
        [Range(GlobalConstants.BoardgameCategoryTypeMinRange, GlobalConstants.BoardgameCategoryTypeMaxRange)]
        public int CategoryType { get; set; }

        [XmlElement("Mechanics")]
        [Required]
        public string Mechanics { get; set; } = null!;
    }
}
