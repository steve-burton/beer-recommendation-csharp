using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace BeerRecommendation.Objects
{
  public class Brewery
  {
    private int _id;
    private string _name;
    private string _location;

    public Brewery(string newName, string newLocation, int newId = 0)
    {
      _id = newId;
      _name = newName;
      _location = newLocation;
    }

    //Static methods
    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM beers_breweries; DELETE FROM breweries;", conn);
      cmd.ExecuteNonQuery();

      if (conn != null) conn.Close();
    }

    public static void Delete(int breweryId)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM beers_breweries WHERE brewery_id = @BreweryId; DELETE FROM breweries = @BreweryId;", conn);
      cmd.Parameters.AddWithValue("@BreweryId", breweryId);
      cmd.ExecuteNonQuery();

      if (conn != null) conn.Close();
    }

    public static Brewery Find(int targetId)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM breweries WHERE id = @targetId;", conn);
      cmd.Parameters.AddWithValue("@targetId", targetId);

      SqlDataReader rdr = cmd.ExecuteReader();

      int newId = 0;
      string newName = null;
      string newLocation = null;

      while(rdr.Read())
      {
        newId = rdr.GetInt32(0);
        newName = rdr.GetString(1);
        newLocation = rdr.GetString(2);
      }
      Brewery foundBrewery = new Brewery(newName, newLocation, newId);

      if (rdr != null) rdr.Close();
      if (conn != null) conn.Close();

      return foundBrewery;
    }

    public static List<Brewery> GetAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM breweries;", conn);

      SqlDataReader rdr = cmd.ExecuteReader();

      var allBreweries = new List<Brewery>{};

      while(rdr.Read())
      {
        int newId = rdr.GetInt32(0);
        string newName = rdr.GetString(1);
        string newLocation = rdr.GetString(2);
        Brewery newBrewery = new Brewery(newName, newLocation, newId);
        allBreweries.Add(newBrewery);
      }
      if (rdr != null) rdr.Close();

      if (conn != null) conn.Close();

      return allBreweries;
    }

    //Other methods
    public List<Beer> GetBeers()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT beers.* From breweries JOIN beers_breweries ON (breweries.id = beers_breweries.brewery_id) JOIN beers ON (beers_breweries.beer_id = beers.id) WHERE breweries.id = @BreweryId;", conn);

      cmd.Parameters.AddWithValue("@BreweryId", this.GetId());

      SqlDataReader rdr = cmd.ExecuteReader();

      List<Beer> beers = new List<Beer>{};
      while(rdr.Read())
      {
        int newId = rdr.GetInt32(0);
        string newName = rdr.GetString(1);
        double newAbv = (rdr.IsDBNull(2))? 0.0 : rdr.GetDouble(2);
        double newIbu = (rdr.IsDBNull(3))? 0.0 : rdr.GetDouble(3);
        Beer newBeer = new Beer(newName, newAbv, newIbu, newId);

        beers.Add(newBeer);
      }
      if (rdr != null) rdr.Close();
      if (conn != null) conn.Close();

      return beers;
    }

    public void AddBeer(int beerId)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO beers_breweries (beer_id, brewery_id) VALUES (@BeerId, @BreweryId);", conn);
      cmd.Parameters.AddWithValue("@BeerId", beerId);
      cmd.Parameters.AddWithValue("@BreweryId", this.GetId());

      cmd.ExecuteNonQuery();
      if (conn != null) conn.Close();
    }

    public void RemoveBeer(int beerId)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM beers_breweries WHERE beer_id = @BeerId AND brewery_id = @BreweryId);", conn);
      cmd.Parameters.AddWithValue("@BeerId", beerId);
      cmd.Parameters.AddWithValue("@BreweryId", this.GetId());

      cmd.ExecuteNonQuery();
      if (conn != null) conn.Close();
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO breweries (name, location) OUTPUT INSERTED.id VALUES (@Name, @Location);", conn);

      cmd.Parameters.AddWithValue("@Name", this.GetName());
      cmd.Parameters.AddWithValue("@Location", this.GetLocation());

      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }

      if (rdr != null) conn.Close();

      if (conn != null) conn.Close();
    }

    //Overrides
    public override bool Equals(System.Object otherObject)
    {
      if (!(otherObject is Brewery))
      {
        return false;
      }
      else
      {
        Brewery testBrewery = (Brewery) otherObject;
        bool idEquality = this.GetId() == testBrewery.GetId();
        bool nameEquality = this.GetName() == testBrewery.GetName();
        bool locationEquality = this.GetLocation() == testBrewery.GetLocation();
        return (idEquality && nameEquality && locationEquality);
      }
    }

    public override int GetHashCode()
    {
      return this.GetName().GetHashCode();
    }

    //Getters and Setters
    public void SetId(int newId)
    {
      _id = newId;
    }
    public int GetId()
    {
      return _id;
    }

    public void SetName(string newName)
    {
      _name = newName;
    }
    public string GetName()
    {
      return _name;
    }

    public void SetLocation(string newLocation)
    {
      _location = newLocation;
    }
    public string GetLocation()
    {
      return _location;
    }
  }
}
