using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trivia.Refactored
{
    public class GameRunner
    {
        private static bool notAWinner;

        public static void Run(IMessageChannel messagechannel, Random random)
        {
            Game aGame = new Game(messagechannel);

            do
            {
                aGame.roll(random.Next(5) + 1);

                notAWinner = random.Next(9) == 7 ? aGame.wrongAnswer() : aGame.wasCorrectlyAnswered();
            } while (notAWinner);
        }
    }
}