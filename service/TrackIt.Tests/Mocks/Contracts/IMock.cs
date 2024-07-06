namespace TrackIt.Tests.Mocks.Contracts;

public interface IMock<T>
{
  void Verify (T expect, T current);
}