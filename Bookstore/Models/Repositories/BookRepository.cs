using Bookstore.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookeStore.Models.Repositories
{
    public class BookRepository : IBookstorRepository<Book>
    {
        List<Book> books;

        public BookRepository()
        {
            books = new List<Book>()
            {
                new Book
                {
                    Id = 1, Title = "C# programing",
                    Description = "No description",
                    ImageUrl = "csharp.jpg",
                    Author = new Author{Id = 2} 
                },
                new Book
                {
                    Id = 2, Title = "Java programing", 
                    Description = "Nothing", 
                    ImageUrl = "java.jpg",
                    Author = new Author()
                },
                new Book
                {
                    Id = 3, Title = "Paython programing",
                    Description = "No data",
                    ImageUrl = "paython.jpg",
                    Author = new Author()
                },
              };
            }
        

        public void Add(Book entity)
        {
            entity.Id = books.Max(b => b.Id) + 1;
            books.Add(entity);
        }

        public void Delete(int id)
        {
            var book = Find(id);
            books.Remove(book);
        }

        public Book Find(int id)
        {
            var Book = books.SingleOrDefault(b => b.Id == id);
            return Book;
        }

        public IList<Book> List()
        {
           return books;
        }

        public List<Book> Search(string term)
        {
            return books.Where(a => a.Title.Contains(term)).ToList();
        }

        public void Update(int id,Book newBook)
        {
            var book = Find(id);
            book.Title = newBook.Title;
            book.Description = newBook.Description;
            book.Author = newBook.Author;
            book.ImageUrl = newBook.ImageUrl;
        }

       /* IList<Book> IBookstorRepository<Book>.List()
        {
            throw new System.NotImplementedException();
        }*/
    }
}
