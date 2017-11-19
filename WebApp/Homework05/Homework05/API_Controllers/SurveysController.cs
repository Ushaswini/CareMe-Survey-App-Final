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

namespace Homework05.API_Controllers
{
    public class SurveysController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Surveys
        public IList<SurveyDTO> GetSurveys()
        {
            return db.Surveys.Include(s => s.StudyGroup).Select(s => new SurveyDTO
            {
                SurveyId = s.SurveyId,
                SurveyCreatedTime = s.SurveyCreatedTime,
                QuestionText = s.QuestionText,
                StudyGroupId = s.StudyGroupId,
                StudyGroupName = s.StudyGroup.StudyName

            }).ToList();
        }

        public IList<SurveyDTO> GetSurveysForStudyGroup(string studyGroupId)
        {
            var surveys = db.Surveys.Where(s => s.StudyGroupId == studyGroupId).Include(s => s.StudyGroup).Select(s => new SurveyDTO
            {
                SurveyId = s.SurveyId,
                SurveyCreatedTime = s.SurveyCreatedTime,
                QuestionText = s.QuestionText,
                StudyGroupId = s.StudyGroupId,
                StudyGroupName = s.StudyGroup.StudyName

            });

            return surveys.ToList();
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
                switch (survey.FrequencyOfNotifications)
                {
                    case Frequency.Daily:
                        {
                            String[] times = survey.Time1.Split(':');
                            String cornExpression = times[1] + " " + times[0] + " * * *";
                            RecurringJob.AddOrUpdate(survey.SurveyId, () => SendNotification(survey), cornExpression, TimeZoneInfo.Local);
                            break;
                        }
                    case Frequency.Hourly:
                        //PushNotificationsAsync();
                        SendNotification(survey);
                        RecurringJob.AddOrUpdate(survey.SurveyId, () => SendNotification(survey), Cron.Hourly, TimeZoneInfo.Local);
                        break;
                    case Frequency.TwiceDaily:
                        {
                            String[] times = survey.Time1.Split(':');
                            String cornExpression = times[1] + " " + times[0] + " * * *";
                            String[] times2 = survey.Time2.Split(':');
                            String cornExpression2 = times2[1] + " " + times2[0] + " * * *";
                            RecurringJob.AddOrUpdate(survey.SurveyId + "First", () => SendNotification(survey), cornExpression, TimeZoneInfo.Local);
                            RecurringJob.AddOrUpdate(survey.SurveyId + "Second", () => SendNotification(survey), cornExpression2, TimeZoneInfo.Local);
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
            var applicationID = "AIzaSyC0Ian0Yr7JK9tZEi7-dZ3GcO-2dzomG1M";
            // applicationID means google Api key 
            var SENDER_ID = "283278634859";
            // SENDER_ID is nothing but your ProjectID (from API Console- google code)                                                                             

            WebRequest tRequest;

            tRequest = WebRequest.Create("https://android.googleapis.com/gcm/send");

            tRequest.Method = "post";

            tRequest.ContentType = " application/json";

            tRequest.Headers.Add(string.Format("Authorization: key={0}", applicationID));

            tRequest.Headers.Add(string.Format("Sender: id={0}", SENDER_ID));

            string postData = "{\"collapse_key\":\"score_update\",\"time_to_live\":108,\"delay_while_idle\":true,\"data\": { \"message\" : " + "\"" + survey.QuestionText + "\",\"time\": " + "\"" + System.DateTime.Now.ToString() + "\"},\"registration_ids\":[\"" + "AAAAQfS472s:APA91bFkVobFc9H0Jvxom5_z6aYyGWCoIgZkv_U2-7qT4yUcKvgKvl_ZdCyEoyYDDWihiCfxe5u7mk_KXItsVGeB-mW4-_HXMB8B6_pickvBLkoZbFP5VlmT6X2-Lh8S3qiEegv7WkIp" + "\"]}";

            Console.WriteLine(postData);
            Byte[] byteArray = Encoding.UTF8.GetBytes(postData);
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