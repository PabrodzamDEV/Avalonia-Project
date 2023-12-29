using System;
using AvaloniaPeliculas.Exceptions;

namespace AvaloniaPeliculas.Model;

public class Movie
{
    public int ReleaseDate { get; set;} //The movie's release date
    public float ImdbScore { get; set; } // Movie's score in IMDb (0-10)
    public bool AdultContent { get; set; } // Determines if the movie contains adult content
    public bool Watched { get; set; } // Determines if the movie has been watched
    public string Title { get; set; } // Title of the movie
    public string Director { get; set; } // Director(s) of the movie
    public string Genre { get; set; } // Genre of the movie
    public byte[] Cover { get; set; } // Cover image of the movie

    public Movie(int ReleaseDate, float ImdbScore, bool AdultContent, bool Watched, string Title, string Director, string Genre, byte[] Cover)
    {
        this.ReleaseDate = ReleaseDate >= 1895 && ReleaseDate <= 2050 ? ReleaseDate : throw new InvalidReleaseDateException(nameof(ReleaseDate));
        this.ImdbScore = ImdbScore >= 0 && ImdbScore <= 10 ? ImdbScore : throw new InvalidScoreException(nameof(ImdbScore));
        this.AdultContent = AdultContent;
        this.Watched = Watched;
        this.Title = Title ?? throw new ArgumentNullException(nameof(Title));
        this.Director = Director ?? throw new ArgumentNullException(nameof(Director));
        this.Genre = Genre ?? throw new ArgumentNullException(nameof(Genre));
        this.Cover = Cover ?? throw new ArgumentNullException(nameof(Cover));
    }
}