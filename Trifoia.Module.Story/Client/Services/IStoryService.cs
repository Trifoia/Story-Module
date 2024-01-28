using System.Collections.Generic;
using System.Threading.Tasks;
using Trifoia.Module.Story.Models;

namespace Trifoia.Module.Story.Services
{
    public interface IStoryService 
    {
        Task<List<Models.Story>> GetStorysAsync(int ModuleId);

        Task<Models.Story> GetStoryAsync(int StoryId, int ModuleId);

        Task<Models.Story> AddStoryAsync(Models.Story Story);

        Task<Models.Story> UpdateStoryAsync(Models.Story Story);

        Task DeleteStoryAsync(int StoryId, int ModuleId);
    }
}
