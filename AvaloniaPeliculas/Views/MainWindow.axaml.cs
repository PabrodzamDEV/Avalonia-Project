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

    private Button PreviousArrowBtn;
    private Button NextArrowBtn;

    private static TextBox TxtYear;

    private static TextBox TxtScore;

    private static TextBox TxtTitle;

    private static TextBox TxtDirect;

    private static TextBox TxtGenre;
    
    private static Image ImgAdultCont;
    
    private static PathIcon IconWatch;
    
    private static Image cover;

    private static TextBox TxtInd;

    private static TextBox TxtCnt;
    

    
    public MainWindow()
    {
        InitializeComponent();
        ctrl = new MoviesControler();
        PreviousArrowBtn = this.FindControl<Button>("PreviousArrowButton");
        NextArrowBtn = this.FindControl<Button>("NextArrowButton");
        TxtYear = this.FindControl<TextBox>("TxtAnio");
        TxtScore = this.FindControl<TextBox>("TxtPuntuacion");
        TxtTitle = this.FindControl<TextBox>("TxtTitulo");
        TxtDirect = this.FindControl<TextBox>("TxtDirector");
        TxtGenre = this.FindControl<TextBox>("TxtGenero");
        IconWatch = this.FindControl<PathIcon>("IconWatched");
        ImgAdultCont = this.FindControl<Image>("ImgAdultContent");
        cover = this.FindControl<Image>("ImgCover");
        cover.Source = new Bitmap("..\\..\\..\\assets\\default_cover.png");
        TxtInd = this.FindControl<TextBox>("TxtCurrentIndex");
        TxtCnt = this.FindControl<TextBox>("TxtMovieCount");
        UpdateControlsVisibility();
        
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
        TxtInd.Text = (ctrl.GetCurrentIndex() + 1).ToString();
        TxtCnt.Text = ctrl.GetListCount().ToString();
    }
    
    public static string GetDocumentsFolderPath()
    {
        try
        {
            // Get the path to the Documents folder
            string documentsFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            // Return the path
            return documentsFolderPath;
        }
        catch (Exception ex)
        {
            LaunchErrorDialog("Error getting Documents folder path", ex.Message);
            return null;
        }
    }

    private async void OnLoadFromFileDoubleTapped(object? sender, TappedEventArgs e)
    {
        // Get top level from the current control. Alternatively, you can use Window reference instead.
        var topLevel = TopLevel.GetTopLevel(this);
        // Get the documents folder from any computer
        var documentsFolderLocation = await StorageProvider.TryGetFolderFromPathAsync(GetDocumentsFolderPath());
        // Start async operation to open the dialog.
        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            SuggestedStartLocation = documentsFolderLocation,
            Title = "Open File to Load",
            AllowMultiple = false
        });

        if (files.Count >= 1)
        {
            // Open reading stream from the first file.
            await using var stream = await files[0].OpenReadAsync();
            
            ctrl.ClearList();
            ctrl.SetMoviesList(ctrl.LoadMoviesFromFile(stream));
            if (ctrl.GetListCount() != 0)
            {
                ctrl.SetCurrentIndex(0);
                showCurrentMovie(ctrl.GetMovieFromList(0));
            }
        }
        UpdateControlsVisibility();
    }

    private void UpdateControlsVisibility()
    {
        PreviousArrowBtn.IsEnabled = true;
        NextArrowBtn.IsEnabled = true;
        if (ctrl.GetListCount() == 0 || ctrl.GetListCount() == 1)
        {
            PreviousArrowBtn.IsEnabled = false;
            NextArrowBtn.IsEnabled = false;
        }
        else if (ctrl.GetCurrentIndex() == 0 || ctrl.GetCurrentIndex() == -1)
        {
            PreviousArrowBtn.IsEnabled = false;
        }
        else if (ctrl.GetCurrentIndex() == ctrl.GetListCount() - 1)
        {
            NextArrowBtn.IsEnabled = false;
        }
    }
    private void OnSaveMoviesToFileDoubleTapped(object? sender, TappedEventArgs e)
    {
        string path = GetDocumentsFolderPath() + "\\databank.data";
        ctrl.WriteMoviesToFile(path, ctrl.GetMoviesList());
    }

    private void OnPreviousArrowButtonClicked(object? sender, RoutedEventArgs e)
    {
        ctrl.SetCurrentIndex(ctrl.GetCurrentIndex() - 1);
        showCurrentMovie(ctrl.GetMovieFromList(ctrl.GetCurrentIndex()));
        UpdateControlsVisibility();
    }

    private void OnNextArrowButtonClicked(object? sender, RoutedEventArgs e)
    {
        ctrl.SetCurrentIndex(ctrl.GetCurrentIndex() + 1);
        showCurrentMovie(ctrl.GetMovieFromList(ctrl.GetCurrentIndex()));
        UpdateControlsVisibility();
    }
}