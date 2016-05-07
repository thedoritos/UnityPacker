# UnityPacker
Export Unity package :package:

## Install

Import `UnityPacker.unitypackage`.

## Define Package

Create YourPackageName.pack file.

**UnityPacker.pack**

```
Assets/UnityPacker/Editor/UnityPacker.cs
Assets/UnityPacker/Editor/UnityPacker.pack
Assets/Plugins/iOS/UnityPacker
```

### NOTE :memo:

- The paths are relative paths from the project root.
- The paths can be either files or directories. 
- The paths can not be patterns. (i.e. `*` will not behave like a wildcard)

## Export Package

UnityEditor Menu > Tools > Packer > Export Packages (Cmd + Shift + E)
