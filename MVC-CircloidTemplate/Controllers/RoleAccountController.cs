using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MVC_CircloidTemplate.Controllers
{
    public class RoleAccountController : Controller
    {
        // GET: RoleAccount
        public ActionResult Index()
        {
            //Sistemde tanımlı bütün rolleri çekmek için
            List<string> rolesList=Roles.GetAllRoles().ToList();

            return View(rolesList);
        }
        public ActionResult AddRoleAccount(string message=null)
        { //string message=null=> eğer hiç birşey girilmezse değeri null olarak al. string message=Ahmet dersek değeri Ahmet alır.

            ViewBag.Message = message;
            return View();
        }
        //İmzası aynı olan(Burada olduğu gibi parametre tipleri  aynı) iki action tanımlanabilir. Bu durumda birine
        //(burada post action)  [ActionName("AddRoleAccount")] tanımlarsak ismi "AddRoleAccountPost" olsa bile program ismini 
        //"AddRoleAccount" alacaktır. Tıpkı Data Annotationslarda ismi Çalışanlar olan tabloya [Table("Personel")] tanımladığımızda
        //SQL'deki tablo adı "Personel" olduğu gibi

        [HttpPost]                      
        [ActionName("AddRoleAccount")] 
        public ActionResult AddRoleAccountPost(string RoleName)
        {
            if (string.IsNullOrWhiteSpace(RoleName)) //eğer birşey girilmediyse 
            {
                return RedirectToAction("AddRoleAccount", new { message = "Rol boş olamaz." });
            }
            if (Roles.RoleExists(RoleName)) //Varolan bir Role girildiyse
            {
                return RedirectToAction("AddRoleAccount", new { message = "Rol zaten kayıtlı" });
            }
            
            Roles.CreateRole(RoleName);
            return RedirectToAction("AddRoleAccount", new { message = "Başarılı" });

        }
        public string DeleteRoleAccount(string RoleName)
        {
            Roles.DeleteRole(RoleName);    //AJAX İLE SİLME YAPTIĞIMIZ için(yeni bir silme sayfası açmıyoruz)
            return "Hata";                 //bu metot için View eklemedik. Burayı sadece indeksi(RoleName) almak için kullandık.
        }
    }
}