using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;
using Oqtane.Databases.Interfaces;
using Oqtane.Migrations;
using Oqtane.Migrations.EntityBuilders;

namespace Trifoia.Module.Story.Migrations.EntityBuilders
{
    public class StoryEntityBuilder : AuditableBaseEntityBuilder<StoryEntityBuilder>
    {
        private const string _entityTableName = "TrifoiaStory";
        private readonly PrimaryKey<StoryEntityBuilder> _primaryKey = new("PK_TrifoiaStory", x => x.StoryId);
        private readonly ForeignKey<StoryEntityBuilder> _moduleForeignKey = new("FK_TrifoiaStory_Module", x => x.ModuleId, "Module", "ModuleId", ReferentialAction.Cascade);

        public StoryEntityBuilder(MigrationBuilder migrationBuilder, IDatabase database) : base(migrationBuilder, database)
        {
            EntityTableName = _entityTableName;
            PrimaryKey = _primaryKey;
            ForeignKeys.Add(_moduleForeignKey);
        }

        protected override StoryEntityBuilder BuildTable(ColumnsBuilder table)
        {
            StoryId = AddAutoIncrementColumn(table,"StoryId");
            ModuleId = AddIntegerColumn(table,"ModuleId");
            Name = AddMaxStringColumn(table,"Name");
            AddAuditableColumns(table);
            return this;
        }

        public OperationBuilder<AddColumnOperation> StoryId { get; set; }
        public OperationBuilder<AddColumnOperation> ModuleId { get; set; }
        public OperationBuilder<AddColumnOperation> Name { get; set; }
    }
}
