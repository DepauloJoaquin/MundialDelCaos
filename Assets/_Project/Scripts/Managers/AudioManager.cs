using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Instancia estática para el patrón Singleton
    public static AudioManager Instancia { get; private set; }

    [Header("Fuentes de Audio")]
    [SerializeField] private AudioSource fuenteEfectos; // Para sonidos cortos (patada, gol, etc.)
    [SerializeField] private AudioSource fuenteMusica;  // Solo para la música de fondo

    [Header("Música del Juego")]
    [SerializeField] private AudioClip musicaMenuPrincipal; 
    [SerializeField] private AudioClip musicaPartida;

    [Header("Efectos de Sonido")]
    [SerializeField] private AudioClip sonidoPatada;
    [SerializeField] private AudioClip sonidoGol;
    [SerializeField] private AudioClip sonidoRebote;
    [SerializeField] private AudioClip sonidoBarrida;

    private void Awake()
    {
        // Configuración del Singleton
        if (Instancia == null)
        {
            Instancia = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // la musicap se asigna en el inspecto por ahora.
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

    public void ActivarMusicaMenu()
    {
        ReproducirMusica(musicaMenuPrincipal);
    }
    public void ActivarMusicaPartida() 
    {
        ReproducirMusica(musicaPartida); 
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

    public void ReproducirRebote()
    {
        ReproducirSonido(sonidoRebote);
    }

    public void ReproducirBarrida()
    {
        ReproducirSonido(sonidoBarrida);
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