using DotNetDemo.Infrastructure.Contracts;

namespace DotNetDemo.Infrastructure
{
    public class ArticleCommentRepository : IArticleCommentRepository
    {
        private const string TableName = "ArticleComment";
        private const string PartitionKey = "ArticleComment";

        private readonly IAzureTableStorage _azureTableStorage;

        public ArticleCommentRepository(IAzureTableStorage azureTableStorage) {
            _azureTableStorage = azureTableStorage;
        }

        public async Task CreateArticleComment(Azure.ArticleComment comment)
        {
            var articleCommentId = Guid.NewGuid();

            comment.ArticleCommentId = articleCommentId;
            comment.PartitionKey = PartitionKey;
            comment.RowKey = articleCommentId.ToString();

            await _azureTableStorage.AddAsync(TableName, comment);
        }

        public async Task<Azure.ArticleComment> Get(Guid id)
        {
            return await _azureTableStorage.GetAsync<Azure.ArticleComment>(TableName, PartitionKey, id.ToString());
        }

        public async Task<IEnumerable<Azure.ArticleComment>> GetPending()
        {
            return await _azureTableStorage.QueryAsync<Azure.ArticleComment>(TableName, (_) => true);
        }

        public async Task Approve(Guid id)
        {
            var entity = await _azureTableStorage.GetAsync<Azure.ArticleComment>(TableName, PartitionKey, id.ToString());

            entity.IsApproved = true;

            await _azureTableStorage.UpdateAsync(TableName, entity);
        }

        public async Task Decline(Guid id)
        {
            var entity = await _azureTableStorage.GetAsync<Azure.ArticleComment>(TableName, PartitionKey, id.ToString());

            entity.IsDeclined = true;

            await _azureTableStorage.UpdateAsync(TableName, entity);
        }

        public async Task Delete(Guid id)
        {
            var entity = await _azureTableStorage.GetAsync<Azure.ArticleComment>(TableName, PartitionKey, id.ToString());

            entity.DeletionDate = DateTime.UtcNow;

            await _azureTableStorage.UpdateAsync(TableName, entity);
        }

        public async Task Purge(Guid id)
        {
            var entity = await _azureTableStorage.GetAsync<Azure.ArticleComment>(TableName, PartitionKey, id.ToString());

            await _azureTableStorage.DeleteAsync(TableName, entity);
        }
    }
}