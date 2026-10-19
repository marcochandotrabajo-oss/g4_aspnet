# G4 · ASP.NET Core Gestión de incidencias informáticas (Help Desk)

Temática ya elegida: Help Desk.

Repositorio del **Grupo 4** del curso de .NET: aplicación web con **C#, ASP.NET Core MVC, Razor y Entity Framework Core**, desarrollada en equipo con Git y GitHub.

## Qué incluye el esqueleto

Punto de partida común de todos los grupos: **solo la entidad `ApplicationUser` (usuario)** y la infraestructura para que todo arranque a la primera. Las entidades del dominio, sus relaciones, servicios, controladores y vistas las construye el equipo.

| Área | Contenido |
| --- | --- |
| Usuarios | `ApplicationUser` (ASP.NET Core Identity) con nombre visible, avatar, fecha de alta y estado activo; roles `Admin` y `User`. |
| Cuenta | Registro, login por usuario o email, logout y página de acceso denegado. |
| Perfil | Consulta y edición del perfil, avatar y cambio de contraseña. |
| Administración | Panel `/Users` (solo `Admin`): alta, edición, activación y borrado de usuarios. |
| Datos | Entity Framework Core + SQLite (`App_Data/g4.db`), migración inicial de Identity y datos demo al arrancar. |
| Imágenes | `ImageStorage` para guardar imágenes subidas; hoy solo avatares (añadid vuestras carpetas en `ImageFolder`). |
| Interfaz | Razor + Bootstrap 5 en local, con tema claro/oscuro. |
| Calidad y despliegue | Proyecto de tests xUnit, GitHub Actions (compilar y testear) y `Dockerfile`. |

## Requisitos

- SDK de .NET 10. `global.json` acepta cualquier SDK de la versión 10.0.
- Visual Studio Code y C# Dev Kit. Es el IDE estándar del curso en Windows, macOS y
  Linux.
- Git. Docker es opcional y solo se usa al explicar despliegue.

No instales ASP.NET, C#, SQLite ni Entity Framework por separado: los aporta el SDK o
se restauran como dependencias del proyecto.

## Instalación inicial

Instala .NET 10 SDK, Visual Studio Code y C# Dev Kit una sola vez en el ordenador.

### Windows (PowerShell)

```powershell
winget install --id Microsoft.DotNet.SDK.10 --exact
winget install --id Microsoft.VisualStudioCode --exact
winget install --id Git.Git --exact
```

Cierra y abre una terminal nueva; después instala la extensión:

```powershell
code --install-extension ms-dotnettools.csdevkit
```

### macOS (Terminal, con Homebrew)

```bash
brew install dotnet
brew install --cask visual-studio-code
brew install git
code --install-extension ms-dotnettools.csdevkit
```

Si no utilizas Homebrew, instala el SDK de .NET 10 con el instalador oficial de
[macOS](https://dotnet.microsoft.com/download/dotnet/10.0) y añade C# Dev Kit desde
el panel Extensions de VS Code.

### Ubuntu 26.04 (Terminal)

```bash
sudo apt-get update
sudo apt-get install -y dotnet-sdk-10.0 git
sudo snap install code --classic
code --install-extension ms-dotnettools.csdevkit
```

Para otra distribución Linux, sigue el instalador oficial de
[.NET para Linux](https://learn.microsoft.com/dotnet/core/install/linux) y, una vez
instalado VS Code, ejecuta el último comando.

Comprueba la instalación desde esta carpeta:

```bash
dotnet --version
```

Debe mostrar una versión que empiece por `10.0`.
Si el comando `code` no se reconoce, abre VS Code e instala C# Dev Kit desde
**Extensions**.

## Primer arranque

Clona el repositorio y abre **su carpeta** (no una carpeta superior) en VS Code. C# Dev Kit carga automáticamente `G4AspNet.sln`.

```bash
git clone https://github.com/certidevs/g4_aspnet.git
cd g4_aspnet
code .
dotnet restore G4AspNet.sln
dotnet tool restore
dotnet run --project G4AspNet.csproj --launch-profile http
```

Abre `http://localhost:5107`. El primer arranque crea `App_Data/g4.db`, aplica la migración de Identity y añade los usuarios demo:

- `admin` / `Admin123!`
- `user` / `User123!`

## Trabajo en equipo en GitHub

Todo el grupo tiene permiso de escritura en este repositorio. Para no pisaros el trabajo:

1. **Una rama por tarea**, partiendo de `main` actualizado:

   ```bash
   git switch main
   git pull
   git switch -c feature/nombre-corto
   ```

2. **Commits pequeños y con mensaje claro** (`feat: ...`, `fix: ...`, `docs: ...`).
3. **Subid la rama y abrid una Pull Request hacia `main`** (`git push -u origin feature/nombre-corto`). Que la revise al menos otra persona del equipo antes de fusionarla y que el *check* de GitHub Actions esté en verde.
4. **No hagáis `push` directo a `main` ni `git push --force`.**
5. Antes de abrir la PR, y cada vez que `main` avance, actualizad vuestra rama: `git pull --rebase origin main`.
6. Repartid el trabajo con **Issues** (una persona por issue) y, si queréis, un tablero de **Projects**.

### Migraciones de EF Core en equipo

Las migraciones (`Data/Migrations`) son lo que más choca al trabajar en paralelo, sobre todo `ApplicationDbContextModelSnapshot.cs`:

- Avisad al equipo antes de crear una migración y fusionad su PR cuanto antes.
- Si al actualizar con `main` hay conflicto en `Data/Migrations`, no lo resolváis a mano: descartad vuestros cambios en esa carpeta, integrad `main`, borrad vuestra base local (`App_Data/*.db`) y volved a crear la migración con `dotnet ef migrations add`.
- Nunca editéis ni borréis migraciones que ya estén en `main`.

## Comandos de trabajo diario

Todos se ejecutan desde la raíz de este repositorio.

```bash
# Restaurar dependencias NuGet y herramientas locales tras clonar o actualizar
dotnet restore G4AspNet.sln
dotnet tool restore

# Ejecutar la aplicación
dotnet run --project G4AspNet.csproj --launch-profile http

# Ejecutar con recarga al guardar archivos
dotnet watch --project G4AspNet.csproj run --launch-profile http

# Compilar y ejecutar los tests
dotnet build G4AspNet.sln
dotnet test G4AspNet.sln
```

### Entity Framework y SQLite

`dotnet-ef` está fijado en `.config/dotnet-tools.json`, por lo que `dotnet tool restore` lo deja disponible sin instalar nada de forma global.

```bash
# Después de cambiar una entidad: crear y aplicar una migración
dotnet ef migrations add NombreDescriptivo
dotnet ef database update

# Corregir la última migración solo antes de aplicarla a la base de datos
dotnet ef migrations remove
```

Las migraciones se aplican también al arrancar la aplicación. El comando `database update` se incluye para conocer el flujo explícito.

### CI y Docker

[GitHub Actions](.github/workflows/build-and-test.yml) ejecuta restauración, compilación Release y tests en cada `push` a `main` y en cada *pull request* hacia `main`.

Docker es opcional:

```bash
docker build -t g4_aspnet:local .
docker run --rm -p 10000:10000 g4_aspnet:local
```

El contenedor queda disponible en `http://localhost:10000`.

## IDEs

En VS Code, selecciona el perfil `http` y pulsa F5 para depurar. Las tareas `G4: compilar` y `G4: ejecutar tests` están disponibles en **Terminal → Run Task**. En Windows, Visual Studio 2026 también abre `G4AspNet.sln`; consulta [la guía de IDEs](docs/IDE-SETUP.md).
