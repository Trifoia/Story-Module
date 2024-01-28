using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Oqtane.Shared;
using Oqtane.Enums;
using Oqtane.Infrastructure;
using Trifoia.Module.Story.Repository;
using Oqtane.Controllers;
using System.Net;

namespace Trifoia.Module.Story.Controllers
{
    [Route(ControllerRoutes.ApiRoute)]
    public class StoryController : ModuleControllerBase
    {
        private readonly IStoryRepository _StoryRepository;

        public StoryController(IStoryRepository StoryRepository, ILogManager logger, IHttpContextAccessor accessor) : base(logger, accessor)
        {
            _StoryRepository = StoryRepository;
        }

        // GET: api/<controller>?moduleid=x
        [HttpGet]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public IEnumerable<Models.Story> Get(string moduleid)
        {
            int ModuleId;
            if (int.TryParse(moduleid, out ModuleId) && IsAuthorizedEntityId(EntityNames.Module, ModuleId))
            {
                return _StoryRepository.GetStorys(ModuleId);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Story Get Attempt {ModuleId}", moduleid);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return null;
            }
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public Models.Story Get(int id)
        {
            Models.Story Story = _StoryRepository.GetStory(id);
            if (Story != null && IsAuthorizedEntityId(EntityNames.Module, Story.ModuleId))
            {
                return Story;
            }
            else
            { 
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Story Get Attempt {StoryId}", id);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return null;
            }
        }

        // POST api/<controller>
        [HttpPost]
        [Authorize(Policy = PolicyNames.EditModule)]
        public Models.Story Post([FromBody] Models.Story Story)
        {
            if (ModelState.IsValid && IsAuthorizedEntityId(EntityNames.Module, Story.ModuleId))
            {
                Story = _StoryRepository.AddStory(Story);
                _logger.Log(LogLevel.Information, this, LogFunction.Create, "Story Added {Story}", Story);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Story Post Attempt {Story}", Story);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                Story = null;
            }
            return Story;
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public Models.Story Put(int id, [FromBody] Models.Story Story)
        {
            if (ModelState.IsValid && Story.StoryId == id && IsAuthorizedEntityId(EntityNames.Module, Story.ModuleId) && _StoryRepository.GetStory(Story.StoryId, false) != null)
            {
                Story = _StoryRepository.UpdateStory(Story);
                _logger.Log(LogLevel.Information, this, LogFunction.Update, "Story Updated {Story}", Story);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Story Put Attempt {Story}", Story);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                Story = null;
            }
            return Story;
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public void Delete(int id)
        {
            Models.Story Story = _StoryRepository.GetStory(id);
            if (Story != null && IsAuthorizedEntityId(EntityNames.Module, Story.ModuleId))
            {
                _StoryRepository.DeleteStory(id);
                _logger.Log(LogLevel.Information, this, LogFunction.Delete, "Story Deleted {StoryId}", id);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Story Delete Attempt {StoryId}", id);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            }
        }
    }
}
