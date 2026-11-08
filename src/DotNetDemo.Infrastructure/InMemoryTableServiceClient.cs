using System.Linq.Expressions;
using Azure;
using Azure.Data.Tables;
using Azure.Data.Tables.Models;
using Moq;
using StringToExpression.LanguageDefinitions;

namespace DotNetDemo.Infrastructure
{
    public class InMemoryTableServiceClient : TableServiceClient
    {
        private static Dictionary<string, InMemoryTableClient> s_tableClients = new Dictionary<string, InMemoryTableClient>();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static InMemoryTableServiceClient()
        {

        }

        public override TableClient GetTableClient(string tableName)
        {
            CreateTableIfNotExists(tableName);

            return s_tableClients[tableName];
        }

        public override async Task<Response<TableItem>> CreateTableIfNotExistsAsync(string tableName, CancellationToken cancellationToken = default)
        {
            Mock<Response> responseMock = new();

            if (!s_tableClients.ContainsKey(tableName))
            {
                s_tableClients[tableName] = new InMemoryTableClient();

                responseMock.SetupGet(r => r.Status).Returns(200);
            }
            else
            {
                responseMock.SetupGet(r => r.Status).Returns(409);
            }


            Response response = responseMock.Object;
            return Response.FromValue(new TableItem(tableName), response);
        }

        public override Response<TableItem> CreateTableIfNotExists(string tableName, CancellationToken cancellationToken = default)
        {
            return CreateTableIfNotExistsAsync(tableName, cancellationToken).Result;
        }

        public override async Task<Response<TableItem>> CreateTableAsync(string tableName, CancellationToken cancellationToken = default)
        {
            Mock<Response> responseMock = new();

            if (!s_tableClients.ContainsKey(tableName))
            {
                s_tableClients[tableName] = new InMemoryTableClient();

                responseMock.SetupGet(r => r.Status).Returns(200);
            }
            else
            {
                throw new RequestFailedException($"Table name '{tableName}' already exists and could not be created.");
            }

            Response response = responseMock.Object;
            return Response.FromValue(new TableItem(tableName), response);
        }

        public override Response<TableItem> CreateTable(string tableName, CancellationToken cancellationToken = default)
        {
            return CreateTableAsync(tableName, cancellationToken).Result;
        }

        public override async Task<Response> DeleteTableAsync(string tableName, CancellationToken cancellationToken = default)
        {
            if (s_tableClients.ContainsKey(tableName))
            {
                s_tableClients.Remove(tableName);
            }

            Mock<Response> responseMock = new();

            Response response = responseMock.Object;
            return response;
        }

        public override Response DeleteTable(string tableName, CancellationToken cancellationToken = default)
        {
            return DeleteTableAsync(tableName, cancellationToken).Result;
        }

        public override AsyncPageable<TableItem> QueryAsync(Expression<Func<TableItem, bool>> filter, int? maxPerPage = null, CancellationToken cancellationToken = default)
        {
            var result = s_tableClients.Select(c => new TableItem(c.Key));

            Mock<Response> responseMock = new();

            responseMock.SetupGet(r => r.Status).Returns(200);

            Response response = responseMock.Object;

            List<Page<TableItem>> pages = new List<Page<TableItem>>();

            int maxPageSize = maxPerPage ?? int.MaxValue;

            for(int i = 0; i < result.Count(); i += maxPageSize)
            {
                pages
                    .Add(Page<TableItem>.FromValues(result.Skip(i).Take(maxPageSize).ToList().AsReadOnly(), null, response));
            }

            return AsyncPageable<TableItem>.FromPages(pages);
        }

        public override AsyncPageable<TableItem> QueryAsync(FormattableString filter, int? maxPerPage = null, CancellationToken cancellationToken = default)
        {
            return QueryAsync(filter.ToString(), maxPerPage, cancellationToken);
        }

        public override AsyncPageable<TableItem> QueryAsync(string filter = null, int? maxPerPage = null, CancellationToken cancellationToken = default)
        {
            var language = new ODataFilterLanguage();

            Expression<Func<TableItem, bool>> predicate = filter == null ? ((TableItem) => true) : language.Parse<TableItem>(filter);

            return base.QueryAsync(predicate, maxPerPage, cancellationToken);
        }
    }
}