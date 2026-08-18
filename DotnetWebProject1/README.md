# SmartHome Sentinel

Application web de supervision de capteurs domestiques (IoT) avec détection automatique d'anomalies et diffusion des alertes en temps réel.

## Contexte

Ce projet simule un environnement de maison connectée avec 4 types de capteurs (température, consommation électrique, humidité, latence réseau). Chaque nouvelle mesure est analysée automatiquement : si une valeur sort de sa plage normale, une anomalie est détectée, enregistrée et diffusée instantanément à un dashboard web, sans rechargement de page.

## Fonctionnalités

- Simulation de télémétrie en arrière-plan (nouvelle mesure toutes les ~3 secondes)
- Détection d'anomalie basée sur des règles métier par type de capteur
- Diffusion temps réel des lectures et des anomalies via SignalR
- Dashboard web avec graphique multi-capteurs en direct (Chart.js) et tableau de bord (KnockoutJS)
- API REST documentée avec Swagger
- Tests unitaires xUnit sur la logique de détection
- Environnement PostgreSQL prêt à l'emploi via Docker Compose

## Stack technique

| Catégorie | Technologies |
|---|---|
| Backend | ASP.NET Core, Entity Framework Core |
| Base de données | PostgreSQL |
| Temps réel | SignalR |
| Frontend | ASP.NET MVC (Razor), KnockoutJS, Chart.js |
| Tests | xUnit |
| Infrastructure | Docker, Docker Compose |

## Architecture

```
Simulateur (BackgroundService)
        │
        ▼
   API REST (Contrôleurs)
        │
   ┌────┴────┐
   ▼         ▼
Détection   SignalR Hub
anomalie        │
   │            ▼
   ▼        Dashboard web
Base de       (temps réel)
données
(PostgreSQL)
```

Chaque mesure générée est analysée par un service de détection indépendant (basé sur des seuils par type de capteur), puis stockée en base. Toute anomalie détectée est immédiatement poussée aux clients connectés via SignalR, sans passer par un rechargement de page ou du polling.

## Lancer le projet

### Prérequis
- .NET 8 SDK
- Docker Desktop (pour PostgreSQL) — ou une instance PostgreSQL locale déjà installée

### Étapes

1. Cloner le dépôt
```bash
git clone https://github.com/TON-PSEUDO/smarthome-sentinel.git
cd smarthome-sentinel
```

2. Démarrer PostgreSQL via Docker
```bash
docker-compose up -d
```

> **Note** : les identifiants PostgreSQL par défaut (`postgres` / `postgres`) sont définis dans `docker-compose.yml` et repris dans `appsettings.json`. Ce sont des identifiants de démonstration à usage local uniquement — à ne jamais utiliser tels quels dans un environnement de production.

3. Appliquer les migrations de base de données
```bash
dotnet ef database update
```

4. Lancer l'application
```bash
dotnet run
```

5. Ouvrir le dashboard dans le navigateur
```
https://localhost:7073
```

L'API REST documentée est accessible sur `https://localhost:7073/swagger`.

### Lancer les tests
```bash
dotnet test
```

## Endpoints principaux

| Méthode | Route | Description |
|---|---|---|
| GET | `/api/sensors` | Liste des capteurs |
| GET | `/api/readings/{sensorId}` | Historique d'un capteur |
| GET | `/api/readings/latest` | Dernières lectures tous capteurs confondus |
| GET | `/api/anomalies` | Liste des anomalies détectées |

## Captures d'écran

*(à ajouter : capture du dashboard en direct, capture Swagger)*

## Pistes d'amélioration

- Détection d'anomalie par méthode statistique (z-score) ou par apprentissage automatique (Isolation Forest / scikit-learn) en complément des seuils fixes actuels
- Authentification et gestion multi-utilisateurs
- Historique et export des données sur une période donnée
