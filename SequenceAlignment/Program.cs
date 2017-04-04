using System;
using System.Collections;
using System.IO;
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
            if (AutoDebug) seqInputLength = checkInt("5");
            else seqInputLength = checkInt(Console.ReadLine());

            string seqInput;
            Console.WriteLine("Please input the original sequence as string: ");
            if (AutoDebug) seqInput = "AACAG"; //ATATA
            else seqInput = Console.ReadLine();
            checkSeqLength(seqInput, seqInputLength);

            int seqTargetLength;
            Console.WriteLine("Please input the lenght of target sequence as int: ");
            if (AutoDebug) seqTargetLength = checkInt("3");
            else seqTargetLength = checkInt(Console.ReadLine());

            string seqTarget;
            Console.WriteLine("Please input the target sequence as string: ");
            if (AutoDebug) seqTarget = "TAA"; //TAT
            else seqTarget = Console.ReadLine();
            checkSeqLength(seqTarget, seqTargetLength);

            string outputFileName;
            Console.WriteLine("Please input the target sequence as string: ");
            if (AutoDebug) outputFileName = "CostMatrix.txt"; //TAT
            else outputFileName = Console.ReadLine();

            int numRows = seqInputLength + 2;
            int numCols = seqTargetLength + 2;
            string[,] costMatrix = createEmptyCostMatrix(seqInput, seqTarget, numRows, numCols);
            costMatrix = prepCostMatrix(costMatrix, numRows, numCols);
            costMatrix = fillCostMatrix(costMatrix, numRows, numCols);
            string optimalAlignment = solveCostMatrix(costMatrix, numRows, numCols);
            exportMatrix(costMatrix, numRows, numCols, outputFileName);

            Console.WriteLine("Press any key to exit... ");
            Console.ReadKey();
        }

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

        public static void printMatrix(string[,] matrix, int numRows, int numCols) {
            for (int row = 0; row < numRows; row++) {
                for (int col = 0; col < numCols; col++) {
                    if (matrix[row, col] == null) Console.Write("-");
                    Console.Write(matrix[row, col] + " ");
                }
                Console.WriteLine("");
            }
            Console.WriteLine("");
        }

        private static string[,] createEmptyCostMatrix(string input, string target, int numRows, int numCols) {
            string[,] matrix = new string[numRows, numCols];
            for (int row = 1; row <= input.Length; row++) {
                matrix[row, 0] = Convert.ToString(input[row - 1]);
            }
            for (int col = 1; col <= target.Length; col++) {
                matrix[0, col] = Convert.ToString(target[col - 1]);
            }
            if (DebugLevel > 1) printMatrix(matrix, numRows, numCols);
            return matrix;
        }

        private static string[,] prepCostMatrix(string[,] matrix, int numRows, int numCols) {
            int costLevel = 0;
            for (int row = numRows - 1; row > 0; row--) {
                matrix[row, numCols - 1] = Convert.ToString(costLevel);
                costLevel += 2;
            }
            costLevel = 0;
            for (int col = numCols - 1; col > 0; col--) {
                matrix[numRows - 1, col] = Convert.ToString(costLevel);
                costLevel += 2;
            }
            if (DebugLevel > 2) printMatrix(matrix, numRows, numCols);
            return matrix;
        }

        private static string[,] fillCostMatrix(string[,] matrix, int numRows, int numCols) {
            for (int row = numRows - 2; row > 0; row--) {
                for (int col = numCols - 2; col > 0; col--) {
                    int right = Convert.ToInt32(matrix[row, col + 1]) + 2;
                    int diag = Convert.ToInt32(matrix[row + 1, col + 1]);
                    int down = Convert.ToInt32(matrix[row + 1, col]) + 2;
                    if (matrix[row, 0] != matrix[0, col]) diag += 1;
                    ArrayList costs = new ArrayList();
                    costs.Add(right);
                    costs.Add(diag);
                    costs.Add(down);
                    matrix[row, col] = Convert.ToString(getMin(costs));
                    if (DebugLevel > 4) Console.WriteLine(matrix[row, col]);
                }
            }
            if (DebugLevel > 3) printMatrix(matrix, numRows, numCols);
            return matrix;
        }

        private static string solveCostMatrix(string[,] matrix, int numRows, int numCols) {
            string optimalString = "";
            int row = 1;
            int col = 1;
            int cost = 0;//Convert.ToInt32(matrix[row + 1, col + 1]);
            string nextChar = "";
            while (row < numRows - 1 && col < numCols - 1) {
                int right = Convert.ToInt32(matrix[row, col + 1]);
                int diag = Convert.ToInt32(matrix[row + 1, col + 1]);
                int down = Convert.ToInt32(matrix[row + 1, col]);
                if (matrix[row, 0] == matrix[0, col]) {
                    cost += 0;
                    nextChar = matrix[0, col];
                    row++;
                    col++;
                } else if (diag <= right && diag <= down) {
                    cost += 1;
                    nextChar = matrix[0, col]; //matched char
                    row++;
                    col++;
                } else if (right <= diag && right <= down) { //keep char
                    cost += 2;
                    nextChar = "-";
                    col++;
                } else if (down <= diag && down <= right) {
                    cost += 2;
                    nextChar = "-"; //insert char
                    row++;
                }
                optimalString += nextChar;
                if (DebugLevel > 6) Console.WriteLine("Curr Cost = " + cost);
                if (DebugLevel > 6) Console.WriteLine(nextChar);
            }
            if (row == numRows - 1) {
                while (col != numCols) {
                    cost += 2;
                    col++;
                    optimalString += "-";
                    if (DebugLevel > 6) Console.WriteLine("Curr Cost = " + cost);
                    if (DebugLevel > 6) Console.WriteLine(nextChar);
                }
            } else {
                while (row != numRows - 1) {
                    cost += 2;
                    row++;
                    optimalString += "-";
                    if (DebugLevel > 6) Console.WriteLine("Curr Cost = " + cost);
                    if (DebugLevel > 6) Console.WriteLine(nextChar);
                }
            }
            if (DebugLevel > 5) Console.WriteLine(optimalString);
            if (DebugLevel > 5) Console.WriteLine("Total Cost = " + cost);
            return optimalString;
        }

        private static int getMin(ArrayList values) {
            int minimum = int.MaxValue;
            foreach (int value in values) {
                if (value < minimum) minimum = value;
            }
            return minimum;
        }

        public static void exportMatrix(string[,] matrix, int numRows, int numCols, string fileName) {
            if (File.Exists(fileName)) File.Delete(fileName);
            string[] matrixLines = new string[numRows];
            for (int row = 0; row < numRows; row++) {
                for (int col = 0; col < numCols; col++) {
                    if (matrix[row, col] == null) matrixLines[row] += "-";
                    matrixLines[row] += (matrix[row, col] + " ");
                }
            }
            File.WriteAllLines(fileName, matrixLines);
        }


    } // end of class
} // end of namespace
