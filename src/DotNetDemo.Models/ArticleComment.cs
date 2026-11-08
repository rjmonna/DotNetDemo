using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetDemo.Models
{
    public class ArticleComment
    {
        public Guid ArticleCommentId { get; set; }

        public Guid ArticleId { get; set; }
        
        public string Subject { get; set; }

        public string Body { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime ModificationDate { get; set; }

        public DateTime? DeletionDate { get; set; }

        public Article? Article { get; set; }
    }
}