using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Oqtane.Modules;
using Oqtane.Models;
using Oqtane.Infrastructure;
using Oqtane.Enums;
using Oqtane.Repository;
using Trifoia.Module.Story.Repository;

namespace Trifoia.Module.Story.Manager
{
    public class StoryManager : MigratableModuleBase, IInstallable, IPortable
    {
        private readonly IStoryRepository _StoryRepository;
        private readonly IDBContextDependencies _DBContextDependencies;

        public StoryManager(IStoryRepository StoryRepository, IDBContextDependencies DBContextDependencies)
        {
            _StoryRepository = StoryRepository;
            _DBContextDependencies = DBContextDependencies;
        }

        public bool Install(Tenant tenant, string version)
        {
            return Migrate(new StoryContext(_DBContextDependencies), tenant, MigrationType.Up);
        }

        public bool Uninstall(Tenant tenant)
        {
            return Migrate(new StoryContext(_DBContextDependencies), tenant, MigrationType.Down);
        }

        public string ExportModule(Oqtane.Models.Module module)
        {
            string content = "";
            List<Models.Story> Storys = _StoryRepository.GetStorys(module.ModuleId).ToList();
            if (Storys != null)
            {
                content = JsonSerializer.Serialize(Storys);
            }
            return content;
        }

        public void ImportModule(Oqtane.Models.Module module, string content, string version)
        {
            List<Models.Story> Storys = null;
            if (!string.IsNullOrEmpty(content))
            {
                Storys = JsonSerializer.Deserialize<List<Models.Story>>(content);
            }
            if (Storys != null)
            {
                foreach(var Story in Storys)
                {
                    _StoryRepository.AddStory(new Models.Story { ModuleId = module.ModuleId, Name = Story.Name });
                }
            }
        }
    }
}
