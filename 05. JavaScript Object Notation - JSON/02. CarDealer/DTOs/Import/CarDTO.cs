namespace CarDealer.DTOs.Import
{
    public class CarDTO
    {
        public CarDTO()
        {
            PartsId = new HashSet<int>();
        }

        public string Make { get; set; } = null!;

        public string Model { get; set; } = null!;

        public long TraveledDistance { get; set; }

        public virtual ICollection<int> PartsId { get; set; }
    }
}
