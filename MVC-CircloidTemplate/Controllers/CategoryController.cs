using MVC_CircloidTemplate.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_CircloidTemplate.Controllers
{
    public class CategoryController : Controller
    {
        NorthwindEntities1 ctx = new NorthwindEntities1();
        // GET: Category
        public ActionResult Index()
        {
            List<Category> ctgList = ctx.Categories.ToList();
            return View(ctgList);
        }

        public ActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddCategory([Bind(Include="CategoryName, Description")] Category ktg, HttpPostedFileBase Picture)
        {
            if (Picture == null)
                return View();

            ktg.Picture = ConvertToBytes(Picture);
            
            if (ModelState.IsValid)//database bağlantısı doğru oluştu mu? Oluştuysa şart true, değilse false 
            {
                ctx.Categories.Add(ktg);
                ctx.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        //Category Picture nesnesi DataBase'de byte[] şeklinde tutulduğu için seçilen resmi byte[] array'e çevrilmesini sağlayan method.
        public byte[] ConvertToBytes(HttpPostedFileBase image)
        {
            byte[] imageBytes = null;
            BinaryReader reader = new BinaryReader(image.InputStream);
            imageBytes = reader.ReadBytes(image.ContentLength);
            byte[] bytes = new byte[imageBytes.Length + 78];
            Array.Copy(imageBytes, 0, bytes, 78, imageBytes.Length);
            return bytes;
        }
        //public ActionResult DeleteCategory(int catID)
        //{
        //    Category cat = ctx.Categories.FirstOrDefault(x => x.CategoryID == catID);  Silme işlemi 
        //    return View(cat);
        //}


        //[HttpPost]
        //public ActionResult DeleteCategory(Category cat)
        //{                                                  Sorarak silme işlemi için
        //    Category ktg = ctx.Categories.FirstOrDefault(x => x.CategoryID == cat.CategoryID);
        //    ctx.Categories.Remove(ktg);
        //    ctx.SaveChanges();
        //    return RedirectToAction("Index");  /*Silme işleminden sonra Index View'ını çalıştır'*/
        //}
        [HttpPost]
        public ActionResult DeleteCategory(int ID)
        {                                              /*Ajax ile silme için*/
            Category ktg = ctx.Categories.FirstOrDefault(x => x.CategoryID == ID);
            ctx.Categories.Remove(ktg);
            ctx.SaveChanges();
            return RedirectToAction("Index");  /*Silme işleminden sonra Index View'ını çalıştır'*/
        }
        public ActionResult UpdateCategory(int catID)
        {
            Category cat = ctx.Categories.FirstOrDefault(x => x.CategoryID == catID);
            return View(cat);
        }
        [HttpPost]
        public ActionResult UpdateCategory([Bind(Include = "CategoryID, CategoryName, Description")] Category ktg, HttpPostedFileBase Picture)
        {
            if (ModelState.IsValid)
            {
                Category c = ctx.Categories.FirstOrDefault(x => x.CategoryID == ktg.CategoryID);

                ktg.CategoryName = c.CategoryName;
                ktg.Description = c.Description;
                if (Picture != null)
                {
                    c.Picture = ConvertToBytes(Picture);
                }
                ctx.SaveChanges();
            }
            
            return RedirectToAction("Index");
          
            
        }
    }
}