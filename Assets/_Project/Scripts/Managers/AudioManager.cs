using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Instancia estática para el patrón Singleton
    public static AudioManager Instancia { get; private set; }

    [Header("Fuentes de Audio")]
    [SerializeField] private AudioSource fuenteEfectos; // Para sonidos cortos (patada, gol, etc.)
    [SerializeField] private AudioSource fuenteMusica;  // Solo para la música de fondo

    [Header("Música del Juego")]
    [SerializeField] private AudioClip musicaMenuPrincipal; //
    [SerializeField] private AudioClip PublicoHablando;//
    [SerializeField] private AudioClip PublicoCantando; //

    [Header("Efectos de Sonido")]
    [SerializeField] private AudioClip sonidoPatada;//-
    [SerializeField] private AudioClip sonidoGol;//-
    //[SerializeField] private AudioClip sonidoRebote;//-
    [SerializeField] private AudioClip sonidoBarrida;//
    [SerializeField] private AudioClip sonidoPase;//-
    [SerializeField] private AudioClip sonidoSilvatoInicial;//-
    [SerializeField] private AudioClip sonidoSilvatoFinal;
    [SerializeField] private AudioClip sonidoActivarHabilidad;//-
    [SerializeField] private AudioClip sonidoBotonMenu;//-
    [SerializeField] private AudioClip sonidoPantallaFinalVictoria;//-
    [SerializeField] private AudioClip sonidoCuentaRegresiva;//-
   

    private void Awake()
    {
        // Configuración del Singleton
        if (Instancia == null)
        {
            Instancia = this;
            DontDestroyOnLoad(gameObject); // Evita que se destruya al cambiar de escena
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Llama automáticamente a la música del menú en cuanto arranca el juego
        ActivarMusicaMenu();
    }

    // Métodos para reproducir música
    public void ReproducirMusica(AudioClip cancion, bool reproducirEnBucle = true)
    {
        if (fuenteMusica == null || cancion == null) return;

        // Si ya está sonando esa misma canción, no la reinicies
        if (fuenteMusica.clip == cancion && fuenteMusica.isPlaying) return;

        fuenteMusica.clip = cancion;
        fuenteMusica.loop = reproducirEnBucle;
        fuenteMusica.Play();
    }

    public void DetenerMusica()
    {
        if (fuenteMusica != null) fuenteMusica.Stop();
    }
    public void PausarMusica()
    {
        if (fuenteMusica != null && fuenteMusica.isPlaying)
        {
            fuenteMusica.Pause();
        }
    }

    public void ReanudarMusica()
    {
        if (fuenteMusica != null && !fuenteMusica.isPlaying)
        {
            fuenteMusica.UnPause(); 
        }
    }

    public void ActivarMusicaMenu()
    {
        ReproducirMusica(musicaMenuPrincipal);
    }
    public void ActivarMusicaPartida() 
    {
        ReproducirMusica(PublicoHablando); 
    }

    //metodos para reproducir cada sonido específico
    public void ReproducirPatada()
    {
        ReproducirSonido(sonidoPatada);
    }

    public void ReproducirGol()
    {
        ReproducirSonido(sonidoGol);
    }

    public void ReproducirSilvatoInicial()
    {
        ReproducirSonido(sonidoSilvatoInicial);
    }
    public void ReproducirSilvatoFinal()
    {
        ReproducirSonido(sonidoSilvatoFinal);
    }
    public void ReproducirActivarHabilidad()
    {
        ReproducirSonido(sonidoActivarHabilidad);
    }
    public void ReproducirBotonMenu()
    {
        ReproducirSonido(sonidoBotonMenu);
    }
    public void ReproducirPantallaFinalVictoria()
    {
        ReproducirSonido(sonidoPantallaFinalVictoria);
    }


    public void ReproducirBarrida()
    {
        ReproducirSonido(sonidoBarrida);
    }

    public void ReproducirPase()
    {
        ReproducirSonido(sonidoPase);
    }

    public void ReproducirCuentaRegresiva()
    {
        ReproducirSonido(sonidoCuentaRegresiva);
    }

    // Método privado interno para procesar la reproducción sin repetir código
    private void ReproducirSonido(AudioClip clip)
    {
        if (clip != null && fuenteEfectos != null)
        {
            // PlayOneShot permite que los efectos se superpongan si ocurren al mismo tiempo
            fuenteEfectos.PlayOneShot(clip);
        }
        else if (clip == null)
        {
            Debug.LogWarning("Se intentó reproducir un sonido, pero el archivo de audio (AudioClip) está vacío.");
        }
    }
}