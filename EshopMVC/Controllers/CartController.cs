﻿using EshopMVC.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EshopMVC.Controllers
{
    public class CartController : Controller
    {
        public CartController()
        {
            UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
        }

        public UserManager<ApplicationUser> UserManager { get; private set; }

        //
        // GET: /Cart/
        public ActionResult Index()
        {
            ShoppingCart cart;
            using (var dbCtx = new DB_9FCCB1_eshopEntities())
            {
                if (User.Identity.IsAuthenticated)
                {
                    ApplicationUser user = UserManager.FindByName(User.Identity.Name);

                    cart = dbCtx.ShoppingCart.FirstOrDefault(c => c.UserId == user.Id);
                }
                else
                {
                    cart = (ShoppingCart)Session["Cart"];
                }

                if (cart == null || cart.CartItem.Count < 1)
                {
                    return View("Empty");
                }
                return View(cart.CartItem);
            }
        }

        public void Add(int id, string title, decimal price, int quantity)
        {
            var cartItem = new CartItem { Id = id, Title = title, Price = price, Quantity = quantity };
            if (User.Identity.IsAuthenticated)
            {
                ApplicationUser user = UserManager.FindByName(User.Identity.Name);
                using (var dbCtx = new DB_9FCCB1_eshopEntities())
                {
                    var dbCart = dbCtx.ShoppingCart.FirstOrDefault(c => c.UserId == user.Id);
                    if (dbCart == null)
                    {
                        dbCart = new ShoppingCart() { UserId = user.Id };
                        dbCart.AddItem(cartItem);
                        dbCtx.ShoppingCart.Add(dbCart);
                        dbCtx.SaveChanges();
                    }
                    else
                    {
                        dbCart.AddItem(cartItem);
                        dbCtx.SaveChanges();
                    }
                }
            }
            else
            {
                var cart = (ShoppingCart)Session["Cart"];
                if (cart == null)
                {
                    cart = new ShoppingCart();
                    cart.AddItem(cartItem);
                    Session.Add("Cart", cart);
                }
                else
                {
                    cart.AddItem(cartItem);
                }
            }
        }

        public ActionResult Empty()
        {
            if (User.Identity.IsAuthenticated)
            {
                ApplicationUser user = UserManager.FindByName(User.Identity.Name);
                using (var dbCtx = new DB_9FCCB1_eshopEntities())
                {
                    var dbCart = dbCtx.ShoppingCart.FirstOrDefault(c => c.UserId == user.Id);
                    if (dbCart == null)
                    {
                        throw new Exception("Should not get here."); //TODO
                    }
                    else
                    {
                        dbCtx.CartItem.RemoveRange(dbCart.CartItem);
                        dbCart.CartItem.Clear();
                        dbCtx.SaveChanges();
                    }
                }
            }
            else
            {
                var cart = (ShoppingCart)Session["Cart"];
                if (cart == null)
                {
                    throw new Exception("Should not get here."); //TODO
                }
                else
                {
                    cart.CartItem.Clear();
                }
            }
            return View();
        }

        //private ShoppingCart GetCart()
        //{
        //    if (User.Identity.IsAuthenticated)
        //    { }
        //    else
        //    {

        //    }
        //}
    }
}