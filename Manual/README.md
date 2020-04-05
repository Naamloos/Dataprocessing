# Setup Guide
## Requirements
To setup this project, first make sure the following prerequisites are installed:
- a MySQL server (MariaDB is okay too of course.)
- .NET Core 3.0 SDK or Runtime
- A modern web browser

System requirements for this software are simple, all you need is a computer or server running a modern version of Windows or a modern Linux distribution. It should also work on MacOS but I am unable to try because my wallet is not that large.

## Preparing the database
This project uses a MySQL server to operate. A guide on setting up your database can be found in the `Database.md` file.

## Preparing and running the software
To start the web server and API all you have to do is obtain compiled binaries of this project or compile them yourself. After this, start your MySQL server. Once that's done, run `dotnet DataprocessingApi.dll` with admin privileges. Admin privileges are not required but recommended if you decide to host this online for whatever reason. The server will serve files and api endpoint on port 5000. To access it, browse to `localhost:5000` or `127.0.0.1:5000`.

### First run
On the first run, the program generates a `config.json` file. Edit this with your database connection settings.

## Compiling yourself
To compile this software yourself, you will need the .Net Core 3.0 SDK and optionally a version of Visual Studio that supports .Net Core 3.0.

### Compiling with Visual Studio
1. Run `Dataprocessing.sln` with Visual Studio.
2. Build -> Build Solution
3. Grab your files from `/DataprocessingApi/bin/{configuration}/{target}/`.
4. Success!

### Compiling using the command line
1. Run the following command: `dotnet build Dataprocessing.sln`
2. Grab your files from the build directory (might be the same as vs, I'm not sure)
3. Done!

## API Documentation
The API documentation is available at `/docs` when you run the program.

### Schema's
Local paths to the XML/JSON schema files will be provided in the `link` header.