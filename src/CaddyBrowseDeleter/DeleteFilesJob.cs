using System.Threading.Tasks;

using CaddyBrowseDeleter;

using Coravel.Invocable;
using Microsoft.EntityFrameworkCore;

public class DeleteFilesJob : IInvocable
{
    private readonly AppDbContext _context;

    public DeleteFilesJob(AppDbContext context)
    {
        _context = context;
    }

    public async Task Invoke()
    {
        var allUsers = await _context.Users.ToListAsync();
        var filesToDelete = await _context.ToDoDeleteFiles
            .Include(x => x.Users)
            .Where(x => x.Users.All(u => allUsers.Contains(u)))
            .ToListAsync();

        foreach (var file in filesToDelete)
        {
            // 判斷路徑是否資料夾或檔案
            if (!System.IO.File.Exists(file.FilePath) && !System.IO.Directory.Exists(file.FilePath))
            {
                // 如果檔案不存在，則刪除資料庫中的記錄
                _context.ToDoDeleteFiles.Remove(file);
                continue;
            }
            
            // 刪除檔案
            if (System.IO.Directory.Exists(file.FilePath))
                System.IO.Directory.Delete(file.FilePath, true);
            else
                System.IO.File.Delete(file.FilePath);

            // 刪除資料庫中的記錄
            _context.ToDoDeleteFiles.Remove(file);
        }

        await _context.SaveChangesAsync();
    }
}
