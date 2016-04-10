using System.Collections.Generic;

/// <summary>
/// Used as a normal queue that still allows regular list actions such as getting from any spot in the list, and removing from any spot in the list
/// </summary>
public class AnywhereQueue<T> : List<T>
{
	/// <summary>
	/// Enqueue the specified item into the queue.
	/// </summary>
	/// <param name="item">Item to add to queue.</param>
	public void Enqueue(T item)
	{
		Add(item);
	}

	/// <summary>
	/// Dequeue the first element of the queue
	/// </summary>
	public T Dequeue()
	{
		var tempReturn = this[0];
		RemoveAt(0);
		return tempReturn;
	}

	/// <summary>
	/// Peek at the first element of the queue
	/// </summary>
	public T Peek()
	{
		return this[0];
	}

	/// <summary>
	/// Gets a value indicating whether the queue is empty.
	/// </summary>
	/// <value><c>true</c> if this queue is empty; otherwise, <c>false</c>.</value>
	public bool IsEmpty { get { return this.Count == 0; } }
}
