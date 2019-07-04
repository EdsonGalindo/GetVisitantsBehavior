using GetVisitantsBehaviorRobot.Models;
using Microsoft.Extensions.Configuration;
using Nancy.Json;
using System;
using System.IO;
using System.Threading;

namespace GetVisitantsBehaviorRobot
{
    public class Program
    {
        public static RobotSettings robotSettings = new RobotSettings();
        public static RabbitMQSettings rabbitMQSettings = new RabbitMQSettings();
        public static ConnectionStrings connectionStrings = new ConnectionStrings();

        static void Main(string[] args)
        {
            #region Defines the application settings
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddEnvironmentVariables()
                .AddUserSecrets<Program>();
            #endregion

            #region Gets the system settings
            IConfigurationRoot configurations = builder.Build();

            configurations.GetSection("RobotSettings").Bind(robotSettings);            
            configurations.GetSection("RabbitMQSettings").Bind(rabbitMQSettings);            
            configurations.GetSection("ConnectionStrings").Bind(connectionStrings);
            #endregion

            Console.WriteLine("------------ Get Visitants Behavior Robot ------------");

            while(robotSettings.Enabled)
            {
                try
                {
                    #region Opens the channel and connection to RabbitMQ server
                    ConnectionRabbitMQ connectionRabbitMQ = new ConnectionRabbitMQ();
                    connectionRabbitMQ.GetConnection();
                    connectionRabbitMQ.routingKey = "UserBehaviorQueue";
                    connectionRabbitMQ.OpenChannel();
                    #endregion

                    #region Reads data from RabbitMQ queue
                    var messageGetResult = connectionRabbitMQ.GetIndividualMessage();

                    if (messageGetResult == null)
                    {
                        Console.WriteLine("=> (" + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + ") No data was returned from RabbitMQ queue, the robot will try again in " + robotSettings.TimeoutGetData + " milliseconds.");
                        Thread.Sleep(robotSettings.TimeoutGetData);
                        continue;
                    }

                    #region Deserializes UserBehavior entity from JSon format
                    JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                    var jsonUserBehavior = javaScriptSerializer.Deserialize<UserBehavior>(
                        System.Text.Encoding.UTF8.GetString(messageGetResult.Body));
                    #endregion

                    #region Writes data to SQL Server Database
                    using (var context = new VisitantsBehaviorContext())
                    {
                        context.UserBehavior.Add(jsonUserBehavior);
                        context.SaveChanges();
                    }
                    #endregion

                    #region Writes data to Couchbase Database
                    // todo
                    #endregion

                    #endregion

                    #region Closes the connection to RabbitMQ
                    connectionRabbitMQ.channel.BasicAck(messageGetResult.DeliveryTag, false);
                    connectionRabbitMQ.CloseChannel();
                    #endregion
                }
                catch (Exception ex) {
                    Console.WriteLine("=> An error has ocurred. Details: " + ex.Message);
                }

                
            }
        }
    }
}
