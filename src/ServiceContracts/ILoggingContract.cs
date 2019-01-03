using System.ServiceModel;

namespace ServiceContracts
{
    [ServiceContract]
    public interface ILoggingContract
    {
        [OperationContract]
        void Debug(string name, string message);

        [OperationContract]
        void Info(string name, string message);

        [OperationContract]
        void Warn(string name, string message);

        [OperationContract]
        void Error(string name, string message);
    }
}
