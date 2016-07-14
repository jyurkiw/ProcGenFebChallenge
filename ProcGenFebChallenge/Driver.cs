﻿using PlatformerLSystem;
using PlatformerLSystem.Models;
using PlatformerLSystem.Models.Configuration;
using PlatformerLSystem.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcGenFebChallenge
{
    public class Driver
    {
        public static void Main(string[] args)
        {
            Language2D lang = new Language2D("conf/lsys.json");

            IList<string> l = new List<string>() { "123", "7O8", "456" };
            NounDimension d = new NounDimension();
            d.Height = 3;
            d.Width = 3;

            Noun2D n = new Noun2D(l, d);

            for (int i = 0; i < 5; i++)
            {
                Noun2D n2 = lang[n];
                if (n2 != null)
                    Console.WriteLine(lang[n].ToString() + '\n');
                else Console.WriteLine("No match!");
            }

            Console.Read();
        }

        public static void OldMain(string[] args)
        {
            PRandLSystem system = new PRandLSystem("conf/lsys.json");
            HighLevelRoom head = new HighLevelRoom();
            HighLevelRoomConfigurator configurator = new HighLevelRoomConfigurator();

            for (int i = 0; i < 40; i++)
            {

                WriteLevelToConsole();
                Console.WriteLine("\n===================================\n");

                system.ProcessSystem();

                foreach (GridPos pos in new List<GridPos>(HighLevelRoom.HighLevelLocDict.Keys))
                {
                    configurator.Configure(HighLevelRoom.HighLevelLocDict[pos], HighLevelRoom.HighLevelLocDict[pos].Type);
                }
            }

            WriteLevelToConsole();

            Console.Read();
        }

        public static void WriteLevelToConsole()
        {
            IEnumerable<GridPos> roomPositions = HighLevelRoom.HighLevelLocDict.Keys;

            int minX = roomPositions.Min(g => g.X);
            int maxX = roomPositions.Max(g => g.X);
            int minY = roomPositions.Min(g => g.Y);
            int maxY = roomPositions.Max(g => g.Y);

            Console.WriteLine("({0}, {1}) to ({2}, {3})", minX, minY, maxX, maxY);
            
            Console.WriteLine();

            for (GridPos g = new GridPos(minX, maxY); g.Y >= minY; g.Y--)
            {
                for (g.X = minX; g.X <= maxX; g.X++)
                {
                    if (HighLevelRoom.HighLevelLocDict.ContainsKey(g))
                        Console.Write(HighLevelRoom.HighLevelLocDict[g].Type);
                    else Console.Write(" ");
                }
                Console.WriteLine(string.Empty);
            }
        }
    }
}