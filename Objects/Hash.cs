using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace BeerRecommendation.Objects
{
  public class Hash
  {
    public static UInt64 CalculateHash(string read)
    {
      UInt64 hashedValue = 3074457345618258791ul;
      for(int i=0; i<read.Length; i++)
      {
          hashedValue += read[i];
          hashedValue *= 3074457345618258799ul;
      }
      return hashedValue;
    }
  }
}
