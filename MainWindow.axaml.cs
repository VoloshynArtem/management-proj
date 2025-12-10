using Avalonia.Controls;
using System;
using Avalonia.Interactivity;
using Avalonia.Layout;

using System.Collections;
namespace managementProj;


public partial class MainWindow : Window{
  public delegate void EventHandler(object sender, EventArgs e);
  public event EventHandler SelectionEvent;

  private UserControl[] Panels;

  public MainWindow(){
    Panels = [new simplePanel("kontinent", this), new simplePanel("gehege", this), new simplePanel("tierart", this), new simplePanel("tiere", this)];


      InitializeComponent();

      EnvReader.Load(".env");
      new DatabaseManager();
      
      Con.Content = Panels [0];
  }



    public void ClickHandler(object sender, RoutedEventArgs args){
      Button clickedButton = sender as Button;
      Con.Content = Panels[int.Parse(clickedButton.Tag.ToString())];
      SelectionEvent.Invoke(clickedButton, EventArgs.Empty);
    }
}
