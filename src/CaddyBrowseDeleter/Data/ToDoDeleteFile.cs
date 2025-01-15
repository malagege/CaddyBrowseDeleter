using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data;

/// <summary>
/// 表示要刪除的檔案資訊。
/// </summary>
public class ToDoDeleteFile
{
    /// <summary>
    /// 檔案的唯一識別碼。
    /// </summary>
    public long Id { get; set; }
    
    /// <summary>
    /// 檔案的路徑。
    /// </summary>
    public string FilePath { get; set; }
    
    /// <summary>
    /// 目錄的路徑。
    /// </summary>
    public string DirPath { get; set; }
    
    /// <summary>
    /// 關聯的使用者。
    /// </summary>
    public User User { get; set; }
    
    /// <summary>
    /// 使用者的唯一識別碼。
    /// </summary>
    public long UserId { get; set; }
}