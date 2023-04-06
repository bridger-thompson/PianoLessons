using Azure.Identity;
using Azure.Storage;
using Azure.Storage.Blobs;
using PianoLessons.Shared.Data;
using System.Collections;

namespace PianoLessonsApi.Repositories;

public class RecordingRepo
{
	private readonly IConfiguration config;
	private BlobServiceClient client;
	public RecordingRepo(IConfiguration config)
	{
		Uri accountUri = new Uri("https://pianorecordings.blob.core.windows.net/");
		string accountName = config["StorageAccountName"];
		string accountKey = config["StorageAccountAccessKey"];
		StorageSharedKeyCredential credential = new StorageSharedKeyCredential(accountName, accountKey);
		client = new BlobServiceClient(accountUri, credential);
		this.config = config;
	}

	public async Task SendRecordingToAzure(FileData data, string studentId)
	{
		BlobContainerClient containerClient = client.GetBlobContainerClient(studentId);
		await containerClient.CreateIfNotExistsAsync();
		MemoryStream stream = new MemoryStream(data.Data);
		await containerClient.UploadBlobAsync(data.FileName, stream);
	}
}
