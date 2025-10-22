using System;
using System.Collections.Generic;

namespace FMS_Collection.Core.Response
{
    public class ListItem
    {
        public Int64 ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string OtherInfo { get; set; }

        public bool Contains(List<ListItem> List) {
            foreach (ListItem item in List)
            {
                if (item.Name.Equals(this.Name)) {
                    return true;
                }
            }
            return false;
        }
    }
    
}
