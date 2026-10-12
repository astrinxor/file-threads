using DragTrans.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using DragTrans.Application.Interfaces;
using DragTrans.Domain.Entities;

namespace DragTrans.Infrastructure.Repositories;

public class FileRepository : IFileRepository
{
    private readonly DragTransDbContext _db;

    public FileRepository(DragTransDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(StoredFile file)
    {
        await _db.StoredFiles.AddAsync(file);
    }

    public async Task<StoredFile?> GetByIdAsync(Guid id)
    {
        var AllFiles = await _db.StoredFiles.FirstOrDefaultAsync(file => file.Id == id);

        if(AllFiles == null)
        {
            return null;
        }
        return AllFiles;
    }

    public async Task<List<StoredFile>> GetByUserIdAsync(Guid userId)
    {
        return await _db.StoredFiles.Where(file => file.UserId == userId).ToListAsync();
    }

    public async Task DeleteAsync(StoredFile file)
    {
        _db.StoredFiles.Remove(file);

        await _db.SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _db.SaveChangesAsync();
    }


}
