using System;
using System.Collections;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Linq;
namespace managementProj;

public partial class simplePanel : UserControl{
  private string tableName;
  private string collName;
  private int selectedIndex;


  public simplePanel(string tableName){
    this.tableName = tableName;
    this.collName = collName;
    InitializeComponent();
   
    foreach(ArrayList box in new DatabaseManager().getTableFields(tableName)){

      options.Children.Add(new Input(box).getInput());
    }


    populateList();
    
  }
  



  public void ClickHandler(object sender, RoutedEventArgs args)
    {
      Button clickedButton = sender as Button;
      
      if (clickedButton.Content == "Update"){
        handleUpdate();
      } 
      else if(clickedButton.Content == "Hinzufügen"){
        handleADD();
      }
      else if(clickedButton.Content == "Delete"){
        new DatabaseManager().delete(tableName, selectedIndex);
      }
      populateList();
    }

  private void handleUpdate(){
        ArrayList updateAL = new ArrayList();
        foreach (Input i in options.Children.OfType<Input>())
        {
          updateAL.Add(new String []{i.getName(), i.getValue()});
          Console.WriteLine(i.getName() + "|" + i.getValue());
        }
        new DatabaseManager().update(tableName, selectedIndex, updateAL);

  }


  private void handleADD(){
    ArrayList insertAL = new ArrayList();

    foreach (Input i in options.Children.OfType<Input>())
    {
      insertAL.Add(new String []{i.getName(), i.getValue()});
      i.clear(); //TODO do this as an event after inserting into db
    }
    new DatabaseManager().insert(tableName, insertAL);

    


  }
  private void listSelectionChanged(object sender, SelectionChangedEventArgs e)
    { 
     if (List.SelectedItem == null){
      DelButton.IsEnabled = false;
      ContectDepButton.Content = "Hinzufügen";
      return;

     }
      DelButton.IsEnabled = true;
      ContectDepButton.Content = "Update";

      ListBoxItem selectedItem = List.SelectedItem as ListBoxItem;

      selectedIndex = int.Parse(selectedItem.Tag.ToString());
      populateBoxes(selectedIndex);

    }


  private void populateBoxes(int id){
    List<string> result = new DatabaseManager().selectAll(tableName, id);
    for(int i = 0; i < result.Count-1; i++){
      options.Children.OfType<Input>().ToList()[i].setValue(result[i+1]);
    }
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
