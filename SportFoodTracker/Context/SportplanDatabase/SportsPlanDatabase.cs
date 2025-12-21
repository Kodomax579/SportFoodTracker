using SportFoodTracker.Models.Sportsplan;
using SQLite;

namespace SportFoodTracker.Context.Sportplan
{
    public class SportsPlanDatabase
    {
        private SQLiteAsyncConnection _database;

        /// <summary>
        /// Creates and initializes the database connection if it hasn't been created yet.
        /// </summary>
        public async Task Init()
        {
            if (_database != null)
                return;
            _database = new SQLiteAsyncConnection(SportsPlanDbConstans.DatabasePath, SportsPlanDbConstans.Flags);
            // Initialize your tables here if needed
            await _database.CreateTableAsync<ExerciseModel>();
            await _database.CreateTableAsync<TrainingsplanModel>();
            await _database.CreateTableAsync<SportsplanModel>();
        }
        #region --- Get Lists ---
        /// <summary>
        /// Lädt alle Sportspläne inklusive Trainingspläne und Übungen.
        /// </summary>
        public async Task<List<SportsplanModel>> GetAllSportsplansAsync()
        {
            await Init();

            var sportsplans = await _database.Table<SportsplanModel>().ToListAsync();

            var trainings = await _database.Table<TrainingsplanModel>().ToListAsync();

            var exercises = await _database.Table<ExerciseModel>().ToListAsync();


                // Trainingspläne den Sportsplänen zuordnen
                foreach (var sp in sportsplans)
                {
                    var spTrainings = trainings.Where(tp => tp.SportsplanId == sp.Id).ToList();

                    // Übungen den Trainingsplänen zuordnen
                    foreach (var tp in spTrainings)
                    {
                        tp.Exercise = exercises.Where(ex => ex.TrainingsplanId == tp.Id).ToList();
                    }

                    sp.Trainingsplan = spTrainings;
                }

            return sportsplans;
        }

        /// <summary>
        /// Lädt alle Trainingspläne inklusive Übungen.
        /// </summary>
        public async Task<List<TrainingsplanModel>> GetAllTrainingsplansAsync()
        {
            await Init();

            var trainings = await _database.Table<TrainingsplanModel>().ToListAsync();
            var exercises = await _database.Table<ExerciseModel>().ToListAsync();

            foreach (var tp in trainings)
            {
                tp.Exercise = exercises.Where(ex => ex.TrainingsplanId == tp.Id).ToList();
            }

            return trainings;
        }

        /// <summary>
        /// Lädt alle Übungen.
        /// </summary>
        public async Task<List<ExerciseModel>> GetAllExercisesAsync()
        {
            await Init();
            return await _database.Table<ExerciseModel>().ToListAsync();
        }
        #endregion

        #region --- Get Single Items ---
        /// <summary>
        /// Lädt einen einzelnen Sportsplan inklusive Trainingspläne und Übungen.
        /// </summary>
        public async Task<SportsplanModel> GetSportsplanByIdAsync(int id)
        {
            await Init();

            var sportsplan = await _database.Table<SportsplanModel>()
                                             .Where(sp => sp.Id == id)
                                             .FirstOrDefaultAsync();
            if (sportsplan == null)
                return null;

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
        /// Lädt einen einzelnen Trainingsplan inklusive Übungen.
        /// </summary>
        public async Task<TrainingsplanModel> GetTrainingsplanByIdAsync(int id)
        {
            await Init();

            var tp = await _database.Table<TrainingsplanModel>()
                                    .Where(t => t.Id == id)
                                    .FirstOrDefaultAsync();
            if (tp == null)
                return null;

            tp.Exercise = await _database.Table<ExerciseModel>()
                                         .Where(ex => ex.TrainingsplanId == tp.Id)
                                         .ToListAsync();
            return tp;
        }

        /// <summary>
        /// Lädt eine einzelne Übung.
        /// </summary>
        public async Task<ExerciseModel> GetExerciseByIdAsync(int id)
        {
            await Init();
            return await _database.Table<ExerciseModel>().Where(ex => ex.Id == id).FirstOrDefaultAsync();
        }
        #endregion

        #region --- Save Items ---
        public async Task<int> SaveSportplanAsync(SportsplanModel sportplan)
        {
            await Init();

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
        public async Task DeleteSportplanAsync(SportsplanModel sportsplan)
        {
            if (sportsplan == null)
                throw new ArgumentNullException(nameof(sportsplan));

            await Init();

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
