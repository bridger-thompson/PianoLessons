using Azure.Identity;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using PianoLessons.Shared.Data;
using System.Collections;

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
		await containerClient.SetAccessPolicyAsync(PublicAccessType.Blob);
		await containerClient.CreateIfNotExistsAsync();
		MemoryStream stream = new MemoryStream(data.Data);
		logger.LogInformation($"Uploading blob");
		await containerClient.UploadBlobAsync(data.FileName, stream);
		logger.LogInformation("Successfully uploaded recording");
		return $"https://pianorecordings.blob.core.windows.net/{containerName}/{data.FileName}";
	}
}
