using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Reflection.Emit;

namespace StomatoloskaOrdinacija.Models
{
    public class zubarContext : DbContext
    {
        public DbSet<User> users { get; set; }
        public DbSet<Rating> ratings { get; set; }
        public DbSet<Comment> comments { get; set; }
        public DbSet<Blog> blogs { get; set; }
        public DbSet<US> uss { get; set; }
        public DbSet<SpecInt> SpecInt { get; set; }
        public DbSet<Term> Term { get; set; }
        public DbSet<Intervention> Intervention { get; set; }
        public DbSet<WorkingDay> WorkingDay { get; set; }
        public DbSet<CV> CV { get; set; }

        public DbSet<SI> SI { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Rating>().HasOne(u=>u.FromWho).WithMany(r=>r.RatingsFrom).HasForeignKey(r => r.FromWhoID).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Rating>().HasOne(u => u.ToWho).WithMany(r => r.RatingsTo).HasForeignKey(r => r.ToWhoID).OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Comment>().HasOne(u => u.CommentFrom).WithMany(r => r.CommentsFrom).HasForeignKey(r => r.CommentFromID).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Comment>().HasOne(u => u.CommentTo).WithMany(r => r.CommentsTo).HasForeignKey(r => r.CommentToID).OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Term>()
              .HasOne(t => t.WorkingDay)
              .WithMany(w => w.Terms)
              .HasForeignKey(t => t.WorkingDayID)
              .OnDelete(DeleteBehavior.NoAction);


        }
        public zubarContext(DbContextOptions options):base(options)
        {
            
        }
    
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Server=(localdb)\\Ordinacija;Database=OrdinacijaDB;Trusted_Connection=true;TrustServerCertificate=true;");
        }
        
    }

}
