﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using AvaloniaPeliculas.Controller;
using AvaloniaPeliculas.Exceptions;
using AvaloniaPeliculas.Model;

namespace AvaloniaPeliculas.Views;

public partial class MovieCreationWindow : Window
{

    private MoviesControler ctrl;
    private Movie movieEdited = null;
    private bool isEditMode = false;
    private ComboBox comboBox;
    private Image newCover;
    private byte[] fileBytes;
    private int releaseDate;
    private float imdbScore;
    private bool adultContent;
    private bool watched;
    private string title;
    private string director;
    private string genre;
    private PathIcon icon;

    public MovieCreationWindow(Movie movieToEdit)
    {
        MainWindow.DisableWindowInteractivity();
        isEditMode = true;
        InitializeComponent();
        PrepareWindow();
        LoadMovieInfo(movieToEdit);
        comboBox.SelectedItem = movieToEdit.Genre;
        
    }
    public MovieCreationWindow()
    {
        MainWindow.DisableWindowInteractivity();
        InitializeComponent();

        PrepareWindow();
    }

    private void PrepareWindow()
    {
        ctrl = new MoviesControler();

        comboBox = this.FindControl<ComboBox>("CbGenero");

        newCover = this.FindControl<Image>("NewCover");

        icon = this.FindControl<PathIcon>("WindowIcon");

        List<string> items = new List<string>
        {
            "Acción",
            "Animación",
            "Aventura",
            "Bélico",
            "Biografía",
            "Ciencia Ficción",
            "Comedia",
            "Crimen",
            "Deporte",
            "Documental",
            "Drama",
            "Familiar",
            "Fantasía",
            "Historia",
            "Misterio",
            "Musical",
            "Romance",
            "Suspense",
            "Terror",
            "Western"
        };
        foreach (string item in items)
        {
            comboBox.Items.Add(item);
        }
    }

    private void LoadMovieInfo(Movie movieToEdit)
    {
        var releaseDateTextBox = this.FindControl<TextBox>("TxtAnio");
        var imdbScoreTextBox = this.FindControl<TextBox>("TxtPuntuacion");
        var adultContentCheckBox = this.FindControl<CheckBox>("CbAdultContent");
        var watchedCheckBox = this.FindControl<CheckBox>("CbWatched");
        var titleTextBox = this.FindControl<TextBox>("TxtTitulo");
        var directorTextBox = this.FindControl<TextBox>("TxtDirector");
        var genreTextBox = this.FindControl<TextBox>("TxtGenero");
        icon.Data = (StreamGeometry)Application.Current.FindResource("EditRegular");
        
        movieEdited = movieToEdit;
        
        releaseDateTextBox.Text = movieToEdit.ReleaseDate.ToString();
        imdbScoreTextBox.Text = movieToEdit.ImdbScore.ToString(CultureInfo.CurrentCulture);
        adultContentCheckBox.IsChecked = movieToEdit.AdultContent;
        watchedCheckBox.IsChecked = movieToEdit.Watched;
        titleTextBox.Text = movieToEdit.Title;
        directorTextBox.Text = movieToEdit.Director;
        genreTextBox.Text = movieToEdit.Genre;
        fileBytes = movieToEdit.Cover;
        MainWindow.DisplayImage(fileBytes, newCover);
    }

    private void OnGenreSelectionEvent(object? sender, SelectionChangedEventArgs e)
    {
        var txtGenre = this.FindControl<TextBox>("TxtGenero");
        txtGenre.Text = comboBox.SelectedItem.ToString();
    }

    private async void OnNewCoverTapped(object? sender, TappedEventArgs e)
    {
        // Get top level from the current control. Alternatively, you can use Window reference instead.
        var topLevel = TopLevel.GetTopLevel(this);

        // Start async operation to open the dialog.
        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Open Cover to Load",
            FileTypeFilter = new[] { FilePickerFileTypes.ImageAll },
            AllowMultiple = false
        });

        if (files.Count >= 1)
        {
            IStorageFile file = files[0];
            // Open reading stream from the first file.
            await using var stream = await file.OpenReadAsync();
            // Read the stream directly as bytes
            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            fileBytes = memoryStream.ToArray();
            MainWindow.DisplayImage(fileBytes, newCover);
        }
    }

    private void OnAddMovieClicked(object? sender, RoutedEventArgs e)
    {
        string message = "";
        string caption = "";
        try
        {
            releaseDate = int.Parse(this.FindControl<TextBox>("TxtAnio").Text);
            imdbScore = float.Parse(this.FindControl<TextBox>("TxtPuntuacion").Text);
            adultContent = this.FindControl<CheckBox>("CbAdultContent").IsChecked ?? false;
            watched = this.FindControl<CheckBox>("CbWatched").IsChecked ?? false;
            title = this.FindControl<TextBox>("TxtTitulo").Text;
            director = this.FindControl<TextBox>("TxtDirector").Text;
            genre = this.FindControl<TextBox>("TxtGenero").Text;
            Movie newMovie = new Movie(releaseDate, imdbScore, adultContent, watched, title, director, genre,
                fileBytes);
            if (isEditMode)
            {
                ctrl.EditMovie(newMovie);
                
            }
            else
            {
                ctrl.AddMovieToList(newMovie);
            }
            MainWindow.showCurrentMovie(newMovie);
            Close();
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

    protected override void OnClosing(WindowClosingEventArgs e)
    {
        MainWindow.EnableWindowInteractivity();
        base.OnClosing(e);
    }
}