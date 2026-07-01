# MirageUI Sample

MirageUI library demo and development plugin for Dalamud.

## Setup

This repository depends on [MirageUI](https://github.com/exatrines/MirageUI) as a git submodule at `vendor/MirageUI/`.

```bash
git clone --recurse-submodules https://github.com/exatrines/MirageUISample.git
cd MirageUiSample
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
rmdir vendor\MirageUI
git submodule add https://github.com/exatrines/MirageUI.git vendor/MirageUI
```

4. Create the **MirageUISample** repository on GitHub and push.

For local development before publishing, a directory junction may point `vendor/MirageUI` at a sibling `MirageUI` checkout.

## Build

```bash
dotnet build MirageUiSample.sln -c Release
```

Output: `bin/Release/MirageUiSample.dll`

## Submodule path

MirageUI is vendored at `vendor/MirageUI/`. The sample project references:

`vendor/MirageUI/MirageUI/MirageUI.csproj`

## Commands

- `/mirageuisample` or `/muisample` — open layout preview (main UI)
- Plugin list settings button — open layout options (config UI)
