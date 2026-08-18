using Microsoft.AspNetCore.SignalR;

namespace DotnetWebProject1.Hubs
{
    // Le Hub reste vide : il sert uniquement de canal de diffusion.
    // Toute la logique d'envoi se fait depuis le service ou les contrôleurs.
    public class AnomalyHub : Hub
    {
    }
}