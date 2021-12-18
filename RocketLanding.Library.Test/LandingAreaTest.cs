using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace RocketLanding.Library.Test
{
    public class LandingAreaTest
    {
        private LandingArea _landingArea; 
        public LandingAreaTest()
        {
            _landingArea = LandingArea.GetInstance;
        }

        public IEnumerable<LandingLocationData> GetData_AllOnlyLastChecks()
        {
            return new List<LandingLocationData>
            {
                new LandingLocationData { X = 5, Y = 5, Status = LandingLocationStatus.OkForLanding},
                new LandingLocationData { X = 5, Y = 5, Status = LandingLocationStatus.Clash },
                new LandingLocationData { X = 5, Y = 6, Status = LandingLocationStatus.OkForLanding },
                new LandingLocationData { X = 5, Y = 7, Status = LandingLocationStatus.Clash },
                new LandingLocationData { X = 5, Y = 7, Status = LandingLocationStatus.OkForLanding },
                new LandingLocationData { X = 15, Y = 17, Status = LandingLocationStatus.OutOfPlatform },
                new LandingLocationData { X = -1, Y = -1, Status = LandingLocationStatus.OutOfPlatform }
            };
        }

        public IEnumerable<LandingLocationData> GetData_AllowAllChecks()
        {
            return new List<LandingLocationData>
            {
                new LandingLocationData { X = 5, Y = 5, Status = LandingLocationStatus.OkForLanding},
                new LandingLocationData { X = 5, Y = 5, Status = LandingLocationStatus.Clash },
                new LandingLocationData { X = 5, Y = 6, Status = LandingLocationStatus.Clash },
                new LandingLocationData { X = 5, Y = 7, Status = LandingLocationStatus.OkForLanding },
                new LandingLocationData { X = 15, Y = 17, Status = LandingLocationStatus.OutOfPlatform },
                new LandingLocationData { X = -1, Y = -1, Status = LandingLocationStatus.OutOfPlatform }
            };
        }

        [Fact]
        public void GetLandingStatus_AllowAllChecks()
        {
 
            foreach (var locationData in GetData_AllowAllChecks())
            {
                var result = _landingArea.GetLandingStatus(locationData.X, locationData.Y);

                Assert.Equal(result, locationData.Status.ToString());
            }
            
        }


        [Fact]
        public void GetLandingStatus_AllOnlyLastChecks()
        {

            foreach (var locationData in GetData_AllOnlyLastChecks())
            {
                var result = _landingArea.GetLandingStatus(locationData.X, locationData.Y);

                Assert.Equal(result, locationData.Status.ToString());
            }
        }


        [Fact]
        public void GetLandingStatus_AllOnlyLastChecks_Parallel()
        {
            string result1 = "", result2 = "";
            Parallel.Invoke(() =>
            {
                result1 = _landingArea.GetLandingStatus(5, 5);
                result2 = _landingArea.GetLandingStatus(5, 5);
            });

            Assert.NotEqual(result1, result2);
            Assert.True(result1 == LandingLocationStatus.OkForLanding.ToString() || result2 == LandingLocationStatus.OkForLanding.ToString());
            Assert.True(result1 == LandingLocationStatus.Clash.ToString() || result2 == LandingLocationStatus.Clash.ToString());
        }
    }

    public class LandingLocationData
    {
        public int X { get; set; }

        public int Y { get; set; }

        public LandingLocationStatus Status { get; set; }
    }
}
