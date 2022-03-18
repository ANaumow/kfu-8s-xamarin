using System;
using System.Diagnostics;
using System.IO;
using App5.Entities;
using App5.model;
using SQLite;
using SQLiteNetExtensions.Extensions;
using Xamarin.Essentials;
using Element = Xamarin.Forms.Element;
using FileInfo = App5.model.FileInfo;

namespace App5.repo
{
    public class Repository
    {
        // public static Repository Instance = new Repository();

        public SQLiteConnection Connection;

        public Repository()
        {
            
            var databasePath = Path.Combine(FileSystem.AppDataDirectory, "MyData.db");

            Connection = new SQLiteConnection(databasePath);

            Connection.CreateTable<Account>();
            Connection.CreateTable<TextElement>();
            Connection.CreateTable<FileInfo>();
            Connection.CreateTable<ImageElement>();
            Connection.CreateTable<Note>();
        }
        

        public void saveUser(Account account)
        {
            Connection.Insert(account);
        }
    }
}