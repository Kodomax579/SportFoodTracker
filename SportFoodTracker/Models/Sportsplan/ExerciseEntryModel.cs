using SportFoodTracker.Models.enums;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportFoodTracker.Models.Sportsplan
{
    public class ExerciseEntryModel
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        public int InternalId { get; set; }

        public int WorkoutSessionId { get; set; }

        public string Description { get; set; } = string.Empty;

        public int Repetition { get; set; }
        public int Meter { get; set; }
        public int TimeInSeconds { get; set; }
        public int WeightKg { get; set; }
        public int Sets { get; set; }
        public bool IsCreated { get; set; }
        public int PauseInSeconds { get; set; }

        public TrainingParameter trainingParameter { get; set; }
    }
}