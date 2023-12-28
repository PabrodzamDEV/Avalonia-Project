using System;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using AvaloniaPeliculas.Views;

namespace AvaloniaPeliculas;

public partial class MainWindow : Window
{
    
    public MainWindow()
    {
        InitializeComponent();
        
        // Load an image from a file
        var imageFilePath = "C:\\Users\\pablo\\RiderProjects\\AvaloniaPeliculas\\AvaloniaPeliculas\\assets\\cover_power_of_dog.jpg";
        var image = new Bitmap(imageFilePath);

        // Do something with the bitmap, e.g., set it as the source of an Image control
        var imageControl = this.FindControl<Image>("Aa");
        imageControl.Source = image;
        
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
        LaunchErrorDialog("Error", "Esto es un ejemplo de mensaje de error");
    }
    
}