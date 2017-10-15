# Changelog

## Version 2.6.0.0
Developed & Released by Melissa (MythicalCuddles)

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
[General]
- Cleaned up the Configuration File to make it look nice and new. (ﾉ◕ヮ◕)ﾉ*:･ﾟ✧

[Added]
- Added the Bot token to the configuration file. It will now check for a token there before asking MelissasCode for it.
- Added offlineList Tuple to log new users and the guild they joined whilst the bot was offline.
- Added a console message to display if no new users were found whilst the bot was offline.

[Removed]
- Removed offlineUsersList due to offlineList Tuple being implemented.

[Other]
- Nothing here.
