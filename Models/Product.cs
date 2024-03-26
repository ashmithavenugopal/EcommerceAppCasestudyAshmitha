using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceApplicationCasestudy.Models
{
    public class Product
    {
        private int productId;
        private string name;
        private decimal price;
        private string description;
        private int stockQuantity;

        // Product_id (Primary Key)
        public int ProductId
        {
            get { return productId; }
            set { productId = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public decimal Price
        {
            get { return price; }
            set { price = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public int StockQuantity
        {
            get { return stockQuantity; }
            set { stockQuantity = value; }
        }


        public Product()
        {

        }

        public Product(int productId, string name, int price, string description, int stockQuantity)
        {
            ProductId = productId;
            Name = name;
            Price = price;
            Description = description;
            StockQuantity = stockQuantity;
        }

        public override string ToString()
        {
            return $"{ProductId} {Name} {Price} {Description} {StockQuantity}";
        }
    }
}
