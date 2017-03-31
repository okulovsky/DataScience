using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataScience.Tables
{


    public class TableDimension<T> : IEnumerable<T>
    {
        Dictionary<T, int> index = new Dictionary<T, int>();
        List<T> values = new List<T>();
        public readonly ReadOnlyCollection<T> Values;
        public T GetValue(int index) { return values[index]; }
        public int GetIndex(T value) { return index[value]; }
        public readonly DimensionIndexerFactory<T> Indexing;

        public readonly string Name;

        public TableDimension(string name)
        {
            this.Name = name;
            Values = new ReadOnlyCollection<T>(values);
            Indexing = new DimensionIndexerFactory<T>(this);
        }

        internal TableDimension(params T[] initialValues)
            :this("")
        {
            foreach (var e in initialValues)
                Add(e);
        }




        public int Add(T value)
        {
                if (index.ContainsKey(value))
                    throw new ArgumentException();
                int result;
            index[value] = result = values.Count;
            values.Add(value);
            return result;
        }

        void Reindex()
        {
            for (int i = 0; i < values.Count; i++)
                index[values[i]] = i;
        }

        public void Insert(T value, int position)
        {
            if (index.ContainsKey(value))
                throw new ArgumentException();
            values.Insert(position, value);
            index[value] = position;
            Reindex();
        }

        public void Remove(T value)
        {
            var i = index[value];
            index.Remove(value);
            values.RemoveAt(i);
            Reindex();
        }

        public bool Contains(T value)
        {
            return index.ContainsKey(value);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
