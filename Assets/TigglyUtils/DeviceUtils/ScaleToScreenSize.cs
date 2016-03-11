using UnityEngine;
using System.Collections;

public class ScaleToScreenSize : MonoBehaviour {

	public enum ScaleType { ScaleToRatio, ScaleToSize }
	public ScaleType scaleType = ScaleType.ScaleToRatio;

	public enum Type { Stretch, DoNotStretch, ScaleToFit, ScaleToFill }
	public Type type = Type.Stretch;

	public bool cancelSizingOnChildren = false;

	const float REFERENCE_RATIO = 4f / 3f;

	public Vector2 referenceScreenSize;

	[ContextMenu("Initialize ref screen size")]
	void InitRefScreenSize()
	{
		referenceScreenSize = new Vector2((float)Screen.width, (float)Screen.height);
	}

	void Start () 
	{
		if (scaleType == ScaleType.ScaleToRatio)
		{
			if (Mathf.Abs(DeviceUtils.aspectRatio - REFERENCE_RATIO) < 0.05f)
				return;
			float multiplier = DeviceUtils.aspectRatio / REFERENCE_RATIO;
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
		else
		{

			if (type == Type.DoNotStretch || type == Type.ScaleToFit)
			{
				float multiplier = Mathf.Min((float)Screen.width / referenceScreenSize.x, (float)Screen.height / referenceScreenSize.y);
				transform.localScale *= multiplier;
			}
			else if (type == Type.ScaleToFill)
			{
				float multiplier = Mathf.Max((float)Screen.width / referenceScreenSize.x, (float)Screen.height / referenceScreenSize.y);
				transform.localScale *= multiplier;
			}
			else
			{
				Vector3 scale = transform.localScale;
				scale.x *= (float)Screen.width / referenceScreenSize.x;
				scale.y *= (float)Screen.height / referenceScreenSize.y;
				transform.localScale = scale;
			}
		}
	}

}
