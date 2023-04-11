using Azure.Identity;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using PianoLessons.Shared.Data;
using System.Collections;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.IO;

namespace PianoLessonsApi.Repositories;

public class RecordingRepo
{
	private readonly ILogger logger;
	private BlobServiceClient client;
	public RecordingRepo(IConfiguration config, ILogger<RecordingRepo> logger)
	{
		this.logger = logger;
		Uri accountUri = new Uri("https://pianorecordings.blob.core.windows.net/");
		string accountName = config["StorageAccountName"];
		string accountKey = config["StorageAccountAccessKey"];
		StorageSharedKeyCredential credential = new StorageSharedKeyCredential(accountName, accountKey);
		client = new BlobServiceClient(accountUri, credential);
	}

	public async Task<string> SendRecordingToAzure(FileData data, string studentId)
	{
		var containerName = studentId.Split('|')[1];
		logger.LogInformation($"Getting container client {containerName}");
		BlobContainerClient containerClient = client.GetBlobContainerClient(containerName);
		logger.LogInformation($"Done. Creating if not exist");
		await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);
		MemoryStream stream = new MemoryStream(data.Data);
		logger.LogInformation($"Uploading blob");
		await containerClient.UploadBlobAsync(data.FileName, stream);
		logger.LogInformation("Successfully uploaded recording");
		return $"https://pianorecordings.blob.core.windows.net/{containerName}/{data.FileName}";
	}

	public async Task DeleteAzureRecording(string studentId, string fileName)
	{
		var containerName = studentId.Split('|')[1];
		logger.LogInformation($"Deleting recording {fileName} for container {containerName}");
		BlobContainerClient containerClient = client.GetBlobContainerClient(containerName);
		await containerClient.DeleteBlobAsync(fileName);
		logger.LogInformation($"Deleted recording {fileName} for container {containerName}");
	}
}
