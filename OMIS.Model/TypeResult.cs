using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OMIS.Model
{
   public  class TypeResult
   {

       public int TypeId { get; set; }

       public int ChildCount { get; set; }

       public int DataCount { get; set; }

       public TypeResult() { }

       public TypeResult(int typeId)
       {
           this.TypeId = typeId;
       }
    }
}