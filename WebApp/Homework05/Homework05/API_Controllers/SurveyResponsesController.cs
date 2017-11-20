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
using Homework05.DTOs;
using System.Data.Entity.Validation;

namespace Homework05.API_Controllers
{
    [Authorize]
    [RoutePrefix("api/SurveyResponses")]
    public class SurveyResponsesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/SurveyResponses
        public IQueryable<SurveyResponse> GetSurveyResponses()
        {
            return db.SurveyResponses;
        }
        [ResponseType(typeof(SurveyResponse))]
        public IList<ResponseDTO> GetSurveyResponseForStudy(string studyGroupId)
        {
            var result = db.SurveyResponses.Include(r => r.StudyGroup)
                                            .Include(r => r.Survey)
                                            .Include(r => r.Survey.Question)
                                            .Include(r => r.User).Where(r => r.StudyGroupId == studyGroupId)
                                            .Select(r => new ResponseDTO
                                            {
                                                StudyGroupName = r.StudyGroup.StudyName,
                                                SurveyId = r.SurveyId,
                                                UserName = r.User.UserName,
                                                ResponseReceivedTime = r.SurveyResponseReceivedTime,
                                                ResponseText = r.UserResponseText,
                                                QuestionFrequency = ((Frequency)r.Survey.FrequencyOfNotifications).ToString(),
                                                QuestionText = r.Survey.Question.QuestionText,
                                                QuestionId = r.Survey.Question.QuestionId,
                                                QuestionType = r.Survey.Question.QuestionType,
                                                Options = r.Survey.Question.Options,
                                                ResponseId = r.SurveyResponseId

                                                // SurveyComments = r.SurveyComments
                                            });
            return result.ToList();

        }

        [ResponseType(typeof(SurveyResponse))]
        public IList<ResponseDTO> GetSurveyResponseOfUser(string userId)
        {
            var result = db.SurveyResponses.Include(r => r.StudyGroup)
                                            .Include(r => r.Survey)
                                            .Include(r => r.Survey.Question)
                                            .Include(r => r.User).Where(r => r.UserId == userId)
                                            .Select(r => new ResponseDTO
                                            {
                                                ResponseId = r.SurveyResponseId,
                                                StudyGroupName = r.StudyGroup.StudyName,
                                                SurveyId = r.SurveyId,
                                                UserName = r.User.UserName,
                                                ResponseReceivedTime = r.SurveyResponseReceivedTime,
                                                ResponseText = r.UserResponseText,
                                                QuestionFrequency = ((Frequency)r.Survey.FrequencyOfNotifications).ToString(),
                                                QuestionText = r.Survey.Question.QuestionText,
                                                QuestionId = r.Survey.Question.QuestionId,
                                                QuestionType = r.Survey.Question.QuestionType,
                                                Options = r.Survey.Question.Options                                            
                                            });
            return result.ToList();

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
            surveyResponse.SurveyResponseId = System.Guid.NewGuid().ToString();
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
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
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