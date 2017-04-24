using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using SCGrillConfig.Models;

namespace SCGrillConfig.Controllers
{
    public class BuildTasksDataController : ApiController
    {
        private SCGrillConfigContext db = new SCGrillConfigContext();

        // GET: api/GetBuildTaskGrillConfgurations/?BuildTaskId=1
        [Route("api/GetBuildTaskGrillConfgurations/")]
        public List<GrillConfguration> GetBuildTaskGrillConfgurations(int BuildTaskId)
        {
            BuildTask buildTask = db.BuildTasks.Find(BuildTaskId);
            if (buildTask == null)
            {
                return null;
            }
            return buildTask.GrillConfgurations;
        }

        // PUT: api/AddGrillConfgurationToBuildTask/?BuildTaskId=1&GrillConfgurationId=1
        [HttpPut]
        [Route("api/AddGrillConfgurationToBuildTask/")]
        [ResponseType(typeof(void))]
        public IHttpActionResult AddGrillConfgurationToBuildTask(int BuildTaskId, int GrillConfgurationId)
        {
            BuildTask buildTask = db.BuildTasks.Find(BuildTaskId);
            GrillConfguration grillConfguration = db.GrillConfgurations.Find(GrillConfgurationId);
            if (buildTask != null && grillConfguration != null)
            {
                try
                {
                    buildTask.GrillConfgurations.Add(grillConfguration);
                    db.Entry(buildTask).State = EntityState.Modified;
                    db.SaveChanges();
                    LogManager.Log("api/AddGrillConfgurationToBuildTask - BuildTaskId:" + buildTask.Id.ToString() + " GrillConfgurationId:" + grillConfguration.Id.ToString());
                }
                catch
                {
                    throw;
                }
            }
            else
            {
                return NotFound();
            }
            return StatusCode(HttpStatusCode.NoContent);
        } // AddGrillConfgurationToBuildTask

        // PUT: api/RemoveGrillConfgurationFromBuildTask/?BuildTaskId=1&GrillConfgurationId=1
        [HttpPut]
        [Route("api/RemoveGrillConfgurationFromBuildTask/")]
        [ResponseType(typeof(void))]
        public IHttpActionResult RemoveGrillConfgurationFromBuildTask(int BuildTaskId, int GrillConfgurationId)
        {
            BuildTask buildTask = db.BuildTasks.Find(BuildTaskId);
            GrillConfguration grillConfguration = db.GrillConfgurations.Find(GrillConfgurationId);
            if (buildTask != null && grillConfguration != null)
            {
                try
                {
                    buildTask.GrillConfgurations.Remove(grillConfguration);
                    db.Entry(buildTask).State = EntityState.Modified;
                    db.SaveChanges();
                    LogManager.Log("api/RemoveGrillConfgurationToBuildTask - BuildTaskId:" + buildTask.Id.ToString() + " GrillConfgurationId:" + grillConfguration.Id.ToString());
                }
                catch
                {
                    throw;
                }
            }
            else
            {
                return NotFound();
            }
            return StatusCode(HttpStatusCode.NoContent);
        } // RemoveGrillConfgurationFromBuildTask

        // GET: api/BuildTasksData
        public IQueryable<BuildTask> GetBuildTasks()
        {
            return db.BuildTasks;
        }

        // GET: api/BuildTasksData/5
        [ResponseType(typeof(BuildTask))]
        public IHttpActionResult GetBuildTask(int id)
        {
            BuildTask buildTask = db.BuildTasks.Find(id);
            if (buildTask == null)
            {
                return NotFound();
            }

            return Ok(buildTask);
        }

        // PUT: api/BuildTasksData/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBuildTask(int id, BuildTask buildTask)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != buildTask.Id)
            {
                return BadRequest();
            }

            db.Entry(buildTask).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
                LogManager.Log("PUT: api/BuildTasksData/ - BuildTaskId:" + buildTask.Id.ToString());
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BuildTaskExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/BuildTasksData
        [ResponseType(typeof(BuildTask))]
        public IHttpActionResult PostBuildTask(BuildTask buildTask)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.BuildTasks.Add(buildTask);
            db.SaveChanges();
            LogManager.Log("POST: api/BuildTasksData/ - BuildTaskId:" + buildTask.Id.ToString());

            return CreatedAtRoute("DefaultApi", new { id = buildTask.Id }, buildTask);
        }

        // DELETE: api/BuildTasksData/5
        [ResponseType(typeof(BuildTask))]
        public IHttpActionResult DeleteBuildTask(int id)
        {
            BuildTask buildTask = db.BuildTasks.Find(id);
            if (buildTask == null)
            {
                return NotFound();
            }

            db.BuildTasks.Remove(buildTask);
            db.SaveChanges();
            LogManager.Log("DELETE: api/BuildTasksData/ - BuildTaskId:" + buildTask.Id.ToString());

            return Ok(buildTask);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BuildTaskExists(int id)
        {
            return db.BuildTasks.Count(e => e.Id == id) > 0;
        }
    }
}
