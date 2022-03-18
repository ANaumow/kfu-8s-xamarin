using System;
using SQLite;

namespace App5.model
{
    public class FileInfo
    {
        [PrimaryKey]
        public string id { get; set; }

        public FileInfo()
        {
            id = Guid.NewGuid().ToString();
        }
    }
}