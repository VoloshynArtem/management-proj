using System.Collections;
using Avalonia.Controls;
using System;
using Avalonia.Media;

namespace managementProj;

public class Input : StackPanel{

  private ComboBox cb;
  private string expectedDatatyp;

  public Input(ArrayList fields){
    expectedDatatyp = fields[1].ToString();
    this.Name = fields[0].ToString();


    if(fields[2] is DBNull){
      this.Children.Add(new TextBlock(){Text = fields[0].ToString()});
      if(fields[1].Equals("date")){
        this.Children.Add(new DatePicker());
      }else{
        this.Children.Add(new TextBox(){Text = ""});
      }
    }else{
      this.Children.Add(new TextBlock(){Text = fields[2].ToString()});

      cb = new ComboBox(){Tag = fields[2].ToString()};
      populateComboBox();
      this.Children.Add(cb);

    }

  }
  public Input getInput(){
    return this; 
  }

  private void populateComboBox(){
    cb.Items.Clear();
    foreach (ArrayList e in new DatabaseManager().selectAll(cb.Tag.ToString().ToString())){
      var listBoxItem = new ListBoxItem(){
        Content = e[1],
        Tag = e[0]
      };

      cb.Items.Add(listBoxItem);
    }
    
  }

  public void setValue(string s){
    if (this.Children[1] is TextBox textBox){
      textBox.Text = s;

    }else if (this.Children[1] is ComboBox cb){
      foreach(ListBoxItem v in cb.Items){
        if(v.Tag.ToString().Equals(s)){
          cb.SelectedItem = v;
        }
      }
    }else if(this.Children[1] is DatePicker dp){
      dp.SelectedDate = DateTime.Parse(s);


    } 
  }

  public string getValue(){
    if (this.Children[1] is TextBox textBox){
      if(!float.TryParse(textBox.Text, out var v) && expectedDatatyp == "double precision"){
          throw new FormatException();
      }
      return (textBox.Text);

    }else if (this.Children[1] is ComboBox cb){ 
      return (cb.SelectedItem as ListBoxItem)?.Tag.ToString();

    }else if(this.Children[1] is DatePicker dp){
      return dp.SelectedDate.ToString();

    }
    return "";
  }

  public string getName(){
    return this.Name;
  }

  private void clear(){
    this.Tag = null;
    foreach(var child in this.Children){
      if(child is TextBox textBox){
        textBox.Text = "";

      }else if(child is ComboBox cb){
        cb.Clear();


      }else if(child is DatePicker dp){
        dp.Clear();

      }

    }

  }
  public Input attachHandler(simplePanel parentObj){
    parentObj.ClearEvent += HandleClear;
    parentObj.RefreshEvent += HandleRefresh;
    
    if (this.Children[1] is TextBox tb){
      tb.TextChanged += HandleTextChanged;

    }
    return this;
  }

  public void HandleClear(object sender, EventArgs e){
    clear();
  }
  
  public void HandleTextChanged(object sender, EventArgs e){
    TextBox tb = sender as TextBox;
    if (expectedDatatyp == "double precision"){
      if(float.TryParse(tb.Text, out var v) || tb.Text.Length == 0){
        tb.BorderBrush = new TextBox().BorderBrush;

      }else{
        tb.BorderBrush = Brushes.Red;

      }
    }

  }


  public void HandleRefresh(object sender, EventArgs e){
    if (this.Children[1] is ComboBox){
      //TODO retain selected item 
      populateComboBox();

    }
  }


}
