using UnityEngine;
using System.Collections;

public class ScaleToScreenSize : MonoBehaviour {

	public enum Type { Stretch, DoNotStretch }
	public Type type = Type.Stretch;

	public bool cancelSizingOnChildren = false;


	void Start () 
	{
		if (Mathf.Abs(DeviceUtils.aspectRatio - 1.3333333f) < 0.05f)
			return;
		float multiplier = DeviceUtils.aspectRatio / 1.3333f;
		if (type == Type.DoNotStretch)
		{
			transform.localScale *= multiplier;
		}
		else
		{
			Vector3 scale = transform.localScale;
			scale.x *= multiplier;
			transform.localScale = scale;
		}
		if (cancelSizingOnChildren)
		{
			for(int i = 0 ; i < transform.childCount ; i++)
			{
				Transform child = transform.GetChild(i);
				Vector3 childScale = child.localScale;
				childScale.x /= multiplier;
				child.localScale = childScale;
			}
		}
	}

}
