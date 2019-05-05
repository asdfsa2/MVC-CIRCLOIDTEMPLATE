using MVC_CircloidTemplate.App_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MVC_CircloidTemplate.Controllers
{
    public class UserAccountController : Controller
    {
       public UserAccountController()
        {
            ViewBag.UserSelected = "selected";
        }

        // GET: UserAccount
        public ActionResult Index()
        {
            MembershipUserCollection userCollection= Membership.GetAllUsers();

            return View(userCollection);
        }
        public ActionResult AddUserAccount()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult AddUserAccount(UserClass uc)  //UserClass'ı biz oluşturduk.Bizim oluşturduğumuz entity.
        {

            MembershipCreateStatus createStatus;
            Membership.CreateUser(uc.UserName, uc.Password, uc.Email, uc.PasswordQuestion, uc.PasswordAnswer, true, out createStatus);
            string messageStatus = "";
  
            switch (createStatus)
            {

                case MembershipCreateStatus.Success:
                   
                    break;
                case MembershipCreateStatus.InvalidUserName:
                    messageStatus = "Geçersiz kullanıcı adı";
                    break;
                case MembershipCreateStatus.InvalidPassword:
                    messageStatus = "Geçersiz parola";
                    break;
                case MembershipCreateStatus.InvalidQuestion:
                    messageStatus = "Geçersiz gizli soru";
                    break;
                case MembershipCreateStatus.InvalidAnswer:
                    messageStatus = "Geçersiz gizli cevap";
                    break;
                case MembershipCreateStatus.InvalidEmail:
                    messageStatus = "Geçersiz email";
                    break;
                case MembershipCreateStatus.DuplicateUserName:
                    messageStatus = "Kullanılmış kullanıcı adı";
                    break;
                case MembershipCreateStatus.DuplicateEmail:
                    messageStatus = "Kullanılmış email";
                    break;
                case MembershipCreateStatus.UserRejected:
                    messageStatus = "Engellenmiş kullanıcı";
                    break;
                case MembershipCreateStatus.InvalidProviderUserKey:
                    messageStatus = "Geçersiz kullanıcı anahtarı";
                    break;
                case MembershipCreateStatus.DuplicateProviderUserKey:
                    messageStatus = "Kullanılmış kullanıcı anahtarı";
                    break;
                case MembershipCreateStatus.ProviderError:
                    messageStatus = "Üye yönetimi sağlayıcı hatası";
                    break;
                default:
                    break;
            }
            ViewBag.messageStatus = messageStatus;
            if (createStatus== MembershipCreateStatus.Success)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
            
        }

        [HttpPost]
        public string DeleteUserAccount(string UserName)
        {                //Silme işlemi AJAX kodlarıyla yapıldı. Silme işlemi esnasında farklı bir sayfa açmadığımız için
                           //Bu metot için View açmadık.*** Bu metot sadece idyi(UserName) yakalamak için***
            try
            {
                Membership.DeleteUser(UserName);

                return "OK";
            }
            catch (Exception)
            {

                return "ERROR";
            }
        }
        public ActionResult AssignRole(string username,string message=null)
        {
            ////******Parametre olarak id yazmak zorundayız. sebebi projenin App_Start klasörünün altında Route.config.cs
            ////dosyasında "{controller}/{action}/{id}" bu parametre adının defoult adı id olduğu için parametre adının 
            ////da id olması gerekiyor.

            ////*****Kullanıcı Rolata'ya tıkladığında  kullanıcı adını parametre olarak buraya alıyoruz. Buradan da kullanıcının
            ////adını Viewa gönderiyoruz. Amacımız parametre bilgisini viewa taşımak. View tarafında ekle butonuna basınca
            ////tekrar kullanıcı adını ve rol adını viewdan alıp Post tarafına taşımak 

            if (string.IsNullOrWhiteSpace(username))
            {
                return RedirectToAction("Index");
            }
            MembershipUser user = Membership.GetUser(username);

            if (user==null)
            {
                return HttpNotFound();
            }

            string[] userRoles = Roles.GetRolesForUser(username); //Her kullanıcı için aldığı roller için bir string tipli dizi
            string[] allRoles = Roles.GetAllRoles(); //Varolan bütün roller için string tipli bir dizi

            List<string> availableRoles = new List<string>();
            //Kullanıcı için roller eklendiğinde bütün rollerin olduğu listbox içinde kullanıcıya atanan rollerin görünmesini
            //istemiyoruz.Sebebi bir kullanıcıya aynı iki rolü atamamak. Bu yüzden availableRoles adında bir List oluşturduk.
            //Ve bu listte varolan bütün rollerden kullanıcıya eklenen rolleri çıkartıp kalan rolleri gösterdik.
            foreach (string role in allRoles)
            //allRoles dizisi içinde gez. Eğer role userRoles(kullanıcının aldığı roller) içinde yoksa availableRoles dizisine ekle.
            {
                if (!userRoles.Contains(role))
                {
                    availableRoles.Add(role);
                }
            }

            ViewBag.AvailableRoles = availableRoles; //Bunu sağdaki listbox kutusunda göstermek için
            ViewBag.UserRoles = userRoles;//Bunu soldaki listbox kutusu içinde göstermek için
            ViewBag.UserName = username;
            ViewBag.Message = message;

            return View();
        }
        [HttpPost]
        public ActionResult AssignRole(string username, List<string> addedRoles)
        {
            if (addedRoles == null)
                return RedirectToAction("AssignRole", new { username = username, message = "Önce rol seçiniz" });

            if(addedRoles.Count<1)
                return RedirectToAction("AssignRole", new { username = username, message = "Hata" });

            Roles.AddUserToRoles(username, addedRoles.ToArray());
            return RedirectToAction("AssignRole", new { username = username, message = "Başarılı" });

        }

        [HttpPost]
        public string DeleteRole(string username, string removedRoles)
        {
            string[] removeRolesArray = removedRoles.Split(',');
            if (removeRolesArray.Length < 1 || string.IsNullOrWhiteSpace(removeRolesArray[0]))
                return "hata";
            Roles.RemoveUserFromRoles(username, removeRolesArray);
            return "Başarılı";
        }
    }
}