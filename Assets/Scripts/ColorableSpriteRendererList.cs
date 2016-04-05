using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColorableSpriteRendererList : Colorable 
{

	public List<SpriteRenderer> renderersToColorize;

	public override void SetColor (Color c)
	{
		foreach(SpriteRenderer sr in renderersToColorize)
			sr.color = c;
	}
}
