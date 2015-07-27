namespace AS.Client.Messages
{
    [System.Serializable]
    public class BatchedMessage
    {
        public object[] Messages { get; private set; }

        public BatchedMessage(object[] messages)
        {
            Messages = messages;
        }
    }
}
