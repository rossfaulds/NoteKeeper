using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Notekeeper.Models;

namespace Notekeeper.Controllers
{
    public class HomeController : Controller
    {
        static HttpClient _client = new HttpClient();
        static async Task RunAsync()
        {
            // New code:
            _client.BaseAddress = new Uri("http://localhost:55268/");
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        static async Task<Note> GetNoteAsync(string path)
        {
            Note note = null;
            HttpResponseMessage response = await _client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                note = await response.Content.ReadAsAsync<Note>();
            }
            return note;
        }

        static async Task<List<Note>> GetNotesAsync(string path)
        {
            List<Note> notes = new List<Note>();
            HttpResponseMessage response = await _client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                notes = await response.Content.ReadAsAsync<List<Note>>();
            }
            return notes;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        //[Authorize]
        public ActionResult Notes()
        {

            ViewBag.Message = "Your notes.";
            var db = new NotesDbContext();
            var notes = db.Notes.ToList();
           
//            notes.Add( new Note {Title="Entity Framework Annotations",Categories = "CSharp",ColorClass="pastel-red", Content= "KeyAttribute<br/>StringLengthAttribute<br/>MaxLengthAttribute<br/>ConcurrencyCheckAttribute<br/>RequiredAttribute<br/>TimestampAttribute<br/>ComplexTypeAttribute <br/>ColumnAttribute <br/>TableAttribute <br/>InversePropertyAttribute <br/>ForeignKeyAttribute <br/>DatabaseGeneratedAttribute <br/>NotMappedAttribute <br/>", WidthClass=string.Empty });
//            notes.Add(new Note { Title = "OOP Design Principles", Categories = "Javascript",ColorClass = "pastel-purple", Content = "Encapsulate what varies<br/>Favor composition over inheritance<br/>Program to interfaces, not to implementations<br/>Strive for loosely coupled design between objects that interact<br/>Depend upon abstractions.Do not depend upon concrete classes</br>", WidthClass = string.Empty });
//            notes.Add(new Note { Title = "TSQL Cursor example", Categories="Node",Content = @"<br/>This backs up all databases in a serial fashion but a good example of the 
//<br/>format of a TSQL cursor for loop.
//<br/>
//<br/>
//<br/>DECLARE @name VARCHAR(50) -- database name  
//<br/>DECLARE @path VARCHAR(256) -- path for backup files  
//<br/>DECLARE @fileName VARCHAR(256) -- filename for backup  
//<br/>DECLARE @fileDate VARCHAR(20) -- used for file name 
//<br/>
//<br/>SET @path = 'C:\Backup\'  
//<br/>
//<br/>SELECT @fileDate = CONVERT(VARCHAR(20),GETDATE(),112) 
//<br/>
//<br/>DECLARE db_cursor CURSOR FOR  
//<br/>SELECT name 
//<br/>FROM MASTER.dbo.sysdatabases 
//<br/>WHERE name NOT IN ('master','model','msdb','tempdb')  
//<br/>
//<br/>OPEN db_cursor   
//<br/>FETCH NEXT FROM db_cursor INTO @name   
//<br/>
//<br/>WHILE @@FETCH_STATUS = 0   
//<br/>BEGIN   
//<br/>       SET @fileName = @path + @name + '_' + @fileDate + '.BAK'  
//<br/>       BACKUP DATABASE @name TO DISK = @fileName  
//<br/>
//<br/>       FETCH NEXT FROM db_cursor INTO @name   
//<br/>END   
//<br/>
//<br/>CLOSE db_cursor   
//<br/>DEALLOCATE db_cursor", WidthClass = string.Empty, ColorClass= "pastel-orange" });

            return View(notes);
        }
    }
}