using SportFoodTracker.Models.Sportsplan;
using SQLite;

namespace SportFoodTracker.Context.Sportplan
{
    public class SportsPlanDatabase
    {
        private SQLiteAsyncConnection? _database;

        /// <summary>
        /// Creates and initializes the database connection if it hasn't been created yet.
        /// </summary>
        public async Task Init()
        {
            if (_database != null)
                return;
            _database = new SQLiteAsyncConnection(SportsPlanDbConstans.DatabasePath, SportsPlanDbConstans.Flags);
            await _database.CreateTableAsync<ExerciseModel>();
            await _database.CreateTableAsync<TrainingsplanModel>();
            await _database.CreateTableAsync<WorkoutModel>();
        }
        #region --- Get Lists ---
        /// <summary>
        /// Get all sportplans
        /// </summary>
        /// <returns></returns>
        public async Task<List<WorkoutModel>> GetAllSportsplansAsync()
        {
            await Init();
            if (_database == null)
            {
                return new List<WorkoutModel>();
            }
            var sportsplans = await _database.Table<WorkoutModel>().ToListAsync();

            var trainings = await _database.Table<TrainingsplanModel>().ToListAsync();

            var exercises = await _database.Table<ExerciseModel>().ToListAsync();

                foreach (var sp in sportsplans)
                {
                    var spTrainings = trainings.Where(tp => tp.SportsplanId == sp.Id).ToList();

                    foreach (var tp in spTrainings)
                    {
                        tp.Exercise = exercises.Where(ex => ex.TrainingsplanId == tp.Id).ToList();
                    }

                    sp.Trainingsplan = spTrainings;
                }

            return sportsplans;
        }

        /// <summary>
        /// Get all training plans
        /// </summary>
        /// <returns></returns>
        public async Task<List<TrainingsplanModel>> GetAllTrainingsplansAsync()
        {
            await Init();

            if (_database == null)
            {
                return new List<TrainingsplanModel>();
            }
            var trainings = await _database.Table<TrainingsplanModel>().ToListAsync();
            var exercises = await _database.Table<ExerciseModel>().ToListAsync();

            foreach (var tp in trainings)
            {
                tp.Exercise = exercises.Where(ex => ex.TrainingsplanId == tp.Id).ToList();
            }

            return trainings;
        }

        /// <summary>
        /// get all exercises
        /// </summary>
        /// <returns></returns>
        public async Task<List<ExerciseModel>> GetAllExercisesAsync()
        {
            await Init();
            if (_database == null)
            {
                return new List<ExerciseModel>();
            }
            return await _database.Table<ExerciseModel>().ToListAsync();
        }
        #endregion

        #region --- Get Single Items ---
        /// <summary>
        /// get specific sport plan
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<WorkoutModel> GetSportsplanByIdAsync(int id)
        {
            await Init();

            if (_database == null)
            {
                return new WorkoutModel();
            }

            var sportsplan = await _database.Table<WorkoutModel>()
                                             .Where(sp => sp.Id == id)
                                             .FirstOrDefaultAsync();
            if (sportsplan == null)
                return null!;

            var trainings = await _database.Table<TrainingsplanModel>()
                                           .Where(tp => tp.SportsplanId == id)
                                           .ToListAsync();

            var exercises = await _database.Table<ExerciseModel>()
                                           .ToListAsync();

            foreach (var tp in trainings)
            {
                tp.Exercise = exercises.Where(ex => ex.TrainingsplanId == tp.Id).ToList();
            }

            sportsplan.Trainingsplan = trainings;
            return sportsplan;
        }

        /// <summary>
        /// get a specific trainings plan
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TrainingsplanModel> GetTrainingsplanByIdAsync(int id)
        {
            await Init();

            if (_database == null)
            {
                return new TrainingsplanModel();
            }

            var tp = await _database.Table<TrainingsplanModel>()
                                    .Where(t => t.Id == id)
                                    .FirstOrDefaultAsync();
            if (tp == null)
                return null!;

            tp.Exercise = await _database.Table<ExerciseModel>()
                                         .Where(ex => ex.TrainingsplanId == tp.Id)
                                         .ToListAsync();
            return tp;
        }

        /// <summary>
        /// get specific exersice
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ExerciseModel> GetExerciseByIdAsync(int id)
        {
            await Init();
            if (_database == null)
            {
                return new ExerciseModel();
            }
            return await _database.Table<ExerciseModel>().Where(ex => ex.Id == id).FirstOrDefaultAsync();
        }
        #endregion

        #region --- Save Items ---
        /// <summary>
        /// save or update sport plan
        /// </summary>
        /// <param name="sportplan"></param>
        /// <returns></returns>
        public async Task<int> SaveSportplanAsync(WorkoutModel sportplan)
        {
            await Init();
            if (_database == null)
            {
                return 0;
            }
            await _database.RunInTransactionAsync(conn =>
            {
                // Sportsplan
                if (sportplan.Id == 0)
                    conn.Insert(sportplan);
                else
                    conn.Update(sportplan);

                // Trainingspläne
                if (sportplan.Trainingsplan == null)
                    return;

                foreach (var training in sportplan.Trainingsplan)
                {
                    training.SportsplanId = sportplan.Id;

                    if (training.Id == 0)
                        conn.Insert(training);
                    else
                        conn.Update(training);

                    // Exercises
                    if (training.Exercise == null)
                        continue;

                    foreach (var exercise in training.Exercise)
                    {
                        exercise.TrainingsplanId = training.Id;

                        if (exercise.Id == 0)
                            conn.Insert(exercise);
                        else
                            conn.Update(exercise);
                    }
                }
            });

            return sportplan.Id;
        }
        #endregion

        #region --- Delete Items ---
        /// <summary>
        /// Delete specific sports plan
        /// </summary>
        /// <param name="sportsplan"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task DeleteSportplanAsync(WorkoutModel sportsplan)
        {
            if (sportsplan == null)
                throw new ArgumentNullException(nameof(sportsplan));

            await Init();
            if (_database == null)
            {
                return;
            }
            var trainings = await GetTrainingsplanByIdAsync(sportsplan.Id);

            await _database.RunInTransactionAsync(conn =>
            {
                foreach (var exercise in trainings.Exercise)
                {
                    conn.Delete(exercise);
                }
                conn.Delete(trainings);
                conn.Delete(sportsplan);
            });
        }
        #endregion
    }
}
