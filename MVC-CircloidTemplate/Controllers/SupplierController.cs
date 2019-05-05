using MVC_CircloidTemplate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_CircloidTemplate.Controllers
{
    public class SupplierController : Controller
    {
        NorthwindEntities1 ctx = new NorthwindEntities1();
        // GET: Supplier
        public ActionResult Index()
        {
            List<Supplier> supList = ctx.Suppliers.ToList();
            return View(supList);
        }
        public ActionResult AddSupplier()
        {
            ViewBag.supList = ctx.Suppliers.ToList();
            return View();
        }
        [HttpPost]
        public ActionResult AddSupplier(Supplier sup)
        {
            ctx.Suppliers.Add(sup);
            ctx.SaveChanges();
            return RedirectToAction("Index");  /*ürün ekledikten sonra Index actionını çağırıp listeleme yapacak*/
        }
        // ***************************************************1. yol silme işlemi *********************
        //public ActionResult DeleteSupplier(int supID) Burası normal silme için gerekli. Normal silme de farklı bir sayfa açıp "ürünü silmek istiyormusunuz?"
        //{                                             şeklinde bir soru sorulacağından burası için view oluşturmuştuk.
        //    Supplier sup = ctx.Suppliers.FirstOrDefault(x => x.SupplierID == supID);
        //    return View(sup);
        //}

        [HttpPost]
        public string DeleteSupplier(int id) //Burda view oluşturmadık çünkü silme işlemi esnasında farklı bir sayfa açmıyor. Sadece bir modal üzerinde 
        {                                    //"tedarikçiyi silmek istiyormusunuz" diye soruyor.
            try
            {
                Supplier s = ctx.Suppliers.Find(id);
                ctx.Suppliers.Remove(s);
                ctx.SaveChanges();

                return "OK";
            }
            catch (Exception)
            {
                return "ERROR";
            }
        }
        //[HttpPost]    *****silme işlemi 1. yol post için*****
        //public ActionResult DeleteSupplier(Supplier sp)
        //{
        //    Supplier sup = ctx.Suppliers.FirstOrDefault(x => x.SupplierID == sp.SupplierID);
        //    ctx.Suppliers.Remove(sup);
        //    ctx.SaveChanges();
        //    return RedirectToAction("Index");
        //}
        public ActionResult UpdateSupplier(int supID)
        {
            ViewBag.ctgList = ctx.Categories.ToList();
            ViewBag.supList = ctx.Suppliers.ToList();
            Supplier sup = ctx.Suppliers.FirstOrDefault(x => x.SupplierID == supID);
            return View(sup);
        }
        [HttpPost]
        public ActionResult UpdateSupplier([Bind(Include = "SupplierID, CompanyName, ContactName, ContactTitle, Address, City, Region, PostalCode, Country, Phone")] Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                ctx.Entry(supplier).State = System.Data.Entity.EntityState.Modified;
                ctx.SaveChanges();
                return RedirectToAction("Index");
            }

            return RedirectToAction("UpdateSupplier", new { id = supplier.SupplierID });
        }
        //******************************BU UPDATE İÇİN İKİNCİ YOL***********************
        //[HttpPost]        
        //public ActionResult UpdateSupplier(Supplier sup)
        //{
        //    Supplier s = ctx.Suppliers.FirstOrDefault(x => x.SupplierID == sup.SupplierID);
        //    s.CompanyName = sup.CompanyName;
        //    //p.Supplier.CompanyName = prd.Supplier.CompanyName;
        //    //p.Category.CategoryName = prd.Category.CategoryName;
        //    s.ContactName = sup.ContactName;
        //    s.ContactTitle = sup.ContactTitle;
        //    s.Address = sup.Address;
        //    s.City = sup.City;
        //    s.Region = sup.Region;
        //    s.PostalCode = sup.PostalCode;
        //    s.Country = sup.Country;
        //    s.Phone = sup.Phone;
        //    s.Fax = sup.Fax;
        //    ctx.SaveChanges();
        //    return RedirectToAction("Index");
        //}
        //******************************************************************
    }
}