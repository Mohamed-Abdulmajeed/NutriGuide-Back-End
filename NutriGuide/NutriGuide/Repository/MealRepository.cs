using Microsoft.EntityFrameworkCore;
using NutriGuide.Models;

namespace NutriGuide.Repository
{
    public class MealRepository : GenericRepository<Meal>
    {
        public MealRepository(NutriGuideDbContext db) : base(db)
        {

        }

        // get By id 
        public Meal? GetById(int id)
        {
            return Db.Meals.Include(m => m.IngredientMeals).ThenInclude(m => m.Ingredient).FirstOrDefault(m => m.Id == id);
        }

        public Meal? GetIncludeCustomer(int id)
        {
            return Db.Meals
                .Include(m => m.Customers)
                .FirstOrDefault(m => m.Id == id);
        }
        public List<Meal>? GetAllFavorite(int customerId)
        {
            return Db.Meals
                .Include(m => m.Customers).Include(m => m.IngredientMeals).ThenInclude(m => m.Ingredient)
                .Where(m => m.Customers.Any(c => c.Id == customerId))
                .ToList();
        }
    }
}
