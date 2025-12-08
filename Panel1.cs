using System;
using System.Collections;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace managementProj;

public partial class Panel1 : UserControl{
  public Panel1(){
    InitializeComponent();
    populateList();

  }
  
  public void ClickHandler(object sender, RoutedEventArgs args)
    {
      Button clickedButton = sender as Button;
      
      if (clickedButton.Content == "Update"){
        new DatabaseManager().update("Kontinent", int.Parse(Bez.Tag.ToString()),  new ArrayList{
            new string[] { "kontinentbezeichnung", Bez.Text}
          } 
        );

      } else if(clickedButton.Content == "Hinzufügen"){
        new DatabaseManager().insert("kontinent", new ArrayList{new String[]{"kontinentbezeichnung",Bez.Text}});
        Bez.Text = "";

      } else if(clickedButton.Content == "Delete"){
        new DatabaseManager().delete("kontinent", int.Parse(Bez.Tag.ToString()));


      }
      populateList();
    }


  private void listSelectionChanged(object sender, SelectionChangedEventArgs e)
    { 
     if (List.SelectedItem == null){
      DelButton.IsEnabled = false;
      ContectDepButton.Content = "Hinzufügen";
      Bez.Text = "";
      return;

     }
      DelButton.IsEnabled = true;
      ContectDepButton.Content = "Update";
      
      ListBoxItem selectedItem = List.SelectedItem as ListBoxItem;

      Bez.Tag = int.Parse(selectedItem.Tag.ToString());
      populateBoxes(int.Parse(selectedItem.Tag.ToString()));
    }


  private void populateBoxes(int id){
   Bez.Text = new DatabaseManager().selectAll("Kontinent", id)[1];
  }


  private void populateList()
  {
    var items = new DatabaseManager().selectAll("Kontinent");
    List.Items.Clear();

    foreach (ArrayList e in new DatabaseManager().selectAll("Kontinent"))
    {
        var listBoxItem = new ListBoxItem()
        {
            Content = e[1],
            Tag = e[0]
        };

        List.Items.Add(listBoxItem);
    }
  }
}
