using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataScience.Tables
{
    public static class Table
    {

        static string Unquote(string s)
        {
            if (s.StartsWith("\"") && s.EndsWith("\""))
                return s.Substring(1, s.Length - 2);
            return s;
        }


        public static DateTime ParseDateTime(string s)
        {
            var parts = s.Split('-', ':', ' ');
            var day = int.Parse(parts[0]);
            var month = int.Parse(parts[1]);
            var year = int.Parse(parts[2]);
            var hour = int.Parse(parts[3]);
            var minute = int.Parse(parts[4]);
            var second = int.Parse(parts[5]);
            return new DateTime(year, month, day, hour, minute, second);
        }

        public static double ParseDouble(string s)
        {
            return double.Parse(s, CultureInfo.InvariantCulture);
        }

        public static string WriteDouble(double e)
        {
            return e.ToString(CultureInfo.InvariantCulture);
        }

        public static string WriteNullableDouble(double? e)
        {
            if (!e.HasValue) return "NA";
            return WriteDouble(e.Value);
        }

        public static string WriteDateTime(DateTime d)
        {
            return d.ToString("dd-MM-yyyy HH:mm:ss");
        }

        public static string Transliterate(object s)
        {
            return Transliterator.Transliterate(s.ToString());
        }

        public static Table<TRow, TColumn, TValue> ReadCsv<TRow, TColumn, TValue>(
            string filename,
            Func<string, TRow> rowParser,
            Func<string, TColumn> columnParser,
            Func<string, TValue> valueParser)
        {
            bool firstRow = true;
            var result = new Table<TRow, TColumn, TValue>();

            var indexer = result.CreateIndexer(
                result.Rows.Indexing.OnlyExisted(), 
                result.Columns.Indexing.Int());

            using (var reader = new StreamReader(filename))
                while (true)
                {
                    var e = reader.ReadLine();
                    if (e == null) break;

                    var values = e.Split(',').Select(Unquote).ToList();
                    if (firstRow)
                    {
                        foreach (var c in values.Skip(1))
                            result.Columns.Add(columnParser(c));
                        firstRow = false;
                        continue;
                    }
                    var rowName = rowParser(values[0]);
                    result.Rows.Add(rowName);
                    for (int i = 0; i < values.Count - 1; i++)
                        indexer[rowName, i] = valueParser(values[i + 1]);
                }
            return result;
        }

        public static void CollapseRows<TResultRowOuter, TResultRowInner, TResultColumnOuter, TResultColumnInner, TResultValue, TSourceRow, TSourceColumn, TSourceValue>
                    (
                    TableIndexer<TResultRowOuter, TResultRowInner, TResultColumnOuter, TResultColumnInner, TResultValue> indexer,
                    Table<TSourceRow, TSourceColumn, TSourceValue> source,
                    Func<TSourceRow, TResultRowOuter> RowSelector,
                    Func<TSourceColumn, TResultColumnOuter> ColumnSelector,
                    Func<List<TSourceValue>, TResultValue> compressor
                    )
        {
            foreach (var column in source.Columns)
            {
                var rows = source
                    .Rows
                    .GroupBy(z => RowSelector(z))
                    .Select(z => new { Key = z.Key, Values = z.Select(x => source.GetValue(x, column)).ToList() })
                    .ToList();
                foreach (var e in rows)
                {
                    indexer[e.Key, ColumnSelector(column)] = compressor(e.Values);
                }
            }
        }

    }
}