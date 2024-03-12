using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cafe.Models;
using System.Data.Entity;
using Cafe.DTO;
using PagedList;
using Newtonsoft.Json;

namespace Cafe.Controllers
{
    public class ChiPhiController : Controller
    {
        //
        // GET: /ChiPhi/

        CafeContext db = new CafeContext();

       
        public ActionResult Index(int? page)// tham so co the null
        {
            try
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
                ViewBag.nhanvien = a;

                ViewBag.loaichiphis = db.loaichiphis.ToList();

                // 


                var query = from b in db.chiphis
                            where b.active == "true"
                            select b;

                List<chiphi> list = query.OrderByDescending(x => x.ngay).ToList();

                int pageSize = 16;

                int pageNumber = (page ?? 1);//neu page null thi gia tri =1



                return View(list.ToPagedList(pageNumber, pageSize));
            }
            catch
            {
                return Redirect("/Error");
            }
            

        }



       
        public ActionResult ThemChiPhi()
        {
            try
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
                ViewBag.nhanvien = a;

                ViewBag.loaichiphis = db.loaichiphis.ToList();

                return View();
            }
            catch
            {
                return Redirect("/Error");
            }
            
        }


        
        [HttpPost]
        public ActionResult ThemChiPhi2()
        {
            try
            {
                DateTime ngay = DateTime.Parse(Request.Form["ngay"]);
                var loaichiphi = Int32.Parse(Request.Form["loaichiphi"]);
                var chiphi = Int32.Parse(Request.Form["chiphi"]);

                chiphi a = new chiphi
                {
                    idloaichiphi = loaichiphi,
                    chiphi1 = chiphi,
                    ngay = ngay,
                    active = "true"
                };
                db.chiphis.Add(a);
                db.SaveChanges();

                return Redirect("/ChiPhi");
            }
            catch
            {
                return Redirect("/Error");
            }
            
        }


        public ActionResult CapNhat(int id)
        {
            try
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
                ViewBag.nhanvien = a;

                var query = from b in db.chiphis
                            where b.id == id
                            select b;
                if (query.Count() != 1)
                {
                    return Redirect("/ChiPhi");
                }
                ViewBag.loaichiphis = db.loaichiphis.ToList();
                ViewBag.chiphi = query.FirstOrDefault();

                return View();
            }
            catch
            {
                return Redirect("/Error");
            }
           
        }



        [HttpPost]
        public ActionResult CapNhat2()
        {
            try
            {
                var id = Int32.Parse(Request.Form["id"]);
                DateTime ngay = DateTime.Parse(Request.Form["ngay"]);
                var loaichiphi1 = Int32.Parse(Request.Form["loaichiphi"]);
                var chiphi = Int32.Parse(Request.Form["chiphi"]);

                var query = from b in db.chiphis
                            where b.id == id
                            select b;
                chiphi a = query.FirstOrDefault();
                if (a != null)
                {
                    a.ngay = ngay;
                    a.idloaichiphi = loaichiphi1;
                    a.chiphi1 = chiphi;

                    db.SaveChanges();
                }

                return Redirect("/ChiPhi");
            }
            catch
            {
                return Redirect("/Error");
            }
           
        }


        public ActionResult Xoachiphi(int id)
        {
            try
            {
                var query = from b in db.chiphis
                            where b.id == id
                            select b;
                chiphi a = query.FirstOrDefault();
                if (a != null)
                {
                    a.active = "false";
                    db.SaveChanges();
                }
                return Redirect("/ChiPhi");
            }
            catch
            {
                return Redirect("/Error");
            }
           
        }
	}
}