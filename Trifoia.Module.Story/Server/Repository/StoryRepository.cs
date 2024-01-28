using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using Oqtane.Modules;
using Trifoia.Module.Story.Models;

namespace Trifoia.Module.Story.Repository
{
    public class StoryRepository : IStoryRepository, ITransientService
    {
        private readonly StoryContext _db;

        public StoryRepository(StoryContext context)
        {
            _db = context;
        }

        public IEnumerable<Models.Story> GetStorys(int ModuleId)
        {
            return _db.Story.Where(item => item.ModuleId == ModuleId);
        }

        public Models.Story GetStory(int StoryId)
        {
            return GetStory(StoryId, true);
        }

        public Models.Story GetStory(int StoryId, bool tracking)
        {
            if (tracking)
            {
                return _db.Story.Find(StoryId);
            }
            else
            {
                return _db.Story.AsNoTracking().FirstOrDefault(item => item.StoryId == StoryId);
            }
        }

        public Models.Story AddStory(Models.Story Story)
        {
            _db.Story.Add(Story);
            _db.SaveChanges();
            return Story;
        }

        public Models.Story UpdateStory(Models.Story Story)
        {
            _db.Entry(Story).State = EntityState.Modified;
            _db.SaveChanges();
            return Story;
        }

        public void DeleteStory(int StoryId)
        {
            Models.Story Story = _db.Story.Find(StoryId);
            _db.Story.Remove(Story);
            _db.SaveChanges();
        }
    }
}
