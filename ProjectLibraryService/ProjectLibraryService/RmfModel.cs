namespace ProjectLibraryService
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class RmfModel : DbContext
    {
        public RmfModel()
            : base("name=RmfModel")
        {
        }

        public virtual DbSet<Rmf> Rmf { get; set; }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<BookCode> BookCodes { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<OnHand> OnHandsBooks { get; set; }
        public virtual DbSet<BookCodeOwned> BookCodesOwned { get; set; }
        public virtual DbSet<ReservedBook> ReservedBooks { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
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
        }
    }
}
