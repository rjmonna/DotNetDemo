using System.Linq.Expressions;
using Azure.Data.Tables;
using DotNetDemo.Infrastructure.Contracts;

namespace DotNetDemo.Infrastructure
{
    public class AzureTableStorage : IAzureTableStorage
    {
        private TableServiceClient _tableServiceClient;

        private Dictionary<string, TableClient> clients = new Dictionary<string, TableClient>();

        public AzureTableStorage(TableServiceClient tableServiceClient)
        {
            _tableServiceClient = tableServiceClient;
        }

        public async Task<T> GetAsync<T>(string tableName, string partitionKey, string rowKey, CancellationToken cancellationToken = default) where T : class, ITableEntity {
            var client = await EnsureTable(tableName);

            var result = await client.GetEntityAsync<T>(partitionKey, rowKey, null, cancellationToken);

            return result.Value;
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>(string tableName, CancellationToken cancellationToken = default) where T : class, ITableEntity {
            var client = await EnsureTable(tableName);

            var result = new List<T>();

            await foreach(var page in client.QueryAsync<T>(f => true).AsPages())
            {
                if (cancellationToken.IsCancellationRequested) break;

                result.AddRange(page.Values);
            }

            return result;
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string tableName, Expression<Func<T, bool>> query, CancellationToken cancellationToken = default) where T : class, ITableEntity {
            var client = await EnsureTable(tableName);

            var result = new List<T>();

            await foreach(var page in client.QueryAsync<T>(query).AsPages())
            {
                if (cancellationToken.IsCancellationRequested) break;

                result.AddRange(page.Values);
            }

            return result;
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string tableName, string? query = null, CancellationToken cancellationToken = default) where T : class, ITableEntity {
            var client = await EnsureTable(tableName);

            var result = new List<T>();

            await foreach(var page in client.QueryAsync<T>(query).AsPages())
            {
                if (cancellationToken.IsCancellationRequested) break;

                result.AddRange(page.Values);
            }

            return result;
        }

        public async Task<object> UpsertAsync<T>(string tableName, T entity, CancellationToken cancellationToken = default) where T : class, ITableEntity
        {
            var client = await EnsureTable(tableName);
            
            var response = await client.UpsertEntityAsync(entity, cancellationToken: cancellationToken);

            return response;
        }

        public async Task<object> DeleteAsync(string tableName, ITableEntity entity, CancellationToken cancellationToken = default)
        {
            var client = await EnsureTable(tableName);

            var response = await client.DeleteEntityAsync(entity.PartitionKey, entity.RowKey, default, cancellationToken);

            return response;
        }

        public async Task<object> AddAsync<T>(string tableName, T entity) where T : class, ITableEntity
        {
            var client = await EnsureTable(tableName);

            var response = await client.AddEntityAsync(entity);

            return response;
        }

        public async Task<IEnumerable<T>> AddBatchAsync<T>(string tableName) where T : class, ITableEntity
        {
            var client = await EnsureTable(tableName);

            throw new NotImplementedException();
        }

        public async Task<object> UpdateAsync<T>(string tableName, T entity, CancellationToken cancellationToken = default) where T : class, ITableEntity
        {
            var client = await EnsureTable(tableName);
            
            var response = await client.UpdateEntityAsync(entity, entity.ETag, cancellationToken: cancellationToken);

            return response;
        }

        private async Task<TableClient> EnsureTable(string tableName)
        {
            if (!clients.ContainsKey(tableName))
            {
                await _tableServiceClient.CreateTableIfNotExistsAsync(tableName);

                clients.Add(tableName, _tableServiceClient.GetTableClient(tableName));
            }

            return clients[tableName];
        }
    }
}