using System.ComponentModel.DataAnnotations.Schema;
using SQLite;
using extraordinarioNET.Model;

namespace extraordinarioNET.Servicios
{
    public class BaseDeDatos
    {
        private readonly SQLiteAsyncConnection _database;
        private static bool _initialized = false;
        private static readonly object _lock = new object();

        public BaseDeDatos(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
        }

        // Método para inicializar la base de datos de forma asíncrona
        public async Task InitializeAsync()
        {
            if (_initialized) return;

            lock (_lock)
            {
                if (_initialized) return;
                _initialized = true;
            }

            try
            {
                await _database.CreateTableAsync<Artista>();
                await _database.CreateTableAsync<Cancion>();
                await SeedDataAsync();
            }
            catch (Exception ex)
            {
                // Log el error o manéjalo según necesites
                System.Diagnostics.Debug.WriteLine($"Error inicializando base de datos: {ex.Message}");
                throw;
            }
        }

        // Método helper para asegurar que la DB esté inicializada
        private async Task EnsureInitializedAsync()
        {
            if (!_initialized)
            {
                await InitializeAsync();
            }
        }

        // Artists
        public async Task<List<Artista>> GetArtistasAsync()
        {
            await EnsureInitializedAsync();
            return await _database.Table<Artista>().OrderBy(a => a.Ranking).ToListAsync();
        }

        public async Task<Artista> GetArtistaAsync(int id)
        {
            await EnsureInitializedAsync();
            return await _database.Table<Artista>().Where(a => a.Id == id).FirstOrDefaultAsync();
        }

        public async Task<int> SaveArtistaAsync(Artista artista)
        {
            await EnsureInitializedAsync();
            if (artista.Id != 0)
                return await _database.UpdateAsync(artista);
            else
                return await _database.InsertAsync(artista);
        }

        // Songs
        public async Task<List<Cancion>> GetCancionesPorArtistaAsync(int artistaId)
        {
            await EnsureInitializedAsync();
            return await _database.Table<Cancion>().Where(s => s.IdArtista == artistaId).ToListAsync();
        }

        public async Task<Cancion> GetSongAsync(int id)
        {
            await EnsureInitializedAsync();
            return await _database.Table<Cancion>().Where(s => s.Id == id).FirstOrDefaultAsync();
        }

        public async Task<int> SaveCancionAsync(Cancion cancion)
        {
            await EnsureInitializedAsync();
            if (cancion.Id != 0)
                return await _database.UpdateAsync(cancion);
            else
                return await _database.InsertAsync(cancion);
        }

