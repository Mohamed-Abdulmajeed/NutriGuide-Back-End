using NutriGuide.Models;

namespace NutriGuide.Repository
{
    public class IngredientRepository : GenericRepository<Ingredient>
    {
        public IngredientRepository(NutriGuideDbContext db) : base(db)
        {

        }
        // get By Name And Unit 
        public Ingredient? GetByNameAndUnit(string name, string unit)
        {
            return Db.Ingredients.Where(i => i.Name == name && i.Unit == unit).FirstOrDefault();
        }

    }
}
