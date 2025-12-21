using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportFoodTracker.Models.Ernähungsplan
{
    public class NutritionplanModel
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        public int CalorieGoal { get; set; }
        public int FatGoal { get; set; }
        public int ProteinGoal { get; set; }
        public double WeightGoal { get; set; }
        public double CurrentWeight { get; set; }
        public int Height { get; set; }
        public int Age { get; set; }
        public bool IsActiv { get; set; }
    }
}
