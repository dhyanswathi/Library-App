using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Library.Mvc.Models;
using Library.Mvc.ViewModel;

namespace Library.Mvc.Controllers
{
    public class BorrowedBooksController : Controller
    {
        private readonly LibraryDBContext _context;

        public BorrowedBooksController(LibraryDBContext context)
        {
            _context = context;
        }

        // GET: BorrowedBooks
        public async Task<IActionResult> Index()
        {
            var libraryDBContext = _context.BorrowedBooks.OrderByDescending(x => x.ReturnDate).Include(b => b.Book).Include(b => b.User);
            return View(await libraryDBContext.ToListAsync());
        }

        // GET: BorrowedBooks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.BorrowedBooks == null)
            {
                return NotFound();
            }

            var borrowedBook = await _context.BorrowedBooks
                .Include(b => b.Book)
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.BorrowId == id);
            if (borrowedBook == null)
            {
                return NotFound();
            }
            var diff = (borrowedBook.ReturnDate - borrowedBook.BorrowDate);
            var penaltyDays = int.Parse(diff.ToString());
            var penalty = penaltyDays > 30 ? ((penaltyDays - 30) * 10) : 0;

            var borrowModel = new BorrowViewModel
            {
                BorrowId = borrowedBook.BorrowId,
                BookTitle = borrowedBook.Book.Title,
                UserName = borrowedBook.User.Name,
                BorrowDate = borrowedBook.BorrowDate,
                ExpectedReturn = borrowedBook.BorrowDate.AddDays(30),
                ReturnDate = borrowedBook.ReturnDate,
                Penalty = penalty
            };

            return View(borrowModel);
        }

        // GET: BorrowedBooks/Create
        public IActionResult Create()
        {
            ViewData["BookId"] = new SelectList(_context.Books, "BookId", "Title");
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email");
            return View();
        }

        // POST: BorrowedBooks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BorrowId,BorrowDate,ReturnDate,BookId,UserId")] BorrowedBook borrowedBook)
        {
            if (ModelState.IsValid)
            {
                _context.Add(borrowedBook);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookId"] = new SelectList(_context.Books, "BookId", "Title", borrowedBook.BookId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", borrowedBook.UserId);
            return View(borrowedBook);
        }

        // GET: BorrowedBooks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.BorrowedBooks == null)
            {
                return NotFound();
            }

            var borrowedBook = await _context.BorrowedBooks.FindAsync(id);
            if (borrowedBook == null)
            {
                return NotFound();
            }
            ViewData["BookId"] = new SelectList(_context.Books, "BookId", "Title", borrowedBook.BookId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", borrowedBook.UserId);
            return View(borrowedBook);
        }

        // POST: BorrowedBooks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BorrowId,BorrowDate,ReturnDate,BookId,UserId")] BorrowedBook borrowedBook)
        {
            if (id != borrowedBook.BorrowId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(borrowedBook);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BorrowedBookExists(borrowedBook.BorrowId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookId"] = new SelectList(_context.Books, "BookId", "Title", borrowedBook.BookId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", borrowedBook.UserId);
            return View(borrowedBook);
        }

        // GET: BorrowedBooks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.BorrowedBooks == null)
            {
                return NotFound();
            }

            var borrowedBook = await _context.BorrowedBooks
                .Include(b => b.Book)
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.BorrowId == id);
            if (borrowedBook == null)
            {
                return NotFound();
            }

            return View(borrowedBook);
        }

        // POST: BorrowedBooks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.BorrowedBooks == null)
            {
                return Problem("Entity set 'LibraryDBContext.BorrowedBooks'  is null.");
            }
            var borrowedBook = await _context.BorrowedBooks.FindAsync(id);
            if (borrowedBook != null)
            {
                _context.BorrowedBooks.Remove(borrowedBook);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BorrowedBookExists(int id)
        {
          return (_context.BorrowedBooks?.Any(e => e.BorrowId == id)).GetValueOrDefault();
        }
    }
}
