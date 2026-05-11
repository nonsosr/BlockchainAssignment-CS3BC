# RadCoin — Mini Blockchain (CS3BC Coursework)

A C# / Windows Forms implementation of an offline blockchain, built for my **CS3BC Blockchain and Security** module at the University of Reading (2025/26).

This is an academic implementation for coursework. The cryptocurrency name "RadCoin" is for project use only and has no connection to any existing token of a similar name.

## Features

### Core blockchain (Parts 1–5)
- Cryptographically-linked blocks with SHA-256 hashing
- Genesis block creation
- ECDSA wallet key pairs (public + private) and digital signatures
- Transactions with sender/recipient/amount/fee, signed by the sender's private key
- Transaction pool for pending transactions
- Proof-of-Work consensus algorithm with adjustable difficulty (default 4 leading zeros)
- Mining rewards (1 RadCoin) plus accumulated transaction fees as a coinbase transaction
- Merkle root computation over each block's transactions
- Full chain validation: hash-chain coherence, per-block hash integrity, Merkle-root integrity, transaction signature verification
- Wallet balance check and double-spend prevention (balance is verified before a transaction is admitted to the pool)

### Bug fixes applied to the starter code
- **Single-transaction Merkle root crash** — fixed an off-by-one that referenced `hashes[1]` when only one transaction existed (now correctly returns `hashes[0]`).
- **Standard block constructor ordering** — Merkle root is now computed *before* `Mine()` so the mined hash includes it.
- **Genesis block `merkleRoot`** — initialised to `String.Empty` instead of `null`, eliminating a NullReference during printing.
- **Chain validation off-by-one** — the loop ran `i < Count - 1` and silently skipped the last block; corrected to `i < Count` and extended to re-hash each block via `ValidateHash` so post-mining tampering is caught.

### Assignment extensions (Part 6)

**Task 6.1 — Multi-threaded mining**
- N parallel mining threads, each with its own `eNonce` field so no two threads ever try the same `(nonce, eNonce)` input — true work-splitting, not redundant search.
- `CancellationTokenSource` signals all threads to stop the moment one wins; each thread holds its own `SHA256` instance because that class is not thread-safe.
- `MineThreaded(int threadCount)` and a `RunBenchmark()` routine that sweeps difficulty `D ∈ {4, 5, 6}` × thread count `T ∈ {1, 2, 4, 8}` with three trials per cell, reporting median wall-clock time (`System.Diagnostics.Stopwatch`) and speed-up factor `T₁/T_T`.
- Mining runs on a background `Task` so the UI stays responsive.

**Task 6.2 — Adaptive difficulty**
- `difficulty` is a `double` rather than `int`, so a single +1 step no longer multiplies expected work by 16. Fractional difficulty is enforced by `MeetsDifficulty(hash, D)`: the first `⌊D⌋` characters must be zero AND the next character must be below a `16^(1−frac(D))` threshold, giving smooth `16^D` expected work.
- Exponential moving average of block times: `EMA = 0.3 × actual + 0.7 × EMA_old`.
- Cube-root damped retarget: `D_new = D_old × (target / EMA)^(1/3)` to prevent oscillation.
- v2 outlier safeguards: per-block observation clamped to `[target/4, target × 4]` before entering the EMA, and per-step difficulty change capped at `±0.3`.
- Target block time = 5 seconds (short enough to demo convergence in a single sequence, long enough that OS noise does not dominate).
- "Mine Adaptive Sequence" button mines N consecutive blocks and prints a live convergence table (block, observed time, EMA, new difficulty).

**Task 6.3 — Mining preferences**
- `MiningPreference` enum: **Greedy** / **Altruistic** / **Random** / **AddressPreference**, selected from a UI dropdown.
- Greedy → `OrderByDescending(t => t.fee)` (maximise miner revenue).
- Altruistic → `OrderBy(t => t.timestamp)` (oldest first; prevents starvation).
- Random → Fisher-Yates shuffle (fee-pattern resistant).
- AddressPreference → partition by sender/recipient = miner address; miner's own transactions confirm first, remaining slots filled in arrival order.
- Both `New Block` and `Mine Adaptive Sequence` respect the currently-selected preference.
- "Seed Sample Transactions" button generates 20 transactions across multiple wallets, with timestamps spread over the previous 30 minutes, fees in `[0, 0.5]` and amounts in `[1, 10]` — gives each preference meaningfully different input. Signatures are properly recomputed after backdating so the seeded pool still validates.

### UI additions
- **Read All Blocks** — print the entire chain in one click.
- **Print Pending Tx** — list the current transaction pool.
- **Seed Sample Transactions** — populate the pool for Task 6.3 demos.
- **Run Benchmark** — execute the Task 6.1 sweep.
- **Mine Adaptive Sequence** — execute the Task 6.2 retarget run.
- **Resizable form** — the output text box is anchored on all four sides; group boxes and bottom controls anchor correctly so the form scales without controls overlapping. Useful for taking clean screenshots.

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

## How to use

A walk-through that exercises every feature:

1. **Generate wallets** — click *New Wallet* twice; copy the public keys into the sender/receiver fields.
2. **Create a few transactions** — fill in amount/fee and click *New Transaction* (insufficient-funds attempts are rejected here).
3. **Inspect the pool** — *Print Pending Tx*.
4. **Mine a block** — *New Block*. Observe the leading zeros on the resulting hash.
5. **Validate the chain** — *Validate Chain* should return `Blockchain is valid!`.
6. **Read the chain** — *Read All Blocks*.
7. **Task 6.1** — set Threads = 8, Difficulty = 5, click *New Block* and note the elapsed time; then click *Run Benchmark* for the full sweep.
8. **Task 6.2** — set target time = 5 s, sequence length = 10, click *Mine Adaptive Sequence*; the convergence table prints live.
9. **Task 6.3** — click *Seed Sample Transactions*, then mine the same pool four times (one per preference) to compare which transactions were selected.

## Project structure

```
BlockchainAssignment/
├── BlockchainAssignment.sln
└── BlockchainAssignment/
    ├── Block.cs                    -- Block class: fractional-difficulty hashing, Merkle root, single-thread + multi-thread mining, e-nonce
    ├── Blockchain.cs               -- Chain container, transaction pool, validation, mining preferences, adaptive controller, benchmark sweep
    ├── Transaction.cs              -- Transaction class with digital signature; timestamp made public for Task 6.3 sorting
    ├── BlockchainApp.cs            -- Windows Forms event handlers (async mining, preference dropdown, sample seeder)
    ├── BlockchainApp.Designer.cs   -- UI layout with anchoring for a resizable form
    ├── HashCode/HashTools.cs       -- SHA-256 utilities (provided)
    └── Wallet/Wallet.cs            -- ECDSA key pair + signature (provided)
```

## Author
**Chukwunonso Okpala**

Coursework submission for CS3BC, 2025/26.
