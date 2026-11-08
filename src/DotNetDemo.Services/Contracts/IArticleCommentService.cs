using DotNetDemo.Models;

namespace DotNetDemo.Services.Contracts
{
    public interface IArticleCommentService
    {

        Task CommentArticle(ArticleComment comment);

        Task<IEnumerable<ArticleComment>> GetPendingArticleComments();

        Task<ArticleComment> GetArticleComment(Guid id);

        Task ApproveArticleComment(Guid id);

        Task DeclineArticleComment(Guid id);

        Task ProcessArticleComment(Guid id);

        Task PurgeArticleComment(Guid id);
    }
}