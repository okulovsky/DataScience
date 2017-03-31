using DataScience.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataScience
{
    public class HistogramRow<TBucket> : IComparable
    {
        public readonly TBucket Bucket;
        public readonly int Index;

        public HistogramRow(TBucket bucket, int index)
        {
            Bucket = bucket;
            Index = index;
        }

        public int CompareTo(object obj)
        {
            return Index.CompareTo(((HistogramRow<TBucket>)obj).Index);
        }

        public override bool Equals(object obj)
        {
            return Index.Equals(((HistogramRow<TBucket>)obj).Index);
        }

        public override int GetHashCode()
        {
            return Index.GetHashCode();
        }

    }


    public class Histogram<TBucket,TColumn>
    {
        public readonly Table<HistogramRow<TBucket>, TColumn, double> Data;
        public readonly Func<int, TBucket> IndexToBucket;
        private TableIndexer<HistogramRow<TBucket>, HistogramRow<TBucket>, TColumn, TColumn, double> indexer;

        public Histogram(Func<int, TBucket> IndexToBucket)
        {
            this.IndexToBucket = IndexToBucket;
            Data = new Table<HistogramRow<TBucket>, TColumn, double>();
            indexer = Data.Indexer(
                row => row.SortedContinous(z => Row(z.Index+1)),
                column => column.AutoCreate());
        }

        public void AddColumn<TData>(TColumn header, IEnumerable<TData> data, Func<TData,int> bucketSelector, int trimValue, bool omitBadValues=false)
        {
            foreach (var e in data)
            {
                var i = bucketSelector(e);
                if (i < -trimValue)
                {
                    if (omitBadValues) continue;
                    i = -trimValue;
                }
                if (i > trimValue)
                {
                    if (omitBadValues) continue;
                    i = trimValue;
                }
                var b = Row(i);
                indexer[b, header]++;
            }
        }

        public void AddValue(TColumn header, int bucket, double value)
        {
            indexer[Row(bucket), header] += value;
        }

        HistogramRow<TBucket> Row(int index)
        {
            return new HistogramRow<TBucket>(IndexToBucket(index), index);
        }

        public Histogram<TBucket,TColumn> ToPercentage(double percentTotal=100)
        {
            var sums = Data.Columns.Select(z => new
            {
                Key = z,
                Value = Data.GetColumnData(z).Sum(x => x.Value)
            })
            .ToDictionary(z => z.Key, z => z.Value);
        
            var h= new Histogram<TBucket, TColumn>(IndexToBucket);
            foreach (var e in Data.Rows)
                foreach (var c in Data.Columns)
                    h.indexer[e, c] = percentTotal*Data.GetValue(e, c) / sums[c];

            return h;
        }

      
    }
    
}
