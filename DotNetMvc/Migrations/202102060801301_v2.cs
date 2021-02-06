namespace DotNetMvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Directors", "BirthDay");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Directors", "BirthDay", c => c.DateTime(nullable: false));
        }
    }
}
