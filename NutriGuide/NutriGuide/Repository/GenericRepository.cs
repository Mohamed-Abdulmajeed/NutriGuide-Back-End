using NutriGuide.Models;

namespace NutriGuide.Repository
{
    public class GenericRepository<TEntity> where TEntity : class
    {
        public NutriGuideDbContext Db;
        public GenericRepository(NutriGuideDbContext db)
        {
            Db = db;
        }
        // getAll
        public virtual List<TEntity>? GetAll()
        {
            return Db.Set<TEntity>().ToList();
        }

        //getById
        public virtual TEntity? GetById(int? id)
        {
            return Db.Set<TEntity>().Find(id)!;
        }
        //add
        public virtual void Add(TEntity s)
        {
            Db.Set<TEntity>().Add(s);
        }
        //edit
        public virtual void Edit(TEntity s)
        {
            Db.Entry(s).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }
        //delete
        public virtual void Delete(int id)
        {
            TEntity t = GetById(id)!;
            Db.Set<TEntity>().Remove(t);
        }



    }
}

