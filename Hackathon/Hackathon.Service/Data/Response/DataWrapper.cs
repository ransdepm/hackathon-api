namespace Hackathon.Service.Data.Response
{
    public class DataWrapper<T>
    {
        public DataWrapper(T value)
        {
            Data = value;
        }

        public T Data { get; }
    }
}
