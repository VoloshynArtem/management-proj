using System.Collections;
using Avalonia.Controls;
using System;

namespace managementProj;

public class Input : StackPanel{

  private string expectedDatatyp;

  public Input(ArrayList fields){
    expectedDatatyp = fields[1].ToString();
    this.Name = fields[0].ToString();


    if(fields[2] is DBNull){
      this.Children.Add(new TextBlock(){Text = fields[0].ToString()});
      Console.WriteLine(fields[1]);
      if(fields[1].Equals("date")){
        this.Children.Add(new DatePicker());
      }else{
        this.Children.Add(new TextBox(){Text = ""});
      }
    }else{
      // comboBox
      this.Children.Add(new TextBlock(){Text = fields[2].ToString()});

      var cb = new ComboBox();
      foreach (ArrayList e in new DatabaseManager().selectAll(fields[2].ToString())){
        var listBoxItem = new ListBoxItem(){
            Content = e[1],
            Tag = e[0]
        };

        cb.Items.Add(listBoxItem);
      }
      this.Children.Add(cb);

    }

  }
  public Input getInput(){
    return this; 
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

//check values return if valid or throw numberformat exc 
  public string getValue(){
    if (this.Children[1] is TextBox textBox){
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

  public void clear(){
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

}
