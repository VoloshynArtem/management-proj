using Avalonia.Controls;
using System;
using Avalonia.Interactivity;
using Avalonia.Layout;
namespace simpleRoute;

public partial class MainWindow : Window
{
  private UserControl[] Panels = [new Panel1(), new Panel2(), new Panel3(), new Panel4()];

    public MainWindow()
    {
        InitializeComponent();
    }



    public void ClickHandler(object sender, RoutedEventArgs args)
    {
      Button clickedButton = sender as Button;

      Console.WriteLine(int.Parse(clickedButton.Tag.ToString()));
      Con.Content = Panels[int.Parse(clickedButton.Tag.ToString())]; 
    }
}
