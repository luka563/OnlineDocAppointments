using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using StomatoloskaOrdinacija.Models;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Diagnostics;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Linq;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Http.Extensions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Collections.Generic;
using System;
using System.Security.Cryptography;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using System.Collections;
using System.Drawing;
using Microsoft.CodeAnalysis.Operations;
using System.Globalization;

namespace StomatoloskaOrdinacija.Controllers
{

    public class UserController : Controller
    {
        public zubarContext _context { get; set; }
        public UserController(zubarContext context) { _context = context; }
        [HttpGet]
        //prikaz stranice za registraciju
        public ActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        //pokusaj registrovanja modela
        public ActionResult Registration([Bind] User user)
        {
            bool Status = false;
            string message = "";
            if (ModelState.IsValid)
            {
                var isExist = IsEmailExist(user.EmailID);
                var isUserName = IsUserNameExist(user.UserName);
                if (isExist)
                {
                    ModelState.AddModelError("EmailID", "Email already exist");
                    return View(user);
                }
                if (isUserName)
                {
                    ModelState.AddModelError("UserName", "UserName already exist");
                    return View(user);
                }
                user.ActivationCode = Guid.NewGuid();

                user.Password = Convert.ToBase64String(
                System.Security.Cryptography.SHA256.Create()
                .ComputeHash(Encoding.UTF8.GetBytes(user.Password))
                );
                user.ConfirmPassword = Convert.ToBase64String(
                System.Security.Cryptography.SHA256.Create()
                .ComputeHash(Encoding.UTF8.GetBytes(user.ConfirmPassword))
                );
                user.IsEmailVerified = false;
                user.Picture = "/uploads/default.png";

                _context.users.Add(user);
                _context.SaveChanges();

                SendVerificationLinkEmail(user.EmailID, user.ActivationCode.ToString());
                message = "Registration successfully done. Account activation link " +
                    " has been sent to your email id:" + user.EmailID;
                Status = true;
            }
            else
            {
                message = "Invalid Request";
            }
            ViewBag.Message = message;
            ViewBag.Status = Status;
            return View(user);
        }
        [HttpGet]
        public ActionResult MyAccount(string id)
        {
            return View();
        }


