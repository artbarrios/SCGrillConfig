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
    public class GrillConfgurationsController : Controller
    {
        private SCGrillConfigContext db = new SCGrillConfigContext();

        // GrillConfgurations/GrillConfgurationBuildTasksFlowchartDiagram/1
        public ActionResult GrillConfgurationBuildTasksFlowchartDiagram(int? id)
        {
            GrillConfguration grillConfguration = db.GrillConfgurations.Find(id);

            if (grillConfguration.BuildTaskFlowchartDiagramData == null || grillConfguration.BuildTaskFlowchartDiagramData.Length == 0)
            {
                Flowchart flowchart = GrillConfgurationBuildTasksToFlowchart(grillConfguration);
                grillConfguration.BuildTaskFlowchartDiagramData = flowchart.ToJSON();
                db.Entry(grillConfguration).State = EntityState.Modified;
                db.SaveChanges();
            }
            ViewBag.FlowchartTitle = "BuildTask Diagram for " + grillConfguration.Name;
            ViewBag.FlowchartData = grillConfguration.BuildTaskFlowchartDiagramData;
            return View(grillConfguration);
        }

        // POST: GrillConfgurations/GrillConfgurationBuildTasksFlowchartDiagram/1
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult GrillConfgurationBuildTasksFlowchartDiagram([Bind(Include = "Id")] GrillConfguration grillConfguration, string flowchartData)
        {
            grillConfguration = db.GrillConfgurations.Find(grillConfguration.Id);
            if (flowchartData.Length > 0)
            {
                grillConfguration.BuildTaskFlowchartDiagramData = flowchartData;
                db.SaveChanges();
            }
            return RedirectToAction("Details", new { id = grillConfguration.Id });
        }

        // GrillConfgurationBuildTasksToFlowchart
        public static Flowchart GrillConfgurationBuildTasksToFlowchart(GrillConfguration grillConfguration)
        {
            // converts the specified objects into a Flowchart object and returns it
            Flowchart flowchart = new Flowchart();
            FlowchartOperator fcOperator = null;
            FlowchartOperator fcOperatorPrevious = null;
            FlowchartConnector fcInput = null;
            FlowchartConnector fcOutput = null;
            FlowchartLink fcLink = null;
            int top = 0;
            int left = 0;
            int opCount = 0;

            // check for valid input
            if (grillConfguration != null)
            {
                flowchart.Id = grillConfguration.Id.ToString();
                // add operators
                opCount = 1;
                foreach (BuildTask buildTask in grillConfguration.BuildTasks)
                {
                    fcOperator = new FlowchartOperator();
                    fcOperator.Id = "op" + grillConfguration.Id.ToString() + buildTask.Id.ToString();
                    fcOperator.Title = buildTask.Name;
                    fcOperator.Top = top;
                    fcOperator.Left = left;
                    top += 20;
                    left += 200;
                    // inputs
                    fcInput = new FlowchartConnector();
                    fcInput.Id = fcOperator.Id + "in1";
                    fcInput.Label = "In";
                    fcOperator.Inputs.Add(fcInput);
                    // outputs
                    fcOutput = new FlowchartConnector();
                    fcOutput.Id = fcOperator.Id + "out1";
                    fcOutput.Label = "Out";
                    fcOperator.Outputs.Add(fcOutput);
                    // popup
                    fcOperator.Popup.header = "<h2>" + buildTask.Name + "</h2>";
                    fcOperator.Popup.body = @"<p>Additional Item Information Goes Here</p>";
                    // add the operator
                    flowchart.Operators.Add(fcOperator);
                    opCount += 1;
                }

                // add links
                foreach (FlowchartOperator myOperator in flowchart.Operators)
                {
                    if (fcOperatorPrevious != null)
                    {
                        fcLink = new FlowchartLink();
                        fcLink.Id = myOperator.Id + "lnk1";
                        fcLink.FromOperatorId = fcOperatorPrevious.Id;
                        fcLink.FromConnectorId = fcOperatorPrevious.Outputs.FirstOrDefault().Id;
                        fcLink.ToOperatorId = myOperator.Id;
                        fcLink.ToConnectorId = myOperator.Inputs.FirstOrDefault().Id;
                        flowchart.Links.Add(fcLink);
                    }
                    fcOperatorPrevious = myOperator;
                }
            }
            return flowchart;
        } // GrillConfgurationBuildTasksToFlowchart ()

        // GET: AddBuildTaskToGrillConfguration
        public ActionResult AddBuildTaskToGrillConfguration(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GrillConfguration grillConfguration = db.GrillConfgurations.Find(id);
            var buildTasksAvailable = db.BuildTasks.ToList().Except(grillConfguration.BuildTasks.ToList()).ToList();
            ViewBag.BuildTaskId = new SelectList(buildTasksAvailable, "Id", "Name");
            if (grillConfguration == null)
            {
                return HttpNotFound();
            }
            BuildTaskGrillConfgurationViewModel viewModel = new BuildTaskGrillConfgurationViewModel();
            viewModel.GrillConfgurationId = grillConfguration.Id;
            viewModel.GrillConfguration_Name = grillConfguration.Name;
            return View(viewModel);
        }

        // POST: GrillConfgurations/AddBuildTaskToGrillConfguration/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddBuildTaskToGrillConfguration([Bind(Include = "GrillConfgurationId,GrillConfguration_Name,BuildTaskId,BuildTask_Name")] BuildTaskGrillConfgurationViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                GrillConfguration grillConfguration = db.GrillConfgurations.Find(viewModel.GrillConfgurationId);
                BuildTask buildTask = db.BuildTasks.Find(viewModel.BuildTaskId);
                grillConfguration.BuildTasks.Add(buildTask);
                Flowchart flowchart = GrillConfgurationBuildTasksToFlowchart(grillConfguration);
                grillConfguration.BuildTaskFlowchartDiagramData = flowchart.ToJSON();
                db.Entry(grillConfguration).State = EntityState.Modified;
                db.SaveChanges();
                LogManager.Log("GrillConfgurations/AddBuildTaskToGrillConfguration/ - BuildTaskId:" + buildTask.Id.ToString() + " to GrillConfgurationId: " + grillConfguration.Id.ToString());
                return RedirectToAction("Details", new { id = viewModel.GrillConfgurationId });
            }
            return View(viewModel);
        }

        // GET: RemoveBuildTaskFromGrillConfguration
        public ActionResult RemoveBuildTaskFromGrillConfguration(int? grillConfgurationId, int? buildTaskId)
        {
            if (grillConfgurationId == null || buildTaskId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GrillConfguration grillConfguration = db.GrillConfgurations.Find(grillConfgurationId);
            BuildTask buildTask = db.BuildTasks.Find(buildTaskId);
            if (grillConfguration == null || buildTask == null)
            {
                return HttpNotFound();
            }
            grillConfguration.BuildTasks.Remove(buildTask);
                Flowchart flowchart = GrillConfgurationBuildTasksToFlowchart(grillConfguration);
                grillConfguration.BuildTaskFlowchartDiagramData = flowchart.ToJSON();
            db.Entry(grillConfguration).State = EntityState.Modified;
            db.SaveChanges();
            LogManager.Log("GrillConfgurations/RemoveBuildTaskFromGrillConfguration/ - BuildTaskId:" + buildTask.Id.ToString() + " from GrillConfgurationId: " + grillConfguration.Id.ToString());
            return RedirectToAction("Details", new { id = grillConfgurationId });
        }

        // GET: GrillConfgurations
        public ActionResult Index()
        {
            string printerFriendlyUrl = WebConfigurationManager.AppSettings["AppEngineUrl"];
            if (printerFriendlyUrl.EndsWith("/")) { printerFriendlyUrl = printerFriendlyUrl.TrimEnd('/'); }
            printerFriendlyUrl += ":" + WebConfigurationManager.AppSettings["AppEnginePort"];
            printerFriendlyUrl += "/api/reports/GrillConfgurationsIndexPrinterFriendly";
            ViewBag.PrinterFriendlyUrl = printerFriendlyUrl;
            ViewBag.AppEngineTimeout = WebConfigurationManager.AppSettings["AppEngineTimeout"];
            var grillConfgurations = db.GrillConfgurations.Include(g => g.GrillType).Include(g => g.Fuel).Include(g => g.SideBurnerType).Include(g => g.GrillSize).Include(g => g.Material).Include(g => g.Color);
            return View(grillConfgurations.ToList());
        }

        // GET: GrillConfgurations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GrillConfguration grillConfguration = db.GrillConfgurations.Find(id);
            if (grillConfguration == null)
            {
                return HttpNotFound();
            }
            return View(grillConfguration);
        }

        // GET: GrillConfgurations/Create
        public ActionResult Create(int? id, string modelType)
        {
            if (modelType != null && modelType.Length > 0)
            {
                ViewBag.GrillTypeId = new SelectList(db.GrillTypes, "Id", "Name", modelType.Contains("GrillType") ? id : -1);
                ViewBag.FuelId = new SelectList(db.Fuels, "Id", "Name", modelType.Contains("Fuel") ? id : -1);
                ViewBag.SideBurnerTypeId = new SelectList(db.SideBurnerTypes, "Id", "Name", modelType.Contains("SideBurnerType") ? id : -1);
                ViewBag.GrillSizeId = new SelectList(db.GrillSizes, "Id", "Name", modelType.Contains("GrillSize") ? id : -1);
                ViewBag.MaterialId = new SelectList(db.Materials, "Id", "Name", modelType.Contains("Material") ? id : -1);
                ViewBag.ColorId = new SelectList(db.Colors, "Id", "Name", modelType.Contains("Color") ? id : -1);
            }
            else
            {
                ViewBag.GrillTypeId = new SelectList(db.GrillTypes, "Id", "Name");
                ViewBag.FuelId = new SelectList(db.Fuels, "Id", "Name");
                ViewBag.SideBurnerTypeId = new SelectList(db.SideBurnerTypes, "Id", "Name");
                ViewBag.GrillSizeId = new SelectList(db.GrillSizes, "Id", "Name");
                ViewBag.MaterialId = new SelectList(db.Materials, "Id", "Name");
                ViewBag.ColorId = new SelectList(db.Colors, "Id", "Name");
            }
            // modelType not always properly being passed as query string parameter so send it in ViewBag
            ViewBag.modelType = modelType;
            return View();
        }

        // POST: GrillConfgurations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,MainBurnerCount,InfraredBurnerCount,GrillTypeId,FuelId,SideBurnerTypeId,GrillSizeId,MaterialId,ColorId,BuildTaskFlowchartDiagramData")] GrillConfguration grillConfguration, int? id, string modelType)
        {
            string controllerName = "";
            if (ModelState.IsValid)
            {
                db.GrillConfgurations.Add(grillConfguration);
                db.SaveChanges();
                LogManager.Log("GrillConfgurations/Create - GrillConfgurationId:" + grillConfguration.Id.ToString());
                if (modelType != null && modelType.Length > 0)
                {
                    if (modelType.Contains("GrillType"))
                    {
                        db.GrillTypes.Find(id).GrillConfgurations.Add(grillConfguration);
                        db.SaveChanges();
                        LogManager.Log("GrillConfgurations/Create added - GrillConfgurationId:" + grillConfguration.Id.ToString() + " to GrillTypeId: " + id.ToString());
                        controllerName = "GrillTypes";
                    }
                    if (modelType.Contains("Fuel"))
                    {
                        db.Fuels.Find(id).GrillConfgurations.Add(grillConfguration);
                        db.SaveChanges();
                        LogManager.Log("GrillConfgurations/Create added - GrillConfgurationId:" + grillConfguration.Id.ToString() + " to FuelId: " + id.ToString());
                        controllerName = "Fuels";
                    }
                    if (modelType.Contains("SideBurnerType"))
                    {
                        db.SideBurnerTypes.Find(id).GrillConfgurations.Add(grillConfguration);
                        db.SaveChanges();
                        LogManager.Log("GrillConfgurations/Create added - GrillConfgurationId:" + grillConfguration.Id.ToString() + " to SideBurnerTypeId: " + id.ToString());
                        controllerName = "SideBurnerTypes";
                    }
                    if (modelType.Contains("GrillSize"))
                    {
                        db.GrillSizes.Find(id).GrillConfgurations.Add(grillConfguration);
                        db.SaveChanges();
                        LogManager.Log("GrillConfgurations/Create added - GrillConfgurationId:" + grillConfguration.Id.ToString() + " to GrillSizeId: " + id.ToString());
                        controllerName = "GrillSizes";
                    }
                    if (modelType.Contains("Material"))
                    {
                        db.Materials.Find(id).GrillConfgurations.Add(grillConfguration);
                        db.SaveChanges();
                        LogManager.Log("GrillConfgurations/Create added - GrillConfgurationId:" + grillConfguration.Id.ToString() + " to MaterialId: " + id.ToString());
                        controllerName = "Materials";
                    }
                    if (modelType.Contains("Color"))
                    {
                        db.Colors.Find(id).GrillConfgurations.Add(grillConfguration);
                        db.SaveChanges();
                        LogManager.Log("GrillConfgurations/Create added - GrillConfgurationId:" + grillConfguration.Id.ToString() + " to ColorId: " + id.ToString());
                        controllerName = "Colors";
                    }
                    if (modelType.Contains("BuildTask"))
                    {
                        db.BuildTasks.Find(id).GrillConfgurations.Add(grillConfguration);
                        db.SaveChanges();
                        LogManager.Log("GrillConfgurations/Create added - GrillConfgurationId:" + grillConfguration.Id.ToString() + " to BuildTaskId: " + id.ToString());
                        controllerName = "BuildTasks";
                    }
                    return RedirectToAction("Details", controllerName, new { Id = id });
                }
                return RedirectToAction("Index");
            }
            ViewBag.GrillTypeId = new SelectList(db.GrillTypes, "Id", "Name", grillConfguration.GrillTypeId);
            ViewBag.FuelId = new SelectList(db.Fuels, "Id", "Name", grillConfguration.FuelId);
            ViewBag.SideBurnerTypeId = new SelectList(db.SideBurnerTypes, "Id", "Name", grillConfguration.SideBurnerTypeId);
            ViewBag.GrillSizeId = new SelectList(db.GrillSizes, "Id", "Name", grillConfguration.GrillSizeId);
            ViewBag.MaterialId = new SelectList(db.Materials, "Id", "Name", grillConfguration.MaterialId);
            ViewBag.ColorId = new SelectList(db.Colors, "Id", "Name", grillConfguration.ColorId);

            return View(grillConfguration);
        }

        // GET: GrillConfgurations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GrillConfguration grillConfguration = db.GrillConfgurations.Find(id);
            if (grillConfguration == null)
            {
                return HttpNotFound();
            }
            ViewBag.GrillTypeId = new SelectList(db.GrillTypes, "Id", "Name", grillConfguration.GrillTypeId);
            ViewBag.FuelId = new SelectList(db.Fuels, "Id", "Name", grillConfguration.FuelId);
            ViewBag.SideBurnerTypeId = new SelectList(db.SideBurnerTypes, "Id", "Name", grillConfguration.SideBurnerTypeId);
            ViewBag.GrillSizeId = new SelectList(db.GrillSizes, "Id", "Name", grillConfguration.GrillSizeId);
            ViewBag.MaterialId = new SelectList(db.Materials, "Id", "Name", grillConfguration.MaterialId);
            ViewBag.ColorId = new SelectList(db.Colors, "Id", "Name", grillConfguration.ColorId);

            return View(grillConfguration);
        }

        // POST: GrillConfgurations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,MainBurnerCount,InfraredBurnerCount,GrillTypeId,FuelId,SideBurnerTypeId,GrillSizeId,MaterialId,ColorId,BuildTaskFlowchartDiagramData")] GrillConfguration grillConfguration)
        {
            if (ModelState.IsValid)
            {
                db.Entry(grillConfguration).State = EntityState.Modified;
                db.SaveChanges();
                LogManager.Log("GrillConfgurations/Edit/ - GrillConfgurationId:" + grillConfguration.Id.ToString());
                return RedirectToAction("Index");
            }
            ViewBag.GrillTypeId = new SelectList(db.GrillTypes, "Id", "Name", grillConfguration.GrillTypeId);
            ViewBag.FuelId = new SelectList(db.Fuels, "Id", "Name", grillConfguration.FuelId);
            ViewBag.SideBurnerTypeId = new SelectList(db.SideBurnerTypes, "Id", "Name", grillConfguration.SideBurnerTypeId);
            ViewBag.GrillSizeId = new SelectList(db.GrillSizes, "Id", "Name", grillConfguration.GrillSizeId);
            ViewBag.MaterialId = new SelectList(db.Materials, "Id", "Name", grillConfguration.MaterialId);
            ViewBag.ColorId = new SelectList(db.Colors, "Id", "Name", grillConfguration.ColorId);

            return View(grillConfguration);
        }

        // GET: GrillConfgurations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GrillConfguration grillConfguration = db.GrillConfgurations.Find(id);
            if (grillConfguration == null)
            {
                return HttpNotFound();
            }
            return View(grillConfguration);
        }

        // POST: GrillConfgurations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GrillConfguration grillConfguration = db.GrillConfgurations.Find(id);
            db.GrillConfgurations.Remove(grillConfguration);
            db.SaveChanges();
            LogManager.Log("GrillConfgurations/Delete/ - GrillConfgurationId:" + grillConfguration.Id.ToString());
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
