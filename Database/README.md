# Database
In this folder, you will find a `database_schema.sql` file. This file does **not** contain all data used in this project. This file contains an empty version of this database in case you want to import the data yourself.

## Datasets
The datasets can be found at the following pages on *Kaggle*.
- [Spotify](https://www.kaggle.com/edumucelli/spotifys-worldwide-daily-song-ranking)
- [YouTube](https://www.kaggle.com/datasnaek/youtube-new)
- [Global Terrorism Database](https://www.kaggle.com/START-UMD/gtd)

## I want the full database!!!1
Contact me and we'll sort stuff out.

## Importing using SQL
Use the following scripts to import the datasets. Please do not that I am not entirely sure whether these were the right queries, as I just looked them up in some chat logs from today.

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