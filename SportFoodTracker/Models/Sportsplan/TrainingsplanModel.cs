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
        [Ignore]
        public List<ExerciseModel> Exercise { get; set; } = new List<ExerciseModel>();

        public int SportsplanId { get; set; }
    }
}
