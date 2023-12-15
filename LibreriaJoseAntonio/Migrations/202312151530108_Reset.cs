namespace LibreriaJoseAntonio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Reset : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Autors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Editorials",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Estadoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Formatoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ItemCarritoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Id_libro = c.Int(nullable: false),
                        Cantidad = c.Int(nullable: false),
                        IdUser = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Libroes", t => t.Id_libro, cascadeDelete: true)
                .Index(t => t.Id_libro);
            
            CreateTable(
                "dbo.Libroes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ISBN = c.String(nullable: false, maxLength: 13),
                        AutorId = c.Int(nullable: false),
                        EditorialId = c.Int(nullable: false),
                        FormatoId = c.Int(nullable: false),
                        EstadoId = c.Int(nullable: false),
                        Titulo = c.String(nullable: false),
                        Precio = c.Single(nullable: false),
                        Cantidad = c.Int(nullable: false),
                        Imagen = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Autors", t => t.AutorId, cascadeDelete: true)
                .ForeignKey("dbo.Editorials", t => t.EditorialId, cascadeDelete: true)
                .ForeignKey("dbo.Estadoes", t => t.EstadoId, cascadeDelete: true)
                .ForeignKey("dbo.Formatoes", t => t.FormatoId, cascadeDelete: true)
                .Index(t => t.ISBN, unique: true)
                .Index(t => t.AutorId)
                .Index(t => t.EditorialId)
                .Index(t => t.FormatoId)
                .Index(t => t.EstadoId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
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
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.ItemCarritoes", "Id_libro", "dbo.Libroes");
            DropForeignKey("dbo.Libroes", "FormatoId", "dbo.Formatoes");
            DropForeignKey("dbo.Libroes", "EstadoId", "dbo.Estadoes");
            DropForeignKey("dbo.Libroes", "EditorialId", "dbo.Editorials");
            DropForeignKey("dbo.Libroes", "AutorId", "dbo.Autors");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Libroes", new[] { "EstadoId" });
            DropIndex("dbo.Libroes", new[] { "FormatoId" });
            DropIndex("dbo.Libroes", new[] { "EditorialId" });
            DropIndex("dbo.Libroes", new[] { "AutorId" });
            DropIndex("dbo.Libroes", new[] { "ISBN" });
            DropIndex("dbo.ItemCarritoes", new[] { "Id_libro" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Libroes");
            DropTable("dbo.ItemCarritoes");
            DropTable("dbo.Formatoes");
            DropTable("dbo.Estadoes");
            DropTable("dbo.Editorials");
            DropTable("dbo.Autors");
        }
    }
}
