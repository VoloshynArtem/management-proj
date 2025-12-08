using System;
using Npgsql;
using System.Collections;
namespace managementProj;


public partial class DatabaseManager{
  
  private NpgsqlConnection conn;

  public DatabaseManager(){
    EnvReader.Load(".env");
    
    connectToDB();
    createTabels();
  }


  private void connectToDB(){
   try{
      conn = new NpgsqlConnection($@"
        Server={Environment.GetEnvironmentVariable("DATABASE_SERVER")};
        Port={Environment.GetEnvironmentVariable("DATABASE_PORT")};
        Database={Environment.GetEnvironmentVariable("DB_PROJ_NAME")};
        User Id={Environment.GetEnvironmentVariable("DB_PROJ_ID")};
        Password={Environment.GetEnvironmentVariable("DB_PROJ_PASS")}
        ");
      conn.Open();
   }catch(PostgresException ex){

     Console.WriteLine(ex.SqlState);
      if (ex.SqlState == "28P01"){
          createUser();
          connectToDB();
      } else if (ex.SqlState == "3D000"){
        createDB();
          connectToDB();
      }
    }
  }


  private NpgsqlConnection getAdminConn(){
        NpgsqlConnection conn =  new NpgsqlConnection($@"
        Server={Environment.GetEnvironmentVariable("DATABASE_SERVER")};
        Port={Environment.GetEnvironmentVariable("DATABASE_PORT")};
        Database=postgres;
        User Id={Environment.GetEnvironmentVariable("DB_ADMIN_USERNAME")};
        Password={Environment.GetEnvironmentVariable("DB_ADMIN_PASS")}
        ");
        conn.Open();
        return conn;
    
  }

  private void createDB(){
        
      using (var adminConn = getAdminConn()){
        using (var command = new NpgsqlCommand($@"
          CREATE DATABASE {Environment.GetEnvironmentVariable("DB_PROJ_NAME")} OWNER management;",adminConn))
        {
            command.ExecuteNonQuery();
        }

    }

  }

  private void createUser(){
    using (var adminConn = getAdminConn()){

        using (var command = new NpgsqlCommand($@"
              CREATE USER 
              {Environment.GetEnvironmentVariable("DB_PROJ_ID")} 
              WITH PASSWORD '{Environment.GetEnvironmentVariable("DB_PROJ_PASS")}';",
              adminConn))
        {
            command.ExecuteNonQuery();
        }
        using (var command = new NpgsqlCommand($@"
          CREATE DATABASE {Environment.GetEnvironmentVariable("DB_PROJ_NAME")} OWNER {Environment.GetEnvironmentVariable("DB_PROJ_ID")};",adminConn))
        {
            command.ExecuteNonQuery();
        }


    }
  }

  private void createTabels(){
    using var command = new NpgsqlCommand { Connection = conn };
    command.CommandText = "";
    
    command.CommandText +=
                      $@"CREATE TABLE IF NOT EXISTS Kontinent(
                      id SERIAL PRIMARY KEY,
                      Kontinentbezeichnung VARCHAR(255));";
  
    command.CommandText +=
                      $@"CREATE TABLE IF NOT EXISTS Gehege(
                      id SERIAL PRIMARY KEY,
                      Gehegebezeichnung VARCHAR(255),
                      kontinentID INT,
                      FOREIGN KEY (kontinentID) REFERENCES Kontinent(id));";

    command.CommandText +=
                      $@"CREATE TABLE IF NOT EXISTS Tierart(
                      id SERIAL PRIMARY KEY,
                      TierartenBezeichnung VARCHAR(255));";

    command.CommandText +=
                      $@"CREATE TABLE IF NOT EXISTS Tiere(
                      id SERIAL PRIMARY KEY,
                      Name VARCHAR(255),
                      Gewicht REAL,
                      Geburtsdatum DATE,
                      TierartID INT,
                      TierartenID INT,
                      GehegeID INT,
                      FOREIGN KEY (TierartenID) REFERENCES Tierart(id),
                      FOREIGN KEY (GehegeID) REFERENCES Gehege(id));";


    command.ExecuteNonQuery();

  }

// • Kontinent: kID (PK), Kbezeichnung
// • Gehege: gID (PK), GBezeichnung, kontinentID (FK)
// • Tierart: tierartID (PK), TABezeichnung
// • Tiere: tierID (PK), Name, Gewicht, Geburtsdatum, TierartID (FK), GehegeID (FK)
//
//
//
  public ArrayList getKontinente(){
    using var cmd = new NpgsqlCommand("SELECT * FROM Kontinent", conn);
    using var reader = cmd.ExecuteReader();

    ArrayList outerAL = new ArrayList();
    ArrayList fieldArrayList;
    while (reader.Read())
    {
        fieldArrayList = new ArrayList();

        for (int i = 1; i < reader.FieldCount; i++)
        {
            fieldArrayList.Add(reader.GetValue(i));
        }

        outerAL.Add(fieldArrayList);
    }

    return outerAL;
    


  }
}
