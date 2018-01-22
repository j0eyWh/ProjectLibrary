namespace ProjectLibraryService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class moreUserInfo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Email", c => c.String());
            AddColumn("dbo.Users", "PhoneNumber", c => c.String());
            
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "PhoneNumber");
            DropColumn("dbo.Users", "Email");
        }
    }
}
