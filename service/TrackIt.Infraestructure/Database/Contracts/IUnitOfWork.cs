namespace TrackIt.Infraestructure.Database.Contracts;

public interface IUnitOfWork
{
  Task SaveChangesAsync ();
}