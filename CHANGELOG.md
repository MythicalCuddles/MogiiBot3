# Changelog

## Version 2.8.1.0

### Added
	- Added "Server Time" to stats.
	- Added Activity Type ID's to editconfig.
	- Added extension method to convert Integer to ActivityType.
	- Added DatabaseHandler.cs to handle database initialisation and queries. (Still in early development)
	- Added uPrefix and gPrefix to the MessageReveived event to work with.
	- Added check to see if the user has a custom prefix for commands.

### Changed
	- Fixed command "showconfig" by adding null-coalescing operators to show which settings have yet to be set.
	- Fixed "showconfig" to show the ActivityType instead of an integer value.
	- Changed "showconfig" to show YES/NO instead of TRUE/FALSE.
	- Modified the Leave Embed to include more information about the user leaving a guild.
	- Created a variable for MinLengthForCoin to prevent configuration being loaded everytime a message is posted.
	- Changed MessageReceived message var to use pattern matching.
	- Changed "Channel Count" in stats to specify amount of text channels and voice channels.

### Fixed
	- Fixed an issue where some users wouldn't receive coins after sending a message. 🎉🎉

### Removed
	- Removed "Development For" from stats command.
	- Removed MessageHandler.cs
	- Removed MessageLogger.cs
	- Removed MessageUpdated event.
	- Removed MessageDeleted event.
	- Removed private messages being sent to the log channel.

### Other
	- Changed formatting of CHANGELOG.md to make it appear nicer in the IDE.


## Version 2.8.0.0

### Added
	- Added Project URL to console on startup.
	- Added a die command for the Bot Owner to log the Bot out and terminate the application.
	- Added EmbedModule for Moderators+ to create and send custom embeds to specified channels.
	- EmbedModule - Added command "embed" which will display the available commands for the embed prefix.
	- EmbedModule - Added subcommand "new" which will create a new embed in memory for the user.
	- EmbedModule - Added subcommand "withtitle" which will add the title passed to the stored embed.
	- EmbedModule - Added subcommand "withdescription" which will add the description passed to the stored embed.
	- EmbedModule - Added subcommand "withfooter" which will add the footer text and URL passed to the stored embed.
	- EmbedModule - Added subcommand "withcolor" which accepts RGB values to assign to the stored embed.
	- EmbedModule - Added subcommand "send" which will send the stored embed either to the channel where the command was issued or to the channel specified.
	- EmbedModule - [Future Development to include more commands and embed configuration options.]
	- Added ShowConfigModule for Guild Moderators, Administrators, Owners and the Bot Owners to see the configuration of the Bot and Guild.
	- ShowConfigModule - Added command "showconfig" which will display both Guild and Bot Configuration Information Embeds.
	- ShowConfigModule - Added subcommand "bot" to only send the Bot Configuration Information Embed.
	- ShowConfigModule - Added subcommand "guild" to only send the Guild Configuration Information Embed.
	- Added a message to display once the Bot is added to a new guild, instructing how to configure the Bot to the guild.

### Changed
	- Changed the way the bot token is read by saving it in the configuration file instead of using MelissasCode.
	- Changed gameactivity to activity to amend for the change in Discord.Net.
	- Fixed a formatting issue in "editconfig" where "status" would appear on the same line as "gameactivity" (now activity).
	- Fixed an issue with Rule34Gamble that prevented images from being sent properly.
	- Fixed an issues where new guild configurations would assign the ID 0 to channels.
	- Fixed an issue where a guild configuration wouldn't be created once the Bot joined the guild for the first time.
	- Replaced UpdateJson to UpdateUser in User.cs, changing the method in which a User is updated.
	- Replaced UpdateJson to UpdateChannel in Channel.cs, changing the method in which a Channel Configuration is updated.
	- Replaced UpdateJson to UpdateGuild in GuildConfiguration.cs, changing the method in which a Guild Configuration is updated.
	- Replaced UpdateJson to UpdateConfiguration in Configuration.cs, changing the method in which the Bot Configuration is updated.

### Removed
	- MelissasCode has been completely removed!! 🎉🎉
	- Removed music commands and features where the user would issue a command to have a random* link posted in the chat. (*Links were added to the database by a Guild Member with the required permissions.)


## Version 2.7.1.0
	- Updated Discord.Net


## Version 2.6.3.0 / 2.7.0.0

### Added
	- Added toggleawardingcoins for server owners to toggle which channels receive coins and which don't.
	- Added channel check to startup to catch any channels created whilst the bot is offline.

### Backend Notes
	- Added SetCoins to User Object to see if any changes are made.
	- Added Channel Object to keep track of channels awarding coins.


## Version 2.6.2.0

### Added
	- Added more emotes to the slot machines.
	- Added an algorithm to judge if a user is awarded a coin or not.
	- Added minlengthforcoins for the Bot Owner to force change one of the algorithm's variables.

### Changed
	- Changed the leaderboard to show the users current position on the board if they don't show within the top list.
	- Changed buyquote to check if quotes are enabled before allowing a user to buy a quote spot.
	- Changed the arrow in the slots from '<' to ':arrow_left:'
	- Fixed an issue with the leaderboard where it would show duplicates of users.
	- Fixed an issue with the leaderboard where it would still show bot users.

### Removed
	- Removed 'F' version info.
	- Removed GUID from stats.

### Backend Notes
	- Changed the way commands and messages are read, ensuring that the AwardCoinsToPlayer method is called.


## Version 2.6.1.0

### Changed
	- Reverted to an older version of the leaderboard module due to issue with duplicates appearing in the list.


## Version 2.6.0.0
Many thanks to Marceline for getting MelissaNet added to MogiiBot in time for the update (Private Library)

### Added
	- Added MelissaNet.dll reference added due to the discontinue of MelissasCode.
	- Added MelissaNet version logging to startup log.
	- Added MelissaNet version added to stats.
	- Added resetallcoins for the Bot Owner to force reset all user coins.
	- Added gameactivity to editconfig to allow the Bot Owner to set the playing message and an optional twitch link.
	- Added a new alias for rule34gamble - rule34.
	- Added editiconurl for the Bot Owner to change the icons that appears in a users about embed.
	- Added a total count to the amount of respects that have been paid.

### Changed
	- Changed setaboutrgb to setcustomrgb, as it is used in more places than the about embed now.
	- Changed the way "F" looks by writing the message into an embed.
	- Changed the leaderboard to show users from all guilds.
	- Changed the leaderboard to not show any bots.
	- Changed force commands to output as an embed instead of a message.
	- Fixed an issue where the bot wouldn't load into streaming mode on restarting. (RB 149991092337639424)
	- Fixed an issue where AwardCoinsToPlayer didn't award any coins to the user. (RB 95941149587427328)
	- Fixed an issue with Rule34 where the link would not embed the image from the website. (RB 95941149587427328)
	- Fixed an issue where Rule34 would sometimes throw out a Thumbnail or Sample image. (RB 95941149587427328)

### Removed
	- Removed playing command to set the Bot playing message.
	- Removed twitch command to set the Bots status into twitch streaming status.
	- Removed a switch which checked if the user can be awarded coins for reactions.
	- Removed an error message which contained sensitive information.

### Backend Notes
	- Cleaned up the Configuration File to make it look nice and new. (ﾉ◕ヮ◕)ﾉ*:･ﾟ✧
	- Added the Bot token to the configuration file. It will now check for a token there before asking MelissasCode for it.
	- Added offlineList Tuple to log new users and the guild they joined whilst the bot was offline.
	- Added a console message to display if no new users were found whilst the bot was offline.
	- Removed offlineUsersList due to offlineList Tuple being implemented.