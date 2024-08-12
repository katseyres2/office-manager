# PGBD-Project
## Description
La société FlexiWorkspace est une jeune startup, localisée en Région Wallonne, qui souhaite lancer
un service de location de bureaux inoccupés.
Son idée part de deux constats.

- L’activité économique se dynamise de plus en plus. De toutes petites entreprises et startups
se créent et ont besoin de trouver des locaux pour pouvoir être rapidement opérationnelles.
- D’un autre côté, avec le développement du télétravail structurel, de nombreuses entreprises
ou organisations disposent de bureaux inoccupés. Certaines d’entre elles pourraient être
intéressées de valoriser ces espaces, en les louant, si elles pouvaient être déchargées de la
gestion locative qui en découle.

FlexiWorkspace veut donc se positionner comme un intermédiaire entre les organisations qui
disposent de locaux à louer et celles qui ont besoin de locaux. Pour démarrer son projet, la société à
besoin de vos services pour mettre en place une base de données relationnelle associée à un logiciel
de gestion adapté.

Le cœur du métier de FlexiWorkspace est la mise en correspondance de données. La première
version de la base de données devra permettre de gérer au minimum :
- Les entreprises ou organisations (nom, adresse, …) qui font appel aux services de
FlexiWorkspace. Il peut s’agir soit de propriétaires qui souhaitent mettre en location des
espaces de travail, soit de locataires.
- Les locaux disponibles à la location. Chaque local est caractérisé par : un propriétaire, une
adresse, une surface, une description, un montant de loyer mensuel, un type (meublé ou non
meublé), …
- Les contrats de location. On loue un bureau ou un local donné pendant une période donnée.

Le personnel de FlexiWorkspace devra disposer d’une application de gestion, de type WPF, utilisable
sur les postes de travail du réseau local de l’entreprise et articulée autour d’une base de données
relationnelle implémentée sous SQL Serveur (voir contraintes techniques).
Avant de démarcher les futurs locataires, la première priorité de FlexiWorkspace sera de prospecter
les entreprises en vue de constituer une réserve de bureaux à louer. La première version de
l’application devra donc prendre en charge les fonctionnalités suivantes :
- Afficher la liste des entreprises.
- Ajouter, modifier et supprimer une entreprise. Une entreprise qui a déjà proposé des
bureaux en location ou qui est locataire ne pourra pas être supprimée mais uniquement
désactivée.
- Gérer les locaux mis en location par une entreprise donnée.
- Permettre l’encodage des premières locations.
Une attention particulière devra être donnée à la conception de l’interface utilisateur, à son
ergonomie et à sa facilité d’utilisation.

## Appropriation du projet (personnalisation)
L’énoncé ci-dessus trace les lignes directrices du projet. Certains points sont restés volontairement
flous pour vous permettre une interprétation en termes de fonctionnalités ainsi qu’une
personnalisation de l’application.

A titre d’exemple, des améliorations possibles pourraient être :
- La gestion des équipements de bureau disponibles.
- Permettre à l’utilisateur de pouvoir filtrer les locaux disponibles sur base de critères.
- ...

Le schéma de la base de données devra être conçu en fonction de votre appropriation de l’énoncé
et des fonctionnalités spécifiques que vous souhaitez développer.

## Contraintes techniques
Le projet devra respecter les contraintes techniques suivantes :
- L'application de gestion sera implémentée en C# et utilisera le framework .NET 6.
- L’interface utilisateur sera développée avec la technologie WPF.
- Pour l’accès aux données, l’application utilisera l’Entity Framework comme ORM.
- L’application mettra en œuvre une architecture en couches dans le but de séparer trois éléments : l’interface utilisateur (UI), la logique business et la logique d’accès aux données.
- L’application sera dotée d’un système adapté de gestion des erreurs et des exceptions. On veillera également à mettre en pratique les « bonne pratiques de développement » vues au cours ainsi que les techniques permettant d'améliorer la robustesse du code.
- Le code sera documenté de façon judicieuse via le système de « xml comments ».
- La base de données sera implémentée sous SQL Serveur (version >= 2012). Elle sera conçue pour garantir au maximum la consistance et l’intégrité des données par une utilisation appropriée des : types de données, clauses nullables, clés primaires et clés étrangères.
- La base de données sera alimentée avec un jeu de données de test significatif (min 50 records dans les tables principales) et reprenant des données réalistes (pas de Lorem Ipsum). Pour ce faire, il est, par exemple, possible d’utiliser des générateurs de données tels que : http://www.generatedata.com.














