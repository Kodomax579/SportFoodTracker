using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportFoodTracker.Models.Training
{
    public class TrainingModel
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public bool IsDone { get; set; }
        public string? Description{ get; set; }
        public int time { get; set; }
        [Ignore]
        public List<RepetionsModel> Repetions { get; set; } = new List<RepetionsModel>();
    }
}
