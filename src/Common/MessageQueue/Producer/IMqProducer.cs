using Common.MessageQueue.Message;

namespace Common.MessageQueue.Producer
{
    public interface IMqProducer
    {
        Task<bool> ProduceAsync(List<JobMessage> taskMessage);
    }
}
