namespace TrackIt.Queries.Views;

public record ActivityView (
  Guid Id,
  
  string Title,
  
  string Description,
    
  bool Checked,
  
  int Order
);