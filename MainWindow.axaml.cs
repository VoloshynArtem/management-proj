using Avalonia.Controls;
using System;
using Avalonia.Interactivity;
using Avalonia.Layout;

using System.Collections;
namespace managementProj;

public partial class MainWindow : Window
{
  private UserControl[] Panels = [new simplePanel("kontinent", "Kontinentbezeichnung"), new Panel2(), new simplePanel("tierart", "TierartenBezeichnung"), new Panel4(), new Panel5()];

    public MainWindow()
    {
        InitializeComponent();

        EnvReader.Load(".env");
        new DatabaseManager();
        
        Con.Content = Panels [0];
    }



    public void ClickHandler(object sender, RoutedEventArgs args)
    {
      Button clickedButton = sender as Button;
      Console.WriteLine(int.Parse(clickedButton.Tag.ToString()));
      Con.Content = Panels[int.Parse(clickedButton.Tag.ToString())]; 
    }
}
