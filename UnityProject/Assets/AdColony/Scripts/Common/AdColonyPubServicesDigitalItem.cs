using System;
using System.Collections;

namespace AdColony {
    public class PubServicesDigitalItem {
        // ProductId of the digital item as set in the AdColony Developer Portal
        public string ProductId { get; private set; }

        // Quantity of the item, i.e. 500 Coins
        public int Quantity { get; private set; }

        // Name as set in the AdColony Developer Portal
        public string Name { get; private set; }

        // Description as set in the AdColony Developer Portal
        public string ProductDescription { get; private set; }

        // Additional parameters set on the developer portal
        public Hashtable UserParams { get; private set; }

        public PubServicesDigitalItem(Hashtable values) {
            ProductId = "";
            Quantity = 0;
            Name = "";
            ProductDescription = "";
            UserParams = new Hashtable();

            if (values != null) {
                if (values.ContainsKey("product_id")) {
                    ProductId = values["product_id"] as string;
                }
                if (values.ContainsKey("quantity")) {
                    Quantity = Convert.ToInt32(values["quantity"]);
                }
                if (values.ContainsKey("name")) {
                    Name = values["name"] as string;
                }
                if (values.ContainsKey("description")) {
                    ProductDescription = values["description"] as string;
                }
                if (values.ContainsKey("user_params")) {
                    UserParams = values["user_params"] as Hashtable;
                }
            }
        }
    }
}
