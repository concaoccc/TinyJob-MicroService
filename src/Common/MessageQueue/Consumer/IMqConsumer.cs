using Common.MessageQueue.Message;

namespace Common.MessageQueue.Consumer
{
    public interface IMqConsumer
    {
        JobMessage? Consume();
        void Commit();
    }
}
