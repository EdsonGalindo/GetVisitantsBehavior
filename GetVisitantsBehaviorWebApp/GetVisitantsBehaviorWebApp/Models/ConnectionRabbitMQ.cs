﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RabbitMQ.Client;
using System.Text;

namespace GetVisitantsBehaviorWebApp.Models
{
	/// <summary>
    /// This is a class for managing RabbitMQ connections and to executing commands
    /// </summary>
    public class ConnectionRabbitMQ
    {
        private IConnection connection;
        private IModel channel;
        public string username = "guest";
        public string password = "guest";
        public string virtualHost = "/";
        public string hostName = "locahost";
        public int port = 5672;
        public string exchangeName = "";

        internal void PublishMessage(object json, UserBehavior userBehavior)
        {
            throw new NotImplementedException();
        }

        public string routingKey = "";

		/// <summary>
        /// Creates a new connection to a RabbitMQ server
        /// </summary>
        public void GetConnection()
        {
            ConnectionFactory factory = new ConnectionFactory();
            factory.UserName = username;
            factory.Password = password;
            factory.VirtualHost = virtualHost;
            factory.HostName = hostName;
            factory.Port = port;

            connection = factory.CreateConnection();
        }

		/// <summary>
        /// Opens a channel using a connection of a RabbitMQ server
        /// </summary>
		public void OpenChannel()
        {
            channel = connection.CreateModel();
        }

		/// <summary>
        /// Closes a connection of a RabbitMQ server
        /// </summary>
		public void CloseChannel()
        {
            channel.Close();
            channel.Close();
        }

		/// <summary>
        /// Sends a message to a RabbitMQ server
        /// </summary>
        /// <param name="message">A message to be sent to the RabbitMQ server</param>
        /// <returns></returns>
		public bool PublishMessage(string message)
        {
            try
            {
                byte[] messageBytes = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchangeName, routingKey, null, messageBytes);
            }
            catch { return false; }

            return true;
        }

    }
}
