namespace apiCRUD.DOMAIN
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    [MetadataType(typeof(CustomersMetadata))]
    public partial class Customers
    {
        private class CustomersMetadata
        {
            [Key]

            public string CustomerID { get; set; }

            //在特定導覽屬性上套用 [JsonIgnore] 屬性(Attribute)即可防止參考循環問題發生
            [JsonIgnore]
            public virtual ICollection<Orders> Orders { get; set; }
        }
    }
}