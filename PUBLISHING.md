# Guia de Publicação - Decode Libraries

Este guia explica como compilar, empacotar e publicar as bibliotecas Decode no NuGet.org.

## 1. Pré-requisitos
- Uma conta registrada no [NuGet.org](https://www.nuget.org).
- SDK do .NET instalado.

## 2. Gerar Chave de API (API Key)
1. Faça login no [NuGet.org](https://www.nuget.org).
2. Vá em **API Keys** no menu do seu perfil.
3. Clique em **+ Create**:
   - **Key Name**: `DecodeLibrariesKey`
   - **Scopes**: Selecione `Push`.
   - **Glob Pattern**: `*`
4. Clique em **Create** e depois em **Copy** para copiar a chave. 
   > **Nota**: Guarde esta chave em um lugar seguro; ela não será exibida novamente.

## 3. Compilar e Empacotar
Antes de publicar, você deve gerar os arquivos `.nupkg`. Execute este comando na raiz do repositório:

```powershell
dotnet pack src/Decode.sln -c Release -o ./nupkg
```

Isso criará os seguintes arquivos na pasta `./nupkg`:
- `Decode.Data.Abstractions.x.x.x.nupkg`
- `Decode.Data.x.x.x.nupkg`

## 4. Publicar no NuGet.org

### Usando a Linha de Comando (Recomendado)
Substitua `SUA_CHAVE_AQUI` pela chave que você copiou:

**Passo 1: Publicar as Abstrações**
```powershell
dotnet nuget push ./nupkg/Decode.Data.Abstractions.*.nupkg --api-key SUA_CHAVE_AQUI --source https://api.nuget.org/v3/index.json
```

**Passo 2: Publicar a Implementação**
```powershell
dotnet nuget push ./nupkg/Decode.Data.*.nupkg --api-key SUA_CHAVE_AQUI --source https://api.nuget.org/v3/index.json
```

### Usando a Interface Web
Como alternativa, você pode carregar manualmente os arquivos `.nupkg` em [nuget.org/packages/manage/upload](https://www.nuget.org/packages/manage/upload).

## 5. Regras Importantes

### Versionamento
O NuGet não permite sobrescrever versões existentes. Para publicar atualizações:
1. Abra o arquivo `.csproj`.
2. Aumente a tag `<Version>` (ex: `1.0.0` -> `1.0.1`).
3. Rode novamente o `dotnet pack` e o `dotnet nuget push`.

### Tempo de Indexação
Após o envio, seu pacote passará por uma "Validação". Isso geralmente leva de **5 a 15 minutos**. Ele só estará disponível para busca e instalação após a conclusão desse processo.

## 6. Atualizar o GitHub
Não esqueça de manter seu código atualizado no repositório:

```powershell
git add .
git commit -m "chore: atualiza metadados e documentação"
git push origin main
```
