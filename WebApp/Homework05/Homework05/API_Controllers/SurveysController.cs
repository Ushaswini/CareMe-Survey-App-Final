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
using System.Text;
using System.IO;
using Hangfire;
using Newtonsoft.Json;

namespace Homework05.API_Controllers
{
    [Authorize]
    [RoutePrefix("api/Surveys")]
    public class SurveysController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Surveys
        public IList<SurveyDTO> GetSurveys()
        {
            return db.Surveys.Include(s => s.StudyGroup).Include(s => s.Question).Select(s => new SurveyDTO
            {
                SurveyId = s.SurveyId,
                SurveyCreatedTime = s.SurveyCreatedTime,
                QuestionText = s.Question.QuestionText,
                StudyGroupId = s.StudyGroupId,
                StudyGroupName = s.StudyGroup.StudyName,
                QuestionId = s.QuestionId,
                QuestionType = s.Question.QuestionType,
                Options = s.Question.Options,
                QuestionFrequency = ((Frequency)s.FrequencyOfNotifications).ToString(),
                Time1 = s.Time1,
                Time2 = s.Time2

            }).ToList();
        }

        public IList<SurveyDTO> GetSurveysForStudyGroup(string studyGroupId)
        {
            var surveys = db.Surveys.Where(s => s.StudyGroupId == studyGroupId).Include(s => s.Question).Include(s => s.StudyGroup).Select(s => new SurveyDTO
            {
                SurveyId = s.SurveyId,
                SurveyCreatedTime = s.SurveyCreatedTime,
                QuestionText = s.Question.QuestionText,
                StudyGroupId = s.StudyGroupId,
                StudyGroupName = s.StudyGroup.StudyName,
                QuestionId = s.QuestionId,
                QuestionType = s.Question.QuestionType,
                Options = s.Question.Options,
                QuestionFrequency = ((Frequency)s.FrequencyOfNotifications).ToString(),
                Time1 = s.Time1,
                Time2 = s.Time2

            });

            return surveys.ToList();
        }

