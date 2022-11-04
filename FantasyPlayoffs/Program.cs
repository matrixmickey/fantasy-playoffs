﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FantasyPlayoffs
{
    class Program
    {
        static void Main(string[] args)
        {
            var teams = new Team[] {
                new Team(6),
                new Team(5),
                new Team(5),
                new Team(4),
                new Team(4),
                new Team(4),
                new Team(4),
                new Team(4),
                new Team(4),
                new Team(4),
                new Team(4),
                new Team(3),
                new Team(3),
                new Team(2)
            };
            var remainingGames = new Game[]
            {
                new Game(teams[(int)TeamName.TeamTebowTearsHeavenEleven], teams[(int)TeamName.RunCMC]),
                new Game(teams[(int)TeamName.Fumbledore], teams[(int)TeamName.AFuckingTeamName]),
                new Game(teams[(int)TeamName.WeAmHerschel], teams[(int)TeamName.AllBarkleyNoBite]),
                new Game(teams[(int)TeamName.CriticalChaseTheory], teams[(int)TeamName.Bryan]),
                new Game(teams[(int)TeamName.LightsKamaraAction], teams[(int)TeamName.ItIsMeSickos]),
                new Game(teams[(int)TeamName.DillonPanthers], teams[(int)TeamName.TheReplacements]),
                new Game(teams[(int)TeamName.PeakedLastSeason], teams[(int)TeamName.TheKGarDynasty]),
                new Game(teams[(int)TeamName.TeamTebowTearsHeavenEleven], teams[(int)TeamName.AllBarkleyNoBite]),
                new Game(teams[(int)TeamName.Fumbledore], teams[(int)TeamName.CriticalChaseTheory]),
                new Game(teams[(int)TeamName.AFuckingTeamName], teams[(int)TeamName.TheKGarDynasty]),
                new Game(teams[(int)TeamName.WeAmHerschel], teams[(int)TeamName.TheReplacements]),
                new Game(teams[(int)TeamName.LightsKamaraAction], teams[(int)TeamName.RunCMC]),
                new Game(teams[(int)TeamName.DillonPanthers], teams[(int)TeamName.Bryan]),
                new Game(teams[(int)TeamName.ItIsMeSickos], teams[(int)TeamName.PeakedLastSeason]),
                new Game(teams[(int)TeamName.TeamTebowTearsHeavenEleven], teams[(int)TeamName.TheReplacements]),
                new Game(teams[(int)TeamName.Fumbledore], teams[(int)TeamName.DillonPanthers]),
                new Game(teams[(int)TeamName.AFuckingTeamName], teams[(int)TeamName.PeakedLastSeason]),
                new Game(teams[(int)TeamName.WeAmHerschel], teams[(int)TeamName.Bryan]),
                new Game(teams[(int)TeamName.CriticalChaseTheory], teams[(int)TeamName.TheKGarDynasty]),
                new Game(teams[(int)TeamName.LightsKamaraAction], teams[(int)TeamName.AllBarkleyNoBite]),
                new Game(teams[(int)TeamName.ItIsMeSickos], teams[(int)TeamName.RunCMC]),
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

            new Timer((e) => { Console.WriteLine(scenarios); }, null, TimeSpan.FromMinutes(0), TimeSpan.FromMinutes(1));

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
                    var winThreshold = teamsOrderedWorstToBest.ElementAt(8).wins;
                    var arePlayoffsClear = teamsOrderedWorstToBest.ElementAt(7).wins != winThreshold;
                    foreach (var team in teams)
                    {
                        if (!arePlayoffsClear && team.wins == winThreshold)
                        {
                            team.playoffsMaybeMadeBasedOnPoints++;
                        }
                        else if (team.wins >= winThreshold)
                        {
                            team.playoffsDefinitelyMade++;
                        }
                        else
                        {
                            team.playoffsDefinitelyNotMade++;
                        }
                    }
                    scenarios++;
                }
            }

            playGames(0);

            var fileText = "Total scenarios: " + scenarios;

            for (var i = 0; i < teams.Length; i++)
            {
                fileText += "\n" + (TeamName)i + " Playoffs definitely made: " + teams[i].playoffsDefinitelyMade + " Playoffs maybe made based on points: " + teams[i].playoffsMaybeMadeBasedOnPoints + " Playoffs definitely not made: " + teams[i].playoffsDefinitelyNotMade;
            }

            File.WriteAllText("obama.txt", fileText);
        }
    }

    class Team
    {
        internal int wins;
        internal long playoffsDefinitelyMade = 0;
        internal long playoffsMaybeMadeBasedOnPoints = 0;
        internal long playoffsDefinitelyNotMade = 0;

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
}