﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPortal.Core.Models
{
    public class Comment : BaseEntity
    {
        // Link to users table
        public string UserAccountId { get; set; }
        public virtual UserAccount UserAccount { get; set; }

        public string ERApplicationId { get; set; }
        public virtual ERApplication ERApplication { get; set; }

        public CommentType Type { get; set; }
        public string Text { get; set; }
    }

    public enum CommentType
    {
        DGHCommentForApplication,
        DGHCommentForPilot,
        ERCCommentForApplication,
        ERCCommentforPilot,
        Others
    }
}
