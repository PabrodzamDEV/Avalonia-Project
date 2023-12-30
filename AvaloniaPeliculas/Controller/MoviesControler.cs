using System;
using System.Collections.Generic;
using System.IO;
using AvaloniaPeliculas.Exceptions;
using AvaloniaPeliculas.Model;

namespace AvaloniaPeliculas.Controller;

public class MoviesControler
{
    private static int CurrentIndex = -1;
    private static List<Movie> MoviesList = new List<Movie>();

    public void SetCurrentIndex(int index)
    {
        CurrentIndex = index;
    }

    public int GetCurrentIndex()
    {
        return CurrentIndex;
    }

    public void ClearCurrentIndex()
    {
        CurrentIndex = -1;
    }

    public void SetMoviesList(List<Movie> list)
    {
        MoviesList = list;
    }

    public List<Movie> GetMoviesList()
    {
        return MoviesList;
    }

    public void AddMovieToList(Movie movie)
    {
        MoviesList.Add(movie);
    }

    public void RemoveMovieFromList(Movie movie)
    {
        MoviesList.Remove(movie);
    }

    public void EditMovie(Movie editedMovie)
    {
        MoviesList[CurrentIndex] = editedMovie;
    }

    public Movie GetMovieFromList(int index)
    {
        return MoviesList[index];
    }

    public void ClearList()
    {
        MoviesList.Clear();
    }

    public int GetListCount()
    {
        return MoviesList.Count;
    }

    public void WriteMoviesToFile(string filePath, List<Movie> list, bool append = false)
    {
        string message = "";
        string caption = "";
        using (var stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
        {
            using (var writer = new BinaryWriter(stream))
            {
                if (append)
                {
                    stream.Seek(0, SeekOrigin.End);
                }
                else
                {
                    stream.SetLength(0);
                }

                foreach (var item in list)
                {
                    try
                    {
                        writer.Write(item.ReleaseDate);
                        writer.Write(item.ImdbScore);
                        writer.Write(item.AdultContent);
                        writer.Write(item.Watched);
                        writer.Write(item.Title);
                        writer.Write(item.Director);
                        writer.Write(item.Genre);
                        writer.Write(item.Cover.Length);
                        writer.Write(item.Cover);
                    }
                    catch (IOException ex)
                    {
                        message = ex.Message;
                        caption = "IOError";
                        MainWindow.LaunchErrorDialog(caption, message);
                    }
                    catch (ObjectDisposedException ex)
                    {
                        message = ex.Message;
                        caption = "IOError";
                        MainWindow.LaunchErrorDialog(caption, message);
                    }
                }

                message = "Películas guardadas";
                caption = "Guardado";
                MainWindow.LaunchInfoDialog(caption, message);
            }
        }
    }

    public List<Movie> LoadMoviesFromFile(Stream stream)
    {
        string message = "";
        string caption = "";

        List<Movie> ListRead = new List<Movie>();
        using (stream)
        {
            using (var reader = new BinaryReader(stream))
            {
                while (stream.Position < stream.Length)
                {
                    try
                    {
                        int releaseDate = reader.ReadInt32();
                        float imdbScore = reader.ReadSingle();
                        bool adultContent = reader.ReadBoolean();
                        bool watched = reader.ReadBoolean();
                        string title = reader.ReadString();
                        string director = reader.ReadString();
                        string genre = reader.ReadString();
                        int coverArraySize = reader.ReadInt32();
                        byte[] cover = reader.ReadBytes(coverArraySize);

                        Movie movie = new Movie(releaseDate, imdbScore, adultContent, watched, title,
                            director, genre, cover);

                        ListRead.Add(movie);
                    }
                    catch (FormatException)
                    {
                        message = "La película cargada contiene valores no válidos";
                        caption = "Error";
                        MainWindow.LaunchErrorDialog(caption, message);
                    }
                    catch (OverflowException)
                    {
                        message = "Ha ingesado valores fuera de los intervalos válidos";
                        caption = "Error";
                        MainWindow.LaunchErrorDialog(caption, message);
                    }
                    catch (InvalidReleaseDateException)
                    {
                        message = "La fecha de publicación de la película cargada debe estar entre 1895 y 2050";
                        caption = "Error";
                        MainWindow.LaunchErrorDialog(caption, message);
                    }
                    catch (InvalidScoreException)
                    {
                        message = "La puntuación debe estar entre 0 y 10";
                        caption = "Error";
                        MainWindow.LaunchErrorDialog(caption, message);
                    }
                    catch (ArgumentNullException)
                    {
                        message = "Debe rellenar todos los campos";
                        caption = "Error";
                        MainWindow.LaunchErrorDialog(caption, message);
                    }
                    catch (IOException ex)
                    {
                        message = ex.Message;
                        caption = "IOError";
                        MainWindow.LaunchErrorDialog(caption, message);
                    }
                    catch (ObjectDisposedException ex)
                    {
                        message = ex.Message;
                        caption = "IOError";
                        MainWindow.LaunchErrorDialog(caption, message);
                    }
                    catch (Exception ex)
                    {
                        message = ex.Message;
                        caption = "Error";
                        MainWindow.LaunchErrorDialog(caption, message);
                    }
                }
            }
        }
        message = "Películas cargadas";
        caption = "Cargado";
        MainWindow.LaunchInfoDialog(caption, message);

        return ListRead;
    }
}