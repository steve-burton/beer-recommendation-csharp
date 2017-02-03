using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeerDb
{
    public class DB
    {
        public static SqlConnection Connection()
        {
            SqlConnection conn = new SqlConnection(DBConfiguration.ConnectionString);
            return conn;
        }
    }
    public static class DBConfiguration
    {
        public static string ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=beer_recommendation;Integrated Security=SSPI";
    }

    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Check 1");
            string path = "../../oregonBeers.txt";

            try
            {
                SqlConnection conn = DB.Connection();
                conn.Open();
                Console.WriteLine("Check 2");
                using (StreamReader rdr = new StreamReader(path))
                {
                    Console.WriteLine("Check 3");

                    string line;
                    int counter = 0;
                    int beerId = 0;
                    int breweryId = 0;
                    while ((line = rdr.ReadLine()) != null)
                    {
                        counter++;
                        string[] beerEntry = line.Split(';');
                        string breweryName = beerEntry[0];
                        string name = beerEntry[1];
                        double abv = Convert.ToDouble(beerEntry[2]);
                        double ibu = Convert.ToDouble(beerEntry[3]);
                        string breweryLocation = beerEntry[5];

                        SqlCommand beerCmd = new SqlCommand("INSERT INTO beers (name, abv, ibu) OUTPUT INSERTED.id VALUES (@Name, @Abv, @Ibu);", conn);
                        if (name != "-1")
                        {
                            beerCmd.Parameters.AddWithValue("@Name", name);
                        }
                        else
                        {
                            Console.WriteLine(counter.ToString());
                            Console.ReadLine();
                        }
                        if ((abv == -1) && (ibu == -1))
                        {
                            beerCmd.CommandText = "INSERT INTO beers (name) OUTPUT INSERTED.id VALUES (@Name);";
                        }
                        else
                        {
                            if (abv != -1)
                            {
                                beerCmd.Parameters.AddWithValue("@Abv", abv);
                            }
                            else
                            {
                                beerCmd.CommandText = "INSERT INTO beers (name, ibu) OUTPUT INSERTED.id VALUES (@Name, @Ibu);";
                            }
                            if (ibu != -1)
                            {
                                beerCmd.Parameters.AddWithValue("@Ibu", ibu);
                            }
                            else
                            {
                                beerCmd.CommandText = "INSERT INTO beers (name, abv) OUTPUT INSERTED.id VALUES (@Name, @Abv);";
                            }
                        }
                        SqlDataReader beerRdr = beerCmd.ExecuteReader();
                        
                        while (beerRdr.Read())
                        {
                          beerId = beerRdr.GetInt32(0);
                        }
                        if (beerRdr != null) beerRdr.Close();

                        SqlCommand breweryCmd = new SqlCommand("IF NOT EXISTS (SELECT * FROM breweries WHERE name = @BreweryName) BEGIN INSERT INTO breweries (name, location) OUTPUT INSERTED.id VALUES (@BreweryName, @Location) END;", conn);
                        breweryCmd.Parameters.AddWithValue("@BreweryName", breweryName);
                        breweryCmd.Parameters.AddWithValue("@Location", breweryLocation);
                        SqlDataReader breweryRdr = breweryCmd.ExecuteReader();
                        
                        while (breweryRdr.Read())
                        {
                          breweryId = breweryRdr.GetInt32(0);
                        }
                        if (breweryRdr != null) breweryRdr.Close();

                        SqlCommand beersBreweriesCmd = new SqlCommand("INSERT INTO beers_breweries (beer_id, brewery_id) VALUES (@BeerId, @BreweryId);", conn);
                        beersBreweriesCmd.Parameters.AddWithValue("@BeerId", beerId);
                        beersBreweriesCmd.Parameters.AddWithValue("@BreweryId", breweryId);
                        beersBreweriesCmd.ExecuteNonQuery();
                    }
                }
                if (conn != null) conn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }
        }
    }
}
