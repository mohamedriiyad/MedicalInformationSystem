using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MedicalInformationSystem.Dtos;
using MedicalInformationSystem.Models.Notifications;
using MedicalInformationSystem.Persistant;
using Microsoft.Extensions.Hosting.Internal;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MedicalInformationSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private static readonly Uri FireBasePushNotificationsURL = new Uri("https://fcm.googleapis.com/fcm/send");
        private static readonly string ServerKey = "AAAAlB9nlnY:APA91bFiuguj921fcJvC05YnOSJpHQgN45EhueMeGYdZtkfYr-kPQBs0QEvLYjaBNzXBoKmoXKZ4YfkE3waIuO5fkuaeYvD3xHubL67GzOHqlIsGxkizQo2XDv0bM5HT6x6yaeYRn_aw";


        // POST api/<NotificationsController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] NotificationInput input)
        {
            bool sent = false;
            var user = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (input.DeviceTokens.Any())
            {
                //Object creation

                var messageInformation = new Message()
                {
                    notification = new Notification()
                    {
                        title = input.Title,
                        text = input.Body
                    },
                    data = input.Data,
                    registration_ids = input.DeviceTokens
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
                var request = new HttpRequestMessage(HttpMethod.Post, FireBasePushNotificationsURL);

                request.Headers.TryAddWithoutValidation("Authorization", "key=" + ServerKey);
                request.Content = new StringContent(jsonMessage, Encoding.UTF8, "application/json");

                HttpResponseMessage result;
                using (var client = new HttpClient())
                {
                    result = await client.SendAsync(request);
                    sent = sent && result.IsSuccessStatusCode;
                }
            }

            return Ok();
        }
    }
}



