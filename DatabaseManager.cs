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

    try{
      cmd.ExecuteNonQuery();
    }catch(PostgresException ex){
      if (ex.SqlState == "23503"){
        throw new DeleteReferenceException{table = table, id = id };
      }
    }

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
      int value;
      DateTime dateValue;
      cmd.Parameters.AddWithValue($"@{names[i]}", parseString(valuearray[i]));


  }
  cmd.ExecuteNonQuery();

  }

  public void update(string table, int id, ArrayList values){
    var names = getColName(values);
    var valuearray = getColValue(values);

    using var cmd = new NpgsqlCommand { Connection = conn };
    cmd.CommandText = $"UPDATE {table} SET ";

    foreach(string s in names){
      cmd.CommandText +=  s + " = @" + s + ",";

    }
    cmd.CommandText = cmd.CommandText.Remove(cmd.CommandText.Length - 1);

    cmd.CommandText += $" WHERE id={id}"; 
    cmd.CommandText += ";";
    
    for(int i = 0; i < names.Length; i++){
      cmd.Parameters.AddWithValue($"@{names[i]}", parseString(valuearray[i]));

    }
    cmd.ExecuteNonQuery();

  }
  
  public object parseString(string s){
    if (int.TryParse(s, out var intValue)){
        return intValue;
    }else if (float.TryParse(s, out var floatValue)){
        return floatValue;
    }else if (DateTime.TryParse(s, out var dateValue)){
        return dateValue;
    }
    return s;

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

  public ArrayList getTableFields(string table){

  
    using var cmd = new NpgsqlCommand { Connection = conn };
//TODO fix this
    cmd.CommandText = $@"SELECT 
          att.attname                 AS column_name,
          
          pg_catalog.format_type(att.atttypid, att.atttypmod) AS data_type,
          
          rcls.relname                AS referenced_table,
          ratt.attname                AS referenced_column

      FROM pg_catalog.pg_attribute att
      JOIN pg_catalog.pg_class cls     ON cls.oid = att.attrelid
      JOIN pg_catalog.pg_namespace ns  ON ns.oid = cls.relnamespace

      LEFT JOIN pg_catalog.pg_constraint con 
            ON con.conrelid = cls.oid 
            AND att.attnum = ANY(con.conkey) 
            AND con.contype = 'f'

      LEFT JOIN pg_catalog.pg_class rcls 
            ON rcls.oid = con.confrelid

      LEFT JOIN pg_catalog.pg_attribute ratt
            ON ratt.attrelid = con.confrelid
            AND ratt.attnum = con.confkey[1]
      WHERE att.attnum > 0 
        AND NOT att.attisdropped
        AND ns.nspname NOT IN ('pg_catalog', 'information_schema')
        AND cls.relname = '{table}'
      OFFSET 1
        ;";

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

}
