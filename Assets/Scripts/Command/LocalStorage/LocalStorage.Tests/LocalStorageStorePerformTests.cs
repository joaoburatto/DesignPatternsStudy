using System.Collections;
using Coimbra.Services;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Command.LocalStorage.LocalStorage.Tests
{
    public class LocalStorageStorePerformTests
    {
        [UnityTest]
        public IEnumerator T001_PlayerPrefsStoreCommand_Perform_OneCommand_CheckValid_TestsSimplePasses()
        {            
            ILocalStorageService localStorageService = ServiceLocator.GetChecked<ILocalStorageService>();

            Assert.That(localStorageService, !Is.Null);

            int currentCount = localStorageService!.CommandCount;

            const string path = "DefaultUsername";
            const string oldData = "Joe";
            const string newData = "Mike";

            PlayerPrefs.SetString(path, oldData);
            PlayerPrefs.Save();

            yield return new WaitForSeconds(1);

            PlayerPrefsStoreCommand playerPrefsStoreCommand = new PlayerPrefsStoreCommand(path, newData);

            UniTask<StorageCommandResult> storageCommandResult = localStorageService!.ProcessCommand(playerPrefsStoreCommand);

            yield return new WaitUntil(() => storageCommandResult.GetAwaiter().IsCompleted);

            string savedData = PlayerPrefs.GetString(path);

            Assert.IsTrue(localStorageService.CommandCount == currentCount + 1);

            Assert.IsTrue(storageCommandResult.GetAwaiter().GetResult().Success);
            Assert.IsTrue(savedData.Equals(newData));
            Assert.IsTrue(playerPrefsStoreCommand.PreviousStoredData.Equals(oldData));
        }

        [UnityTest]
        public IEnumerator T002_LocalStorageStoreCommandPerform_TwoCommand_CheckCount_TestsSimplePasses()
        {            
            ILocalStorageService localStorageService = ServiceLocator.GetChecked<ILocalStorageService>();

            Assert.That(localStorageService, !Is.Null);

            int currentCount = localStorageService!.CommandCount;

            const string path = "DefaultUsername";
            
            const string newData = "Mike";
            const string newData2 = "Will";

            PlayerPrefsStoreCommand playerPrefsStoreCommand = new PlayerPrefsStoreCommand(path, newData);
            PlayerPrefsStoreCommand playerPrefsStoreCommand2 = new PlayerPrefsStoreCommand(path, newData2);

            UniTask<StorageCommandResult> storageCommandResult = localStorageService!.ProcessCommand(playerPrefsStoreCommand);
            UniTask<StorageCommandResult> storageCommandResult2 = localStorageService!.ProcessCommand(playerPrefsStoreCommand2);

            yield return new WaitUntil(() => storageCommandResult.GetAwaiter().IsCompleted);

            Assert.IsTrue(storageCommandResult.GetAwaiter().GetResult().Success);
            Assert.IsTrue(storageCommandResult2.GetAwaiter().GetResult().Success);

            Assert.IsTrue(localStorageService.CommandCount == currentCount + 2);
        }

        [UnityTest]
        public IEnumerator T003_PlayerPrefsStoreCommand_Rollback_OneCommand_CheckValid_TestsSimplePasses()
        {
            ILocalStorageService localStorageService = ServiceLocator.Get<ILocalStorageService>();

            Assert.That(localStorageService, !Is.Null);

            int currentCount = localStorageService!.CommandCount;

            const string path = "DefaultUsername";
            const string oldData = "Joe";
            const string newData = "Mike";

            PlayerPrefs.SetString(path, oldData);
            PlayerPrefs.Save();

            yield return new WaitForSeconds(1);

            PlayerPrefsStoreCommand playerPrefsStoreCommand = new PlayerPrefsStoreCommand(path, newData);

            UniTask<StorageCommandResult> storageCommandResult = localStorageService!.ProcessCommand(playerPrefsStoreCommand);

            yield return new WaitUntil(() => storageCommandResult.GetAwaiter().IsCompleted);

            string savedData = PlayerPrefs.GetString(path);

            Assert.IsTrue(localStorageService.CommandCount == currentCount + 1);

            Assert.IsTrue(storageCommandResult.GetAwaiter().GetResult().Success);
            Assert.IsTrue(savedData.Equals(newData));
            Assert.IsTrue(playerPrefsStoreCommand.PreviousStoredData.Equals(oldData));

            UniTask<StorageCommandResult> rollbackCommand = localStorageService!.RollbackCommand(playerPrefsStoreCommand);

            yield return new WaitUntil(() => rollbackCommand.GetAwaiter().IsCompleted);

            StorageCommandResult rollbackResult = rollbackCommand.GetAwaiter().GetResult();

            Assert.IsTrue(rollbackResult.Success);

            savedData = PlayerPrefs.GetString(path);

            Assert.IsTrue(savedData.Equals(oldData));
            Assert.IsTrue(localStorageService.CommandCount == currentCount);
        }
    }
}
