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
using Homework05.Models;

namespace Homework05.API_Controllers
{
    public class SurveyResponsesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/SurveyResponses
        public IQueryable<SurveyResponse> GetSurveyResponses()
        {
            return db.SurveyResponses;
        }

        // GET: api/SurveyResponses/5
        [ResponseType(typeof(SurveyResponse))]
        public IHttpActionResult GetSurveyResponse(string id)
        {
            SurveyResponse surveyResponse = db.SurveyResponses.Find(id);
            if (surveyResponse == null)
            {
                return NotFound();
            }

            return Ok(surveyResponse);
        }

        // PUT: api/SurveyResponses/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutSurveyResponse(string id, SurveyResponse surveyResponse)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != surveyResponse.SurveyResponseId)
            {
                return BadRequest();
            }

            db.Entry(surveyResponse).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SurveyResponseExists(id))
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

        // POST: api/SurveyResponses
        [ResponseType(typeof(SurveyResponse))]
        public IHttpActionResult PostSurveyResponse(SurveyResponse surveyResponse)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.SurveyResponses.Add(surveyResponse);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (SurveyResponseExists(surveyResponse.SurveyResponseId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = surveyResponse.SurveyResponseId }, surveyResponse);
        }

        // DELETE: api/SurveyResponses/5
        [ResponseType(typeof(SurveyResponse))]
        public IHttpActionResult DeleteSurveyResponse(string id)
        {
            SurveyResponse surveyResponse = db.SurveyResponses.Find(id);
            if (surveyResponse == null)
            {
                return NotFound();
            }

            db.SurveyResponses.Remove(surveyResponse);
            db.SaveChanges();

            return Ok(surveyResponse);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SurveyResponseExists(string id)
        {
            return db.SurveyResponses.Count(e => e.SurveyResponseId == id) > 0;
        }
    }
}