#!/bin/bash
%F=$1
%L=$2
%N=$3
echo "param 1:"$1
echo "param 2:"$2
echo "param 3:"$3
#line for qbittorrent:
#/config/torrentUnrar.sh "%F" "%L" "%N"
yourPathEscaped=$(printf %q "$1")
eval cd $yourPathEscaped
count=`ls -1 *.rar 2>/dev/null | wc -l` 
echo "count files rar :"$count
if [ $count != 0 ]; then
    echo "Exist rar files"
	if [[ $2 == 'tv-sonarr' ]]; then
		echo "Is sonarrrr torrent! tag: $2"
		#1. Extract the rar to the folder /downloads/temp-unrar/***TORRENT NAME***/
		echo "1. Extract the rar to the folder /downloads/temp-unrar/***TORRENT NAME***/"
		echo "run line: unrar x '$1/*.rar' '/downloads/temp-unrar/$3/'"
		unrar x "$1/*.rar" "/downloads/temp-unrar/$3/"
		echo "run line: mkdir '$1/unrar'"
		mkdir "$1/unrar"
		#2. When finished, move the unzipped files back to the folder where the rar is. In a '/unrar' 
		echo "2. When finished, move the unzipped files back to the folder where the rar is. In a '/unrar'"
		echo "run line: find '/downloads/temp-unrar/$3/' -maxdepth 1 -type f -not -name 'exe_*' -exec mv {} '$1/unrar' \;"
		find "/downloads/temp-unrar/$3/" -maxdepth 1 -type f -not -name 'exe_*' -exec mv {} "$1/unrar" \;
		echo "rm -r '/downloads/temp-unrar/$3/'"
		rm -R "/downloads/temp-unrar/$3/"
	fi
	if [[ $2 != 'tv-sonarr' ]]; then
		echo "NOT sonarr torrent, tag: $2"
		#1. Extract the rar to same folder
		echo "1. Extract the rar to same folder"
		echo "run line: unrar x '$1/*.rar' '$1'"
		unrar x "$1/*.rar" "$1"
	fi
else
    echo "NOT found rar files"
fi
echo "MAINTENANCE"
#3. Maintenance, finds all /unrar folders and deletes them when they are older than 24h
echo "3 Maintenance, finds all /unrar folders and deletes them when they are older than 24h"
find /downloads/ -type d -name "unrar" -mtime +1 -exec echo "This file found" {} \;
echo "run line: find /downloads/ -type d -name "unrar" -mtime +1  -exec rm {} -r \;"
find /downloads/ -type d -name "unrar" -mtime +1  -exec rm {} -r \;
