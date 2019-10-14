using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MvcMusicStoreFall2018.Models
{
    public class MusicStoreDbInitializer : DropCreateDatabaseAlways<MusicStoreDB>
    {
        protected override void Seed(MusicStoreDB context)
        {
            
            context.Genres.Add(new Genre { Name = "Alternative Rock" });
            context.Genres.Add(new Genre { Name = "Country" });

            context.Artists.Add(new Artist { Name= "Pink Floyd" });
            context.Artists.Add(new Artist { Name = "John Denver" });

            context.Albums.Add(
                new Album
                {
                    Title = "Achtung Baby",
                    AlbumArtUrl = "~/Images/albumImage.jpg",
                    Price = 12.99m,
                    Artist = new Artist { Name = "U2" },
                    Genre = new Genre { Name = "Rock" }
                }
                );

            context.SaveChanges();


            context.Albums.Add(
                new Album
                {
                    Title = "Use your Illusion",
                    AlbumArtUrl = "~/Images/albumImage.jpg",
                    Price = 13.99m,
                    Artist = new Artist { Name = "Guns N Roses" },
                    GenreId = 3 
                }
                );



            context.SaveChanges();

            base.Seed(context);
        }

    }
}