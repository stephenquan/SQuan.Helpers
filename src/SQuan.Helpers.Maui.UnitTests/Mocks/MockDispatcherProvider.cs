// MockDispatcherProvider.cs

namespace SQuan.Helpers.Maui.UnitTests.Mocks;

// Inspired by https://github.com/dotnet/maui/blob/main/src/Core/tests/UnitTests/TestClasses/DispatcherStub.cs
sealed partial class MockDispatcherProvider : IDispatcherProvider, IDisposable
{
	readonly ThreadLocal<IDispatcher> dispatcherInstance = new(static () => new MockDispatcher());

	public IDispatcher GetForCurrentThread() => dispatcherInstance.Value ?? throw new InvalidOperationException();

	public void Dispose() => dispatcherInstance.Dispose();
}
