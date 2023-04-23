﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practice.Data;
using Practice.Helpers;
using Practice.Models;
using Practice.Services.Interfaces;

namespace Practice.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BlogController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IBlogService _blogService;
        private readonly IWebHostEnvironment _env;

        public BlogController(AppDbContext context,
                              IWebHostEnvironment env,
                              IBlogService blogService)
        {
            _context = context;
            _env = env;
            _blogService = blogService;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Blog> blogs = await _blogService.GetAll();
            return View(blogs);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Blog blog)
        {
            try
            {
                if (!ModelState.IsValid) return View();

                if (!blog.Photo.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Photo", "File type must be image");
                    return View();
                }
                if (!blog.Photo.CheckFileSize(200))
                {
                    ModelState.AddModelError("Photo", "File size must be max 200kb");
                    return View();
                }
                blog.Image = blog.Photo.CreateFile(_env, "img");

                await _context.Blogs.AddAsync(blog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                ViewBag.error = ex.Message;
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();
            var dbBlog = await _context.Blogs.FirstOrDefaultAsync(b => b.Id == id);
            if (dbBlog is null) return NotFound();
            return View(dbBlog);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return BadRequest();
            var dbBlog = await _context.Blogs.FirstOrDefaultAsync(b=>b.Id == id);
            if (dbBlog is null) return NotFound();
            return View(dbBlog);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, Blog blog)
        {
            try
            {
                if (id is null) return BadRequest();
                var dbBlog = await _context.Blogs.FirstOrDefaultAsync(m => m.Id == id);
                if (dbBlog == null) return NotFound();

                if (!ModelState.IsValid) return View();

                if(blog.Photo is not null)
                {
                    if (!blog.Photo.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("Photo", "File type must be image");
                        return View();
                    }
                    if (!blog.Photo.CheckFileSize(200))
                    {
                        ModelState.AddModelError("Photo", "Image size must be max 200kb");
                        return View();
                    }
                    string oldPath = FileHelper.GetFilePath(_env.WebRootPath, "img", dbBlog.Image);
                    FileHelper.DeleteFile(oldPath);

                    dbBlog.Image = blog.Photo.CreateFile(_env, "img");
                }
                dbBlog.Title = blog.Title;
                dbBlog.Description = blog.Description;
                dbBlog.Date = blog.Date;

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.error = ex.Message;
                return View();
            }
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (!ModelState.IsValid) return View();
                if (id is null) return BadRequest();
                var dbBlog = await _context.Blogs.FirstOrDefaultAsync(b => b.Id == id);
                if (dbBlog is null) return NotFound();

                string path = FileHelper.GetFilePath(_env.WebRootPath, "img", dbBlog.Image);
                FileHelper.DeleteFile(path);

                _context.Blogs.Remove(dbBlog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.error = ex.Message;
                return View();
            }
        }
    }
}
