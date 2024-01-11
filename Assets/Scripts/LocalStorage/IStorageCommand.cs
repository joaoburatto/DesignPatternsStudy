using System;
using Cysharp.Threading.Tasks;

namespace LocalStorage
{
    public interface IStorageCommand
    {
        UniTask<StorageCommandResult> Perform();

        UniTask<StorageCommandResult> Rollback();
    }
}