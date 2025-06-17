using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlexanderShemarov.Domain.Entities
{
    public class Cart
    {
        public int Id { get; set; }
        
        /// <summary> 
        /// Objects List in Cart 
        /// key - Object's ID
        /// </summary> 
        public Dictionary<int, CartItem> CartItems { get; set; } = new();
        
        /// <summary> 
        /// Adding an Object into a Cart
        /// </summary> 
        /// <param name="train">Train Adding</param> 
        public virtual void AddToCart(Trains train)
        {
            if (CartItems.ContainsKey(train.ID))
            {
                CartItems[train.ID].Qty++;
            }
            else
            {
                CartItems.Add(train.ID, new CartItem
                {
                    trainItem = train,
                    Qty = 1
                });
            };
        }
        
        /// <summary> 
        /// Removing an Object from a Cart
        /// </summary> 
        /// <param name="train">Train Removing</param> 
        public virtual void RemoveItems(int id)
        {
            CartItems.Remove(id);
        }
        
        /// <summary> 
        /// Cart Clearing
        /// </summary> 
        public virtual void ClearAll()
        {
            CartItems.Clear();
        }
        
        /// <summary> 
        /// Objects Amount in a Cart
        /// </summary> 
        public int Count { get => CartItems.Sum(item => item.Value.Qty); }
        
        /// <summary> 
        /// Common Price
        /// </summary> 
        public decimal TotalPrice
        {
            get => CartItems.Sum(item => item.Value.trainItem.Cost * item.Value.Qty);
        }
    }
}
