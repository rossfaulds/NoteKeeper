using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Notekeeper.Models
{

    public class NotesDbContext : DbContext
    {
        public DbSet<Note> Notes { get; set; }
    }
    public class Note
    {
        public int Id { get; set; }

        [Required (ErrorMessage="Title is mandatory")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Content is mandatory")]
        public string Content { get; set; }
        
        public string Categories { get; set; }

        [Required(ErrorMessage = "Color is mandatory")]
        public string ColorClass { get; set; }
    }

}