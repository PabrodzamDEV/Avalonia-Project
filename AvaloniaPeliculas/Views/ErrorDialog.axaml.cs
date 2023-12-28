using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;


namespace AvaloniaPeliculas.Views;

public partial class ErrorDialog : Window
{
    private string caption;
    private string message;
    public ErrorDialog(string caption, string message)
    {
        this.caption = caption;
        this.message = message;
        
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