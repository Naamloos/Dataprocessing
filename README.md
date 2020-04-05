# Dataprocessing
End assignment for Dataprocessing @ NHL Stenden

To set up this project, you need to set up the two parts it requires to operate. The Database and the Software. The following setup guide consists of two parts: Database Setup and Software Setup.

## Database Setup
In the root directory, you will find a `database_schema.sql` file. This file does **not** contain any data used in this project. This file contains an empty version of this database in case you want to import the data yourself. Of course, a prebuilt database is also available.

### Datasets
The datasets I used can be found at the following pages on *Kaggle*.
- [Spotify's Worldwide Daily Song Ranking](https://www.kaggle.com/edumucelli/spotifys-worldwide-daily-song-ranking)
- [YouTube Trending Videos](https://www.kaggle.com/datasnaek/youtube-new)
- [Global Terrorism Database](https://www.kaggle.com/START-UMD/gtd)

### Preset database
Lucky you! I've set up the database myself! To prevent storing large files on GitHub, a dump can be found on my [OneDrive](https://newuniversity-my.sharepoint.com/:u:/g/personal/ryan_de_jonge_student_nhlstenden_com/Ee6t_UjXR7VKoG89icNRFwoBll7kllj1FewMUNaHtzY_9A?e=0cYXSX). I recommend not importing this with phpmyadmin but instead importing it via the command line. It's _that **big**_.

### Importing from SQL dump
To do this, make sure you can run the `mysql` command from your command line.
1. Create a new database.
2. Open a command line in the directory you saved the sql file.
3. Run the following command: 
```
mysql -u [username] -p [password] [database name] < database.sql
```
4. Done! :)

### Importing from datasets
You can skip these step if you've imported the database from it's SQL file.

If you want to import these yourself, make sure to use the `database_schema.sql` file. Saves a _lot_ of work.

Use the following SQL queries to import the datasets. Please do note that I am not entirely sure whether these were the right queries, as I just looked them up in some chat logs. 

Before you do all this,makesure your database has all it's text entries encoded in `utf8mb4_unicode_ci`.

**For the YouTube datasets**:
```sql
LOAD DATA INFILE 'path/to/MXVideos.csv' IGNORE INTO TABLE youtube
    CHARACTER SET utf8mb4
        FIELDS TERMINATED BY ','
        ENCLOSED BY '"'
        LINES TERMINATED BY '\n'
        (VideoId, TrendingDate, Title, ChannelTitle, CategoryId, PublishTime, Tags, Views, Likes, Dislikes, CommentCount, ThumbnailLink, CommentsDisabled, RatingsDisabled, VideoErrorOrRemoved, Description)
    SET CountryCode = 'MX',
        TrendingDate = STR_TO_DATE(TrendingDate, '%y.%m.%d');
```
**NOTE:** *The YouTube dataset consists of a couple of files. I've imported them all into one table with their country code appended to the end of each row. Make sure to change the country code in both the filepath and the `SET` statement for each csv file.*

**NOTE 2:** *The YouTube dataset has it's TrendingDate in a weird format. The format is `yy.dd.mm`. I had a lot of trouble importing this but eventually it worked out. I remember it being something with `STR_TO_DATE(field, '%y.%m.%d')`. Don't quote me on that.*

**For the GTD dataset**:
```sql
LOAD DATA INFILE 'path/to/globalterrorismdb_0718dist.csv' IGNORE INTO TABLE gtd
    CHARACTER SET utf8mb4
        FIELDS TERMINATED BY ','
        ENCLOSED BY '"'
        LINES TERMINATED BY '\n';
```

**Lastly, for the Spotify dataset**:
```sql
LOAD DATA INFILE 'path/to/data.csv' IGNORE INTO TABLE spotify
    CHARACTER SET utf8mb4
        FIELDS TERMINATED BY ','
        ENCLOSED BY '"'
        LINES TERMINATED BY '\n';
```
These last two datasets don't need a lot of work to import :)

## Software setup
### Software Requirements
To setup this project, first make sure the following prerequisites are installed:
- a MySQL server
- .NET Core 3.0 SDK or Runtime
- A modern web browser

System requirements for this software are simple, all you need is a computer or server running a modern version of Windows or a modern Linux distribution. It should also work on MacOS but I am unable to try because my wallet is not that large. 

### Preparing and running the software
To start the web server and API all you have to do is obtain compiled binaries of this project or compile them yourself. After this, start your MySQL server. Once that's done, run `dotnet DataprocessingApi.dll` with admin privileges. Admin privileges are not required but recommended if you decide to host this online for whatever reason. The server will serve files and api endpoints on port 5000. To access it, browse to `localhost:5000` or `127.0.0.1:5000` using a modern web browser.

#### First run
On the first run, the program generates a `config.json` file. Edit this with your database connection settings.

### Compiling
To compile this software yourself, you will need the .Net Core 3.0 SDK and optionally a version of Visual Studio that supports .Net Core 3.0.

#### Using Visual Studio
1. Run `Dataprocessing.sln` with Visual Studio.
2. Build -> Build Solution
3. Grab your files from `/DataprocessingApi/bin/{configuration}/{target}/`.
4. Success!

#### Using the command line
1. Run the following command: `dotnet build Dataprocessing.sln`
2. Grab your files from the build directory (might be the same as vs, I'm not sure)
3. Done!

### API Documentation
The API documentation is available at `/docs` when you run the program.

#### Schema's
Local paths to the XML/JSON schema files will be provided in the `link` header.