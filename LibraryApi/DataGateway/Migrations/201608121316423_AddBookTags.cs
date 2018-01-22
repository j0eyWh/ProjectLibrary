namespace DataGateway.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBookTags : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BookTags",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BookId = c.Int(nullable: false),
                        Tag = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Books", t => t.BookId, cascadeDelete: true)
                .Index(t => t.BookId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BookTags", "BookId", "dbo.Books");
            DropIndex("dbo.BookTags", new[] { "BookId" });
            DropTable("dbo.BookTags");
        }
    }
}
