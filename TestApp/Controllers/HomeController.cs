using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestApp.Models;
using TestApp.Models.ViewModel;

namespace TestApp.Controllers
{
    /// <summary>
    /// Controller to handle work with library
    /// User needs to be authorized to have access here
    /// </summary>
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IClient client;

        public HomeController(IClient client) => this.client = client;

        /// <summary>
        /// Home will show all books and two other lists that are filtered by if the book is borrowed or not
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Borrowed = client.GetBooks().Where(b => b.IsBorrowed()).Select(b => CreateBook(b)).ToList();
            ViewBag.Free = client.GetBooks().Where(b => !b.IsBorrowed()).Select(b => CreateBook(b)).ToList();
            return View(client.GetBooks().Select(b => CreateBook(b)).ToList());
        }

        /// <summary>
        /// Take instance of object that implements IBook interface 
        /// and create instance of ViewModel.Book
        /// </summary>
        /// <param name="b"></param>
        /// <returns>Book if parameter not empty, otherwise null</returns>
        private Book CreateBook(IBook b)
        {
            if (b == null)
                return null;
            return new Book
            {
                Id = b.GetId(),
                Name = b.GetName(),
                Author = b.GetAuthor(),
                Borrowed = new Borrowed
                {
                    FirstName = b.GetBorrowed().GetFirstName(),
                    LastName = b.GetBorrowed().GetLastName(),
                    From = b.GetBorrowed().GetFrom()
                }
            };
        }

        /// <summary>
        /// Show edit form of book
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();
            Book book = CreateBook(await client.FindAsync(id.Value));
            if (book == null)
                return NotFound();
            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Book book)
        {
            if (id != book.Id)
            {
                Static.Logger.Info("Edit book, link id is not equal to object id");
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                Book bookToSave = CreateBook(await client.FindAsync(id));
                if (bookToSave == null || bookToSave.Id == 0)
                    return NotFound();

                //bookToSave.Name = book.Name;
                //bookToSave.Author = book.Author;

                try
                {
                    client.Update(bookToSave);
                    await client.SaveChangesAsync();
                    ViewData["Message"] = $"Kniha {bookToSave.Name} bola upravena a ulozena";
                }
                catch (Exception ex)
                {
                    Static.Logger.Fatal("Could not add or update book", ex);
                }
            }
            else
                Static.Logger.Info("Not valid state of book (edit action)");

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                Static.Logger.Info("Call Details on Book with empty id parameter");
                return NotFound();
            }
            Book book = CreateBook(await client.FindAsync(id.Value));
            if (book == null)
                return NotFound();
            return View(book);
        }

        public IActionResult Create() => View();
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book book)
        {
            if (ModelState.IsValid)
            {
                client.Add(book);
                if (book.Id > 0)
                    await client.SaveChangesAsync();
                else
                {
                    Static.Logger.Fatal("Could create book");
                    goto End;
                }

                ViewData["Message"] = $"Kniha {book.Name} bola ulozena";
                return RedirectToAction(nameof(Index));
            }

            End:
            return RedirectToAction(nameof(Create));
        }

        [HttpGet]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                client.Remove(await client.FindAsync(id));
                await client.SaveChangesAsync();
            } catch (Exception ex)
            {
                Static.Logger.Fatal("Could not remove book", ex);
            }

            return RedirectToAction(nameof(Index));
        } 
    }
}
