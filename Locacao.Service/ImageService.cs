using Amazon.S3;
using Amazon.S3.Transfer;
using Locacao.Service.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

public class ImageService : IImageUploadService
{
    private readonly IAmazonS3 _s3Client;
    private readonly string _bucketName;

    public ImageService(IAmazonS3 s3Client, string bucketName)
    {
        _s3Client = s3Client;
        _bucketName = bucketName;
    }

    public async Task<string> UploadImageAsync(IFormFile image)
    {
        if (image == null || image.Length == 0)
            throw new ArgumentException("Imagem inválida");

        var fileName = $"{Guid.NewGuid()}_{image.FileName}";

        using var newMemoryStream = new MemoryStream();
        image.CopyTo(newMemoryStream);

        var uploadRequest = new TransferUtilityUploadRequest
        {
            InputStream = newMemoryStream,
            Key = fileName,
            BucketName = _bucketName,
            ContentType = image.ContentType,
            CannedACL = S3CannedACL.PublicRead
        };

        var fileTransferUtility = new TransferUtility(_s3Client);
        await fileTransferUtility.UploadAsync(uploadRequest);

        return $"https://{_bucketName}.s3.amazonaws.com/{fileName}";
    }
}
