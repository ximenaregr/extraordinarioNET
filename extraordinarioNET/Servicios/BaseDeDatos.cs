using System.ComponentModel.DataAnnotations.Schema;
using SQLite;
using System.Threading.Tasks;
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
                new Artista { Nombre = "BTS", Foto = "bts.jpeg", Ranking = 1, UltimoAlbum = "Proof"},
                new Artista { Nombre = "Stray Kids", Foto = "sk.jpeg", Ranking = 2, UltimoAlbum = "Mixtape:Dominate" },
                new Artista { Nombre = "Enhypen", Foto = "enhypen.jpeg", Ranking = 3, UltimoAlbum = "Desire : Unleash" },
                new Artista { Nombre = "Twice", Foto = "twice.jpeg", Ranking = 4, UltimoAlbum = "This is For" },
                new Artista { Nombre = "Black Pink", Foto = "bp.jpeg", Ranking = 5, UltimoAlbum = "Born Pink" },
                new Artista { Nombre = "New Jeans", Foto = "nj.jpeg", Ranking = 6, UltimoAlbum = "NJWMX"},
                new Artista { Nombre = "Ive", Foto = "ive.jpeg", Ranking = 7, UltimoAlbum = "I've Ive" },
                new Artista { Nombre = "TXT", Foto = "txt.jpeg", Ranking = 8, UltimoAlbum = "Freefall" },
                new Artista { Nombre = "EXO", Foto = "exo.jpeg", Ranking = 9, UltimoAlbum = "Exist"},
                new Artista { Nombre = "The rose", Foto = "tr.jpeg", Ranking = 10, UltimoAlbum = "WRLD" }
            };

            foreach (var artista in artistas)
            {
                await _database.InsertAsync(artista);
            }

            var canciones = new List<Cancion>
            {
                // BTS
                new Cancion { Nombre = "The Truth Untold", IdArtista = 1, Imagen = "ttu.jpg", AnoEstreno = 2018, Duracion = "4:02", Letra = "Letra" },
                new Cancion { Nombre = "Film Out ", IdArtista = 1, Imagen = "fo.jpg", AnoEstreno = 2021, Duracion = "3:35", Letra = "Letra" },
                new Cancion { Nombre = "ON ", IdArtista = 1, Imagen = "on.jpg", AnoEstreno = 2020, Duracion = "4:07", Letra = "Letra" },
                new Cancion { Nombre = "I NEED U", IdArtista = 1, Imagen = "inu.jpg", AnoEstreno = 2015, Duracion = "3:30", Letra = "Letra" },
                new Cancion { Nombre = "Butterfly", IdArtista = 1, Imagen = "bf.jpg", AnoEstreno = 2015, Duracion = "3:59", Letra = "Letra" },

                // Stray Kids
                new Cancion { Nombre = "God's Menu", IdArtista = 2, Imagen = "gsm.jpg", AnoEstreno = 2020, Duracion = "2:48", Letra = "Letra" },
                new Cancion { Nombre = "Maniac ", IdArtista = 2, Imagen = "mnc.jpg", AnoEstreno = 2022, Duracion = "3:03", Letra = "Letra" },
                new Cancion { Nombre = "Chk Chk Boom", IdArtista = 2, Imagen = "ccb.jpg", AnoEstreno = 2024, Duracion = "2:28", Letra = "Letra" },
                new Cancion { Nombre = "Thunderous", IdArtista = 2, Imagen = "thr.jpg", AnoEstreno = 2021, Duracion = "3:03", Letra = "Letra" },
                new Cancion { Nombre = "Miroh", IdArtista = 2, Imagen = "mrh.jpg", AnoEstreno = 2019, Duracion = "3:28", Letra = "Letra" },

                // Enhypen
                new Cancion { Nombre = "Drunk Dazed", IdArtista = 3, Imagen = "dd.jpg", AnoEstreno = 2021, Duracion = "3:13", Letra = "Letra" },
                new Cancion { Nombre = "Polaroid Love", IdArtista = 3, Imagen = "pl.jpg", AnoEstreno = 2022, Duracion = "3:04", Letra = "Letra" },
                new Cancion { Nombre = "Hey Tayo", IdArtista = 3, Imagen = "ht.jpg", AnoEstreno = 2021, Duracion = "1:52", Letra = "Letra" },
                new Cancion { Nombre = "Bite Me", IdArtista = 3, Imagen = "bm.jpg", AnoEstreno = 2023, Duracion = "2:38", Letra = "Letra" },
                new Cancion { Nombre = "Let Me In", IdArtista = 3, Imagen = "lmi.jpg", AnoEstreno = 2020, Duracion = "3:10", Letra = "Letra" },

                // Twice
                new Cancion { Nombre = "Cheer Up", IdArtista = 4, Imagen = "cu.jpg", AnoEstreno = 2016, Duracion = "3:30", Letra = "Letra" },
                new Cancion { Nombre = "What Is Love?", IdArtista = 4, Imagen = "wil.jpg", AnoEstreno = 2018, Duracion = "3:28", Letra = "Letra" },
                new Cancion { Nombre = "Alcohol Free", IdArtista = 4, Imagen = "af.jpg", AnoEstreno = 2021, Duracion = "3:31", Letra = "Letra" },
                new Cancion { Nombre = "TT", IdArtista = 4, Imagen = "tt.jpg", AnoEstreno = 2016, Duracion = "3:33", Letra = "Letra" },
                new Cancion { Nombre = "Signal", IdArtista = 4, Imagen = "sgn.jpg", AnoEstreno = 2017, Duracion = "3:17", Letra = "Letra" },

                // Black Pink
                new Cancion { Nombre = "As If It's Your Last ", IdArtista = 5, Imagen = "aiiyl.jpg", AnoEstreno = 2017, Duracion = "3:35", Letra = "Letra" },
                new Cancion { Nombre = "Playing With Fire", IdArtista = 5, Imagen = "pwf.jpg", AnoEstreno = 2016, Duracion = "3:19", Letra = "Letra" },
                new Cancion { Nombre = "Pink Venom", IdArtista = 5, Imagen = "pv.jpg", AnoEstreno = 2022, Duracion = "3:07", Letra = "Letra" },
                new Cancion { Nombre = "Lovesick Girls", IdArtista = 5, Imagen = "lg.jpg", AnoEstreno = 2020, Duracion = "3:13", Letra = "Letra" },
                new Cancion { Nombre = "Stay", IdArtista = 5, Imagen = "sty.jpg", AnoEstreno = 2016, Duracion = "3:50", Letra = "Letra" },
            
                // New Jeans 
                new Cancion { Nombre = "Ditto", IdArtista = 6, Imagen = "dto.jpg", AnoEstreno = 2022, Duracion = "3:08", Letra = "Letra" },
                new Cancion { Nombre = "Super Shy", IdArtista = 6, Imagen = "ss.jpg", AnoEstreno = 2023, Duracion = "2:35", Letra = "Letra" },
                new Cancion { Nombre = "OMG", IdArtista = 6, Imagen = "omg.jpg", AnoEstreno = 2023, Duracion = "3:37", Letra = "Letra" },
                new Cancion { Nombre = "Hype Boy", IdArtista = 6, Imagen = "hb.jpg", AnoEstreno = 2023, Duracion = "3:00", Letra = "Letra" },
                new Cancion { Nombre = "Zero", IdArtista = 6, Imagen = "zr.jpg", AnoEstreno = 2023, Duracion = "2:36", Letra = "Letra" },

                // IVE
                new Cancion { Nombre = "Love Dive", IdArtista = 7, Imagen = "ld.jpg", AnoEstreno = 2022, Duracion = "2:57", Letra = "Letra" },
                new Cancion { Nombre = "After Like", IdArtista = 7, Imagen = "al.jpg", AnoEstreno = 2022, Duracion = "2:57", Letra = "Letra" },
                new Cancion { Nombre = "I AM", IdArtista = 7, Imagen = "ia.jpg", AnoEstreno = 2023, Duracion = "3:04", Letra = "Letra" },
                new Cancion { Nombre = "Eleven ", IdArtista = 7, Imagen = "eln.jpg", AnoEstreno = 2021, Duracion = "3:00", Letra = "Letra" },
                new Cancion { Nombre = "Attitude", IdArtista = 7, Imagen = "att.jpg", AnoEstreno = 2025, Duracion = "3:15", Letra = "Letra" },

                // TXT
                new Cancion { Nombre = "Cat & Dog", IdArtista = 8, Imagen = "cyd.jpg", AnoEstreno = 2019, Duracion = "3:08", Letra = "Letra" },
                new Cancion { Nombre = "Blue Hour", IdArtista = 8, Imagen = "bh.jpg", AnoEstreno = 2020, Duracion = "3:29", Letra = "Letra" },
                new Cancion { Nombre = "Sugar Rush Ride", IdArtista = 8, Imagen = "srr.jpg", AnoEstreno = 2023, Duracion = "3:07", Letra = "Letra" },
                new Cancion { Nombre = "Anti Romantic ", IdArtista = 8, Imagen = "ar.jpg", AnoEstreno = 2021, Duracion = "3:35", Letra = "Letra" },
                new Cancion { Nombre = "0X1 = LOVESONG", IdArtista = 8, Imagen = "lvs.jpg", AnoEstreno = 2021, Duracion = "3:22", Letra = "Letra" },

                // EXO
                new Cancion { Nombre = "Love Shot", IdArtista = 9, Imagen = "ls.jpg", AnoEstreno = 2018, Duracion = "3:20", Letra = "Letra" },
                new Cancion { Nombre = "Growl", IdArtista = 9, Imagen = "gwl.jpg", AnoEstreno = 2013, Duracion = "3:27", Letra = "Letra" },
                new Cancion { Nombre = "Tempo", IdArtista = 9, Imagen = "tmp.jpg", AnoEstreno = 2018, Duracion = "3:44", Letra = "Letra" },
                new Cancion { Nombre = "Ko Ko Bop ", IdArtista = 9, Imagen = "kkb.jpg", AnoEstreno = 2017, Duracion = "3:10", Letra = "Letra" },
                new Cancion { Nombre = "Call Me Baby", IdArtista = 9, Imagen = "cmb.jpg", AnoEstreno = 2015, Duracion = "3:31", Letra = "Letra" },

                // The Rose
                new Cancion { Nombre = "California", IdArtista = 10, Imagen = "cal.jpg", AnoEstreno = 2019, Duracion = "2:51", Letra = "Letra" },
                new Cancion { Nombre = "Sorry", IdArtista = 10, Imagen = "srry.jpg", AnoEstreno = 2018, Duracion = "3:35", Letra = "Letra" },
                new Cancion { Nombre = "Red", IdArtista = 10, Imagen = "red.jpg", AnoEstreno = 2019, Duracion = "3:56", Letra = "Letra" },
                new Cancion { Nombre = "She's In The Rain", IdArtista = 10, Imagen = "sitr.jpg", AnoEstreno = 2018, Duracion = "3:57", Letra = "Letra" },
                new Cancion { Nombre = "Insomnia", IdArtista = 10, Imagen = "ism.jpg", AnoEstreno = 2018, Duracion = "4:02", Letra = "Letra" }
            };

            foreach (var cancion in canciones)
            {
                await _database.InsertAsync(cancion);
            }
        }
    }
}