using DragTrans.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using DragTrans.Domain.Entities;

namespace DragTrans.Application.Services.Files;

public class FileService
{
    private readonly IFileRepository _fileRepository;
    
    public FileService(IFileRepository fileRepository)
    {
        _fileRepository = fileRepository;
    }


    public async Task<StoredFile> UploadAsync(IFormFile file, Guid userId)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("Файл пустой.");

        var storedName = Guid.NewGuid().ToString();

        var storagePath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "..", "..", "..", "database", "storage", userId.ToString(), "Files");
        Directory.CreateDirectory(storagePath);

        var filePath = Path.Combine(storagePath, storedName);

        await using var steam = new FileStream(filePath, FileMode.Create);

        await file.CopyToAsync(steam);

        var storedFile = new StoredFile
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            OriginalName = file.FileName,
            StoredName = storedName,
            Size = file.Length,
            ContentType = file.ContentType,
            CreatedAt = DateTime.UtcNow
        };

        await _fileRepository.AddAsync(storedFile);
        await _fileRepository.SaveChangesAsync();

        return storedFile;
    }

    public async Task DeleteAsync(Guid fileId, Guid userId)
    {
        StoredFile? file = await _fileRepository.GetByIdAsync(fileId);

        if (file == null)
        {
            throw new KeyNotFoundException("Файл не найден.");
        }

        if (file.UserId != userId)
        {
            throw new UnauthorizedAccessException(
                "Вы не можете удалить этот файл.");
        }

        var filePath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "..", "..", "..", "database", "storage", userId.ToString(), "Files", file.StoredName);

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        await _fileRepository.DeleteAsync(file);
    }
}