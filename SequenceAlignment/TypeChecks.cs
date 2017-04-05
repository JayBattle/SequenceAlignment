using System;

namespace SequenceAlignment {
    public static class TypeChecks {

        public static int CheckInt(string intAsString) {
            int currInt;
            if (Int32.TryParse(intAsString, out currInt)) return currInt;
            else {
                Console.WriteLine("Input was not an integer!");
                Console.WriteLine("Press any key to exit... ");
                Console.ReadKey();
                Environment.Exit(0);
                return 0;
            }
        }

        public static bool CheckSeqLength(string currSeq, int length) {
            if (currSeq.Length == length) return true;
            else {
                Console.WriteLine("Input sequence did not match the specified length!");
                Console.WriteLine("Press any key to exit... ");
                Console.ReadKey();
                Environment.Exit(0);
                return false;
            }
        }
    }
}
