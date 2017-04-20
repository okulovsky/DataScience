using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataScience.Tables
{




    public class Table<TRow,TColumn,TValue>
    {
        public readonly TableDimension<TRow> Rows = new TableDimension<TRow>("Rows");
        public readonly TableDimension<TColumn> Columns = new TableDimension<TColumn>("Columns");
        Dictionary<TRow, Dictionary<TColumn, TValue>> data = new Dictionary<TRow, Dictionary<TColumn, TValue>>();

        public int[] Shape { get { return new[] { Rows.Count(), Columns.Count() }; } }

        public TValue GetValue(TRow row, TColumn column)
        {
            if (!Rows.Contains(row)) throw new ArgumentException();
            if (!Columns.Contains(column)) throw new ArgumentException();
            if (!data.ContainsKey(row)) return default(TValue);
            if (!data[row].ContainsKey(column)) return default(TValue);
            return data[row][column];
        }

        public void SetValue(TRow row, TColumn column, TValue value)
        {
            if (!Rows.Contains(row)) throw new ArgumentException();
            if (!Columns.Contains(column)) throw new ArgumentException();
            if (!data.ContainsKey(row)) data[row] = new Dictionary<TColumn, TValue>();
            data[row][column] = value;
        }

        public TableIndexer<TRowOuter,TRow,TColumnOuter,TColumn,TValue>
            CreateIndexer<TRowOuter,TColumnOuter>(
                IDimenstionIndexer<TRow,TRowOuter> rowIndexer,
                IDimenstionIndexer<TColumn,TColumnOuter> columnIndexer)
        {
            return new TableIndexer<TRowOuter, TRow, TColumnOuter, TColumn, TValue>(this, rowIndexer, columnIndexer);
        }

        public TableIndexer<TRowOuter, TRow, TColumnOuter, TColumn, TValue>
           Indexer<TRowOuter, TColumnOuter>(
            Func<DimensionIndexerFactory<TRow>,IDimenstionIndexer<TRow,TRowOuter>> row,
            Func<DimensionIndexerFactory<TColumn>, IDimenstionIndexer<TColumn, TColumnOuter>> column)
        {
            return CreateIndexer(row(Rows.Indexing), column(Columns.Indexing));
        }





        public void WriteToCsv(string filename, Func<TRow, string> rowWriter = null, Func<TColumn, string> columnWriter = null, Func<TValue, string> valueWriter = null)
        {
            if (rowWriter == null) rowWriter = z => z.ToString();
            if (columnWriter == null) columnWriter = z => z.ToString();
            if (valueWriter == null) valueWriter = z => z.ToString();

            using (var writer = new StreamWriter(File.Open(filename, FileMode.Create, FileAccess.Write), new UTF8Encoding(false)))
            {
                writer.Write($"\"{typeof(TRow).Name}\"");
                for (int i = 0; i < Columns.Values.Count; i++)
                    writer.Write($",\"{columnWriter(Columns.Values[i])}\"");
                writer.WriteLine();

                foreach (var row in Rows)
                {
                    writer.Write($"\"{rowWriter(row)}\"");
                    foreach (var col in Columns)
                        writer.Write($",{valueWriter(GetValue(row, col))}");
                    writer.WriteLine();
                }
            }
        }

        public IEnumerable<KeyValuePair<TRow,TValue>> GetColumnData(TColumn column)
        {
            foreach (var e in Rows)
                yield return new KeyValuePair<TRow, TValue>(e, GetValue(e, column));
        }

        public IEnumerable<KeyValuePair<TRow,TValue>> GetColumnDataByIndex(int index)
        {
            return GetColumnData(Columns.Values[index]);
        }
    }
}
