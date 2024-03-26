using EcommerceApplicationCasestudy.Exceptions;
using EcommerceApplicationCasestudy.Models;
using EcommerceApplicationCasestudy.Utility;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceApplicationCasestudy.Repository
{
    public class OrderProcessorRepository : IOrderProcessorRepository
    {
        SqlConnection connect = null;
        SqlCommand cmd = null;
        public OrderProcessorRepository()
        {
            connect = new SqlConnection(DataConnectionUtility.GetConnectionString());
            cmd = new SqlCommand();
        }
        

        public bool CreateCustomer(Customer customer)
        {
            cmd.CommandText = "Insert into customers values(@c_name,@email,@password)";
            cmd.Parameters.AddWithValue("@c_name", customer.Name);
            cmd.Parameters.AddWithValue("@email", customer.Email);
            cmd.Parameters.AddWithValue("@password", customer.Password);
            connect.Open();
            cmd.Connection = connect;
            cmd.ExecuteNonQuery();
            connect.Close();
            return true;
        }

        public bool CreateProduct(Product product)
        {
            cmd.CommandText = "Insert into products values(@p_name,@price,@desc,@stockquantity)";
            cmd.Parameters.AddWithValue("@p_name", product.Name);
            cmd.Parameters.AddWithValue("@price", product.Price);
            cmd.Parameters.AddWithValue("@desc", product.Description);
            cmd.Parameters.AddWithValue("@stockquantity", product.StockQuantity);
            connect.Open();
            cmd.Connection = connect;
            cmd.ExecuteNonQuery();
            connect.Close();
            return true;
        }

        public bool DeleteCustomer(int customerId)
        {
            cmd.CommandText = "delete from customers where customer_id=@c_id";
            cmd.Parameters.AddWithValue("@c_id", customerId);
            connect.Open();
            cmd.Connection = connect;
            cmd.ExecuteNonQuery();
            connect.Close();
            return true;

        }

        public bool DeleteProduct(int productId)
        {
            cmd.CommandText = "delete from products where product_id=@p_id";
            cmd.Parameters.AddWithValue("@p_id", productId);
            connect.Open();
            cmd.Connection = connect;
            cmd.ExecuteNonQuery();
            connect.Close();
            return true;
        }


        public bool PlaceOrder(Customer customer, Order order, OrderItem orderitem, Product product)
        {
            decimal productprice = 0m;
            int currentStock = 0;

            // Open connection once to execute all queries in sequence
            connect.Open();
            cmd.Connection = connect;

            // Fetch product price and stock
            cmd.CommandText = "SELECT prod_price, stock_Quantity FROM products WHERE product_id = @productid";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@productid", product.ProductId);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read()) // Ensure there is a row to read
                {
                    productprice = reader.GetInt32(reader.GetOrdinal("prod_price"));
                    currentStock = reader.GetInt32(reader.GetOrdinal("stock_Quantity"));
                }
            }

            // Check if product price and stock were successfully fetched
            if (productprice == 0 || currentStock == 0)
            {
                connect.Close();
                return false; // Or handle the error appropriately
            }

            // Calculate total price
            int quantityOrdered = orderitem.Quantity;
            decimal totalPrice = productprice * quantityOrdered;
            // Check if the customer exists
            cmd.CommandText = "SELECT COUNT(*) FROM customers WHERE customer_id = @customerid";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@customerid", customer.CustomerId);
            int customerCount = (int)cmd.ExecuteScalar();

            if (customerCount == 0)
            {
                connect.Close();
                return false; // Customer does not exist, handle the error appropriately
            }

            // Insert into orders and retrieve the generated OrderId
            cmd.CommandText = "INSERT INTO orders (customer_id, order_date, total_price, shipping_address) OUTPUT INSERTED.order_id VALUES (@customerid, @orderdate, @totalprice, @shippingadd)";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@customerid", customer.CustomerId);
            cmd.Parameters.AddWithValue("@orderdate", DateTime.Now);
            cmd.Parameters.AddWithValue("@totalprice", totalPrice);
            cmd.Parameters.AddWithValue("@shippingadd", order.ShippingAddress);

            int generatedOrderId;
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    generatedOrderId = reader.GetInt32(0); // Assuming order_id is the first column
                }
                else
                {
                    connect.Close();
                    return false; // Failed to insert order, handle the error appropriately
                }
            }

            // Insert into order_items
            cmd.CommandText = "INSERT INTO order_items (order_id, product_id, quantity) VALUES (@orderid, @productid, @quantity)";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@orderid", generatedOrderId);
            cmd.Parameters.AddWithValue("@productid", product.ProductId);
            cmd.Parameters.AddWithValue("@quantity", quantityOrdered);
            cmd.ExecuteNonQuery();

            // Update product stock
            int newStock = currentStock - quantityOrdered;
            cmd.CommandText = "UPDATE products SET stock_Quantity = @newstock WHERE product_id = @productid";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@newstock", newStock);
            cmd.Parameters.AddWithValue("@productid", product.ProductId);
            cmd.ExecuteNonQuery();
            // Close connection
            connect.Close();

            return true;

        }

        Customer customer = new Customer();
        Order order= new Order();

        internal bool CustomerNotExists(int customer_id)
        {
            int count = 0;
            cmd.CommandText = "Select count(*) as total from customers where customer_id=@c_id";
            cmd.Parameters.AddWithValue("@c_id", customer_id);
            connect.Open();
            cmd.Connection = connect;
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                count = (int)reader["total"];
            }
            connect.Close();
            if (count > 0)
            {
                return true;
            }
            return false;
        }


        public bool ProductNotFound(int product)
        {
            int count = 0;
            cmd.CommandText = "Select count(*) as total from products where product_id=@p_id";
            cmd.Parameters.AddWithValue("@p_id", product);
            connect.Open();
            cmd.Connection = connect;
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                count = (int)reader["total"];
            }
            connect.Close();
            if (count > 0)
            {
                return true;
            }
            return false;
        }

        public bool AddToCart(Customer customer,Product product, int quantity)
        {
            cmd.CommandText = "Insert into cart values(@c_id,@p_id,@quantity)";
            cmd.Parameters.AddWithValue("@c_id", customer.CustomerId);
            cmd.Parameters.AddWithValue("@p_id", product.ProductId );
            cmd.Parameters.AddWithValue("@quantity", quantity);

            connect.Open();
            cmd.Connection = connect;
            cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            connect.Close();
            return true;

        }

        internal int availablestockquantity(int productid)
        {
            int count = 0;
            cmd.CommandText = "Select stock_Quantity as avalquantity from products where product_id=@pid";
            cmd.Parameters.AddWithValue("@pid", productid);
            connect.Open();
            cmd.Connection = connect;
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                count = (int)reader["avalquantity"];
            }
            connect.Close();
            return count;


        }

        public bool RemoveFromCart(Customer customer, Product products)
        {
            cmd.CommandText = "delete from cart where customer_id = @c_id and product_id=@pid";
            cmd.Parameters.AddWithValue("@c_id", customer.CustomerId);
            cmd.Parameters.AddWithValue("@pid", products.ProductId);
            connect.Open();
            cmd.Connection = connect;
            cmd.ExecuteNonQuery();
            connect.Close();

            return true;

        }

         internal bool ProductNotExistinCart(int productid, int customerid)
         {
                int count = 0;
                cmd.CommandText = "Select count(*) as total from cart where product_id=@pid and customer_id=@cid";
                cmd.Parameters.AddWithValue("@pid", productid);
                cmd.Parameters.AddWithValue("@cid", customerid);
                connect.Open();
                cmd.Connection = connect;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    count = (int)reader["total"];
                }
                connect.Close();
                if (count > 0)
                {
                    return true;
                }
                return false;
         }
        public List<Cart> GetAllFromCart(Customer customer)
        {
            List<Cart> cartList = new List<Cart>();
            cmd.CommandText = "Select * from cart where customer_id=@customerid";
            cmd.Parameters.AddWithValue("@customerid", customer.CustomerId);
            connect.Open();
            cmd.Connection = connect;
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Cart cart1 = new Cart();
                cart1.CartId = (int)reader["cart_id"];
                cart1.CustomerId = (int)reader["customer_id"];
                cart1.ProductId = (int)reader["product_id"]; 
                cart1.Quantity = Convert.IsDBNull(reader["quantity"]) ? null : (int)reader["quantity"];
                cartList.Add(cart1);
            }
            connect.Close();
            return cartList;
        }

        public List<OrderItem> GetOrderByCustomer(Order order)
        {
            List<OrderItem> OrderList = new List<OrderItem>();

            cmd.CommandText = "select oi.product_id as product,oi.quantity as quantity from order_items as oi " +
                "join orders as o on o.order_id = oi.order_id where o.customer_id = @ocustomer";

            cmd.Parameters.AddWithValue("@ocustomer", order.CustomerId);
            connect.Open();
            cmd.Connection = connect;
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                OrderItem orderItem = new OrderItem();
                orderItem.ProductId = (int)reader["product"];
                orderItem.Quantity = (int)reader["quantity"];

                OrderList.Add(orderItem);
            }
            connect.Close();
            return OrderList;
        }

        internal bool OrderNotExist(int orderid)
        {
            int count = 0;
            cmd.CommandText = "Select count(*) as total from orders where order_id=@oid";
            cmd.Parameters.AddWithValue("@oid", orderid);
            connect.Open();
            cmd.Connection = connect;
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                count = (int)reader["total"];
            }
            connect.Close();
            if (count > 0)
            {
                return true;
            }
            return false;
        }


    }
}
