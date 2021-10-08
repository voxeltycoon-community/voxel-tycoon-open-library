<a href="https://voxeltycoon.xyz/">
	<img src="https://cdn.discordapp.com/emojis/507727751801864192.png?size=64" alt="The Voxel Tycoon Thinking Logo" title="Voxel Tycoon Think Logo" align="right" width="64px" />
</a>

<h1 align="center">Voxel Tycoon Open Library (VTOL)</h1>

<p align="center">
	<a href="https://github.com/victorleblais/voxel-tycoon-open-library/releases">
		<img src="https://img.shields.io/github/downloads/victorleblais/voxel-tycoon-open-library/total?color=success&label=Downloads"/ alt="The GitHub Downloads">
	</a>
	<a href="https://github.com/victorleblais/voxel-tycoon-open-library/blob/main/LICENSE">
		<img src="https://img.shields.io/github/license/victorleblais/voxel-tycoon-open-library?color=informational&label=License"/ alt="The GitHub License">
	</a>
</p>

<p align="center">
A community-made open-source library containing functions to assist in
developing mods for the game <a href="https://voxeltycoon.xyz/">Voxel Tycoon</a>.
</p>

<details open><summary><b>Table of Contents</b></summary>

- [Getting Started](#getting-started)
  - [⭐️ Using the library](#using-the-library)
  - [Contributing Prerequisites](#contributing-prerequisites)
    - [Code Editor](#code-editor)
	- [Voxel Tycoon (recommended)](#voxel-tycoon-recommended)
	- [GitHub Desktop (advised)](#github-desktop-advised)
  - [Installation](#installation)
- [Running Tests](#running-tests)
  - [Unit Tests](#unit-tests)
  - [Integration Tests](#integration-tests)
  - [Mutation Tests](#mutation-tests)
- [Deployment](#deployment)
- [Contributing](#contributing)
- [Authors](#authors)
- [License](#license)
- [Built with](#build-with)
- [Acknowledgments](#acknowledgments)
</details>

## Getting Started

### ⭐️ Using the library

__**TODO: Fill in how to use the library after our first deployment.**__

### Contributing Prerequisites

This game has been written in C# as a mod for Voxel Tycoon (made in Unity),
therefore, we expect every contributor to have C# and Voxel Tycoon installed on
their device before they can continue.

#### Code Editor

In order to contribute to this repository, you will need an editor which
supports writing code in C#. A couple of commonly used
[IDE](https://en.wikipedia.org/wiki/Integrated_development_environment)s are:

- [Microsoft Visual Studio](https://visualstudio.microsoft.com/downloads/)
- [Microsoft Visual Studio Code](https://code.visualstudio.com/download)
- [Jet Brains Rider](https://www.jetbrains.com/rider/)

However, for this project we will be using *Visual Studio (Community 2019)*
as proposed by the [Voxel Tycoon Tutorial](https://docs.voxeltycoon.xyz/guides/script-mods/creating-your-first-script-mod/).
Using a newer version won't give problems when writing code, however, it might
give problems when you try to help someone out in another version or editor.

In the installer, you will be asked to specify workloads. These are extra
components to help enhance development in a variety of different branches.
For this project you should select *.NET desktop development*. Within the
list of workloads there is also an option for *Game Development with Unity*,
however, we will not pick this one, since we will be using the Unity code
provided with the Voxel Tycoon installation.

After you select this component, then you can continue following the installer
and you will be done with the Visual Studio environment installation.

#### Voxel Tycoon (recommended)

Since this library is made as a mod for [Voxel Tycoon](https://voxeltycoon.xyz/),
we do expect all contributors to have a copy of the game which they can use for
testing purposes. If you do not own the game then you can buy it on
[Steam](https://store.steampowered.com/app/732050/Voxel_Tycoon/).

#### GitHub Desktop (advised)

Since you are reading this you most likely have a GitHub account already.
So to help with uploading your changes we advise to make use of
[GitHub Desktop](https://desktop.github.com/), a Git GUI made by GitHub.

![Github Desktop Image](https://desktop.github.com/images/github-desktop-screenshot-windows.png)

### Installation

Once you have downloaded all the environments and required software, then you
can install the project. To do this you can either use the git console or use
the GitHub Desktop app. For the console approach you can follow the
[provided tutorial](https://help.github.com/en/github/creating-cloning-and-archiving-repositories/cloning-a-repository).
With GitHub desktop it is as easy as clicking in the top left **file** and then
**clone repository**. In this menu you can now see all the repositories which
you have access to on GitHub, from here you can select the
**Voxel Tycoon Open Library** repository and download it.

Next we need to setup a user variable to point to our Voxel Tycoon game.
The reason for this variable is so that all contributors can install Steam
or Voxel Tycoon in a different folder and still being able to contribute.
To add the variable you need to navigate to [the environmental variables](https://docs.oracle.com/en/database/oracle/machine-learning/oml4r/1.5.1/oread/creating-and-modifying-environment-variables-on-windows.html).
On Windows 10 this can be reached as follows:

1. Right click on the Windows icon in the bottom left of your task bar.
2. In this menu, select "System".
3. Within the system info, there will be an extra menu on the right,
   from this menu select the "Advanced system settings" text.
4. A new window will pop-up for the System properties,
   in here select the "Environment Variables..." button at the bottom.
5. This menu will have 2 areas: User variables and System variables.
   The first one is only for your account and the second one for everyone
   using this PC. We will add our variable to the user variant in the top
   by pressing the "New..." button.
6. The name of our variable will be `VoxelTycoonInstallationDir` and the value
   is the directory of your Voxel Tycoon game folder. The game folder is the one
   containing the `VoxelTycoon.exe` and the important `VoxelTycoon_Data` folder.
7. When that is all set, make sure to press **Ok** in all windows and that will
   automatically save your settings.

To test if the installation was a success, you can now open Visual Studio by
double click the **Voxel Tycoon Open Library.sln** solution inside your
downloaded project folder. If no errors pop up than the installation has been
successful!

From now on you can use GitHub Desktop or your favourite Git system to commit
new changes which you've made to your feature specific branch on GitHub.

## Running Tests

__**TODO: Fill in testing chapter when we have a testing library.**__

### Unit Tests

### Integration Tests

### Mutation Tests

## Deployment

__**TODO: Fill in deployment when we have the CD active.**__

## Contributing

Interested in helping us make an amazing public library? Go ahead and fork
this project so you can make a [Pull Request](https://docs.github.com/en/github/collaborating-with-pull-requests/proposing-changes-to-your-work-with-pull-requests/creating-a-pull-request-from-a-fork)
or contact us on the [Voxel Tycoon Discord Server](https://discord.gg/voxeltycoon)!

## Authors

- <span>![Refreshfr pfp](https://cdn.discordapp.com/avatars/68831516825759744/b8aaa97ab6fad21ace52ef73de104cf5.webp?size=16)</span> <span>[Refreshfr](https://github.com/victorleblais)</span>
- <span>![Personal_Builder pfp](https://cdn.discordapp.com/avatars/104632306676809728/031723c293aa44c81d7ae42f4e54de8e.webp?size=16)</span> <span>[Personal_Builder](https://github.com/kevin4998)</span>
- <span>![Nebruzias pfp](https://cdn.discordapp.com/avatars/252190977207435266/a2b227048295861260c61f9816f4ca5a.webp?size=16)</span> <span>[Nebruzias](https://github.com/evertn)</span>
- <span>![KyleRokuKyu pfp](https://cdn.discordapp.com/avatars/119493641944170496/c2d7b2fbfa05bc0e197b7aa281caac32.webp?size=16)</span> <span>[KyleRokuKyu](https://github.com/KyleRokuKyu)</span>
- <span><img src="https://discord.com/assets/6f26ddd1bf59740c536d2274bb834a05.png" alt="Xmnovotny pfp" width="16px" /></span> <span>[Xmnovotny](https://github.com/xmnovotny)</span>

## ⚖️ License

<h3 align="center"><a href="https://github.com/victorleblais/voxel-tycoon-open-library/blob/main/LICENSE"><b>GNU Affero General Public License v3.0</b></a></h3>

<p align="center"><b>Summary:</b> (This is not legal advice)</p>
<p align="justify"><i>
Permissions of this strongest copy-left license are conditioned on making
available complete source code of licensed works and modifications, which
include larger works using a licensed work, under the same license. Copyright
and license notices must be preserved. Contributors provide an express grant of
patent rights. When a modified version is used to provide a service over a
network, the complete source code of the modified version must be made available.
</i></p>

| Permissions       | Limitations  | Conditions                     |
| ----------------- | ------------ | ------------------------------ |
| ✔️ Commercial use | ❌ Liability | 🔵 License and copyright notice |
| ✔️ Modification   | ❌ Warranty  | 🔵 State changes                |
| ✔️ Distribution   |              | 🔵 Disclose source              |
| ✔️ Patent use     |              | 🔵 Network use is distribution  |
| ✔️ Private use    |              | 🔵 Same license                 |

## Built with

This project would not have been possible without the following frameworks,
public packages and awesome people.

- [C#](https://docs.microsoft.com/en-us/dotnet/csharp/) - The main programming
  language used to write this library in.
- [Unity](https://unity.com/) - The engine which allows for quick game
  development.
- [Voxel Tycoon](https://voxeltycoon.xyz/) - The wonderful game which inspired
  us to start developing this project.

## Acknowledgments