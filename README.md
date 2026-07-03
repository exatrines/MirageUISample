# MirageUI Monorepo

This repository contains:

- [`MirageUI/`](MirageUI/) — MirageUI library (git submodule)
- [`MirageUISample/`](MirageUISample/) — Dalamud sample plugin

## Setup

```bash
git clone --recurse-submodules https://github.com/exatrines/MirageUISample.git
cd MirageUISample
```

If you already cloned without submodules:

```bash
git submodule update --init --recursive
```

## Build

```bash
dotnet build MirageUISample/MirageUiSample.sln -c Release
```

Output: `MirageUISample/bin/Release/MirageUiSample.dll`

See [MirageUISample/README.md](MirageUISample/README.md) for plugin details.
