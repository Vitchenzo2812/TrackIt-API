namespace TrackIt.Entities.Core;

public class Session : Data
{
  public Guid Id { get; set; }
  
  public string Name { get; set; }
  
  public string Email { get; set; }
  
  public double Income { get; set; }
}