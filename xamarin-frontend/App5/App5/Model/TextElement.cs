using System;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace App5.Entities
{
    public class TextElement
    {
        [PrimaryKey]
        public string id { get; set; }
        [ForeignKey(typeof(Note))] // Specify the foreign key
        public string noteId { get; set; }
        public long x { get; set; }
        public long y { get; set; }
        public string text { get; set; }
        public int fontSize { get; set; }

        public TextElement()
        {
            id = Guid.NewGuid().ToString();
        }
    }

}