using DotNetDemo.Infrastructure;

namespace DotNetDemo.Infrastructure.Contracts
{
    public interface IArticleCommentRepository
    {

        Task CreateArticleComment(Azure.ArticleComment articleComment);

        Task<Azure.ArticleComment> Get(Guid id);

        Task<IEnumerable<Azure.ArticleComment>> GetPending();

        Task Approve(Guid id);

        Task Decline(Guid id);

        Task Delete(Guid id);

        Task Purge(Guid id);
    }
}