using AutoMapper;
using Book_Store_App.Data;
using Book_Store_App.Interfaces;
using Book_Store_App.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Book_Store_App.Controllers
{
    [Route("books")]
    public class BookController : Controller
    {
        private readonly IBookRepository _bookRepo;
        private readonly ILanguageRepository _langRepo;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        public BookController(IBookRepository bookRepo,
            ILanguageRepository langRepo,
            IMapper mapper,
            IWebHostEnvironment env)
        {
            _bookRepo = bookRepo;
            _langRepo = langRepo;
            _mapper = mapper;
            _env = env;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllBooks()
        {
            IEnumerable<Book> books = await _bookRepo.GetAllBooksAsync();
            IEnumerable<BookModel> booksModel = _mapper.Map<IEnumerable<BookModel>>(books);
            return View(booksModel);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetBookById([FromRoute] int id)
        {
            if(id < 1)
            {
                return Content("invalid details");
            }
            var book = await _bookRepo.GetByIdAsync(id);
            if(book == null)
            {
                return Content("invalid details");
            }
            BookModel bookModel = _mapper.Map<BookModel>(book);
            return View(bookModel);
        }

        [Authorize]
        [HttpGet("new")]
        public IActionResult AddNewBook(bool isSuccess = false, int bookId = 0)
        {
            ViewBag.IsSuccess = isSuccess;
            ViewBag.BookId = bookId;
            return View()
;       }

        [HttpPost("new")]
        public async Task<IActionResult> AddNewBook(BookModel model)
        {
            if (ModelState.IsValid)
            {
                // upload the cover photo
                if(model.CoverPhoto != null)
                {
                    string uploadsFolder = "uploads/book-cover/";
                    model.CoverPhotoUrl =  await UploadFile(uploadsFolder, model.CoverPhoto);
                }

                // upload gallery photos
                if(model.GalleryFiles != null)
                {
                    string uploadsFolder = "uploads/gallery/";
                    foreach(var img in model.GalleryFiles)
                    {
                        GalleryModel gallery = new GalleryModel()
                        {
                            Name = img.FileName,
                            Url = await UploadFile(uploadsFolder, img)
                        };
                        model.Gallery.Add(gallery);
                    }
                }

                // upload the book in pdf format
                if (model.BookPdf != null)
                {
                    string uploadsFolder = "uploads/books-pdf/";
                    model.BookPdfUrl = await UploadFile(uploadsFolder, model.BookPdf);
                }

                Book book = _mapper.Map<Book>(model);
                int addedBookId = await _bookRepo.AddNewBookAsync(book);
                if(addedBookId > 0)
                {
                    return RedirectToAction(nameof(GetBookById), new { id = addedBookId });
                }
            }
            return View(model);
        }

        private async Task<string> UploadFile(string filePath, IFormFile file)
        {
            
            string photoName = Guid.NewGuid().ToString() + "_" + file.FileName;
            string fullPhotoPath = filePath + photoName;
            string FullPathOnServer = Path.Combine(_env.WebRootPath, fullPhotoPath);
            await file.CopyToAsync(new FileStream(FullPathOnServer, FileMode.Create));

            return photoName;
        }
    }
}
