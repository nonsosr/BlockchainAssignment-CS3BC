using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BlockchainAssignment
{
    internal class Blockchain
    {
        public List<Block> Blocks = new List<Block>();
        public List<Transaction> transactionPool = new List<Transaction>();
        int transactionsPerBlock = 5;

        public Blockchain()
        {
            Blocks.Add(new Block());   // create and append the Genesis Block
        }

        public String GetBlockAsString(int index)
        {
            if (index >= 0 && index < Blocks.Count)
            {
                return Blocks[index].ToString();
            }
            return "No such block exists";
        }

        public Block GetLastBlock()
        {
            return Blocks[Blocks.Count - 1];
        }

        public List<Transaction> getPendingTransactions()
        {
            int n = Math.Min(transactionsPerBlock, transactionPool.Count);
            List<Transaction> transactions = transactionPool.GetRange(0, n);
            transactionPool.RemoveRange(0, n);
            return transactions;
        }

        public override string ToString()
        {
            return String.Join("\n\n", Blocks);
        }

        public double GetBalance(String address)
        {
            double balance = 0.0;
            foreach (Block b in Blocks)
            {
                foreach (Transaction t in b.transactionList)
                {
                    if (t.recipientAddress.Equals(address))
                    {
                        balance += t.amount;
                    }
                    if (t.senderAddress.Equals(address))
                    {
                        balance -= (t.amount + t.fee);
                    }
                }
            }
            return balance;
        }

        public static bool ValidateMerkleRoot(Block b)
        {
            String reMerkle = Block.MerkleRoot(b.transactionList);
            return reMerkle.Equals(b.merkleRoot);
        }

        public static bool ValidateHash(Block b)
        {
            String reHash = b.CreateHash();
            return reHash.Equals(b.hash);
        }

        // ====================================================================
        // Task 6.1 — Benchmarking multi-threaded mining
        // ====================================================================
        // Runs the threading vs single-thread comparison automatically:
        // for each (difficulty, threadCount) combination, mine `trials` times and report the median time.
        // Uses standalone hashing (does NOT add anything to the chain) so it's safe to run repeatedly.
        // The progressCallback (optional) is invoked between trials so the UI can show a status indicator.
        // ====================================================================

        public delegate void BenchmarkProgressCallback(string message);

        public static String RunBenchmark(BenchmarkProgressCallback progressCallback = null)
        {
            // Defaults chosen so the whole sweep finishes in roughly a minute on a typical laptop:
            //   DD=4  is fast enough that thread overhead may dominate
            //   DD=5  is where multi-threading typically starts to pay off
            //   DD=6  shows the scaling clearly but adds the most time
            int[] difficulties = new int[] { 4, 5, 6 };
            int[] threadCounts = new int[] { 1, 2, 4, 8 };
            int trials = 3;

            // results[dd][tc] = list of elapsed times in ms
            Dictionary<int, Dictionary<int, List<long>>> results = new Dictionary<int, Dictionary<int, List<long>>>();

            int totalRuns = difficulties.Length * threadCounts.Length * trials;
            int currentRun = 0;

            foreach (int dd in difficulties)
            {
                results[dd] = new Dictionary<int, List<long>>();
                foreach (int tc in threadCounts)
                {
                    results[dd][tc] = new List<long>();
                    for (int trial = 0; trial < trials; trial++)
                    {
                        currentRun++;
                        if (progressCallback != null)
                        {
                            progressCallback(String.Format("[{0}/{1}] DD={2}, threads={3}, trial={4}",
                                currentRun, totalRuns, dd, tc, trial + 1));
                        }
                        long elapsed = BenchmarkSingleMine(dd, tc);
                        results[dd][tc].Add(elapsed);
                    }
                }
            }

            // Build the report-ready summary table
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("============================================================");
            sb.AppendLine("   TASK 6.1 — MULTI-THREADED MINING BENCHMARK");
            sb.AppendLine("============================================================");
            sb.AppendLine("Each cell shows the MEDIAN of " + trials + " trials, in milliseconds.");
            sb.AppendLine("Lower = faster. Compare across rows to see the threading benefit at each difficulty.");
            sb.AppendLine();

            // Header row
            sb.Append("  Difficulty");
            foreach (int tc in threadCounts)
                sb.Append(String.Format(" |{0,7} thread{1}", tc, tc == 1 ? " " : "s"));
            sb.AppendLine(" |");
            sb.AppendLine("  ----------" + String.Concat(threadCounts.Select(_ => "-+--------------")) + "-+");

            // Body rows
            foreach (int dd in difficulties)
            {
                sb.Append(String.Format("  {0,10}", dd));
                foreach (int tc in threadCounts)
                {
                    long med = Median(results[dd][tc]);
                    sb.Append(String.Format(" |{0,11} ms ", med));
                }
                sb.AppendLine(" |");
            }

            // Speedup analysis (relative to single-threaded baseline at the same difficulty)
            sb.AppendLine();
            sb.AppendLine("  SPEEDUP vs single-threaded (higher is better, 1.00x = no improvement):");
            sb.Append("  Difficulty");
            foreach (int tc in threadCounts)
                sb.Append(String.Format(" |{0,7} thread{1}", tc, tc == 1 ? " " : "s"));
            sb.AppendLine(" |");
            sb.AppendLine("  ----------" + String.Concat(threadCounts.Select(_ => "-+--------------")) + "-+");
            foreach (int dd in difficulties)
            {
                sb.Append(String.Format("  {0,10}", dd));
                long baseline = Median(results[dd][threadCounts[0]]);  // 1-thread time at this difficulty
                foreach (int tc in threadCounts)
                {
                    long med = Median(results[dd][tc]);
                    double speedup = med > 0 ? (double)baseline / med : 1.0;
                    sb.Append(String.Format(" |{0,11:F2}x  ", speedup));
                }
                sb.AppendLine(" |");
            }

            // Raw data dump for the appendix
            sb.AppendLine();
            sb.AppendLine("  RAW DATA (all " + trials + " trials per cell, ms):");
            foreach (int dd in difficulties)
            {
                sb.AppendLine();
                sb.AppendLine("  Difficulty " + dd + ":");
                foreach (int tc in threadCounts)
                {
                    sb.AppendLine("    " + tc + " thread" + (tc == 1 ? " " : "s") + ": [" +
                        String.Join(", ", results[dd][tc].Select(x => x.ToString())) + "]");
                }
            }

            return sb.ToString();
        }

        // Standalone, chain-free mining used by the benchmark above.
        // Mirrors the threading and e-nonce logic of Block.MineThreaded but operates on synthetic data
        // so the benchmark does not pollute the real blockchain with throwaway test blocks.
        private static long BenchmarkSingleMine(int difficulty, int threadCount)
        {
            // Distinct synthetic input per call so caches don't bias the measurement
            String fakeBlockData = "BENCHMARK|" + DateTime.Now.Ticks + "|prev:" + Guid.NewGuid().ToString() + "|";
            String target = new String('0', difficulty);

            Stopwatch sw = Stopwatch.StartNew();
            CancellationTokenSource cts = new CancellationTokenSource();
            bool found = false;
            object lockObj = new object();

            Task[] tasks = new Task[threadCount];
            for (int t = 0; t < threadCount; t++)
            {
                long myEnonce = t;
                CancellationToken token = cts.Token;
                tasks[t] = Task.Run(() =>
                {
                    SHA256 hasher = SHA256Managed.Create();
                    long localNonce = 0;
                    while (!token.IsCancellationRequested)
                    {
                        String input = fakeBlockData + "n=" + localNonce + ",en=" + myEnonce;
                        Byte[] bytes = hasher.ComputeHash(Encoding.UTF8.GetBytes(input));
                        String h = String.Empty;
                        foreach (byte b in bytes) h += String.Format("{0:x2}", b);

                        if (h.StartsWith(target))
                        {
                            lock (lockObj) { if (!found) { found = true; cts.Cancel(); } }
                            return;
                        }
                        localNonce++;
                    }
                }, token);
            }

            try { Task.WaitAll(tasks); }
            catch (AggregateException) { /* expected on cancellation */ }

            sw.Stop();
            return sw.ElapsedMilliseconds;
        }

        private static long Median(List<long> values)
        {
            List<long> sorted = values.OrderBy(x => x).ToList();
            int n = sorted.Count;
            if (n == 0) return 0;
            if (n % 2 == 1) return sorted[n / 2];
            return (sorted[(n / 2) - 1] + sorted[n / 2]) / 2;
        }
    }
}
