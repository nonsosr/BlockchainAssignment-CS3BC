using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BlockchainAssignment
{
    class Transaction
    {
        public DateTime timestamp;       // public so the "Altruistic" mining selection (Task 6.3) can sort by age
        public String senderAddress, recipientAddress, hash, signature;
        public Double amount, fee;

        public Transaction(String from, String to, Double amount, Double fee, String privateKey)
        {
            this.timestamp = DateTime.Now;
            this.senderAddress = from;
            this.recipientAddress = to;
            this.amount = amount;
            this.fee = fee;
            this.hash = CreateHash();
            this.signature = Wallet.Wallet.CreateSignature(from, privateKey, this.hash);
        }

        public String CreateHash()
        {
            String hashStr = String.Empty;

            SHA256 hasher = SHA256Managed.Create();
            String input = timestamp.ToString() + senderAddress + recipientAddress + amount.ToString() + fee.ToString();

            Byte[] hashByte = hasher.ComputeHash(Encoding.UTF8.GetBytes(input));

            foreach (byte x in hashByte)
                hashStr += String.Format("{0:x2}", x);

            return hashStr;
        }

        public override string ToString()
        {
            return "Timestamp: " + timestamp.ToString() +
                "\nSender Address: " + senderAddress +
                "\nRecipient Address: " + recipientAddress +
                "\nAmount: " + amount.ToString() + " RadCoin" +
                "\nFee: " + fee.ToString() + " RadCoin" +
                "\nHash: " + hash +
                "\nSignature: " + signature;
        }
    }
}
