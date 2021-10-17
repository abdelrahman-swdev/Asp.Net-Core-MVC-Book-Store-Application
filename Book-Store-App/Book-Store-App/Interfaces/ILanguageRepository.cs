using Book_Store_App.Data;
using Book_Store_App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Book_Store_App.Interfaces
{
    public interface ILanguageRepository
    {
        Task<IEnumerable<LanguageModel>> GetAllLanguagesAsync();
    }
}
