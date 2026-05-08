using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BlockchainAssignment
{
    internal class Block
    {
        public int index;
        public DateTime timestamp;          // made public so Blockchain.cs can compute block times for adaptive difficulty (Task 6.2)
        public String hash;
        public String prevHash;

        public List<Transaction> transactionList = new List<Transaction>();
        public String merkleRoot = String.Empty;   // FIX (bug #3): explicit init so validateMerkleRoot doesn't compare against null

        // Proof of Work variables
        public long nonce = 0;
        public int difficulty = 4;

        // Reward and fee for mining a block
        public double reward = 1.0; // Fixed reward for mining a block
        public double fees = 0.0;

        public String minerAddress = String.Empty; // Address of the miner who mined this block

        /* Genesis Block Constructor */
        public Block()
        {
            this.timestamp = DateTime.Now;
            this.index = 0;
            this.prevHash = String.Empty;
            this.reward = 0;
            this.transactionList = new List<Transaction>();
            this.merkleRoot = String.Empty;     // explicit empty Merkle root for the genesis block (no transactions)
            this.hash = Mine();
        }

        /* Standard Block constructor — reference to previous block, list of pending transactions, miner address */
        public Block(Block lastBlock, List<Transaction> transactions, String address = "", int difficulty = 4)
        {
            this.timestamp = DateTime.Now;
            this.index = lastBlock.index + 1;
            this.prevHash = lastBlock.hash;
            this.minerAddress = address;
            this.difficulty = difficulty;       // accept caller-provided difficulty so adaptive difficulty (Task 6.2) can drive it

            transactions.Add(CreateRewardTransaction(transactions));   // reward + accumulated fees as a coinbase transaction
            this.transactionList = new List<Transaction>(transactions);

            this.merkleRoot = MerkleRoot(this.transactionList);        // FIX (bug #2): compute Merkle root BEFORE Mine() so the hash actually depends on it
            this.hash = Mine();
        }

        public Transaction CreateRewardTransaction(List<Transaction> transactions)
        {
            // Sum the fees in the list of transactions to calculate the total fees for this block
            fees = transactions.Aggregate(0.0, (acc, t) => acc + t.fee);
            // Coinbase: sender "Mine Rewards" handled specially by Wallet.cs (no private key required)
            return new Transaction("Mine Rewards", minerAddress, (reward + fees), 0, "");
        }

        public String CreateHash()
        {
            String hashStr = String.Empty;
            SHA256 hasher = SHA256Managed.Create();

            // Concatenate block properties so a change to any of them produces a different hash; nonce drives the PoW search
            String input = index.ToString() + timestamp.ToString() + prevHash + nonce.ToString() + reward.ToString() + (merkleRoot ?? String.Empty);
            Byte[] hashByte = hasher.ComputeHash(Encoding.UTF8.GetBytes(input));
            foreach (byte x in hashByte)
            {
                hashStr += String.Format("{0:x2}", x);
            }

            return hashStr;
        }

        public String Mine()
        {
            nonce = 0;                                     // reset so re-mining the same block (e.g. validation re-hash) is deterministic
            String hashStr = CreateHash();
            String target = new String('0', difficulty);   // PoW target: difficulty leading hex zeros
            while (!hashStr.StartsWith(target))
            {
                nonce++;
                hashStr = CreateHash();
            }
            return hashStr;
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
                return HashCode.HashTools.CombineHash(hashes[0], hashes[0]);   // FIX (bug #1): was hashes[1] which doesn't exist when count == 1
            }
            while (hashes.Count != 1)
            {
                List<String> merkleLeaves = new List<String>();
                for (int i = 0; i < hashes.Count; i += 2)
                {
                    if (i == hashes.Count - 1)
                    {
                        // Odd leaf at the end: combine with itself to keep the tree binary
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
                + "\nDifficulty: " + difficulty.ToString()
                + "\nMerkle Root: " + merkleRoot
                + "\nReward: " + reward.ToString() + " RadCoin"
                + "\nFees: " + fees.ToString() + " RadCoin"
                + "\nMiner Address: " + minerAddress
                + "\nTransactions:\n" + String.Join("\n---\n", transactionList);
        }
    }
}
