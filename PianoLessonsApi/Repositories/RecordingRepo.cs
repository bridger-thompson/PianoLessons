using Azure.Identity;
using Azure.Storage.Blobs;
using PianoLessons.Shared.Data;
using System.Collections;

namespace PianoLessonsApi.Repositories;

public class RecordingRepo
{
	private BlobServiceClient client;
	public RecordingRepo()
	{
		Uri accountUri = new Uri("https://pianorecordings.blob.core.windows.net/");
		client = new BlobServiceClient(accountUri, new DefaultAzureCredential());
	}

	public async Task SendRecordingToAzure(FileData data, string studentId)
	{
		BlobContainerClient containerClient = client.GetBlobContainerClient(studentId);
		await containerClient.CreateIfNotExistsAsync();
		MemoryStream stream = new MemoryStream(data.Data);
		await containerClient.UploadBlobAsync(data.FileName, stream);
	}
}
