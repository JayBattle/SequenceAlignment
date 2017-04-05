using System.Collections;

namespace SequenceAlignment {
    public static class Stats {
        public static int GetMin(ArrayList values) {
            int minimum = int.MaxValue;
            foreach (int value in values) {
                if (value < minimum) minimum = value;
            }
            return minimum;
        }
    }
}
