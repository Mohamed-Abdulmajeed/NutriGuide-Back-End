using NutriGuide.Models;

namespace NutriGuide.Repository
{
    public class GoalPlanRepository : GenericRepository<GoalPlan>
    {
        public GoalPlanRepository(NutriGuideDbContext db) : base(db)
        {

        }

        // get By name
        public GoalPlan? GetByName(string name)
        {
            return Db.GoalPlans.FirstOrDefault(gp => gp.Name == name);
        }

    }
}
