namespace DataGateway.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
           
            CreateTable(
                "dbo.UserCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        CategoryTitle = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId); 
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserCategories", "UserId", "dbo.Users");
            
            DropIndex("dbo.UserCategories", new[] { "UserId" });
            
            DropTable("dbo.OnHands");
            DropTable("dbo.UserCategories");
      
        }
    }
}
