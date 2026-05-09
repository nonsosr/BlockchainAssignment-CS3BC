using System;
using System.Collections.Generic;
using System.Diagnostics;        // Stopwatch — Task 6.1 timing
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;          // CancellationTokenSource — Task 6.1
using System.Threading.Tasks;

namespace BlockchainAssignment
{
    internal class Block
    {
        public int index;
        public DateTime timestamp;
        public String hash;
        public String prevHash;

        public List<Transaction> transactionList = new List<Transaction>();
        public String merkleRoot = String.Empty;

        // Proof of Work variables
        public long nonce = 0;
        public long eNonce = 0;             // TASK 6.1: extra nonce, distinct per mining thread, prevents duplicated work
        public int difficulty = 4;
        public long miningTimeMs = 0;       // TASK 6.1: time taken to mine this block (ms) — for benchmarking and report screenshots

        // Reward and fee for mining a block
        public double reward = 1.0;
        public double fees = 0.0;

        public String minerAddress = String.Empty;

        /* Genesis Block Constructor */
        public Block()
        {
            this.timestamp = DateTime.Now;
            this.index = 0;
            this.prevHash = String.Empty;
            this.reward = 0;
            this.transactionList = new List<Transaction>();
            this.merkleRoot = String.Empty;
            this.hash = Mine();
        }

        /* Standard Block constructor — accepts threadCount so caller can drive single- or multi-threaded mining (Task 6.1) */
        public Block(Block lastBlock, List<Transaction> transactions, String address = "", int difficulty = 4, int threadCount = 1)
        {
            this.timestamp = DateTime.Now;
            this.index = lastBlock.index + 1;
            this.prevHash = lastBlock.hash;
            this.minerAddress = address;
            this.difficulty = difficulty;

            transactions.Add(CreateRewardTransaction(transactions));
            this.transactionList = new List<Transaction>(transactions);
            this.merkleRoot = MerkleRoot(this.transactionList);

            if (threadCount <= 1)
                this.hash = Mine();
            else
                this.hash = MineThreaded(threadCount);
        }

        public Transaction CreateRewardTransaction(List<Transaction> transactions)
        {
            fees = transactions.Aggregate(0.0, (acc, t) => acc + t.fee);
            return new Transaction("Mine Rewards", minerAddress, (reward + fees), 0, "");
        }

        // Standard hash composition: includes both nonce AND eNonce so the threaded miner's eNonce affects the hash
        public String CreateHash()
        {
            String hashStr = String.Empty;
            SHA256 hasher = SHA256Managed.Create();
            String input = index.ToString() + timestamp.ToString() + prevHash
                + nonce.ToString() + eNonce.ToString() + reward.ToString() + (merkleRoot ?? String.Empty);
            Byte[] hashByte = hasher.ComputeHash(Encoding.UTF8.GetBytes(input));
            foreach (byte x in hashByte)
            {
                hashStr += String.Format("{0:x2}", x);
            }
            return hashStr;
        }

        // TASK 6.1: parameter-based hash function used by MineThreaded.
        // Threads call this with their own (n, en) values without mutating shared block state — so it's thread-safe.
        // Each thread is given its own SHA256 instance because SHA256 instances are not thread-safe.
        private String HashWithParams(SHA256 hasher, long n, long en)
        {
            String input = index.ToString() + timestamp.ToString() + prevHash
                + n.ToString() + en.ToString() + reward.ToString() + (merkleRoot ?? String.Empty);
            Byte[] hashByte = hasher.ComputeHash(Encoding.UTF8.GetBytes(input));
            String hashStr = String.Empty;
            foreach (byte x in hashByte)
            {
                hashStr += String.Format("{0:x2}", x);
            }
            return hashStr;
        }

        /* Single-threaded mining (Part 4 — original behaviour, now timed) */
        public String Mine()
        {
            Stopwatch sw = Stopwatch.StartNew();
            nonce = 0;
            eNonce = 0;
            String hashStr = CreateHash();
            String target = new String('0', difficulty);
            while (!hashStr.StartsWith(target))
            {
                nonce++;
                hashStr = CreateHash();
            }
            sw.Stop();
            miningTimeMs = sw.ElapsedMilliseconds;
            return hashStr;
        }

