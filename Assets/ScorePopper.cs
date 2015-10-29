using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TextMesh))]
public class ScorePopper : MonoBehaviour {

	private TextMesh _textMesh;
	private TextMesh textMesh
	{
		get
		{
			if (_textMesh == null)
				_textMesh = GetComponent<TextMesh>();
			return _textMesh;
		}
	}
	private MeshRenderer _meshRenderer;
	private MeshRenderer meshRenderer
	{
		get
		{
			if (_meshRenderer == null)
				_meshRenderer = GetComponent<MeshRenderer>();
			return _meshRenderer;
		}
	}

	public void Init(Color c)
	{
		textMesh.color = c;
		meshRenderer.enabled = false;
	}

	public void Pop(int score)
	{
		textMesh.text = score.ToString();
		meshRenderer.enabled = true;
		iTween.MoveBy(gameObject, iTween.Hash(
			"y", 2f,
			"time", 1f,
			"easetype", iTween.EaseType.easeOutQuad));
		iTween.FadeTo(gameObject, iTween.Hash(
			"alpha", 0f,
			"time", 1f,
			"easetype", iTween.EaseType.linear));
	}



}
