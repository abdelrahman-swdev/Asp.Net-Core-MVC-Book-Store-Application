using Book_Store_App.Data;
using Book_Store_App.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Book_Store_App.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly ApplicationDbContext _context;
        public BookRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync() => await _context.Books.ToListAsync();
        

        public async Task<IEnumerable<Book>> GetTopBooksAsync(int numOfBooksToReturn)
        {
            IQueryable<Book> query = _context.Books.Take(numOfBooksToReturn);
            return await query.ToListAsync();
        }

        public async Task<Book> GetByIdAsync(int id)
        {
            IQueryable<Book> books = _context.Books
                .Where(b => b.Id == id)
                .Select(b => new Book()
                {
                    Id = b.Id,
                    Author = b.Author,
                    Category = b.Category,
                    Description = b.Description,
                    LangName = b.Language.LanguageName,
                    LanguageId = b.LanguageId.HasValue ? b.LanguageId.Value : 0,
                    PublicationDate = b.PublicationDate,
                    Title = b.Title,
                    TotalPages = b.TotalPages,
                    UpdatedOn = b.UpdatedOn,
                    CoverPhotoUrl = b.CoverPhotoUrl,
                    Gallery = b.Gallery,
                    BookPdfUrl = b.BookPdfUrl
                });


            Book book = await books.FirstOrDefaultAsync();

            if(book != null)
            {
                return book;
            }
            return null;
        }

        public async Task<int> AddNewBookAsync(Book book)
        {
            book.PublicationDate = DateTime.UtcNow;
            book.UpdatedOn = DateTime.UtcNow;

            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();

            return book.Id;
        }
    }
}
