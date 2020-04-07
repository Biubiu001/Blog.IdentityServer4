using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace Web.Core.Model.Models
{
   public class RootEntity
    {
        /// <summary>
        /// ID
        /// </summary>
   //     [SugarColumn(IsNullable = false, IsPrimaryKey = true, IIdentity = true)]
        public int Id { get; set; }
    }
}