        [Route("GetSurvey")]
        public SurveysForUser GetSurveysForUser(string userId)
        {
            var surveysTaken = db.SurveyResponses.Where(s => s.UserId == userId).Select(s => s.SurveyId).ToList();

            var surveysResponded = db.SurveyResponses.Include(r => r.StudyGroup)
                                            .Include(r => r.Survey)
                                            .Include(r => r.User)
                                            .Include(r => r.Survey.Question)
                                            .Where(r => r.UserId == userId)
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
                                            }).ToList();

            var surveys = (from r in db.Surveys.Include("Question")
                          where !surveysTaken.Contains(r.SurveyId)                          
                          select new SurveyDTO
                          {
                            SurveyId = r.SurveyId,
                            SurveyCreatedTime = r.SurveyCreatedTime,
                            QuestionText = r.Question.QuestionText,
                            StudyGroupId = r.StudyGroupId,
                            StudyGroupName = r.StudyGroup.StudyName,
                            Options =r.Question.Options,
                            QuestionId = r.QuestionId,
                            QuestionType = r.Question.QuestionType,
                            QuestionFrequency = ((Frequency)r.FrequencyOfNotifications).ToString(),
                            Time1 = r.Time1,
                            Time2 = r.Time2
                          }).ToList();
 
            return new SurveysForUser { Surveys = surveys, SurveysResponded = surveysResponded };
        }


        // GET: api/Surveys/5
        [ResponseType(typeof(Survey))]
        public IHttpActionResult GetSurvey(string id)
        {
            Survey survey = db.Surveys.Find(id);
            if (survey == null)
            {
                return NotFound();
            }

            return Ok(survey);
        }

        // PUT: api/Surveys/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutSurvey(string id, Survey survey)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != survey.SurveyId)
            {
                return BadRequest();
            }

            db.Entry(survey).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SurveyExists(id))
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

        
        // POST: api/Surveys
        [Route("Post")]
        [ResponseType(typeof(Survey))]
        public IHttpActionResult PostSurvey(Survey survey)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Surveys.Add(survey);

            try
            {
                db.SaveChanges();
                var question = db.Questions.Where(q => q.QuestionId.Equals(survey.QuestionId));
                var surveyToSend = db.Surveys.Include(s => s.Question).Where(s => s.QuestionId.Equals(survey.QuestionId)).FirstOrDefault();
                switch (survey.FrequencyOfNotifications)
                {
                    case Frequency.Daily:
                        {
                            String[] times = survey.Time1.Split(':');
                            String cornExpression = times[1] + " " + times[0] + " * * *";
                            RecurringJob.AddOrUpdate(survey.SurveyId, () => SendNotification(surveyToSend), cornExpression, TimeZoneInfo.Local);
                            break;
                        }
                    case Frequency.Hourly:
                        //PushNotificationsAsync();
                        SendNotification(survey);
                        RecurringJob.AddOrUpdate(survey.SurveyId, () => SendNotification(surveyToSend), Cron.Hourly, TimeZoneInfo.Local);
                        break;
                    case Frequency.TwiceDaily:
                        {
                            String[] times = survey.Time1.Split(':');
                            String cornExpression = times[1] + " " + times[0] + " * * *";
                            String[] times2 = survey.Time2.Split(':');
                            String cornExpression2 = times2[1] + " " + times2[0] + " * * *";
                            RecurringJob.AddOrUpdate(survey.SurveyId + "First", () => SendNotification(surveyToSend), cornExpression, TimeZoneInfo.Local);
                            RecurringJob.AddOrUpdate(survey.SurveyId + "Second", () => SendNotification(surveyToSend), cornExpression2, TimeZoneInfo.Local);
                            break;
                        }
                }
                //RecurringJob.AddOrUpdate(survey.SurveyId,() => PushNotificationsAsync(), Cron.Minutely);
                //PushNotificationsAsync();
                //push notification to users who opted in
            }
            catch (DbUpdateException)
            {
                if (SurveyExists(survey.SurveyId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Ok(survey);
        }

        public void SendNotification(Survey survey)
        {
            try
            {
                List<string> deviceIds = new List<string>();
                var usersInGroup = db.Users.Where(u => u.StudyGroupId.Equals(survey.StudyGroupId)).ToList();
                foreach (var user in usersInGroup)
                {
                    if (user.DeviceId != null)
                        deviceIds.Add(user.DeviceId);
                }
                //deviceIds.RemoveAt(0);
                //deviceIds.Add("ebR53cvFzu8:APA91bHk0D_Bwth1jeD-pJ4Q3aztg8C8USt8qFf5_fOV4aflIMVqjoc0HsAYQARcUfik3NfkuQG21jh265tJzBi7efPXw77__JEzaDSbPG8rAiZBTguobpNEjCPnCUPzM9zawIpgcO2o");
                SurveyPushNotification notification = new SurveyPushNotification
                {
                    RegisteredDeviceIds = deviceIds,
                    Data = new PushNotificationData
                    {
                        Message = survey.Question.QuestionText,
                        Time = DateTime.Now.ToString()
                    }

                };
                if (deviceIds.Count > 0)
                {
                    var applicationID = "AIzaSyC0Ian0Yr7JK9tZEi7-dZ3GcO-2dzomG1M";
                    // applicationID means google Api key 
                    var SENDER_ID = "283278634859";
                    // SENDER_ID is nothing but your ProjectID (from API Console- google code)  

                    string serializedNotification = JsonConvert.SerializeObject(notification);

                    WebRequest tRequest;

                    tRequest = WebRequest.Create("https://android.googleapis.com/gcm/send");

                    tRequest.Method = "post";

                    tRequest.ContentType = " application/json";

                    tRequest.Headers.Add(string.Format("Authorization: key={0}", applicationID));

                    tRequest.Headers.Add(string.Format("Sender: id={0}", SENDER_ID));

                    Byte[] byteArray = Encoding.UTF8.GetBytes(serializedNotification);
                    tRequest.ContentLength = byteArray.Length;
                    Stream dataStream = tRequest.GetRequestStream();
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Close();
                    WebResponse tResponse = tRequest.GetResponse();
                    dataStream = tResponse.GetResponseStream();
                    StreamReader tReader = new StreamReader(dataStream);
                    String sResponseFromServer = tReader.ReadToEnd();   //Get response from GCM server.
                    Console.WriteLine(sResponseFromServer);
                    tReader.Close();
                    dataStream.Close();
                    tResponse.Close();
                }
            }
            catch (Exception e)
            {

            }
                     
            

           
        }

        // DELETE: api/Surveys/5
        [ResponseType(typeof(Survey))]
        public IHttpActionResult DeleteSurvey(string id)
        {
            Survey survey = db.Surveys.Find(id);
            if (survey == null)
            {
                return NotFound();
            }

            db.Surveys.Remove(survey);
            db.SaveChanges();

            return Ok(survey);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SurveyExists(string id)
        {
            return db.Surveys.Count(e => e.SurveyId == id) > 0;
        }
    }
}