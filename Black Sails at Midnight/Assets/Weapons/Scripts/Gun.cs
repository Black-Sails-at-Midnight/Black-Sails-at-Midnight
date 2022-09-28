using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        public float palletSpread = 0;

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

    [Header("VFX")]
    [SerializeField]
    public ParticleSystem muzzleFlash;

    [Header("SFX")]
    [SerializeField]
    public AudioClip shootAudio;

    [SerializeField]
    public AudioClip reloadAudio;

    private int clip;
    private bool isReloading = false;
    
    private Animator animator;
    private AudioSource audioSource;

    // Monobehaviour Methods
    public void Start()
    {
        clip = gunSettings.clipSize;
    }

    public void OnEnable() 
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        StartCoroutine(ShootHandler());

        Debug.Log("Enabled: " + gameObject.name);
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
            if (Input.GetButton("Fire1") && clip > 0 && !isReloading)
            {
                for (int i = 0; i < gunSettings.palletsPerShot; i++)
                {
                    Vector3 randomVector = 
                                Quaternion.AngleAxis(Random.Range(-gunSettings.palletSpread, gunSettings.palletSpread), Vector3.Cross((transform.forward).normalized, Vector3.up)) * (transform.forward).normalized +
                                Quaternion.AngleAxis(Random.Range(-gunSettings.palletSpread, gunSettings.palletSpread), Vector3.Cross((transform.forward).normalized, Vector3.right)) * (transform.forward).normalized;

                    GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
                    bullet.GetComponent<Rigidbody>().AddForce(randomVector.normalized * gunSettings.bulletVelocity, ForceMode.Impulse);
                    bullet.GetComponent<Bullet>().ShotBy = this;
                }

                audioSource.PlayOneShot(shootAudio);
                clip--;

                if (clip > 0)
                    yield return new WaitForSeconds(60 / gunSettings.RPM);                
            }

            if ((clip == 0 || Input.GetKey(reloadKey)) && !isReloading)
            {
                StartCoroutine(Reload());
                isReloading = true;
            }

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator Reload()
    {
        animator.SetTrigger("Reload");
        audioSource.PlayOneShot(reloadAudio);

        yield return new WaitForSeconds(gunSettings.reloadTime);

        clip = gunSettings.clipSize;
        isReloading = false;
    }
}
