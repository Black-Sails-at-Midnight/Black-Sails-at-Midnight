using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Gun : Attack
{
    [System.Serializable]
    public class GunSettings 
    {
        [Min(0)]
        public float RPM = 60;

        [Min(1)]
        public int palletsPerShot = 1;

        [Range(0, 90)]
        public float pelletSpread = 0;

        [Min(0)]
        public int clipSize = 1;

        [Min(0)]
        public float reloadTime = 2;

        [Min(0)]
        public float bulletVelocity = 150;
    }

    [SerializeField]
    public GunSettings gunSettings;

    [SerializeField]
    public GameObject bulletPrefab;

    [SerializeField]
    public KeyCode reloadKey = KeyCode.R;

    [SerializeField]
    public float muzzleLength = 2.5f;

    [Header("VFX")]
    [SerializeField]
    public ParticleSystem muzzleFlash;


    [Header("SFX")]
    [SerializeField]
    public AudioClip shootAudio;

    [SerializeField]
    [Range(-3, 3)]
    public float lowerShotPitchRange = 1;

    [SerializeField]
    [Range(-3, 3)]
    public float upperShotPitchRange = 1;

    [SerializeField]
    public AudioClip reloadAudio;
    
    
    Vector3 DefaultPosition;
    Quaternion DefaultRotation;

    public int clip {get; private set;}
    private bool isReloading = false;
    public bool IsReloading { get { return isReloading; } }
    
    private Animator animator;
    private AudioSource audioSource;

    // Monobehaviour Methods
    public void Start()
    {
        clip = gunSettings.clipSize;
        DefaultPosition = new Vector3(1, -0.25f, -0.5f);
        DefaultRotation = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w);
    }

    public void OnEnable() 
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        if (DefaultPosition != Vector3.zero && DefaultRotation != Quaternion.identity)
        {
            transform.localPosition = DefaultPosition;
            transform.localRotation = DefaultRotation;
        }
        StartCoroutine(ShootHandler());
    }

    public void OnDisable() {
        isReloading = false;
    }

    // Public Methods

    // Private Methods
    private IEnumerator ShootHandler()
    {
        while(true)
        {
            if (Input.GetButton("Fire1") && clip > 0 && !isReloading && attackSettings.canAttack)
            {
                animator.SetTrigger("Fired");

                for (int i = 0; i < gunSettings.palletsPerShot; i++)
                {
                    Vector3 randomVector = 
                                Quaternion.AngleAxis(Random.Range(-gunSettings.pelletSpread, gunSettings.pelletSpread), Vector3.Cross((transform.forward).normalized, Vector3.up)) * (transform.forward).normalized +
                                Quaternion.AngleAxis(Random.Range(-gunSettings.pelletSpread, gunSettings.pelletSpread), Vector3.Cross((transform.forward).normalized, Vector3.right)) * (transform.forward).normalized;

                    GameObject _bullet = Instantiate(bulletPrefab, transform.position + ((transform.forward.normalized) * muzzleLength), transform.rotation);
                    _bullet.GetComponent<Rigidbody>().AddForce(randomVector.normalized * gunSettings.bulletVelocity, ForceMode.Impulse);
                    _bullet.GetComponent<Rigidbody>().AddForce(FindObjectOfType<PlayerMovement>().GetComponent<Rigidbody>().velocity, ForceMode.Impulse);
                    _bullet.GetComponent<Bullet>().ShotBy = this;
                }

                ParticleSystem _muzzleFlash = Instantiate(muzzleFlash, transform.position + ((transform.forward.normalized) * muzzleLength), transform.rotation);
                Destroy(_muzzleFlash.gameObject, 2.5f);

                audioSource.pitch = Random.Range(lowerShotPitchRange, upperShotPitchRange);
                audioSource.PlayOneShot(shootAudio, 1);
                clip--;

                if (clip > 0)
                    yield return new WaitForSeconds(60 / gunSettings.RPM);                
            }

            if ((clip == 0 || Input.GetKey(reloadKey)) && !isReloading && attackSettings.canAttack)
            {
                StartCoroutine(Reload());
                isReloading = true;
            }

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator Reload()
    {
        animator.SetBool("Reload", true);
        animator.SetBool("IsScopedIn", false);

        yield return new WaitForSeconds(gunSettings.reloadTime);
        animator.SetBool("Reload", false);

        clip = gunSettings.clipSize;
        isReloading = false;
    }
}
