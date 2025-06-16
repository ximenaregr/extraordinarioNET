using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SQLite;

namespace extraordinarioNET.Model
{
    [SQLite.Table("Canciones")]
    public class Cancion
    {
        [SQLite.PrimaryKey,  AutoIncrement]
        public int Id {  get; set; }

        [SQLite.MaxLength(100)]

        public string Nombre {  get; set; }

        public int IdArtista {  get; set; }
        public string Imagen {  get; set; }
        public int AnoEstreno {  get; set; }
        public string Duracion { get; set; }
        public string Letra {  get; set; }


    }
}
