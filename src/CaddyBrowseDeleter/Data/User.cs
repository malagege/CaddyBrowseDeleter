using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data
{
    public class User
    {
        public long Id { get; set; }
        public string Name { get; set; }

        // 多對多關係
        public ICollection<ToDoDeleteFile> ToDoDeleteFiles { get; set; }
    }
}