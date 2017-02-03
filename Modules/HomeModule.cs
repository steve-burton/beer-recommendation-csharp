using System;
using Nancy;
using Nancy.Cookies;
using System.Collections.Generic;
using BeerRecommendation.Objects;

namespace BeerRecommendation
{
	public class HomeModule : NancyModule
	{
		public HomeModule()
		{
			//Homepage
			Get["/"] = _ =>
			{
				return View["index.cshtml"];
			};

			//Login routes
			Get["/login"] = _ =>
			{
				return View["login.cshtml"];
			};
			Post["/login"] = _ =>
			{
				string userName = Request.Form["user-name"];
				string userPassword = Request.Form["password"];
				string passwordHash = Hash.CalculateHash(userPassword).ToString();
				int userId = User.CheckUserName(userName);
				var foundUser = User.Find(userId);

				if (userId != 0 && foundUser.CheckPassword(passwordHash))
				{
					NancyCookie idNumber = new NancyCookie("userId", userId.ToString());
					return View["login_success.cshtml", foundUser].WithCookie(idNumber);
				}
				else
				{
					bool userExists = false;
					return View["login.cshtml", userExists];
				}
			};
			Get["/logout"] = _ =>
			{
				int userId = int.Parse(Request.Cookies["userId"]);
				var foundUser = User.Find(userId);
				NancyCookie idNumber = new NancyCookie("userId", "0");
				return View["logout_success.cshtml", foundUser].WithCookie(idNumber);
			};

			//User routes
			Get["/users/profile"] = _ =>
			{
				int userId = int.Parse(Request.Cookies["userId"]);
				User foundUser = User.Find(userId);
				return View["user.cshtml", foundUser];
			};
			Get["/users/new"] = _ =>
			{
				return View["new_user.cshtml"];
			};
			Post["/users/new/success"] = _ =>
			{
				string name = Request.Form["name"];
				string password = Request.Form["password"];
				string passwordHash = Hash.CalculateHash(password).ToString();
				if (!(User.UserExists(name)))
				{
					User newUser = new User(name, passwordHash);
					newUser.Save();
					return View["new_user_success.cshtml", newUser];
				}
				else
				{
					bool userExists = true;
					return View["new_user.cshtml", userExists];
				}
			};

			//Recommendation routes
			Get["/recommend"] = _ =>
			{
				List<Beer> allBeers = Beer.GetAll();
				return View["recommend_form.cshtml", allBeers];
			};
			Get["/recommend/results"] = _ =>
			{
				List<Beer> recommendedBeers = new List<Beer>{};
				if (!(Request.Cookies.ContainsKey("userId")) || (Request.Cookies["userId"] == "0"))
				{
					User guest = new User("Guest");
					int beerId = int.Parse(Request.Query["beer-id"]);
					int listSize = int.Parse(Request.Query["list-size"]);
					recommendedBeers = guest.GetRecommendations(beerId, listSize);
				}
				else
				{
					int userId = int.Parse(Request.Cookies["userId"]);
					User foundUser = User.Find(userId);
					int beerId = int.Parse(Request.Query["beer-id"]);
					int listSize = int.Parse(Request.Query["list-size"]);
					recommendedBeers = foundUser.GetRecommendations(beerId, listSize);
				}
				return View["recommend_result.cshtml", recommendedBeers];
			};

			//Beer routes
			Get["/beers"] = _ =>
			{
				List<Beer> allBeers = Beer.GetAll();
				return View["beers.cshtml", allBeers];
			};
			Get["/beers/ordered/{col}"] = parameters =>
			{
				List<Beer> allBeers = Beer.GetAll(parameters.col);
				return View["beers.cshtml", allBeers];
			};
			Get["/beers/search"] = _ =>
			{
				return View["search_beers.cshtml"];
			};
			Post["/beers/search"] = _ =>
			{
				string searchBy = Request.Form["search-type"];
				List<Beer> foundBeers = new List<Beer> {};

				if (searchBy == "abv" || searchBy == "ibu")
				{
					try
					{
						double searchInput = Request.Form["search-input"];
						foundBeers = Beer.Search(searchBy, searchInput);
					}
					catch
					{
						//do nothing, return empty list
					}
				}
				else
				{
					string searchInput = Request.Form["search-input"];
					foundBeers = Beer.Search(searchBy, searchInput);
				}
				return View["search_beers.cshtml", foundBeers];
			};
			Get["/beers/{id}"] = parameters =>
			{
				Beer foundBeer = Beer.Find(parameters.id);
				return View["beer.cshtml", foundBeer];
			};
			Post["/beers/{id}"] = parameters =>
			{
				int userId = int.Parse(Request.Cookies["userId"]);
				int beerId = int.Parse(parameters.id);
				int rating = int.Parse(Request.Form["beerRating"]);
				User foundUser = User.Find(userId);
				foundUser.RateBeer(beerId, rating);
				Beer foundBeer = Beer.Find(beerId);
				return View["beer.cshtml", foundBeer];
			};

			//Brewery routes
			Get["/breweries"] = _ =>
			{
				List<Brewery> allBreweries = Brewery.GetAll();
				return View["breweries.cshtml", allBreweries];
			};
			Get["/breweries/{id}"] = parameters =>
			{
				Brewery foundBrewery = Brewery.Find(parameters.id);
				return View["brewery.cshtml", foundBrewery];
			};

			//Database routes
			Get["/database"] = _ =>
			{
				return View["database_tools.cshtml"];
			};
			Get["/database/beers/new"] = _ =>
			{
				List<Brewery> allBreweries = Brewery.GetAll();
				return View["new_beer.cshtml", allBreweries];
			};
			Post["/database/beers/new/success"] = _ =>
			{
				string name = Request.Form["name"];
				double abv = (double) Request.Form["abv"];
				double ibu = (double) Request.Form["ibu"];
				Beer newBeer = new Beer(name, abv, ibu);
				newBeer.Save();
				return View["new_beer_success.cshtml", newBeer];
			};

			Get["/database/beers/update"] = _ => {
				int beerId = int.Parse(Request.Query["beer-id"]);
				Beer foundBeer = Beer.Find(beerId);
				List<Brewery> allBreweries = Brewery.GetAll();

				Dictionary<string, object> beerAndBreweries = new Dictionary<string, object>()
				{
					{"beer", foundBeer},
					{"breweries", allBreweries}
				};

				return View["beer_update.cshtml", beerAndBreweries];
			};

			Patch["/database/beers/{id}"] = parameters => {
				string name = Request.Form["name"];
				double abv = (double) Request.Form["abv"];
				double ibu = (double) Request.Form["ibu"];
				int breweryId = int.Parse(Request.Form["brewery"]);

				Beer foundBeer = Beer.Find(int.Parse(parameters.id));
				foundBeer.Update(name, abv, ibu);
				foundBeer.RemoveAllBreweries();
				foundBeer.AddBrewery(breweryId);
				return View["beer.cshtml", foundBeer];
			};
			
			Get["/database/breweries/new"] = _ =>
			{
				return View["new_brewery.cshtml"];
			};
			Post["/database/breweries/new/success"] = _ =>
			{
				string name = Request.Form["name"];
				string location = Request.Form["location"];
				Brewery newBrewery = new Brewery(name, location);
				newBrewery.Save();
				return View["new_brewery_success.cshtml", newBrewery];
			};
		}
	}
}
