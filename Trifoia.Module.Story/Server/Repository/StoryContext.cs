using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Oqtane.Modules;
using Oqtane.Repository;
using Oqtane.Infrastructure;
using Oqtane.Repository.Databases.Interfaces;

namespace Trifoia.Module.Story.Repository
{
    public class StoryContext : DBContextBase, ITransientService, IMultiDatabase
    {
        public virtual DbSet<Models.Story> Story { get; set; }

        public StoryContext(IDBContextDependencies DBContextDependencies) : base(DBContextDependencies)
        {
            // ContextBase handles multi-tenant database connections
        }
    }
}
