using DotNetDemo.Infrastructure.Contracts;
using DotNetDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace DotNetDemo.Infrastructure
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly AppSecondDbContext _appDbContext;
        public ArticleRepository(AppSecondDbContext appDbContext) {
            _appDbContext = appDbContext;
        }

        public async Task<Article?> CreateArticle(Article article)
        {
            await _appDbContext
                .Articles
                .AddAsync(article);

            return article;
        }

        public async Task<Article?> DeleteArticle(Guid articleId)
        {
            var result = await _appDbContext.Articles.FirstOrDefaultAsync(a => a.ArticleId == articleId);

            if (result != null)
            {
                result.DeletionDate = DateTime.UtcNow;

                await _appDbContext.SaveChangesAsync();
            }

            return result;
        }

        public async Task<Article?> GetArticle(Guid articleId)
        {
            var result = await _appDbContext
                .Articles
                .FirstOrDefaultAsync(e => e.ArticleId == articleId);

            return result;
        }

        public async Task<IEnumerable<Article>> SearchArticles()
        {
            return await _appDbContext
                .Articles
                .ToListAsync();
        }

        public async Task<Article?> UpdateArticle(Article article)
        {
            var result = await _appDbContext.Articles.FirstOrDefaultAsync(a => a.ArticleId == article.ArticleId);

            if (result != null)
            {
                result.Subject = article.Subject;
                result.Body = article.Body;
                result.ModificationDate = DateTime.UtcNow;

                await _appDbContext.SaveChangesAsync();
            }

            return result;
        }
    }
}