# Budget App
> Application web de gestion de budget et de comparaison des prix entre magasins.

BudgetApp permet de saisir ses tickets de caisse, de suivre ses dépenses mois par mois et de comparer le prix d'un même article d'un magasin à un autre.
L'application est conçue pour un usage personnel, sans publicité.

Projet réalisé dans le cadre d'un **TPI (Travail Pratique Individuel) – certification IDEC 2026**.

---

## Fonctionnalités

- **Tickets** – saisie d'un ticket (date + magasin), ajout/modification/suppression de lignes d'achats,
filtrage par mois et par année, version imprimable.
- **Articles** – gestion des produits (nom, unité, catégorie), recherche, filtrage par catégorie, historique des prix par article
avec statistique (min, max, moyen). Création d'un article « à la volée » pendant la saisie d'un ticket.
- **Magasins** – chaque magasin est identifié par son nom et sa localité; création possible à la volée, avec contrôle anti-doublon.
- **Catégories** – 14 catégories prédéfinies (fruits et légumes, boulangerie, produits laitiers, viandes, surgelés, conserves, pâtes et riz, épices, snacks, boissons, papeterie, cosmétiques, entretien, autres).
- **Vue d'ensemble** — page d'accueil avec le total des dépenses, le nombre de tickets, le nombre d'articles et les 10 derniers achats.
- **Graphique** — évolution des dépenses par mois sous forme de graphique en barres interactif (Chart.js).

---

## Stack technique

| Technologies  | Rôle |
| :--- | :--- |
| **ASP.NET Core MVC(.NET 10)** | Framework web – structure MVC |
| **C#** | Langage orienté objet |
| **MS SQL Server Express** | Base de données relationnelle locale |
| **Entity Framework Core** | ORM - accès aux données (Database First) |
| **Bootstrap 5** | Mise en page et style de l'interface |
| **Font Awesome** | Icônes de l'interface |
| **Chart.js** | Graphique d'évolution des dépenses |
| **User Secrets** | Stockage sécurisé de la chaîne de connexion (hors code source) |

---

## Prérequis

- Windows 10 ou supérieur
- [.NET 10 SDK / Runtime (ASP.NET Core)](https://dotnet.microsoft.com/)
- Microsoft SQL Server Express (instance `.\SQLEXPRESS`)
- Visual Studio 2026 (ou VS Code avec les outils EF Core)

---

