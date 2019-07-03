using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GetVisitantsBehaviorWebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nancy.Json;
using System.Web;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;

namespace GetVisitantsBehaviorWebApp.Controllers
{
    [EnableCors("AllowMyOrigin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AnalyticsController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post(UserBehavior userBehavior)
        {
            #region Opens the channel and connection to RabbitMQ server
            ConnectionRabbitMQ connectionRabbitMQ = new ConnectionRabbitMQ();
            connectionRabbitMQ.GetConnection();
            connectionRabbitMQ.routingKey = "UserBehaviorQueue";
            connectionRabbitMQ.OpenChannel();
            #endregion

            #region Sets the visitor IP
            userBehavior.ip = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            #endregion

            #region Serializes the entity UserBehavior to JSon format
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            var jsonUserBehavior = javaScriptSerializer.Serialize(userBehavior);
            #endregion

            #region Publishes the message on RabbitMQ
            connectionRabbitMQ.PublishMessage(jsonUserBehavior);
            #endregion

            #region Closes the connection to RabbitMQ
            connectionRabbitMQ.CloseChannel();
            #endregion

            return Ok("User Behavior has been sent.");
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Get user behavior Ok.");
        }
    }
}