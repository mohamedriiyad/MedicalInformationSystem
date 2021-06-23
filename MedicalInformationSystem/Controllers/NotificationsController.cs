using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MedicalInformationSystem.Dtos;
using MedicalInformationSystem.Models;
using MedicalInformationSystem.Models.Notifications;
using MedicalInformationSystem.Persistant;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MedicalInformationSystem.Controllers
{
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly MedicalSystemDbContext _context;
        private static readonly Uri FireBasePushNotificationsUrl = new Uri("https://fcm.googleapis.com/fcm/send");
        private static readonly string ServerKey = "AAAAfchY5EQ:APA91bEI0tMBKnn7_poCRoxIDR69CSp_uMZfxYRzB3fj6-3DtFdL4dlq5mjOKWSNTa-CDuW0rA1A8PheOMWaJxLOVO_5SyakehmleMbttnUqoMM3sgW_DXXgws-UF-sH_0H-j_m1cvBx";

        public NotificationsController(MedicalSystemDbContext context)
        {
            _context = context;
        }


        // POST api/<NotificationsController>
        [HttpPost]
        [Route("api/Notifications/Post/{id}")]
        public async Task<IActionResult> Post(int id, [FromBody] NotificationInput input)
        {
            var sent = false;
            var hospital = _context.Hospitals.SingleOrDefault(h => h.Id == id);

            if (hospital == null)
                return BadRequest("This id is invalid");

            var credentials = _context.Tokens
                .Include(t => t.ApplicationUser)
                .ThenInclude(u => u.MedicalHistory)
                .Where(t => t.ApplicationUser.City == input.City &&
                            t.ApplicationUser.MedicalHistory.BloodType == input.BloodType)
                .Select(t => new
                {
                    t.Token,
                    UserId = t.ApplicationUserId
                }).ToArray();

            var deviceTokens = credentials.Select(c => c.Token).ToArray();
            var usersId = credentials.Select(c => c.UserId).ToArray();

            if (deviceTokens.Any())
            {
                //Object creation
                var messageInformation = new Message()
                {
                    notification = new Notification()
                    {
                        title = input.Title,
                        text = input.Body
                    },
                    registration_ids = deviceTokens
                };

                //Object to JSON STRUCTURE => using Newtonsoft.Json;
                string jsonMessage = JsonConvert.SerializeObject(messageInformation);

                /*
                 ------ JSON STRUCTURE ------
                 {
                    notification: {
                                    title: "",
                                    text: ""
                                    },
                    data: {
                            action: "Play",
                            playerId: 5
                            },
                    registration_ids = ["id1", "id2"]
                 }
                 ------ JSON STRUCTURE ------
                 */

                //Create request to Firebase API
                var request = new HttpRequestMessage(HttpMethod.Post, FireBasePushNotificationsUrl);

                request.Headers.TryAddWithoutValidation("Authorization", "key=" + ServerKey);
                request.Content = new StringContent(jsonMessage, Encoding.UTF8, "application/json");

                HttpResponseMessage result;
                using (var client = new HttpClient())
                {
                    result = await client.SendAsync(request);
                    sent = result.IsSuccessStatusCode;
                }
            }

            var bloodDonations = new List<NotificationModel>();
            if (usersId.Any())
            {
                bloodDonations.AddRange(usersId.Select(userId => new NotificationModel
                {
                    ApplicationUserId = userId,
                    Date = DateTime.Now.ToUniversalTime(),
                    HospitalModelId = hospital.Id,
                    Note = input.Body,
                    PatientName = input.PatientName,
                    NumberOfBags = input.NumberOfBags,
                    BloodType = input.BloodType
                }));
                _context.BloodDonations.AddRange(bloodDonations);
                _context.SaveChanges();
            }

            if (sent)
                return Ok();

            return BadRequest("there is something wrong!! or no user available!!");
        }

        [HttpGet]
        [Route("api/Notifications/Get/{id}")]
        public async Task<IEnumerable<NotificationList>> Get(string id)
        {
            var notifications = await _context.BloodDonations
                .Include(b => b.HospitalModel)
                .Where(b => b.ApplicationUserId == id)
                .Select(b => new NotificationList
                {
                    Date = b.Date,
                    NumberOfBags = b.NumberOfBags,
                    PatientName = b.PatientName,
                    Note = b.Note,
                    BloodType = b.BloodType,
                    Hospital = new HospitalDto
                    {
                        Name = b.HospitalModel.Name,
                        Email = b.HospitalModel.Email,
                        Location = b.HospitalModel.Location
                    }
                }).ToListAsync();

            return notifications;
        }
    }
}



