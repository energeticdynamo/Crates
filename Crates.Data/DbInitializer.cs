using Crates.Domain;

namespace Crates.Data
{
    public static class DbInitializer
    {
        public static void Initialize(CratesContext context)
        {
            context.Database.EnsureCreated();

            if (context.Albums.Any())
            {
                return;
            }

            // Create Generes & Tags first (so we can reuse them)
            var hipHop = new Genre { Name = "Hip Hop" };
            var jazzRap = new Genre { Name = "Jazz Rap" };
            var underground = new Genre { Name = "Underground" };

            var vinyl = new Tag { Name = "Vinyl" };
            var classic = new Tag { Name = "Classic" };
            var wishList = new Tag { Name = "Wishlist" };

            // Create Artists
            var nas = new Artist { Name = "Nas", Bio = "Queensbridge legend." };
            var aTribeCalledQuest = new Artist { Name = "A Tribe Called Quest", Bio = "Native Tongues pioneers." };
            var kDot = new Artist { Name = "Kendrick Lamar", Bio = "Compton's finest." };

            // Create Albums with Relationships

            var albums = new Album[]
            {
                new Album
                {
                    Title = "Illmatic",
                    Artist = nas,
                    ReleaseDate = new DateTime(1994, 4, 19).ToUniversalTime(),
                    Price = 9.99m,
                    Description = "The genesis of Queensbridge lyricism.",
                    Genres = new List<Genre> { hipHop },
                    Tags = new List<Tag> { vinyl, classic }
                },
                new Album
                {
                    Title = "The Low End Theory",
                    Artist = aTribeCalledQuest,
                    ReleaseDate = new DateTime(1991, 9, 24).ToUniversalTime(),
                    Price = 11.99m,
                    Description = "Jazz-infused hip hop masterpiece.",
                    Genres = new List<Genre> { hipHop, jazzRap },
                    Tags = new List<Tag> { classic, wishList }
                },
                new Album
                {
                    Title = "good kid, m.A.A.d city",
                    Artist = kDot,
                    ReleaseDate = new DateTime(2012, 10, 22).ToUniversalTime(),
                    Price = 10.99m,
                    Description = "A cinematic journey through Compton.",
                    Genres = new List<Genre> { hipHop, underground },
                    Tags = new List<Tag> { vinyl, wishList }
                }
            };

            context.Albums.AddRange(albums);
            context.SaveChanges(); ;
        }
    }
}
