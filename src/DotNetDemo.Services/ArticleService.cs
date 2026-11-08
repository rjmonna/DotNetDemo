using System.Net.Http.Json;
using DotNetDemo.Models;
using DotNetDemo.Services.Contracts;

namespace DotNetDemo.Services
{
    public class ArticleService : IArticleService
    {
        private readonly HttpClient _httpClient;

        public ArticleService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task AddArticle(Article article)
        {
            await _httpClient.PostAsJsonAsync($"api/article", article);
        }

        public async Task UpdateArticle(Article article)
        {
            await _httpClient.PutAsJsonAsync($"api/article", article);
        }

        public async Task DeleteArticle(Guid id)
        {
            await _httpClient.DeleteAsync($"api/article/{id}");
        }

        public async Task<Article> GetArticle(Guid id)
        {
            return await _httpClient.GetFromJsonAsync<Article>($"api/article/{id}");
        }

        public async Task<IEnumerable<Article>> SearchArticles()
        {
            return await _httpClient.GetFromJsonAsync<Article[]>("api/article?top=10");
        }

        public async Task CommentArticle(ArticleComment comment)
        {
            await _httpClient.PostAsJsonAsync($"api/article/{comment.ArticleId}/comment", comment);
        }
    }
}