        /* Task 6.1: Multi-threaded mining.
         * Each thread t gets a distinct e-nonce (= t) and iterates its own private nonce starting at 0.
         * Because the (nonce, eNonce) pair is part of the hash input, different threads NEVER hash the same data
         * even when their local nonce values coincide — this is the "duplicated work prevention" the spec requires.
         * The first thread to find a hash satisfying the difficulty target locks the result, signals cancellation,
         * and the remaining threads see the cancellation token and exit.
         */
        public String MineThreaded(int threadCount)
        {
            if (threadCount < 1) threadCount = 1;
            Stopwatch sw = Stopwatch.StartNew();
            CancellationTokenSource cts = new CancellationTokenSource();

            // Shared result fields, protected by the lock below
            String foundHash = null;
            long foundNonce = 0;
            long foundEnonce = 0;
            object lockObj = new object();

            String target = new String('0', difficulty);

            Task[] tasks = new Task[threadCount];
            for (int t = 0; t < threadCount; t++)
            {
                long myEnonce = t;                   // each thread's distinct e-nonce
                CancellationToken token = cts.Token;
                tasks[t] = Task.Run(() =>
                {
                    SHA256 hasher = SHA256Managed.Create();   // one hasher per thread (thread-local)
                    long localNonce = 0;
                    while (!token.IsCancellationRequested)
                    {
                        String h = HashWithParams(hasher, localNonce, myEnonce);
                        if (h.StartsWith(target))
                        {
                            // First-to-finish wins; lock prevents two threads claiming the result simultaneously
                            lock (lockObj)
                            {
                                if (foundHash == null)
                                {
                                    foundHash = h;
                                    foundNonce = localNonce;
                                    foundEnonce = myEnonce;
                                    cts.Cancel();    // tell the other threads to stop
                                }
                            }
                            return;
                        }
                        localNonce++;
                    }
                }, token);
            }

            try { Task.WaitAll(tasks); }
            catch (AggregateException) { /* expected — cancellation throws when threads are cancelled */ }

            sw.Stop();
            miningTimeMs = sw.ElapsedMilliseconds;

            // Persist the winning nonce/eNonce so subsequent calls to CreateHash
            this.nonce = foundNonce;
            this.eNonce = foundEnonce;
            return foundHash;
        }

        public static String MerkleRoot(List<Transaction> transactionList)
        {
            List<String> hashes = transactionList.Select(t => t.hash).ToList();
            if (hashes.Count == 0)
            {
                return String.Empty;
            }
            if (hashes.Count == 1)
            {
                return HashCode.HashTools.CombineHash(hashes[0], hashes[0]);
            }
            while (hashes.Count != 1)
            {
                List<String> merkleLeaves = new List<String>();
                for (int i = 0; i < hashes.Count; i += 2)
                {
                    if (i == hashes.Count - 1)
                    {
                        merkleLeaves.Add(HashCode.HashTools.CombineHash(hashes[i], hashes[i]));
                    }
                    else
                    {
                        merkleLeaves.Add(HashCode.HashTools.CombineHash(hashes[i], hashes[i + 1]));
                    }
                }
                hashes = merkleLeaves;
            }
            return hashes[0];
        }

        public override string ToString()
        {
            return "Index: " + index.ToString()
                + "\nTimestamp: " + timestamp.ToString()
                + "\nPrevious Hash: " + prevHash
                + "\nHash: " + hash
                + "\nNonce: " + nonce.ToString()
                + "\neNonce: " + eNonce.ToString()
                + "\nDifficulty: " + difficulty.ToString()
                + "\nMining time: " + miningTimeMs.ToString() + " ms"
                + "\nMerkle Root: " + merkleRoot
                + "\nReward: " + reward.ToString() + " RadCoin"
                + "\nFees: " + fees.ToString() + " RadCoin"
                + "\nMiner Address: " + minerAddress
                + "\nTransactions:\n" + String.Join("\n---\n", transactionList);
        }
    }
}
