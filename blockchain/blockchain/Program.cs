using System;
using System.Collections.Generic;
using NBitcoin;
using NBitcoin.Crypto;
using NBitcoin.DataEncoders;

public class Block
{
    public int Index { get; set; }
    public DateTime Timestamp { get; set; }
    public string Data { get; set; }
    public string PreviousHash { get; set; }
    public string Hash { get; set; }
    public int Nonce { get; set; }

    public Block(int index, DateTime timestamp, string data, string previousHash)
    {
        Index = index;
        Timestamp = timestamp;
        Data = data;
        PreviousHash = previousHash;
        Hash = CalculateHash();
    }

    // This method calculates hash using NBitcoin's DoubleSHA256 function
    public string CalculateHash()
    {
        string hashInput = Index + Timestamp.ToString() + Data + PreviousHash + Nonce;
        return Hashes.DoubleSHA256(Encoders.ASCII.DecodeData(hashInput)).ToString();
    }
}

public class Blockchain
{
    public List<Block> Chain { get; private set; }
    public int Difficulty { get; set; } = 4; // Adjust difficulty as needed

    public Blockchain()
    {
        Chain = new List<Block> { CreateGenesisBlock() };
    }

    private Block CreateGenesisBlock()
    {
        return new Block(0, DateTime.Now, "Genesis Block", "0");
    }

    public Block GetLatestBlock()
    {
        return Chain[^1]; // Get the last block
    }

    public void AddBlock(Block newBlock)
    {
        newBlock.PreviousHash = GetLatestBlock().Hash;
        MineBlock(newBlock);
        Chain.Add(newBlock);
    }

    public void MineBlock(Block block)
    {
        string target = new string('0', Difficulty); // Target hash pattern
        while (!block.Hash.StartsWith(target))
        {
            block.Nonce++;
            block.Hash = block.CalculateHash();
        }
        Console.WriteLine($"Block mined: {block.Hash}");
    }

    public void DisplayBlockchain()
    {
        Console.WriteLine("\n--- Blockchain ---");
        foreach (var block in Chain)
        {
            Console.WriteLine($"Index: {block.Index}");
            Console.WriteLine($"Timestamp: {block.Timestamp}");
            Console.WriteLine($"Data: {block.Data}");
            Console.WriteLine($"PreviousHash: {block.PreviousHash}");
            Console.WriteLine($"Hash: {block.Hash}");
            Console.WriteLine($"Nonce: {block.Nonce}\n");
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Create the blockchain
        Blockchain blockchain = new Blockchain();

        // Mine and add blocks
        Console.WriteLine("Mining block 1...");
        blockchain.AddBlock(new Block(1, DateTime.Now, "This is block 1", blockchain.GetLatestBlock().Hash));

        Console.WriteLine("Mining block 2...");
        blockchain.AddBlock(new Block(2, DateTime.Now, "This is block 2", blockchain.GetLatestBlock().Hash));

        Console.WriteLine("Mining block 3...");
        blockchain.AddBlock(new Block(3, DateTime.Now, "This is block 3", blockchain.GetLatestBlock().Hash));

        // Display the blockchain
        blockchain.DisplayBlockchain();
    }
}
