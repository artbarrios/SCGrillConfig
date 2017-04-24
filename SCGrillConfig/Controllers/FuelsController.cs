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
    public class FuelsController : Controller
    {
        private SCGrillConfigContext db = new SCGrillConfigContext();

        // GET: AddGrillConfgurationToFuel
        public ActionResult AddGrillConfgurationToFuel(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fuel fuel = db.Fuels.Find(id);
            var grillConfgurationsAvailable = db.GrillConfgurations.ToList().Except(fuel.GrillConfgurations.ToList()).ToList();
            ViewBag.GrillConfgurationId = new SelectList(grillConfgurationsAvailable, "Id", "Name");
            if (fuel == null)
            {
                return HttpNotFound();
            }
            FuelGrillConfgurationViewModel viewModel = new FuelGrillConfgurationViewModel();
            viewModel.FuelId = fuel.Id;
            viewModel.Fuel_Name = fuel.Name;
            return View(viewModel);
        }

        // POST: Fuels/AddGrillConfgurationToFuel/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddGrillConfgurationToFuel([Bind(Include = "FuelId,Fuel_Name,GrillConfgurationId,GrillConfguration_Name")] FuelGrillConfgurationViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Fuel fuel = db.Fuels.Find(viewModel.FuelId);
                GrillConfguration grillConfguration = db.GrillConfgurations.Find(viewModel.GrillConfgurationId);
                fuel.GrillConfgurations.Add(grillConfguration);
                db.Entry(fuel).State = EntityState.Modified;
                db.SaveChanges();
                LogManager.Log("Fuels/AddGrillConfgurationToFuel/ - GrillConfgurationId:" + grillConfguration.Id.ToString() + " to FuelId: " + fuel.Id.ToString());
                return RedirectToAction("Details", new { id = viewModel.FuelId });
            }
            return View(viewModel);
        }

        // GET: RemoveGrillConfgurationFromFuel
        // NOTE: the GrillConfguration.FuelId property is not nullable so this routine must be omitted

        // GET: Fuels
        public ActionResult Index()
        {
            string printerFriendlyUrl = WebConfigurationManager.AppSettings["AppEngineUrl"];
            if (printerFriendlyUrl.EndsWith("/")) { printerFriendlyUrl = printerFriendlyUrl.TrimEnd('/'); }
            printerFriendlyUrl += ":" + WebConfigurationManager.AppSettings["AppEnginePort"];
            printerFriendlyUrl += "/api/reports/FuelsIndexPrinterFriendly";
            ViewBag.PrinterFriendlyUrl = printerFriendlyUrl;
            ViewBag.AppEngineTimeout = WebConfigurationManager.AppSettings["AppEngineTimeout"];
            return View(db.Fuels.ToList());
        }

        // GET: Fuels/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fuel fuel = db.Fuels.Find(id);
            if (fuel == null)
            {
                return HttpNotFound();
            }
            return View(fuel);
        }

        // GET: Fuels/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Fuels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Fuel fuel, int? id, string modelType)
        {
            string controllerName = "";
            if (ModelState.IsValid)
            {
                db.Fuels.Add(fuel);
                db.SaveChanges();
                LogManager.Log("Fuels/Create - FuelId:" + fuel.Id.ToString());
                return RedirectToAction("Index");
            }

            return View(fuel);
        }

        // GET: Fuels/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fuel fuel = db.Fuels.Find(id);
            if (fuel == null)
            {
                return HttpNotFound();
            }
            return View(fuel);
        }

        // POST: Fuels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Fuel fuel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fuel).State = EntityState.Modified;
                db.SaveChanges();
                LogManager.Log("Fuels/Edit/ - FuelId:" + fuel.Id.ToString());
                return RedirectToAction("Index");
            }
            return View(fuel);
        }

        // GET: Fuels/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fuel fuel = db.Fuels.Find(id);
            if (fuel == null)
            {
                return HttpNotFound();
            }
            return View(fuel);
        }

        // POST: Fuels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Fuel fuel = db.Fuels.Find(id);
            db.Fuels.Remove(fuel);
            db.SaveChanges();
            LogManager.Log("Fuels/Delete/ - FuelId:" + fuel.Id.ToString());
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
