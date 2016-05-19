using System;

namespace osmutil
{
    class Program
    {
        static void Main(string[] args)
        {
            //var ticker = new TickboxTicker();
            //ticker.DoIt();
            var query = new FindMembersWithMisingData();
            query.DoIt();

            Console.ReadLine();
        }
    }
}
