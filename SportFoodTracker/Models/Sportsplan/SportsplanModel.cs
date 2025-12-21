using SQLite;

namespace SportFoodTracker.Models.Sportsplan
{
    public class SportsplanModel
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public string? Description { get; set; }
        [Ignore]
        public List<TrainingsplanModel> Trainingsplan{ get; set; } = new List<TrainingsplanModel>();
    }
}
