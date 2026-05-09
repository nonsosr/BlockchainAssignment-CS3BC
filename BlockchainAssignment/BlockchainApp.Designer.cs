namespace BlockchainAssignment
{
    partial class BlockchainApp
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.publicKey = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.newBlock = new System.Windows.Forms.Button();
            this.createTransaction = new System.Windows.Forms.Button();
            this.amount = new System.Windows.Forms.TextBox();
            this.fee = new System.Windows.Forms.TextBox();
            this.recieverKey = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.validateChain = new System.Windows.Forms.Button();
            this.checkBalance = new System.Windows.Forms.Button();
            this.readAll = new System.Windows.Forms.Button();
            this.printPending = new System.Windows.Forms.Button();
            // ===== TASK 6.1 controls =====
            this.miningGroupBox = new System.Windows.Forms.GroupBox();
            this.threadLabel = new System.Windows.Forms.Label();
            this.threadCountInput = new System.Windows.Forms.NumericUpDown();
            this.difficultyLabel = new System.Windows.Forms.Label();
            this.difficultyInput = new System.Windows.Forms.NumericUpDown();
            this.runBenchmark = new System.Windows.Forms.Button();
            this.miningHelp = new System.Windows.Forms.Label();

            this.miningGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.threadCountInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.difficultyInput)).BeginInit();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.SystemColors.InfoText;
            this.richTextBox1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.richTextBox1.Location = new System.Drawing.Point(16, 15);
            this.richTextBox1.Margin = new System.Windows.Forms.Padding(4);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(875, 386);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(16, 406);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(119, 25);
            this.button1.TabIndex = 1;
            this.button1.Text = "Print Block";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(143, 407);
            this.textBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(216, 22);
            this.textBox1.TabIndex = 2;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(784, 407);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(107, 57);
            this.button2.TabIndex = 3;
            this.button2.Text = "Generate Wallet";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // publicKey
            // 
            this.publicKey.Location = new System.Drawing.Point(500, 407);
            this.publicKey.Name = "publicKey";
            this.publicKey.Size = new System.Drawing.Size(278, 22);
            this.publicKey.TabIndex = 4;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(500, 435);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(278, 22);
            this.textBox3.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(419, 410);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 16);
            this.label1.TabIndex = 6;
            this.label1.Text = "Public Key";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(419, 438);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 16);
            this.label2.TabIndex = 7;
            this.label2.Text = "Private Key";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(767, 470);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(124, 33);
            this.button3.TabIndex = 8;
            this.button3.Text = "Validate Keys";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // newBlock
            // 
            this.newBlock.Location = new System.Drawing.Point(16, 470);
            this.newBlock.Name = "newBlock";
            this.newBlock.Size = new System.Drawing.Size(97, 53);
            this.newBlock.TabIndex = 9;
            this.newBlock.Text = "Generate New Block";
            this.newBlock.UseVisualStyleBackColor = true;
            this.newBlock.Click += new System.EventHandler(this.newBlock_Click);
            // 
            // createTransaction
            // 
            this.createTransaction.Location = new System.Drawing.Point(16, 535);
            this.createTransaction.Name = "createTransaction";
            this.createTransaction.Size = new System.Drawing.Size(97, 51);
            this.createTransaction.TabIndex = 10;
            this.createTransaction.Text = "Create Transaction";
            this.createTransaction.UseVisualStyleBackColor = true;
            this.createTransaction.Click += new System.EventHandler(this.createTransaction_Click);
            // 
            // amount
            // 
            this.amount.Location = new System.Drawing.Point(175, 535);
            this.amount.Name = "amount";
            this.amount.Size = new System.Drawing.Size(100, 22);
            this.amount.TabIndex = 11;
            // 
            // fee
            // 
            this.fee.Location = new System.Drawing.Point(175, 564);
            this.fee.Name = "fee";
            this.fee.Size = new System.Drawing.Size(100, 22);
            this.fee.TabIndex = 12;
            // 
            // recieverKey
            // 
            this.recieverKey.Location = new System.Drawing.Point(500, 564);
            this.recieverKey.Name = "recieverKey";
            this.recieverKey.Size = new System.Drawing.Size(278, 22);
            this.recieverKey.TabIndex = 13;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(117, 535);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 16);
            this.label3.TabIndex = 14;
            this.label3.Text = "Amount";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(138, 564);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 16);
            this.label4.TabIndex = 15;
            this.label4.Text = "Fee";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(406, 567);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(88, 16);
            this.label5.TabIndex = 16;
            this.label5.Text = "Reciever Key";
            // 
            // validateChain
            // 
            this.validateChain.Location = new System.Drawing.Point(767, 510);
            this.validateChain.Name = "validateChain";
            this.validateChain.Size = new System.Drawing.Size(123, 41);
            this.validateChain.TabIndex = 17;
            this.validateChain.Text = "Validate Chain";
            this.validateChain.UseVisualStyleBackColor = true;
            this.validateChain.Click += new System.EventHandler(this.validateChain_Click);
            // 
            // checkBalance
            // 
            this.checkBalance.Location = new System.Drawing.Point(630, 470);
            this.checkBalance.Name = "checkBalance";
            this.checkBalance.Size = new System.Drawing.Size(131, 33);
            this.checkBalance.TabIndex = 18;
            this.checkBalance.Text = "Check Balance";
            this.checkBalance.UseVisualStyleBackColor = true;
            this.checkBalance.Click += new System.EventHandler(this.checkBalance_Click);
            // 
            // readAll
            // 
            this.readAll.Location = new System.Drawing.Point(125, 470);
            this.readAll.Name = "readAll";
            this.readAll.Size = new System.Drawing.Size(110, 33);
            this.readAll.TabIndex = 19;
            this.readAll.Text = "Read All Blocks";
            this.readAll.UseVisualStyleBackColor = true;
            this.readAll.Click += new System.EventHandler(this.readAll_Click);
            // 
            // printPending
            // 
            this.printPending.Location = new System.Drawing.Point(247, 470);
            this.printPending.Name = "printPending";
            this.printPending.Size = new System.Drawing.Size(140, 33);
            this.printPending.TabIndex = 20;
            this.printPending.Text = "Print Pending Tx";
            this.printPending.UseVisualStyleBackColor = true;
            this.printPending.Click += new System.EventHandler(this.printPending_Click);
            // 
            // ============ TASK 6.1 — Mining Settings GroupBox ============
            // 
            // miningGroupBox
            // 
            this.miningGroupBox.Controls.Add(this.miningHelp);
            this.miningGroupBox.Controls.Add(this.runBenchmark);
            this.miningGroupBox.Controls.Add(this.difficultyInput);
            this.miningGroupBox.Controls.Add(this.difficultyLabel);
            this.miningGroupBox.Controls.Add(this.threadCountInput);
            this.miningGroupBox.Controls.Add(this.threadLabel);
            this.miningGroupBox.Location = new System.Drawing.Point(16, 600);
            this.miningGroupBox.Name = "miningGroupBox";
            this.miningGroupBox.Size = new System.Drawing.Size(875, 95);
            this.miningGroupBox.TabIndex = 21;
            this.miningGroupBox.TabStop = false;
            this.miningGroupBox.Text = "Task 6.1 — Threaded Mining Settings (applies to \"Generate New Block\")";
            // 
            // threadLabel
            // 
            this.threadLabel.AutoSize = true;
            this.threadLabel.Location = new System.Drawing.Point(15, 30);
            this.threadLabel.Name = "threadLabel";
            this.threadLabel.Size = new System.Drawing.Size(60, 16);
            this.threadLabel.TabIndex = 0;
            this.threadLabel.Text = "Threads:";
            // 
            // threadCountInput
            // 
            this.threadCountInput.Location = new System.Drawing.Point(80, 28);
            this.threadCountInput.Maximum = new decimal(new int[] { 16, 0, 0, 0 });
            this.threadCountInput.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.threadCountInput.Name = "threadCountInput";
            this.threadCountInput.Size = new System.Drawing.Size(60, 22);
            this.threadCountInput.TabIndex = 1;
            this.threadCountInput.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // difficultyLabel
            // 
            this.difficultyLabel.AutoSize = true;
            this.difficultyLabel.Location = new System.Drawing.Point(160, 30);
            this.difficultyLabel.Name = "difficultyLabel";
            this.difficultyLabel.Size = new System.Drawing.Size(64, 16);
            this.difficultyLabel.TabIndex = 2;
            this.difficultyLabel.Text = "Difficulty:";
            // 
            // difficultyInput
            // 
            this.difficultyInput.Location = new System.Drawing.Point(230, 28);
            this.difficultyInput.Maximum = new decimal(new int[] { 8, 0, 0, 0 });
            this.difficultyInput.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.difficultyInput.Name = "difficultyInput";
            this.difficultyInput.Size = new System.Drawing.Size(60, 22);
            this.difficultyInput.TabIndex = 3;
            this.difficultyInput.Value = new decimal(new int[] { 4, 0, 0, 0 });
            // 
            // runBenchmark
            // 
            this.runBenchmark.Location = new System.Drawing.Point(320, 23);
            this.runBenchmark.Name = "runBenchmark";
            this.runBenchmark.Size = new System.Drawing.Size(180, 33);
            this.runBenchmark.TabIndex = 4;
            this.runBenchmark.Text = "Run Benchmark";
            this.runBenchmark.UseVisualStyleBackColor = true;
            this.runBenchmark.Click += new System.EventHandler(this.runBenchmark_Click);
            // 
            // miningHelp
            // 
            this.miningHelp.AutoSize = true;
            this.miningHelp.Location = new System.Drawing.Point(15, 60);
            this.miningHelp.Name = "miningHelp";
            this.miningHelp.Size = new System.Drawing.Size(700, 16);
            this.miningHelp.TabIndex = 5;
            this.miningHelp.Text = "Threads = 1 → single-threaded.  Benchmark sweeps DD={4,5,6} × threads={1,2,4,8}, 3 trials each (~1 min).";
            // 
            // BlockchainApp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(919, 720);
            this.Controls.Add(this.miningGroupBox);
            this.Controls.Add(this.printPending);
            this.Controls.Add(this.readAll);
            this.Controls.Add(this.checkBalance);
            this.Controls.Add(this.validateChain);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.recieverKey);
            this.Controls.Add(this.fee);
            this.Controls.Add(this.amount);
            this.Controls.Add(this.createTransaction);
            this.Controls.Add(this.newBlock);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.publicKey);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.richTextBox1);
            this.ForeColor = System.Drawing.Color.Black;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "BlockchainApp";
            this.Text = "Blockchain App — RadCoin";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.miningGroupBox.ResumeLayout(false);
            this.miningGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.threadCountInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.difficultyInput)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox publicKey;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button newBlock;
        private System.Windows.Forms.Button createTransaction;
        private System.Windows.Forms.TextBox amount;
        private System.Windows.Forms.TextBox fee;
        private System.Windows.Forms.TextBox recieverKey;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button validateChain;
        private System.Windows.Forms.Button checkBalance;
        private System.Windows.Forms.Button readAll;
        private System.Windows.Forms.Button printPending;
        // Task 6.1 controls
        private System.Windows.Forms.GroupBox miningGroupBox;
        private System.Windows.Forms.Label threadLabel;
        private System.Windows.Forms.NumericUpDown threadCountInput;
        private System.Windows.Forms.Label difficultyLabel;
        private System.Windows.Forms.NumericUpDown difficultyInput;
        private System.Windows.Forms.Button runBenchmark;
        private System.Windows.Forms.Label miningHelp;
    }
}
