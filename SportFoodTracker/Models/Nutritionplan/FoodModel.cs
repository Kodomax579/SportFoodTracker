using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportFoodTracker.Models.Ernähungsplan
{
    public class FoodModel
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string? DayOfWeek { get; set; }
        public string? Description { get; set; }
        public int Weight { get; set; }
        public NutritionalValuesModel NutritionalValues{ get; set; }
    }
}
