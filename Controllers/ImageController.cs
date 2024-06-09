using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

public class ImageController : Controller
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly string _containerName;

    public ImageController(BlobServiceClient blobServiceClient, IConfiguration configuration)
    {
        _blobServiceClient = blobServiceClient;
        _containerName = configuration["AzureBlobStorage:ContainerName"];
    }


    [HttpGet]
    public IActionResult UploadImage(string deskId)
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> UploadImage(IFormFile file, string deskId)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("Nie wybrano pliku.");
        }

        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        await containerClient.CreateIfNotExistsAsync();

        var blobClient = containerClient.GetBlobClient($"{deskId}/{Guid.NewGuid()}_{file.FileName}");

        using (var stream = file.OpenReadStream())
        {
            await blobClient.UploadAsync(stream, true);
        }

        return RedirectToAction("UploadImage", "Image");
    }

    public async Task<IActionResult> ListImages()
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        var blobs = containerClient.GetBlobsAsync();

        var imageUrls = new List<string>();

        await foreach (var blob in blobs)
        {
            var blobClient = containerClient.GetBlobClient(blob.Name);
            imageUrls.Add(blobClient.Uri.ToString());
        }

        return View(imageUrls);
    }
}

