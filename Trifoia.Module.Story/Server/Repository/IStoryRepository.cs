using System.Collections.Generic;
using Trifoia.Module.Story.Models;

namespace Trifoia.Module.Story.Repository
{
    public interface IStoryRepository
    {
        IEnumerable<Models.Story> GetStorys(int ModuleId);
        Models.Story GetStory(int StoryId);
        Models.Story GetStory(int StoryId, bool tracking);
        Models.Story AddStory(Models.Story Story);
        Models.Story UpdateStory(Models.Story Story);
        void DeleteStory(int StoryId);
    }
}
