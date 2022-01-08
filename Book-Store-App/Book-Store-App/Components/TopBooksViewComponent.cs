using AutoMapper;
using Book_Store_App.Data;
using Book_Store_App.Interfaces;
using Book_Store_App.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Book_Store_App.Components
{
    public class TopBooksViewComponent : ViewComponent
    {
        private readonly IMapper _mapper;

        public IBookRepository _bookRepository { get; set; }
        public TopBooksViewComponent(IBookRepository bookRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
        }
        public async Task<IViewComponentResult> InvokeAsync(int numOfBooksToReturn)
        {
            IEnumerable<Book> books = await _bookRepository.GetTopBooksAsync(numOfBooksToReturn);
            IEnumerable<BookModel> models = _mapper.Map<IEnumerable<BookModel>>(books);
            return View(models);
        }
    }
}
