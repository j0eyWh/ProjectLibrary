namespace DataGateway.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddReadingPositionsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ReadingPositions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        BookCodeId = c.Int(nullable: false),
                        Longitude = c.Double(nullable: false),
                        Latitude = c.Double(nullable: false),
                        SharingTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BookCodes", t => t.BookCodeId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.BookCodeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ReadingPositions", "UserId", "dbo.Users");
            DropForeignKey("dbo.ReadingPositions", "BookCodeId", "dbo.BookCodes");
            DropIndex("dbo.ReadingPositions", new[] { "BookCodeId" });
            DropIndex("dbo.ReadingPositions", new[] { "UserId" });
            DropTable("dbo.ReadingPositions");
        }
    }
}
