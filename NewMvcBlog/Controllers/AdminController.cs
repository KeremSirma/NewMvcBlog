﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NewMvcBlog.Models;

namespace NewMvcBlog.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        MVCBlogDb db = new MVCBlogDb();
        public ActionResult Index()
        {
            return View();
        }
        
    }
}