using RocketLanding.Library;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace RocketLanding.App
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int userInputX, userInputY;

            string status = "R";
            const string CloseStr = "C";

            


            while (status.ToUpper() != CloseStr)
            {
                Console.WriteLine("Please Enter Location X,Y");
                string strLocation = Console.ReadLine();

                if(strLocation.ToUpper() == CloseStr)
                {
                    status = CloseStr;
                    continue;
                }

                var regex = new Regex(@"^([1-9]{1}[0-9]*)\,([1-9]{1}[0-9]*)$", RegexOptions.Singleline);

                var locationMatch = regex.Match(strLocation);

                if (!locationMatch.Success)
                {
                    Console.WriteLine("Invalid Location please try again");
                    continue;
                }

                userInputX = int.Parse(locationMatch.Groups[1].Value);
                userInputY = int.Parse(locationMatch.Groups[2].Value);

                LandingArea area = LandingArea.GetInstance;

                Console.WriteLine(area.GetLandingStatus(userInputX, userInputY));

            }


        }
    }
}
