using Book_Store_App.Data;
using Book_Store_App.Helpers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Book_Store_App.Models
{
    public class BookModel
    {
        public BookModel()
        {
            Gallery = new List<GalleryModel>();
        }
        public int Id { get; set; }

        [Required]
        //[MaxLength(256)]
        [MaxLength(256, ErrorMessage = "Max langth for book title is 256 letters")]
        public string Title { get; set; }

        [Required]
        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        [Display(Name ="Book Language")]
        public int LanguageId { get; set; }
        public string LangName { get; set; }


        [Display(Name = "Publication Date")]
        public DateTime? PublicationDate { get; set; }

        [Display(Name = "Total Pages")]
        [Required]
        public int? TotalPages { get; set; }

        [Display(Name ="Upload Book Cover Photo")]
        [Required(ErrorMessage ="Cover photo is required")]
        public IFormFile CoverPhoto { get; set; }


        [Display(Name = "Upload Book Photos")]
        [Required(ErrorMessage = "Cover photo is required")]
        public IFormFileCollection GalleryFiles{ get; set; }

        public List<GalleryModel> Gallery { get; set; }

        public string CoverPhotoUrl { get; set; }

        [Required]
        public string Author { get; set; }

        [Display(Name ="Updated On")]
        public DateTime? UpdatedOn { get; set; }

        [Display(Name = "Upload book in pdf format")]
        [Required(ErrorMessage = "Book pdf is required")]
        public IFormFile BookPdf { get; set; }

        public string BookPdfUrl { get; set; }
    }
}
