using UnityEngine;
using System.Collections;

public class ObjectTransformer : MonoBehaviour {
	
	public float TranslationSpeed
	{
		get { return this.translationSpeed; }
		set
		{
			if (value == this.translationSpeed)
				return;
			
			this.translationSpeed = value;
			
			this.recalculateTranslation();
		}
	}
	public Vector3 TranslationDirection
	{
		get { return this.translationDirection; }
		set
		{
			this.translationDirection = value.normalized;
			
			this.recalculateTranslation();
		}
	}
	public Vector3 Translation
	{
		get { return this.translation; }
		set
		{
			this.translation = value;			
			this.translationSpeed = value.magnitude;
			this.translationDirection = value.normalized;
		}
	}
	
	public Vector3 Rotation
	{
		get { return this.rotation; }
		set 
		{
			this.rotation = value;
		}
	}
	
	// Use this for initialization
	protected virtual void Start () {
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		
		this.transform.Translate(this.Translation * Time.deltaTime);
		
		this.transform.Rotate(0, this.Rotation.x, 0, Space.Self);
		this.transform.Rotate(this.Rotation.y, 0, 0, Space.Self);
		
		this.transform.Rotate(0, 0, this.Rotation.z, Space.Self);
		
		
	}
	
	private void recalculateTranslation()
	{
		this.translation = this.translationDirection * this.translationSpeed;
		
	}
	
	private float translationSpeed;
	private Vector3 translationDirection;
	private Vector3 translation;
	
	private Vector3 rotation;
}
