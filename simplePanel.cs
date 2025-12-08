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

  public simplePanel (string tableName, string[] inputs):this(tableName, inputs, new string[,]{}){
  }

  public simplePanel(string tableName, string[] inputs, string[,] references){
    this.tableName = tableName;
    this.collName = collName;
    InitializeComponent();
    
    foreach(string s in inputs ){
      options.Children.Add(new TextBlock(){Text = s});
      options.Children.Add(new TextBox(){
          Text = "", 
          Name = s
      });
    }
    populateList();
    
  }
  


  public void initCombobox(string [,] references){

    for (int i = 0; i<references.Length && references.Length >0; i+=2){
       options.Children.Add(new TextBox(){
           Text=references[0,i]
        }); 

       foreach (ArrayList e in new DatabaseManager().selectAll(references[0,i])){
        Console.WriteLine(e[1]);

       }    
    }
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
        foreach (var textBox in options.Children.OfType<TextBox>())
        {
          updateAL.Add(new string[]{textBox.Name, textBox.Text});
        }
        new DatabaseManager().update(tableName, selectedIndex, updateAL);
  }


  private void handleADD(){
    ArrayList insertAL = new ArrayList();
    foreach (var textBox in options.Children.OfType<TextBox>())
    {
      if(textBox.Text.Length <1) return;
      insertAL.Add(new string[]{textBox.Name, textBox.Text});
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
      Console.WriteLine(result[+1]);
      options.Children.OfType<TextBox>().ToList()[i].Text = result[i+1];
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
