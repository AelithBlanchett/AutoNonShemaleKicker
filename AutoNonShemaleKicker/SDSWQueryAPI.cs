using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace AutoNonShemaleKicker
{
    class SDSWQueryAPI
    {
        public static List<string> GetAllCharacters()
        {
            var returnedList = new List<string>();
            try
            {
                var client = new WebClient();
                var response = client.DownloadString("http://shemaledsgame.tk/FListCharacters/GetAllCharacters");
                var allCharacters = JArray.Parse(response);
                
                foreach (var item in allCharacters)
                {
                    if(item.Value<bool>("IsDeleted") == false)
                    {
                        returnedList.Add(item.Value<string>("FListCharacterName"));
                    }
                }
                Console.WriteLine($"{returnedList.Count} characters playing.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            
            return returnedList;
        }
    }
}
