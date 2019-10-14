using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcMusicStoreFall2018.Models
{
    public class ShoppingCart
    {
        MusicStoreDB db = new MusicStoreDB();

        public string ShoppingCartId { get; set; }

        public const string CartSessionKey = "CartId";

        public static ShoppingCart GetCart(HttpContextBase context)
        {
            var cart = new ShoppingCart();

            cart.ShoppingCartId = cart.GetCartId(context);

            return cart;
        }


        public string GetCartId(HttpContextBase context)
        {
            if( context.Session[CartSessionKey] == null)//they dont have a cart id
            {
                if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    context.Session[CartSessionKey] = context.User.Identity.Name;

                    
                }
                else
                {
                    Guid tempCartId = Guid.NewGuid();//creates a globally unique identifier

                    context.Session[CartSessionKey] = tempCartId;//writes the CartId on user's machine

                }

            }

            return context.Session[CartSessionKey].ToString();


        }

        //migrate the cart to the username
        public void MigrateCart(string username)
        {
            //retrieve all cart items for the current session's cartid
            var cartItems = GetCartItems();


            //iterate through each cart item and change the cartid to username
            foreach(var cartItem in cartItems)
            {
                cartItem.CartId = username;
            }



            db.SaveChanges();




        }







        public void AddToCart(Album album)
        {
            var cartItem = db.Carts.SingleOrDefault(
                cart => cart.CartId == ShoppingCartId
                && cart.AlbumId == album.AlbumId);//fetches the cart row that may have the user cart and the album already


            if(cartItem == null) //if no cart exists with that album, a new row is created with the session key and the specific album
            {
                cartItem = new Cart
                {
                    CartId = ShoppingCartId,
                    AlbumId = album.AlbumId,
                    Count = 1,
                    DateCreated = DateTime.Now

                };

                db.Carts.Add(cartItem);
            }

            else // if the album already exists in the user's cart, just add to count
            {
                cartItem.Count++;
            }

            db.SaveChanges();

        }

        public int RemoveFromCart(int id)
        {
            var cartItem = db.Carts.Single(
                cart => cart.CartId == ShoppingCartId
                && cart.RecordId == id);


            int itemCount = 0;

            if(cartItem != null)
            {
                if(cartItem.Count > 1)
                {
                    cartItem.Count--;
                    itemCount = cartItem.Count;
                }
                else
                {
                    db.Carts.Remove(cartItem);

                }

                db.SaveChanges();


            }

            return itemCount;
        }

        public List<Cart> GetCartItems()
        {

            return db.Carts.Where(cart => cart.CartId == ShoppingCartId).ToList();

        }

        public void EmptyCart()
        {
            var cartItems = db.Carts.Where(cart => cart.CartId == ShoppingCartId);

            foreach(var cartItem in cartItems)
            {
                db.Carts.Remove(cartItem);


            }


            db.SaveChanges();

        }

        public int GetCount()// get the number of items in our cart
        {
            int? count = (from cartItems in db.Carts
                          where cartItems.CartId == ShoppingCartId
                          select (int?)cartItems.Count).Sum();


            return count ?? 0;


        }


        public decimal GetTotal()//get the cart total 
        {
            decimal? count = (from cartItems in db.Carts
                          where cartItems.CartId == ShoppingCartId
                          select (int?)cartItems.Count * cartItems.Album.Price).Sum();


            return count ?? decimal.Zero;

        }

        //Create order
        public int CreateOrder(Order order)
        {
            //retrieve all items from cart

            var cartItems = GetCartItems();


            decimal orderTotal = 0;

            //iterate through each cart item and convert that item into an OrderDetail. Update order total.

            foreach(var cartItem in cartItems)
            {
                OrderDetail orderDetail = new OrderDetail
                {
                    OrderId = order.OrderId,
                    AlbumId = cartItem.AlbumId,
                    Count = cartItem.Count,
                    UnitPrice = cartItem.Album.Price

                };

                db.OrderDetails.Add(orderDetail);

                orderTotal += (cartItem.Count * cartItem.Album.Price);
            }

            order.OrderTotal = orderTotal;

            EmptyCart();

            db.SaveChanges();
           


            return order.OrderId;

        }




    }
}