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
            return View(notes);
        }
    }
}