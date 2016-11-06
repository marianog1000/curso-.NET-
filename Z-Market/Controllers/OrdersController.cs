using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Z_Market.Models;
using Z_Market.ViewModels;

namespace Z_Market.Controllers
{
    public class OrdersController : Controller
    {
        Z_MarketContext db = new Z_MarketContext();
        public ActionResult NewOrder()
        {
            var orderView = new OrderView();
            orderView.Customer = new Customers();
            orderView.Products = new List<ProductOrder>();
            Session["orderView"] = orderView;

            var lista = db.Customers.ToList();
            lista.Add(new Customers { CustomerID = 0, FirstName = "[Seleccione un Cliente...]" });
            lista = lista.OrderBy(c => c.FullName).ToList();
            ViewBag.CustomerID = new SelectList(lista, "CustomerID", "FullName");

            return View(orderView);
        }

        [HttpPost]
        public ActionResult NewOrder(OrderView orderView)
        {
            orderView = Session["orderView"] as OrderView;
            var customerId = int.Parse(Request["CustomerID"]);

            if (customerId == 0)
            {
                var list = db.Customers.ToList();
                list.Add(new Customers { CustomerID = 0, FirstName = "[Seleccione un Cliente...]" });
                list = list.OrderBy(c => c.FirstName).ToList();
                ViewBag.CustomerID = new SelectList(list, "CustomerID", "FullName");
                ViewBag.Error = "Debe seleccionar un Cliente";
                return View(orderView);
            }

            var customer = db.Customers.Find(customerId);
            if (customer == null)
            {
                var list = db.Customers.ToList();
                list.Add(new Customers { CustomerID = 0, FirstName = "[Seleccione un Cliente...]" });
                list = list.OrderBy(c => c.FirstName).ToList();
                ViewBag.CustomerID = new SelectList(list, "CustomerID", "FullName");
                ViewBag.Error = "Cliente no Existe";
                return View(orderView);
            }

            if (orderView.Products.Count == 0)
            {
                var list = db.Customers.ToList();
                list.Add(new Customers { CustomerID = 0, FirstName = "[Seleccione un Cliente...]" });
                list = list.OrderBy(c => c.FirstName).ToList();
                ViewBag.CustomerID = new SelectList(list, "CustomerID", "FullName");
                ViewBag.Error = "Debe ingresar un Detalle";
                return View(orderView);

            }

            int orderID = 0;

            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var order = new Order
                    {
                        CustomerId = customerId,
                        DateOrder = DateTime.Now,
                        OrderStatus = OrderStatus.Created
                    };

                    db.Orders.Add(order);
                    db.SaveChanges();

                    orderID = db.Orders.ToList().Select(o => o.OrderID).Max();
                    foreach (var item in orderView.Products)
                    {
                        var orderDetail = new OrderDetail
                        {
                            Descripcion = item.Descripcion,
                            Price = item.Price,
                            Quantity = item.Quantity,
                            OrderID = orderID,
                            ProductID = item.ProductID
                        };
                        db.OrderDetails.Add(orderDetail);
                        db.SaveChanges();
                    }
                    transaction.Commit();
                }
                catch (Exception ex) {
                    transaction.Rollback();
                    ViewBag.Error = "ERROR: " + ex.Message;

                    var listCu = db.Customers.ToList();
                    listCu.Add(new Customers { CustomerID = 0, FirstName = "[Seleccione un Cliente...]" });
                    listCu = listCu.OrderBy(c => c.FirstName).ToList();
                    ViewBag.CustomerID = new SelectList(listCu, "CustomerID", "FullName");

                    return View(orderView);
                }
                
            }
            ViewBag.Message = string.Format("La Orden {0} fue grabada correctamente...",orderID);

            var listC = db.Customers.ToList();
            listC.Add(new Customers { CustomerID = 0, FirstName = "[Seleccione un Cliente...]" });
            listC = listC.OrderBy(c => c.FirstName).ToList();
            ViewBag.CustomerID = new SelectList(listC, "CustomerID", "FullName");

            orderView = new OrderView();
            orderView.Customer = new Customers();
            orderView.Products = new List<ProductOrder>();
            Session["orderView"] = orderView;

            return View(orderView);
        }

        public ActionResult AddProduct()
        {
            var lista = db.Products.ToList();
            lista.Add(new Product { ProductID = 0, Descripcion = "[Seleccione un Producto...]" });
            lista = lista.OrderBy(c => c.Descripcion).ToList();
            // para ordernar los combos o listas
            ViewBag.ProductID = new SelectList(lista, "ProductID", "Descripcion");

            return View();

        }

        [HttpPost]
        public ActionResult AddProduct(ProductOrder productOrder)
        {
            var orderView = Session["orderView"] as OrderView;
            var productID = int.Parse(Request["ProductID"]);

            if (productID == 0) {
                var lista = db.Products.ToList();
                lista.Add(new Product { ProductID = 0, Descripcion = "[Seleccione un Producto...]" });
                lista = lista.OrderBy(c => c.Descripcion).ToList();
                ViewBag.ProductID = new SelectList(lista, "ProductID", "Descripcion");
                ViewBag.Error = "Debe seleccionar un Producto";
                return View(productOrder);
            }

            var product = db.Products.Find(productID);
            if (product == null)
            {
                var lista = db.Products.ToList();
                lista.Add(new Product { ProductID = 0, Descripcion = "[Seleccione un Producto...]" });
                lista = lista.OrderBy(c => c.Descripcion).ToList();
                ViewBag.ProductID = new SelectList(lista, "ProductID", "Descripcion");
                ViewBag.Error = "Producto no Existe.";
                return View(productOrder);
            }

            productOrder = orderView.Products.Find(p => p.ProductID == productID);

            if (productOrder == null)
            {
                productOrder = new ProductOrder
                {
                    Descripcion = product.Descripcion,
                    Price = product.Price,
                    ProductID = product.ProductID,
                    Quantity = float.Parse(Request["Quantity"])
                };
                orderView.Products.Add(productOrder);
            }
            else {
                productOrder.Quantity += float.Parse(Request["Quantity"]);
            }

            var listC = db.Customers.ToList();
            listC.Add(new Customers { CustomerID = 0, FirstName = "[Seleccione un Cliente...]" });
            listC = listC.OrderBy(c => c.FullName).ToList();
            ViewBag.CustomerID = new SelectList(listC, "CustomerID", "FullName");

            return View("NewOrder", orderView);
        }

        // cierra la bd para que no queden conexiones abiertas
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}