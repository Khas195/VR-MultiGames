using UnityEngine;

namespace script.BoidBehavior
{
    [RequireComponent(typeof(BoidController),typeof(BoidUnit))]
    public abstract class BoidBehavior : MonoBehaviour 
    {
        private BoidController _boidController = null;
        private Vector3 _steeringForce = Vector3.zero;

        [SerializeField] protected bool IsDrawGizmos = true;
        
        [Range(1,100)]
        [SerializeField] private float _blendScale = 1;
        
        public float BlendScale
        {
            get { return _blendScale; }
        }

        public Vector3 SteeringForce
        {
            get { return _steeringForce; }
            protected set { _steeringForce = value; }
        }

        protected BoidController BoidController
        {
            get { return _boidController; }
        }

        private void Awake()
        {
            _boidController = GetComponent<BoidController>();
        }

        public abstract void PerformBehavior();
    }
}
