using Avalonia.Controls;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
namespace managementProj;
public partial class GridPanel : UserControl {

  public GridPanel(MainWindow parent){
    InitializeComponent();
    populateGrid();

    parent.SelectionEvent += HandleRefresh;
  }

  public void populateGrid(){
    grid.ItemsSource = new DatabaseManager().selectGridView();
  }
 
  private void saveCSV(){
    try{
      using (StreamWriter outputFile = new StreamWriter(".csv")){ 
        foreach(GridRow gr in new DatabaseManager().selectGridView()){
          outputFile.WriteLine(gr.stringifyCSV());

        }
      NotificationsWrapper.sendNotification("wrote to .csv file");
      }
    }catch (Exception ex){
      NotificationsWrapper.sendNotification("failed to write to csv");
      

    }
   }

  private void HandleRefresh(object sender, EventArgs e){
   //TODO change to only when needed
    if ((sender as Button).Tag.ToString() == "4" ){ 
      populateGrid();
      saveCSV();
    }
  }
  
}
