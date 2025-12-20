using Microsoft.EntityFrameworkCore;
using NutriGuide.Models;

namespace NutriGuide.Repository
{
    public class PlanRepository : GenericRepository<Plan>
    {
        public PlanRepository(NutriGuideDbContext db) : base(db)
        {

        }
        // get By id 
        public Plan? GetwithInclude(int id)
        {
            return Db.Plans.Include(p => p.Meals).ThenInclude(i => i.IngredientMeals).ThenInclude(i => i.Ingredient).FirstOrDefault(m => m.Id == id);
        }
        // get all with iclude By id 
        public List<Plan>? GetAllwithInclude(int custId)
        {
            return Db.Plans.Where(p => p.CustomerId == custId)
                            .OrderByDescending(p => p.StartDate) // ترتيب من الأحدث للأقدم
                            .Take(15)                             // آخر 15 خطة
                            .Include(p => p.Goal)
                            .Include(p => p.SystemType)
                            .Include(p => p.Meals)
                                .ThenInclude(m => m.IngredientMeals)
                                .ThenInclude(im => im.Ingredient)
                            .ToList();
        }
        //get current
        public Plan? GetCurrentPlan(int id)
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);

            var plans = Db.Plans.Include(p => p.Goal)
                                .Include(p => p.SystemType)
                                .Include(p => p.Meals)
                                .ThenInclude(m => m.IngredientMeals)
                                .ThenInclude(i => i.Ingredient)
                                .Where(p => p.CustomerId == id && p.StartDate <= today)
                                .ToList();

            DateTime now = DateTime.Now;

            return plans.LastOrDefault(p =>
            {
                var start = p.StartDate.ToDateTime(TimeOnly.MinValue);
                var end = p.StartDate.AddDays(p.NumberOfDays).ToDateTime(TimeOnly.MaxValue);

                return start <= now && end >= now;
            });
        }




    }
}
