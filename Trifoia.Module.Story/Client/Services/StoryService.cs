using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Oqtane.Modules;
using Oqtane.Services;
using Oqtane.Shared;
using Trifoia.Module.Story.Models;

namespace Trifoia.Module.Story.Services
{
    public class StoryService : ServiceBase, IStoryService, IService
    {
        public StoryService(HttpClient http, SiteState siteState) : base(http, siteState) { }

        private string Apiurl => CreateApiUrl("Story");

        public async Task<List<Models.Story>> GetStorysAsync(int ModuleId)
        {
            List<Models.Story> Storys = await GetJsonAsync<List<Models.Story>>(CreateAuthorizationPolicyUrl($"{Apiurl}?moduleid={ModuleId}", EntityNames.Module, ModuleId), Enumerable.Empty<Models.Story>().ToList());
            return Storys.OrderBy(item => item.Name).ToList();
        }

        public async Task<Models.Story> GetStoryAsync(int StoryId, int ModuleId)
        {
            return await GetJsonAsync<Models.Story>(CreateAuthorizationPolicyUrl($"{Apiurl}/{StoryId}", EntityNames.Module, ModuleId));
        }

        public async Task<Models.Story> AddStoryAsync(Models.Story Story)
        {
            return await PostJsonAsync<Models.Story>(CreateAuthorizationPolicyUrl($"{Apiurl}", EntityNames.Module, Story.ModuleId), Story);
        }

        public async Task<Models.Story> UpdateStoryAsync(Models.Story Story)
        {
            return await PutJsonAsync<Models.Story>(CreateAuthorizationPolicyUrl($"{Apiurl}/{Story.StoryId}", EntityNames.Module, Story.ModuleId), Story);
        }

        public async Task DeleteStoryAsync(int StoryId, int ModuleId)
        {
            await DeleteAsync(CreateAuthorizationPolicyUrl($"{Apiurl}/{StoryId}", EntityNames.Module, ModuleId));
        }
    }
}
