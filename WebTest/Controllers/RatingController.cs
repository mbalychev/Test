using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTest.Services;
using WebTest.Interfaces;
using WebTest.Models;
using System.Linq;

namespace WebTest.Controllers
{
    public class RatingController : Controller
    {
        IServices<OrgRatingsModel> ratings;
        public RatingController(IServices<OrgRatingsModel> service)
        {
            ratings = service;
        }
        // GET: RatingController1
        public async Task<ActionResult> Index()
        {
            List<OrgRatingsModel> models = await ratings.ReadAllAsync();
            return View(models.OrderBy(x=>x.Rating).ToList());
        }

        // GET: RatingController1/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: RatingController1/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RatingController1/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string inn, string rating)
        {
            try
            {
                ratings.CreateAsync(inn, rating);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View();
            }
        }

        // GET: RatingController1/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: RatingController1/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: RatingController1/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: RatingController1/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
