using System.Collections.Generic;
using System.IO;
using Microsoft.AspNet.Builder;
using Nancy;
using Nancy.Owin;
using Nancy.ViewEngines.Razor;

namespace BeerRecommendation
{
  public static class DBConfiguration
  {
    public static string ConnectionString =  "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=beer_recommendation;Integrated Security=SSPI";
  }
  public class Startup
  {
    public void Configure(IApplicationBuilder app)
    {
      app.UseOwin(x => x.UseNancy());
    }
  }
  public class CustomRootPathProvider : IRootPathProvider
  {
    public string GetRootPath()
    {
      return Directory.GetCurrentDirectory();
    }
  }
  public class RazorConfig : IRazorConfiguration
  {
    public IEnumerable<string> GetAssemblyNames()
    {
      return null;
    }

    public IEnumerable<string> GetDefaultNamespaces()
    {
      return null;
    }

    public bool AutoIncludeModelNamespace
    {
      get { return false; }
    }
  }
}
