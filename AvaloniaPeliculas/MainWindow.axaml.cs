using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;

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

    private void OnPanelButtonCliked(object? sender, RoutedEventArgs e)
    {
        var splitView = this.FindControl<SplitView>("SplitViewPanel");
        splitView.IsPaneOpen = !splitView.IsPaneOpen;
    }
}