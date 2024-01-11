using Coimbra.Services;
using Cysharp.Threading.Tasks;

namespace Command.LocalStorage
{
    [RequiredService]
    public interface ILocalStorageService : IService
    {
        int CommandCount { get; }

        public UniTask<StorageCommandResult> ProcessCommand(IStorageCommand command);

        public UniTask<StorageCommandResult> RollbackLastCommand();

        public UniTask<StorageCommandResult> RollbackCommand(IStorageCommand storageCommand);
    }
}
