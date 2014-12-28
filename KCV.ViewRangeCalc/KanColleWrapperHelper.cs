using Grabacr07.KanColleWrapper.Models;
using System;
using System.Reflection;

namespace Gizeta.KCV.ViewRangeCalc
{
    public static class RawDataWrapperHelper
    {
        public static T GetRawData<T>(this RawDataWrapper<T> source) where T : class
        {
            Type type = typeof(RawDataWrapper<T>);
            return type.GetProperty("RawData", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).GetValue(source) as T;
        }
    }

    public static class FleetHelper
    {
        public static void SetTotalViewRange(this Fleet fleet, int value)
        {
            Type type = typeof(Fleet);
            type.GetProperty("TotalViewRange", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).SetValue(fleet, value);
        }
    }
}
