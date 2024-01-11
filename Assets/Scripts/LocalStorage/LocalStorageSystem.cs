using System.Collections.Generic;
using System.Threading.Tasks;
using Coimbra;
using Coimbra.Services;
using Cysharp.Threading.Tasks;

namespace LocalStorage
{
    [PreloadService]
    public class LocalStorageSystem : ILocalStorageService
    {
        private readonly List<IStorageCommand> _processedCommands = new List<IStorageCommand>();

        public int CommandCount => _processedCommands.Count;

        public void Dispose()
        {
            _processedCommands.Clear();
        }

        public async UniTask<StorageCommandResult> ProcessCommand(IStorageCommand command)
        {
            StorageCommandResult storageCommandResult = await command.Perform();

            if (storageCommandResult.Success)
            {
                _processedCommands.Add(command);
            }

            return storageCommandResult;
        }

        public async UniTask<StorageCommandResult> RollbackLastCommand()
        {
            IStorageCommand storageCommand = _processedCommands.Last();

            return await RollbackCommand(storageCommand);
        }

        public async UniTask<StorageCommandResult> RollbackCommand(IStorageCommand storageCommand)
        {
            StorageCommandResult commandResult = await storageCommand.Rollback();

            if (commandResult.Success)
            {
                _processedCommands.Remove(storageCommand);
            }

            return commandResult;
        }
    }
}
