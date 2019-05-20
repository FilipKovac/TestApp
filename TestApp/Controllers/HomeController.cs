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
            ViewBag.Borrowed = client.GetBooks().Where(b => b.IsBorrowed()).Select(b => new Book(b)).ToList();
            ViewBag.Free = client.GetBooks().Where(b => !b.IsBorrowed()).Select(b => new Book(b)).ToList();
            return View(client.GetBooks().Select(b => new Book(b)).ToList());
        }

        /// <summary>
        /// Show edit form of book
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Edit(int? id)
        {
            if (!id.HasValue)
                return NotFound();
            try
            {
                Book book = new Book(await client.FindAsync(id.Value));
                return View(book);
            }
            catch (ArgumentNullException ex)
            {
                Static.Logger.Fatal("Could not find book to edit", ex);
                return NotFound();
            }
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
                try
                {
                    client.Update(book);
                    await client.SaveChangesAsync();
                    ViewData["Message"] = $"Kniha {book.Name} bola upravena a ulozena";
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
            try
            {
                Book book = new Book(await client.FindAsync(id.Value));
                return View(book);
            } catch (ArgumentNullException ex)
            {
                Static.Logger.Fatal($"Could not get details of book with id {id}", ex);
                return NotFound();
            }
        }

        // GET Home/Create
        public IActionResult Create() => View();

        // POST Home/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book book)
        {
            if (ModelState.IsValid)
            {
                if (client.Add(book).GetId() > 0)
                    await client.SaveChangesAsync();
                else
                {
                    ViewData["Message"] = "Knihu sa nepodarilo pridat. Skuste neskor prosim.";
                    Static.Logger.Fatal("Could not create a book");
                    goto End;
                }

                ViewData["Message"] = $"Kniha {book.Name} bola ulozena";
                return RedirectToAction(nameof(Index));
            }

            End:
            return RedirectToAction(nameof(Create));
        }

        [HttpGet]
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
