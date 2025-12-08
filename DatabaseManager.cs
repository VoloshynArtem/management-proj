using System.Collections;
using System.Collections.Generic;

using Npgsql;
using System;
 
namespace managementProj; 


public partial class DatabaseManager{

  public ArrayList selectAll(string table){
    using var cmd = new NpgsqlCommand($"SELECT * FROM {table}", conn);
    using var reader = cmd.ExecuteReader();

    ArrayList outerAL = new ArrayList();
    ArrayList fieldArrayList;
    while (reader.Read()){
        fieldArrayList = new ArrayList();

        for (int i = 0; i < reader.FieldCount; i++){
            fieldArrayList.Add(reader.GetValue(i));
        }

        outerAL.Add(fieldArrayList);
    }

    return outerAL;
  }
  
  public List<string> selectAll(string table, int id){
    using var cmd = new NpgsqlCommand($"SELECT * FROM {table} WHERE ID ={id.ToString()} ", conn);
    using var reader = cmd.ExecuteReader();


    List<string> fieldList = new List<string>();

    while(reader.Read()){

      for (int i = 0; i < reader.FieldCount; i++){
          fieldList.Add(reader.GetValue(i).ToString());
      } 
    }

    return fieldList;
  }

  public void delete(string table, int id){
    using var cmd = new NpgsqlCommand { Connection = conn };
    cmd.CommandText = $"DELETE FROM {table} WHERE id  = {id};";

    cmd.ExecuteNonQuery();
      

    }
    


 
  public void insert(string table, ArrayList values){
    var names = getColName(values);
    var valuearray = getColValue(values);

    using var cmd = new NpgsqlCommand { Connection = conn };
    cmd.CommandText = $"INSERT INTO {table} ({getColNameAsString(values)}) VALUES (";
    
    foreach(string s in names){
      cmd.CommandText += "@" + s + ",";
    }

    cmd.CommandText = cmd.CommandText.Remove(cmd.CommandText.Length - 1);
    cmd.CommandText += ");";
    
    for(int i = 0; i < names.Length; i++){
      cmd.Parameters.AddWithValue($"@{names[i]}",valuearray[i]);

  }
  cmd.ExecuteNonQuery();

  }

  public void update(string table, int id, ArrayList values){
    var names = getColName(values);
    var valuearray = getColValue(values);

    using var cmd = new NpgsqlCommand { Connection = conn };
    cmd.CommandText = $"UPDATE {table}  SET ";
    
    foreach(string s in names){
      cmd.CommandText += s + " = @" + s;
    }
    cmd.CommandText += $" WHERE id={id}"; 

    cmd.CommandText += ";";
    
    for(int i = 0; i < names.Length; i++){
      cmd.Parameters.AddWithValue($"@{names[i]}",valuearray[i]);

    }
    cmd.ExecuteNonQuery();

  }
  

  public string[] getColName(ArrayList list){
    string[] tempString = new string[list.Count];    

    for (int i = 0; i < list.Count; i++){
      tempString[i] = ((string[])list[i])[0];
    }

    return tempString;
  }
  

  public string getColNameAsString(ArrayList list){
    string tempString ="";    
    foreach(string [] s  in list){
      tempString += s[0] + ",";
    }
    tempString = tempString.Remove(tempString.Length - 1);
    return tempString;
  }
  
  public string[] getColValue(ArrayList list){
    string[] tempString = new string[list.Count];    

    for (int i = 0; i < list.Count; i++){
      tempString[i] = ((string[])list[i])[1];
    }

    return tempString;
  }



}
