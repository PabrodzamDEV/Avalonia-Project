using Avalonia.Controls;
using Avalonia.Interactivity;

namespace AvaloniaPeliculas.Views;

public partial class InfoDialog : Window
{
    public InfoDialog(string caption, string message)
    {
        InitializeComponent();
        
        Title = caption;
        
        var captionTextBlock = this.FindControl<TextBlock>("CaptionTextBlock");
        captionTextBlock.Text = message;
    }

    private void OnAceptarClicked(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}