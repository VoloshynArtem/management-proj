using System;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace managementProj;

public partial class simplePanel : UserControl{

  public delegate void EventHandler(object sender, EventArgs e);
  public event EventHandler ClearEvent;
  public event EventHandler RefreshEvent;


  public void ClickHandler(object sender, RoutedEventArgs args){
    Button clickedButton = sender as Button;
    try{
      
      if (clickedButton.Content == "Update"){
        handleUpdate();
      } 
      else if(clickedButton.Content == "Hinzufügen"){
        handleADD();
      }
      else if(clickedButton.Content == "Delete"){
          new DatabaseManager().delete(tableName, selectedIndex);
      }
      ClearEvent.Invoke(this, EventArgs.Empty);

    }catch(DeleteReferenceException ex){
      Console.WriteLine($"Delete error for {ex.table}.id = {ex.id}"); 
    
    }catch(FormatException ex){
      Console.WriteLine("Format except");
    }
    populateList();
  }

  private void listSelectionChanged(object sender, SelectionChangedEventArgs e){ 
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
  
  public void HandlePanelSelection(object sender, EventArgs e){
    RefreshEvent.Invoke(sender, e);
  }

  private void HandleSearchTextChanged(object sender, Avalonia.Controls.TextChangedEventArgs e){
    var tB = sender as TextBox;
    populateList(tB.Text);
  }


}
