using System;
using System.Globalization;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using AvaloniaPeliculas.Controller;
using AvaloniaPeliculas.Model;
using AvaloniaPeliculas.Views;

namespace AvaloniaPeliculas;

public partial class MainWindow : Window
{

    private static MoviesControler ctrl;

    private static TextBox TxtYear;

    private static TextBox TxtScore;

    private static TextBox TxtTitle;

    private static TextBox TxtDirect;

    private static TextBox TxtGenre;
    
    private static Image ImgAdultCont;
    
    private static PathIcon IconWatch;
    
    private static Image cover;
    

    
    public MainWindow()
    {
        InitializeComponent();
        ctrl = new MoviesControler();
        TxtYear = this.FindControl<TextBox>("TxtAnio");
        TxtScore = this.FindControl<TextBox>("TxtPuntuacion");
        TxtTitle = this.FindControl<TextBox>("TxtTitulo");
        TxtDirect = this.FindControl<TextBox>("TxtDirector");
        TxtGenre = this.FindControl<TextBox>("TxtGenero");
        IconWatch = this.FindControl<PathIcon>("IconWatched");
        ImgAdultCont = this.FindControl<Image>("ImgAdultContent");
        cover = this.FindControl<Image>("ImgCover");
        cover.Source = new Bitmap("..\\..\\..\\assets\\default_cover.png");
        
    }

    public static void LaunchErrorDialog(string caption, string message)
    {
        ErrorDialog errorDialog = new ErrorDialog(caption, message);
        errorDialog.Show();
    }
    
    public static void LaunchInfoDialog(string caption, string message)
    {
        new InfoDialog(caption, message).Show();
    }
    
    public static void DisplayImage(byte[] array, Image image)
    {
        try
        {
            // Create a MemoryStream from the byte array
            using (MemoryStream stream = new MemoryStream(array))
            {
                // Create a Bitmap from the MemoryStream
                Bitmap bitmap = new Bitmap(stream);
                image.Source = bitmap;
            }
        }
        catch (Exception ex)
        {
            LaunchErrorDialog("Error displaying image", ex.Message);
        }
    }

    private void OnPanelButtonCliked(object? sender, RoutedEventArgs e)
    {
        var splitView = this.FindControl<SplitView>("SplitViewPanel");
        splitView.IsPaneOpen = !splitView.IsPaneOpen;
    }

    private async void OnLoadFromFileDoubleTapped(object? sender, TappedEventArgs e)
    {
        // Get top level from the current control. Alternatively, you can use Window reference instead.
        var topLevel = TopLevel.GetTopLevel(this);

        // Start async operation to open the dialog.
        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Open File to Load",
            AllowMultiple = false
        });

        if (files.Count >= 1)
        {
            // Open reading stream from the first file.
            await using var stream = await files[0].OpenReadAsync();
            // Read the stream directly as bytes
            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            var fileBytes = memoryStream.ToArray();
            
            
            
            
        }
    }

    private void OnAddMovieDoubleTapped(object? sender, TappedEventArgs e)
    {
        new MovieCreationWindow().Show();
    }

    public static void showCurrentMovie(Movie movie)
    {
        TxtYear.Text = movie.ReleaseDate.ToString();
        TxtScore.Text = movie.ImdbScore.ToString(CultureInfo.CurrentCulture);
        TxtTitle.Text = movie.Title;
        TxtDirect.Text = movie.Director;
        TxtGenre.Text = movie.Genre;
        IconWatch.Data = movie.Watched ? (StreamGeometry)Application.Current.FindResource("EyeShowRegular") : (StreamGeometry)Application.Current.FindResource("EyeHideRegular");
        ImgAdultCont.IsVisible = movie.AdultContent;
        DisplayImage(movie.Cover, cover);
        ctrl.SetCurrentIndex(ctrl.GetMoviesList().IndexOf(movie));
    }
    
}