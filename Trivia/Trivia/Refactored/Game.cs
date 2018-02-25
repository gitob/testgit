using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trivia.Refactored
{
    public class Game
    {
        private readonly IMessageChannel _channel;
        readonly List<string> _players = new List<string>();
        readonly int[] _places = new int[6];
        readonly int[] _purses = new int[6];
        readonly bool[] _inPenaltyBox = new bool[6];

        readonly LinkedList<string> _popQuestions = new LinkedList<string>();
        readonly LinkedList<string> _scienceQuestions = new LinkedList<string>();
        readonly LinkedList<string> _sportsQuestions = new LinkedList<string>();
        readonly LinkedList<string> _rockQuestions = new LinkedList<string>();

        int currentPlayer = 0;
        bool isGettingOutOfPenaltyBox;

        public Game(IMessageChannel messagechannel)
        {
            _channel = messagechannel;
            for (int i = 0; i < 50; i++)
            {
                _popQuestions.AddLast("Pop Question " + i);
                _scienceQuestions.AddLast("Science Question " + i);
                _sportsQuestions.AddLast("Sports Question " + i);
                _rockQuestions.AddLast(createRockQuestion(i));
            }
        }

        public bool wrongAnswer()
        {
            _channel.WriteMessage("Question was incorrectly answered");
            _channel.WriteMessage(_players[currentPlayer] + " was sent to the penalty box");
            _inPenaltyBox[currentPlayer] = true;

            currentPlayer++;
            if (currentPlayer == _players.Count) currentPlayer = 0;
            return true;
        }

        public bool add(String playerName)
        {
            _players.Add(playerName);
            _places[howManyPlayers()] = 0;
            _purses[howManyPlayers()] = 0;
            _inPenaltyBox[howManyPlayers()] = false;

            _channel.WriteMessage(playerName + " was added");
            _channel.WriteMessage("They are player number " + _players.Count);
            return true;
        } 

        private String createRockQuestion(int index)
        {
            return "Rock Question " + index;
        }

        private bool isPlayable()
        {
            return (howManyPlayers() >= 2);
        }
           

        public int howManyPlayers()
        {
            return _players.Count;
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
                    _places[currentPlayer] = _places[currentPlayer] + roll;
                    if (_places[currentPlayer] > 11) _places[currentPlayer] = _places[currentPlayer] - 12;

                    _channel.WriteMessage(_players[currentPlayer]
                            + "'s new location is "
                            + _places[currentPlayer]);
                    _channel.WriteMessage("The category is " + currentCategory());
                    askQuestion();
                }
                else
                {
                    _channel.WriteMessage(_players[currentPlayer] + " is not getting out of the penalty box");
                    isGettingOutOfPenaltyBox = false;
                }

            }
            else
            {

                _places[currentPlayer] = _places[currentPlayer] + roll;
                if (_places[currentPlayer] > 11) _places[currentPlayer] = _places[currentPlayer] - 12;

                _channel.WriteMessage(_players[currentPlayer]
                        + "'s new location is "
                        + _places[currentPlayer]);
                _channel.WriteMessage("The category is " + currentCategory());
                askQuestion();
            }

        }

        private void askQuestion()
        {
            if (currentCategory() == "Pop")
            {
                _channel.WriteMessage(_popQuestions.First());
                _popQuestions.RemoveFirst();
            }
            if (currentCategory() == "Science")
            {
                _channel.WriteMessage(_scienceQuestions.First());
                _scienceQuestions.RemoveFirst();
            }
            if (currentCategory() == "Sports")
            {
                _channel.WriteMessage(_sportsQuestions.First());
                _sportsQuestions.RemoveFirst();
            }
            if (currentCategory() == "Rock")
            {
                _channel.WriteMessage(_rockQuestions.First());
                _rockQuestions.RemoveFirst();
            }
        }


        private String currentCategory()
        {
            if (_places[currentPlayer] == 0) return "Pop";
            if (_places[currentPlayer] == 4) return "Pop";
            if (_places[currentPlayer] == 8) return "Pop";
            if (_places[currentPlayer] == 1) return "Science";
            if (_places[currentPlayer] == 5) return "Science";
            if (_places[currentPlayer] == 9) return "Science";
            if (_places[currentPlayer] == 2) return "Sports";
            if (_places[currentPlayer] == 6) return "Sports";
            if (_places[currentPlayer] == 10) return "Sports";
            return "Rock";
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
            if (currentPlayer == _players.Count) currentPlayer = 0;

            return winner;
        }

        private void GetPlayerTurn()
        {
            currentPlayer++;
            if (currentPlayer == _players.Count) currentPlayer = 0;
        }

        private bool didPlayerWin()
        {
            return !(_purses[currentPlayer] == 6);
        }
    }
}
