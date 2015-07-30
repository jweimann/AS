﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Actors.UserActors
{
    /// <summary>
    /// This is a copy of ClientActorSystemStats - Create some base for these.
    /// </summary>
    public class UserConnectionStats
    {
        private const float INTERVAL = 1f;
        private static Dictionary<Type, int> _messageCounts = new Dictionary<Type, int>();
        private static DateTime _lastPrint = DateTime.MinValue;

        public static void LogMessage(object message)
        {
            lock(_messageCounts)
            {
                if (_messageCounts.ContainsKey(message.GetType()) == false)
                    _messageCounts.Add(message.GetType(), 0);

                _messageCounts[message.GetType()]++;

                if (ShouldPrintStats())
                    PrintStats();
            }
        }

        private static bool ShouldPrintStats()
        {
            return (DateTime.Now - _lastPrint).TotalSeconds > INTERVAL;
        }

        private static void PrintStats()
        {
            string result = "Message Summary\n";
            foreach (var key in _messageCounts.Keys)
            {
                result += String.Format("{0} {1}/s\n", _messageCounts[key] / INTERVAL, key.ToString());
            }
            Console.WriteLine(result);
            _messageCounts.Clear();
            _lastPrint = DateTime.Now;
        }
    }
}
