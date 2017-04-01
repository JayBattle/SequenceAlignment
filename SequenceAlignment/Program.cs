using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceAlignment {
    class Program {
        public static int DebugLevel = int.MaxValue;
        public static bool AutoDebug = true;
        static void Main(string[] args) {

            int seqInputLength;
            Console.WriteLine("Please input the lenght of original sequence as int: ");
            if (AutoDebug) seqInputLength = checkInt("3");
            else seqInputLength = checkInt(Console.ReadLine());

            string seqInput;
            Console.WriteLine("Please input the original sequence as string: ");
            if (AutoDebug) seqInput = "GGC"; //ATATA
            else seqInput = Console.ReadLine();
            checkSeqLength(seqInput, seqInputLength);

            int seqTargetLength;
            Console.WriteLine("Please input the lenght of target sequence as int: ");
            if (AutoDebug) seqTargetLength = checkInt("2");
            else seqTargetLength = checkInt(Console.ReadLine());

            string seqTarget;
            Console.WriteLine("Please input the lenght of target sequence as string: ");
            if (AutoDebug) seqTarget = "GC"; //TAT
            else seqTarget = Console.ReadLine();
            checkSeqLength(seqTarget, seqTargetLength);

            char[] seqInputArray = seqInput.ToCharArray();

            int costMatrixSize;
            if (seqInputLength > seqTargetLength) costMatrixSize = seqInputLength + 2;
            else costMatrixSize = seqTargetLength + 2;

            string[,] costMatrix = new string[costMatrixSize, costMatrixSize];

            for (int row = 1; row <= seqInputLength; row++) {
                costMatrix[row, 0] = Convert.ToString(seqInputArray[row - 1]);
            }
            for (int col = 1; col <= seqTargetLength; col++) {
                costMatrix[0, col] = Convert.ToString(seqTarget[col - 1]);
            }

            if (DebugLevel > 1) printMatrix(costMatrix, costMatrixSize, costMatrixSize);

            int lastIndex = costMatrixSize - 1;
            int costLevel = 0;
            for (int row = lastIndex; row > 0; row--) {
                costMatrix[row, lastIndex] = Convert.ToString(costLevel);
                costLevel += 2;
            }
            costLevel = 0;
            for (int col = lastIndex; col > 0; col--) {
                costMatrix[lastIndex, col] = Convert.ToString(costLevel);
                costLevel += 2;
            }
            if (DebugLevel > 2) printMatrix(costMatrix, costMatrixSize, costMatrixSize);

            costMatrix = fillCostMatrix(costMatrix, costMatrixSize);

            if (DebugLevel > 3) printMatrix(costMatrix, costMatrixSize, costMatrixSize);


            Console.WriteLine("Press any key to exit... ");
            Console.ReadKey();
        }

        //int costMatrixSize;
        //if (seqInputLength > seqTargetLength) costMatrixSize = seqInputLength + 2;
        //else costMatrixSize = seqTargetLength + 2;

        //string[,] costMatrix = new string[costMatrixSize, costMatrixSize];

        //for (int row = 1; row <= seqInputLength; row++) {
        //    costMatrix[row, 0] = Convert.ToString(seqInputArray[row - 1]);
        //}
        //for (int col = 1; col <= seqTargetLength; col++) {
        //    costMatrix[0, col] = Convert.ToString(seqTarget[col - 1]);
        //}

        //if (DebugLevel > 1) printMatrix(costMatrix, costMatrixSize, costMatrixSize);

        //int lastIndex = costMatrixSize - 1;
        //int costLevel = 0;
        //for (int row = lastIndex; row > 0; row--) {
        //    costMatrix[row, lastIndex] = Convert.ToString(costLevel);
        //    costLevel += 2;
        //}
        //costLevel = 0;
        //for (int col = lastIndex; col > 0; col--) {
        //    costMatrix[lastIndex, col] = Convert.ToString(costLevel);
        //    costLevel += 2;
        //}
        //if (DebugLevel > 2) printMatrix(costMatrix, costMatrixSize, costMatrixSize);

        //costMatrix = fillCostMatrix(costMatrix, costMatrixSize);

        //if (DebugLevel > 3) printMatrix(costMatrix, costMatrixSize, costMatrixSize);



        public static int checkInt(string currIntString) {
            int currInt;
            if (Int32.TryParse(currIntString, out currInt)) return currInt;
            else {
                Console.WriteLine("Input was not an integer!");
                Console.WriteLine("Press any key to exit... ");
                Console.ReadKey();
                Environment.Exit(0);
                return 0;
            }
        }

        public static bool checkSeqLength(string currSeq, int length) {
            if (currSeq.Length == length) return true;
            else {
                Console.WriteLine("Input sequence did not match the specified length!");
                Console.WriteLine("Press any key to exit... ");
                Console.ReadKey();
                Environment.Exit(0);
                return false;
            }
        }

        public static void printMatrix(string[,] matrix, int rows, int columns) {
            for (int row = 0; row < rows; row++) {
                for (int col = 0; col < columns; col++) {
                    if (matrix[row, col] == null) Console.Write("-");
                    Console.Write(matrix[row, col] + " ");
                }
                Console.WriteLine("");
            }
            Console.WriteLine("");
        }

        private static string[,] fillCostMatrix(string[,] matrix, int size) {
            int startIndex = size - 2;
            for (int row = startIndex; row > 0; row--) {
                for (int col = startIndex; col > 0; col--) {
                    int right = Convert.ToInt32(matrix[row, col + 1]) + 2;
                    int diag = Convert.ToInt32(matrix[row + 1, col + 1]);
                    int down = Convert.ToInt32(matrix[row + 1, col]) + 2;
                    if (matrix[row, 0] != matrix[0, col]) diag += 1;
                    ArrayList costs = new ArrayList();
                    costs.Add(right);
                    costs.Add(diag);
                    costs.Add(down);
                    matrix[row, col] = Convert.ToString(getMin(costs));
                    //if (DebugLevel > 4) Console.WriteLine(matrix[row, col]);
                }
            }
            return matrix;
        }

        private static int getMin(ArrayList values) {
            int minimum = int.MaxValue;
            foreach (int value in values) {
                if (value < minimum) minimum = value;
            }
            return minimum;
        }


    } // end of class
} // end of namespace
