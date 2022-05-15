using BookeStore.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookstore.Models.Repositories
{
    public class AuthorRepository : IBookstorRepository<Author>
    {
        IList<Author> authors;

        public AuthorRepository()
        {
            authors = new List<Author>()
            {
                new Author
                {
                    Id = 1, FullName = "Moaz Elzeny"
                },
                new Author
                {
                    Id = 2, FullName = "Mo Mohamed"
                },
                new Author
                {
                Id = 3, FullName = "Moaz Mohamed"
                },
              };
        }

        public void Add(Author entity)
        {
            entity.Id = authors.Max(a => a.Id) + 1;
            authors.Add(entity);
        }

        public void Delete(int id)
        {
            var author = Find(id);

            authors.Remove(author);
        }

        public Author Find(int id)
        {
            var author = authors.SingleOrDefault(a=> a.Id ==id);
            return author;
        }

        public IList<Author> List()
        {
            return authors;
        }

        public List<Author> Search(string term)
        {
            return authors.Where(a => a.FullName.Contains(term)).ToList();
        }

        public void Update(int id, Author newauthor)
        {
            var author = Find(id);

            author.FullName = newauthor.FullName;
        }
    }
}
