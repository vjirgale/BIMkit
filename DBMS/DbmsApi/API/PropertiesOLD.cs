using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DbmsApi.API
{
    public class PropertiesOLD : IEnumerable<KeyValuePair<string,string>>, ICollection<KeyValuePair<string, string>>
    {
        private Dictionary<string, string> _properties = new Dictionary<string, string>();

        public string this[string key] { get { return _properties[key.ToLower()]; } set { _properties[key.ToLower()] = value; } }

        public ICollection<string> Keys => _properties.Keys;

        public ICollection<string> Values => _properties.Values;

        public int Count => _properties.Count;

        public bool IsReadOnly => false;

        public void Add(string key, string value)
        {
            _properties.Add(key.ToLower(), value);
        }

        public void Add(KeyValuePair<string, string> item)
        {
            _properties.Add(item.Key.ToLower(), item.Value);
        }

        public void Clear()
        {
            _properties.Clear();
        }

        public bool Contains(KeyValuePair<string, string> item)
        {
            return _properties.Contains(new KeyValuePair<string, string>(item.Key.ToLower(), item.Value));
        }

        public bool ContainsKey(string key)
        {
            return _properties.ContainsKey(key.ToLower());
        }

        public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
        {
            _properties.ToList().CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return _properties.GetEnumerator();
        }

        public bool Remove(string key)
        {
            return _properties.Remove(key);
        }

        public bool Remove(KeyValuePair<string, string> item)
        {
            if (_properties.ContainsKey(item.Key.ToLower()))
                if (_properties[item.Key.ToLower()].Equals(item.Value))
                    return _properties.Remove(item.Key);
            return false;
        }

        public bool TryGetValue(string key, out string value)
        {
            return _properties.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _properties.GetEnumerator();
        }


    }
}
