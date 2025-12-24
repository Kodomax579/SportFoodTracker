using SportFoodTracker.Context.Sportplan;
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
        private List<WorkoutModel> sportsplans;
        private WorkoutModel activeSportsplan;

        private SportsPlanDatabase _sportsPlanDatabase;

        public SportsplanService(SportsPlanDatabase sportsPlanDatabase)
        {
            _sportsPlanDatabase = sportsPlanDatabase;
            activeSportsplan = new WorkoutModel();
            sportsplans = new List<WorkoutModel>();
        }

        public async Task<List<WorkoutModel>> GetAllAsync()
        {
            var plans = await _sportsPlanDatabase.GetAllSportsplansAsync();
            sportsplans = plans;
            return plans;
        }

        public async Task<WorkoutModel> GetActiveSportsplanAsync()
        {
            var plans = await _sportsPlanDatabase.GetAllSportsplansAsync();
            activeSportsplan = plans.FirstOrDefault(plan => plan.IsActive)!;
            return activeSportsplan;
        }

        public async Task<List<ExerciseModel>> GetAllExerciseAsync()
        {
            var exercises = await _sportsPlanDatabase.GetAllExercisesAsync();
            return exercises;
        }

        public async Task<WorkoutModel> GetByIdAsync(int id)
        {
            var plan = await _sportsPlanDatabase.GetSportsplanByIdAsync(id);
            return plan;
        }

        public async Task CreateNewSportsplan(WorkoutModel sportsplan)
        {
            if(sportsplan == null)
            {
                throw new ArgumentNullException(nameof(sportsplan), "Sportsplan cannot be null");
            }
            await _sportsPlanDatabase.SaveSportplanAsync(sportsplan);
            
        }

        public async Task UpdateSportsplan(WorkoutModel sportsplan)
        {
            if (sportsplan == null)
            {
                throw new ArgumentNullException(nameof(sportsplan), "Sportsplan cannot be null");
            }
            await ResetActiveStatus(sportsplan);
            sportsplan.IsActive = true;
            activeSportsplan = sportsplan;
            await _sportsPlanDatabase.SaveSportplanAsync(sportsplan);
        }

        public async Task DeleteSportsplan(WorkoutModel sportsplan)
        {
            if (sportsplan == null)
            {
                throw new ArgumentNullException(nameof(sportsplan), "Sportsplan cannot be null");
            }
            await _sportsPlanDatabase.DeleteSportplanAsync(sportsplan);
        }

        private async Task ResetActiveStatus(WorkoutModel sportsplan)
        {
            foreach(var plan in sportsplans)
            {
                plan.IsActive = false;
                await _sportsPlanDatabase.SaveSportplanAsync(plan);
            }
        }

        public TrainingsplanModel GetTodaysTrainingsPlanAsync(string DayOfWeek)
        {
            return activeSportsplan.Trainingsplan.FirstOrDefault(tp => tp.DayOfWeek.ToString() == DayOfWeek)!;
        }
    }
}
