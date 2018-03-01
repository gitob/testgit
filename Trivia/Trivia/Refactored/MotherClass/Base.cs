using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trivia.Refactored.Interface;

namespace Trivia.Refactored.MotherClass
{
    public abstract class Base
    {
        protected string GetQuestion { get; }
        protected int GetId { get; }
        protected QuestionType GetType { get; }

        protected Base(string question, int id, QuestionType type)
        {
            GetQuestion = question;
            GetId = id;
            GetType = type;
        }
    }
}
