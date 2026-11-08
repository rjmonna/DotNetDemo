using DotNetDemo.Infrastructure;
using DotNetDemo.Infrastructure.Contracts;
using DotNetDemo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;

namespace DotNetDemo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleCommentController : ControllerBase
    {
        private readonly IArticleCommentRepository _articleCommentRepository;
        private readonly IArticleRepository _articleRepository;

        public ArticleCommentController(IArticleCommentRepository articleCommentRepository, IArticleRepository articleRepository)
        {
            _articleCommentRepository = articleCommentRepository;
            _articleRepository = articleRepository;
        }

        [HttpGet("open")]
        public async Task<ActionResult<IEnumerable<DotNetDemo.Infrastructure.Azure.ArticleComment>>> GetPendingArticleComments()
        {
            return Ok(await _articleCommentRepository.GetPending());
        }

        [HttpPut("{id}/process")]
        public async Task<ActionResult> ProcessArticleComment(Guid id)
        {
            var articleComment = await _articleCommentRepository.Get(id);

            var article = await _articleRepository.GetArticle(articleComment.ArticleId);

            if (article == null) throw new InvalidOperationException($"Could not find Article with Id '{id}' to process comment for.");

            if (articleComment.IsApproved)
            {
                article.ArticleComments.Add(new ArticleComment{
                    Body = articleComment.Body,
                    Subject = articleComment.Subject
                });

                await _articleRepository.UpdateArticle(article);

                await _articleCommentRepository.Delete(id);
            }
            else if (articleComment.IsDeclined)
            {
                await _articleCommentRepository.Delete(id);
            }

            return Ok();
        }

        [HttpPut("{id}/purge")]
        public async Task<ActionResult> PurgeArticleComment(Guid id)
        {
            await _articleCommentRepository.Purge(id);

            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DotNetDemo.Infrastructure.Azure.ArticleComment>> GetArticleComment(Guid id)
        {
            return Ok(await _articleCommentRepository.Get(id));
        }

        [HttpPut("{id}/approve")]
        public async Task<ActionResult> ApproveArticleComment(Guid id)
        {
            await _articleCommentRepository.Approve(id);
            
            return Ok();
        }

        [HttpPut("{id}/decline")]
        public async Task<ActionResult> DeclineArticleComment(Guid id)
        {
            await _articleCommentRepository.Decline(id);
            
            return Ok();
        }
    }
}
