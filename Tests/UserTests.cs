using Xunit;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace BeerRecommendation.Objects
{
  public class UserTest : IDisposable
  {
    public UserTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=beer_recommendation_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Save_AddSingleUserToDB_1()
    {
      //Arrange
      User newUser = new User("Bob");

      //Act
      newUser.Save();
      List<User> allUsers = User.GetAll();

      //Assert
      Assert.Equal(1, allUsers.Count);
    }

    [Fact]
    public void Find_GetSingleUserFromDB_EquivalentUser()
    {
      //Arrange
      User user1 = new User("Bob");
      User user2 = new User("Fred");
      user1.Save();
      user2.Save();

      //Act
      User testUser = User.Find(user1.GetId());

      //Assert
      Assert.Equal(user1, testUser);
    }

    [Fact]
    public void GetAll_ReturnAListOfAllUsers_ListOfUsers()
    {
      //Arrange
      User user1 = new User("Bob");
      User user2 = new User("Fred");
      user1.Save();
      user2.Save();

      //Act
      List<User> testUsers = User.GetAll();
      List<User> expectedUsers = new List<User> {user1, user2};

      //Assert
      Assert.Equal(expectedUsers, testUsers);
    }

    [Fact]
    public void Update_UpdateUserName_EquivalentUser()
    {
      //Arrange
      User expectedUser = new User("Bob");

      //Act
      expectedUser.Save();
      expectedUser.Update("Robert");
      User foundUser = User.Find(expectedUser.GetId());

      //Assert
      Assert.Equal(expectedUser, foundUser);
    }

    [Fact]
    public void DeleteUser_RemoveSingleUserFromDB_0()
    {
      //Arrange
      User newUser = new User("Robert");

      //Act
      newUser.Save();
      User.DeleteUser(newUser.GetId());
      List<User> allUsers = User.GetAll();

      //Assert
      Assert.Equal(0, allUsers.Count);
    }

    [Fact]
    public void RateBeer_AddSingleBeerRating_1()
    {
      //Arrange
      User newUser = new User("Bob");
      Beer newBeer = new Beer("Alpha IPA", 6.8, 70.0);
      newUser.Save();
      newBeer.Save();

      //Act
      newUser.RateBeer(newBeer.GetId(), 5);
      List<Beer> ratedBeers = newUser.GetRated();
      Beer testBeer = (Beer) ratedBeers[0];

      //Assert
      Assert.Equal(newBeer, testBeer);
    }

    [Fact]
    public void RateBeer_ChangeRating_4()
    {
      //Arrange
      User newUser = new User("Bob");
      Beer newBeer = new Beer("Alpha IPA", 6.8, 70.0);
      newUser.Save();
      newBeer.Save();

      //Act
      newUser.RateBeer(newBeer.GetId(), 5);
      newUser.RateBeer(newBeer.GetId(), 4);
      float testRating = newBeer.GetRating();

      //Assert
      Assert.Equal(4.0, testRating);
    }

    [Fact]
    public void CheckUserName_ReturnsUserId_true()
    {
      User newUser = new User("Bob");
      newUser.Save();

      int foundId = User.CheckUserName("Bob");

      Assert.Equal(newUser.GetId(), foundId);
    }

    [Fact]
    public void CheckUserName_Returns0IfNameNotOnTable_true()
    {
      User newUser = new User("Bob");
      newUser.Save();

      int foundId = User.CheckUserName("Joe");

      Assert.Equal(0, foundId);
    }

    [Fact]
    public void GetRecommendations_ReturnListOfBeersWithPerfectMatch_5()
    {
      //Arrange
      User newUser = new User("Bob");
      newUser.Save();
      Beer beer1 = new Beer("Beer One", 5.0, 5.0);
      beer1.Save();
      Beer beer2 = new Beer("Beer Two", 5.0, 5.0);
      beer2.Save();
      Beer beer3 = new Beer("Beer Three", 5.0, 5.0);
      beer3.Save();
      Beer beer4 = new Beer("Beer Four", 5.0, 5.0);
      beer4.Save();
      Beer beer5 = new Beer("Beer Five", 5.0, 5.0);
      beer5.Save();
      Beer beer6 = new Beer("Beer Six", 5.0, 5.0);
      beer6.Save();
      Beer beer7 = new Beer("Beer Seven", 9.0, 9.0);
      beer7.Save();

      //Act
      List<Beer> recommendedBeers = newUser.GetRecommendations(beer1.GetId());

      //Assert
      Assert.Equal(5, recommendedBeers.Count);
    }

    [Fact]
    public void GetRecommendations_ReturnListOfBeersWith1RangeIncrease_5()
    {
      //Arrange
      User newUser = new User("Bob");
      newUser.Save();
      Beer beer1 = new Beer("Beer One", 5.0, 5.0);
      beer1.Save();
      Beer beer2 = new Beer("Beer Two", 5.0, 5.0);
      beer2.Save();
      Beer beer3 = new Beer("Beer Three", 5.0, 5.0);
      beer3.Save();
      Beer beer4 = new Beer("Beer Four", 5.0, 5.0);
      beer4.Save();
      Beer beer5 = new Beer("Beer Five", 6.0, 5.0);
      beer5.Save();
      Beer beer6 = new Beer("Beer Six", 5.0, 6.0);
      beer6.Save();
      Beer beer7 = new Beer("Beer Seven", 9.0, 9.0);
      beer7.Save();

      //Act
      List<Beer> recommendedBeers = newUser.GetRecommendations(beer1.GetId());

      //Assert
      Assert.Equal(5, recommendedBeers.Count);
    }

    [Fact]
    public void GetRecommendations_ReturnOnlyFiveMatchingBeers_5()
    {
      //Arrange
      User newUser = new User("Bob");
      newUser.Save();
      Beer beer1 = new Beer("Beer One", 5.0, 5.0);
      beer1.Save();
      Beer beer2 = new Beer("Beer Two", 5.0, 5.0);
      beer2.Save();
      Beer beer3 = new Beer("Beer Three", 5.0, 5.0);
      beer3.Save();
      Beer beer4 = new Beer("Beer Four", 5.0, 5.0);
      beer4.Save();
      Beer beer5 = new Beer("Beer Five", 5.0, 5.0);
      beer5.Save();
      Beer beer6 = new Beer("Beer Six", 5.0, 5.0);
      beer6.Save();
      Beer beer7 = new Beer("Beer Seven", 5.0, 5.0);
      beer7.Save();

      //Act
      List<Beer> recommendedBeers = newUser.GetRecommendations(beer1.GetId());

      //Assert
      Assert.Equal(5, recommendedBeers.Count);
    }

    [Fact]
    public void GetRecommendations_ReturnOnlytenMatchingBeers_10()
    {
      //Arrange
      User newUser = new User("Bob");
      newUser.Save();
      Beer beer1 = new Beer("Beer One", 5.0, 5.0);
      beer1.Save();
      Beer beer2 = new Beer("Beer Two", 5.0, 5.0);
      beer2.Save();
      Beer beer3 = new Beer("Beer Three", 5.0, 5.0);
      beer3.Save();
      Beer beer4 = new Beer("Beer Four", 5.0, 5.0);
      beer4.Save();
      Beer beer5 = new Beer("Beer Five", 5.0, 5.0);
      beer5.Save();
      Beer beer6 = new Beer("Beer Six", 5.0, 5.0);
      beer6.Save();
      Beer beer7 = new Beer("Beer Seven", 5.0, 5.0);
      beer7.Save();
      Beer beer8 = new Beer("Beer Eight", 5.0, 5.0);
      beer8.Save();
      Beer beer9 = new Beer("Beer Nine", 5.0, 5.0);
      beer9.Save();
      Beer beer10 = new Beer("Beer Ten", 5.0, 5.0);
      beer10.Save();
      Beer beer11 = new Beer("Beer Eleven", 5.0, 5.0);
      beer11.Save();
      Beer beer12 = new Beer("Beer Twelve", 5.0, 5.0);
      beer12.Save();

      //Act
      List<Beer> recommendedBeers = newUser.GetRecommendations(beer1.GetId(), 10);

      //Assert
      Assert.Equal(10, recommendedBeers.Count);
    }

    public void Dispose()
    {
      Beer.DeleteAll();
      User.DeleteAll();
    }
  }
}
