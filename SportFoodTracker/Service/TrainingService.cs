using SportFoodTracker.Context.TrainingDatabase;
using SportFoodTracker.Models.Training;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SportFoodTracker.Service
{
    public class TrainingService
    {
        private TrainingDatabase _trainingDatabase;
        private SportsplanService _sportsplanService;

        public TrainingService(TrainingDatabase trainingDatabase, SportsplanService sportsplanService)
        {
            _trainingDatabase = trainingDatabase;
            _sportsplanService = sportsplanService;
        }

        public async Task<int> SaveTrainingAsync(TrainingModel training)
        {
            if (training == null)
            {
                throw new ArgumentNullException(nameof(training), "Training cannot be null");
            }
            return await _trainingDatabase.SaveTrainingAsync(training);
        }

        public async Task<List<TrainingModel>> GetAllTrainingsAsync()
        {
            var trainings = await _trainingDatabase.GetAllTrainingsAsync();
            var exercises = await _sportsplanService.GetAllExerciseAsync();
            foreach (var training in trainings)
            {
                foreach (var repetion in training.TrainingSets)
                {
                    repetion.Exercise = exercises.FirstOrDefault(ex => ex.Id == repetion.ExerciseId)!;
                }
            }
            return trainings;
        }

        public async Task<bool> IsTodayTrainingDoneAsync()
        {
            var today = DateTime.Today;

            var trainings = await _trainingDatabase.GetAllTrainingsAsync();
            var training = trainings.Where(t => t.Date.Date == today)
                .FirstOrDefault();

            return training?.IsDone == true;
        }
    }
}
