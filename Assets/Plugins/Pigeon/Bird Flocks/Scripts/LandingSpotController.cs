///	Copyright Unluck Software /// www.chemicalbliss.com																															
using UnityEngine;
using System.Collections;

public class LandingSpotController : MonoBehaviour
{
	public bool _rotateAfterLanding = true;							// Rotates the bird to the rotation of the landing spot
	public bool _randomRotate = true;                               // Random rotation when a bird lands
	public Vector2 _autoCatchDelay = new Vector2(10.0f, 20.0f);     // Random Min/Max time for landing spot to make a bird land
	public Vector2 _autoDismountDelay = new Vector2(10.0f, 20.0f);  // Random Min/Max time for birds to automaticly fly away from landing spot
	public float _maxBirdDistance = 20.0f;                          // The maximum distance to a bird for it to land
	public float _minBirdDistance = 5.0f;                           // The minimum distance to a bird for it to land
	public bool _takeClosest;                                       // Toggle this to make landingspots make the closest bird to it land
	public FlockController _flock;                                  // Assign the FlockController to pick birds from
	public bool _landOnStart;                                       // Put birds on the landing spots at start
	public bool _soarLand = true;                                   // Birds will soar while aproaching landing spot
	public bool _onlyBirdsAbove;                                    // Only birds above landing spot will land
	public float _landingSpeedModifier = .5f;                       // Adjust bird movement speed while close to the landing spot
	public float _closeToSpotSpeedModifier = 1f;						// Speed modifier at the very end of the landing sequence

	public float _landingTurnSpeedModifier = 5.0f;
	[HideInInspector]
	public Transform _transformCache;                                        
	[HideInInspector]
	public int _activeLandingSpots;
	[Range(0.01f, 1f)]
	public float _snapLandDistance = 0.05f;                         
	public float _landedRotateSpeed = 2f;
	public bool _drawGizmos = true;
	public float _gizmoSize = 0.2f;
	public bool _parentBirdToSpot;				  //Used in cases where landing spots moves, makes it easier for birds to land
	public bool _abortLanding;
	[Range(1, 20)]
	public float _abortLandingTimer = 10f;        // If birds have a tendency to get stuck while landing this can be used as a safety measure 
	
	public void Start() {
		if (_transformCache == null) _transformCache = transform;
		if (_flock == null) {
			_flock = (FlockController)FindObjectOfType(typeof(FlockController));
			Debug.Log(this + " has no assigned FlockController, a random FlockController has been assigned");
		}
		if (_landOnStart) {
			StartCoroutine(InstantLandOnStart(.1f));
		}
	}

	public void ScareAll() {
		for (int i = 0; i < _transformCache.childCount; i++) {
			if (_transformCache.GetChild(i).GetComponent<LandingSpot>() != null) {
				LandingSpot spot = _transformCache.GetChild(i).GetComponent<LandingSpot>();

				spot.ReleaseFlockChild();
			}
		}
	}

	public void LandAll() {
		for (int i = 0; i < _transformCache.childCount; i++) {
			if (_transformCache.GetChild(i).GetComponent<LandingSpot>() != null) {
				LandingSpot spot = _transformCache.GetChild(i).GetComponent<LandingSpot>();
				StartCoroutine(spot.GetFlockChild(0.0f, 2.0f));
			}
		}
	}

	//This function was added to fix a error with having a button calling InstantLand
	private IEnumerator InstantLandOnStart(float delay) {
		yield return new WaitForSeconds(delay);
		for (var i = 0; i < _transformCache.childCount; i++)
		{
			if (_transformCache.GetChild(i).GetComponent<LandingSpot>() == null) continue;
			LandingSpot spot = _transformCache.GetChild(i).GetComponent<LandingSpot>();
			spot.InstantLand();
		}
	}

	public IEnumerator InstantLand(float delay) {
		yield return new WaitForSeconds(delay);
		for (var i = 0; i < _transformCache.childCount; i++)
		{
			if (_transformCache.GetChild(i).GetComponent<LandingSpot>() == null) continue;
			LandingSpot spot = _transformCache.GetChild(i).GetComponent<LandingSpot>();
			spot.InstantLand();
		}
	}
}
