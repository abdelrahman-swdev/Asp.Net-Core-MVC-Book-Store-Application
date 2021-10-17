using AutoMapper;
using Book_Store_App.Data;
using Book_Store_App.Interfaces;
using Book_Store_App.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Book_Store_App.Repositories
{
    public class LanguageRepository : ILanguageRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public LanguageRepository(ApplicationDbContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<IEnumerable<LanguageModel>> GetAllLanguagesAsync()
        {
            return _mapper.Map<IEnumerable<LanguageModel>>(await _context.Languages.ToListAsync());
            
        }
    }
}
