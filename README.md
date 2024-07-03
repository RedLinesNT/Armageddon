# Project ARMAGEDDON (French README)

<hr>


### Le projet a été testé sur (Editeur):
  - OS: Ubuntu 24.04 LTS (64-bits)<br/>
  - IDE: JetBrains Rider<br/>
  - Rendering API: OpenGL Core 4.5<br/>

 
### Problèmes connus:
 - La caméra 3ème personne est plus que abjecte.
 - Aucun rendu 3D dans l'éditeur si l'API de rendu est VULKAN.
 - Il serait préférable de "tweaker" le Physics Material du joueur, les collisions sont bof...

<hr>

## Table des matières

<!--ts-->
   * [Programmes requis](#programmes-requis)
   * [Packages du projet](#packages-du-projet)
   * [Norme de programmation](#norme-de-programmation)
<!--te-->

<hr>

## Programmes requis
  - Unity Hub
  - Unity Engine (2023.1.19f1)

Les programmeurs auront besoin d'avoir un "IDE" de leur choix. Ces deux IDE sont recommandés :
  - Visual Studio Community 2019/2022
  - JetBrains Rider 2023

### Vous ne possedez pas la version d'Unity mentionné au dessus ?
  Pour installer la version du moteur demandée avec Unity Hub, rendez-vous sur [cette page d'Unity](https://unity.com/releases/editor/whats-new/2023.1.19).<br>
  En haut de cette page, cliquez sur "<i>Install this version with Unity Hub</i>".

<hr>

## Packages du projet

Voici la liste des packages installés sur ce projet :
 - Burst (1.8.16)
 - Cinemachine (2.9.7)
 - Core RP Library (15.0.7)
 - Custom NUnit (2.0.3)
 - Input System (1.7.0)
 - JetBrains Rider Editor (3.0.28)
 - Mathematics (1.2.6)
 - Searcher (4.9.2)
 - Ssettings Manager (2.0.1)
 - Shader Graph (15.0.7)
 - Sysroot Base (2.0.10)
 - Sysroot Linux x64 (2.0.9)
 - Test Framework (1.3.9)
 - TextMeshPro (3.0.9)
 - Toolchain Linux x64 (2.0.9)
 - Unity UI (1.0.0)
 - Universal Render Pipeline (15.0.7)

<hr>

## Norme de programmation

  Class :
    ```
    CamelCase
    ```<br>
  Attributs :
    ```
    camelCase
    ```<br>
  Variables :
    ```
    _camelCase
    ```<br>
  Méthodes :
    ```
    CamelCase()
    ```<br>
  Enums :
    ```
    ENameOfEnum
    ```<br>
  Valeur d'un Enum :
    ```
    VALUE
    ```<br><br>
L'entièreté des attributs doivent être en private. Utilisez à la place des <i>Properties</i> ou des <i>Getters/Setters</i>.<br>
<strong>Tous les noms doivent être de préférence en anglais!</strong>

### Commentaires
  Afin que votre code puisse être comprit par le reste de l'équipe, il est important de commenter votre code.<br>
  Commentez votre code avec des "<i>//</i>" pour les attributs et les variables et des passages de code si nécessaire!<br>
  Commentez vos "<i>methods</i>" avec des ```<summary>```.

<hr>

Le projet à <i>3'199</i> lignes de code (C#)

<hr>  
