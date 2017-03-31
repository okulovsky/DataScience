using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataScience.DataAccess
{
    public static class IDbConnectionExtensions
    {
        public static IEnumerable<object[]> Query(this IDbConnection cn, string query)
        {
            var cmd = cn.CreateCommand();
            cmd.CommandText = query;
            using (var reader = cmd.ExecuteReader())
                while (reader.Read())
                {
                    var data = Enumerable.Range(0, reader.FieldCount).Select(z => reader[z]).ToArray();
                    yield return data;
                }
        }

        public static IEnumerable<Dictionary<string,object>> QueryDict(this IDbConnection cn, string query)
        {
            var cmd = cn.CreateCommand();
            cmd.CommandText = query;
            cmd.CommandTimeout = 300;
            using (var reader = cmd.ExecuteReader())
                while (reader.Read())
                {
                    var data = Enumerable
                        .Range(0, reader.FieldCount)
                        .ToDictionary(z => reader.GetName(z), z => reader[z]);
                    yield return data;
                }
        }

        public static void Execute(this IDbConnection cn, string command)
        {
            var cmd = cn.CreateCommand();
            cmd.CommandText = command;
            cmd.ExecuteNonQuery();
        }
    }
}
