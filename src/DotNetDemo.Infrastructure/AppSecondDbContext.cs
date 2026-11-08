using DotNetDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace DotNetDemo.Infrastructure
{
    public class AppSecondDbContext : DbContext
    {
        public AppSecondDbContext(DbContextOptions<AppSecondDbContext> options) : base(options)
        {

        }

        public DbSet<Article> Articles { get; set; }

        public DbSet<ArticleComment> ArticleComments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder is null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Entity<Article>()
                .HasKey(p => p.ArticleId);

            modelBuilder
                .Entity<ArticleComment>()
                .HasKey(p => p.ArticleCommentId);

            modelBuilder
                .Entity<Article>()
                .HasMany(e => e.ArticleComments)
                .WithOne(e => e.Article)
                .HasForeignKey(e => e.ArticleId);
        }
    }
}