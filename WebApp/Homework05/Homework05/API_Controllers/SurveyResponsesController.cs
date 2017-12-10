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

        //Get responses for a study group
        //Get responses for a study coordinator
        //Get responses for a user
        //Get responses of a survey

        // GET: api/SurveyResponses
        public IList<ResponseDTO> GetSurveyResponses()
        {
            var ques = db.SurveyResponses.GroupBy(r => r.SurveyId);
            /* var result = db.SurveyResponses.Include(r => r.StudyGroup)
                                            .Include(r => r.Survey)
                                            .Include(r => r.Survey.Question)
                                            .Include(r => r.User)
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
             return result.ToList();*/

            return null;
        }

        [Route("StudyResponses")]
        //Get responses for a study group
        [ResponseType(typeof(SurveyResponse))]
        public IList<ResponseDTO> GetSurveyResponseForStudy(int studyGroupId)
        {
            List<ResponseDTO> responses = new List<ResponseDTO>();

          //  var surveysForStudyGroup = db.X_Survey_Groups.Where(r => r.StudyGroupId == studyGroupId);

            var surveysForStudyGroup = from u in db.X_Survey_Groups
                                join r in db.X_User_Groups.Include("User")
                                on u.StudyGroupId equals r.StudyGroupId
                                where u.StudyGroupId == studyGroupId
                                select new { r, u };

            foreach (var survey in surveysForStudyGroup)
            {
                var responsesForQuestionsInSurvey = db.SurveyResponses
                                                       .Where(r => r.SurveyId == survey.u.SurveyId)
                                                       .Include(r => r.Question)
                                                       .Select(r => new QuestionResponseDTO
                                                       {
                                                           ResponseReceivedTime = r.ResponseReceivedTime,
                                                           ResponseText = r.ResponseText,
                                                           QuestionText = r.Question.QuestionText,
                                                           QuestionId = r.Question.Id,
                                                           QuestionType = r.Question.QuestionType,
                                                           Options = r.Question.Options
                                                       })
                                                       .ToList();

                var responseDTO = new ResponseDTO
                {
                    SurveyId = survey.u.SurveyId,
                    UserName = survey.r.User.UserName,
                    QuestionResponses = responsesForQuestionsInSurvey
                };

                responses.Add(responseDTO);
            }

            return responses;
            /* var result = db.SurveyResponses.Include(r => r.StudyGroup)
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
             return result.ToList();*/           


        }

        [Route("CoordinatorSurveyResponses")]
        //Get responses for a coordinator
        [ResponseType(typeof(SurveyResponse))]
        public IList<ResponseDTO> GetResponsesForStudyCoordinator(int coordinatorId)
        {
            List<ResponseDTO> responses = new List<ResponseDTO>();

            var surveysForCoordinator = from s in db.StudyGroups

                                        join g in db.X_Survey_Groups.Include("Survey") on s.Id equals g.StudyGroupId

                                        join r in db.X_User_Groups.Include("User") on g.StudyGroupId equals r.StudyGroupId

                                        where s.StudyCoordinatorId.Equals(coordinatorId)

                                        select new { s, g, r };

            foreach (var user in surveysForCoordinator)
            {
                var responsesForQuestionsInSurvey = db.SurveyResponses
                                                         .Where(r => r.SurveyId == user.g.SurveyId)
                                                         .Include(r => r.Question)
                                                         .Select(r => new QuestionResponseDTO
                                                         {
                                                             ResponseReceivedTime = r.ResponseReceivedTime,
                                                             ResponseText = r.ResponseText,
                                                             QuestionText = r.Question.QuestionText,
                                                             QuestionId = r.Question.Id,
                                                             QuestionType = r.Question.QuestionType,
                                                             Options = r.Question.Options
                                                         })
                                                         .ToList();

                var responseDTO = new ResponseDTO
                {
                    SurveyId = user.g.SurveyId,
                    UserName = user.r.User.UserName,
                    QuestionResponses = responsesForQuestionsInSurvey
                };

                responses.Add(responseDTO);
            }

            return responses;
        }

        [Route("SurveyResponses")]
        //Get responses for a survey
        [ResponseType(typeof(SurveyResponse))]
        public IList<ResponseDTO> GetResponsesForSurvey(int surveyId)
        {
            List<ResponseDTO> responses = new List<ResponseDTO>();

            var usersInSurvey = from u in db.X_Survey_Groups
                                join r in db.X_User_Groups.Include("User") on u.StudyGroupId equals r.StudyGroupId
                                where u.SurveyId == surveyId
                                select  new { r, u };

            foreach(var user in usersInSurvey)
            {
                var responsesForQuestionsInSurvey = db.SurveyResponses
                                                         .Where(r => r.SurveyId == surveyId)
                                                         .Where(r => r.UserId.Equals(user.r.UserId))
                                                         .Include(r => r.Question)
                                                         .Select(r => new QuestionResponseDTO
                                                         {
                                                             ResponseReceivedTime = r.ResponseReceivedTime,
                                                             ResponseText = r.ResponseText,
                                                             QuestionText = r.Question.QuestionText,
                                                             QuestionId = r.Question.Id,
                                                             QuestionType = r.Question.QuestionType,
                                                             Options = r.Question.Options
                                                         })
                                                         .ToList();

                var responseDTO = new ResponseDTO
                {
                    SurveyId = user.u.SurveyId,
                    UserName = user.r.User.UserName,
                    QuestionResponses = responsesForQuestionsInSurvey
                };

                responses.Add(responseDTO);
            }
            
            return responses;
        }

        [Route("UserResponses")]
        //Get responses for a user
        [ResponseType(typeof(SurveyResponse))]
        public IList<ResponseDTO> GetSurveyResponseOfUser(string userId)
        {
            List<ResponseDTO> responses = new List<ResponseDTO>();

            var user = db.Users.Where(u => u.Id.Equals(userId)).FirstOrDefault();

            var groupsAssociated = db.X_User_Groups.Where(u => u.UserId.Equals(userId));

            var surveysOfUser = from u in db.X_Survey_Groups
                                join r in db.X_User_Groups.Include("User")
                                on u.StudyGroupId equals r.StudyGroupId
                                where r.UserId.Equals(userId)
                                select new { r, u };

            foreach (var survey in surveysOfUser)
            {
                var responsesForQuestionsInSurvey = db.SurveyResponses
                                                         .Where(r => r.SurveyId == survey.u.SurveyId)
                                                         .Where(r => r.UserId.Equals(survey.r.UserId))
                                                         .Include(r => r.Question)
                                                         .Select(r => new QuestionResponseDTO
                                                         {
                                                             ResponseReceivedTime = r.ResponseReceivedTime,
                                                             ResponseText = r.ResponseText,
                                                             QuestionText = r.Question.QuestionText,
                                                             QuestionId = r.Question.Id,
                                                             QuestionType = r.Question.QuestionType,
                                                             Options = r.Question.Options
                                                         })
                                                         .ToList();

                var responseDTO = new ResponseDTO
                {
                    SurveyId = survey.u.SurveyId,
                    UserName = survey.r.User.UserName,
                    QuestionResponses = responsesForQuestionsInSurvey
                };

                responses.Add(responseDTO);
            }

           /* foreach (var group in groupsAssociated)
            {
                var surveysForStudyGroup = db.X_Survey_Groups.Where(r => r.StudyGroupId == group.StudyGroupId);

                foreach (var s in surveysForStudyGroup)
                {
                    var responsesForQuestionsInSurvey = db.SurveyResponses
                                                           .Where(r => r.SurveyId == s.SurveyId)
                                                           .Include(r => r.Question)
                                                           .Select(r => new QuestionResponseDTO
                                                           {
                                                               ResponseReceivedTime = r.ResponseReceivedTime,
                                                               ResponseText = r.ResponseText,
                                                               QuestionText = r.Question.QuestionText,
                                                               QuestionId = r.Question.Id,
                                                               QuestionType = r.Question.QuestionType,
                                                               Options = r.Question.Options
                                                           })
                                                           .ToList();

                    var responseDTO = new ResponseDTO
                    {
                        SurveyId = s.SurveyId,
                        //UserName = user.UserName,
                        QuestionResponses = responsesForQuestionsInSurvey
                    };

                    responses.Add(responseDTO);
                }
            }*/

            

            return responses;
            /*
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
            return result.ToList();*/
           
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
        public IHttpActionResult PutSurveyResponse(int id, SurveyResponse surveyResponse)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != surveyResponse.Id)
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
            //surveyResponse.Id = System.Guid.NewGuid();
            db.SurveyResponses.Add(surveyResponse);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (SurveyResponseExists(surveyResponse.Id))
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

            return CreatedAtRoute("DefaultApi", new { id = surveyResponse.Id }, surveyResponse);
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

        private bool SurveyResponseExists(int id)
        {
            return db.SurveyResponses.Count(e => e.Id == id) > 0;
        }
    }
}