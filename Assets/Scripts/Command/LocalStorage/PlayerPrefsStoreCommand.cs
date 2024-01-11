using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Command.LocalStorage
{
    public class PlayerPrefsStoreCommand : IStorageCommand
    {
        public readonly string Path;

        public readonly string Data;

        public string PreviousStoredData;

        public PlayerPrefsStoreCommand() { }

        public PlayerPrefsStoreCommand(string path, string data)
        {
            Path = path;
            Data = data;
        }

        public async UniTask<StorageCommandResult> Perform()
        {
            StorageCommandResult storageCommandResult = new StorageCommandResult()
            {
                Success = true,
                Error = null,
            };

            PreviousStoredData = PlayerPrefs.GetString(Path);

            PlayerPrefs.SetString(Path, Data);
            PlayerPrefs.Save();

            return storageCommandResult;
        }

        public async UniTask<StorageCommandResult> Rollback()
        {
            StorageCommandResult storageCommandResult = new StorageCommandResult()
            {
                Success = true,
                Error = null,
            };

            PlayerPrefs.SetString(Path, PreviousStoredData);

            return storageCommandResult;
        }
    }
}