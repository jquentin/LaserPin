using UnityEngine;
using System.Collections;

public abstract class TweenLoop : MonoBehaviour {
	
	public enum Axis { X, Y, Z, All }
	
	public bool constantSpeed = false;
	public bool setTargetAtStart = false;
	public float target = 0f;
	public iTween.EaseType easeType = iTween.EaseType.easeInOutSine;
	public float time = 0.765f;
	public bool startByLeft = false;
	public float timeRand = 0f;
	public float amplitude = 20f;
	private float _currentAmplitude;
	public float speedBack = 5f;
	public bool wobbleAtStart = false;
	public bool isLocal = false;
	public Axis axis = Axis.Z;
	public AudioClip clip;
	public bool optimized = false;

	abstract protected string allAxisPropertyName
	{
		get;
	}

	private string axisString 
	{ 
		get 
		{ 
			switch (axis) 
			{ 
			case Axis.X: return "x";
			case Axis.Y: return "y";
			case Axis.Z: return "z";
			case Axis.All: return allAxisPropertyName;
			default: return allAxisPropertyName;
			}
		}
	}

	protected abstract Vector3 coordinatesValues
	{
		get;
		set;
	}
	
	private float coordinateValue
	{
		get
		{
			switch (axis) 
			{ 
			case Axis.X: return coordinatesValues.x;
			case Axis.Y: return coordinatesValues.y;
			case Axis.Z: return coordinatesValues.z;
			default: return coordinatesValues.z;
			}
		}
		set
		{
			Vector3 current = coordinatesValues;
			switch (axis) 
			{ 
			case Axis.X: current.x = value; break;
			case Axis.Y: current.y = value; break;
			case Axis.Z: current.z = value; break;
			case Axis.All: current = value * Vector3.one; break;
			default: current.z = value; break;
			}
		}
	}

	private float relativeCoordinateValue
	{
		get
		{
			return coordinateValue - target;
		}
		
		set
		{
			coordinateValue = value + target;
		}
	}
	
	void Awake()
	{
		if (setTargetAtStart)
			target = coordinateValue;
		_currentAmplitude = amplitude;
		time = time + Random.Range(-timeRand / 2f, timeRand / 2f);
	}
	
	void Start()
	{
		if (wobbleAtStart)
			Wobble(true);
	}

	
	void RotateLeft()
	{
		Tween(gameObject, iTween.Hash(
			axisString, -_currentAmplitude + target,
			"time", time,
			"easetype", easeType,
			"oncomplete", "RotateRight",
			"islocal", isLocal));
	}
	
	void RotateRight()
	{
		Tween(gameObject, iTween.Hash(
			axisString, _currentAmplitude + target,
			"time", time,
			"easetype", easeType,
			"oncomplete", "RotateLeft",
			"islocal", isLocal));
	}
	
	void RotateOptimized()
	{
		Tween(gameObject, iTween.Hash(
			axisString, _currentAmplitude + target,
			"time", time,
			"easetype", easeType,
			"islocal", isLocal));
	}

	protected abstract void Tween(GameObject go, Hashtable hash);
	
	void UpdateAmplitude(float value)
	{
		_currentAmplitude = value;
	}

	public void Wobble(bool enable, bool comeBack = true, bool inertia = true)
	{
		if (enable)
		{
			float angle = this.relativeCoordinateValue;
			if (optimized)
			{
				this.relativeCoordinateValue = - amplitude;
				Tween(gameObject, iTween.Hash(
					axisString, (startByLeft ? -1f : 1f) * amplitude + target,
					"time", time,
					"easetype", easeType,
					"islocal", isLocal,
					"looptype", iTween.LoopType.pingPong));
			}
			else
			{
				if (inertia && Mathf.Abs(angle) > Mathf.Abs(amplitude))
				{
					float initialAmp = Mathf.Max( Mathf.Abs(amplitude), Mathf.Abs(angle) - speedBack * time);
					UpdateAmplitude(initialAmp);
					iTween.ValueTo(gameObject, iTween.Hash(
						"from", initialAmp,
						"to", amplitude,
						"speed", speedBack,
						"onupdate", "UpdateAmplitude",
						"easetype", iTween.EaseType.linear));
					if (angle < target)
					{
						RotateRight();
					}
					else
					{
						RotateLeft();
					}
				}
				else
				{
					Tween(gameObject, iTween.Hash(
						axisString, (startByLeft ? -_currentAmplitude : _currentAmplitude) + target,
						"time", time * 0.5f,
						"easetype", iTween.EaseType.easeOutSine,
						"oncomplete", (startByLeft ? "RotateRight" : "RotateLeft"),
						"islocal", isLocal));
				}
			}
		}
		else
		{
			iTween.Stop(gameObject, "rotate");
			if (!optimized && comeBack)
			{
				Tween(gameObject, iTween.Hash(
					axisString, target,
					"time", 0.2f,
					"easetype", easeType,
					"islocal", isLocal));
			}
		}
	}
}
