using MVC_CircloidTemplate.App_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MVC_CircloidTemplate.Controllers
{
    public class MemberController : Controller
    {
        // GET: Member
        public ActionResult LoginToSystem()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LoginToSystem(UserClass uc,string RememberMe )
        {
            bool validateResult = Membership.ValidateUser(uc.UserName,uc.Password);
            if (validateResult == true)
            {
                if (RememberMe == "on")
                {
                    FormsAuthentication.RedirectFromLoginPage(uc.UserName, true);
                }
                else
                {
                    FormsAuthentication.RedirectFromLoginPage(uc.UserName, false);
                }
            }
            else
            {
                ViewBag.validateMessage = "Kullanıcı adı ya da parola hatalı";
            }
            ViewBag.nameu = uc.UserName;
            return View();
        }
        public ActionResult LogoutToSystem()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("LoginToSystem");
        }
        public ActionResult ResetPassword(string UserName)
        {
            UserClass uc = new UserClass();
            uc.UserName = UserName;
            return View(uc);
        }
        [HttpPost]
        public ActionResult ResetPassword(UserClass uc)
        {
            uc.UserName = ViewBag.nameu;
            MembershipUser mu = Membership.GetUser(uc.UserName);
 
            if (mu.PasswordQuestion == uc.PasswordQuestion)
            {
                string newPwd = mu.ResetPassword(uc.PasswordAnswer);
                if (newPwd != null)
                {
                    mu.ChangePassword(newPwd,uc.Password);
                }
            }
            else
            {
                ViewBag.resetPasswordMessage = "Girilen bilgiler yanlıştır";
            }
            return View();
        }
        public ActionResult CreateMemberAccount()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateMemberAccount(UserClass uc,string passwordConfirm)
        {
            if (uc.Password != passwordConfirm)
            {
                ViewBag.messageStatus = "Lütfen parolayı tekrar giriniz";
                return View();
            }
            MembershipCreateStatus createStatus;
            Membership.CreateUser(uc.UserName,  uc.Password, uc.Email, uc.PasswordQuestion, uc.PasswordAnswer, true, out createStatus);
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
            if (createStatus == MembershipCreateStatus.Success)
            {
                return RedirectToAction("LoginToSystem");
            }
            else
            {
                return View();
            }
        }

    }
}