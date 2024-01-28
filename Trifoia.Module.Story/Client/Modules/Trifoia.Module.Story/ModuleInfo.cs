using Oqtane.Models;
using Oqtane.Modules;

namespace Trifoia.MindBoba.Client.Modules.Ink
{
    public class ModuleInfo : IModule
    {
        public ModuleDefinition ModuleDefinition => new ModuleDefinition
        {
            Name = "Story",
            Description = "Renders Ink content, interfaces with the Lottie module for images and animations",
            Version = "1.0.0",
            ServerManagerType = "Trifoia.Module.Story.Manager.StoryManager, Trifoia.Module.Story.Server.Oqtane",
            ReleaseVersions = "1.0.0",
            Dependencies = "Trifoia.Module.Story.Shared.Oqtane",
            PackageName = "Trifoia.Module.Story"
        };
    }
}
