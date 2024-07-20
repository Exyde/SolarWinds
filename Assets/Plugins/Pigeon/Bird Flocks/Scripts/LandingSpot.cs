///	Copyright Unluck Software /// www.chemicalbliss.com																															

using UnityEngine;
using System.Collections;
using System.Linq;

public class LandingSpot : MonoBehaviour
{
	#region Global Members
	public FlockChild _landingChild;
	public bool _landing;
	public LandingSpotController _controller;
	public Vector3 _preLandWaypoint;
	
	private Vector3 _cachePreLandingWaypoint;
	private float _distance;
	public Transform Transform => transform;

	#endregion
	
	#region Core Behavior
	public void Start()
	{
		_cachePreLandingWaypoint = _preLandWaypoint;
		if (_controller == null)
			_controller = Transform.parent.GetComponent<LandingSpotController>();
		
		if (_controller._autoCatchDelay.x > 0)
			StartCoroutine(GetFlockChild(_controller._autoCatchDelay.x, _controller._autoCatchDelay.y));
		
		if (_controller._randomRotate && _controller._parentBirdToSpot)
		{
			Debug.LogWarning(_controller + "\nEnabling random rotate and parent bird to spot is not yet available, disabling random rotate");
			_controller._randomRotate = false;
		}
	}
	#endregion

	#region Landing Spot
	public IEnumerator GetFlockChild(float minDelay, float maxDelay)
	{
		yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
		if (_controller._flock.gameObject.activeInHierarchy && (_landingChild == null))
		{
			FlockChild foundChild = null;   //Used to tell if a bird has been found to land
			foreach (var child in _controller._flock._roamers.Where(child => child != null && !child._landing && child.gameObject.activeInHierarchy))
			{
				child._landingSpot = this;
				_distance = Vector3.Distance(child._thisT.position, Transform.position);
				if (!_controller._onlyBirdsAbove)
				{
					if ((foundChild == null) && _controller._maxBirdDistance > _distance && _controller._minBirdDistance < _distance)
					{
						foundChild = child;
						if (!_controller._takeClosest) break;
					} else if ((foundChild != null) && Vector3.Distance(foundChild._thisT.position, Transform.position) > _distance)
					{
						foundChild = child;
					}
				} else
				{
					if ((foundChild == null) && child._thisT.position.y > Transform.position.y && _controller._maxBirdDistance > _distance && _controller._minBirdDistance < _distance)
					{
						foundChild = child;
						if (!_controller._takeClosest) break;
					} else if ((foundChild != null) && child._thisT.position.y > Transform.position.y && Vector3.Distance(foundChild._thisT.position, Transform.position) > _distance)
					{
						foundChild = child;
					}
				}
			}
			if (foundChild != null)
			{
				if (_controller._abortLanding) Invoke(nameof(ReleaseFlockChild), _controller._abortLandingTimer);
				_landingChild = foundChild;
				if (_controller._parentBirdToSpot) _landingChild.transform.SetParent(this.transform);
				_landing = true;
				_landingChild._landing = true;
				if (_controller._autoDismountDelay.x > 0) Invoke("ReleaseFlockChild", Random.Range(_controller._autoDismountDelay.x, _controller._autoDismountDelay.y));
				_controller._activeLandingSpots++;
				RandomRotate();
			} else if (_controller._autoCatchDelay.x > 0)
			{
				StartCoroutine(GetFlockChild(_controller._autoCatchDelay.x, _controller._autoCatchDelay.y));
			}
		}
	}
	public void InstantLand()
	{
		if (!_controller._flock.gameObject.activeInHierarchy || (_landingChild != null)) return;
		
		FlockChild foundChild = null;

		//Todo : Find first not all
		foreach (var child in _controller._flock._roamers.Where(child => !child._landing))
		{
			foundChild = child;
		}
			
		if (foundChild != null)
		{
			_landingChild = foundChild;

			if (_controller._parentBirdToSpot) _landingChild.transform.SetParent(this.transform);
			_landingChild._move = false;

			foundChild._speed = 0;
			foundChild._targetSpeed = 0;
			foundChild._landingSpot = this;
			_landing = true;
			_controller._activeLandingSpots++;
			_landingChild._landing = true;

			_landingChild._thisT.position = _landingChild.GetLandingSpotPosition();

			if (_controller._randomRotate) _landingChild._thisT.Rotate(Vector3.up, Random.Range(0f, 360f));
			else _landingChild._thisT.rotation = Transform.rotation;


			if (!_landingChild._animationIsBaked)
			{
				if (!_landingChild._animator) _landingChild._modelAnimation.Play(_landingChild._spawner._idleAnimation);
				else _landingChild._animator.Play(_landingChild._spawner._idleAnimation);
			} else _landingChild._bakedAnimator.SetAnimation(2, -1);
				
			if (_controller._autoDismountDelay.x > 0) Invoke("ReleaseFlockChild", Random.Range(_controller._autoDismountDelay.x, _controller._autoDismountDelay.y));
		} else if (_controller._autoCatchDelay.x > 0)
		{
			StartCoroutine(GetFlockChild(_controller._autoCatchDelay.x, _controller._autoCatchDelay.y));
		}
	}
	public void ReleaseFlockChild()
	{
		if (_controller._flock.gameObject.activeInHierarchy && _landingChild != null)
		{

			_preLandWaypoint = _cachePreLandingWaypoint;
			_landingChild._modelT.localEulerAngles = new Vector3(0, 0, 0);
			_landing = false;
			_landingChild._avoid = true;
			_landingChild._closeToSpot = false;
			//Reset flock child to flight mode
			_landingChild._damping = _landingChild._spawner._maxDamping;
			_landingChild._targetSpeed = Random.Range(_landingChild._spawner._minSpeed, _landingChild._spawner._maxSpeed);
			_landingChild._move = true;
			_landingChild._landing = false;
			_landingChild.currentAnim = "";
			_landingChild.Flap(.1f);

			if (_controller._parentBirdToSpot) _landingChild._spawner.AddChildToParent(_landingChild._thisT);
			_landingChild._wayPoint = new Vector3(_landingChild._wayPoint.x+5, Transform.position.y + 5, _landingChild._wayPoint.z+5);
			_landingChild._damping = 0.1f;
			if (_controller._autoCatchDelay.x > 0)
			{
				StartCoroutine(GetFlockChild(_controller._autoCatchDelay.x + 0.1f, _controller._autoCatchDelay.y + 0.1f));
			}
			_controller._activeLandingSpots--;
			if (_controller._abortLanding)
			{
				CancelInvoke(nameof(ReleaseFlockChild));
				if (_landingChild.currentAnim != "Idle") _landingChild.FindWaypoint();
			}
			_landingChild = null;

		}
	}
	#endregion

