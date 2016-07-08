using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DigitalRuby.ThunderAndLightning;

[RequireComponent(typeof(LightningBoltPathScript))]
public class LightningLineRenderer : CustomLineRenderer 
{

	LightningBoltPathScript _lightningPath;
	LightningBoltPathScript lightningPath
	{
		get
		{
			if (_lightningPath == null)
				_lightningPath = this.GetComponent<LightningBoltPathScript>();
			return _lightningPath;
		}
	}

	private List<GameObject> pathNodes = new List<GameObject>();

	public override Color color 
	{
		set 
		{
			lightningPath.GlowTintColor = value;
		}
	}

	public override void SetVertexCount (int count)
	{
		while (count > lightningPath.LightningPath.List.Count)
		{
			GameObject go = new GameObject("LineRenderer-Node#" + lightningPath.LightningPath.List.Count);
			lightningPath.LightningPath.List.Add(go);
		}

		for (int i = 0 ; i < lightningPath.LightningPath.List.Count - count ; i++)
		{
			int index = lightningPath.LightningPath.List.Count - 1 - i;
			Destroy(lightningPath.LightningPath.List[index]);
			lightningPath.LightningPath.List.RemoveAt(index);
		}

		lightningPath.CountRange.Minimum = (count - 1) * 2;
		lightningPath.CountRange.Maximum = (count - 1) * 2;
		lightningPath.GlowIntensity = 0.4f + (count - 2) * 0.03f;
		lightningPath.GlowWidthMultiplier = 2f + (count - 2) * 0.3f;
	}

	public override void SetPosition (int i, Vector3 pos)
	{
		lightningPath.LightningPath.List[i].transform.position = pos;
	}

}
