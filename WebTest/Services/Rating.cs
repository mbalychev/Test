using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebTest.Models;

namespace WebTest.Services
{
    public static class Rating
    {
        //private static IServices<OrgRatingsModel> rating = null;
        public static async Task<bool> GetLoadAsync()
        {
            //if (rating == null)
            //    rating = service;
            HttpClient client = new HttpClient();
            HttpResponseMessage response = null;
            try
            {
                response = await client.GetAsync("http://localhost:29461/weatherforecast");
                if (response.IsSuccessStatusCode)
                {
                    byte[] res = await response.Content.ReadAsByteArrayAsync();
                    using (FileStream fstream = new FileStream($"C:\\Users\\Михаил\\Downloads\\note.csv", FileMode.OpenOrCreate))
                    {
                        fstream.Write(res, 0, res.Length);
                    }
                    //ParseFile(res);
                    using (var reader = new StreamReader("C:\\Users\\Михаил\\Downloads\\note.csv"))
                    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                    {
                        List<RatingModel> models = csv.GetRecords<RatingModel>().ToList();
                    }

                    return true;
                }
                else
                {
                    throw new Exception("connection error: " + response.StatusCode);
                }
            }
            catch (Exception e)
            {
                throw new Exception($"connecting error: {e.Message}");
            }
        }

        private static void ParseFile(byte[] bytes)
        {
            //char[] splits = new char[] { ';', '\n', '\r' };
            //StringBuilder builder = new StringBuilder(Encoding.Default.GetString(bytes));
            //string[] strs = Encoding.Default.GetString(bytes).Split(splits);
            //for (int i = 0; i < strs.Length; i++)
            //{
            //    await rating.CreateAsync(strs[i], strs[++i]);
            //}
        }
    }
}
