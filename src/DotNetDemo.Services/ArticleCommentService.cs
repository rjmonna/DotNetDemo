using System.Net.Http.Json;
using DotNetDemo.Models;
using DotNetDemo.Services.Contracts;

namespace DotNetDemo.Services
{
    public class ArticleCommentService : IArticleCommentService
    {
        private readonly HttpClient _httpClient;

        public ArticleCommentService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task CommentArticle(ArticleComment comment)
        {
            await _httpClient.PostAsJsonAsync($"api/articlecomment", comment);
        }

        public async Task<IEnumerable<ArticleComment>> GetPendingArticleComments()
        {
            return await _httpClient.GetFromJsonAsync<ArticleComment[]>("api/articlecomment/open");
        }

        public async Task<ArticleComment> GetArticleComment(Guid id)
        {
            return await _httpClient.GetFromJsonAsync<ArticleComment>($"api/articlecomment/{id}");
        }

        public async Task ApproveArticleComment(Guid id)
        {
            await _httpClient.PostAsJsonAsync($"api/articlecomment/{id}/approve", true);

            return;
        }

        public async Task DeclineArticleComment(Guid id)
        {
            await _httpClient.PostAsJsonAsync($"api/articlecomment/{id}/decline", true);

            return;
        }

        public async Task ProcessArticleComment(Guid id)
        {
            await _httpClient.PostAsJsonAsync($"api/articlecomment/{id}/process", true);

            return;
        }

        public async Task PurgeArticleComment(Guid id)
        {
            await _httpClient.PostAsJsonAsync($"api/articlecomment/{id}/purge", true);

            return;
        }
    }
}