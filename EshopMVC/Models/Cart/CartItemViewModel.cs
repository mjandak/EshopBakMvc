﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EshopMVC.Models.Cart
{
    public class CartItemViewModel
    {
        public CartItemViewModel()
        {

        }

        public CartItemViewModel(CartItem cartItem)
        {
            ProductId = cartItem.Id;
            Title = cartItem.Title;
            Price = cartItem.Price;
            Quantity = cartItem.Quantity;
        }

        public int ProductId
        {
            get;
            set;
        }

        public string Title 
        {
            get;
            set;
        }

        public decimal Price 
        {
            get;
            set;
        }

        public int Quantity 
        {
            get;
            set;
        }
    }
}