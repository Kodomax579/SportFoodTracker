using SQLite;

namespace SportFoodTracker.Models.Sportsplan
{
    public class TrainingProgramModel
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public string? Description { get; set; }
        [Ignore]
        public List<WorkoutSessionModel> WorkoutSessions { get; set; } = new List<WorkoutSessionModel>();
    }
}
