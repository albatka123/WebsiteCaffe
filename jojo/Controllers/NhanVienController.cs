using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cafe.DTO;
using Cafe.Models;
using Newtonsoft.Json;

namespace Cafe.Controllers
{
    public class NhanVienController : Controller
    {
        //
        // GET: /NhanVien/

        CafeContext db = new CafeContext();
        public ActionResult Index()
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
            var query = from b in db.nhanviens
                        where b.active == "true"
                        select b;
            ViewBag.nhanvien = a;
            ViewBag.nhanviens = query.ToList();
            return View();
        }

        public ActionResult ChiTiet(string id)
        {
            if (Request.Cookies["nhanvien"] == null)
            {
                return Redirect("/");
            }
            try
            {
                NhanVienDto a = JsonConvert.DeserializeObject<NhanVienDto>(HttpUtility.UrlDecode(Request.Cookies["nhanvien"].Value));
                if (a.loai == 1)
                {
                    return Redirect("/KinhDoanh");
                }
                ViewBag.nhanvien = a;
                int id2 = Convert.ToInt32(id);
                var query = from b in db.nhanviens
                            where b.id == id2 && b.active == "true"
                            select b;
                nhanvien c = query.FirstOrDefault<nhanvien>();
                if (c == null)
                {
                    return Redirect("/NhanVien");
                }
                else
                {
                    ViewBag.nhanvien2 = c;
                    return View();
                }
            }
            catch
            {
                return Redirect("/NhanVien");
            }
        }
	}
}