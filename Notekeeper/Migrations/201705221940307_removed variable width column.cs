namespace Notekeeper.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removedvariablewidthcolumn : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Notes", "WidthClass");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Notes", "WidthClass", c => c.String());
        }
    }
}
