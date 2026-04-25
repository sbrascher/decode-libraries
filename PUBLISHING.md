# Publishing Guide - Decode Libraries

This guide explains how to build, pack, and publish the Decode libraries to NuGet.org.

## 1. Prerequisites
- A registered account at [NuGet.org](https://www.nuget.org).
- .NET SDK installed.

## 2. Generate API Key
1. Log in to [NuGet.org](https://www.nuget.org).
2. Go to **API Keys** in your profile menu.
3. Click **+ Create**:
   - **Key Name**: `DecodeLibrariesKey`
   - **Scopes**: Select `Push`.
   - **Glob Pattern**: `*`
4. Click **Create** and then **Copy** the key. 
   > **Note**: Store this key securely; it will not be shown again.

## 3. Building and Packing
Before publishing, you must generate the `.nupkg` files. Run this command from the repository root:

```powershell
dotnet pack src/Decode.sln -c Release -o ./nupkg
```

This will create the following files in the `./nupkg` folder:
- `Decode.Data.Abstractions.x.x.x.nupkg`
- `Decode.Data.x.x.x.nupkg`

## 4. Publishing to NuGet.org

### Using CLI (Recommended)
Replace `YOUR_API_KEY` with the key you copied earlier:

**Step 1: Push Abstractions**
```powershell
dotnet nuget push ./nupkg/Decode.Data.Abstractions.*.nupkg --api-key YOUR_API_KEY --source https://api.nuget.org/v3/index.json
```

**Step 2: Push Implementation**
```powershell
dotnet nuget push ./nupkg/Decode.Data.*.nupkg --api-key YOUR_API_KEY --source https://api.nuget.org/v3/index.json
```

### Using Web Interface
Alternatively, you can manually upload the `.nupkg` files at [nuget.org/packages/manage/upload](https://www.nuget.org/packages/manage/upload).

## 5. Important Rules

### Versioning
NuGet does not allow overwriting existing versions. To publish updates:
1. Open the `.csproj` file.
2. Increment the `<Version>` tag (e.g., `1.0.0` -> `1.0.1`).
3. Re-run `dotnet pack` and `dotnet nuget push`.

### Indexing Time
After pushing, your package will undergo "Validation". This usually takes **5 to 15 minutes**. It will only be searchable and available for installation after this process completes.

## 6. Pushing to GitHub
Don't forget to keep your source code updated:

```powershell
git add .
git commit -m "chore: update project metadata and documentation"
git push origin main
```
