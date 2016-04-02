using UnityEngine;
using System.Collections;

public class ScoreLabel : MonoBehaviour {

	UILabel _label;
	UILabel label
	{
		get
		{
			if (_label == null)
				_label = GetComponent<UILabel>();
			return _label;
		}
	}

	private int _score;
	public int score
	{
		get
		{
			return _score;
		}
		set
		{
			_score = value;
			label.text = _score.ToString();
		}
	}


	public void Initialize (int index, Color color) 
	{

		gameObject.name = "Score Player " + index;
		label.color = color;
//		label.fontSize = 30;
		Camera cam = NGUITools.FindCameraForLayer(LayerMask.NameToLayer("UI"));
		label.SetAnchor(cam.transform);
		int absMarginHoriz = 5;
		int absMarginVert = 0;
		if (index == 0)
		{
			label.pivot = UIWidget.Pivot.TopLeft;
			label.leftAnchor.relative = 0f;
			label.leftAnchor.absolute = absMarginHoriz;
			label.topAnchor.relative = 1f;
			label.topAnchor.absolute = -absMarginVert;
			label.bottomAnchor.relative = 0.9f;

		}
		else if (index == 1)
		{
			label.pivot = UIWidget.Pivot.TopRight;
			label.rightAnchor.relative = 1f;
			label.rightAnchor.absolute = -absMarginHoriz;
			label.topAnchor.relative = 1f;
			label.topAnchor.absolute = -absMarginVert;
			label.bottomAnchor.relative = 0.9f;

		}
		else if (index == 2)
		{
			label.pivot = UIWidget.Pivot.BottomLeft;
			label.leftAnchor.relative = 0f;
			label.leftAnchor.absolute = absMarginHoriz;
			label.bottomAnchor.relative = 0f;
			label.bottomAnchor.absolute = absMarginVert;
			label.topAnchor.relative = 0.1f;

		}
		else if (index == 3)
		{
			label.pivot = UIWidget.Pivot.BottomRight;
			label.rightAnchor.relative = 1f;
			label.rightAnchor.absolute = -absMarginHoriz;
			label.bottomAnchor.relative = 0f;
			label.bottomAnchor.absolute = absMarginVert;
			label.topAnchor.relative = 0.1f;

		}
		score = 0;
	}
}
