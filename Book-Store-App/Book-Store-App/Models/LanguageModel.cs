using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Book_Store_App.Models
{
    public class LanguageModel
    {
        public LanguageModel()
        {
            Books = new HashSet<BookModel>();
        }
        public int Id { get; set; }

        [Required]
        public string LanguageName { get; set; }

        public IEnumerable<BookModel> Books { get; set; }
    }
}
