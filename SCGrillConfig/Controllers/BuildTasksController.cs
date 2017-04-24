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
    public class BuildTasksController : Controller
    {
        private SCGrillConfigContext db = new SCGrillConfigContext();

        // GET: AddGrillConfgurationToBuildTask
        public ActionResult AddGrillConfgurationToBuildTask(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BuildTask buildTask = db.BuildTasks.Find(id);
            var grillConfgurationsAvailable = db.GrillConfgurations.ToList().Except(buildTask.GrillConfgurations.ToList()).ToList();
            ViewBag.GrillConfgurationId = new SelectList(grillConfgurationsAvailable, "Id", "Name");
            if (buildTask == null)
            {
                return HttpNotFound();
            }
            BuildTaskGrillConfgurationViewModel viewModel = new BuildTaskGrillConfgurationViewModel();
            viewModel.BuildTaskId = buildTask.Id;
            viewModel.BuildTask_Name = buildTask.Name;
            return View(viewModel);
        }

        // POST: BuildTasks/AddGrillConfgurationToBuildTask/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddGrillConfgurationToBuildTask([Bind(Include = "BuildTaskId,BuildTask_Name,GrillConfgurationId,GrillConfguration_Name")] BuildTaskGrillConfgurationViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                BuildTask buildTask = db.BuildTasks.Find(viewModel.BuildTaskId);
                GrillConfguration grillConfguration = db.GrillConfgurations.Find(viewModel.GrillConfgurationId);
                buildTask.GrillConfgurations.Add(grillConfguration);
                db.Entry(buildTask).State = EntityState.Modified;
                db.SaveChanges();
                LogManager.Log("BuildTasks/AddGrillConfgurationToBuildTask/ - GrillConfgurationId:" + grillConfguration.Id.ToString() + " to BuildTaskId: " + buildTask.Id.ToString());
                return RedirectToAction("Details", new { id = viewModel.BuildTaskId });
            }
            return View(viewModel);
        }

        // GET: RemoveGrillConfgurationFromBuildTask
        public ActionResult RemoveGrillConfgurationFromBuildTask(int? buildTaskId, int? grillConfgurationId)
        {
            if (buildTaskId == null || grillConfgurationId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BuildTask buildTask = db.BuildTasks.Find(buildTaskId);
            GrillConfguration grillConfguration = db.GrillConfgurations.Find(grillConfgurationId);
            if (buildTask == null || grillConfguration == null)
            {
                return HttpNotFound();
            }
            buildTask.GrillConfgurations.Remove(grillConfguration);
            db.Entry(buildTask).State = EntityState.Modified;
            db.SaveChanges();
            LogManager.Log("BuildTasks/RemoveGrillConfgurationFromBuildTask/ - GrillConfgurationId:" + grillConfguration.Id.ToString() + " from BuildTaskId: " + buildTask.Id.ToString());
            return RedirectToAction("Details", new { id = buildTaskId });
        }

        // GET: BuildTasks
        public ActionResult Index()
        {
            string printerFriendlyUrl = WebConfigurationManager.AppSettings["AppEngineUrl"];
            if (printerFriendlyUrl.EndsWith("/")) { printerFriendlyUrl = printerFriendlyUrl.TrimEnd('/'); }
            printerFriendlyUrl += ":" + WebConfigurationManager.AppSettings["AppEnginePort"];
            printerFriendlyUrl += "/api/reports/BuildTasksIndexPrinterFriendly";
            ViewBag.PrinterFriendlyUrl = printerFriendlyUrl;
            ViewBag.AppEngineTimeout = WebConfigurationManager.AppSettings["AppEngineTimeout"];
            return View(db.BuildTasks.ToList());
        }

        // GET: BuildTasks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BuildTask buildTask = db.BuildTasks.Find(id);
            if (buildTask == null)
            {
                return HttpNotFound();
            }
            return View(buildTask);
        }

        // GET: BuildTasks/Create
        public ActionResult Create(int? id, string modelType)
        {
            if (modelType != null && modelType.Length > 0)
            {
            }
            else
            {
            }
            // modelType not always properly being passed as query string parameter so send it in ViewBag
            ViewBag.modelType = modelType;
            return View();
        }

        // POST: BuildTasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Notes")] BuildTask buildTask, int? id, string modelType)
        {
            string controllerName = "";
            if (ModelState.IsValid)
            {
                db.BuildTasks.Add(buildTask);
                db.SaveChanges();
                LogManager.Log("BuildTasks/Create - BuildTaskId:" + buildTask.Id.ToString());
                if (modelType != null && modelType.Length > 0)
                {
                    if (modelType.Contains("GrillConfguration"))
                    {
                        db.GrillConfgurations.Find(id).BuildTasks.Add(buildTask);
                        db.SaveChanges();
                        LogManager.Log("BuildTasks/Create added - BuildTaskId:" + buildTask.Id.ToString() + " to GrillConfgurationId: " + id.ToString());
                        controllerName = "GrillConfgurations";
                    }
                    return RedirectToAction("Details", controllerName, new { Id = id });
                }
                return RedirectToAction("Index");
            }

            return View(buildTask);
        }

        // GET: BuildTasks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BuildTask buildTask = db.BuildTasks.Find(id);
            if (buildTask == null)
            {
                return HttpNotFound();
            }
            return View(buildTask);
        }

        // POST: BuildTasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Notes")] BuildTask buildTask)
        {
            if (ModelState.IsValid)
            {
                db.Entry(buildTask).State = EntityState.Modified;
                db.SaveChanges();
                LogManager.Log("BuildTasks/Edit/ - BuildTaskId:" + buildTask.Id.ToString());
                return RedirectToAction("Index");
            }
            return View(buildTask);
        }

        // GET: BuildTasks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BuildTask buildTask = db.BuildTasks.Find(id);
            if (buildTask == null)
            {
                return HttpNotFound();
            }
            return View(buildTask);
        }

        // POST: BuildTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BuildTask buildTask = db.BuildTasks.Find(id);
            db.BuildTasks.Remove(buildTask);
            db.SaveChanges();
            LogManager.Log("BuildTasks/Delete/ - BuildTaskId:" + buildTask.Id.ToString());
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
