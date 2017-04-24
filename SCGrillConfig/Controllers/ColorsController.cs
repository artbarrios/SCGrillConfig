using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Web.Configuration;
using SCGrillConfig.Models;

namespace SCGrillConfig.Controllers
{
    public class ColorsController : Controller
    {
        private SCGrillConfigContext db = new SCGrillConfigContext();

        // GET: AddGrillConfgurationToColor
        public ActionResult AddGrillConfgurationToColor(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Color color = db.Colors.Find(id);
            var grillConfgurationsAvailable = db.GrillConfgurations.ToList().Except(color.GrillConfgurations.ToList()).ToList();
            ViewBag.GrillConfgurationId = new SelectList(grillConfgurationsAvailable, "Id", "Name");
            if (color == null)
            {
                return HttpNotFound();
            }
            ColorGrillConfgurationViewModel viewModel = new ColorGrillConfgurationViewModel();
            viewModel.ColorId = color.Id;
            viewModel.Color_Name = color.Name;
            return View(viewModel);
        }

        // POST: Colors/AddGrillConfgurationToColor/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddGrillConfgurationToColor([Bind(Include = "ColorId,Color_Name,GrillConfgurationId,GrillConfguration_Name")] ColorGrillConfgurationViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Color color = db.Colors.Find(viewModel.ColorId);
                GrillConfguration grillConfguration = db.GrillConfgurations.Find(viewModel.GrillConfgurationId);
                color.GrillConfgurations.Add(grillConfguration);
                db.Entry(color).State = EntityState.Modified;
                db.SaveChanges();
                LogManager.Log("Colors/AddGrillConfgurationToColor/ - GrillConfgurationId:" + grillConfguration.Id.ToString() + " to ColorId: " + color.Id.ToString());
                return RedirectToAction("Details", new { id = viewModel.ColorId });
            }
            return View(viewModel);
        }

        // GET: RemoveGrillConfgurationFromColor
        // NOTE: the GrillConfguration.ColorId property is not nullable so this routine must be omitted

        // GET: Colors
        public ActionResult Index()
        {
            string printerFriendlyUrl = WebConfigurationManager.AppSettings["AppEngineUrl"];
            if (printerFriendlyUrl.EndsWith("/")) { printerFriendlyUrl = printerFriendlyUrl.TrimEnd('/'); }
            printerFriendlyUrl += ":" + WebConfigurationManager.AppSettings["AppEnginePort"];
            printerFriendlyUrl += "/api/reports/ColorsIndexPrinterFriendly";
            ViewBag.PrinterFriendlyUrl = printerFriendlyUrl;
            ViewBag.AppEngineTimeout = WebConfigurationManager.AppSettings["AppEngineTimeout"];
            return View(db.Colors.ToList());
        }

        // GET: Colors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Color color = db.Colors.Find(id);
            if (color == null)
            {
                return HttpNotFound();
            }
            return View(color);
        }

        // GET: Colors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Colors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Color color, int? id, string modelType)
        {
            string controllerName = "";
            if (ModelState.IsValid)
            {
                db.Colors.Add(color);
                db.SaveChanges();
                LogManager.Log("Colors/Create - ColorId:" + color.Id.ToString());
                return RedirectToAction("Index");
            }

            return View(color);
        }

        // GET: Colors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Color color = db.Colors.Find(id);
            if (color == null)
            {
                return HttpNotFound();
            }
            return View(color);
        }

        // POST: Colors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Color color)
        {
            if (ModelState.IsValid)
            {
                db.Entry(color).State = EntityState.Modified;
                db.SaveChanges();
                LogManager.Log("Colors/Edit/ - ColorId:" + color.Id.ToString());
                return RedirectToAction("Index");
            }
            return View(color);
        }

        // GET: Colors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Color color = db.Colors.Find(id);
            if (color == null)
            {
                return HttpNotFound();
            }
            return View(color);
        }

        // POST: Colors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Color color = db.Colors.Find(id);
            db.Colors.Remove(color);
            db.SaveChanges();
            LogManager.Log("Colors/Delete/ - ColorId:" + color.Id.ToString());
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
