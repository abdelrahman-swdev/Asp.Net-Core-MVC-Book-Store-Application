using Book_Store_App.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Book_Store_App.Interfaces
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetAllBooksAsync();


        Task<IEnumerable<Book>> GetTopBooksAsync(int numOfBooksToReturn);

        Task<Book> GetByIdAsync(int id);


        Task<int> AddNewBookAsync(Book book);

    }
}
