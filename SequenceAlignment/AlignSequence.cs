using System;

namespace SequenceAlignment {
    public class AlignSequence {

        //run on ubuntu.16.04-x64
        private static int debugLevel => 2;
        private static bool autoDebug => false;
        static void Main(string[] args) {

            int seqInputLength;
            Console.WriteLine("Please input the lenght of original sequence as int: ");
            if (autoDebug) seqInputLength = TypeChecks.CheckInt("5");
            else seqInputLength = TypeChecks.CheckInt(Console.ReadLine());

            string seqInput;
            Console.WriteLine("Please input the original sequence as string: ");
            if (autoDebug) seqInput = "TTATT";
            else seqInput = Console.ReadLine();
            TypeChecks.CheckSeqLength(seqInput, seqInputLength);

            int seqTargetLength;
            Console.WriteLine("Please input the lenght of target sequence as int: ");
            if (autoDebug) seqTargetLength = TypeChecks.CheckInt("5");
            else seqTargetLength = TypeChecks.CheckInt(Console.ReadLine());

            string seqTarget;
            Console.WriteLine("Please input the target sequence as string: ");
            if (autoDebug) seqTarget = "ATTTT";
            else seqTarget = Console.ReadLine();
            TypeChecks.CheckSeqLength(seqTarget, seqTargetLength);

            string outputFileName;
            Console.WriteLine("Please input the desired name for the output file: ");
            if (autoDebug) outputFileName = "CostMatrixOut.txt";
            else outputFileName = Console.ReadLine();

            CostMatrix Matrix = new CostMatrix(seqInput, seqTarget);
            Matrix.ExportMatrix(outputFileName);

            Console.WriteLine("Press any key to exit... ");
            Console.ReadKey();
        }

    }
}
