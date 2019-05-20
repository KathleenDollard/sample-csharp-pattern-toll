using TollingData;
using TollEngine;
using Common;
using System;

namespace TollRunner

{
    class Program
    {
        static void Main(string[] args)
        {
            DisplaySomeData.DisplayTolls();

            TollRunner.BillTolls();

        }
    }
}