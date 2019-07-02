using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GetVisitantsBehaviorWebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nancy.Json;

namespace GetVisitantsBehaviorWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalyticsController : ControllerBase
    {
        public void SetUserBehavior(UserBehavior userBehavior)
        {
            ConnectionRabbitMQ connectionRabbitMQ = new ConnectionRabbitMQ();
            connectionRabbitMQ.GetConnection();
            connectionRabbitMQ.OpenChannel();

            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            var jsonUserBehavior = javaScriptSerializer.Serialize(userBehavior);

            connectionRabbitMQ.PublishMessage(jsonUserBehavior);
        }
    }
}