using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace BookStore.Models
{
    public class BookDbInitializer : DropCreateDatabaseAlways<BookContext>
    {
        protected override void Seed(BookContext db)
        {
            db.Books.Add(new Book { Name = "War and Peace", Author = "L.Tolstoj", Price = 220 });
            db.Books.Add(new Book { Name = "Fathers and children", Author = "I.Turgenev", Price = 180 });
            db.Books.Add(new Book { Name = "Seagull", Author = "A.Chehov", Price = 150 });
            base.Seed(db);
        }
    }
}