using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsManagement.Models
{
  public class DatabaseOptions
  {
    [JsonProperty("dbServer")]
    public string dbServer { get; set; }

    [JsonProperty("dbInstance")]
    public string dbInstance { get; set; }

    [JsonProperty("userName")]
    public string userName { get; set; }

    [JsonProperty("password")]
    public string password { get; set; }
  }
}
