namespace managementProj;

public class GridRow{
  public string Tiername { get; set; }
  public double Gewicht { get; set; }
  public string Tierart { get; set; }
  public string Gehege { get; set; }
  public string Kontinent { get; set; }



  public string stringifyCSV(){
    return $"{Tiername},{Gewicht},{Tierart},{Gehege},{Kontinent}";
  }

}
