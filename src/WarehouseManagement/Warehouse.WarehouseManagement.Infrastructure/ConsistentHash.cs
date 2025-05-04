using System.Security.Cryptography;
using System.Text;

namespace Warehouse.WarehouseManagement.Infrastructure;

public class ConsistentHash<TNode> where TNode : struct
{
    private readonly SortedDictionary<uint, TNode> _ring = new();
    private readonly int _virtualNodesPerNode;
    private readonly Func<string, uint> _hashFunction;

    public ConsistentHash(
        int virtualNodesPerNode = 3,
        Func<string, uint>? hashFunction = null)
    {
        _virtualNodesPerNode = virtualNodesPerNode;
        _hashFunction = hashFunction ?? DefaultHash;
    }
    
    public void AddNode(TNode node)
    {
        for (int i = 0; i < _virtualNodesPerNode; i++)
        {
            string virtualNodeKey = $"{node.GetHashCode()}#{i}";
            
            uint hash = _hashFunction(virtualNodeKey);
            
            _ring[hash] = node;
        }
    }
    
    public TNode? GetNode(string key)
    {
        if (_ring.Count == 0)
            return null;

        uint hash = _hashFunction(key);
        
        foreach (var kvp in _ring)
            if (kvp.Key >= hash)
                return kvp.Value;
        
        return _ring[_ring.Keys.Min()];
    }
    
    private static uint DefaultHash(string input)
    {
        using var sha256 = SHA256.Create();
        byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
        return BitConverter.ToUInt32(hashBytes, 0);
    }
}