using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Homework05.Models
{
    public class Question
    {
        public string QuestionId { get; set; }
        public string QuestionText { get; set; }
        public QuestionType QuestionType { get; set; }
        public List<string> Options { get; set; }

    }

    public enum QuestionType
    {
        TextEntry,
        Choice,
        Message
    }
}