namespace MSharp.ModLoader.StagingSystem;
public class StagingManager<T>
{
	private readonly Stack<T> _history = new();
	private T? _current;
	private readonly Action<T> _applyCallback;
	private readonly Action<T> _rollbackCallback;

	public StagingManager(Action<T> applyCallback, Action<T> rollbackCallback)
	{
		_applyCallback = applyCallback;
		_rollbackCallback = rollbackCallback;
	}

	public void MSadd(T next)
	{
		if (_current is not null) _history.Push(_current);

		_current = next;

		try
		{
			_applyCallback(_current);
		}
		catch (Exception)
		{
			MSrevert(); // Automático
			return;
		}
	}

	public void MSrevert()
	{
		if (_history.Count > 0)
		{
			_current = _history.Pop();
			_rollbackCallback(_current);
		}
		else
		{
			Console.WriteLine("[Staging] No hay versiones previas para hacer rollback.");
		}
	}

	public void MScommit()
	{
		_history.Clear(); // Confirmamos estado actual como final
	}

	public T? MSgetCurrent() => _current;
}
