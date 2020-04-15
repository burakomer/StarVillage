using UnityEngine;
namespace DwarfEngine
{
[RequireComponent(typeof(Collider2D))]
    public class Projectile : MonoBehaviour
    {
        public SpriteRenderer model;
        public int damage;
        public float invincibilityDuration;
        public float speed;
        public float maxDistance;
        public float knockbackForce;
        public Transform impactPoint;
        public float impactRadius;
        public LayerMask hitMask;
        public Vector2 fireOffset;

        //[Header("Feedbacks")]
        //public float feedbackTime = 2f;
        //public Feedback impactFeedback;
        //private ParticleSystem trail;

        private Collider2D _collider;
        private Rigidbody2D _rb;
        private Vector3 originalPos;

        private void Start()
        {
            _collider = GetComponent<Collider2D>();
            //_rb = GetComponent<Rigidbody2D>();

            //if (impactFeedback != null)
            //{
            //    impactFeedback.OnFeedbackStart = OnFeedbackStart;
            //    impactFeedback.OnFeedbackEnd = OnFeedbackEnd;
            //}

            //ParticleSystem[] particles = GetComponentsInChildren<ParticleSystem>();
            //foreach (ParticleSystem particle in particles)
            //{
            //    if (particle.CompareTag("Trail"))
            //    {
            //        trail = particle;
            //        break;
            //    }
            //}

            //invincibilityDuration = ScalingManager.Instance.invincibilityDuration;
        }

        private void OnEnable()
        {
            originalPos = transform.position;
        }

        private void FixedUpdate()
        {
            transform.Translate(Quaternion.AngleAxis(transform.rotation.z, Vector2.right).normalized * Vector2.right * speed * Time.fixedDeltaTime);
            if (Vector2.Distance(transform.position, originalPos) >= maxDistance)
            {
                OnFeedbackEnd();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Default"))
            {
                return;
            }

            Collider2D[] hitObjects = Physics2D.OverlapCircleAll(impactPoint.position, impactRadius, hitMask);
            if (hitObjects.Length > 0)
            {
                foreach (Collider2D hitObj in hitObjects)
                {
                    Health _health = hitObj.GetComponent<Health>();
                    if (_health != null)
                    {
                        _health.Damage(damage, invincibilityDuration);
                    }

                    if (knockbackForce > 0.05f)
                    {
                        Rigidbody2D colRb = hitObj.GetComponent<Rigidbody2D>();

                        if (colRb != null)
                        {
                            colRb.AddForce(Vector3.Normalize(collision.transform.position - transform.position));
                        }
                    }
                }
                //impactFeedback.Initiate(feedbackTime);
                OnFeedbackEnd();
            }
        }

        private void OnFeedbackStart()
        {
            model.enabled = false;
            _collider.enabled = false;

            //if (trail != null)
            //{
            //    trail.Stop();
            //}
        }

        private void OnFeedbackEnd()
        {
            gameObject.SetActive(false);
            model.enabled = true;
            _collider.enabled = true;

            //if (trail != null)
            //{
            //    trail.Play();
            //}
        }

        private void OnDisable()
        {
            transform.localPosition = Vector2.zero + fireOffset;
            transform.rotation = Quaternion.identity;
        }

        public void Shoot(Vector2 direction, float speed)
        {
            //_rb.AddForce(direction * speed * Time.deltaTime);
        }
    }

}