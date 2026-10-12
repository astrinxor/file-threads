using DragTrans.Domain.Entities;

namespace DragTrans.Application.Interfaces;

public interface IFileRepository
{
    Task AddAsync(StoredFile file);
    Task<StoredFile?> GetByIdAsync(Guid id);
    Task<List<StoredFile>> GetByUserIdAsync(Guid userId);
    Task DeleteAsync(StoredFile file);
    Task SaveChangesAsync();
}