	#region Utility
	private void RandomRotate()
	{
		if (!_controller._randomRotate) return;
		var rot = Transform.rotation;
		var rotE = rot.eulerAngles;
		rotE.y = Random.Range(-180, 180);
		rot.eulerAngles = rotE;
		Transform.rotation = rot;
	}
	#endregion

	#region Gizmos
#if UNITY_EDITOR
	public void OnDrawGizmos()
	{
		if (_controller == null)
			_controller = Transform.parent.GetComponent<LandingSpotController>();
		if (!_controller._drawGizmos) return;
		
		Gizmos.color = Color.yellow;
		
		if (_landingChild != null && _landing)
			Gizmos.DrawLine(Transform.position, _landingChild._thisT.position);
		if (Transform.rotation.eulerAngles.x != 0 || Transform.rotation.eulerAngles.z != 0)
			Transform.eulerAngles = new Vector3(0.0f, Transform.eulerAngles.y, 0.0f);
		var position = Transform.position;
		Gizmos.DrawCube(new Vector3(position.x, position.y, position.z), Vector3.one * _controller._gizmoSize);
		Gizmos.DrawCube(position + (Transform.forward * _controller._gizmoSize), Vector3.one * _controller._gizmoSize * .5f);
		if(_preLandWaypoint != Vector3.zero)
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawCube(Transform.position + _preLandWaypoint, Vector3.one * _controller._gizmoSize * .25f);
		}
		if (Transform.parent != null && Transform.parent.GetChild(0) != Transform) return;
		Gizmos.color = new Color(1.0f, 1.0f, 0.0f, 1f);
		var position1 = Transform.position;
		Gizmos.DrawWireSphere(position1, _controller._maxBirdDistance);
		Gizmos.color = new Color(1.0f, 0.0f, 0.0f, 1f);
		Gizmos.DrawWireSphere(position1, _controller._minBirdDistance);
	}
#endif
	#endregion
}