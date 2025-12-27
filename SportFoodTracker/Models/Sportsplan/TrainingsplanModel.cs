using SportFoodTracker.Models.enums;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportFoodTracker.Models.Sportsplan
{
    public class TrainingsplanModel
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        public Weekday DayOfWeek { get; set; }
        public int PauseInSeconds { get; set; }
        public int SportsplanId { get; set; }

        [Ignore]
        public List<ExerciseModel> Exercise { get; set; } = new List<ExerciseModel>();
        [Ignore]
        public TimeOnly pause
        {
            get => TimeOnly.FromTimeSpan(TimeSpan.FromSeconds(PauseInSeconds));
            set => PauseInSeconds = (int)value.ToTimeSpan().TotalSeconds;
        }
    }
}
