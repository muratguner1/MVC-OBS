using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Template.Models;
namespace Template.Controllers
{
    public class HomeController : Controller
    {
        
        OBSEntities obs = new OBSEntities();
        public ActionResult Index(string studentNumber, string error = null)
        {
            ViewBag.Lectures = obs.Lectures.ToList();
            ViewBag.StudentNumber = studentNumber;
            ViewBag.error = error;
            List<Selection> selections = obs.Selections.Where(a=>a.StudentNumber.Equals(studentNumber)).ToList();
            return View(selections);
        }
        public ActionResult Login()
        {
            ViewBag.error = null;
            return View();
        }
        [HttpPost]
        public ActionResult Login(string studentNumber, string password)
        {
            Student loginUser = obs.Students.Where(a => a.StudentNumber.Equals(studentNumber) && a.StudentPassword.Equals(password)).FirstOrDefault();
            if (loginUser != null)
                return RedirectToAction("Index", new { studentNumber  = studentNumber });
            ViewBag.error = "Hatalı giriş";
            return View();
        }
        [HttpPost]
        public ActionResult Add(string StudentNumber, string CourseCode)
        {
            Selection selection = obs.Selections.Where(a => a.StudentNumber.Equals(StudentNumber) && a.CourseCode.Equals(CourseCode)).FirstOrDefault();
            if(selection == null)
            {
                Selection newSelection = new Selection();
                newSelection.CourseCode = CourseCode;
                newSelection.StudentNumber = StudentNumber;
                obs.Selections.Add(newSelection);
                obs.SaveChanges();
                return RedirectToAction("Index", new { studentNumber = StudentNumber });
            }
            return RedirectToAction("Index", new { studentNumber = StudentNumber, error ="This course is already selected" });
        }
        public ActionResult Remove(int SelectionID)
        {
            Selection deleted = obs.Selections.Find(SelectionID);
            string studentNumber = deleted.StudentNumber;
            obs.Selections.Remove(deleted);
            obs.SaveChanges();
            return RedirectToAction("Index", new { studentNumber = studentNumber });
        }
    }
}
