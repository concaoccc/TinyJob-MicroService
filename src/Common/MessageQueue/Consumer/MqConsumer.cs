using Common.MessageQueue.Message;
using Common.MessageQueue.Producer;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.MessageQueue.Consumer
{
    public class MqConsumer : IMqConsumer
    {
        private IConsumer<Null, string> consumer;
        public ILogger logger;

        public MqConsumer(IConfiguration configuration, ILogger<MqProducer> logger)
        {
            this.logger = logger;
            this.consumer = new ConsumerBuilder<Null, string>(new ConsumerConfig
            {
                GroupId = configuration["Kafka:GroupId"],
                BootstrapServers = configuration["Kafka:BootstrapServers"],
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false
            }).Build();
            consumer.Subscribe(configuration["Kafka:Topic"]);
        }

        public JobMessage? Consume()
        {
            try
            {
                var message = consumer.Consume();
                var taskMessage = JsonConvert.DeserializeObject<JobMessage>(message.Message.Value);
                logger.LogInformation($"Get task {taskMessage} from MQ.");
                return taskMessage;
            }
            catch (Exception ex)
            {
                logger.LogError($"Failed to get message from MQ, Error: {ex.Message}.");
                return null;
            }
        }

        public void Commit()
        {
            consumer.Commit();
        }
    }
}
