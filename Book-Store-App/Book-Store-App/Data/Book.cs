using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Book_Store_App.Data
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public int? LanguageId { get; set; }

        [NotMapped]
        public string LangName { get; set; }
        public Language Language { get; set; }
        public DateTime? PublicationDate { get; set; }
        public int TotalPages { get; set; }

        public string CoverPhotoUrl { get; set; }
        public string BookPdfUrl { get; set; }
        public string Author { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public ICollection<Gallery> Gallery { get; set; }
    }
}
