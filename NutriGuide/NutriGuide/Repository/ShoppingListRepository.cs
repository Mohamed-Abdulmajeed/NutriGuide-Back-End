using Microsoft.EntityFrameworkCore;
using NutriGuide.Models;

namespace NutriGuide.Repository
{
    public class ShoppingListRepository : GenericRepository<ShoppingList>
    {
        public ShoppingListRepository(NutriGuideDbContext db) : base(db)
        {

        }
        //  GetWithItems
        public ShoppingList? GetWithItems(int planId)
        {
            return Db.ShoppingLists.Include(s => s.ShoppingListItems).ThenInclude(i => i.Ingredient).FirstOrDefault(s => s.PlanId == planId);
        }
    }
}
