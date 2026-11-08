using Azure;
using Azure.Data.Tables;

namespace DotNetDemo.Infrastructure.Azure
{
    public class ArticleComment : ITableEntity
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        public Guid ArticleCommentId { get; set; }

        public Guid ArticleId { get; set; }
        
        public string Subject { get; set; }

        public string Body { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime ModificationDate { get; set; }

        public DateTime? DeletionDate { get; set; }

        public bool IsApproved { get; set; }

        public bool IsDeclined { get; set; }
    }
}