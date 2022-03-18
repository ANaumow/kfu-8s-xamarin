using System;
using System.Collections.Generic;
using App5.model;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace App5.Entities
{
    public class Note
    {
        [PrimaryKey]
        public string id { get; set; }

        [ForeignKey(typeof(Account))]
        public string accountId { get; set; }

        public string title { get; set; }
        
        public string color { get; set; }
        
        public double? latitude { get; set; }
        
        public double? longitude { get; set; }
        
        public int createdTime { get; set; }
        
        public int changedTime { get; set; }
        
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<TextElement> textElements { get; set; }
        
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<ImageElement> imageElements { get; set; }

        public Note()
        {
            id = Guid.NewGuid().ToString();
            textElements = new List<TextElement>();
            imageElements = new List<ImageElement>();
        }
    }
}