using Common.MessageQueue.Message;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Common.MessageQueue.Producer
{
    public class MqProducer : IMqProducer
    {
        private IProducer<Null, string> producer;
        private string topic;
        public ILogger logger;

        public MqProducer(IConfiguration configuration, ILogger<MqProducer> logger)
        {
            this.logger = logger;
            this.producer = new ProducerBuilder<Null, string>(new ProducerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"],
            }).Build();

            this.topic = configuration["Kafka:Topic"]?? throw new ArgumentException("Should set kafka topic in the setting.");

        }

        public async Task<bool> ProduceAsync(List<JobMessage> taskMessages)
        {
            try
            {
                logger.LogInformation($"Will deliver {taskMessages.Count} messages");
                foreach (var taskMessage in taskMessages)
                {
                    var result = await producer.ProduceAsync(topic, new Message<Null, string> { Value = JsonConvert.SerializeObject(taskMessage) });
                    logger.LogInformation($"Delivered '{result.Value}'");
                }
            }
            catch (ProduceException<Null, string> e)
            {
                logger.LogError($"Delivery failed: {e.Error.Reason}");
                return false;
            }
                return true;
        }
    }
}
