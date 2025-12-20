using Microsoft.EntityFrameworkCore;
using NutriGuide.Models;

namespace NutriGuide.Repository
{
    public class MedicineRepository : GenericRepository<Medicine>
    {
        public MedicineRepository(NutriGuideDbContext db) : base(db)
        {

        }

        // getAll
        public List<Medicine>? GetAllIncluding()
        {
            return Db.Medicines.Include(m=>m.MedicineTimes).ToList();
        }

    }
}
