using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceApplicationCasestudy.Models
{
    public class Cart
    {
        private int cartId;
        private int customerId;
        private int productId;
        private int? quantity;

        // Cart_id (Primary Key)
        public int CartId
        {
            get { return cartId; }
            set { cartId = value; }
        }

        public int CustomerId
        {
            get { return customerId; }
            set { customerId = value; }
        }

        public int ProductId
        {
            get { return productId; }
            set { productId = value; }
        }
        public int? Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }
        public Cart() 
        { 
        
        }

        public Cart(int cartId, int customerId, int productId, int quantity)
        {
            CartId = cartId;
            CustomerId = customerId;
            ProductId = productId;
            Quantity = quantity;
        }

        public override string ToString()
        {
            return $"{CartId} {CustomerId} {ProductId} {Quantity}";
        }
    }
}
