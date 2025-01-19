using System.Threading.Tasks;

using CaddyBrowseDeleter;

using Coravel.Invocable;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public class DeleteFilesJob : IInvocable
{
    private readonly AppDbContext _context;
    private readonly ILogger<DeleteFilesJob> _logger;

    public DeleteFilesJob(AppDbContext context, ILogger<DeleteFilesJob> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task Invoke()
    {
        var currentDirectory = System.IO.Directory.GetCurrentDirectory();
        _logger.LogInformation($"Current Directory: {currentDirectory}");
        var allUsers = await _context.Users.ToListAsync();
        var allUsersId = allUsers.Select(x => x.Id).ToList();
        var filesToDelete = await _context.ToDoDeleteFiles
            .Include(x => x.Users)
            .ToListAsync();
        filesToDelete = filesToDelete.Where(x => allUsersId.All(y => x.Users.Select(z => z.Id).Contains(y))).ToList();
        var prefixPath = "./extHDD";
        foreach (var file in filesToDelete)
        {
            // 判斷路徑是否資料夾或檔案
            if (!System.IO.File.Exists( prefixPath + file.FilePath) && !System.IO.Directory.Exists( prefixPath + file.FilePath))
            {
                // 如果檔案不存在，則刪除資料庫中的記錄
                _context.ToDoDeleteFiles.Remove(file);
                _logger.LogWarning($"File not found: {file.FilePath}");
                continue;
            }
            _logger.LogInformation($"Deleting: {file.FilePath}");
            // 刪除檔案
            if (System.IO.Directory.Exists( prefixPath + file.FilePath))
            {
                _logger.LogInformation($"Deleting directory: {file.FilePath}");
                System.IO.Directory.Delete( prefixPath + file.FilePath, true);
            }
            else
            {
                _logger.LogInformation($"Deleting file: {file.FilePath}");
                System.IO.File.Delete( prefixPath + file.FilePath);
            }

            // 刪除資料庫中的記錄
            _context.ToDoDeleteFiles.Remove(file);
        }

        await _context.SaveChangesAsync();
    }
}
