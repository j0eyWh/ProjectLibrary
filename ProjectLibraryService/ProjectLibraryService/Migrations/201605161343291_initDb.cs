namespace ProjectLibraryService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BookCodes",
                c => new
                    {
                        Code = c.Int(nullable: false, identity: true),
                        BookId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Code)
                .ForeignKey("dbo.Books", t => t.BookId, cascadeDelete: true)
                .Index(t => t.BookId);
            
            CreateTable(
                "dbo.Books",
                c => new
                    {
                        BookId = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Author = c.String(),
                        RegistrationDate = c.DateTime(nullable: false),
                        Amount = c.Int(nullable: false),
                        Publisher = c.String(),
                        Year = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CanLend = c.Boolean(nullable: false),
                        Description = c.String(),
                        Image = c.Binary(),
                        CollectionId = c.String(maxLength: 50, unicode: false),
                        Category = c.String(),
                    })
                .PrimaryKey(t => t.BookId)
                .ForeignKey("dbo.Rmf", t => t.CollectionId)
                .Index(t => t.CollectionId);
            
            CreateTable(
                "dbo.Rmf",
                c => new
                    {
                        IdRmf = c.String(nullable: false, maxLength: 50, unicode: false),
                        DateIn = c.DateTime(nullable: false, storeType: "date"),
                        DateOut = c.DateTime(storeType: "date"),
                        DocId = c.String(nullable: false, maxLength: 50, unicode: false),
                        Quantity = c.Int(nullable: false),
                        TotalValue = c.Decimal(precision: 18, scale: 2),
                        FirstInvNr = c.Int(nullable: false),
                        LastInvNr = c.Int(nullable: false),
                        ContentCat = c.String(maxLength: 50, unicode: false),
                        OutCause = c.String(maxLength: 50, unicode: false),
                        IsOut = c.Boolean(),
                        DocImg = c.Binary(),
                        Origin = c.String(),
                    })
                .PrimaryKey(t => t.IdRmf)
                .Index(t => t.DocId, unique: true);
            
            CreateTable(
                "dbo.BookCodesOwned",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        LendDate = c.DateTime(nullable: false),
                        ReturnDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BookCodes", t => t.Code, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.Code)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Surname = c.String(),
                        Idnp = c.String(),
                        UserPic = c.Binary(),
                        BirthDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.OnHands",
                c => new
                    {
                        OnHandId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        Code = c.Int(nullable: false),
                        LendDate = c.DateTime(nullable: false),
                        ReturnDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.OnHandId)
                .ForeignKey("dbo.BookCodes", t => t.Code, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.Code);
            
            CreateTable(
                "dbo.ReservedBooks",
                c => new
                    {
                        ReservationId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        Code = c.Int(nullable: false),
                        TimeOut = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ReservationId)
                .ForeignKey("dbo.BookCodes", t => t.Code, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.Code);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ReservedBooks", "UserId", "dbo.Users");
            DropForeignKey("dbo.ReservedBooks", "Code", "dbo.BookCodes");
            DropForeignKey("dbo.OnHands", "UserId", "dbo.Users");
            DropForeignKey("dbo.OnHands", "Code", "dbo.BookCodes");
            DropForeignKey("dbo.BookCodesOwned", "UserId", "dbo.Users");
            DropForeignKey("dbo.BookCodesOwned", "Code", "dbo.BookCodes");
            DropForeignKey("dbo.BookCodes", "BookId", "dbo.Books");
            DropForeignKey("dbo.Books", "CollectionId", "dbo.Rmf");
            DropIndex("dbo.ReservedBooks", new[] { "Code" });
            DropIndex("dbo.ReservedBooks", new[] { "UserId" });
            DropIndex("dbo.OnHands", new[] { "Code" });
            DropIndex("dbo.OnHands", new[] { "UserId" });
            DropIndex("dbo.BookCodesOwned", new[] { "UserId" });
            DropIndex("dbo.BookCodesOwned", new[] { "Code" });
            DropIndex("dbo.Rmf", new[] { "DocId" });
            DropIndex("dbo.Books", new[] { "CollectionId" });
            DropIndex("dbo.BookCodes", new[] { "BookId" });
            DropTable("dbo.ReservedBooks");
            DropTable("dbo.OnHands");
            DropTable("dbo.Users");
            DropTable("dbo.BookCodesOwned");
            DropTable("dbo.Rmf");
            DropTable("dbo.Books");
            DropTable("dbo.BookCodes");
        }
    }
}
