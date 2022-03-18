using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using App5.model;
using App5.repo;
using SQLiteNetExtensions.Extensions;
using Xamarin.Forms;

namespace App5.Entities
{
    public class Service
    {
        private Repository _repository;

        public EventHandler<EventArgs> AccoundUpdated;

        public Service(Repository repository)
        {
            _repository = repository;

            _options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }


        public Note CreateNote()
        {
            Note note = new Note();
            note.title = "Новая заметка";
            note.color = "WHITE";
            note.accountId = App.AccountId;
            GetAllNotes().Add(note); // to context
            _repository.Connection.Insert(note);

            StartAccountPushing();

            return note;
        }

        private HttpClient GetClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            return client;
        }

        public async void PushAccountAsync(Account account)
        {
            // HttpClient client = GetClient();
            try
            {
                var serialize = JsonSerializer.Serialize(account);


                HttpClient client = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage();
                request.RequestUri = new Uri("http://localhost:9999/api/push");
                request.Method = HttpMethod.Post;
                request.Content = new StringContent(serialize, Encoding.UTF8, "application/json");
                request.Headers.Add("Accept", "application/json");

                await client.SendAsync(request);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                // throw;
            }
        }

        public async void StartAccountPushing()
        {
            try
            {
                var account = getAccount(App.AccountId);
                PushAccountAsync(account);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                // throw;
            }
        }

        public void StartAccountPullingSync()
        {
            try
            {
                var task = Task.Run(() => PullAccountAsync(App.AccountId));
                task.Wait();

                var account = task.Result;
                _repository.Connection.InsertOrReplaceWithChildren(account, true);
                // UpdateEntity(account);
                AccoundUpdated?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                // throw;
            }
        }

        public async Task<Account> PullAccountAsync(string accountId)
        {
            // HttpClient client = GetClient();

            // var account = App.Account;
            // var serialize = JsonSerializer.Serialize(account);

            // HttpClientHandler handler = new HttpClientHandler();
            // handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
            // {
            // if (cert.Issuer.Equals("CN=localhost"))
            // return true;
            // return errors == System.Net.Security.SslPolicyErrors.None;
            // };

            try
            {
                HttpClient client = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage();
                request.RequestUri = new Uri($"http://localhost:9999/api/pull?accountID={accountId}");
                request.Method = HttpMethod.Get;
                // request.Content = new StringContent(serialize, Encoding.UTF8, "application/json");
                request.Headers.Add("Accept", "application/json");

                HttpResponseMessage response = await client.SendAsync(request);

                var account = JsonSerializer.Deserialize<Account>(
                    await response.Content.ReadAsStringAsync(), _options);
                return account;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }


        public List<Note> GetAllNotes()
        {
            return getAccount(App.AccountId).notes;
        }

        public TextElement CreateTextElement(Note note, long x, long y, int fontSize)
        {
            TextElement textElement = new TextElement();
            textElement.x = x;
            textElement.y = y;
            textElement.fontSize = fontSize;
            textElement.text = "Text...";
            textElement.noteId = note.id;
            note.textElements.Add(textElement);
            _repository.Connection.Insert(textElement);

            StartAccountPushing();

            return textElement;
        }

        public ImageElement CreateImageElement(Note note, long x, long y, FileInfo imageFileInfo)
        {
            ImageElement imageElement = new ImageElement();
            imageElement.x = x;
            imageElement.y = y;
            imageElement.image = imageFileInfo;
            imageElement.noteId = note.id;
            note.imageElements.Add(imageElement);
            _repository.Connection.Insert(imageElement);

            StartAccountPushing();

            return imageElement;
        }

        private Dictionary<FileInfo, ImageSource> map = new Dictionary<FileInfo, ImageSource>();
        private JsonSerializerOptions _options;

        public FileInfo SaveImage(string filepath)
        {
            ImageSource imageSource = ImageSource.FromFile(filepath);
            FileInfo fileInfo = new FileInfo();
            _repository.Connection.Insert(fileInfo);
            map.Add(fileInfo, imageSource);

            StartAccountPushing();

            return fileInfo;
        }

        public ImageSource getImageSource(FileInfo fileInfo)
        {
            var containsKey = map.ContainsKey(fileInfo);

            return containsKey ? map[fileInfo] : null;
        }

        public Account getAccount(string accountId)
        {
            try
            {
                return _repository.Connection.GetWithChildren<Account>(accountId, true);
            }
            catch (Exception e)
            {
                var account = new Account();
                account.id = accountId;
                _repository.Connection.InsertWithChildren(account);
                return account;
            }
        }

        public void UpdateEntity(object entity)
        {
            _repository.Connection.UpdateWithChildren(entity);
            StartAccountPushing();
        }

        public void DeleteNote(Note selectedNoteNote)
        {
            var account = getAccount(selectedNoteNote.accountId);
            account.notes = account.notes.Where(note => note.id != selectedNoteNote.id).ToList();
            _repository.Connection.Delete(selectedNoteNote);
            UpdateEntity(account);


            // StartAccountPushing();
        }
    }
}