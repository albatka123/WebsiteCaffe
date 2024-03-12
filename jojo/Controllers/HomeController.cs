using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cafe.DTO;
using Cafe.Models;
using System.Data.Entity;
using System.Globalization;
using Newtonsoft.Json;
using AutoMapper;

namespace Cafe.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        CafeContext db = new CafeContext();
        public ActionResult Index()
        {

            if (Request.Cookies["nhanvien"] != null)
            {
                NhanVienDto a = JsonConvert.DeserializeObject<NhanVienDto>(HttpUtility.UrlDecode(Request.Cookies["nhanvien"].Value));
                if (a.loai == 1)
                {
                    return Redirect("/KinhDoanh");
                }
                else
                {
                    Chart chart = new Chart();
                    chart.MonthDoanhThu = GetMonthDoanhThu();
                    chart.MonthChiPhi = GetMonthChiPhi();
                    chart.WeekDoanhThu = GetWeekDoanhThu();
                    ViewBag.nhanvien = a;
                    return View("Chart", chart);
                }
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login()
        {
            try
            {
                var taikhoan = Request.Form["taikhoan"];
                var matkhau = Request.Form["matkhau"];
                nhanvien a = (from b in db.nhanviens
                            where b.taikhoan == taikhoan && b.matkhau == matkhau && b.active == "true"
                            select b).FirstOrDefault();
                NhanVienDto nhanVienDto = Mapper.Map<nhanvien, NhanVienDto>(a);

                if (a != null)
                {
                    Response.Cookies["nhanvien"].Value = HttpUtility.UrlEncode(JsonConvert.SerializeObject(nhanVienDto));
                    Response.Cookies["nhanvien"].Expires = DateTime.Now.AddDays(30);
                }
            }
            catch(Exception ex)
            {
                var x = 1;
            }
            return Redirect("/");
        }

        public ActionResult Logout()
        {
            if (Request.Cookies["nhanvien"] != null)
            {
                var c = new HttpCookie("nhanvien");
                c.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(c);
            }
            
            return Redirect("/");
        }


        // chart
        [HttpPost]
        public JsonResult Year(List<string> years)
        {
            Chart chart = new Chart();
            chart.YearDoanhThu = GetYearDoanhThu(years);
            chart.YearChiPhi = GetYearChiPhi(years);


            return Json(chart, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Date(string tungay, string toingay)
        {
            Chart chart = new Chart();
            chart.DateDoanhThu = GetDateDoanhThu(tungay, toingay);
            chart.DateChiPhi = GetDateChiPhi(tungay, toingay);
            return Json(chart, JsonRequestBehavior.AllowGet);
        }



        private string GetMonthDoanhThu()
        {
            string s = "";
            DateTime ngay = DateTime.Now;
            ngay = ngay.AddHours(14);
            var query = (from b in db.hoadons
                         where b.active == "true"
                         && b.ngayhoadon.Year == ngay.Year
                         group b by b.ngayhoadon.Month into g
                         select new
                         {
                             month = g.Key,
                             total = g.Sum(x => x.khachphaitra)
                         }).ToList();
            for (int i = 1; i <= 12; i++)
            {
                int k = -1;
                for (int v = 0; v < query.Count; v++)
                {
                    if (query[v].month == i)
                    {
                        k = v;
                        break;
                    }
                }

                if (k != -1)
                {
                    s = s + query[k].total + ",";
                }
                else
                {
                    s = s + "0" + ",";
                }
            }

            s = s.Substring(0, s.Length - 1);
            return s;
        }

        private string GetMonthChiPhi()
        {
            string s = "";
            DateTime ngay = DateTime.Now;
            ngay = ngay.AddHours(14);
            var query = (from b in db.chiphis
                         where b.active == "true"
                         && b.ngay.Year == ngay.Year
                         group b by b.ngay.Month into g
                         select new
                         {
                             month = g.Key,
                             total = g.Sum(x => x.chiphi1)
                         }).ToList();
            for (int i = 1; i <= 12; i++)
            {
                int k = -1;
                for (int v = 0; v < query.Count; v++)
                {
                    if (query[v].month == i)
                    {
                        k = v;
                        break;
                    }
                }

                if (k != -1)
                {
                    s = s + query[k].total + ",";
                }
                else
                {
                    s = s + "0" + ",";
                }
            }

            s = s.Substring(0, s.Length - 1);
            return s;
        }


        private string GetWeekDoanhThu()
        {
            DateTime ngay = DateTime.Now;
            ngay = ngay.AddHours(14);
            DateTime today = ngay;
            int currentDayOfWeek = (int)today.DayOfWeek;
            DateTime sunday = today.AddDays(-currentDayOfWeek);
            DateTime monday = sunday.AddDays(1);
            // If we started on Sunday, we should actually have gone *back*
            // 6 days instead of forward 1...
            if (currentDayOfWeek == 0)
            {
                monday = monday.AddDays(-7);
            }
            var dates = Enumerable.Range(0, 7).Select(days => monday.AddDays(days)).ToList();
            //


            DateTime tungay = dates[0];
            DateTime toingay = dates[6];
            var query = (from b in db.hoadons
                         where DbFunctions.TruncateTime(b.ngayhoadon) >= DbFunctions.TruncateTime(tungay)
                         && DbFunctions.TruncateTime(b.ngayhoadon) <= DbFunctions.TruncateTime(toingay)
                         && b.active == "true"
                         group b by DbFunctions.TruncateTime(b.ngayhoadon) into g
                         select new
                         {
                             ngayhoadon = g.Key.Value,
                             tongtien = g.Sum(p => p.tongtien),
                             giamgia = g.Sum(p => p.giamgia),
                             khachphaitra = g.Sum(p => p.khachphaitra)
                         }).ToList();

            string s = "";
            for (var date = tungay.Date; date <= toingay.Date; date = date.AddDays(1))
            {
                int k = -1;
                for (int v = 0; v < query.Count; v++)
                {
                    if (query[v].ngayhoadon.Date == date)
                    {
                        k = v;
                        break;
                    }
                }

                if (k != -1)
                {
                    s = s + query[k].khachphaitra + ",";
                }
                else
                {
                    s = s + "0" + ",";
                }
            }
            s = s.Substring(0, s.Length - 1);
            return s;
        }

        private string GetYearDoanhThu(List<string> years)
        {
            string s = "";
            for (int i = 0; i < years.Count; i++)
            {
                int year2 = int.Parse(years[i]);
                var total = 0;
                var query = from b in db.hoadons
                            where b.active == "true"
                            && b.ngayhoadon.Year == year2
                            select b.khachphaitra;
                if (query.Count() > 0)
                {
                    foreach (var item in query)
                    {
                        total = total + item;
                    }
                }

                s = s + total + ",";
            }
            s = s.Substring(0, s.Length - 1);
            return s;
        }

        private string GetYearChiPhi(List<string> years)
        {
            string s = "";
            for (int i = 0; i < years.Count; i++)
            {
                int year2 = int.Parse(years[i]);
                var total = 0;
                var query = from b in db.chiphis
                            where b.active == "true"
                            && b.ngay.Year == year2
                            select b.chiphi1;
                if (query.Count() > 0)
                {
                    foreach (var item in query)
                    {
                        total = total + item;
                    }
                }

                s = s + total + ",";
            }


            s = s.Substring(0, s.Length - 1);
            return s;
        }

        private string GetDateDoanhThu(string tungay, string toingay)
        {

            DateTime date1 = DateTime.ParseExact(tungay, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            DateTime date2 = DateTime.ParseExact(toingay, "yyyy-MM-dd", CultureInfo.InvariantCulture);


            var query = (from b in db.hoadons
                         where DbFunctions.TruncateTime(b.ngayhoadon) >= DbFunctions.TruncateTime(date1)
                         && DbFunctions.TruncateTime(b.ngayhoadon) <= DbFunctions.TruncateTime(date2)
                         && b.active == "true"
                         group b by DbFunctions.TruncateTime(b.ngayhoadon) into g
                         select new
                         {
                             ngayhoadon = g.Key.Value,
                             tongtien = g.Sum(p => p.tongtien),
                             giamgia = g.Sum(p => p.giamgia),
                             khachphaitra = g.Sum(p => p.khachphaitra)
                         }).ToList();

            string s = "";
            for (var date = date1.Date; date <= date2.Date; date = date.AddDays(1))
            {
                int k = -1;
                for (int v = 0; v < query.Count; v++)
                {
                    if (query[v].ngayhoadon.Date == date)
                    {
                        k = v;
                        break;
                    }
                }

                if (k != -1)
                {
                    s = s + query[k].khachphaitra + ",";
                }
                else
                {
                    s = s + "0" + ",";
                }
            }
            s = s.Substring(0, s.Length - 1);
            return s;
        }
        private string GetDateChiPhi(string tungay, string toingay)
        {

            DateTime date1 = DateTime.ParseExact(tungay, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            DateTime date2 = DateTime.ParseExact(toingay, "yyyy-MM-dd", CultureInfo.InvariantCulture);


            var query = (from b in db.chiphis
                         where DbFunctions.TruncateTime(b.ngay) >= DbFunctions.TruncateTime(date1)
                         && DbFunctions.TruncateTime(b.ngay) <= DbFunctions.TruncateTime(date2)
                         && b.active == "true"
                         group b by DbFunctions.TruncateTime(b.ngay) into g
                         select new
                         {
                             ngay = g.Key.Value,
                             chiphi1 = g.Sum(p => p.chiphi1)
                         }).ToList();

            string s = "";
            for (var date = date1.Date; date <= date2.Date; date = date.AddDays(1))
            {
                int k = -1;
                for (int v = 0; v < query.Count; v++)
                {
                    if (query[v].ngay.Date == date)
                    {
                        k = v;
                        break;
                    }
                }

                if (k != -1)
                {
                    s = s + query[k].chiphi1 + ",";
                }
                else
                {
                    s = s + "0" + ",";
                }
            }
            s = s.Substring(0, s.Length - 1);
            return s;
        }
    }
}