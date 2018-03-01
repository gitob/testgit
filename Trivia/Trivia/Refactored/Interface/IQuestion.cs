using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trivia.Refactored.Interface
{
    public enum QuestionType
    {
        Pop,
        Science,
        Sports,
        Rock
    }

    public interface IQuestion
    {
        string GetQuestion();
        int GetId();
        QuestionType GetType();
    }
}
