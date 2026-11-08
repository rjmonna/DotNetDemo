using DotNetDemo.Services.Contracts;

namespace DotNetDemo.Worker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    private readonly IArticleCommentService _articleCommentService;

    public Worker(ILogger<Worker> logger, IArticleCommentService articleCommentService)
    {
        _logger = logger;

        _articleCommentService = articleCommentService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }
            await Task.Delay(1000, stoppingToken);

            // Get pending ArticleComments, preferably without deleted, by calling /open;
            // Loop available comments;
            // When action needed, queue process task which calls /process.
            //
            // Aternatively, call /pendingprocess and then for each call /process.

            var articleComments = await _articleCommentService.GetPendingArticleComments();

            foreach (var articleComment in articleComments)
            {
                await _articleCommentService.ProcessArticleComment(articleComment.ArticleCommentId);
            }

            // In another process, purge deleted ArticleComments, by calling /pendingpurge (returns all ArticleComment from TableStorage With DeletionDate filled);
            // Loop comments to purge;
            // For each, queue purge task which calls /purge.

            articleComments = await _articleCommentService.GetPendingArticleComments();

            foreach (var articleComment in articleComments)
            {
                if (articleComment.DeletionDate.HasValue && articleComment.DeletionDate <= DateTime.UtcNow)
                {
                    await _articleCommentService.PurgeArticleComment(articleComment.ArticleCommentId);
                }
            }
        }
    }
}
