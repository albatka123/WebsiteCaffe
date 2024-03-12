using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cafe.Models;
using System.Data.Entity;
using System.Globalization;
using Cafe.DTO;
using Newtonsoft.Json;

namespace Cafe.Controllers
{
    public class HoaDonController : Controller
    {
        //
        // GET: /HoaDon/

        CafeContext db = new CafeContext();
        public ActionResult Index()
        {
            if (Request.Cookies["nhanvien"] == null)
            {
                return Redirect("/");
            }
            NhanVienDto a = JsonConvert.DeserializeObject<NhanVienDto>(HttpUtility.UrlDecode(Request.Cookies["nhanvien"].Value));

            ViewBag.nhanvien = a;
            if (TempData["ngay"] == null)
            {
                return View();
            }
            else
            {
                ViewBag.ngay = TempData["ngay"];
                ViewBag.hoadons = TempData["hoadon"];
                return View("KetQua");
            }
            
        }

        [HttpPost]
        public ActionResult LichSu()
        {
            DateTime ngay = DateTime.Parse(Request.Form["ngay"]);
            
            var query = from b in db.hoadons
                        where DbFunctions.TruncateTime(b.ngayhoadon) == DbFunctions.TruncateTime(ngay)
                        && b.active == "true"
                        select b;
            TempData["hoadon"] = query.ToList();
            TempData["ngay"] = ngay.ToString("dd/MM/yyyy");
            return Redirect("/HoaDon");
        }

        public ActionResult Xoahoadon(string id, string ngay)
        {
            if (Request.Cookies["nhanvien"] == null)
            {
                return Redirect("/");
            }
            NhanVienDto a = JsonConvert.DeserializeObject<NhanVienDto>(HttpUtility.UrlDecode(Request.Cookies["nhanvien"].Value));
            if (a.loai == 1)
            {
                return Redirect("/KinhDoanh");
            }

            try
            {
                int id2 = Convert.ToInt32(id);
                var query = from b in db.hoadons
                            where b.id == id2 && b.active == "true"
                            select b;
                hoadon c = query.FirstOrDefault<hoadon>();
                if (c != null)
                {
                    c.active = "false";
                    //
                    bangxoahoadon delhoadon = new bangxoahoadon
                    {
                        idhoadon = id2,
                        idnhanvien = a.id,
                        ngay = DateTime.Now.AddHours(14),
                        active = "true"
                    };
                    db.bangxoahoadons.Add(delhoadon);
                    db.SaveChanges();
                }
            }
            catch
            {
                return Redirect("/Error");
            }
            DateTime ngay2 = DateTime.ParseExact(ngay, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            var query2 = from b in db.hoadons
                        where DbFunctions.TruncateTime(b.ngayhoadon) == DbFunctions.TruncateTime(ngay2)
                        && b.active == "true"
                        select b;
            TempData["hoadon"] = query2.ToList();
            TempData["ngay"] = ngay2.ToString("dd/MM/yyyy");
            return Redirect("/HoaDon");
        }
	}
}