using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportFoodTracker.Models.Ernähungsplan
{
    public class NutritionalValuesModel
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        public int Calories { get; set; }
        public int Fat { get; set; }
        public int Protein { get; set; }
        
    }
}
