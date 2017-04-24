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
    public class FuelsDataController : ApiController
    {
        private SCGrillConfigContext db = new SCGrillConfigContext();

        // GET: api/GetFuelGrillConfgurations/?FuelId=1
        [Route("api/GetFuelGrillConfgurations/")]
        public List<GrillConfguration> GetFuelGrillConfgurations(int FuelId)
        {
            Fuel fuel = db.Fuels.Find(FuelId);
            if (fuel == null)
            {
                return null;
            }
            return fuel.GrillConfgurations;
        }

        // PUT: api/AddGrillConfgurationToFuel/?FuelId=1&GrillConfgurationId=1
        [HttpPut]
        [Route("api/AddGrillConfgurationToFuel/")]
        [ResponseType(typeof(void))]
        public IHttpActionResult AddGrillConfgurationToFuel(int FuelId, int GrillConfgurationId)
        {
            Fuel fuel = db.Fuels.Find(FuelId);
            GrillConfguration grillConfguration = db.GrillConfgurations.Find(GrillConfgurationId);
            if (fuel != null && grillConfguration != null)
            {
                try
                {
                    fuel.GrillConfgurations.Add(grillConfguration);
                    db.Entry(fuel).State = EntityState.Modified;
                    db.SaveChanges();
                    LogManager.Log("api/AddGrillConfgurationToFuel - FuelId:" + fuel.Id.ToString() + " GrillConfgurationId:" + grillConfguration.Id.ToString());
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
        } // AddGrillConfgurationToFuel

        // PUT: api/RemoveGrillConfgurationFromFuel/?FuelId=1&GrillConfgurationId=1
        // NOTE: the GrillConfguration.FuelId property is not nullable so this routine must be omitted

        // GET: api/FuelsData
        public IQueryable<Fuel> GetFuels()
        {
            return db.Fuels;
        }

        // GET: api/FuelsData/5
        [ResponseType(typeof(Fuel))]
        public IHttpActionResult GetFuel(int id)
        {
            Fuel fuel = db.Fuels.Find(id);
            if (fuel == null)
            {
                return NotFound();
            }

            return Ok(fuel);
        }

        // PUT: api/FuelsData/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutFuel(int id, Fuel fuel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != fuel.Id)
            {
                return BadRequest();
            }

            db.Entry(fuel).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
                LogManager.Log("PUT: api/FuelsData/ - FuelId:" + fuel.Id.ToString());
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FuelExists(id))
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

        // POST: api/FuelsData
        [ResponseType(typeof(Fuel))]
        public IHttpActionResult PostFuel(Fuel fuel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Fuels.Add(fuel);
            db.SaveChanges();
            LogManager.Log("POST: api/FuelsData/ - FuelId:" + fuel.Id.ToString());

            return CreatedAtRoute("DefaultApi", new { id = fuel.Id }, fuel);
        }

        // DELETE: api/FuelsData/5
        [ResponseType(typeof(Fuel))]
        public IHttpActionResult DeleteFuel(int id)
        {
            Fuel fuel = db.Fuels.Find(id);
            if (fuel == null)
            {
                return NotFound();
            }

            db.Fuels.Remove(fuel);
            db.SaveChanges();
            LogManager.Log("DELETE: api/FuelsData/ - FuelId:" + fuel.Id.ToString());

            return Ok(fuel);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FuelExists(int id)
        {
            return db.Fuels.Count(e => e.Id == id) > 0;
        }
    }
}
