using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QA1_lesson_3.Models;
using System.Diagnostics;
using System.Drawing;

namespace QA1_lesson_3.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
                return Redirect("login");
            else
            {
                var model = new
                {
                    username = HttpContext.Session.GetString("Username"),
                    fullname = HttpContext.Session.GetString("Fullname")
                };
                return View(model);
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult get_course(int Page, int Size, string Group)
        {
            var data = getCourse(Page, Size, Group);
            if (data != null)
            {
                var res = new
                {
                    Success = true,
                    Message = "",
                    Data = data
                };
                return Json(res);
            }
            else
            {
                var res = new
                {
                    Success = false,
                    Message = "Error!!!"
                };
                return Json(res);
            }
        }
        [HttpPost]
        public IActionResult update_course(Course course)
        {
            var c = updateCourse(course);
            if (c != null)
            {
                var res = new
                {
                    Success = true,
                    Message = "",
                    Data = c
                };
                return Json(res);
            }
            else
            {
                var res = new
                {
                    Success = false,
                    Message = "ERROR",
                };
                return Json(res);
            }
        }
        public IActionResult insert_course(Course course)
        {
            var c = insertCourse(course);
            if (c != null)
            {
                var res = new
                {
                    Success = true,
                    Message = "",
                    Data = c
                };
                return Json(res);
            }
            else
            {
                var res = new
                {
                    Success = false,
                    Message = "ERROR",
                };
                return Json(res);
            }
        }
        public IActionResult delete_course(Course course)
        {
            var c = deleteCourse(course);
            if (c != null )
            {
                var res = new
                {
                    Success = c,
                    Message = "",
                };
                return Json(res);
            }
            else
            {
                var res = new
                {
                    Success = false,
                    Message = "ERROR",
                };
                return Json(res);
            }
        }
        ////
        ////
        private object? getCourse(int page, int size, string grp)
        {

            try
            {
                var db = new LtwebContext();
                var ls = db.Courses.Where(x => x.Group == grp);
                var offset = (page - 1) * size;
                var totalRecord = ls.Count();
                var totalPage = (totalRecord % size) == 0 ?
                    (int)(totalRecord / size) :
                    (int)(totalRecord / size + 1);
                var lst = ls.Skip(offset).Take(size).ToList();
                return new
                {
                    Data = lst,
                    TotalRecord = totalRecord,
                    TotalPage = totalPage,
                    Page = page,
                    Size = size,
                };

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        private object? updateCourse(Course c)
        {
            try
            {
                if (c == null)
                    return null;
                var db = new LtwebContext();
                var cl = db.Courses.Where(x=> x.Id == c.Id).FirstOrDefault();
                if(cl.Group != c.Group) 
                {
                    cl.Group = c.Group;
                }
                if (cl.Major != c.Major)
                {
                    cl.Major = c.Major;
                }
                if (cl.Credit != c.Credit)
                {
                    cl.Credit = c.Credit;
                }
                if (cl.Note != c.Note)
                {
                    cl.Note = c.Note;
                }
                if (cl.CourseName != c.CourseName)
                {
                    cl.CourseName = c.CourseName;
                }
                if (cl.SubCode != c.SubCode)
                {
                    cl.SubCode = c.SubCode;
                }
                db.Courses.Update(cl);
                db.SaveChanges();
                return cl;
            }
            catch(Exception ex)
            {
                return null;
            }
        }
        private object? insertCourse(Course c)
        {
            try
            {
                if (c == null)
                    return null;
                var db = new LtwebContext();
                var cl = new Course();
                if (cl.Group != c.Group)
                {
                    cl.Group = c.Group;
                }
                if (cl.Major != c.Major)
                {
                    cl.Major = c.Major;
                }
                if (cl.Credit != c.Credit)
                {
                    cl.Credit = c.Credit;
                }
                if (cl.Note != c.Note)
                {
                    cl.Note = c.Note;
                }
                if (cl.CourseName != c.CourseName)
                {
                    cl.CourseName = c.CourseName;
                }
                if (cl.SubCode != c.SubCode)
                {
                    cl.SubCode = c.SubCode;
                }
                db.Courses.Update(cl);
                db.SaveChanges();
                return cl;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        private Boolean? deleteCourse(Course c)
        {
            try
            {
                if (c == null)
                {
                    return null;
                }
                var db = new LtwebContext();
                Course cl =  db.Courses.Where(x => x.Id == c.Id).FirstOrDefault();
                if (cl!=null)
                {
                    db.Courses.Remove(cl);
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
                
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
