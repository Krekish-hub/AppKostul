//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UchetGIC.DataFiles
{
    using System;
    using System.Collections.Generic;
    
    public partial class EmployeeObject
    {
        public int EmployeeObjectID { get; set; }
        public Nullable<int> EmployeeID { get; set; }
        public Nullable<int> ObjectID { get; set; }
    
        public virtual Employees Employees { get; set; }
        public virtual Objects Objects { get; set; }
    }
}
