using System;
using App5.Entities;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace App5.model
{
    public class ImageElement
    {
        [PrimaryKey]
        public string id { get; set; }
        [ForeignKey(typeof(Note))] // Specify the foreign key
        public string noteId { get; set; }
        public long x { get; set; }
        public long y { get; set; }
        [OneToOne(CascadeOperations = CascadeOperation.All)]
        public FileInfo image { get; set; }
        
        [ForeignKey(typeof(FileInfo))] 
        public String imageId { get; set; }
        
        public ImageElement()
        {
            id = Guid.NewGuid().ToString();
        }
    }
}