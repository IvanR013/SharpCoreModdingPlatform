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
		if (_current != null) _history.Push(_current);

		_current = next;

		try
		{
			_applyCallback(_current);
		}
		catch (Exception)
		{
			MSrevert(); // Automático por si la aplicación falla
			Console.WriteLine("[Staging] Error al aplicar la instrucción. Se ha revertido el cambio.");
			_current = null;
			_history.Clear();
			return;
		}
	}

	public void MSrevert()
	{
		if (_history.Count == 0)
		{
			Console.WriteLine("[Staging] No hay versiones previas para hacer rollback.");
			return;
		}


		_current = _history.Pop();
		_rollbackCallback(_current);
	}

	public void MScommit() => _history.Clear(); // Confirmamos estado actual como final


	public T? MSgetCurrent() => _current; // Devuelve el estado actual, o null si no hay ninguno - es como una memoria ram
}
