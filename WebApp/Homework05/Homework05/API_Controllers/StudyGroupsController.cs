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
    public class StudyGroupsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/StudyGroups
        public IQueryable<StudyGroup> GetStudyGroups()
        {
            return db.StudyGroups;
        }

        public IList<StudyGroup> GetStudyGroupsForCoordinator(string coordinatorId)
        {
            var groups = db.StudyGroups.Where(s => s.StudyCoordinatorId.Equals(coordinatorId));
            return groups.ToList();
        }

        // GET: api/StudyGroups/5
        [ResponseType(typeof(StudyGroup))]
        public IHttpActionResult GetStudyGroup(string id)
        {
            StudyGroup studyGroup = db.StudyGroups.Find(id);
            if (studyGroup == null)
            {
                return NotFound();
            }

            return Ok(studyGroup);
        }

        // PUT: api/StudyGroups/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutStudyGroup(int id, StudyGroup studyGroup)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != studyGroup.Id)
            {
                return BadRequest();
            }

            db.Entry(studyGroup).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudyGroupExists(id))
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

        // POST: api/StudyGroups
        [ResponseType(typeof(StudyGroup))]
        public IHttpActionResult PostStudyGroup(StudyGroup studyGroup)
        {
            //Add group to StudyGroup and X_Coordinator_Group tables
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.StudyGroups.Add(studyGroup);
            db.X_Coordinator_Groups.Add(new X_Coordinator_Group { StudyGroupId = studyGroup.Id, CoordinatorId = studyGroup.StudyCoordinatorId});

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (StudyGroupExists(studyGroup.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = studyGroup.Id }, studyGroup);
        }

        // DELETE: api/StudyGroups/5
        [ResponseType(typeof(StudyGroup))]
        public IHttpActionResult DeleteStudyGroup(string id)
        {
            StudyGroup studyGroup = db.StudyGroups.Find(id);
            if (studyGroup == null)
            {
                return NotFound();
            }

            db.StudyGroups.Remove(studyGroup);
            db.SaveChanges();

            return Ok(studyGroup);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StudyGroupExists(int id)
        {
            return db.StudyGroups.Count(e => e.Id == id) > 0;
        }
    }
}