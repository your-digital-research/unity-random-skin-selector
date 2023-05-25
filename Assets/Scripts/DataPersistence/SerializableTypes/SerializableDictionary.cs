using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    #region SERIALIZED_VARIABLES

    [SerializeField] private List<TKey> keys = new List<TKey>();
    [SerializeField] private List<TValue> values = new List<TValue>();

    #endregion

    #region PUBLIC_FUNCTIONS

    // Save the dictionary to lists
    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();

        foreach (KeyValuePair<TKey, TValue> pair in this)
        {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }
    }

    // Load the dictionary from Lists
    public void OnAfterDeserialize()
    {
        Clear();

        if (keys.Count != values.Count)
        {
            Debug.LogError("Tried to deserialize a SerializableDictionary, but the amount of keys ("
                           + keys.Count + ") does not match the number of values (" + values.Count
                           + ") which indicates that something went wrong");
        }

        for (int i = 0; i < keys.Count; i++)
        {
            Add(keys[i], values[i]);
        }
    }

    #endregion
}