namespace CarDealer.DTO
{
    using System.Collections.Generic;

    public class CarImportDTO
    {
        public string Make { get; set; }

        public string Model { get; set; }

        public int TravelledDistance { get; set; }

        public List<int> PartsId { get; set; }
    }
}
