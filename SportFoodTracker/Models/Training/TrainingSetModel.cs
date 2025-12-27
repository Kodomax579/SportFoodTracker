using SportFoodTracker.Models.Sportsplan;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportFoodTracker.Models.Training
{
    public class TrainingSetModel
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        public int TrainingId { get; set; }
        public int ExerciseId { get; set; }
        public int Weight { get; set; }
        public int Repetitions { get; set; }
        public int Meter { get; set; }
        public int TimeInSeconds { get; set; }
        [Ignore]
        public ExerciseModel? Exercise { get; set; }
        [Ignore]
        public TimeOnly Time
        {
            get => TimeOnly.FromTimeSpan(TimeSpan.FromSeconds(TimeInSeconds));
            set => TimeInSeconds = (int)value.ToTimeSpan().TotalSeconds;
        }
    }
}
