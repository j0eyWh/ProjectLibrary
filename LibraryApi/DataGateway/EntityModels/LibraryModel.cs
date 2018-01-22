namespace DataGateway.EntityModels
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class LibraryModel : DbContext
    {
        public LibraryModel()
            : base("name=LibraryModel")
        {
            this.Configuration.ProxyCreationEnabled = false;
            this.Configuration.LazyLoadingEnabled = false;
        }

        public virtual DbSet<BookCode> BookCodes { get; set; }
        public virtual DbSet<BookCodesOwned> BookCodesOwneds { get; set; }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<OnHand> OnHands { get; set; }
        public virtual DbSet<ReservedBook> ReservedBooks { get; set; }
        public virtual DbSet<Rmf> Rmfs { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserCategory> UserCategories  { get; set; }
        public virtual DbSet<ReadingPosition> ReadoPositions { get; set; }
        public virtual DbSet<BookTag> BookTags  { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .Property(e => e.CollectionId)
                .IsUnicode(false);

            modelBuilder.Entity<Rmf>()
                .Property(e => e.IdRmf)
                .IsUnicode(false);

            modelBuilder.Entity<Rmf>()
                .Property(e => e.DocId)
                .IsUnicode(false);

            modelBuilder.Entity<Rmf>()
                .Property(e => e.ContentCat)
                .IsUnicode(false);

            modelBuilder.Entity<Rmf>()
                .Property(e => e.OutCause)
                .IsUnicode(false);

            modelBuilder.Entity<Rmf>()
                .HasMany(e => e.Books)
                .WithOptional(e => e.Rmf)
                .HasForeignKey(e => e.CollectionId);
            
        }
    }
}
