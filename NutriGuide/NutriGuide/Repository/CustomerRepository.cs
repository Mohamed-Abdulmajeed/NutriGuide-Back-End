using Microsoft.EntityFrameworkCore;
using NutriGuide.Models;

namespace NutriGuide.Repository
{
    public class CustomerRepository : GenericRepository<Customer>
    {
        public CustomerRepository(NutriGuideDbContext db) : base(db)
        {
        }

        //get customer by id

        public Customer? GetWithInclude(int id)
        {
            return Db.Customers.Include(c => c.Medicines)
                .ThenInclude(m=>m.MedicineTimes)
                .Include(c => c.DailyWieghts)
                .Include(c => c.ChronicDiseases)
                .Include(c => c.AvoidFoods).FirstOrDefault(c => c.Id == id);
        }
    }
}