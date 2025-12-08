using System;
using System.Collections;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace managementProj;

public partial class simplePanel : UserControl{
  private string tableName;
  private string collName;
  
  public simplePanel(string tableName, string collName){
    this.tableName = tableName;
    this.collName = collName;
    InitializeComponent();
    Heading.Text=collName + ":";
    populateList();
    
  }
  
  public void ClickHandler(object sender, RoutedEventArgs args)
    {
      Button clickedButton = sender as Button;
      
      if (clickedButton.Content == "Update"){
        new DatabaseManager().update(tableName, int.Parse(Bez.Tag.ToString()),  new ArrayList{
            new string[] { collName, Bez.Text}
          } 
        );

      } else if(clickedButton.Content == "Hinzufügen"){
        if(Bez.Text.Length > 0)
          new DatabaseManager().insert(tableName, new ArrayList{new String[]{collName,Bez.Text}});
        Bez.Text = "";

      } else if(clickedButton.Content == "Delete"){
        new DatabaseManager().delete(tableName, int.Parse(Bez.Tag.ToString()));


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
   Bez.Text = new DatabaseManager().selectAll(tableName, id)[1];
  }


  private void populateList()
  {
    var items = new DatabaseManager().selectAll(tableName);
    List.Items.Clear();

    foreach (ArrayList e in new DatabaseManager().selectAll(tableName))
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
