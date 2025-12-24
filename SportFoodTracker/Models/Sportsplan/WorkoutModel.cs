using SQLite;

namespace SportFoodTracker.Models.Sportsplan
{
    public class WorkoutModel
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
