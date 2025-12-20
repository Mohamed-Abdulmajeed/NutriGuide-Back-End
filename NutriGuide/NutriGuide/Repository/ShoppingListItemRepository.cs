using Microsoft.EntityFrameworkCore;
using NutriGuide.Models;

namespace NutriGuide.Repository
{
    public class ShoppingListItemRepository: GenericRepository<ShoppingListItem>
    {
        public ShoppingListItemRepository(NutriGuideDbContext db) : base(db)
        {
        }

        //get customer by id

        public List<ShoppingListItem>? GetWithInclude()
        {
            return Db.ShoppingListItems.Include(c => c.Ingredient).ToList();
        }
    }
}
