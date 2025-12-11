using System;
using System.Collections;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Linq;
using FuzzySharp;
namespace managementProj;

public partial class simplePanel : UserControl{
  private string tableName;
  private string collName;
  private int selectedIndex;
  private ArrayList items;

  public simplePanel(string tableName, MainWindow parent){
    this.tableName = tableName;
    InitializeComponent();
    parent.SelectionEvent += HandlePanelSelection;

    foreach(ArrayList box in new DatabaseManager().getTableFields(tableName)){

      options.Children.Add(new Input(box).attachHandler(this).getInput());
    }
    refreshItems();
    populateList();
  }

  private void handleUpdate(){
    ArrayList updateAL = new ArrayList();
    foreach (Input i in options.Children.OfType<Input>()){
      updateAL.Add(new String []{i.getName(), i.getValue()});
    }
    new DatabaseManager().update(tableName, selectedIndex, updateAL);

  }


  private void handleADD(){
    ArrayList insertAL = new ArrayList();

    foreach (Input i in options.Children.OfType<Input>()){
      insertAL.Add(new String []{i.getName(), i.getValue()});
    }
    new DatabaseManager().insert(tableName, insertAL);

  }


  private void populateBoxes(int id){
    List<string> result = new DatabaseManager().selectAll(tableName, id);
    for(int i = 0; i < result.Count-1; i++){
      options.Children.OfType<Input>().ToList()[i].setValue(result[i+1]);
    }
  }

  private void populateList(){
    populateList("");
  }

  private void refreshItems(){
    items = new DatabaseManager().selectAll(tableName);
  }


  private void populateList(string filter){
    List.Items.Clear();

    foreach (ArrayList e in items){
      if (Fuzz.PartialRatio(e[1].ToString().ToLower(), filter.ToLower()) >=50 || filter.Length == 0){
        var listBoxItem = new ListBoxItem(){
            Content = e[1],
            Tag = e[0]
        };

        List.Items.Add(listBoxItem);
      }
    }
  }

}
