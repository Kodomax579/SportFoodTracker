using SportFoodTracker.Context.Sportplan;
using SportFoodTracker.Models.enums;
using SportFoodTracker.Models.Sportsplan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportFoodTracker.Service
{
    public class SportsplanService
    {
        private List<TrainingProgramModel> sportsplans;
        private List<TrainingProgramModel> activeSportsplan;

        private SportsPlanDatabase _sportsPlanDatabase;

        public SportsplanService(SportsPlanDatabase sportsPlanDatabase)
        {
            _sportsPlanDatabase = sportsPlanDatabase;
            activeSportsplan = new List<TrainingProgramModel>();
            sportsplans = new List<TrainingProgramModel>();
        }

        public async Task<List<TrainingProgramModel>> GetAllAsync()
        {
            var plans = await _sportsPlanDatabase.GetAllSportsplansAsync();
            sportsplans = plans;
            return plans;
        }

        public async Task<List<TrainingProgramModel>> GetActiveSportsplanAsync()
        {
            var plans = await _sportsPlanDatabase.GetAllSportsplansAsync();
            activeSportsplan = plans.Where(plan => plan.IsActive == true).ToList();
            return activeSportsplan;
        }

        public async Task<List<ExerciseEntryModel>> GetAllExerciseAsync()
        {
            var exercises = await _sportsPlanDatabase.GetAllExercisesAsync();
            return exercises;
        }

        public async Task<TrainingProgramModel> GetWorkoutByIdAsync(int id)
        {
            var plan = await _sportsPlanDatabase.GetSportsplanByIdAsync(id);
            return plan;
        }

        public async Task<WorkoutSessionModel> GetTrainingsplanByIdAsync(int id)
        {
            var plan = await _sportsPlanDatabase.GetTrainingsplanByIdAsync(id);
            return plan;
        }

        public async Task CreateNewSportsplan(TrainingProgramModel sportsplan)
        {
            if(sportsplan == null)
            {
                throw new ArgumentNullException(nameof(sportsplan), "Sportsplan cannot be null");
            }
            await _sportsPlanDatabase.SaveSportplanAsync(sportsplan);
            
        }

        public async Task UpdateSportsplan(TrainingProgramModel sportsplan)
        {
            if (sportsplan == null)
            {
                throw new ArgumentNullException(nameof(sportsplan), "Sportsplan cannot be null");
            }
            
            await _sportsPlanDatabase.SaveSportplanAsync(sportsplan);
        }

        public async Task DeleteSportsplan(TrainingProgramModel sportsplan)
        {
            if (sportsplan == null)
            {
                throw new ArgumentNullException(nameof(sportsplan), "Sportsplan cannot be null");
            }
            await _sportsPlanDatabase.DeleteSportplanAsync(sportsplan);
        }
    }
}
