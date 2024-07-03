<div align="center"> <img src="https://github.com/RedLinesNT/Armageddon/blob/main/Visual/Armageddon_Wide.png"> </div>
Simple Tech-Demo with the goal of making a "Golf It" type game in 4 days on Unity (at the same time as my last school week).
I only endorse the code behind it, the gameplay on the other side could be a LOT better.

<hr>


### The project has been tested on (Editor):
  - OS: Ubuntu 24.04 LTS (64-bits)<br/>
  - IDE: JetBrains Rider<br/>
  - Rendering API: OpenGL Core 4.5<br/>

 
### Known issues:
 - The 3rd person camera is beyond disgusting.
 - If the selected rendering API is VULKAN, the editor won't render anything at all.
 - It would be preferable to tweak the player's "Physics Material", as this makes the collisions hideous.

<hr>

## Summary

<!--ts-->
   * [Required programs](#required-programs)
   * [Project packages](#project-packages)
   * [Programming standards](#programming-standards)
<!--te-->

<hr>

## Required programs
  - Unity Hub
  - Unity Engine (2023.1.19f1)

Programmers will need an IDE of their choice, these twos are recommended :
  - Visual Studio Community 2019/2022
  - JetBrains Rider 2023

### Missing the Unity version mentionned above ?
  To install the engine version required with Unity Hub, go to [this page from Unity](https://unity.com/releases/editor/whats-new/2023.1.19).<br>
  And at the top of this page, click on "<i>Install this version with Unity Hub</i>", then Unity Hub will deal with the rest.

<hr>

## Project packages

Here's the list of packages currently installed :
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

## Programming standards

  Class :
    ```
    CamelCase
    ```<br>
  Attributes :
    ```
    camelCase
    ```<br>
  Variables :
    ```
    _camelCase
    ```<br>
  Methods :
    ```
    CamelCase()
    ```<br>
  Enums :
    ```
    ENameOfEnum
    ```<br>
  Enum's Values :
    ```
    VALUE
    ```<br><br>
All attributes must be private, use Properties or Getters/Setters instead.<br/>
Every names/comments MUST be in English, no matter how broken yours is.

<hr>

The project currently have <i>3'199</i> lines of code (C#)

<hr>  
