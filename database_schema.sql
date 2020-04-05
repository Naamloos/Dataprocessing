-- phpMyAdmin SQL Dump
-- version 4.8.4
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Mar 31, 2020 at 12:54 AM
-- Server version: 10.1.37-MariaDB
-- PHP Version: 7.3.0

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `dataprocessing`
--
CREATE DATABASE IF NOT EXISTS `dataprocessing` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE `dataprocessing`;

-- --------------------------------------------------------

--
-- Table structure for table `authkeys`
--

CREATE TABLE `authkeys` (
  `id` int(11) NOT NULL,
  `keyowner` text COLLATE utf8mb4_unicode_ci NOT NULL,
  `admin` tinyint(1) NOT NULL,
  `authkey` text COLLATE utf8mb4_unicode_ci NOT NULL,
  `totalcalls` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Table structure for table `globalterrorismdatabase`
--

CREATE TABLE `globalterrorismdatabase` (
  `eventid` bigint(20) NOT NULL,
  `iyear` int(11) DEFAULT NULL,
  `imonth` int(11) DEFAULT NULL,
  `iday` int(11) DEFAULT NULL,
  `approxdate` text COLLATE utf8mb4_unicode_ci,
  `extended` tinyint(1) DEFAULT NULL,
  `resolution` text COLLATE utf8mb4_unicode_ci,
  `country` text COLLATE utf8mb4_unicode_ci,
  `country_txt` text COLLATE utf8mb4_unicode_ci,
  `region` text COLLATE utf8mb4_unicode_ci,
  `region_txt` text COLLATE utf8mb4_unicode_ci,
  `provstate` text COLLATE utf8mb4_unicode_ci,
  `city` text COLLATE utf8mb4_unicode_ci,
  `latitude` float DEFAULT NULL,
  `longitude` float DEFAULT NULL,
  `specificity` int(11) DEFAULT NULL,
  `vicinity` int(11) DEFAULT NULL,
  `location` text COLLATE utf8mb4_unicode_ci,
  `summary` text COLLATE utf8mb4_unicode_ci,
  `crit1` int(11) DEFAULT NULL,
  `crit2` int(11) DEFAULT NULL,
  `crit3` int(11) DEFAULT NULL,
  `doubtterr` int(11) DEFAULT NULL,
  `alternative` int(11) DEFAULT NULL,
  `alternative_txt` text COLLATE utf8mb4_unicode_ci,
  `multiple` tinyint(1) DEFAULT NULL,
  `success` tinyint(1) DEFAULT NULL,
  `suicide` tinyint(1) DEFAULT NULL,
  `attacktype1` int(11) DEFAULT NULL,
  `attacktype1_txt` text COLLATE utf8mb4_unicode_ci,
  `attacktype2` int(11) DEFAULT NULL,
  `attacktype2_txt` text COLLATE utf8mb4_unicode_ci,
  `attacktype3` int(11) DEFAULT NULL,
  `attacktype3_txt` text COLLATE utf8mb4_unicode_ci,
  `targtype1` int(11) DEFAULT NULL,
  `targtype1_txt` text COLLATE utf8mb4_unicode_ci,
  `targsubtype1` int(11) DEFAULT NULL,
  `targsubtype1_txt` text COLLATE utf8mb4_unicode_ci,
  `corp1` text COLLATE utf8mb4_unicode_ci,
  `target1` text COLLATE utf8mb4_unicode_ci,
  `natlty1` int(11) DEFAULT NULL,
  `natlty1_txt` text COLLATE utf8mb4_unicode_ci,
  `targtype2` int(11) DEFAULT NULL,
  `targtype2_txt` text COLLATE utf8mb4_unicode_ci,
  `targsubtype2` int(11) DEFAULT NULL,
  `targsubtype2_txt` text COLLATE utf8mb4_unicode_ci,
  `corp2` text COLLATE utf8mb4_unicode_ci,
  `target2` text COLLATE utf8mb4_unicode_ci,
  `natlty2` int(11) DEFAULT NULL,
  `natlty2_txt` text COLLATE utf8mb4_unicode_ci,
  `targtype3` int(11) DEFAULT NULL,
  `targtype3_txt` text COLLATE utf8mb4_unicode_ci,
  `targsubtype3` int(11) DEFAULT NULL,
  `targsubtype3_txt` text COLLATE utf8mb4_unicode_ci,
  `corp3` text COLLATE utf8mb4_unicode_ci,
  `target3` text COLLATE utf8mb4_unicode_ci,
  `natlty3` int(11) DEFAULT NULL,
  `natlty3_txt` text COLLATE utf8mb4_unicode_ci,
  `gname` text COLLATE utf8mb4_unicode_ci,
  `gsubname` text COLLATE utf8mb4_unicode_ci,
  `gname2` text COLLATE utf8mb4_unicode_ci,
  `gsubname2` text COLLATE utf8mb4_unicode_ci,
  `gname3` text COLLATE utf8mb4_unicode_ci,
  `gsubname3` text COLLATE utf8mb4_unicode_ci,
  `motive` text COLLATE utf8mb4_unicode_ci,
  `guncertain1` tinyint(1) DEFAULT NULL,
  `guncertain2` tinyint(1) DEFAULT NULL,
  `guncertain3` tinyint(1) DEFAULT NULL,
  `individual` tinyint(1) DEFAULT NULL,
  `nperps` int(11) DEFAULT NULL,
  `nperpcap` int(11) DEFAULT NULL,
  `claimed` tinyint(1) DEFAULT NULL,
  `claimmode` int(11) DEFAULT NULL,
  `claimmode_txt` text COLLATE utf8mb4_unicode_ci,
  `claim2` tinyint(1) DEFAULT NULL,
  `claimmode2` int(11) DEFAULT NULL,
  `claimmode2_txt` text COLLATE utf8mb4_unicode_ci,
  `claim3` tinyint(1) DEFAULT NULL,
  `claimmode3` int(11) DEFAULT NULL,
  `claimmode3_txt` text COLLATE utf8mb4_unicode_ci,
  `compclaim` text COLLATE utf8mb4_unicode_ci,
  `weaptype1` int(11) DEFAULT NULL,
  `weaptype1_txt` text COLLATE utf8mb4_unicode_ci,
  `weapsubtype1` int(11) DEFAULT NULL,
  `weapsubtype1_txt` text COLLATE utf8mb4_unicode_ci,
  `weaptype2` int(11) DEFAULT NULL,
  `weaptype2_txt` text COLLATE utf8mb4_unicode_ci,
  `weapsubtype2` int(11) DEFAULT NULL,
  `weapsubtype2_txt` text COLLATE utf8mb4_unicode_ci,
  `weaptype3` int(11) DEFAULT NULL,
  `weaptype3_txt` text COLLATE utf8mb4_unicode_ci,
  `weapsubtype3` int(11) DEFAULT NULL,
  `weapsubtype3_txt` text COLLATE utf8mb4_unicode_ci,
  `weaptype4` int(11) DEFAULT NULL,
  `weaptype4_txt` text COLLATE utf8mb4_unicode_ci,
  `weapsubtype4` int(11) DEFAULT NULL,
  `weapsubtype4_txt` text COLLATE utf8mb4_unicode_ci,
  `weapdetail` text COLLATE utf8mb4_unicode_ci,
  `nkill` int(11) DEFAULT NULL,
  `nkillus` int(11) DEFAULT NULL,
  `nkillter` int(11) DEFAULT NULL,
  `nwound` int(11) DEFAULT NULL,
  `nwoundus` int(11) DEFAULT NULL,
  `nwoundte` int(11) DEFAULT NULL,
  `property` int(11) DEFAULT NULL,
  `propextent` int(11) DEFAULT NULL,
  `propextent_txt` text COLLATE utf8mb4_unicode_ci,
  `propvalue` int(11) DEFAULT NULL,
  `propcomment` text COLLATE utf8mb4_unicode_ci,
  `ishostkid` tinyint(1) DEFAULT NULL,
  `nhostkid` int(11) DEFAULT NULL,
  `nhostkidus` int(11) DEFAULT NULL,
  `nhours` int(11) DEFAULT NULL,
  `ndays` int(11) DEFAULT NULL,
  `divert` text COLLATE utf8mb4_unicode_ci,
  `kidhijcountry` text COLLATE utf8mb4_unicode_ci,
  `ransom` tinyint(1) DEFAULT NULL,
  `ransomamt` int(11) DEFAULT NULL,
  `ransomamtus` int(11) DEFAULT NULL,
  `ransompaid` int(11) DEFAULT NULL,
  `ransompaidus` int(11) DEFAULT NULL,
  `ransomnote` text COLLATE utf8mb4_unicode_ci,
  `hostkidoutcome` int(11) DEFAULT NULL,
  `hostkidoutcome_txt` text COLLATE utf8mb4_unicode_ci,
  `nreleased` tinyint(1) DEFAULT NULL,
  `addnotes` text COLLATE utf8mb4_unicode_ci,
  `scite1` text COLLATE utf8mb4_unicode_ci,
  `scite2` text COLLATE utf8mb4_unicode_ci,
  `scite3` text COLLATE utf8mb4_unicode_ci,
  `dbsource` text COLLATE utf8mb4_unicode_ci,
  `INT_LOG` int(11) DEFAULT NULL,
  `INT_IDEO` int(11) DEFAULT NULL,
  `INT_MISC` int(11) DEFAULT NULL,
  `INT_ANY` int(11) DEFAULT NULL,
  `related` text COLLATE utf8mb4_unicode_ci
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Table structure for table `spotify`
--

CREATE TABLE `spotify` (
  `Position` int(11) NOT NULL,
  `TrackName` text CHARACTER SET latin1 NOT NULL,
  `Artist` text CHARACTER SET latin1 NOT NULL,
  `Streams` int(11) NOT NULL,
  `Url` text CHARACTER SET latin1 NOT NULL,
  `Date` date NOT NULL,
  `Region` varchar(2) CHARACTER SET latin1 NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Table structure for table `youtube`
--

CREATE TABLE `youtube` (
  `VideoId` varchar(11) COLLATE utf8mb4_unicode_ci NOT NULL,
  `TrendingDate` date NOT NULL,
  `Title` text COLLATE utf8mb4_unicode_ci NOT NULL,
  `ChannelTitle` text COLLATE utf8mb4_unicode_ci NOT NULL,
  `CategoryId` int(11) NOT NULL,
  `PublishTime` datetime NOT NULL,
  `Tags` text COLLATE utf8mb4_unicode_ci NOT NULL,
  `Views` int(11) NOT NULL,
  `Likes` int(11) NOT NULL,
  `Dislikes` int(11) NOT NULL,
  `CommentCount` int(11) NOT NULL,
  `ThumbnailLink` text COLLATE utf8mb4_unicode_ci NOT NULL,
  `CommentsDisabled` tinyint(1) NOT NULL,
  `RatingsDisabled` tinyint(1) NOT NULL,
  `VideoErrorOrRemoved` tinyint(1) NOT NULL,
  `Description` text COLLATE utf8mb4_unicode_ci NOT NULL,
  `CountryCode` varchar(2) COLLATE utf8mb4_unicode_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Indexes for dumped tables
--

--
-- Indexes for table `authkeys`
--
ALTER TABLE `authkeys`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `globalterrorismdatabase`
--
ALTER TABLE `globalterrorismdatabase`
  ADD PRIMARY KEY (`eventid`);

--
-- Indexes for table `spotify`
--
ALTER TABLE `spotify`
  ADD PRIMARY KEY (`Position`,`Date`,`Region`);

--
-- Indexes for table `youtube`
--
ALTER TABLE `youtube`
  ADD PRIMARY KEY (`VideoId`,`TrendingDate`,`CountryCode`) USING BTREE;

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `authkeys`
--
ALTER TABLE `authkeys`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
