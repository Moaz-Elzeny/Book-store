using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BookeStore.Models.Repositories;
using Bookstore.Models;
using Bookstore.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookstorRepository<Book> bookRepository;
        private readonly IBookstorRepository<Author> authorRepository;
        private readonly IHostingEnvironment hosting;

        public BookController(IBookstorRepository<Book> bookRepository,
            IBookstorRepository<Author> authorRepository,
            IHostingEnvironment hosting) 
        {
            this.bookRepository = bookRepository;
            this.authorRepository = authorRepository;
            this.hosting = hosting;
        }
        // GET: BookController
        public ActionResult Index()
        {
            var books = bookRepository.List();

            return View(books);
        }

        // GET: BookController/Details/5
        public ActionResult Details(int id)
        {
            var book = bookRepository.Find(id);

            return View(book);
        }

        // GET: BookController/Create
        public ActionResult Create()
        {
            var model = new BookAuthorViewModel
            {
                Authors = FillSelectList()
            };
            return View(model);
        }

        // POST: BookController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BookAuthorViewModel model)
        {
            if (ModelState.IsValid) 
            {
                try
                {
                    string fileName = UploadFile(model.File) ?? string.Empty;
                   

                    if (model.AuthorId == -1)
                    {
                        ViewBag.Message = "Please select an author from list!";

                        return View(GetAllAuthors());
                    }

                    var author = authorRepository.Find(model.AuthorId);
                    Book book = new Book
                    {
                        Id = model.BookId,
                        Title = model.Title,
                        Description = model.Description,
                        Author = author,
                        ImageUrl = fileName
                    };

                    bookRepository.Add(book);

                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View();
                }

            }
            ModelState.AddModelError("", "You have to fill all the required fields");
            return View(GetAllAuthors());
        }

        // GET: BookController/Edit/5
        public ActionResult Edit(int id)
        {
            var book = bookRepository.Find(id);
            var authorId = book.Author == null ? book.Author.Id = 0 : book.Author.Id;

            var viewModel = new BookAuthorViewModel
            {
                BookId = book.Id,
                Title = book.Title,
                Description = book.Description,
                AuthorId = book.Author.Id,
                Authors = authorRepository.List().ToList(),
                ImageUrl = book.ImageUrl
                
            };

            return View(viewModel);
        }

        // POST: BookController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BookAuthorViewModel viewModel)
        {
            try
            {
                string fileName = UploadFile(viewModel.File, viewModel.ImageUrl);


                var author = authorRepository.Find(viewModel.AuthorId);
                Book book = new Book
                {
                    Id = viewModel.BookId,
                    Title = viewModel.Title,
                    Description = viewModel.Description,
                    Author = author,
                    ImageUrl = fileName
                };

                bookRepository.Update(viewModel.BookId, book);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: BookController/Delete/5
        public ActionResult Delete(int id)
        {
            var book = bookRepository.Find(id);

            return View(book);
        }

        // POST: BookController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmDelete(int id)
        {
            try
            {
                bookRepository.Delete(id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        List<Author> FillSelectList()
        {
            var authors = authorRepository.List().ToList();
            authors.Insert(0, new Author { Id = -1, FullName = "------" });
            return authors;
        }

        BookAuthorViewModel GetAllAuthors()
        {
            var vmodel = new BookAuthorViewModel
            {
                Authors = FillSelectList()
            };

            return vmodel;
        }
        
        string UploadFile(IFormFile file)
        {
            if (file != null)
            {
                string uploads = Path.Combine(hosting.WebRootPath, "uploads");
                string fullpath = Path.Combine(uploads, file.FileName);
                file.CopyTo(new FileStream(fullpath, FileMode.Create));

                return file.FileName;
            }
            return null;
        }

        string UploadFile(IFormFile file, string imageUrl)
        {
            if (file != null)
            {
                string uploads = Path.Combine(hosting.WebRootPath, "uploads");
                string newpath = Path.Combine(uploads, file.FileName);

                string oldPath = Path.Combine(uploads, imageUrl);

                if (oldPath != newpath)
                {
                    //delate the old file
                    System.IO.File.Delete(oldPath);

                    //save the new file
                    file.CopyTo(new FileStream(newpath, FileMode.Create));

                }
                return file.FileName;
            }
            return imageUrl;
        }

        public ActionResult Search(string term)
        {
            var result = bookRepository.Search(term);
            return View("Index", result);
        }
      }
    }

