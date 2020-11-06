namespace apiCRUD.DOMAIN
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Newtonsoft.Json;

    [MetadataType(typeof(CustomersMetadata))]
    public partial class Customers
    {
        private class CustomersMetadata
        {
            //在特定導覽屬性上套用 [JsonIgnore] 屬性(Attribute)即可防止參考循環問題發生
            [JsonIgnore]
            public virtual ICollection<Orders> Orders { get; set; }
        }
    }
}