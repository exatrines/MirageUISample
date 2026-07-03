# MirageUI Sample

MirageUI library demo and development plugin for Dalamud.

## Setup

This repository depends on [MirageUI](https://github.com/exatrines/MirageUI) as a git submodule at `MirageUI/` (repository root).

```bash
git clone --recurse-submodules https://github.com/exatrines/MirageUISample.git
cd MirageUISample
```

If you already cloned without submodules:

```bash
git submodule update --init --recursive
```

### First-time publish (maintainers)

1. Create the **MirageUI** repository on GitHub and push the library repo.
2. Update `url` in `.gitmodules` to your MirageUI repository URL.
3. Remove the local junction if present, then add the submodule:

```bash
git submodule add https://github.com/exatrines/MirageUI.git MirageUI
```

4. Create the **MirageUISample** repository on GitHub and push.

For local development before publishing, a directory junction may point `MirageUI` at a sibling checkout.

## Build

```bash
dotnet build MirageUiSample.sln -c Release  # run from MirageUISample/
```

Output: `bin/Release/MirageUiSample.dll`

## MirageUI reference

The sample project references:

`../MirageUI/MirageUI/MirageUI.csproj`

## Commands

- `/mirageuisample` or `/muisample` — open layout preview (main UI)
- Plugin list settings button — open layout options (config UI)