        [HttpGet]
        public ActionResult VerifyAccount(string id)
        {
            if (id == null)
                id = "";
            bool Status = false;

            var getUser = _context.users.Where(u => u.ActivationCode == new Guid(id)).FirstOrDefault();
            if (getUser != null)
            {
                getUser.IsEmailVerified = true;
                _context.SaveChanges();
                Status = true;
            }
            else
            {
                ViewBag.Message = "Invalid Request";
            }

            ViewBag.Status = Status;
            return View();
        }
        [HttpGet]
        [Authorize(Roles = "Admin,Patient")]
        public ActionResult Pretrazi(string specializationFilter, string searchByFullName)
        {
            IQueryable<User> zubarip;
            List<DoctorSearch> zubari = new List<DoctorSearch>();




            SpecsDTO spec = new SpecsDTO();
            var s = _context.uss.Select(u=>u.specialization);
            foreach (string str in s)
                if(str != null)
                spec.specializations.Add(str);
            ViewBag.Specs = spec;

            if (searchByFullName == null)
                searchByFullName = "";
            if (specializationFilter == null)
                zubarip = _context.users.Where(p =>
             p.Role == "Doctor" &&(p.FirstName.Contains(searchByFullName) || p.LastName.Contains(searchByFullName) && p.IsEmailVerified == true)
        ).Include(p=>p.CV).Include(s=>s.Specialization).Include(r=>r.RatingsTo);
            else
                zubarip = _context.users.Where(p => p.IsEmailVerified==true &&
             p.Role == "Doctor" && p.Specialization.specialization== specializationFilter && (p.FirstName.Contains(searchByFullName) || p.LastName.Contains(searchByFullName))
        ).Include(p => p.CV).Include(s => s.Specialization).Include(r => r.RatingsTo);


            foreach (User u in zubarip)
                if (u.CV != null && u.CV.IsApproved == 1) 
                zubari.Add(new DoctorSearch { UserName = u.UserName, FirstName = u.FirstName, LastName = u.LastName, Picture = u.Picture, Specialization = u.Specialization.specialization == null ? "" : u.Specialization.specialization, Rating = u.RatingsTo.Count == 0 ? 0 : u.RatingsTo.Select(r => r.Rate).Average() });

            if (zubari.Count > 0)
            {
                ViewBag.Zubari = zubari;
                return View();
            }
            ViewBag.Message = "No doctors found";
            return View();
        }
        [HttpGet]
        [Authorize(Roles = "Admin,Patient,Doctor")]
        //ne koristimo
        public ActionResult ShowProfile(string userName)
        {
            var getUser = _context.users.Where(u => u.UserName == userName).FirstOrDefault();
            if (getUser != null)
            {
                return View(getUser);
            }
            ViewBag.Message = "No user with username";
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin,Doctor")]
        public bool UploadCV([FromForm] CvDTO cv)
        {
            
            if (cv.PdfFile == null || cv.PdfFile.Length == 0)
            {
                return false;
            }
            var getDoctor = _context.users.Where(u => u.UserName == cv.UserName).Include(c=>c.CV).FirstOrDefault();
            if(getDoctor!= null)
            {
            var filePath = "./wwwroot/uploads/cvs/" + cv.UserName+".pdf";
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                cv.PdfFile.CopyTo(stream);
            }
            if(getDoctor.CV == null)
            {
                getDoctor.CV = new CV
                {
                    CvFile = "/uploads/cvs/" + cv.UserName + ".pdf",
                    Doctor = getDoctor,
                    IsApproved = 0
                };
                _context.SaveChanges();
                return true;
            } 
            }
            return false;
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public List<DoctorCvRequestDTO> GetCvRequests()
        {
            List<DoctorCvRequestDTO> list = new List<DoctorCvRequestDTO>();
            var getDoctorsCV = _context.CV.Where(c => c.IsApproved == 0).Include(c => c.Doctor).ToArray();
            foreach (CV cv in getDoctorsCV)
                if(cv.Doctor!= null)
                {
                    DoctorCvRequestDTO doc = new DoctorCvRequestDTO
                    {
                        userName = cv.Doctor.UserName,
                        cvURL = cv.CvFile
                    };
                    list.Add(doc);
                }
            return list;
        }
        [HttpPut]
        [Authorize(Roles = "Admin")]
        public bool ConfirmCV(string UserName, int Confirm)
        {
            var getDoctor = _context.users.Where(u => u.UserName == UserName).Include(u=>u.CV).FirstOrDefault();
            if (getDoctor.CV == null)
                return false;
            if(Confirm == 1)
                getDoctor.CV.IsApproved = 1;
            else
                getDoctor.CV.IsApproved = -1;
            _context.SaveChangesAsync();
            return true;
        }




        [HttpPost]
        [Authorize(Roles = "Admin,Patient")]
        public float RateDoctor(string from, string to, float rating)
        {
            //frombody
            if (from == null)
                from = "";
            if (to == null)
                to = "";
            if (rating < 0 || rating > 5)
                return 0;
            float result = 0;
            var fromUser = _context.users.Where(u => u.UserName == from).FirstOrDefault();
            var toDoctor = _context.users.Where(u => u.UserName == to).Include(r=>r.RatingsTo).FirstOrDefault();
            if(fromUser != null && toDoctor != null)
            {
                bool contains = false;                
                foreach(Rating r in toDoctor.RatingsTo)
                {
                    if (r.FromWho == fromUser)
                    {
                        contains = true;
                        r.Rate = rating;
                        break;
                    }
                }
                if(contains)
                {
                    _context.SaveChanges();
                    result = toDoctor.RatingsTo.Count > 0 ? toDoctor.RatingsTo.Select(r => r.Rate).Average() : 0;
                }
                else
                {
                    Rating r = new Rating()
                    {
                        FromWho = fromUser,
                        ToWho = toDoctor,
                        Rate = rating
                    };
                    toDoctor.RatingsTo.Add(r);
                    _context.SaveChanges();
                    result = toDoctor.RatingsTo.Count > 0 ? toDoctor.RatingsTo.Select(r => r.Rate).Average() : 0;
                }                
            }
            else
            {
                ViewBag.Message = "ERROR";
            }
            return result;
        }
        [HttpPost]
        [Authorize(Roles = "Admin,Patient")]
        public IActionResult CommentDoctor(string from, string to, string text)
        {
            //frombody
            if (from == null)
                from = "";
            if (to == null)
                to = "";
            if (text == null || text == "")
                return Content("<script>window.location.reload();</script>");
            var fromWho = _context.users.Where(u => u.UserName == from).FirstOrDefault();
            var toWho = _context.users.Where(u => u.UserName == to).FirstOrDefault();
            if(fromWho != null && toWho != null)
            {
                Comment c = new Comment()
                {
                    CommentFrom = fromWho,
                    CommentTo = toWho,
                    CommentContent = text,
                    Date = DateTime.Now
                };
                _context.comments.Add(c);
                _context.SaveChanges();
                return Content("<script>window.location.reload();</script>");
            }

            return Content("<script>window.location.reload();</script>");
        }
        [HttpPost]
        [Authorize(Roles = "Admin,Patient")]
        public List<List<TermDTO>> FreeTime([FromBody] Appointment appointment)
        {
            List<List<TermDTO>> data= new List<List<TermDTO>>();
            if (appointment.startTime == "")
                return data;
            DateTime date = DateTime.Parse(appointment.startTime);

            //if (date < DateTime.Now)
            //    return data;

            int totalTime = 0;
            foreach (string intervention in appointment.interventions)
            {
                var time = _context.Intervention.Where(i => i.InterventionID == int.Parse(intervention)).Select(i => i.InterventionTimeMinutes).FirstOrDefault();
                if (time != null)
                    totalTime += time;
            }
            var getDoctor = _context.users
               .Where(d => d.UserName == appointment.ToWho).FirstOrDefault();
            var workdays = _context.WorkingDay.Where(w => w.Doctor == getDoctor && (w.StartTime.DayOfYear >= date.DayOfYear && w.StartTime.DayOfYear < date.DayOfYear + 7)).Include(t => t.Terms);
            var getUser = _context.users.Where(u => u.UserName == appointment.fromWho).FirstOrDefault();
            foreach (WorkingDay workday in workdays)
            {
                var terms = workday.Terms.Select(t => (t.TermStartDate, t.TermEndDate, t.UserID));
                List<(int, int)> list = new List<(int, int)>();
                List<TermDTO> times = new List<TermDTO>();
                foreach ((DateTime, DateTime, int) el in terms)
                {
                    list.Add((el.Item1.Hour * 60 + el.Item1.Minute, el.Item2.Hour * 60 + el.Item2.Minute));
                    TermDTO termDto = new TermDTO { 
                        startDate= $"{{ \"DateTime\": \"{workday.StartTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")}\" }}",
                        startHours = el.Item1.Hour,
                        startMinutes = el.Item1.Minute,
                        endHours = el.Item2.Hour,
                        endMinutes = el.Item2.Minute,
                        isAvailable = false };

                    if (el.Item3 == getUser.UserID)
                        termDto.isItMyTerm = true;
                    else
                        termDto.isItMyTerm = false;
                    times.Add(termDto);
                }
                list.Sort((term1, term2) => term1.Item1.CompareTo(term2.Item1));
                List<int> output = new List<int>();
                int startTime = workday.StartTime.Hour * 60 + workday.StartTime.Minute;
                int endTime = workday.EndTime.Hour * 60 + workday.EndTime.Minute;
                int currentTime = startTime;
                int t1, t2;
                (t1, t2) = list.FirstOrDefault();
                while (currentTime + totalTime < endTime)
                {
                    if ((currentTime >= t1 && currentTime < t2) || (currentTime + totalTime > t1 && currentTime + totalTime < t2) || (currentTime < t1 && currentTime + totalTime > t2))
                    {
                        currentTime = t2;
                        list.RemoveAt(0);
                        (t1, t2) = list.FirstOrDefault();
                    }
                    else
                    {
                        if (t1 == 0 && t2 == 0) //nema vise termina
                        {
                            output.Add(currentTime);
                            currentTime += 10;
                        }
                        else
                        {
                            if (currentTime > t2)
                            {
                                list.RemoveAt(0);
                                (t1, t2) = list.FirstOrDefault();
                            }
                            output.Add(currentTime);
                            currentTime += 10;
                        }
                    }
                }
                foreach (int el in output)
                {
                    int end = el + totalTime;
                    times.Add(new TermDTO {
                        
                        startDate= $"{{ \"DateTime\": \"{workday.StartTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")}\" }}",
                        startHours = el / 60,
                        startMinutes = el % 60,
                        endHours = end / 60,
                        endMinutes = end % 60,
                        isAvailable = true,
                        isItMyTerm = false });
                }
                times.Sort((time1, time2) => time1.StartTime().CompareTo(time2.StartTime()));
            data.Add(times);
            }            
            return data;
        }
        [HttpDelete]
        [Authorize(Roles = "Patient,Admin")]
        public bool DeleteMyAppointment([FromBody] Appointment appointment)
        {

            if (appointment.fromWho == null || appointment.ToWho == null || appointment.startTime == null)
                return false;
            DateTime startTime = DateTime.Parse(appointment.startTime);

            var getDoctor = _context.users
               .Where(d => d.UserName == appointment.ToWho)
               .Include(u => u.WorkingDays.Where(r => r.StartTime.DayOfYear == startTime.DayOfYear)).ThenInclude(t => t.Terms).ThenInclude(u => u.User).FirstOrDefault();
            if (getDoctor != null && getDoctor.WorkingDays.Count> 0)
            {
                //termin koji se poklapa - ne postoje termini sa istim vremenom
                Term t = getDoctor.WorkingDays[0].Terms.FirstOrDefault(t =>
                t.TermStartDate.Hour == startTime.Hour &&
                t.TermStartDate.Minute == startTime.Minute &&
                t.User.UserName == appointment.fromWho);
                if (t != null)
                    _context.Term.Remove(t);
                _context.SaveChanges();
            }
            return true;
        }
        [HttpGet]
        [Authorize(Roles = "Patient,Admin,Doctor")]
        public ActionResult LoadMyProfile(string userName)
        {
            var getUser = _context.users.Where(u => u.UserName == userName).Include(u=>u.CV).FirstOrDefault();
            if (getUser != null)
            {
                bool verified;
                if (getUser.CV == null)
                    verified = false;
                else
                    if (getUser.CV.IsApproved == 1)
                    verified = true;
                else
                    verified = false;
                ProfileDTO p = new ProfileDTO()
                {
                    UserName = getUser.UserName,
                    FirstName = getUser.FirstName,
                    LastName = getUser.LastName,
                    EmailID = getUser.EmailID,
                    Picture = getUser.Picture,
                    DateOfBirth = getUser.DateOfBirth,
                    isVerified = verified
                };
                ViewBag.Profile = p;
            }
            return View();
        }

        [HttpPut]
        [Authorize(Roles = "Patient,Admin,Doctor")]
        public ActionResult ChangeProfile([FromBody]ProfileDTO profile)
        {
            var getUser = _context.users.Where(u => u.UserName == profile.UserName).FirstOrDefault();
            if (getUser != null)
            {
                getUser.FirstName = profile.FirstName;
                getUser.LastName = profile.LastName;
                //DOPUNI
                //getUser.Picture = profile.Picture;
                //getUser.DateOfBirth = profile.DateOfBirth;
                _context.users.Update(getUser);
                _context.SaveChanges();
            }
            return LoadMyProfile(profile.UserName);
        }
        //dodaj obrisi profil
        [HttpGet]
        [Authorize(Roles = "Admin,Doctor,Patient")]
        public List<TermPatientDTO> MyTerms(string userName)
        {
            var getUser = _context.users.Where(u => u.UserName == userName).Include(u=>u.Terms).ThenInclude(t=>t.WorkingDay).ThenInclude(d=>d.Doctor).FirstOrDefault();
            List<TermPatientDTO> list = new List<TermPatientDTO>();
            if (getUser != null && getUser.Terms != null)
                foreach (var term in getUser.Terms)
                {
                    //if(term.TermStartDate >= DateTime.Now)
                    list.Add(new TermPatientDTO
                    {
                        startDate = term.TermStartDate,
                        startDateS = term.TermStartDate.ToString("yyyy-MM-ddTHH:mm:ss"),
                        endDateS = term.TermEndDate.ToString("yyyy-MM-ddTHH:mm:ss"),
                        fullName = term.WorkingDay.Doctor.FirstName + " " + term.WorkingDay.Doctor.LastName,
                        workingDay = term.WorkingDayID,
                        doctorUserName = term.WorkingDay.Doctor.UserName
                        
                    }) ;
                }
                return list;
        }


        [HttpPost]
        [Authorize(Roles = "Admin,Patient")]
        public bool MakeAppointment([FromBody]Appointment appointment)
        {      
            DateTime startTime = DateTime.Parse(appointment.startTime);
            var getDoctor = _context.users
               .Where(d => d.UserName == appointment.ToWho)               
               .Include(u => u.WorkingDays.Where(r => r.StartTime.DayOfYear == startTime.DayOfYear)).ThenInclude(t => t.Terms).ThenInclude(ti => ti.TermInt).ThenInclude(i => i.Intervention)
               .FirstOrDefault();
            //WorkingDay workday = _context.WorkingDay.Where(w=>w.Doctor == getDoctor && w.WorkingDayID == 1).FirstOrDefault();
            var getPatient = _context.users.Where(u => u.UserName == appointment.fromWho).FirstOrDefault();

            int totalTime = 0;
            foreach (string intervention in appointment.interventions)
            {
                var time = _context.Intervention.Where(i => i.InterventionID == int.Parse(intervention)).Select(i => i.InterventionTimeMinutes).FirstOrDefault();
                if (time != null)
                    totalTime += time;
            }

            if ( getPatient!=null && getDoctor!=null && getDoctor.WorkingDays != null && getDoctor.WorkingDays.Count > 0)
            {
                Term t = new Term() {
                    User = getPatient,
                    TermStartDate = DateTime.Parse(appointment.startTime),
                    TermEndDate = DateTime.Parse(appointment.startTime).AddMinutes(totalTime),
                    TermInt = new List<TermInt>()
            };
                foreach(string i in appointment.interventions)
                {
                    TermInt ti = new TermInt()
                    {
                        Term = t,
                        Intervention = _context.Intervention.Where(inte => inte.InterventionID == Int32.Parse(i)).FirstOrDefault()
                    };
                    t.TermInt.Add(ti);
                }
                getDoctor.WorkingDays[0].Terms.Add(t);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
        [HttpGet]
        [Authorize(Roles = "Patient,Admin,Doctor")]
        public ActionResult ShowProfileDoctor(string userName)
        {
            if (userName == null)
                userName = "";
            var getDoctor = _context.users
                .Where(d => d.UserName == userName).Include(t=>t.Specialization)
                .Include(u => u.CommentsTo).ThenInclude(t=>t.CommentFrom)
                .Include(u => u.RatingsTo).FirstOrDefault();
            if(getDoctor != null)
            {
                List<(string, int)> list = new List<(string, int)>();
                var allInterventions = _context.Intervention;
                foreach (Intervention i in allInterventions)
                {
                    list.Add((i.Name, i.InterventionTimeMinutes));
                }

                List<CommentDTO> comments = new List<CommentDTO>();
                foreach(Comment c in getDoctor.CommentsTo)
                {
                    comments.Add(new CommentDTO()
                    {
                        fullName = c.CommentFrom.FirstName + " " + c.CommentFrom.LastName,
                        commentContent = c.CommentContent,
                        date = c.Date,
                        image = c.CommentFrom.Picture
                    }) ;
                }
                List<InterventionDTO> interventions = new List<InterventionDTO>();
                var intIds = _context.SI.Where(si => si.SpecId == getDoctor.Specialization.USID).Select(si=>si.IntId);
                var doctorInterventions = _context.Intervention.Where(iter => intIds.Contains(iter.InterventionID)); ;
                foreach (Intervention i in doctorInterventions)
                {                  
                interventions.Add(new InterventionDTO()
                {
                    interventionID = i.InterventionID,
                    name = i.Name,
                    timeInMinutes = i.InterventionTimeMinutes
                });
                }
                /*
                foreach (Intervention i in _context.Intervention)
                    interventions.Add(new InterventionDTO()
                    {
                        interventionID = i.InterventionID,
                        name = i.Name,
                        timeInMinutes = i.InterventionTimeMinutes
                    }) ;
                */
                GetDoctorDTO doc = new GetDoctorDTO()
                {
                    specialization = getDoctor.Specialization.specialization,
                    comments = comments,
                    fullName = getDoctor.FirstName + " " + getDoctor.LastName,
                    picture = getDoctor.Picture,
                    average = getDoctor.RatingsTo.Count > 0 ? getDoctor.RatingsTo.Select(r => r.Rate).Average() : 0,
                    interventions = interventions,
                    userName = getDoctor.UserName
                };
                ViewBag.Doctor = doc;
                return View();
            }            
            ViewBag.Message = "No user with username";
            return View();
        }
        [HttpGet]
        [Authorize(Roles = "Admin,Doctor")]
        public List<PatientDataDTO> DoctorTerms(string userName,string searchDate)
        {
            DateTime date = DateTime.Parse(searchDate);            
            List<PatientDataDTO> list = new List<PatientDataDTO>();
            var getDoctor = _context.users.Where(u => u.UserName == userName).Include(w => w.WorkingDays.Where(w=>w.StartTime.DayOfYear == date.DayOfYear)).ThenInclude(t => t.Terms).ThenInclude(patient => patient.User).FirstOrDefault();
            
            if (getDoctor != null &&  getDoctor.WorkingDays.Count>0)
            {
                foreach (Term t in getDoctor.WorkingDays[0].Terms)
                {
                    var interventions = _context.Term.Where(x => x.TermID == t.TermID).Include(t => t.TermInt).ThenInclude(i => i.Intervention).FirstOrDefault();
                    List<string> intList = new List<string>();
                    if (interventions != null)
                        foreach (TermInt ti in interventions.TermInt)
                            if (!intList.Contains(ti.Intervention.Name))
                                intList.Add(ti.Intervention.Name);



                    list.Add(new PatientDataDTO()
                    {
                        startDateS = t.TermStartDate.ToString("yyyy-MM-ddTHH:mm:ss"),
                        endDateS = t.TermEndDate.ToString("yyyy-MM-ddTHH:mm:ss"),
                       // startDateS = $"{{ \"DateTime\": \"{t.TermStartDate.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")}\" }}",
                       // endDateS = $"{{ \"DateTime\": \"{t.TermEndDate.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")}\" }}",
                        patientFullName = " " + t.User.FirstName + t.User.LastName,
                        startingTime = t.TermStartDate.Hour * 60 + t.TermStartDate.Minute,
                        interventions = intList
                    }) ;
                }
                list.Sort((term1, term2) => term1.startingTime.CompareTo(term2.startingTime));
            }
            return list;
        }
        public ActionResult Blogs()
        {
            List <BlogDTO> Blogs= GetBlogs();            
            ViewBag.Blogs = Blogs;
            return View();
        }

        [NonAction]
        public List<BlogDTO> GetBlogs()
        {
            List<BlogDTO> Blogs = new List<BlogDTO>();
            var blogs = _context.blogs;
            if(blogs!=null)
            {

                foreach(Blog bl in blogs)
                {
                    Blogs.Add(new BlogDTO
                    {
                        Title = bl.Title,
                        Details = bl.Details,
                        Date = bl.Date.ToShortDateString(),
                        pictureData = bl.Image
                    });
                }
            }
            //List<BlogDTO> sortedBlogs = Blogs.OrderByDescending(b => DateTime.ParseExact(b.Date, "d/M/yyyy", CultureInfo.InvariantCulture)).ToList();
            return Blogs;
        }
        
        [HttpPost]
        public void UploadBlog([FromBody] BlogDTO blog)
        {
            if (blog.Details == null || blog.Details == "" || blog.Title == null || blog.Title == "")
                return;
            string base64Data = blog.pictureData.Substring(blog.pictureData.IndexOf(',') + 1);
            byte[] imageBytes = Convert.FromBase64String(base64Data);
            using (MemoryStream ms = new MemoryStream(imageBytes))
            {
                using (Bitmap image = new Bitmap(ms))
                {
                    string filePath = "./wwwroot/uploads/blogs/" + blog.Title + ".jpg";
                    try
                    {
                        image.Save(filePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }                    
                }                
            }
            Blog newBlog = new Blog()
            {
                Image = "/uploads/blogs/" + blog.Title + ".jpg",
                Title = blog.Title,
                Date = DateTime.Today,
                Details = blog.Details,
                TextHTML = "",
            };
            _context.blogs.Add(newBlog);
            _context.SaveChangesAsync();
        }
        [HttpPost]
        public string GenerateImage([FromBody] PicDTO picture)
        {
            
            string base64Data = picture.pictureData.Substring(picture.pictureData.IndexOf(',') + 1);
            byte[] imageBytes = Convert.FromBase64String(base64Data);
            using (MemoryStream ms = new MemoryStream(imageBytes))
            {
                using (Bitmap image = new Bitmap(ms))
                {
                    string filePath = "./wwwroot/uploads/" + picture.userName+".jpg";
                    try
                    {
                        image.Save(filePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }                   
                }
            }
            var getUser = _context.users.Where(u => u.UserName == picture.userName).FirstOrDefault();
            getUser.Picture = "/uploads/" + picture.userName + ".jpg";
            _context.SaveChanges();
            return "/uploads/" + picture.userName + ".jpg";
        }


        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLogin login, string ReturnUrl = "")
        {
            string message = "";
            
                var getUser = _context.users.Where(u => u.EmailID == login.EmailID).FirstOrDefault();
                if (getUser != null)
                {
                    if (!getUser.IsEmailVerified)
                    {
                        ViewBag.Message = "Please verify your email first";
                        return View();
                    }
                    if (string.Compare(Convert.ToBase64String(System.Security.Cryptography.SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(login.Password))), getUser.Password) == 0)
                    {

                        List<Claim> claims = new List<Claim>();
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, getUser.EmailID));
                        claims.Add(new Claim(ClaimTypes.Name, getUser.UserName));
                        claims.Add(new Claim(ClaimTypes.Role, getUser.Role));
                        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                        HttpContext.SignInAsync(claimsPrincipal);                        
                        
                        if (Url.IsLocalUrl(ReturnUrl))
                        {                            
                            return Redirect(ReturnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        message = "Invalid credential provided";
                    }
                }
                else
                {
                    message = "Invalid credential provided";
                }            
            ViewBag.Message = message;
            return View();
        }
        [HttpGet]
        public ActionResult AdminPage()
        {
            return View();
        }



        //Logout
        [Authorize]
        [HttpGet]
        public ActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return Redirect("/Home");
        }

        [NonAction]
        public bool IsEmailExist(string emailID)
        {
            var getUser = _context.users.Where(u => u.EmailID == emailID).FirstOrDefault();
            return getUser != null;
        }
        [NonAction]
        public bool IsUserNameExist(string userName)
        {
            var getUser = _context.users.Where(u => u.UserName == userName).FirstOrDefault();
            return getUser != null;
        }
        [NonAction]
        public void SendVerificationLinkEmail(string emailID, string activationCode)
        {
            string verifyUrl = "/user/verifyaccount/" + activationCode;
            string link = "https://" + Request.Host.ToString() + verifyUrl;

            var fromEmail = new MailAddress("probazaslanje123@gmail.com", "StomatoloskaOrdinacija");
            var toEmail = new MailAddress(emailID);
            var fromEmailPassword = "fsyxwkprmdicxbii"; // Replace with actual password
            string subject = "Your account is successfully created!";

            string body = "<br/><br/>Account created" +
                " <br/><br/><a href='" + link + "'>" + link + "</a> ";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);        
        }
    }
}
