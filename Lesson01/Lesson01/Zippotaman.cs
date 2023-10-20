using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson01
{
    
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Place
    {
        public string placename { get; set; }
        public string longitude { get; set; }
        public string state { get; set; }

        public string stateabbreviation { get; set; }
        public string latitude { get; set; }
    }

    public class Zippotaman
    {
        public string postcode { get; set; }
        public string country { get; set; }

        public string countryabbreviation { get; set; }
        public List<Place> places { get; set; }
    }


}
