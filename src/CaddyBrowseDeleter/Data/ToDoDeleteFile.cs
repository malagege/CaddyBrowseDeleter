using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
    /// 是否準備刪除。
    /// </summary>
    /// <value></value>
    [NotMapped]
    public bool IsReadyToDelete { get; set; }

    
    // 多對多關係
    public ICollection<User> Users { get; set; }
}