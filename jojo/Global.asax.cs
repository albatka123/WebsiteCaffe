using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using AutoMapper;
using Cafe.Models;
using Cafe.DTO;

namespace Cafe
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Formatting = Formatting.None,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            Mapper.Initialize(cfg => {
                 cfg.CreateMap<nhanvien, NhanVienDto>();
                cfg.CreateMap<admin, AdminDto>();
                /* etc */
            });

           
        }
    }
}
