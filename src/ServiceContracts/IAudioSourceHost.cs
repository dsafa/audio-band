using System.ServiceModel;
using System.Threading.Tasks;

namespace ServiceContracts
{
    [ServiceContract(SessionMode = SessionMode.Required)]
    interface IAudioSourceHost
    {
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
