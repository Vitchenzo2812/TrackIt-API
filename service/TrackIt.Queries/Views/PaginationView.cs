namespace TrackIt.Queries.Views;

public record PaginationView<T> (
  int Page,
  
  int Pages,
  
  T Data
)
{
  public static PaginationView<T> Build (int page, int pages, T data)
  {
    return new PaginationView<T>(
      Page: page,
      
      Pages: pages,
      
      Data: data
    );
  }
}