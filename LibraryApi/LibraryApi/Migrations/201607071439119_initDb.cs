namespace LibraryApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "membership.ApiRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "membership.ApiUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                        IdentityUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("membership.ApiRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("membership.ApiUsers", t => t.IdentityUser_Id)
                .Index(t => t.RoleId)
                .Index(t => t.IdentityUser_Id);
            
            CreateTable(
                "membership.ApiUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                        UserId = c.Int(),
                        Name = c.String(),
                        Surname = c.String(),
                        Idnp = c.String(),
                        UserPic = c.Binary(),
                        BirthDate = c.DateTime(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "membership.ApiUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        IdentityUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("membership.ApiUsers", t => t.IdentityUser_Id)
                .Index(t => t.IdentityUser_Id);
            
            CreateTable(
                "membership.ApiUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                        IdentityUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("membership.ApiUsers", t => t.IdentityUser_Id)
                .Index(t => t.IdentityUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("membership.ApiUserRoles", "IdentityUser_Id", "membership.ApiUsers");
            DropForeignKey("membership.ApiUserLogins", "IdentityUser_Id", "membership.ApiUsers");
            DropForeignKey("membership.ApiUserClaims", "IdentityUser_Id", "membership.ApiUsers");
            DropForeignKey("membership.ApiUserRoles", "RoleId", "membership.ApiRoles");
            DropIndex("membership.ApiUserLogins", new[] { "IdentityUser_Id" });
            DropIndex("membership.ApiUserClaims", new[] { "IdentityUser_Id" });
            DropIndex("membership.ApiUsers", "UserNameIndex");
            DropIndex("membership.ApiUserRoles", new[] { "IdentityUser_Id" });
            DropIndex("membership.ApiUserRoles", new[] { "RoleId" });
            DropIndex("membership.ApiRoles", "RoleNameIndex");
            DropTable("membership.ApiUserLogins");
            DropTable("membership.ApiUserClaims");
            DropTable("membership.ApiUsers");
            DropTable("membership.ApiUserRoles");
            DropTable("membership.ApiRoles");
        }
    }
}
