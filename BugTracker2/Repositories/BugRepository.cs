using BugTracker2.Interfaces;
using BugTracker2.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker2.Repositories
{
    public class BugRepository : IBugRepository
    {
        public DataContext _dataContext { get; set; }
        public BugRepository(DataContext context)
        {
            _dataContext = context;
        }
        public async Task<Bug> GetBugById(int id) => await _dataContext.Bugs.FirstOrDefaultAsync(x => x.Id == id);
    }
}
