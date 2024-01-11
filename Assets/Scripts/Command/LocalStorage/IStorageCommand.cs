using Cysharp.Threading.Tasks;

namespace Command.LocalStorage
{
    public interface IStorageCommand
    {
        UniTask<StorageCommandResult> Perform();

        UniTask<StorageCommandResult> Rollback();
    }
}