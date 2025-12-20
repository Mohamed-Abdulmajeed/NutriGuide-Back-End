using NutriGuide.Models;

namespace NutriGuide.Repository
{
    public class UserRepository : GenericRepository<User>
    {
        public UserRepository(NutriGuideDbContext db) : base(db)
        {

        }

        // getAll
        public List<User>? GetAll(string role)
        {
            return Db.Users.Where(u => u.Role == role).ToList();
        }

        // get By Email 
        public User? GetByEmail(string email)
        {
            return Db.Users.Where(u => u.Email == email).FirstOrDefault();
        }

    }
}

