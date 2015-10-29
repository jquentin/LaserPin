using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class CircularQueue<Type>:Queue<Type>
{
	
	public int maxSize { get; private set; }
	
	public CircularQueue(int size):base(size)
	{
		maxSize = size;
	}
	
	public void EnqueueWithLimit(Type element)
	{
		Enqueue(element);
		if (Count > maxSize)
			Dequeue();
	}
}

public static class CollectionUtils 
{


}
