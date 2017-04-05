using System;

namespace SequenceAlignment {
    class Program {
        private static int debugLevel => 2;
        private static bool autoDebug => true;
        static void Main(string[] args) {

            int seqInputLength;
            Console.WriteLine("Please input the lenght of original sequence as int: ");
            if (autoDebug) seqInputLength = TypeChecks.CheckInt("5");
            else seqInputLength = TypeChecks.CheckInt(Console.ReadLine());

            string seqInput;
            Console.WriteLine("Please input the original sequence as string: ");
            if (autoDebug) seqInput = "AACAG";
            else seqInput = Console.ReadLine();
            TypeChecks.CheckSeqLength(seqInput, seqInputLength);

            int seqTargetLength;
            Console.WriteLine("Please input the lenght of target sequence as int: ");
            if (autoDebug) seqTargetLength = TypeChecks.CheckInt("3");
            else seqTargetLength = TypeChecks.CheckInt(Console.ReadLine());

            string seqTarget;
            Console.WriteLine("Please input the target sequence as string: ");
            if (autoDebug) seqTarget = "TAA";
            else seqTarget = Console.ReadLine();
            TypeChecks.CheckSeqLength(seqTarget, seqTargetLength);

            string outputFileName;
            Console.WriteLine("Please input the target sequence as string: ");
            if (autoDebug) outputFileName = "CostMatrixOut.txt";
            else outputFileName = Console.ReadLine();

            int numRows = seqInputLength + 2;
            int numCols = seqTargetLength + 2;
            CostMatrix Matrix = new CostMatrix(seqInput, seqTarget, numRows, numCols);
            Matrix.ExportMatrix(outputFileName);

            Console.WriteLine("Press any key to exit... ");
            Console.ReadKey();
        }

    }
}
