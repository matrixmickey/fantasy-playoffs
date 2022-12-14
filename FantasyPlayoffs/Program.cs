using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FantasyPlayoffs
{
    enum TeamName
    {
        TeamTebowTearsHeavenEleven,
        DillonPanthers,
        CriticalChaseTheory,
        ItIsMeSickos,
        AFuckingTeamName,
        Bryan,
        LightsKamaraAction,
        WeAmHerschel,
        PeakedLastSeason,
        AllBarkleyNoBite,
        RunCMC,
        Fumbledore,
        TheKGarDynasty,
        TheReplacements
    }

    class Program
    {
        static void Main(string[] args)
        {
            var teams = new Team[] {
                new Team(8), // TeamTebowTearsHeavenEleven
                new Team(6), // DillonPanthers
                new Team(7), // CriticalChaseTheory
                new Team(5), // ItIsMeSickos
                new Team(7), // AFuckingTeamName
                new Team(5), // Bryan
                new Team(5), // LightsKamaraAction
                new Team(6), // WeAmHerschel
                new Team(5), // PeakedLastSeason
                new Team(5), // AllBarkleyNoBite
                new Team(7), // RunCMC
                new Team(3), // Fumbledore
                new Team(4), // TheKGarDynasty
                new Team(4)  // TheReplacements
            };
            var remainingGames = new Game[]
            {
                new Game(teams[(int)TeamName.TeamTebowTearsHeavenEleven], teams[(int)TeamName.Bryan]),
                new Game(teams[(int)TeamName.Fumbledore], teams[(int)TeamName.WeAmHerschel]),
                new Game(teams[(int)TeamName.AFuckingTeamName], teams[(int)TeamName.CriticalChaseTheory]),
                new Game(teams[(int)TeamName.LightsKamaraAction], teams[(int)TeamName.TheReplacements]),
                new Game(teams[(int)TeamName.DillonPanthers], teams[(int)TeamName.TheKGarDynasty]),
                new Game(teams[(int)TeamName.ItIsMeSickos], teams[(int)TeamName.AllBarkleyNoBite]),
                new Game(teams[(int)TeamName.PeakedLastSeason], teams[(int)TeamName.RunCMC]),
                new Game(teams[(int)TeamName.TeamTebowTearsHeavenEleven], teams[(int)TeamName.Fumbledore]),
                new Game(teams[(int)TeamName.AFuckingTeamName], teams[(int)TeamName.DillonPanthers]),
                new Game(teams[(int)TeamName.WeAmHerschel], teams[(int)TeamName.TheKGarDynasty]),
                new Game(teams[(int)TeamName.CriticalChaseTheory], teams[(int)TeamName.PeakedLastSeason]),
                new Game(teams[(int)TeamName.LightsKamaraAction], teams[(int)TeamName.Bryan]),
                new Game(teams[(int)TeamName.ItIsMeSickos], teams[(int)TeamName.TheReplacements]),
                new Game(teams[(int)TeamName.AllBarkleyNoBite], teams[(int)TeamName.RunCMC])
            };

            long scenarios = 0;

            new Timer((e) => { Console.WriteLine(scenarios); }, null, TimeSpan.FromMinutes(0), TimeSpan.FromSeconds(1));

            void playGames(int index)
            {
                if (index < remainingGames.Length)
                {
                    var game = remainingGames[index];
                    game.team1.wins++;
                    playGames(index + 1);
                    game.team1.wins--;
                    game.team2.wins++;
                    playGames(index + 1);
                    game.team2.wins--;
                }
                else
                {
                    var teamsOrderedWorstToBest = teams.OrderBy(team => team.wins);
                    var playoffWinThreshold = teamsOrderedWorstToBest.ElementAt(8).wins;
                    var arePlayoffsClear = teamsOrderedWorstToBest.ElementAt(7).wins != playoffWinThreshold;
                    var playoffByeWinThreshold = teamsOrderedWorstToBest.ElementAt(12).wins;
                    var arePlayoffByesClear = teamsOrderedWorstToBest.ElementAt(11).wins != playoffByeWinThreshold;
                    foreach (var team in teams)
                    {
                        if (!arePlayoffsClear && team.wins == playoffWinThreshold)
                        {
                            team.playoffsMaybeMadeBasedOnPoints++;
                        }
                        else if (team.wins >= playoffWinThreshold)
                        {
                            team.playoffsDefinitelyMade++;
                        }
                        else
                        {
                            team.playoffsDefinitelyNotMade++;
                        }
                        if (!arePlayoffByesClear && team.wins == playoffByeWinThreshold)
                        {
                            team.playoffsByeMaybeMadeBasedOnPoints++;
                        }
                        else if (team.wins >= playoffByeWinThreshold)
                        {
                            team.playoffsByeDefinitelyMade++;
                        }
                        else
                        {
                            team.playoffsByeDefinitelyNotMade++;
                        }
                    }
                    scenarios++;
                }
            }

            playGames(0);

            var fileText = $"{scenarios},In Playoffs,Maybe In Playoffs With Points,Not In Playoffs,,{scenarios},Week 14 Bye,Maybe Week 14 Bye With Points,No Week 14 Bye";

            for (var i = 0; i < teams.Length; i++)
            {
                fileText += $"\n{(TeamName)i},{teams[i].playoffsDefinitelyMade},{teams[i].playoffsMaybeMadeBasedOnPoints},{teams[i].playoffsDefinitelyNotMade},,{(TeamName)i},{teams[i].playoffsByeDefinitelyMade},{teams[i].playoffsByeMaybeMadeBasedOnPoints},{teams[i].playoffsByeDefinitelyNotMade}";
            }

            fileText += $"\n\n100,In Playoffs,Maybe In Playoffs With Points,Not In Playoffs,,100,Week 14 Bye,Maybe Week 14 Bye With Points,No Week 14 Bye";

            for (var i = 0; i < teams.Length; i++)
            {
                fileText += $"\n{(TeamName)i},{String.Format("{0:0.00}", (double)teams[i].playoffsDefinitelyMade/scenarios*100)},{String.Format("{0:0.00}", (double)teams[i].playoffsMaybeMadeBasedOnPoints/scenarios*100)},{String.Format("{0:0.00}", (double)teams[i].playoffsDefinitelyNotMade/scenarios*100)},,{(TeamName)i},{String.Format("{0:0.00}", (double)teams[i].playoffsByeDefinitelyMade/scenarios*100)},{String.Format("{0:0.00}", (double)teams[i].playoffsByeMaybeMadeBasedOnPoints/scenarios*100)},{String.Format("{0:0.00}", (double)teams[i].playoffsByeDefinitelyNotMade/scenarios*100)}";
            }

            File.WriteAllText("obama.csv", fileText);
        }
    }

    class Team
    {
        internal int wins;
        internal long playoffsDefinitelyMade = 0;
        internal long playoffsMaybeMadeBasedOnPoints = 0;
        internal long playoffsDefinitelyNotMade = 0;
        internal long playoffsByeDefinitelyMade = 0;
        internal long playoffsByeMaybeMadeBasedOnPoints = 0;
        internal long playoffsByeDefinitelyNotMade = 0;

        public Team(int currentWins)
        {
            this.wins = currentWins;
        }
    }

    class Game
    {
        internal readonly Team team1;
        internal readonly Team team2;

        public Game(Team team1, Team team2)
        {
            this.team1 = team1;
            this.team2 = team2;
        }
    }
}
