using UnityEngine;

namespace script.BoidBehavior
{
    [RequireComponent(typeof(BoidController),typeof(BoidUnit))]
    public abstract class BoidBehavior : MonoBehaviour 
    {
        private BoidController _boidController = null;
        private Vector3 _steeringForce = Vector3.zero;

        [SerializeField] private bool _isEnable = true;
        [SerializeField] protected bool IsDrawGizmos;
        
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

        public bool IsEnable
        {
            get { return _isEnable; }
            set { _isEnable = value; }
        }
        
        protected BoidController BoidController
        {
            get { return _boidController; }
        }

        private void Awake()
        {
            _boidController = GetComponent<BoidController>();
            if (!_boidController.BehaviorList.Contains(this))
            {
                _boidController.BehaviorList.Add(this);
            }
        }

        private void OnDestroy()
        {
            if (_boidController.BehaviorList.Contains(this))
            {
                _boidController.BehaviorList.Remove(this);
            }
        }

        public abstract void PerformBehavior();
    }
}
