using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataScience.Tables
{
    public interface IDimenstionIndexer<TInner,TOuter>
    {
        TInner Touch(TOuter value);
       
    }





   

}
