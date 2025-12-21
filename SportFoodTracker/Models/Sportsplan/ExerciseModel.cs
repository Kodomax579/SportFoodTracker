using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportFoodTracker.Models.Sportsplan
{
    public class ExerciseModel
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public int Repetition{ get; set; }
        public int Sets { get; set; }
        public int TrainingsplanId { get; set; }
    }
}
