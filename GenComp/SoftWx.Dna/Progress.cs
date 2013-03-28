
namespace SoftWx.Dna {
    /// <summary>
    /// Simple class to allow conveying progress of an operation between threads.
    /// </summary>
    public class Progress {
        volatile int percentComplete = 0;

        /// <summary>
        /// Gets or sets the percentage of completion as a whole integer value from 0 to 100.
        /// </summary>
        public int PercentComplete {
            get { return this.percentComplete; }
            set { this.percentComplete = value; }
        }

        /// <summary>
        /// Set the percentage of completion by specifying an amount completed and the total
        /// amount to be completed.
        /// </summary>
        /// <param name="amount">The amout completed.</param>
        /// <param name="totalAmount">The total amount to be completed (including any already
        /// completed).</param>
        public void Set(int amount, int totalAmount) {
            this.percentComplete = ((100 * amount) + 50) / totalAmount;
        }

        /// <summary>
        /// Set the percentage of completion by specifying an amount completed and the total
        /// amount to be completed.
        /// </summary>
        /// <param name="amount">The amout completed.</param>
        /// <param name="totalAmount">The total amount to be completed (including any already
        /// completed).</param>
        public void Set(long amount, long totalAmount) {
            this.percentComplete = (int) (((100 * amount) + 50) / totalAmount);
        }

        /// <summary>
        /// Set the percentage of completion by specifying an amount completed and the total
        /// amount to be completed.
        /// </summary>
        /// <param name="amount">The amout completed.</param>
        /// <param name="totalAmount">The total amount to be completed (including any already
        /// completed).</param>
        public void Set(double amount, double totalAmount) {
            this.percentComplete = 100 * (int) ((amount / totalAmount) + 0.5);
        }
    }
}
