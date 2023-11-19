using DotNetDemo.Api.Models;
using DotNetDemo.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotNetDemo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleCommentRepository _articleCommentRepository;
        private readonly IArticleRepository _articleRepository;

        public ArticleController(IArticleRepository articleRepository, IArticleCommentRepository articleCommentRepository)
        {
            _articleCommentRepository = articleCommentRepository;
            _articleRepository = articleRepository;
        }

        [HttpGet]
        public async Task<ActionResult> SearchArticles(int? top = 0, int? skip = 0, bool? isDescending = true)
        {
            return Ok(await _articleRepository.SearchArticles());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetArticle(Guid id)
        {
            return Ok(await _articleRepository.GetArticle(id));
        }

        [HttpPost]
        public async Task<ActionResult> CreateArticle(Article article)
        {
            await _articleRepository.CreateArticle(article);

            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult> UpdateArticle(Article article)
        {
            await _articleRepository.UpdateArticle(article);
            
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteArticle(Guid id)
        {
            _articleRepository.DeleteArticle(id);

            return Ok();
        }

        [HttpPost("{id}/comment")]
        public ActionResult CommentArticle(ArticleComment comment)
        {
            _articleCommentRepository.CreateArticleComment(new Models.Azure.ArticleComment{
                Subject = comment.Subject,
                Body = comment.Body
            });

            return Ok();
        }
    }
}