using System;
using Sail.Common;

namespace Poetry.Model
{
    public abstract class UserInfoBase : IModel
    {
        [HColumn(IsPrimary = true, IsIdentity = true)]
        public int Id { set; get; }
 

        [HColumn(IsGuid = true)]
        public string CreaterId { set; get; }

     

        [HColumn(Length = 50)]
        public string CreaterName { set; get; }

        [HColumn]
        public DateTime CreateTime { set; get; }
    }
}