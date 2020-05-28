#if XBOX_LIVE
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xbox.Services;
using Windows.Gaming.XboxLive.Storage;
using Windows.Storage.Streams;
using System.Diagnostics;

namespace Snake
{
	public static class XboxLiveConnectedStorage
	{
		public static async void Save(string data, string filename)
		{
			const string c_saveContainerDisplayName = "GameSave";
			const string c_saveBlobName = "data";
			string c_saveContainerName = filename;

			var gameSaveProvider = await GetGameSaveProvider();
			if (gameSaveProvider == null)
			{
				return;
			}

			//Now you have a GameSaveProvider (formerly ConnectedStorageSpace)
			//Next you need to call CreateContainer to get a GameSaveContainer (formerly ConnectedStorageContainer)
			GameSaveContainer gameSaveContainer = gameSaveProvider.CreateContainer(c_saveContainerName); // this will create a new named game save container with the name = to the input name
																										 //Parameter
																										 //string name

			// To store a value in the container, it needs to be written into a buffer, then stored with
			// a blob name in a Dictionary.
			DataWriter writer = new DataWriter();
			uint dataSize = writer.MeasureString(data);
			writer.WriteUInt32(dataSize);
			writer.WriteString(data);
			IBuffer dataBuffer = writer.DetachBuffer();

			var blobsToWrite = new Dictionary<string, IBuffer>();
			blobsToWrite.Add(c_saveBlobName, dataBuffer);

			GameSaveOperationResult gameSaveOperationResult = await gameSaveContainer.SubmitUpdatesAsync(blobsToWrite, null, c_saveContainerDisplayName);
			//IReadOnlyDictionary<String, IBuffer> blobsToWrite
			//IEnumerable<string> blobsToDelete
			//string displayName
			Debug.WriteLine("XboxLiveConnectedStorage :: Saved Data - " + data);
		}

		public static async Task<string> Load(string filename)
		{
			string loadedData = null;

			const string c_saveBlobName = "data";
			string c_saveContainerName = filename;

			//Now you have a GameSaveProvider
			//Next you need to call CreateContainer to get a GameSaveContainer
			var gameSaveProvider = await GetGameSaveProvider();
			if (gameSaveProvider == null)
			{
				return null;
			}

			GameSaveContainer gameSaveContainer = gameSaveProvider.CreateContainer(c_saveContainerName);
			//Parameter
			//string name (name of the GameSaveContainer Created)

			//form an array of strings containing the blob names you would like to read.
			string[] blobsToRead = new string[] { c_saveBlobName };

			// GetAsync allocates a new Dictionary to hold the retrieved data. You can also use ReadAsync
			// to provide your own preallocated Dictionary.
			GameSaveBlobGetResult result = await gameSaveContainer.GetAsync(blobsToRead);

			//Check status to make sure data was read from the container
			if (result.Status == GameSaveErrorStatus.Ok)
			{
				//prepare a buffer to receive blob
				IBuffer loadedBuffer;

				//retrieve the named blob from the GetAsync result, place it in loaded buffer.
				result.Value.TryGetValue(c_saveBlobName, out loadedBuffer);

				if (loadedBuffer == null)
				{
					throw new Exception(String.Format("Didn't find expected blob \"{0}\" in the loaded data.", c_saveBlobName));
				}
				DataReader reader = DataReader.FromBuffer(loadedBuffer);

				uint bytesToRead = reader.ReadUInt32();
				loadedData = reader.ReadString(bytesToRead);

				Debug.WriteLine("XboxLiveConnectedStorage :: Loaded Data - " + loadedData);
				return loadedData;
			}
			else
			{
				return null;
			}
		}

		private static async Task<GameSaveProvider> GetGameSaveProvider()
		{
			if (XboxLiveObject.CurrentContext == null || XboxLiveObject.CurrentUser == null)
			{
				return null;
			}

			var users = await Windows.System.User.FindAllAsync();

			GameSaveProviderGetResult gameSaveTask = await GameSaveProvider.GetForUserAsync(users[0], XboxLiveObject.CurrentContext.AppConfig.ServiceConfigurationId);
			if (gameSaveTask.Status == GameSaveErrorStatus.Ok)
			{
				return gameSaveTask.Value;
			}
			else
			{
				return null;
			}
		}
	}
}
#endif
