public interface IFsmState
{
	/// <summary>
	/// Called when the state is set to the current state
	/// </summary>
	void OnEntry();

	/// <summary>
	/// Called when the state is removed as the current state
	/// </summary>
	void OnExit();
}
