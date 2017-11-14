﻿using System.Collections.Generic;
using script.ControllerScript;
using UnityEngine;

namespace script.Boid_Flocking
{
	public class BoidController : Controller
	{
		[SerializeField] private float _minDistance;
		[SerializeField] private float _areaOfInterest;
		[SerializeField] private List<GameObject> _neighbourList;
		[SerializeField] private List<Collider> _obstacleList;
		[SerializeField] private Transform _target;

		private Vector3 _aligmentForce;
		private Vector3 _cohesionForce;
		private Vector3 _separationForce;
		private Vector3 _avoidanceForce;
		
		private float _aligmentWeight = 0.2f;
		private float _cohensionWeight = 0.2f;
		private float _separationWeight = 0.3f;
		private float _avoidanceWeight = 0.3f;
		private float _targetWeight = 0.3f;
		
		private void FixedUpdate()
		{
			CheckNeighbour();
			_aligmentForce = Alignment(_neighbourList).normalized * _aligmentWeight;
			_cohesionForce = Cohesion(_neighbourList).normalized * _cohensionWeight;
			_separationForce = Separation(_neighbourList).normalized * _separationWeight;
			_avoidanceForce = AvoidanceObstacle(_obstacleList).normalized * _avoidanceWeight;
			
			Vector3 newVec = _aligmentForce +
			                 _cohesionForce + 
			                 _separationForce + 
			                 _avoidanceForce;

			if (_target != null)
			{
				newVec += (_target.position - transform.position).normalized * _targetWeight;
			}
			
			foreach (var movement in MovementList)
			{
				if (movement.isActiveAndEnabled)
				{
					movement.Move(newVec.normalized);
				}
			}

		}

		private Vector3 Alignment(List<GameObject> neighbourList)
		{
			if (neighbourList.Count == 0) return Vector3.zero;
				
			Vector3 newDir = Vector3.zero;

			foreach (var neighbour in neighbourList)
			{
				newDir += neighbour.GetComponent<Rigidbody>().velocity;
			}
			
			newDir /= _neighbourList.Count;
			return newDir;
		}

		private Vector3 Cohesion(List<GameObject> neighbourList)
		{
			if (neighbourList.Count == 0) return Vector3.zero;
			
			Vector3 newPos = this.transform.position;
			
			foreach (var neighbour in neighbourList)
			{
				newPos += neighbour.transform.position;
			}
			newPos /= neighbourList.Count;
			return newPos - this.transform.position;
		}

		private Vector3 Separation(List<GameObject> neighbourList)
		{
			if (neighbourList.Count == 0) return Vector3.zero;
			
			Vector3 newDir = Vector3.zero;
			int tooCloseNeighbour = 0;

			foreach (var neighbour in neighbourList)
			{
				Vector3 temp = transform.position - neighbour.transform.position;

				if (temp.magnitude <= _minDistance)
				{
					++tooCloseNeighbour;
					newDir += temp.normalized * _minDistance / temp.magnitude;
				}
			}
			newDir /= tooCloseNeighbour;
			return newDir;
		}

		private Vector3 AvoidanceObstacle(List<Collider> obstacleList)
		{
			if (obstacleList.Count == 0) return Vector3.zero;
			
			Vector3 newDir = Vector3.zero;
			int tooCloseObstacle = 0;

			foreach (var obstacle in obstacleList)
			{
				Vector3 temp = transform.position - obstacle.ClosestPointOnBounds(transform.position);

				if (temp.magnitude <= _minDistance)
				{
					++tooCloseObstacle;
					newDir += temp.normalized * _minDistance / temp.magnitude;
				}
			}
			newDir /= tooCloseObstacle;
			return newDir;
		}

		private void CheckNeighbour()
		{
			_neighbourList.Clear();
			_obstacleList.Clear();
			Collider[] hitColliders = Physics.OverlapSphere(transform.position, _areaOfInterest);

			foreach (var col in hitColliders)
			{
				if(col.gameObject == this.gameObject) continue;
				if (col.gameObject.GetComponent<BoidController>())
				{
					_neighbourList.Add(col.gameObject);
				}
				else if (col.gameObject.layer != LayerMask.NameToLayer("Ground"))
				{
					_obstacleList.Add(col);
				}
			}
		}

		private void OnDrawGizmos()
		{
			Gizmos.color = Color.green;
			Gizmos.DrawLine(transform.position, transform.position + _aligmentForce);
			
			Gizmos.color = Color.blue;
			Gizmos.DrawLine(transform.position, transform.position + _cohesionForce);
			
			Gizmos.color = Color.red;
			Gizmos.DrawLine(transform.position, transform.position + _separationForce);

			
			Gizmos.color = Color.gray;
			Gizmos.DrawLine(transform.position, transform.position + _avoidanceForce);
		}

		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere(transform.position, _areaOfInterest);
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(transform.position, _minDistance);
		}
	}
}
