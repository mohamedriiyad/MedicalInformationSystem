using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MedicalInformationSystem.Dtos;
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
        private static readonly string ServerKey = "AAAAlB9nlnY:APA91bFiuguj921fcJvC05YnOSJpHQgN45EhueMeGYdZtkfYr-kPQBs0QEvLYjaBNzXBoKmoXKZ4YfkE3waIuO5fkuaeYvD3xHubL67GzOHqlIsGxkizQo2XDv0bM5HT6x6yaeYRn_aw";

        public NotificationsController(MedicalSystemDbContext context)
        {
            _context = context;
        }


        // POST api/<NotificationsController>
        [HttpPost]
        [Route("api/Notifications/Post")]
        public async Task<IActionResult> Post([FromBody] NotificationInput input)
        {
            var sent = false;
            var deviceTokens = _context.Tokens
                .Include(t => t.ApplicationUser)
                .ThenInclude(u => u.MedicalHistory)
                .Where(t => t.ApplicationUser.City == input.City &&
                            t.ApplicationUser.MedicalHistory.BloodType == input.BloodType)
                .Select(t=>t.Token)
                .ToArray();

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

            if (sent)
                return Ok();

            return BadRequest("there is something wrong!!");
        }
    }
}



