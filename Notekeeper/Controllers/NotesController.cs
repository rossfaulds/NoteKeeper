using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Notekeeper.Models;

namespace Notekeeper.Controllers
{
    public class NotesController : ApiController
    {
        private NotesDbContext db = new NotesDbContext();

        // GET: api/Notes
        public List<Note> GetNotes()
        {
            return db.Notes.ToList();
        }

        // GET: api/Notes/5
        [ResponseType(typeof(Note))]
        public IHttpActionResult GetNote(int id)
        {
            Note note = db.Notes.Find(id);
            if (note == null)
            {
                return NotFound();
            }

            return Ok(note);
        }

        // PUT: api/Notes/5
        [ResponseType(typeof(void))]
        [Route("api/UpdateNote/{id}")]
        [HttpPut]
        public IHttpActionResult UpdateNote(int id, Note note)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            if (id != note.Id)
            {
                var errorString = string.Format($"{id} not {note.Id}");
                return BadRequest(errorString);
            }

            db.Entry(note).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NoteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Notes
        [ResponseType(typeof(Note))]
        [HttpPost]
        [Route("api/PostNote")]
        public IHttpActionResult PostNote(Note note)
        {
           /* if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            */
            db.Notes.Add(note);
            db.SaveChanges();

            return Ok(note.Id);
        }

        // DELETE: api/Notes/5
        [ResponseType(typeof(Note))]
        [HttpDelete]
        [Route("api/DeleteNote/{id}")]
        public IHttpActionResult DeleteNote(int id)
        {
            Note note = db.Notes.Find(id);
            if (note == null)
            {
                return NotFound();
            }

            db.Notes.Remove(note);
            db.SaveChanges();

            return Ok(note);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool NoteExists(int id)
        {
            return db.Notes.Count(e => e.Id == id) > 0;
        }
    }
}