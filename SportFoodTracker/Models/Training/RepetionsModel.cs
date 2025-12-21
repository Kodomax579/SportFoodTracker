using SportFoodTracker.Models.Sportsplan;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportFoodTracker.Models.Training
{
    public class RepetionsModel
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        public int TrainingId { get; set; }
        [Ignore]
        public ExerciseModel Exercise { get; set; }
        public int ExerciseId { get; set; }
        public int Weight { get; set; }
        public int Repetitions { get; set; }
    }
}
