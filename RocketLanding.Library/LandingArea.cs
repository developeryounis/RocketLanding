using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RocketLanding.Library
{
    public sealed class LandingArea
    {
        #region [Private Fields]
        private readonly LandingAreaOptions landingAreaOptions;
        private static LandingArea Instance = null;
        private static readonly object obj = new object();
        private Dictionary<string, LandingLocationStatus> platform;
        private Dictionary<string, int> cachedAreas;
        #endregion

        #region [Properties]
        public static LandingArea GetInstance
        {
            get
            {
                if(Instance == null)
                {
                    lock(obj)
                    {
                        if(Instance == null)
                        {

                            Instance = new LandingArea();
                        }
                    }
                }
                return Instance;
            }
        }
        #endregion

        #region [Constructor]
        private LandingArea()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            var config = builder.Build();
            landingAreaOptions = new LandingAreaOptions();
            config.GetSection("LandingArea").Bind(landingAreaOptions);

            platform = new Dictionary<string, LandingLocationStatus>();
            cachedAreas = new Dictionary<string, int>();
        }
        #endregion

        #region [Public Methods]
        public string GetLandingStatus(int X, int Y)
        {
            LandingLocationStatus status;
            lock (platform)
            {
                if (landingAreaOptions.AllowOnlyLastCheck)
                {
                    cachedAreas.Where(p => p.Value == 0)
                            .Select(s => s.Key)
                            .ToList()
                            .ForEach(e => cachedAreas[e] = 1);
                }


                if (IsXBetweenRange(X) && IsYBetweenRange(Y))
                {
                    if (platform.ContainsKey($"{X},{Y}"))
                    {
                        status = LandingLocationStatus.Clash;
                    }
                    else
                    {
                        if (IsXUnitSeparationValid(X, Y) && IsYUnitSeparationValid(X, Y))
                        {
                            AddToPlatform($"{X},{Y}", LandingLocationStatus.Clash);

                            AddToPlatform($"{X + 1},{Y}", LandingLocationStatus.UnitSeparation);
                            AddToPlatform($"{X - 1},{Y}", LandingLocationStatus.UnitSeparation);
                            AddToPlatform($"{X},{Y + 1}", LandingLocationStatus.UnitSeparation);
                            AddToPlatform($"{X},{Y - 1}", LandingLocationStatus.UnitSeparation);

                            status = LandingLocationStatus.OkForLanding;
                        }
                        else
                        {
                            status = LandingLocationStatus.Clash;
                        }
                    }
                }
                else
                {
                    status = LandingLocationStatus.OutOfPlatform;
                }

                if (landingAreaOptions.AllowOnlyLastCheck)
                {
                    cachedAreas.Where(p => p.Value == 1)
                                .Select(s => s.Key)
                                .ToList()
                                .ForEach(e =>
                                {
                                    platform.Remove(e);
                                    cachedAreas.Remove(e);
                                });
                }

                return status.ToString();
            }
        }
        #endregion

        #region [Private Methods]
        private void AddToPlatform(string Key, LandingLocationStatus status)
        {
            if (!platform.ContainsKey(Key))
            {
                platform.Add(Key, status);
                if (landingAreaOptions.AllowOnlyLastCheck)
                    cachedAreas.Add(Key, 0);
            }
        }

        private bool IsXBetweenRange(int X)
        {
            return X >= landingAreaOptions.LandingPlateformStartX && X <= landingAreaOptions.LandingPlateformStartX + landingAreaOptions.LandingPlateformSizeX;
        }

        private bool IsXUnitSeparationValid(int X, int Y)
        {
            return (!platform.ContainsKey($"{X + 1},{Y}") || platform[$"{X + 1},{Y}"] == LandingLocationStatus.UnitSeparation)
                && (!platform.ContainsKey($"{X - 1},{Y}") || platform[$"{X - 1},{Y}"] == LandingLocationStatus.UnitSeparation);
        }


        private bool IsYBetweenRange(int Y)
        {
            return Y >= landingAreaOptions.LandingPlateformStartY && Y <= landingAreaOptions.LandingPlateformStartY + landingAreaOptions.LandingPlateformSizeY;
        }

        private bool IsYUnitSeparationValid(int X, int Y)
        {
            return (!platform.ContainsKey($"{X},{Y + 1}") || platform[$"{X},{Y + 1}"] == LandingLocationStatus.UnitSeparation) 
                && (!platform.ContainsKey($"{X},{Y - 1}") || platform[$"{X},{Y - 1}"] == LandingLocationStatus.UnitSeparation);
        }

        #endregion
    } 

    
}
