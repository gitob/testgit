﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UglyTrivia;

namespace Trivia
{
    public class GameRunner
    {

        private static bool notAWinner;

        public static void Main(String[] args)
        {
            GameRunner.Run(new ConsoleMessageCannel(), new Random());
        }

        public static void Run(IMessageChannel messagechannel, Random random)
        {
            Game aGame = new Game(messagechannel);

            aGame.add("Chet");
            aGame.add("Pat");
            aGame.add("Sue");

            do
            {
                aGame.roll(random.Next(5) + 1);

                if (random.Next(9) == 7)
                    notAWinner = aGame.wrongAnswer();
                else
                    notAWinner = aGame.wasCorrectlyAnswered();
            } while (notAWinner);
        }
    }

}

