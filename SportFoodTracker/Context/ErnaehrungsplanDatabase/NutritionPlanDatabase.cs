using SportFoodTracker.Context.Sportplan;
using SportFoodTracker.Models.Ernähungsplan;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportFoodTracker.Context.ErnaehrungsplanDatabase
{
    public class NutritionPlanDatabase
    {
        private SQLiteAsyncConnection? _database;

        /// <summary>
        /// Creates and initializes the database connection if it hasn't been created yet.
        /// </summary>
        public async Task Init()
        {
            if (_database != null)
                return;
            _database = new SQLiteAsyncConnection(NutritionPlanDbConstans.DatabasePath, NutritionPlanDbConstans.Flags);
            await _database.CreateTableAsync<NutritionalValuesModel>();
            await _database.CreateTableAsync<FoodModel>();
            await _database.CreateTableAsync<NutritionplanModel>();
        }
        #region --- Get Lists ---
  
        public async Task<List<NutritionplanModel>> GetErnaehrungsplanList()
        {
            await Init();
            if (_database == null)
            {
                return new List<NutritionplanModel>();
            }
            return await _database.Table<NutritionplanModel>().ToListAsync();
        }

        public async Task<List<FoodModel>> GetEssenList()
        {
            await Init();
            if (_database == null)
            {
                return new List<FoodModel>();
            }
            return await _database.Table<FoodModel>().ToListAsync();
        }

        public async Task<List<NutritionalValuesModel>> GetNaehrwertList()
        {
            await Init();
            if (_database == null)
            {
                return new List<NutritionalValuesModel>();
            }
            return await _database.Table<NutritionalValuesModel>().ToListAsync();
        }
        #endregion

        #region --- Get Single Items ---
        public async Task<NutritionplanModel> GetSportplan(int id)
        {
            await Init();
            if (_database == null)
            {
                return new NutritionplanModel();
            }
            return await _database.Table<NutritionplanModel>().Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<FoodModel> GetTrainingsplan(int id)
        {
            await Init();
            if (_database == null)
            {
                return new FoodModel();
            }
            return await _database.Table<FoodModel>().Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<NutritionalValuesModel> GetUebung(int id)
        {
            await Init();
            if (_database == null)
            {
                return new NutritionalValuesModel();
            }
            return await _database.Table<NutritionalValuesModel>().Where(x => x.Id == id).FirstOrDefaultAsync();
        }
        #endregion

        #region --- Save Items ---
        public async Task<int> SaveSportplanAsync(NutritionplanModel ernaerungsplan)
        {
            await Init();
            if (ernaerungsplan.Id != 0)
            {
                if (_database == null)
                {
                    return 0;
                }
                return await _database.UpdateAsync(ernaerungsplan);
            }
            else
            {
                if (_database == null)
                {
                    return 0;
                }
                return await _database.InsertAsync(ernaerungsplan);
            }
        }
        #endregion

        #region --- Delete Items ---
        public async Task<int> DeleteSportplanAsync(NutritionplanModel ernaerungsplan)
        {
            await Init();
            if (_database == null)
            {
                return 0;
            }
            return await _database.DeleteAsync(ernaerungsplan);
        }
        #endregion
    }
}
