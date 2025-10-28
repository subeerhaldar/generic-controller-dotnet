using GenericController.Models;
using Microsoft.EntityFrameworkCore;

namespace GenericController.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<MstMarketingNote> MstMarketingNotes { get; set; }
        public DbSet<MstLegalChecklist> MstLegalChecklists { get; set; }
        public DbSet<MstCommercialChecklist> MstCommercialChecklists { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure MstMarketingNote
            modelBuilder.Entity<MstMarketingNote>()
                .HasKey(m => m.NoteCode);

            modelBuilder.Entity<MstMarketingNote>()
                .Property(m => m.IsDeleted)
                .HasDefaultValue("N");

            // Configure MstLegalChecklist
            modelBuilder.Entity<MstLegalChecklist>()
                .HasKey(l => l.LegalChkId);

            modelBuilder.Entity<MstLegalChecklist>()
                .Property(l => l.IsDeleted)
                .HasDefaultValue("N");

            modelBuilder.Entity<MstLegalChecklist>()
                .Property(l => l.IsMandetory)
                .HasDefaultValue(false);

            // Configure MstCommercialChecklist
            modelBuilder.Entity<MstCommercialChecklist>()
                .HasKey(c => c.ChkId);

            modelBuilder.Entity<MstCommercialChecklist>()
                .Property(c => c.IsDeleted)
                .HasDefaultValue("N");

            modelBuilder.Entity<MstCommercialChecklist>()
                .Property(c => c.IsFileUpload)
                .HasDefaultValue(false);
        }
    }
}