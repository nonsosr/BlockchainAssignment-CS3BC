# RadCoin — Mini Blockchain (CS3BC Coursework)

A C# / Windows Forms implementation of an offline blockchain, built for my **CS3BC Blockchain and Security** module at the University of Reading (2025/26).

This is an academic implementation for coursework. The cryptocurrency name "RadCoin" is for project use only and has no connection to any existing token of a similar name.

## Features

**Core blockchain (Parts 1–5)**
- Cryptographically-linked blocks with SHA-256 hashing
- Genesis block creation
- ECDSA wallet key pairs (public + private) and digital signatures
- Transactions with sender/recipient/amount/fee, signed by the sender's private key
- Transaction pool for pending transactions
- Proof-of-Work consensus algorithm with adjustable difficulty (default 4 leading zeros)
- Mining rewards (1 RadCoin) plus accumulated transaction fees as a coinbase transaction
- Merkle root computation over each block's transactions
- Full chain validation: hash chain coherence, per-block hash integrity, Merkle root integrity
- Wallet balance check and double-spend prevention

**Assignment extensions (Part 6)**
- **Task 6.1 — Multi-threaded mining** with per-thread e-nonce to avoid duplicated work, Stopwatch benchmarking, and configurable thread count.
- **Task 6.2 — Adaptive difficulty** using exponential moving average of block times to converge on a target block time.
- **Task 6.3 — Mining preferences** (Greedy / Altruistic / Random / Address-Preference) for selecting transactions from the pool.

## Build & Run

**Requirements**
- Windows
- Visual Studio 2017 or later (Community edition is fine)
- .NET Framework 4.7.2

**Steps**
1. Clone this repository
2. Open `BlockchainAssignment/BlockchainAssignment.sln` in Visual Studio
3. Build the solution (`Ctrl+Shift+B`)
4. Run (`F5`)

## Project structure

```
BlockchainAssignment/
├── BlockchainAssignment.sln
└── BlockchainAssignment/
    ├── Block.cs              -- Block class: hashing, mining, Merkle root
    ├── Blockchain.cs         -- Chain container, transaction pool, validation helpers
    ├── Transaction.cs        -- Transaction class with digital signature
    ├── BlockchainApp.cs      -- Windows Forms event handlers
    ├── BlockchainApp.Designer.cs  -- UI layout
    ├── HashCode/HashTools.cs -- SHA-256 utilities (provided)
    └── Wallet/Wallet.cs      -- ECDSA key pair + signature (provided)
```

## Author
**Chukwunonso Okpala**

Coursework submission for CS3BC, 2025/26.