        private async Task SeedDataAsync()
        {
            var artistaCount = await _database.Table<Artista>().CountAsync();
            if (artistaCount > 0) return;

            var artistas = new List<Artista>
            {
                new Artista { Nombre = "BTS", Foto = "bts.jpg", Ranking = 1, UltimoAlbum = "Proof"},
                new Artista { Nombre = "Stray Kids", Foto = "sk.jpg", Ranking = 2, UltimoAlbum = "Mixtape:Dominate" },
                new Artista { Nombre = "Enhypen", Foto = "enhypen.jpg", Ranking = 3, UltimoAlbum = "Desire : Unleash" },
                new Artista { Nombre = "Twice", Foto = "twice.jpg", Ranking = 4, UltimoAlbum = "This is For" },
                new Artista { Nombre = "Black Pink", Foto = "bp.jpg", Ranking = 5, UltimoAlbum = "Born Pink" },
                new Artista { Nombre = "New Jeans", Foto = "nj.jpg", Ranking = 6, UltimoAlbum = "NJWMX"},
                new Artista { Nombre = "Ive", Foto = "ive.jpg", Ranking = 7, UltimoAlbum = "I've Ive" },
                new Artista { Nombre = "TXT", Foto = "txt.jpg", Ranking = 8, UltimoAlbum = "Freefall" },
                new Artista { Nombre = "EXO", Foto = "exo.jpg", Ranking = 9, UltimoAlbum = "Exist"},
                new Artista { Nombre = "The rose", Foto = "tr.jpg", Ranking = 10, UltimoAlbum = "WRLD" }
            };

            foreach (var artista in artistas)
            {
                await _database.InsertAsync(artista);
            }

            var canciones = new List<Cancion>
            {
                // BTS
                new Cancion { Nombre = "The Truth Untold", IdArtista = 1, Imagen = "anti_hero.jpg", AnoEstreno = 2018, Duracion = "4:02", Letra = "[Contenido de la canción - letra original]" },
                new Cancion { Nombre = "Film Out ", IdArtista = 1, Imagen = "shake_it_off.jpg", AnoEstreno = 2021, Duracion = "3:35", Letra = "[Contenido de la canción - letra original]" },
                new Cancion { Nombre = "ON ", IdArtista = 1, Imagen = "love_story.jpg", AnoEstreno = 2020, Duracion = "4:07", Letra = "[Contenido de la canción - letra original]" },
                new Cancion { Nombre = "I NEED U", IdArtista = 1, Imagen = "blank_space.jpg", AnoEstreno = 2015, Duracion = "3:30", Letra = "[Contenido de la canción - letra original]" },
                new Cancion { Nombre = "Butterfly", IdArtista = 1, Imagen = "22.jpg", AnoEstreno = 2015, Duracion = "3:59", Letra = "[Contenido de la canción - letra original]" },

                // Stray Kids
                new Cancion { Nombre = "God's Menu", IdArtista = 2, Imagen = "anti_hero.jpg", AnoEstreno = 2020, Duracion = "2:48", Letra = "[Contenido de la canción - letra original]" },
                new Cancion { Nombre = "Maniac ", IdArtista = 2, Imagen = "shake_it_off.jpg", AnoEstreno = 2022, Duracion = "3:03", Letra = "[Contenido de la canción - letra original]" },
                new Cancion { Nombre = "Chk Chk Boom", IdArtista = 2, Imagen = "love_story.jpg", AnoEstreno = 2024, Duracion = "2:28", Letra = "[Contenido de la canción - letra original]" },
                new Cancion { Nombre = "Thunderous", IdArtista = 2, Imagen = "blank_space.jpg", AnoEstreno = 2021, Duracion = "3:03", Letra = "[Contenido de la canción - letra original]" },
                new Cancion { Nombre = "Miroh", IdArtista = 2, Imagen = "22.jpg", AnoEstreno = 2019, Duracion = "3:28", Letra = "[Contenido de la canción - letra original]" },

                // Enhypen
                new Cancion { Nombre = "Drunk Dazed", IdArtista = 3, Imagen = "anti_hero.jpg", AnoEstreno = 2021, Duracion = "3:13", Letra = "[Contenido de la canción - letra original]" },
                new Cancion { Nombre = "Polaroid Love", IdArtista = 3, Imagen = "shake_it_off.jpg", AnoEstreno = 2022, Duracion = "3:04", Letra = "[Contenido de la canción - letra original]" },
                new Cancion { Nombre = "Hey Tayo", IdArtista = 3, Imagen = "love_story.jpg", AnoEstreno = 2021, Duracion = "1:52", Letra = "[Contenido de la canción - letra original]" },
                new Cancion { Nombre = "Bite Me", IdArtista = 3, Imagen = "blank_space.jpg", AnoEstreno = 2023, Duracion = "2:38", Letra = "[Contenido de la canción - letra original]" },
                new Cancion { Nombre = "Let Me In", IdArtista = 3, Imagen = "22.jpg", AnoEstreno = 2020, Duracion = "3:10", Letra = "[Contenido de la canción - letra original]" },

                // Twice
                new Cancion { Nombre = "Cheer Up", IdArtista = 4, Imagen = "anti_hero.jpg", AnoEstreno = 2016, Duracion = "3:30", Letra = "[Contenido de la canción - letra original]" },
                new Cancion { Nombre = "What Is Love?", IdArtista = 4, Imagen = "shake_it_off.jpg", AnoEstreno = 2018, Duracion = "3:28", Letra = "[Contenido de la canción - letra original]" },
                new Cancion { Nombre = "Alcohol Free", IdArtista = 4, Imagen = "love_story.jpg", AnoEstreno = 2021, Duracion = "3:31", Letra = "[Contenido de la canción - letra original]" },
                new Cancion { Nombre = "TT", IdArtista = 4, Imagen = "blank_space.jpg", AnoEstreno = 2016, Duracion = "3:33", Letra = "[Contenido de la canción - letra original]" },
                new Cancion { Nombre = "Signal", IdArtista = 4, Imagen = "22.jpg", AnoEstreno = 2017, Duracion = "3:17", Letra = "[Contenido de la canción - letra original]" },

                // Black Pink
                new Cancion { Nombre = "As If It's Your Last ", IdArtista = 5, Imagen = "anti_hero.jpg", AnoEstreno = 2017, Duracion = "3:35", Letra = "[Contenido de la canción - letra original]" },
                new Cancion { Nombre = "Playing With Fire", IdArtista = 5, Imagen = "shake_it_off.jpg", AnoEstreno = 2016, Duracion = "3:19", Letra = "[Contenido de la canción - letra original]" },
                new Cancion { Nombre = "Pink Venom", IdArtista = 5, Imagen = "love_story.jpg", AnoEstreno = 2022, Duracion = "3:07", Letra = "[Contenido de la canción - letra original]" },
                new Cancion { Nombre = "Lovesick Girls", IdArtista = 5, Imagen = "blank_space.jpg", AnoEstreno = 2020, Duracion = "3:13", Letra = "[Contenido de la canción - letra original]" },
                new Cancion { Nombre = "Stay", IdArtista = 5, Imagen = "22.jpg", AnoEstreno = 2016, Duracion = "3:50", Letra = "[Contenido de la canción - letra original]" },
            
                // New Jeans 
                new Cancion { Nombre = "Ditto", IdArtista = 6, Imagen = "anti_hero.jpg", AnoEstreno = 2022, Duracion = "3:08", Letra = "[Contenido de la canción - letra original]" },
                new Cancion { Nombre = "Super Shy", IdArtista = 6, Imagen = "shake_it_off.jpg", AnoEstreno = 2023, Duracion = "2:35", Letra = "[Contenido de la canción - letra original]" },
                new Cancion { Nombre = "OMG", IdArtista = 6, Imagen = "love_story.jpg", AnoEstreno = 2023, Duracion = "3:37", Letra = "[Contenido de la canción - letra original]" },
                new Cancion { Nombre = "Hype Boy", IdArtista = 6, Imagen = "blank_space.jpg", AnoEstreno = 2023, Duracion = "3:00", Letra = "[Contenido de la canción - letra original]" },
                new Cancion { Nombre = "Zero", IdArtista = 6, Imagen = "22.jpg", AnoEstreno = 2023, Duracion = "2:36", Letra = "[Contenido de la canción - letra original]" },

                // IVE
                new Cancion { Nombre = "Love Dive", IdArtista = 7, Imagen = "anti_hero.jpg", AnoEstreno = 2022, Duracion = "2:57", Letra = "[Contenido de la canción - letra original]" },
                new Cancion { Nombre = "After Like", IdArtista = 7, Imagen = "shake_it_off.jpg", AnoEstreno = 2022, Duracion = "2:57", Letra = "[Contenido de la canción - letra original]" },
                new Cancion { Nombre = "I AM", IdArtista = 7, Imagen = "love_story.jpg", AnoEstreno = 2023, Duracion = "3:04", Letra = "[Contenido de la canción - letra original]" },
                new Cancion { Nombre = "Eleven ", IdArtista = 7, Imagen = "blank_space.jpg", AnoEstreno = 2021, Duracion = "3:00", Letra = "[Contenido de la canción - letra original]" },
                new Cancion { Nombre = "Attitude", IdArtista = 7, Imagen = "22.jpg", AnoEstreno = 2025, Duracion = "3:15", Letra = "[Contenido de la canción - letra original]" },

                // TXT
                new Cancion { Nombre = "Cat & Dog", IdArtista = 8, Imagen = "anti_hero.jpg", AnoEstreno = 2019, Duracion = "3:08", Letra = "[Contenido de la canción - letra original]" },
                new Cancion { Nombre = "Blue Hour", IdArtista = 8, Imagen = "shake_it_off.jpg", AnoEstreno = 2020, Duracion = "3:29", Letra = "[Contenido de la canción - letra original]" },
                new Cancion { Nombre = "Sugar Rush Ride", IdArtista = 8, Imagen = "love_story.jpg", AnoEstreno = 2023, Duracion = "3:07", Letra = "[Contenido de la canción - letra original]" },
                new Cancion { Nombre = "Anti Romantic ", IdArtista = 8, Imagen = "blank_space.jpg", AnoEstreno = 2021, Duracion = "3:35", Letra = "[Contenido de la canción - letra original]" },
                new Cancion { Nombre = "0X1 = LOVESONG", IdArtista = 8, Imagen = "22.jpg", AnoEstreno = 2021, Duracion = "3:22", Letra = "[Contenido de la canción - letra original]" },

                // EXO
                new Cancion { Nombre = "Love Shot", IdArtista = 9, Imagen = "anti_hero.jpg", AnoEstreno = 2018, Duracion = "3:20", Letra = "[Contenido de la canción - letra original]" },
                new Cancion { Nombre = "Growl", IdArtista = 9, Imagen = "shake_it_off.jpg", AnoEstreno = 2013, Duracion = "3:27", Letra = "[Contenido de la canción - letra original]" },
                new Cancion { Nombre = "Tempo", IdArtista = 9, Imagen = "love_story.jpg", AnoEstreno = 2018, Duracion = "3:44", Letra = "[Contenido de la canción - letra original]" },
                new Cancion { Nombre = "Ko Ko Bop ", IdArtista = 9, Imagen = "blank_space.jpg", AnoEstreno = 2017, Duracion = "3:10", Letra = "[Contenido de la canción - letra original]" },
                new Cancion { Nombre = "Call Me Baby", IdArtista = 9, Imagen = "22.jpg", AnoEstreno = 2015, Duracion = "3:31", Letra = "[Contenido de la canción - letra original]" },

                // The Rose
                new Cancion { Nombre = "California", IdArtista = 10, Imagen = "anti_hero.jpg", AnoEstreno = 2019, Duracion = "2:51", Letra = "[Contenido de la canción - letra original]" },
                new Cancion { Nombre = "Sorry", IdArtista = 10, Imagen = "shake_it_off.jpg", AnoEstreno = 2018, Duracion = "3:35", Letra = "[Contenido de la canción - letra original]" },
                new Cancion { Nombre = "Red", IdArtista = 10, Imagen = "love_story.jpg", AnoEstreno = 2019, Duracion = "3:56", Letra = "[Contenido de la canción - letra original]" },
                new Cancion { Nombre = "She's In The Rain", IdArtista = 10, Imagen = "blank_space.jpg", AnoEstreno = 2018, Duracion = "3:57", Letra = "[Contenido de la canción - letra original]" },
                new Cancion { Nombre = "Insomnia", IdArtista = 10, Imagen = "22.jpg", AnoEstreno = 2018, Duracion = "4:02", Letra = "[Contenido de la canción - letra original]" }
            };

            foreach (var cancion in canciones)
            {
                await _database.InsertAsync(cancion);
            }
        }
    }
}