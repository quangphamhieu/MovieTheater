using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MT.Shared.Constant.Database;

namespace MT.Domain
{
    [Table(nameof(Actor), Schema = DbSchema.MovieTheater)]
    public class Actor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        public ICollection<MovieActor> MovieActors { get; set; }
    }
}
