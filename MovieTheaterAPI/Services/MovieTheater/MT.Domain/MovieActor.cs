using System.ComponentModel.DataAnnotations.Schema;
using MT.Shared.Constant.Database;

namespace MT.Domain
{
    [Table(nameof(MovieActor), Schema = DbSchema.MovieTheater)]
    public class MovieActor
    {
        [ForeignKey(nameof(Movie))]
        public int MovieId { get; set; }
        public Movie Movie { get; set; }

        [ForeignKey(nameof(Actor))]
        public int ActorId { get; set; }
        public Actor Actor { get; set; }
    }
}
