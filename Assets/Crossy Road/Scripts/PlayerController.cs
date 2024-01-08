using System;
using UnityEngine;
using System.Collections;


public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public float          moveDistance      = 1;
    public float          moveTime          = 0.4f;
    public float          colliderDistCheck = 1;
    public bool           isIdle            = true;
    public bool           isDead            = false;
    public bool           isMoving          = false;
    public bool           isJumping         = false;
    public bool           jumpStart         = false;
    public ParticleSystem particle          = null;
    public GameObject     chick             = null;
    public Renderer      renderer          ;
    private bool          isVisible         = false;
    public GameObject hat;
    public Joystick joystick;
    public GameObject glass;
    private bool isCanMove=true;
    private bool isJuumping;
    public AudioClip audioIdle1     = null;
    public AudioClip audioIdle2     = null;
    public AudioClip audioHop       = null;
    public AudioClip audioHit       = null;
    public AudioClip audioSplash    = null;

    public ParticleSystem splash = null;
    public bool parentedToObject = false;

    private void Awake()
    {
        CheckBuy();
        instance = this;
        Application.targetFrameRate = 60;
    }
    void Start ()
    {
        if ( !renderer)
        renderer = chick.GetComponent<Renderer> ();

    }
    public void CheckBuy()
    {
        if (PlayerPrefs.HasKey("Buy1"))
        {
            glass.SetActive(true);
        }
        if (PlayerPrefs.HasKey("Buy2"))
        {
            hat.SetActive(true);
        }
    }
    void Update ()
    {
        if ( !Manager.instance.CanPlay () ) return;

        if ( isDead ) return;
        
        CanIdle ();

        CanMove ();
        if (!isCanMove)
        {
            if (joystick.Vertical==0f || joystick.Horizontal==0f)
            {
                isCanMove = true;
            }
        }
        IsVisible ();
    }
    public Vector3 CheckPosition()
    {
        return transform.position;
    }
    void CanIdle ()
    {
        if ( isIdle )
        {
            if ( Input.GetKeyDown ( KeyCode.UpArrow    ) || (joystick.Vertical >= 0.9f && isCanMove)) { CheckIfIdle ( 270,   0, 0 ); }
            if ( Input.GetKeyDown ( KeyCode.DownArrow  ) || (joystick.Vertical <= -0.9f && isCanMove)) { CheckIfIdle ( 270, 180, 0 ); }
            if ( Input.GetKeyDown ( KeyCode.LeftArrow  ) || (joystick.Horizontal <= -0.9f && isCanMove)) { CheckIfIdle ( 270, -90, 0 ); }
            if ( Input.GetKeyDown ( KeyCode.RightArrow ) || (joystick.Horizontal >= 0.9f && isCanMove)) { CheckIfIdle ( 270,  90, 0 ); }
        }
    }
    void CheckIfIdle ( float x, float y, float z )
    {
        chick.transform.rotation = Quaternion.Euler ( x, y, z );
        
        CheckIfCanMove ();

        int a = UnityEngine.Random.Range ( 0, 12 );
        if ( a < 4 ) PlayAudioClip ( audioIdle1 );
    }
    void CheckIfCanMove ()
    {
        RaycastHit hit;

        Physics.Raycast ( this.transform.position, -chick.transform.up, out hit, colliderDistCheck );

        Debug.DrawRay ( this.transform.position, -chick.transform.up * colliderDistCheck, Color.red, 2 );

        if ( hit.collider == null )
        {
            SetMove ();
        }
        else
        {
            if ( hit.collider.tag == "collider" )
            {
                Debug.Log ( "Hit something with collider tag." );

                isIdle = true;
            }
            else
            {
                SetMove ();
            }
        }
    }
    void SetMove ()
    {
        Debug.Log ( "Hit nothing. Keep moving." );

        isIdle = false;
        isMoving = true;
        jumpStart = true;
    }
    void CanMove ()
    {
        if ( isMoving )
        {
                 if ( Input.GetKeyUp ( KeyCode.UpArrow    ) || (joystick.Vertical >= 0.8f && isCanMove)) { Moving ( new Vector3 ( transform.position.x, transform.position.y, transform.position.z + moveDistance ) ); SetMoveForwardState (); }
            else if ( Input.GetKeyUp ( KeyCode.DownArrow  ) || (joystick.Vertical <= -0.8f && isCanMove)) { Moving ( new Vector3 ( transform.position.x, transform.position.y, transform.position.z - moveDistance ) ); }
            else if ( Input.GetKeyUp ( KeyCode.LeftArrow  ) || (joystick.Horizontal <= -0.8f && isCanMove)) { Moving ( new Vector3 ( transform.position.x - moveDistance, transform.position.y, transform.position.z ) ); }
            else if ( Input.GetKeyUp ( KeyCode.RightArrow ) || (joystick.Horizontal >= 0.8f && isCanMove)) { Moving ( new Vector3 ( transform.position.x + moveDistance, transform.position.y, transform.position.z ) ); }
        }
    }
    void Moving ( Vector3 pos )
    {
        isIdle = false;
        isMoving = false;
        isJumping = true;
        jumpStart = false;

        PlayAudioClip ( audioHop );

        LeanTween.move ( this.gameObject, pos, moveTime ).setOnComplete ( MoveComplete );
    }
    void MoveComplete ()
    {
        isJumping = false;
        isIdle = true;
        isCanMove = false;
        int a = UnityEngine.Random.Range ( 0, 12 );
        if ( a < 4 ) PlayAudioClip ( audioIdle2 );
    }

    void SetMoveForwardState ()
    {
        Manager.instance.UpdateDistanceCount ();
    }
    void IsVisible ()
    {
        if ( renderer.isVisible )
        {
            isVisible = true;
        }

        if ( !renderer.isVisible && isVisible )
        {
            Debug.Log ( "Player off screen. Apply GotHit()" );

            GotHit ();
        }

    }
    public void GotHit ()
    {
        isDead = true;
        ParticleSystem.EmissionModule em = particle.emission;
        em.enabled = true;

        PlayAudioClip ( audioHit );
        GetComponent<BoxCollider>().enabled = false;
        Manager.instance.GameOver ();
    }

    public void GotSoaked ()
    {
        isDead = true;
        ParticleSystem.EmissionModule em = splash.emission;
        em.enabled = true;

        PlayAudioClip ( audioSplash );

        chick.SetActive ( false );

        Manager.instance.GameOver ();
    }

    void PlayAudioClip ( AudioClip clip )
    {
        this.GetComponent<AudioSource> ().PlayOneShot ( clip );
    }
}
