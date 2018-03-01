using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trivia.Refactored.Interface;
using Trivia.Refactored.MotherClass;

namespace Trivia.Refactored.Data
{
    public class Question : Base, IQuestion
    {
        public Question(string question, int id, QuestionType type) :
            base(question, id, type)
        {
        }

        string IQuestion.GetQuestion()
        {
            return GetQuestion;
        }

        int IQuestion.GetId()
        {
            return GetId;
        }

        QuestionType IQuestion.GetType()
        {
            return GetType;
        }
    }
}