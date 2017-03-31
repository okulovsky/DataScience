using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataScience.Tables
{
    public class TableIndexer<TRowOuter,TRowInner,TColumnOuter,TColumnInner,TValue>
    {
        Table<TRowInner, TColumnInner, TValue> table;
        IDimenstionIndexer<TRowInner, TRowOuter> rowIndexer;
        IDimenstionIndexer<TColumnInner, TColumnOuter> columnIndexer;

        public TableIndexer(Table<TRowInner,TColumnInner,TValue> table, IDimenstionIndexer<TRowInner,TRowOuter> rowIndexer, IDimenstionIndexer<TColumnInner,TColumnOuter> columnIndexer)
        {
            this.table = table;
            this.rowIndexer = rowIndexer;
            this.columnIndexer = columnIndexer;
        }

        public TValue this[TRowOuter row,TColumnOuter column]
        {
            get
            {
                
                
                return table.GetValue(rowIndexer.Touch(row), columnIndexer.Touch(column));
            }
            set
            {

                table.SetValue(rowIndexer.Touch(row), columnIndexer.Touch(column),value);
            }
        }
    }
}
