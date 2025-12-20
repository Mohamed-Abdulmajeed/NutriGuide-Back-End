using NutriGuide.Models;

namespace NutriGuide.Repository
{
    public class SystemTypeRepository : GenericRepository<SystemTypePlan>
    {
        public SystemTypeRepository(NutriGuideDbContext db) : base(db)
        {

        }

        // get By name
        public SystemTypePlan? GetByName(string name)
        {
            return Db.SystemTypePlans.FirstOrDefault(gp => gp.Name == name);
        }
    }
}
