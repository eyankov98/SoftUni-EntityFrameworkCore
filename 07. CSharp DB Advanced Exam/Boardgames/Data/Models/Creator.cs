﻿using Boardgames.Shared;
using System.ComponentModel.DataAnnotations;

namespace Boardgames.Data.Models
{
    public class Creator
    {
        public Creator()
        {
            Boardgames = new HashSet<Boardgame>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(GlobalConstants.CreatorFirstNameMaxLength)]
        [MinLength(GlobalConstants.CreatorFirstNameMinLength)]
        public string FirstName { get; set; } = null!;

        [Required]
        [MaxLength(GlobalConstants.CreatorLastNameMaxLength)]
        [MinLength(GlobalConstants.CreatorLastNameMinLength)]
        public string LastName { get; set; } = null!;

        public virtual ICollection<Boardgame> Boardgames { get; set; }
    }
}
