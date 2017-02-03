using Xunit;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace BeerRecommendation.Objects
{
  public class BeerTest : IDisposable
  {
    public BeerTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=beer_recommendation_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Save_AddSingleBeerToDB_1()
    {
      //Arrange
      Beer newBeer = new Beer("Alpha IPA", 6.8, 70.0);

      //Act
      newBeer.Save();
      List<Beer> allBeers = Beer.GetAll();

      //Assert
      Assert.Equal(1, allBeers.Count);
    }

    [Fact]
    public void Find_GetSingleBeerFromDB_EquivalentBeer()
    {
      //Arrange
      Beer beer1 = new Beer("Alpha IPA", 6.8, 70.0);
      Beer beer2 = new Beer("Beta IPA", 6.8, 70.0);
      beer1.Save();
      beer2.Save();

      //Act
      Beer testBeer = Beer.Find(beer1.GetId());

      //Assert
      Assert.Equal(beer1, testBeer);
    }

    [Fact]
    public void GetAll_ReturnAListOfAllBeers_ListOfBeers()
    {
      //Arrange
      Beer beer1 = new Beer("Alpha IPA", 6.8, 70.0);
      Beer beer2 = new Beer("Beta IPA", 6.8, 70.0);
      beer1.Save();
      beer2.Save();

      //Act
      List<Beer> testBeers = Beer.GetAll();
      List<Beer> expectedBeers = new List<Beer> {beer1, beer2};

      //Assert
      Assert.Equal(expectedBeers, testBeers);
    }

    [Fact]
    public void ChangeName_UpdateSingleBeerDetails_EquivalentBeer()
    {
      //Arrange
      Beer expectedBeer = new Beer("Alpha IPA", 6.8, 70.0);

      //Act
      expectedBeer.Save();
      expectedBeer.Update("Beta IPA", 7.0, 70.0);
      Beer foundBeer = Beer.Find(expectedBeer.GetId());

      //Assert
      Assert.Equal(expectedBeer, foundBeer);
    }

    [Fact]
    public void DeleteBeer_RemoveSingleBeerFromDB_0()
    {
      //Arrange
      Beer newBeer = new Beer("Alpha IPA", 6.8, 70.0);

      //Act
      newBeer.Save();
      Beer.DeleteBeer(newBeer.GetId());
      List<Beer> allBeers = Beer.GetAll();

      //Assert
      Assert.Equal(0, allBeers.Count);
    }

    [Fact]
    public void GetRating_GetAverageRatingForBeer_3()
    {
      //Arrange
      Beer newBeer = new Beer("Alpha IPA", 6.8, 70.0);
      User user1 = new User("Bob");
      User user2 = new User("Fred");
      newBeer.Save();
      user1.Save();
      user2.Save();

      //Act
      user1.RateBeer(newBeer.GetId(), 4);
      user2.RateBeer(newBeer.GetId(), 2);

      //Assert
      Assert.Equal(3.00F, newBeer.GetRating());
    }

    [Fact]
    public void Search_Name_ReturnsExpectedBeer()
    {
      //Arrange
      Beer beer1 = new Beer("IPA", 6.3, 74.0);
      Beer beer2 = new Beer("Summer Ale", 4.7, 26.0);
      beer1.Save();
      beer2.Save();
      List<Beer> expectedReturn = new List<Beer> { beer1 };
      //Act
      List<Beer> searchResults = Beer.Search("Name", "IPA");
      //Assert
      Assert.Equal(expectedReturn, searchResults);
    }

    [Fact]
    public void Search_Abv_ReturnsExpectedBeer()
    {
      //Arrange
      Beer beer1 = new Beer("IPA", 6.3, 74.0);
      Beer beer2 = new Beer("Summer Ale", 4.7, 26.0);
      beer1.Save();
      beer2.Save();
      List<Beer> expectedReturn = new List<Beer> { beer1 };
      //Act
      List<Beer> searchResults = Beer.Search("ABV", 6.3);
      //Assert
      Assert.Equal(expectedReturn, searchResults);
    }

    [Fact]
    public void Search_Ibu_ReturnsExpectedBeer()
    {
      //Arrange
      Beer beer1 = new Beer("IPA", 6.3, 74.0);
      Beer beer2 = new Beer("Summer Ale", 4.7, 26.0);
      beer1.Save();
      beer2.Save();
      List<Beer> expectedReturn = new List<Beer> { beer1 };
      //Act
      List<Beer> searchResults = Beer.Search("ibu", 73, 10);
      //Assert
      Assert.Equal(expectedReturn, searchResults);
    }

    public void Dispose()
    {
      Beer.DeleteAll();
      User.DeleteAll();
    }
  }
}
