using MVC_CircloidTemplate.App_Classes;
using MVC_CircloidTemplate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_CircloidTemplate.Controllers
{
    public class ProductController : Controller
    {
        NorthwindEntities1 ctx = new NorthwindEntities1();
        // GET: Product
        public ActionResult Index()
        {
            List<Product> prdList = ctx.Products.ToList();


            return View(prdList);
        }

        public ActionResult AddProduct()
        {
            ViewBag.ctgList = ctx.Categories.ToList();
            ViewBag.supList = ctx.Suppliers.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult AddProduct(Product prd)
        {
            ctx.Products.Add(prd);
            ctx.SaveChanges();
            return RedirectToAction("Index");  /*ürün ekledikten sonra Index actionını çağırıp listeleme yapacak*/
        }
        //Index.cshtml sayfasında her ürün satırı için ayrı ayrı bulunan sil butonuna basılınca farklı bir sayfaya yönlendireceğiz ve kullanıcı Evet derse ürünü sileceğiz. Hayır derse listeye geri döneceğiz. silme işlemi için kullanılabilecek 3 yöntemden 1'i budur.(Parametre verme)
        //Diğer Yöntem 1: Sil butonuna basılınca yukarıda alert çıkarılır ve kayıt silinsin mi diye sorulur. Tamam seçilirse silinir. Bu yöntemde sorun bir kaç kez alert kutusu arka arkaya çıkarsa, browser mesajın altına bir checkbox ekler ve bu mesajı bir daha gösterme yazar.Eğer işaretlenirse alert bir daha çıkmaz ve silme işlemi yapılamaz.(AJAX ile kod yazılır.)
        //Diğer Yöntem 2: Sile basınca küçük bir pencere açılır, kullanıcı yeni bir sayfaya yönlendirilmez, aynı sayfada pencere gösterilir, Tamam/İptal soruları sorulur ve Tamam'ı seçerse silme işlemi yapılır.(AJAX ile kod yazılır.)
        public ActionResult DeleteProduct(int prdID)
        {
            Product prd = ctx.Products.FirstOrDefault(x => x.ProductID == prdID);
            return View(prd);
        }

        [HttpPost]
        public ActionResult DeleteProduct(Product prd)
        {
            Product p = ctx.Products.FirstOrDefault(x => x.ProductID == prd.ProductID);
            ctx.Products.Remove(p);
            ctx.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult UpdateProduct()
        {

            int pID = Convert.ToInt32(Request.QueryString["id"]);
            string pName = Request.QueryString["pName"].ToString();
            string pFrom = Request.QueryString["pFrom"].ToString();
            Product prd = ctx.Products.FirstOrDefault(x => x.ProductID == pID);
            ViewBag.ctgList = ctx.Categories.ToList();
            ViewBag.supList = ctx.Suppliers.ToList();


            ViewBag.pFrom = pFrom;

            return View(prd);
        }

        //public ActionResult UpdateProduct(int prdID)
        //{

        //    ViewBag.ctgList = ctx.Categories.ToList();
        //    ViewBag.supList = ctx.Suppliers.ToList();
        //    Product prd = ctx.Products.FirstOrDefault(x => x.ProductID == prdID);
        //    return View(prd);
        //}

        [HttpPost]
        public ActionResult UpdateProduct(Product prd)
        {

            Product p = ctx.Products.FirstOrDefault(x => x.ProductID == prd.ProductID);
            p.ProductName = prd.ProductName;
            p.Category.CategoryName = prd.Category.CategoryName;
            p.Supplier.CompanyName = prd.Supplier.CompanyName;

            p.QuantityPerUnit = prd.QuantityPerUnit;
            p.UnitPrice = prd.UnitPrice;
            p.UnitsInStock = prd.UnitsInStock;
            p.UnitsOnOrder = prd.UnitsOnOrder;
            p.ReorderLevel = prd.ReorderLevel;


            ctx.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public string AddCart(int id)
        {
            UserCart uc;
            if (Session["CurrentCart"] == null)
            {
                uc = new UserCart();

            }
            else
            {
                uc = (UserCart)Session["CurrentCart"];
            }

            string messageAddCart = "";

            foreach (Product prod in uc.PrdList)
            {
                if (id == prod.ProductID)
                {
                    messageAddCart = "Seçmiş olduğunuz ürün sepetinizde mevcut";
                    return messageAddCart;
                }
            }
            Product prd = ctx.Products.FirstOrDefault(x => x.ProductID == id);
            uc.PrdList.Add(prd);
            Session["CurrentCart"] = uc;
            messageAddCart = "Ürün sepete eklendi";
            return messageAddCart;

        }

        public ActionResult MyCart()
        {

            if (Session["CurrentCart"] != null)
            {
                UserCart uc = (UserCart)Session["CurrentCart"];
                return View(uc.PrdList);
            }
            else
            {
                return View();
            }

        }

        [HttpPost]
        public string MyCart(int id)
        {
            string messageRemoveFromCart = "";
            UserCart uc = (UserCart)Session["CurrentCart"];
            foreach (Product prd in uc.PrdList)
            {
                if (prd.ProductID == id)
                {
                    uc.PrdList.Remove(prd);
                    Session["CurrentCart"] = uc;
                    messageRemoveFromCart = "Ürün sepetten çıkarıldı.";
                    break;
                }
            
            messageRemoveFromCart = "hata";
            }
            return messageRemoveFromCart;
        }

        public ActionResult PartialProductView()
        {
            UserCart c = (UserCart)Session["CurrentCart"];
            return PartialView(c.PrdList);
        }
    }
}
