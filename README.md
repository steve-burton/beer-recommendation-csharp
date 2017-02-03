#Beer Recommendation
===========================

Oregon Beer Central. Get recommendations for new beers to try, December 16, 2016

By Levi Bibo, Steve Burton, Anthony Bottemiller, and Erik Killops

##Description

This project is a database of Oregon beers that allows users to browse beers and breweries, create an account, and add and rate beers.

The app includes features to discover new beers. Click the Beer Recommendations link and enter your favorite beer to find similar brews.

##Specifications

| Description                                                                    | Input              | Output             |
|--------------------------------------------------------------------------------|--------------------|--------------------|
| Program will allow user to add a new beer to the DB                            | Hefeweizen         | 1                  |
| Program will allow user to find a specific beer in the DB                      | Hefeweizen         | Hefeweizen         |
| Program will allow user to find all beers in the DB                            | {2 beers}          | {2 beers}          |
| Program will be able to search for beers by name, abv, ibu, or location        | ibu: 70            | {list of beers}    |
| Program will allow user to edit beer in DB                                     | Hefeweizen: IPA    | IPA                |
| Program will allow user to remove beer from DB                                 | Hefeweizen: IPA    | IPA                |
| Program will allow user to add new brewery to DB                               | Widmer             | 1                  |
| Program will allow user to find a specific brewery in the DB                   | Widmer             | Widmer             |
| Program will allow user to find all breweries in the DB                        | {2 breweries}      | {2 breweries}      |
| Program will allow user to edit brewery in the DB                              | Widmer: Deschutes  | Deschutes          |
| Program will allow user to add beer to brewery                                 | Widmer: Hefeweizen | Widmer: Hefeweizen |
| Program will be able to find all beers made by a brewery                       | Widmer             | {list of beers}    |
| Program will allow user to create new account                                  | Steve              | 1                  |
| Program will allow user to remove account                                      | Steve              | 0                  |
| Program will allow user to create new account                                  | Steve              | 1                  |
| Program will allow user to rate a beer in the DB                               | 5                  | 5                  |
| Program will allow user to find a new beer based on a generated recommendation | {beer 1}           | {similar beer 2}   |

## Setup/Installation Requirements

_Requires Windows, .Net, SMSS, and SQL SERVER_

1. Clone repository.
2. In SSMS, open beer_recommendation.sql
3. Click execute.
4. Repeat steps 2-5 for beer_recommendation_test.sql
5. In Powershell,  run "> dnx kestrel" and visit "localhost:5004".

Alternatively, open the .sql files in a text editor and run the commands one at a time in a SQL command line.

An image of the schema is included for reference.


##Check out our GitHub accounts:
* [Levi Bibo.](https://www.github.com/levibibo)
* [Steve Burton.](https://www.github.com/steve-burton)
* [Erik Killops.](https://www.github.com/ekillops)
* [Anthony Bottemiller.](https://www.github.com/anthonybottemiller)

##Technologies used

_This site was built using C#, Nancy, SQL Server, CSS, Bootstrap and ASP.Net._

##License

_Created under an MIT license._

_Copyright (c) 2016 _
