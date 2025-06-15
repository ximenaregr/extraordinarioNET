using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using SQLite;

namespace extraordinarioNET.Model
{
    [SQLite.Table("Artistas")]
    public class Artista
    {
        [SQLite.PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [SQLite.MaxLength(100)]
        public string Nombre { get; set; }
        public string Foto { get; set; }
        public string UltimoAlbum { get; set; }
        public int Ranking { get; set; }
        

    }
}
