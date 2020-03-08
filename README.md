# WebLeagueManager
A first C# application using Asp.net with deployment to azure cloud

## Needed to run

To run this application, the mailsettings must be set (e.g. using appsettings.json) to have a running Smtp Server, in the following form:
```javascript
  "MailSettings": {
    "Password": "SECRET",
    "Username": "SECRETUSER",
    "SmtpServer": "WORKINGSMTP",
    "SmtpPort": 12345,
    "FromAddress": "ROBOT@Domain",
  },
```


# How to Use this application once running:

To get started Register an Account and verify your email

Once registered, navigate by clicking Leagues in the top left

Here all your created leagues will be listed. To add a new one, click Create New League in the top left.

Fill Out the name of the league and the creation date and hit create

The added information can be edited, viewed on a details page, deleted and the seasons can be managed. When Deleting, keep in mind that all seasons and additional information will also be deleted forever, so be sure you really want that to happen

To continue, click show seasons to manage the seasons

Here all seasons will be listed. To get started, click Create new Season in the Top left

Enter a name and click create to continue.

Seasons can also be viewed in Edited, viewed in detail and deleted. When deleted the season is lost forever, so be careful

To Continue click edit to add teams to the season.

To add a Team to a season, Enter the name and hit Add team. The team is added to the season. Teams that are not wanted can be deleted here

Add all the teams you want, and click Start Season, to create a schedule and standings. Be aware, that once a season has started, the teams can no longer be edited

When a season has started, the Standings become available from the seasons management page

On the standings page, the current standings as well as results can be seen. The Scores can be edited and saved. Upon saving, the new standings are calculated

I hope you enjoy this Application, and have Fun :)
