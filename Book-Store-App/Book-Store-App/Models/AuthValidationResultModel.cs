using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Book_Store_App.Models
{
    public class AuthValidationResultModel
    {
        public AuthValidationResultModel()
        {
            errors = new HashSet<string>();
        }
        public bool success { get; set; }

        public int Id { get; set; }

        public IEnumerable<string> errors { get; set; }
    }
}
