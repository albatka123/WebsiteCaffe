using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cafe.Models;
using System.Data.Entity;
using Newtonsoft.Json;
using Cafe.DTO;

namespace Cafe.Controllers
{
    public class BaoCaoCuoiNgayController : Controller
    {
        //
        // GET: /BaoCaoCuoiNgay/

        CafeContext db = new CafeContext();
        public ActionResult Index()
        {
            if (Request.Cookies["nhanvien"] == null)
            {
                return Redirect("/");
            }
            NhanVienDto a = JsonConvert.DeserializeObject<NhanVienDto>(HttpUtility.UrlDecode(Request.Cookies["nhanvien"].Value));//chuyen doi doi tuong json thanh C#(nhanviendto)
            ViewBag.nhanvien = a;
            //

            DateTime ngay = DateTime.Now;

            var query = from b in db.hoadons
                        where DbFunctions.TruncateTime(b.ngayhoadon) == DbFunctions.TruncateTime(ngay)
                        && b.active == "true"
                        select b;
            ViewBag.ngay = ngay.ToString("dd/MM/yyyy");
            ViewBag.hoadon = query.ToList();

            return View();
        }
    }
}