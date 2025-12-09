using System;
namespace managementProj;
public class DBException : Exception {
  public string table;
  public int id;

}

public class DeleteReferenceException : DBException;
