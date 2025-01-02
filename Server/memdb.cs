using System;
using System.Collections.Generic;

namespace rediscsharp
{
    class MemDb
    {
        private Dictionary<string, string> _data;               // Simple key-value store
        private Dictionary<string, List<string>> _hashData;     // Hash store (key -> List of values)

        public MemDb()
        {
            _data = new Dictionary<string, string>();
            _hashData = new Dictionary<string, List<string>>();
        }

        // SET command for simple key-value pairs
        public void Set(string key, string value)
        {
            _data[key] = value;
            Console.WriteLine($"SET: {key} = {value}");
        }

        // HSET command for adding multiple values to a hash (key -> List of values)
        public void SetHash(string key, List<string> values)
        {
            if (!_hashData.ContainsKey(key))
            {
                _hashData[key] = new List<string>();
            }

            // Add all values to the list under the key
            _hashData[key].AddRange(values);
            Console.WriteLine($"HSET: {key} -> [{string.Join(", ", values)}]");
        }

        // GET command for simple key-value pairs
        public string Get(string key)
        {
            return _data.ContainsKey(key) ? _data[key] : null;
        }

        // HGET command for getting all values of a hash (key -> List of values)
        public List<string> GetHashValues(string key)
        {
            return _hashData.ContainsKey(key) ? _hashData[key] : null;
        }

        // Show all stored data (debugging)
        public void ShowData()
        {
            Console.WriteLine("Data Store:");
            foreach (var key in _data.Keys)
            {
                Console.WriteLine($"  {key}: {_data[key]}");
            }

            Console.WriteLine("Hash Store:");
            foreach (var key in _hashData.Keys)
            {
                Console.WriteLine($"  {key}: [{string.Join(", ", _hashData[key])}]");
            }
        }
    }
}

