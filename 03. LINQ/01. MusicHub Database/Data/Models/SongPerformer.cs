using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicHub.Data.Models
{
    public class SongPerformer
    {
        public int SongId { get; set; }
        [ForeignKey(nameof(SongId))]
        [Required]
        public virtual Song Song { get; set; }

        public int PerformerId { get; set; }
        [ForeignKey(nameof(PerformerId))]
        [Required]
        public virtual Performer Performer { get; set;}
    }
}
