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

        // Printing

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

        private void readAll_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = blockchain.ToString();
        }

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

        // Wallets

        private void button2_Click(object sender, EventArgs e)
        {
            String privKey;
            Wallet.Wallet myNewWallet = new Wallet.Wallet(out privKey);
            publicKey.Text = myNewWallet.publicID;
            textBox3.Text = privKey;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (Wallet.Wallet.ValidatePrivateKey(textBox3.Text, publicKey.Text))
            {
                richTextBox1.Text = "Keys are valid!";
            }
            else
            {
                richTextBox1.Text = "Keys are invalid!";
            }
        }

        // Transaction creation with validation of inputs and balance check before adding to pool

        private void createTransaction_Click(object sender, EventArgs e)
        {
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

        // Task 6.1 threading

        // async void event handlers are the standard WinForms pattern for using await in Click handlers
        private async void newBlock_Click(object sender, EventArgs e)
        {
            // Read mining settings from the UI on the UI thread before launching the background task
            int diffNum = (int)difficultyInput.Value;
            int threadNum = (int)threadCountInput.Value;
            String minerAddr = publicKey.Text;
            List<Transaction> transactions = blockchain.getPendingTransactions();
            Block lastBlock = blockchain.GetLastBlock();

            // Disable the buttons while mining is in progress so users can't trigger overlapping mines
            SetMiningButtonsEnabled(false);
            richTextBox1.Text = "Mining new block (threads: " + threadNum + ", difficulty: " + diffNum + ") — please wait...";

            try
            {
                // Run the actual mining on a background thread so the UI stays responsive
                Block newBlock = await Task.Run(() =>
                    new Block(lastBlock, transactions, minerAddr, diffNum, threadNum));

                blockchain.Blocks.Add(newBlock);
                richTextBox1.Text = "New block mined in " + newBlock.miningTimeMs + " ms"
                    + " (threads: " + threadNum + ", difficulty: " + diffNum + ")\n\n"
                    + newBlock.ToString();
            }
            catch (Exception ex)
            {
                richTextBox1.Text = "Mining failed: " + ex.Message;
            }
            finally
            {
                SetMiningButtonsEnabled(true);
            }
        }

        // TASK 6.1 — BENCHMARK

        private async void runBenchmark_Click(object sender, EventArgs e)
        {
            SetMiningButtonsEnabled(false);
            runBenchmark.Text = "Running...";
            richTextBox1.Text = "Running benchmark — this typically takes ~1 minute.\n\nProgress will appear below as each combination completes.";

            try
            {
                String result = await Task.Run(() => Blockchain.RunBenchmark((progress) =>
                {
                    // Marshal the progress update onto the UI thread; rich text box mutation must happen on UI thread
                    if (this.IsHandleCreated)
                    {
                        this.BeginInvoke(new Action(() =>
                        {
                            runBenchmark.Text = "Running... " + progress;
                        }));
                    }
                }));

                richTextBox1.Text = result;
            }
            catch (Exception ex)
            {
                richTextBox1.Text = "Benchmark failed: " + ex.Message;
            }
            finally
            {
                runBenchmark.Text = "Run Benchmark";
                SetMiningButtonsEnabled(true);
            }
        }

        // Helper to enable/disable mining-related buttons together so we can't trigger overlapping work
        private void SetMiningButtonsEnabled(bool enabled)
        {
            newBlock.Enabled = enabled;
            runBenchmark.Enabled = enabled;
            createTransaction.Enabled = enabled;
        }

        // Validation

        private void validateChain_Click(object sender, EventArgs e)
        {
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

            for (int i = 1; i < blockchain.Blocks.Count; i++)
            {
                Block current = blockchain.Blocks[i];
                Block prev = blockchain.Blocks[i - 1];

                if (current.prevHash != prev.hash)
                {
                    richTextBox1.Text = "Blockchain is INVALID — broken hash link at block " + i;
                    return;
                }
                if (!Blockchain.ValidateHash(current))
                {
                    richTextBox1.Text = "Blockchain is INVALID — hash mismatch at block " + i;
                    return;
                }
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
