using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportFoodTracker.Models.Training;
using System.Security.Cryptography.X509Certificates;

namespace SportFoodTracker.Context.TrainingDatabase
{
    public class TrainingDatabase
    {
        private SQLiteAsyncConnection _database;

        /// <summary>
        /// Creates and initializes the database connection if it hasn't been created yet.
        /// </summary>
        public async Task Init()
        {
            if (_database != null)
                return;
            _database = new SQLiteAsyncConnection(TrainingDbConstans.DatabasePath, TrainingDbConstans.Flags);

            await _database.CreateTableAsync<RepetionsModel>();
            await _database.CreateTableAsync<TrainingModel>();
        }

        public async Task<List<TrainingModel>> GetAllTrainingsAsync()
        {
            await Init();
            var trainings = await _database.Table<TrainingModel>().ToListAsync();


            foreach (var training in trainings)
            {
                var repetions = await _database.Table<RepetionsModel>()
                    .Where(r => r.TrainingId == training.Id)
                    .ToListAsync();
                training.Repetions = repetions;
            }
            return trainings;
        }

        public async Task<TrainingModel> GetTrainingByIdAsync(int id)
        {
            await Init();
            var training = await _database.Table<TrainingModel>()
                .Where(t => t.Id == id)
                .FirstOrDefaultAsync();
            if (training != null)
            {
                var repetions = await _database.Table<RepetionsModel>()
                    .Where(r => r.TrainingId == training.Id)
                    .ToListAsync();
                training.Repetions = repetions;
            }
            return training;
        }

        public async Task<int> SaveTrainingAsync(TrainingModel training)
        {
            await Init();
            if (training.Id != 0)
            {
                await _database.UpdateAsync(training);
            }
            else
            {
                training.Id = await _database.InsertAsync(training);
            }

            foreach (var rep in training.Repetions)
            {
                rep.TrainingId = training.Id;
                rep.ExerciseId = rep.Exercise.Id;
                if (rep.Id != 0)
                {
                    await _database.UpdateAsync(rep);
                }
                else
                {
                    await _database.InsertAsync(rep);
                }
                
            }
            return training.Id;
        }
    }
}
