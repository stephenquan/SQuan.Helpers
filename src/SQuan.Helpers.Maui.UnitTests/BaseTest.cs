// BaseTest.cs

using SQuan.Helpers.Maui.UnitTests.Mocks;

namespace SQuan.Helpers.Maui.UnitTests;

public abstract partial class BaseTest : IDisposable
{
	readonly MockDispatcherProvider dispatcherProvider;

	bool isDisposed;

	protected BaseTest()
	{
		DispatcherProvider.SetCurrent(dispatcherProvider = new MockDispatcherProvider());
	}

	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (isDisposed)
		{
			return;
		}

		if (disposing)
		{
			DispatcherProvider.SetCurrent(null);
			dispatcherProvider.Dispose();
		}

		isDisposed = true;
	}
}
