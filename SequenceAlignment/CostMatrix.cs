using System;
using System.Collections;
using System.IO;


namespace SequenceAlignment {
    public class CostMatrix {

        public static string[,] Matrix;
        public static int NumRows;
        public static int NumCols;
        public static string InputSequence;
        public static string TargetSequence;
        public static string AlignedSequence;
        public static int AlignmentCost;
        private static int debugLevel => 2;

        public CostMatrix(string inputSequence, string targetSequence) {
            InputSequence = inputSequence;
            TargetSequence = targetSequence;
            NumRows = inputSequence.Length + 2;
            NumCols = targetSequence.Length + 2;
            Matrix = new string[NumRows, NumCols];
            AddSequences();
            PrepCostMatrix();
            FillCostMatrix();
            SolveCostMatrix();
        }

        public void AddSequences() {
            for (int row = 1; row <= InputSequence.Length; row++) {
                Matrix[row, 0] = Convert.ToString(InputSequence[row - 1]);
            }
            for (int col = 1; col <= TargetSequence.Length; col++) {
                Matrix[0, col] = Convert.ToString(TargetSequence[col - 1]);
            }
            if (debugLevel > 2) PrintMatrix();
        }

        public void PrintMatrix() {
            for (int row = 0; row < NumRows; row++) {
                for (int col = 0; col < NumCols; col++) {
                    if (Matrix[row, col] == null) Console.Write("-");
                    Console.Write(Matrix[row, col] + " ");
                }
                Console.WriteLine("");
            }
            Console.WriteLine("");
        }

        public void PrepCostMatrix() {
            int costLevel = 0;
            for (int row = NumRows - 1; row > 0; row--) {
                Matrix[row, NumCols - 1] = Convert.ToString(costLevel);
                costLevel += 2;
            }
            costLevel = 0;
            for (int col = NumCols - 1; col > 0; col--) {
                Matrix[NumRows - 1, col] = Convert.ToString(costLevel);
                costLevel += 2;
            }
            if (debugLevel > 3) PrintMatrix();
        }

        public void FillCostMatrix() {
            for (int row = NumRows - 2; row > 0; row--) {
                for (int col = NumCols - 2; col > 0; col--) {
                    int right = Convert.ToInt32(Matrix[row, col + 1]) + 2;
                    int diag = Convert.ToInt32(Matrix[row + 1, col + 1]);
                    int down = Convert.ToInt32(Matrix[row + 1, col]) + 2;
                    if (Matrix[row, 0] != Matrix[0, col]) diag += 1;
                    ArrayList costs = new ArrayList();
                    costs.Add(right);
                    costs.Add(diag);
                    costs.Add(down);
                    Matrix[row, col] = Convert.ToString(Stats.GetMin(costs));
                    if (debugLevel > 4) Console.WriteLine(Matrix[row, col]);
                }
            }
            if (debugLevel > 1) PrintMatrix();
        }

        public void SolveCostMatrix() {
            AlignmentCost = 0;
            AlignedSequence = "";
            string nextChar = "";
            int row = 1;
            int col = 1;
            while (row < NumRows - 1 && col < NumCols - 1) {
                int right = Convert.ToInt32(Matrix[row, col + 1]);
                int diag = Convert.ToInt32(Matrix[row + 1, col + 1]);
                int down = Convert.ToInt32(Matrix[row + 1, col]);
                if (Matrix[row, 0] == Matrix[0, col]) {
                    AlignmentCost += 0;
                    nextChar = Matrix[0, col];
                    row++;
                    col++;
                } else if (diag <= right && diag <= down) {
                    AlignmentCost += 1;
                    nextChar = Matrix[0, col];
                    row++;
                    col++;
                } else if (right <= diag && right <= down) {
                    AlignmentCost += 2;
                    nextChar = "-";
                    col++;
                } else if (down <= diag && down <= right) {
                    AlignmentCost += 2;
                    nextChar = "-";
                    row++;
                }
                AlignedSequence += nextChar;
                if (debugLevel > 6) Console.WriteLine("Curr Cost = " + AlignmentCost);
                if (debugLevel > 6) Console.WriteLine(nextChar);
            }
            if (row == col) AlignmentCost += 0;
            else if (row == NumRows - 1) {
                while (col != NumCols) {
                    AlignmentCost += 2;
                    col++;
                    AlignedSequence += "-";
                    if (debugLevel > 6) Console.WriteLine("Curr Cost = " + AlignmentCost);
                    if (debugLevel > 6) Console.WriteLine(nextChar);
                }
            } else {
                while (row != NumRows - 1) {
                    AlignmentCost += 2;
                    row++;
                    AlignedSequence += "-";
                    if (debugLevel > 6) Console.WriteLine("Curr Cost = " + AlignmentCost);
                    if (debugLevel > 6) Console.WriteLine(nextChar);
                }
            }
            if (debugLevel > 1) Console.WriteLine("Aligned sequence is " + AlignedSequence);
            if (debugLevel > 1) Console.WriteLine("Total Cost = " + AlignmentCost);
        }

        public void ExportMatrix(string fileName) {
            if (File.Exists(fileName)) File.Delete(fileName);
            string[] outputLines = new string[NumRows + 4];
            outputLines[0] = ("Input String: " + InputSequence);
            outputLines[1] = ("Target String: " + TargetSequence);
            outputLines[2] = ("Aligned String: " + AlignedSequence);
            outputLines[3] = ("Cost: " + AlignmentCost);
            for (int row = 0; row < NumRows; row++) {
                for (int col = 0; col < NumCols; col++) {
                    if (Matrix[row, col] == null) outputLines[row + 4] += "-";
                    outputLines[row + 4] += (Matrix[row, col] + " ");
                }
            }
            File.AppendAllLines(fileName, outputLines);
        }
    }
}
