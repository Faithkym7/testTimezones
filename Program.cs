using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Collections;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using System.Linq;
using testTimezones;

namespace testTimezones
{
    public class CustomTimeZoneInfo
    {
        public string TimeZone { get; set; }
        public decimal TimeCoordinates { get; set; }
    }
    public class AzureRegion
    {
        public string RegionName { get; set; }
        public string RegionTimezone { get; set; }
        public decimal RegionCoordinates { get; set; }
    }
    class Program
    {     
        static void Main(string[] args)
        {

            string connectionString = "Data Source=DESKTOP-RMKJI6V\\SQLEXPRESS;Initial Catalog=Timezone;Integrated Security=True;Connect Timeout=30;Encrypt=False;";
            List<CustomTimeZoneInfo> timeZones = LoadTimeZones(connectionString);


            List<AzureRegion> azureRegions = LoadAzureRegions(connectionString);

            Console.WriteLine("Enter your timezone");
            string UserTimeZone = Console.ReadLine();

            bool timeZoneExists = timeZones.Any(tz => string.Equals(tz.TimeZone, UserTimeZone, StringComparison.OrdinalIgnoreCase));
            if (timeZoneExists)
            {


                //AzureRegion region = FindClosestAzureRegion(UserTimeZone, azureRegions);
                //Console.WriteLine("Your azure region names:" + region.RegionName);

                AzureRegion azureRegion = FindClosestAzureRegion(timeZones, azureRegions);


                Console.WriteLine("Your azure region names:" + azureRegion.RegionName);
            }
            else
            {
                Console.WriteLine("Region not found");
            }     
            



            

            Console.ReadKey();

        }

        static List<CustomTimeZoneInfo> LoadTimeZones(string connectionString)
        {
            List<CustomTimeZoneInfo> timeZones = new List<CustomTimeZoneInfo>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT timezone, timeCoordinates FROM USTimezones";
                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string timezone = reader.GetString(0);
                        decimal timeCoordinates = reader.GetDecimal(1);

                        CustomTimeZoneInfo timeZone = new CustomTimeZoneInfo
                        {
                            TimeZone = timezone,
                            TimeCoordinates = timeCoordinates
                        };
                        timeZones.Add(timeZone);
                    }
                }
            }

            return timeZones;
        }
        static List<AzureRegion> LoadAzureRegions(string connectionString)
        {
            List<AzureRegion> azureRegions = new List<AzureRegion>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT regionName, regionTimezone, regionCoordinates FROM azureRegion";
                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string regionName = reader.GetString(0);
                        string regionTimezone = reader.GetString(1);
                        decimal regionCoordinates = reader.GetDecimal(2);

                        AzureRegion azureRegion = new AzureRegion
                        {
                            RegionName = regionName,
                            RegionTimezone = regionTimezone,
                            RegionCoordinates = regionCoordinates
                        };

                        azureRegions.Add(azureRegion); // Add the created AzureRegion object to the list
                    }
                }
            }

            return azureRegions;
        }



        static AzureRegion FindClosestAzureRegion(List<CustomTimeZoneInfo> timeZones, List<AzureRegion> azureRegions)
        {
            AzureRegion closestRegion = null;
            decimal minDistance = decimal.MaxValue;
            //decimal minDistance = 0;

            foreach (CustomTimeZoneInfo timeZone in timeZones)
                {
                foreach (AzureRegion region in azureRegions)
                {
                    decimal distance = Math.Abs(timeZone.TimeCoordinates - region.RegionCoordinates);

                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        closestRegion = region;
                    }
                }
            }

            return closestRegion;
        }
        //static AzureRegion FindClosestAzureRegion(string userTimeZone, List<AzureRegion> azureRegions)
        //{
        //    AzureRegion closestRegion = null;
        //    decimal minDistance = decimal.MaxValue;

        //    foreach (AzureRegion region in azureRegions)
        //    {
        //        if (string.Equals(region.RegionTimezone, userTimeZone, StringComparison.OrdinalIgnoreCase))
        //        {
        //            decimal distance = Math.Abs(region.RegionCoordinates);

        //            if (distance < minDistance)
        //            {
        //                minDistance = distance;
        //                closestRegion = region;
        //            }
        //        }
        //    }

        //    return closestRegion;
        //}




    }
}














