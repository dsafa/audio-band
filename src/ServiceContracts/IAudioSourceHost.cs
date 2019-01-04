using System.ServiceModel;
using System.Threading.Tasks;

namespace ServiceContracts
{
    [ServiceContract(CallbackContract = typeof(IAudioSourceHostCallback))]
    public interface IAudioSourceHost
    {
        [OperationContract]
        string GetName();

        [OperationContract]
        Task ActivateAsync();

        [OperationContract]
        Task DeactivateAsync();

        [OperationContract]
        Task PlayTrackAsync();

        [OperationContract]
        Task PauseTrackAsync();

        [OperationContract]
        Task PreviousTrackAsync();

        [OperationContract]
        Task NextTrackAsync();
    }
}
