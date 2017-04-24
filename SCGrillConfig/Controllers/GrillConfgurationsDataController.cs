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
    public class GrillConfgurationsDataController : ApiController
    {
        private SCGrillConfigContext db = new SCGrillConfigContext();

        // GET: api/GetGrillConfgurationBuildTasks/?GrillConfgurationId=1
        [Route("api/GetGrillConfgurationBuildTasks/")]
        public List<BuildTask> GetGrillConfgurationBuildTasks(int GrillConfgurationId)
        {
            GrillConfguration grillConfguration = db.GrillConfgurations.Find(GrillConfgurationId);
            if (grillConfguration == null)
            {
                return null;
            }
            return grillConfguration.BuildTasks;
        }

        // PUT: api/AddBuildTaskToGrillConfguration/?GrillConfgurationId=1&BuildTaskId=1
        [HttpPut]
        [Route("api/AddBuildTaskToGrillConfguration/")]
        [ResponseType(typeof(void))]
        public IHttpActionResult AddBuildTaskToGrillConfguration(int GrillConfgurationId, int BuildTaskId)
        {
            GrillConfguration grillConfguration = db.GrillConfgurations.Find(GrillConfgurationId);
            BuildTask buildTask = db.BuildTasks.Find(BuildTaskId);
            if (grillConfguration != null && buildTask != null)
            {
                try
                {
                    grillConfguration.BuildTasks.Add(buildTask);
                    db.Entry(grillConfguration).State = EntityState.Modified;
                    db.SaveChanges();
                    LogManager.Log("api/AddBuildTaskToGrillConfguration - GrillConfgurationId:" + grillConfguration.Id.ToString() + " BuildTaskId:" + buildTask.Id.ToString());
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
        } // AddBuildTaskToGrillConfguration

        // PUT: api/RemoveBuildTaskFromGrillConfguration/?GrillConfgurationId=1&BuildTaskId=1
        [HttpPut]
        [Route("api/RemoveBuildTaskFromGrillConfguration/")]
        [ResponseType(typeof(void))]
        public IHttpActionResult RemoveBuildTaskFromGrillConfguration(int GrillConfgurationId, int BuildTaskId)
        {
            GrillConfguration grillConfguration = db.GrillConfgurations.Find(GrillConfgurationId);
            BuildTask buildTask = db.BuildTasks.Find(BuildTaskId);
            if (grillConfguration != null && buildTask != null)
            {
                try
                {
                    grillConfguration.BuildTasks.Remove(buildTask);
                    db.Entry(grillConfguration).State = EntityState.Modified;
                    db.SaveChanges();
                    LogManager.Log("api/RemoveBuildTaskToGrillConfguration - GrillConfgurationId:" + grillConfguration.Id.ToString() + " BuildTaskId:" + buildTask.Id.ToString());
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
        } // RemoveBuildTaskFromGrillConfguration

        // GET: api/GrillConfgurationsData
        public IQueryable<GrillConfguration> GetGrillConfgurations()
        {
            return db.GrillConfgurations;
        }

        // GET: api/GrillConfgurationsData/5
        [ResponseType(typeof(GrillConfguration))]
        public IHttpActionResult GetGrillConfguration(int id)
        {
            GrillConfguration grillConfguration = db.GrillConfgurations.Find(id);
            if (grillConfguration == null)
            {
                return NotFound();
            }

            return Ok(grillConfguration);
        }

        // PUT: api/GrillConfgurationsData/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutGrillConfguration(int id, GrillConfguration grillConfguration)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != grillConfguration.Id)
            {
                return BadRequest();
            }

            db.Entry(grillConfguration).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
                LogManager.Log("PUT: api/GrillConfgurationsData/ - GrillConfgurationId:" + grillConfguration.Id.ToString());
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GrillConfgurationExists(id))
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

        // POST: api/GrillConfgurationsData
        [ResponseType(typeof(GrillConfguration))]
        public IHttpActionResult PostGrillConfguration(GrillConfguration grillConfguration)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.GrillConfgurations.Add(grillConfguration);
            db.SaveChanges();
            LogManager.Log("POST: api/GrillConfgurationsData/ - GrillConfgurationId:" + grillConfguration.Id.ToString());

            return CreatedAtRoute("DefaultApi", new { id = grillConfguration.Id }, grillConfguration);
        }

        // DELETE: api/GrillConfgurationsData/5
        [ResponseType(typeof(GrillConfguration))]
        public IHttpActionResult DeleteGrillConfguration(int id)
        {
            GrillConfguration grillConfguration = db.GrillConfgurations.Find(id);
            if (grillConfguration == null)
            {
                return NotFound();
            }

            db.GrillConfgurations.Remove(grillConfguration);
            db.SaveChanges();
            LogManager.Log("DELETE: api/GrillConfgurationsData/ - GrillConfgurationId:" + grillConfguration.Id.ToString());

            return Ok(grillConfguration);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool GrillConfgurationExists(int id)
        {
            return db.GrillConfgurations.Count(e => e.Id == id) > 0;
        }
    }
}
