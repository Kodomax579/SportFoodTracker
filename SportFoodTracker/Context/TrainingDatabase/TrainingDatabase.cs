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
        private SQLiteAsyncConnection? _database;

        /// <summary>
        /// Creates and initializes the database connection if it hasn't been created yet.
        /// </summary>
        public async Task Init()
        {
            if (_database != null)
                return;
            _database = new SQLiteAsyncConnection(TrainingDbConstans.DatabasePath, TrainingDbConstans.Flags);

            await _database.CreateTableAsync<TrainingSetModel>();
            await _database.CreateTableAsync<TrainingModel>();
        }

        /// <summary>
        /// get all trainings
        /// </summary>
        /// <returns></returns>
        public async Task<List<TrainingModel>> GetAllTrainingsAsync()
        {
            await Init();
            if(_database == null)
            {
                return new List<TrainingModel>();
            }
            var trainings = await _database.Table<TrainingModel>().ToListAsync();


            foreach (var training in trainings)
            {
                var repetions = await _database.Table<TrainingSetModel>()
                    .Where(r => r.TrainingId == training.Id)
                    .ToListAsync();
                training.TrainingSets = repetions;
            }
            return trainings;
        }

        /// <summary>
        /// get specific training
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TrainingModel> GetTrainingByIdAsync(int id)
        {
            await Init();
            if (_database == null)
            {
                return new TrainingModel();
            }

            var training = await _database.Table<TrainingModel>()
                .Where(t => t.Id == id)
                .FirstOrDefaultAsync();
            if (training != null)
            {
                var repetions = await _database.Table<TrainingSetModel>()
                    .Where(r => r.TrainingId == training.Id)
                    .ToListAsync();
                training.TrainingSets = repetions;
            }
            return training!;
        }

        /// <summary>
        /// create or update training
        /// </summary>
        /// <param name="training"></param>
        /// <returns></returns>
        public async Task<int> SaveTrainingAsync(TrainingModel training)
        {
            await Init();
            if (training.Id != 0)
            {
                if (_database == null)
                {
                    return 0;
                }
                await _database.UpdateAsync(training);
            }
            else
            {
                if (_database == null)
                {
                    return 0;
                }
                training.Id = await _database.InsertAsync(training);
            }

            foreach (var rep in training.TrainingSets)
            {
                rep.TrainingId = training.Id;
                rep.ExerciseId = rep.Exercise!.Id;
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
