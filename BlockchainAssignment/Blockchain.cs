using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            // Output every block — used by the "Read All Blocks" button on the UI
            return String.Join("\n\n", Blocks);
        }

        public double GetBalance(String address)
        {
            // Walk every confirmed transaction in every block; debit sender, credit recipient
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

        // Validation helper: re-compute the block's Merkle root from its transactions and compare
        public static bool ValidateMerkleRoot(Block b)
        {
            String reMerkle = Block.MerkleRoot(b.transactionList);
            return reMerkle.Equals(b.merkleRoot);
        }

        // FIX (bug #5 helper): re-hash the block with its stored fields and compare to its stored hash
        // If a malicious actor altered any field after mining, the recomputed hash will not match.
        public static bool ValidateHash(Block b)
        {
            // We need to call CreateHash on the block instance. Since CreateHash uses the *current* nonce
            // and the stored merkleRoot, this checks both PoW integrity AND that fields haven't been tampered.
            String reHash = b.CreateHash();
            return reHash.Equals(b.hash);
        }
    }
}
