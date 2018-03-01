using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trivia.Refactored.Data;
using Trivia.Refactored.Interface;

namespace Trivia.Refactored
{
    public class Game
    {
        private readonly IMessageChannel _channel;
        readonly List<string> _players = new List<string>();
        readonly int[] _places = new int[6];
        readonly int[] _purses = new int[6];
        readonly bool[] _inPenaltyBox = new bool[6];
        private readonly Dictionary<string, Action> _askQuestion = new Dictionary<string, Action>();
        private readonly List<IQuestion> _questions = new List<IQuestion>();
        private readonly Dictionary<int, string> _category = new Dictionary<int, string>
        {
            {0, "Pop"},
            {4, "Pop" },
            {8, "Pop" },
            {1, "Science"},
            {5, "Science"},
            {9, "Science"},
            {2, "Sports"},
            {6, "Sports"},
            {10, "Sports"}

        };
        int currentPlayer = 0;
        bool isGettingOutOfPenaltyBox;

        public Game(IMessageChannel messagechannel)
        {
            _channel = messagechannel;
            Initvars();
        }

        private void Initvars()
        {
            _askQuestion.Add("Pop", RemovePop);
            _askQuestion.Add("Sports", RemoveSport);
            _askQuestion.Add("Rock", RemoveRock);
            _askQuestion.Add("Science", RemoveScience);
            add("Chet");
            add("Pat");
            add("Sue");

            for (int i = 0; i < 50; i++)
            {
                _questions.Add(new Question($"Pop Question {i}", i, QuestionType.Pop));
                _questions.Add(new Question($"Science Question {i}", i, QuestionType.Science));
                _questions.Add(new Question($"Sports Question {i}", i, QuestionType.Sports));
                _questions.Add(new Question($"Rock Question {i}", i, QuestionType.Rock));
            }
        }

        public bool wrongAnswer()
        {
            _channel.WriteMessage("Question was incorrectly answered");
            _channel.WriteMessage(_players[currentPlayer] + " was sent to the penalty box");
            _inPenaltyBox[currentPlayer] = true;

            currentPlayer++;
            if (currentPlayer == _players.Count)
                currentPlayer = 0;
            return true;
        }

        void add(String playerName)
        {
            _players.Add(playerName);
            _places[_players.Count] = 0;
            _purses[_players.Count] = 0;
            _inPenaltyBox[_players.Count] = false;

            _channel.WriteMessage(playerName + " was added");
            _channel.WriteMessage("They are player number " + _players.Count);
        }

        public void roll(int roll)
        {
            _channel.WriteMessage(_players[currentPlayer] + " is the current player");
            _channel.WriteMessage("They have rolled a " + roll);

            if (_inPenaltyBox[currentPlayer])
            {
                if (roll % 2 != 0)
                {
                    isGettingOutOfPenaltyBox = true;
                    _channel.WriteMessage(_players[currentPlayer] + " is getting out of the penalty box");
                }
                else
                {
                    _channel.WriteMessage(_players[currentPlayer] + " is not getting out of the penalty box");
                    isGettingOutOfPenaltyBox = false;
                    return;
                }
            }
            _places[currentPlayer] = _places[currentPlayer] + roll;
            if (_places[currentPlayer] > 11) _places[currentPlayer] = _places[currentPlayer] - 12;

            _channel.WriteMessage(_players[currentPlayer]
                    + "'s new location is "
                    + _places[currentPlayer]);
            _channel.WriteMessage("The category is " + currentCategory());
            _askQuestion[currentCategory()].Invoke();
        }

        void RemovePop()
        {
            var first = _questions.FirstOrDefault(x => x.GetType() == QuestionType.Pop);
            _channel.WriteMessage(first.GetQuestion());
            _questions.Remove(first);
        }

        void RemoveScience()
        {
            var first = _questions.FirstOrDefault(x => x.GetType() == QuestionType.Science);
            _channel.WriteMessage(first.GetQuestion());
            _questions.Remove(first);
        }
        void RemoveSport()
        {
            var first = _questions.FirstOrDefault(x => x.GetType() == QuestionType.Sports);
            _channel.WriteMessage(first.GetQuestion());
            _questions.Remove(first);
        }

        void RemoveRock()
        {
            var first = _questions.FirstOrDefault(x => x.GetType() == QuestionType.Rock);

            _channel.WriteMessage(first.GetQuestion());
            _questions.Remove(first);
        }

        private String currentCategory()
        {
            if (!_category.TryGetValue(_places[currentPlayer], out var val))
                return "Rock";
            return val;
        }

        public bool wasCorrectlyAnswered()
        {
            if (_inPenaltyBox[currentPlayer] && !isGettingOutOfPenaltyBox)
            {
                GetPlayerTurn();
                return true;
            }

            _channel.WriteMessage("Answer was correct!!!!");
            _purses[currentPlayer]++;
            _channel.WriteMessage(_players[currentPlayer]
                        + " now has "
                        + _purses[currentPlayer]
                        + " Gold Coins.");

            bool winner = didPlayerWin();
            currentPlayer++;
            if (currentPlayer == _players.Count)
                currentPlayer = 0;
            return winner;
        }

        private void GetPlayerTurn()
        {
            currentPlayer++;
            if (currentPlayer == _players.Count)
                currentPlayer = 0;
        }

        private bool didPlayerWin()
        {
            return _purses[currentPlayer] != 6;
        }
    }
}
