using SportFoodTracker.Models.enums;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportFoodTracker.Models.Sportsplan
{
    public class WorkoutSessionModel
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        public Weekday DayOfWeek { get; set; }
        public int TrainingProgramId { get; set; }

        [Ignore]
        public List<ExerciseEntryModel> Exercise { get; set; } = new List<ExerciseEntryModel>();        
    }
}
