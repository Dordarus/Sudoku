using System;

namespace Sudoku
{
    internal static class TrueRandom
    {
        private static readonly Random GetRandom = new Random();
        private static readonly object SyncLock = new object();

        public static int GetRandomNumber(int min, int max)
        {
            lock (SyncLock)
            {
                return GetRandom.Next(min, max);
            }
        }
    }
}