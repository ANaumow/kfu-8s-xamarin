using System;
using System.Collections.Generic;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace App5.Entities
{
    public class Account
    {
        [PrimaryKey]
        public string id { get; set; }
        
        public string name { get; set; }
        
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Note> notes { get; set; }

        public Account()
        {
            id = Guid.NewGuid().ToString();
            notes = new List<Note>();
        }
    }
}