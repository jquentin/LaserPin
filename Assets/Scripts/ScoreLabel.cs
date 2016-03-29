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
		label.fontSize = 30;
		Camera cam = NGUITools.FindCameraForLayer(LayerMask.NameToLayer("UI"));
		label.SetAnchor(cam.transform);
		if (index == 0)
		{
			label.rightAnchor = new UIRect.AnchorPoint(0f);
			label.bottomAnchor = new UIRect.AnchorPoint(1f);
			label.pivot = UIWidget.Pivot.TopLeft;

		}
		else if (index == 1)
		{
			label.leftAnchor = new UIRect.AnchorPoint(1f);
			label.bottomAnchor = new UIRect.AnchorPoint(1f);
			label.pivot = UIWidget.Pivot.TopRight;

		}
		else if (index == 2)
		{
			label.rightAnchor = new UIRect.AnchorPoint(0f);
			label.topAnchor = new UIRect.AnchorPoint(0f);
			label.pivot = UIWidget.Pivot.BottomLeft;

		}
		else if (index == 3)
		{
			label.leftAnchor = new UIRect.AnchorPoint(1f);
			label.topAnchor = new UIRect.AnchorPoint(0f);
			label.pivot = UIWidget.Pivot.BottomRight;

		}
		score = 0;
	}
}
