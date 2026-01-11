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
            await _database.CreateTableAsync<ExerciseEntryModel>();
            await _database.CreateTableAsync<WorkoutSessionModel>();
            await _database.CreateTableAsync<TrainingProgramModel>();
        }
        #region --- Get Lists ---
        /// <summary>
        /// Get all sportplans
        /// </summary>
        /// <returns></returns>
        public async Task<List<TrainingProgramModel>> GetAllSportsplansAsync()
        {
            await Init();
            if (_database == null)
            {
                return new List<TrainingProgramModel>();
            }
            var sportsplans = await _database.Table<TrainingProgramModel>().ToListAsync();

            var trainings = await _database.Table<WorkoutSessionModel>().ToListAsync();

            var exercises = await _database.Table<ExerciseEntryModel>().ToListAsync();

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
        public async Task<List<WorkoutSessionModel>> GetAllTrainingsplansAsync()
        {
            await Init();

            if (_database == null)
            {
                return new List<WorkoutSessionModel>();
            }
            var trainings = await _database.Table<WorkoutSessionModel>().ToListAsync();
            var exercises = await _database.Table<ExerciseEntryModel>().ToListAsync();

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
        public async Task<List<ExerciseEntryModel>> GetAllExercisesAsync()
        {
            await Init();
            if (_database == null)
            {
                return new List<ExerciseEntryModel>();
            }
            return await _database.Table<ExerciseEntryModel>().ToListAsync();
        }
        #endregion

        #region --- Get Single Items ---
        /// <summary>
        /// get specific sport plan
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TrainingProgramModel> GetSportsplanByIdAsync(int id)
        {
            await Init();

            if (_database == null)
            {
                return new TrainingProgramModel();
            }

            var sportsplan = await _database.Table<TrainingProgramModel>()
                                             .Where(sp => sp.Id == id)
                                             .FirstOrDefaultAsync();
            if (sportsplan == null)
                return null!;

            var trainings = await _database.Table<WorkoutSessionModel>()
                                           .Where(tp => tp.SportsplanId == id)
                                           .ToListAsync();

            var exercises = await _database.Table<ExerciseEntryModel>()
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
        public async Task<WorkoutSessionModel> GetTrainingsplanByIdAsync(int id)
        {
            await Init();

            if (_database == null)
            {
                return new WorkoutSessionModel();
            }

            var tp = await _database.Table<WorkoutSessionModel>()
                                    .Where(t => t.Id == id)
                                    .FirstOrDefaultAsync();
            if (tp == null)
                return null!;

            tp.Exercise = await _database.Table<ExerciseEntryModel>()
                                         .Where(ex => ex.TrainingsplanId == tp.Id)
                                         .ToListAsync();
            return tp;
        }

        /// <summary>
        /// get specific exersice
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ExerciseEntryModel> GetExerciseByIdAsync(int id)
        {
            await Init();
            if (_database == null)
            {
                return new ExerciseEntryModel();
            }
            return await _database.Table<ExerciseEntryModel>().Where(ex => ex.Id == id).FirstOrDefaultAsync();
        }
        #endregion

        #region --- Save Items ---
        /// <summary>
        /// save or update sport plan
        /// </summary>
        /// <param name="sportplan"></param>
        /// <returns></returns>
        public async Task<int> SaveSportplanAsync(TrainingProgramModel sportplan)
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
        public async Task DeleteSportplanAsync(TrainingProgramModel sportsplan)
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
