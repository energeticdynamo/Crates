using Crates.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.SemanticKernel.Embeddings;
using Pgvector; // Required for the Vector type

namespace Crates.Data
{
    public static class DbInitializer
    {
        // 1. Changed to 'async Task' to handle the AI calls
        // 2. Added 'ITextEmbeddingGenerationService' parameter
        public static async Task InitializeAsync(CratesContext context, ITextEmbeddingGenerationService embeddingService)
        {
            // Ensure the DB exists
            await context.Database.MigrateAsync();

            // If we already have data, don't do anything
            if (context.Albums.Any())
            {
                return;
            }

            // --- Define Metadata ---
            var hipHop = new Genre { Name = "Hip Hop" };
            var jazzRap = new Genre { Name = "Jazz Rap" };
            var underground = new Genre { Name = "Underground" };

            var vinyl = new Tag { Name = "Vinyl" };
            var classic = new Tag { Name = "Classic" };
            var wishList = new Tag { Name = "Wishlist" };

            var nas = new Artist { Name = "Nas", Bio = "Queensbridge legend." };
            var aTribeCalledQuest = new Artist { Name = "A Tribe Called Quest", Bio = "Native Tongues pioneers." };
            var kDot = new Artist { Name = "Kendrick Lamar", Bio = "Compton's finest." };

            // --- Define Albums (Use a List instead of an Array for flexibility) ---
            var albums = new List<Album>
            {
                new Album
                {
                    Title = "Illmatic",
                    Artist = nas,
                    ReleaseDate = new DateTime(1994, 4, 19).ToUniversalTime(),
                    Price = 9.99m,
                    Description = "The genesis of Queensbridge lyricism. Gritty, raw, and poetic depictions of NYC street life.",
                    Genres = new List<Genre> { hipHop },
                    Tags = new List<Tag> { vinyl, classic }
                },
                new Album
                {
                    Title = "The Low End Theory",
                    Artist = aTribeCalledQuest,
                    ReleaseDate = new DateTime(1991, 9, 24).ToUniversalTime(),
                    Price = 11.99m,
                    Description = "Jazz-infused hip hop masterpiece. Smooth basslines and conscious lyrics.",
                    Genres = new List<Genre> { hipHop, jazzRap },
                    Tags = new List<Tag> { classic, wishList }
                },
                new Album
                {
                    Title = "good kid, m.A.A.d city",
                    Artist = kDot,
                    ReleaseDate = new DateTime(2012, 10, 22).ToUniversalTime(),
                    Price = 10.99m,
                    Description = "A cinematic journey through Compton. Introspective storytelling and West Coast sounds.",
                    Genres = new List<Genre> { hipHop, underground },
                    Tags = new List<Tag> { vinyl, wishList }
                }
            };

            // --- NEW LOGIC: The Loop ---
            // We iterate through the list we just made to generate vectors
            foreach (var album in albums)
            {
                if (!string.IsNullOrEmpty(album.Description))
                {
                    // Call the AI service to turn text into numbers
                    var embedding = await embeddingService.GenerateEmbeddingAsync(album.Description);

                    // Save those numbers into the new Vector column
                    // We use the Pgvector 'Vector' type here
                    album.Vector = new Vector(embedding.ToArray());
                }
            }

            // Save everything to the database
            context.Albums.AddRange(albums);
            await context.SaveChangesAsync();
        }
    }
}