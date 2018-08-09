# R.O.G.U.E
**R**eal-time **O**pen-source **G**ame **U**sing **E**ntities

## Copyright ##
Copyright (C) 2018 "Daniel Bramblett" <bram4@pdx.edu>, "Daniel Dupriest" <kououken@gmail.com>, "Brandon Goldbeck" <bpg@pdx.edu>

## Contact Information ##
daniel.r.bramblett@gmail.com
bram4@pdx.edu
kououken@gmail.com
bpg@pdx.edu

## Bug Tracker ##
https://github.com/bgoldbeck/rogue/issues

## What is this repository for? ##

* This repository contains the term project for CS461P Open Source Software development with professor Bart Massey.

## Setting up for Development ##

### Windows ###

The software is set up as a Visual Studio 2017 solution.

1. Clone the project. `git clone https://github.com/bgoldbeck/rogue.git`
2. Download and install Visual Studio 2017. [Visual Studio 2017](https://visualstudio.microsoft.com/downloads/)
3. Download and install .NET Core app SDK 2.1 from Microsoft. [.NET Core SDK 2.1](https://www.microsoft.com/net/download/dotnet-core/sdk-2.1.300)
4. Open the solution `.sln` file in Visual Studio 2017.
5. Run the project with `F5`.

### Linux ###

1. Clone the project. `git clone https://github.com/bgoldbeck/rogue.git`
2. Download and install Visual Studio Code. [Visual Studio Code](https://code.visualstudio.com/)
3. Download and install .NET Core app SDK 2.1 from Microsoft. [.NET Core SDK 2.1](https://www.microsoft.com/net/download/dotnet-core/sdk-2.1.300) Instructions for your distribution may vary.
4. Launch Visual Studio Code.
5. Open the project folder from VS Code and select View > Extensions.
  1. Install "C# for visual studio code" extension.
  2. Install "C# FixFormat" extension.
  3. Install "C# Extensions" extension.
  4. Install "C# XML Documentation Comments" extension.
  5. Install NuGet package Manager extension.
6. Open the terminal in Visual Studio Code "Ctrl + tilde" and type `dotnet run` to run the application.
7. Run tests with `dotnet test`.

# License

This work is licensed under the MIT license. See LICENSE for full text.
