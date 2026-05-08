using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlockchainAssignment
{
    public partial class BlockchainApp : Form
    {
        Blockchain blockchain;

        public BlockchainApp()
        {
            InitializeComponent();
            blockchain = new Blockchain();
            richTextBox1.Text = "New Blockchain Initialised!";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        // ===================== PRINTING =====================

        // Print BlockN (based on user input)
        private void button1_Click(object sender, EventArgs e)
        {
            int index;
            if (Int32.TryParse(textBox1.Text, out index))
            {
                richTextBox1.Text = blockchain.GetBlockAsString(index);
            }
            else
            {
                richTextBox1.Text = "Invalid block number.";
            }
        }

        // NEW: Read All Blocks — prints the entire chain
        private void readAll_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = blockchain.ToString();
        }

        // NEW: Print Pending Transactions — prints the transaction pool contents
        private void printPending_Click(object sender, EventArgs e)
        {
            if (blockchain.transactionPool.Count == 0)
            {
                richTextBox1.Text = "Transaction pool is empty.";
            }
            else
            {
                richTextBox1.Text = "Pending Transactions (" + blockchain.transactionPool.Count + "):\n\n"
                    + String.Join("\n---\n", blockchain.transactionPool);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        // ===================== WALLETS =====================

        private void button2_Click(object sender, EventArgs e)
        {
            // Generate Wallet
            String privKey;
            Wallet.Wallet myNewWallet = new Wallet.Wallet(out privKey);
            publicKey.Text = myNewWallet.publicID;
            textBox3.Text = privKey;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Validate Keys
            if (Wallet.Wallet.ValidatePrivateKey(textBox3.Text, publicKey.Text))
            {
                richTextBox1.Text = "Keys are valid!";
            }
            else
            {
                richTextBox1.Text = "Keys are invalid!";
            }
        }

        // ===================== TRANSACTIONS =====================

        private void createTransaction_Click(object sender, EventArgs e)
        {
            // Basic input validation
            double amt, f;
            if (!Double.TryParse(amount.Text, out amt) || !Double.TryParse(fee.Text, out f))
            {
                richTextBox1.Text = "Invalid amount or fee. Please enter numeric values.";
                return;
            }
            if (String.IsNullOrWhiteSpace(publicKey.Text) || String.IsNullOrWhiteSpace(textBox3.Text)
                || String.IsNullOrWhiteSpace(recieverKey.Text))
            {
                richTextBox1.Text = "Please ensure public key, private key, and receiver key are all filled in.";
                return;
            }

            // FIX (missing feature): balance check before allowing the transaction.
            // Prevents double-spend / overspend; the spec calls for this in Part 5.
            double senderBalance = blockchain.GetBalance(publicKey.Text);
            if (senderBalance < amt + f)
            {
                richTextBox1.Text = "Transaction rejected: insufficient funds.\n"
                    + "Sender balance: " + senderBalance + " RadCoin\n"
                    + "Required (amount + fee): " + (amt + f) + " RadCoin";
                return;
            }

            Transaction newTransaction = new Transaction(publicKey.Text, recieverKey.Text, amt, f, textBox3.Text);
            blockchain.transactionPool.Add(newTransaction);
            richTextBox1.Text = "Transaction created and added to pool:\n\n" + newTransaction.ToString();
        }

        // ===================== BLOCKS =====================

        private void newBlock_Click(object sender, EventArgs e)
        {
            // Pull pending transactions, mine a new block referencing the previous, append to chain
            List<Transaction> transactions = blockchain.getPendingTransactions();
            Block newBlock = new Block(blockchain.GetLastBlock(), transactions, publicKey.Text);
            blockchain.Blocks.Add(newBlock);

            richTextBox1.Text = "New block mined:\n\n" + newBlock.ToString();
        }

        // ===================== VALIDATION =====================

        private void validateChain_Click(object sender, EventArgs e)
        {
            // CASE: only the genesis block exists — validate its hash and Merkle root only
            if (blockchain.Blocks.Count == 1)
            {
                Block g = blockchain.Blocks[0];
                if (!Blockchain.ValidateHash(g) || !Blockchain.ValidateMerkleRoot(g))
                {
                    richTextBox1.Text = "Blockchain is INVALID — genesis block tampered with.";
                }
                else
                {
                    richTextBox1.Text = "Blockchain is valid (genesis only).";
                }
                return;
            }

            // FIX (bug #4): walk every block from index 1 to Count-1 inclusive.
            // The previous loop used `i < Count - 1` which silently skipped the most recent block.
            for (int i = 1; i < blockchain.Blocks.Count; i++)
            {
                Block current = blockchain.Blocks[i];
                Block prev = blockchain.Blocks[i - 1];

                // 1. Hash chain coherence — current.prevHash must equal previous block's stored hash
                if (current.prevHash != prev.hash)
                {
                    richTextBox1.Text = "Blockchain is INVALID — broken hash link at block " + i;
                    return;
                }
                // 2. Block hash integrity — re-hash the block and compare (FIX bug #5)
                if (!Blockchain.ValidateHash(current))
                {
                    richTextBox1.Text = "Blockchain is INVALID — hash mismatch at block " + i;
                    return;
                }
                // 3. Merkle root integrity — re-compute the Merkle root from transactions and compare
                if (!Blockchain.ValidateMerkleRoot(current))
                {
                    richTextBox1.Text = "Blockchain is INVALID — Merkle root mismatch at block " + i;
                    return;
                }
            }

            richTextBox1.Text = "Blockchain is valid! All " + blockchain.Blocks.Count + " blocks pass integrity checks.";
        }

        private void checkBalance_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(publicKey.Text))
            {
                richTextBox1.Text = "Please enter or generate a public key first.";
                return;
            }
            richTextBox1.Text = "Address: " + publicKey.Text
                + "\nBalance: " + blockchain.GetBalance(publicKey.Text).ToString() + " RadCoin";
        }
    }
}
