using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceApplicationCasestudy.Models
{
    public class OrderItem
    {
        private int orderItemId;
        private int orderId;
        private int productId;
        private int quantity;

        // Order_item_id (Primary Key)
        public int OrderItemId
        {
            get { return orderItemId; }
            set { orderItemId = value; }
        }

        public int OrderId
        {
            get { return orderId; }
            set { orderId = value; }
        }

        public int ProductId
        {
            get { return productId; }
            set { productId = value; }
        }

        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }

        public OrderItem() 
        {
        
        }

        public OrderItem(int orderItemId, int orderId, int productId, int quantity)
        {
            OrderItemId = orderItemId;
            OrderId = orderId;
            ProductId = productId;
            Quantity = quantity;
        }

        public override string ToString()
        {
            return $"{OrderItemId} {OrderId} {ProductId} {Quantity}";
        }
    }
}
