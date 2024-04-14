using EcommerceApplicationCasestudy.Exceptions;
using EcommerceApplicationCasestudy.Models;
using EcommerceApplicationCasestudy.Utility;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
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
            
                if (!customer.Email.Contains('@'))
                {
                    throw new CustomerNotFoundException("Invalid Email");
                }
               
                              
            cmd.CommandText = "Insert into customers values(@c_name,@email,@password)";
            cmd.Parameters.AddWithValue("@c_name", customer.Name);
            cmd.Parameters.AddWithValue("@email", customer.Email);
            cmd.Parameters.AddWithValue("@password", customer.Password);
            connect.Open();
            cmd.Connection = connect;
            cmd.ExecuteNonQuery();
            connect.Close();
            Console.WriteLine("Customer Records inserted successfully");
            return true;

        }

        public bool CreateProduct(Product product)
        {
           
                if (product.Price < 0)
                {
                    throw new ProductNotFoundException("Invalid Price amount");
                }
                       
            cmd.CommandText = "Insert into products values(@p_name,@price,@desc,@stockquantity)";
            cmd.Parameters.AddWithValue("@p_name", product.Name);
            cmd.Parameters.AddWithValue("@price", product.Price);
            cmd.Parameters.AddWithValue("@desc", product.Description);
            cmd.Parameters.AddWithValue("@stockquantity", product.StockQuantity);
            connect.Open();
            cmd.Connection = connect;
            cmd.ExecuteNonQuery();
            connect.Close();
            Console.WriteLine("Product created successfully");
            return true;
        }

        public bool DeleteCustomer(int customerId)
        {
            
                if (!CustomerNotExists(customerId))
                    throw new CustomerNotFoundException("Customer not found!!!");
                
           

            cmd.CommandText = "delete from customers where customer_id=@customer_id";
            cmd.Parameters.AddWithValue("@customer_id", customerId);
            connect.Open();
            cmd.Connection = connect;
            cmd.ExecuteNonQuery();           
            connect.Close();            
            Console.WriteLine("Customer Records deleted successfully");
            return true;
           

        }

        public bool DeleteProduct(int productId)
        {
                      
           if (!ProductNotExists(productId))
               throw new ProductNotFoundException("Product not found!!!");
            
            
            cmd.CommandText = "delete from products where product_id=@product_id";
            cmd.Parameters.AddWithValue("@product_id", productId);
            connect.Open();
            cmd.Connection = connect;
            cmd.ExecuteNonQuery();
            connect.Close();
            Console.WriteLine("Product Records deleted successfully");
            return true;
        }

        public bool AddToCart(Customer customer, Product product, int quantity)
        {

            NotEnoughStock(quantity, product.ProductId);

            cmd.CommandText = "Insert into cart values(@cust_id,@p_id,@quantity)";
            cmd.Parameters.AddWithValue("@cust_id", customer.CustomerId);
            cmd.Parameters.AddWithValue("@p_id", product.ProductId);
            cmd.Parameters.AddWithValue("@quantity", quantity);

            connect.Open();
            cmd.Connection = connect;
            cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            connect.Close();
            Console.WriteLine("Product added to cart ");
            return true;

        }
        public bool RemoveFromCart(Customer customer, Product products)
        {

            if (!ProductNotExistinCart(products.ProductId,customer.CustomerId))
                throw new ProductNotFoundException("Product not found in cart for the customer!!!");

            cmd.CommandText = "delete from cart where customer_id = @cusid and product_id=@pid";
            cmd.Parameters.AddWithValue("@cusid", customer.CustomerId);
            cmd.Parameters.AddWithValue("@pid", products.ProductId);
            connect.Open();
            cmd.Connection = connect;
            cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            connect.Close();
            Console.WriteLine("Product deleted from cart");

            return true;

        }

        public List<Cart> GetAllFromCart(Customer customer)
        {
            if (!CustomerNotExists(customer.CustomerId))
            {
                throw new CustomerNotFoundException("Customer not found");
            }
            
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


        public bool PlaceOrder(Customer customer, List<Dictionary<Product, int>> productsAndQuantity, string shippingAddress)
        {
            
                if (!CustomerNotExists(customer.CustomerId))
                    throw new CustomerNotFoundException("Customer not found!!!");
                List<Cart> cartItems = GetAllFromCart(customer);
                int orderId = CreateOrder(customer, shippingAddress, cartItems);

                foreach (var productQuantityPair in productsAndQuantity)
                {
                    foreach (var keyvaluepair in productQuantityPair)
                    {
                        Product product = keyvaluepair.Key;
                        int quantity = keyvaluepair.Value;
                        InsertOrderItemAndUpdateStock(orderId, product, quantity);
                    }
                }

                RemoveProductFromCart(customer);
                return true;
                       
        }

        private int CreateOrder(Customer customer, string shippingAddress, List<Cart> cartItems)
        {
            int generatedOrderId = 0;
            int totalPrice = CalculateTotalPrice(cartItems);


            try
            {
                cmd.CommandText = "INSERT INTO orders (customer_id, order_date, total_price, shipping_address) OUTPUT INSERTED.order_id VALUES (@customerId_order, @orderDate, @totalPrice, @shippingAddress); ";
                cmd.Parameters.AddWithValue("@customerId_order", customer.CustomerId);
                cmd.Parameters.AddWithValue("@orderDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@shippingAddress", shippingAddress);
                cmd.Parameters.AddWithValue("@totalPrice", totalPrice);

                connect.Open();
                cmd.Connection = connect;
                object result = cmd.ExecuteScalar();

                if (result != null)
                {
                    generatedOrderId = Convert.ToInt32(result);
                    Console.WriteLine("Order Total price is Rs. " + totalPrice);
                }
                else
                {
                    Console.WriteLine("Generated order ID is null.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error creating order: " + ex.Message);
            }
            finally
            {
                cmd.Parameters.Clear(); // Clear parameters after executing the command
                connect.Close();
            }

            return generatedOrderId;
        }

        public int CalculateTotalPrice(List<Cart> cartItems)
        {
            int totalPrice = 0;

            foreach (var cartItem in cartItems)
            {
                int productPrice = GetProductPrice(cartItem.ProductId); // Directly using ProductId as it's not nullable
                totalPrice += productPrice * (cartItem.Quantity ?? 0);
            }

            return totalPrice;
        }


        private void InsertOrderItemAndUpdateStock(int orderId, Product product, int quantity)
        {
            try
            {
                int productPrice = GetProductPrice(product.ProductId);
                InsertOrderItem(orderId, product.ProductId, quantity);
                UpdateProductStock(product.ProductId, quantity);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in inserting order item and updating stock: " + ex.Message);
                throw;
            }
        }

        private void InsertOrderItem(int orderId, int productId, int quantity)
        {
            try
            {
                cmd.CommandText = "INSERT INTO order_items (order_id, product_id, quantity) VALUES (@orderId_item, @prod_Id, @quantity);";
                cmd.Parameters.AddWithValue("@orderId_item", orderId);
                cmd.Parameters.AddWithValue("@prod_Id", productId);
                cmd.Parameters.AddWithValue("@quantity", quantity);

                connect.Open();
                cmd.Connection = connect;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in inserting order item: " + ex.Message);
                throw;
            }
            finally
            {
                cmd.Parameters.Clear();
                connect.Close();
            }
        }

        private void RemoveProductFromCart(Customer customer)
        {
            try
            {
                cmd.CommandText = "DELETE FROM cart WHERE customer_id = @customerId_remove_cart";
                cmd.Parameters.AddWithValue("@customerId_remove_cart", customer.CustomerId);

                connect.Open();
                cmd.Connection = connect;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in removing products from cart: " + ex.Message);
                throw;
            }
            finally
            {
                cmd.Parameters.Clear();
                connect.Close();
            }
        }

        public int GetProductPrice(int productId)
        {
            int productprice = 0;

            try
            {
                connect.Open();
                cmd.Connection = connect;
                cmd.CommandText = "SELECT prod_price FROM products WHERE product_id = @productid";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@productid", productId);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        productprice = reader.GetInt32(reader.GetOrdinal("prod_price"));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching product price: " + ex.Message);
            }
            finally
            {
                connect.Close();
            }

            return productprice;
        }

        private void UpdateProductStock(int productId, int quantity)
        {
            try
            {
                cmd.CommandText = "UPDATE products SET stock_Quantity = stock_Quantity - @quantity WHERE product_id = @product_Id AND stock_Quantity >= @quantity;";
                cmd.Parameters.AddWithValue("@quantity", quantity);
                cmd.Parameters.AddWithValue("@product_Id", productId);

                connect.Open();
                cmd.Connection = connect;
                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    throw new Exception("Not enough stock for product ID " + productId);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating product stock: " + ex.Message);
                throw;
            }
            finally
            {
                cmd.Parameters.Clear();
                connect.Close();
            }
        }
       
        public List<Dictionary<Product, int>> GetOrdersByCustomer(int customerId)
        {
            List<Dictionary<Product, int>> orders = new List<Dictionary<Product, int>>();
            using (SqlConnection connect = new SqlConnection(DataConnectionUtility.GetConnectionString()))
            using (SqlCommand cmd = connect.CreateCommand())
            {
                if (connect.State != ConnectionState.Open)
                    connect.Open();

                // Construct the SQL command to fetch orders by customer
                cmd.CommandText = "SELECT o.order_id, oi.product_id, oi.quantity " +
                                  "FROM orders o " +
                                  "JOIN order_items oi ON o.order_id = oi.order_id " +
                                  "WHERE o.customer_id = @customerId";

                // Clear previous parameters before adding new ones
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@customerId", customerId);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    Dictionary<int, Dictionary<Product, int>> tempOrders = new Dictionary<int, Dictionary<Product, int>>();
                    while (reader.Read())
                    {
                        int orderId = Convert.ToInt32(reader["order_id"]);
                        int productId = Convert.ToInt32(reader["product_id"]);
                        int quantity = Convert.ToInt32(reader["quantity"]);
                        if (!tempOrders.ContainsKey(orderId))
                        {
                            tempOrders[orderId] = new Dictionary<Product, int>();
                        }
                        Product product = GetProductInfo(productId);
                        if (product != null)
                        {
                            tempOrders[orderId].Add(product, quantity);
                        }
                    }
                    orders = tempOrders.Values.ToList();
                }
            }
            return orders;
        }

        public bool CustomerNotExists(int customerId)
        {

            int count = 0;
            cmd.Parameters.Clear();
            cmd.CommandText = "Select count(*) as total from customers where customer_id=@c_id";
            cmd.Parameters.AddWithValue("@c_id", customerId);
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


        public bool ProductNotExists(int product)
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

      

        internal int Availablestockquantity(int productid)
        {
            int count = 0;
            cmd.CommandText = "Select stock_Quantity as avalquantity from products where product_id=@proid";
            cmd.Parameters.AddWithValue("@proid", productid);
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
       

         internal bool ProductNotExistinCart(int productid, int customerid)
         {
                int count = 0;
                cmd.CommandText = "Select count(*) as total from cart where product_id=@prodid and customer_id=@cid";
                cmd.Parameters.AddWithValue("@prodid", productid);
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
          

        internal bool OrderNotExist(int customerId) 
        {
            int count = 0;
            cmd.CommandText = "Select count(*) as total from orders where customer_id=@cid";
            cmd.Parameters.AddWithValue("@cid", customerId);
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

        public void NotEnoughStock(int stockvalue, int productid)
        {

            if (stockvalue > Availablestockquantity( productid))
            {
                throw new ProductNotFoundException("Not Enough stock");
            }
        }
        public void Productnotincart(int productid, int customerid)
        {
           
            if (!ProductNotExistinCart(productid, customerid))
                throw new ProductNotFoundException("Product not found in cart for the customer!!!");
        }

        public bool CheckCustomerCredentials(string email, string password)
        {
            bool isValid = false;

            int count = 0;

            cmd.CommandText = "SELECT COUNT(*) FROM customers WHERE customer_email = @email AND customer_password = @password";
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@password", password);

            connect.Open();
            cmd.Connection = connect;
            count = Convert.ToInt32(cmd.ExecuteScalar());

            // If count is greater than 0, it means the customer with the given name and password exists
            isValid = count > 0;


            cmd.Parameters.Clear();
            connect.Close();

            return isValid;
        }

        public List<Product> ShowProductList()
        {
            List<Product> ProductName = new List<Product>();

            cmd.CommandText = "SELECT product_id,prod_name,prod_price from products ";
            cmd.Parameters.Clear();

            connect.Open();
            cmd.Connection = connect;

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Product product = new Product();          
                product.ProductId = (int)reader["product_id"];
                product.Name = (string)reader["prod_name"];
                product.Price = Convert.ToInt32(reader["prod_price"]);
                ProductName.Add(product);
            }

            connect.Close();

            return ProductName;

        }
        public int CurrentLoggedInCId(string email, string password)
        {
            int customerID = 0;
            cmd.CommandText = "Select customer_id as CustomerID from customers where customer_email=@email and customer_password=@password";

            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@password", password);
            connect.Open();
            cmd.Connection = connect;
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                cmd.Parameters.Clear();
                customerID = (int)reader["CustomerID"];
            }
            connect.Close();
            return customerID;
        }

        public int GetTotalPriceOfCustomerId(int customerId)
        {
            int totalPrice = 0;
            cmd.CommandText = "SELECT SUM(total_price) as Total FROM orders WHERE customer_id = @customerId";

            cmd.Parameters.AddWithValue("@customerId", customerId);
            connect.Open();
            cmd.Connection = connect;
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                cmd.Parameters.Clear();
                totalPrice = (int)reader["Total"];
            }
            connect.Close();
            return totalPrice;
        }
        public Product GetProductInfo(int productId)
        {
            Product product = null;

            try
            {
                using (SqlConnection connect = new SqlConnection(DataConnectionUtility.GetConnectionString()))
                using (SqlCommand cmd = connect.CreateCommand())
                {

                    if (connect.State != ConnectionState.Open)
                        connect.Open();
                    cmd.CommandText = "SELECT * FROM products WHERE product_id = @productId";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@productId", productId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        if (reader.Read())
                        {
                            product = new Product
                            {
                                ProductId = (int)reader["product_id"],
                                Name = reader["prod_name"].ToString(),
                                Price = (int)reader["prod_price"],
                                Description = reader["prod_description"].ToString(),
                                StockQuantity = (int)reader["stock_Quantity"]
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching product details: " + ex.Message);
            }

            // Return the fetched product (or null if not found)
            return product;
        }
    }
}
