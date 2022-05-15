using Bookstore.Models;
using System.Collections.Generic;

namespace BookeStore.Models.Repositories
{
    public interface IBookstorRepository<TEntity>
    {
        IList<TEntity> List();

        TEntity Find(int id);

        void Add(TEntity entity);

        void Update(int id ,TEntity entity);

        void Delete(int id);

         List<TEntity> Search(string term);
    }
}