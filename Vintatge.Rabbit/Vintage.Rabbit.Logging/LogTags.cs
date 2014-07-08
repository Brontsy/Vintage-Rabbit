using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Vintage.Rabbit.Logging
{
    public class LogTags : ICollection<KeyValuePair<object, object>>
    {
        private readonly Dictionary<object, object> items;

        public object this[object key]
        {
            get { return items[key]; }
            set { items[key] = value; }
        }

        public static implicit operator LogTags(KeyValuePair<object, object>[] items)
        {
            return new LogTags(items);
        }

        public LogTags(IEnumerable<KeyValuePair<object, object>> items)
        {
            this.items = items.ToDictionary(x => x.Key, x => x.Value);
        }

        public LogTags()
        {
            items = new Dictionary<object, object>();
        }

        public IEnumerator<KeyValuePair<object, object>> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(KeyValuePair<object, object> item)
        {
            items.Add(item.Key, item.Value);
        }

        public void Clear()
        {
            items.Clear();
        }

        public bool Contains(KeyValuePair<object, object> item)
        {
            return items.Contains(item);
        }

        public void CopyTo(KeyValuePair<object, object>[] array, int arrayIndex)
        {
            items.ToList().CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<object, object> item)
        {
            return items.Remove(item);
        }

        public int Count
        {
            get { return items.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }
    }
}