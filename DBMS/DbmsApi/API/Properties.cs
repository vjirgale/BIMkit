using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DbmsApi.API
{
    public class Properties
    {
        private List<Property> _properties = new List<Property>();

        public ReadOnlyCollection<Property> Data { get { return _properties.AsReadOnly(); } }

        public Property this[string key]
        {
            get
            {
                return _properties.First(p => p.Name == key);
            }
        }
        public Property this[int index]
        {
            get
            {
                return _properties[index];
            }
        }

        public IEnumerable<string> Keys => _properties.Select(p => p.Name);

        public IEnumerable<string> Values => _properties.Select(p => p.GetValueString());

        public int Count => _properties.Count;

        public bool IsReadOnly => false;

        public bool Add(string key, string value)
        {
            return Add(new PropertyString(key, value));
        }
        public bool Add(string key, bool value)
        {
            return Add(new PropertyBool(key, value));
        }
        public bool Add(string key, double value)
        {
            return Add(new PropertyNum(key, value));
        }
        public bool Add(Property property)
        {
            if (_properties.Any(p => p.Name == property.Name))
            {
                return false;
            }
            else
            {
                _properties.Add(property);
                return true;
            }
        }

        public void Clear()
        {
            _properties.Clear();
        }

        public bool Contains(Property property)
        {
            return _properties.Any(p => p.Name == property.Name && p.GetValueString() == property.GetValueString());
        }

        public bool ContainsKey(string key)
        {
            return _properties.Any(p => p.Name == key);
        }

        public void CopyTo(Property[] array, int arrayIndex)
        {
            _properties.CopyTo(array, arrayIndex);
        }

        public IEnumerator<Property> GetEnumerator()
        {
            return _properties.GetEnumerator();
        }

        public bool Remove(Property property)
        {
            return _properties.Remove(property);
        }

        public bool Remove(string key)
        {
            Property property = _properties.FirstOrDefault(p => p.Name == key);
            return _properties.Remove(property);
        }

        public bool TryGetValue(string key, out string value)
        {
            if (ContainsKey(key))
            {
                value = this[key].GetValueString();
                return true;
            }
            else
            {
                value = default;
                return false;
            }
        }

        public bool TryGetProperty(string key, out Property value)
        {
            if (ContainsKey(key))
            {
                value = this[key];
                return true;
            }
            else
            {
                value = default;
                return false;
            }
        }
    }
}