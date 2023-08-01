using System.Collections.Generic;
using UnityEngine;

namespace Models
{
    [System.Serializable]
    public class SerializableDictionary<TKey, TValue> : ISerializationCallbackReceiver
    {
        [SerializeField] private List<TKey> keys = new List<TKey>();
        [SerializeField] private List<TValue> values = new List<TValue>();

        public Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();

        public void OnBeforeSerialize()
        {
            keys.Clear();
            values.Clear();

            foreach (var pair in dictionary)
            {
                keys.Add(pair.Key);
                values.Add(pair.Value);
            }
        }

        public void OnAfterDeserialize()
        {
            dictionary = new Dictionary<TKey, TValue>();
            for (int i = 0; i != Mathf.Min(keys.Count, values.Count); i++)
                dictionary.Add(keys[i], values[i]);
        }

        public void Add(TKey key, TValue value)
        {
            dictionary[key] = value;
        }
    }
}