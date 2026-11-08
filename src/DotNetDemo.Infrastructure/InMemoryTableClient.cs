using System.Linq.Expressions;
using System.Reflection;
using Azure;
using Azure.Data.Tables;
using Moq;
using StringToExpression.LanguageDefinitions;

namespace DotNetDemo.Infrastructure
{
    public class InMemoryTableClient : TableClient
    {
        private Dictionary<string, Dictionary<string, DynamicTableEntity>> _tableStore = new Dictionary<string, Dictionary<string, DynamicTableEntity>>();

        public override async Task<Response<T>> GetEntityAsync<T>(string partitionKey, string rowKey, IEnumerable<string> select = null, CancellationToken cancellationToken = default)
        {
            if (String.IsNullOrWhiteSpace(partitionKey)) throw new ArgumentOutOfRangeException(nameof(partitionKey));
            if (String.IsNullOrWhiteSpace(rowKey)) throw new ArgumentOutOfRangeException(nameof(rowKey));

            var record = _tableStore.ContainsKey(partitionKey) && _tableStore[partitionKey].ContainsKey(rowKey) ? _tableStore[partitionKey][rowKey] : null;

            Mock<Response> responseMock = new();

            if (record == null)
            {
                responseMock.SetupGet(r => r.Status).Returns(404);
            }
            else
            {
                responseMock.SetupGet(r => r.Status).Returns(200);
            }

            Response response = responseMock.Object;
            return Response.FromValue<T>(MapDynamic<T>(record, select), response);
        }

        public override async Task<Response> AddEntityAsync<T>(T entity, CancellationToken cancellationToken = default)
        {
            var typedEntity = (ITableEntity)entity;

            if (String.IsNullOrWhiteSpace(entity.PartitionKey) || String.IsNullOrWhiteSpace(entity.RowKey)) throw new InvalidOperationException("ITableEntity must hava a PartitionKey and an RowKey.");

            var record = _tableStore.ContainsKey(typedEntity.PartitionKey) && _tableStore[typedEntity.PartitionKey].ContainsKey(typedEntity.RowKey) ? _tableStore[typedEntity.PartitionKey][typedEntity.RowKey] : null;

            Mock<Response> responseMock = new();

            if (record == null)
            {
                if (!_tableStore.ContainsKey(typedEntity.PartitionKey))
                {
                    _tableStore.Add(typedEntity.PartitionKey, new Dictionary<string, DynamicTableEntity>());
                }

                _tableStore[typedEntity.PartitionKey][typedEntity.RowKey] = MapDynamic(entity);

                responseMock.SetupGet(r => r.Status).Returns(200);
            }
            else
            {
                responseMock.SetupGet(r => r.Status).Returns(409);
            }

            Response response = responseMock.Object;
            return response;
        }

        public override AsyncPageable<T> QueryAsync<T>(Expression<Func<T, bool>> filter, int? maxPerPage = null, IEnumerable<string> select = null, CancellationToken cancellationToken = default)
        {
            var result = _tableStore
                .Values
                .SelectMany(d => MapDynamicList<T>(d.Values, select));

            Mock<Response> responseMock = new();

            responseMock.SetupGet(r => r.Status).Returns(200);

            Response response = responseMock.Object;

            List<Page<T>> pages = new List<Page<T>>();

            int maxPageSize = maxPerPage ?? int.MaxValue;

            for(int i = 0; i < result.Count(); i += maxPageSize)
            {
                pages
                    .Add(Page<T>.FromValues(result.Skip(i).Take(maxPageSize).ToList().AsReadOnly(), null, response));
            }

            return AsyncPageable<T>.FromPages(pages);
        }

        public override AsyncPageable<T> QueryAsync<T>(string filter = null, int? maxPerPage = null, IEnumerable<string> select = null, CancellationToken cancellationToken = default)
        {
            var language = new ODataFilterLanguage();

            Expression<Func<T, bool>> predicate = filter == null ? ((T) => true) : language.Parse<T>(filter);

            return QueryAsync<T>(predicate, maxPerPage, select, cancellationToken);
        }

        public override async Task<Response> UpdateEntityAsync<T>(T entity, ETag ifMatch, TableUpdateMode mode = TableUpdateMode.Merge, CancellationToken cancellationToken = default)
        {
            var typedEntity = (ITableEntity)entity;

            if (String.IsNullOrWhiteSpace(entity.PartitionKey) || String.IsNullOrWhiteSpace(entity.RowKey)) throw new InvalidOperationException("ITableEntity must hava a PartitionKey and an RowKey.");

            var record = _tableStore.ContainsKey(typedEntity.PartitionKey) && _tableStore[typedEntity.PartitionKey].ContainsKey(typedEntity.RowKey) ? _tableStore[typedEntity.PartitionKey][typedEntity.RowKey] : null;

            Mock<Response> responseMock = new();

            if (record == null)
            {
                responseMock.SetupGet(r => r.Status).Returns(404);
            }
            else
            {
                _tableStore[typedEntity.PartitionKey][typedEntity.RowKey] = MapDynamic(entity);

                responseMock.SetupGet(r => r.Status).Returns(200);
            }

            Response response = responseMock.Object;
            return response;
        }

        public override async Task<Response> UpsertEntityAsync<T>(T entity, TableUpdateMode mode = TableUpdateMode.Merge, CancellationToken cancellationToken = default)
        {
            var typedEntity = (ITableEntity)entity;

            if (String.IsNullOrWhiteSpace(entity.PartitionKey) || String.IsNullOrWhiteSpace(entity.RowKey)) throw new InvalidOperationException("ITableEntity must hava a PartitionKey and an RowKey.");

            Mock<Response> responseMock = new();

            _tableStore[typedEntity.PartitionKey][typedEntity.RowKey] = MapDynamic(entity);

            responseMock.SetupGet(r => r.Status).Returns(200);

            Response response = responseMock.Object;
            return response;
        }

        private static List<T> MapDynamicList<T>(IEnumerable<DynamicTableEntity> obj, IEnumerable<string>? select = null)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            return obj
                .Select(o => MapDynamic<T>(o, select))
                .ToList();
        }

        private static T MapDynamic<T>(DynamicTableEntity obj, IEnumerable<string>? select = null)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            T result = Activator.CreateInstance<T>();

            obj
                .Where(kvp => (select == null || select.Contains(kvp.Key))
                    && (typeof(T).GetProperty(kvp.Key) != null))
                .ToList()
                .ForEach(kvp => typeof(T).GetProperty(kvp.Key)!.SetValue(result, kvp.Value));

            return result;
        }

        private static List<DynamicTableEntity> MapDynamicList<T>(IEnumerable<T> obj, IEnumerable<string>? select = null)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            return obj
                .Select(o => MapDynamic<T>(o, select))
                .ToList();
        }

        private static DynamicTableEntity MapDynamic<T>(T obj, IEnumerable<string>? select = null)
        {
            var result = new DynamicTableEntity();

            var sourceType = typeof(T);

            sourceType
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => select == null || select.Contains(p.Name))
                .ToList()
                .ForEach(p => result[p.Name] = p.GetValue(obj));

            return result;
        }
    }
}