using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Book_Store_App.Data
{
    public class Language
    {
        public Language()
        {
            Books = new HashSet<Book>();
        }
        public int Id { get; set; }
        public string LanguageName { get; set; }

        public IEnumerable<Book> Books { get; set; }
    }
}
