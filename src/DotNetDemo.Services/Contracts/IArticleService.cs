using DotNetDemo.Models;

namespace DotNetDemo.Services.Contracts
{
    public interface IArticleService
    {
        Task<IEnumerable<Article>> SearchArticles();

        Task<Article> GetArticle(Guid id);

        Task AddArticle(Article article);

        Task UpdateArticle(Article article);

        Task DeleteArticle(Guid id);

        Task CommentArticle(ArticleComment comment);
    }
}