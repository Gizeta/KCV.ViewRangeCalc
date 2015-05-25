using Grabacr07.KanColleWrapper;
using Grabacr07.KanColleWrapper.Models;
using System;
using System.Linq;

namespace Gizeta.KCV.ViewRangeCalc
{
    public class ViewRangeType4 : ViewRangeCalcLogic
    {
        public override sealed string Id
        {
            get { return "KCV.ViewRangeCalc.Type4"; }
        }

        public override string Name
        {
            get { return "新秋式簡易"; }
        }

        public override string Description
        {
            get
            {
                return @"(艦上爆撃機 × 0.6) + (艦上攻撃機 × 0.8) + (艦上偵察機 × 1.0)
+ (水上偵察機 × 1.2) + (水上爆撃機 × 1.1) + (探照灯 × 0.5)
+ (電探 × 0.6) + (√各艦毎の素索敵) + (司令部レベル × -0.4)";
            }
        }

        public override double Calc(Ship[] ships)
        {
            if (ships == null || ships.Length == 0) return 0;

            // http://wikiwiki.jp/kancolle/?%C6%EE%C0%BE%BD%F4%C5%E7%B3%A4%B0%E8#search-calc
            // > 参考ですが2-5式(秋)を水偵の補正2ではなく艦娘の√後素索敵を基準にする為、全ての係数を1.6841056で割って小数第2位で四捨五入した結果です。

            var itemScore = ships
                            .SelectMany(x => x.EquippedSlots)
                            .Select(x => x.Item.Info)
                            .GroupBy(
                                x => x.Type,
                                x => x.GetRawData().api_saku,
                                (type, scores) => new { type, score = scores.Sum() })
                            .Aggregate(.0, (score, item) => score + GetScore(item.type, item.score));

            var shipScore = ships
                            .Select(x => x.ViewRange - x.EquippedSlots.Sum(s => s.Item.Info.GetRawData().api_saku))
                            .Select(x => Math.Sqrt(x))
                            .Sum();

            var level = KanColleClient.Current.Homeport.Admiral.Level;
            var admiralScore = level * -0.4;

            return itemScore + shipScore + admiralScore;
        }

        private static double GetScore(SlotItemType type, int score)
        {
            switch (type)
            {
                case SlotItemType.艦上爆撃機:
                    return score * 0.6;
                case SlotItemType.艦上攻撃機:
                    return score * 0.8;
                case SlotItemType.艦上偵察機:
                    return score * 1.0;
                case SlotItemType.水上偵察機:
                    return score * 1.2;
                case SlotItemType.水上爆撃機:
                    return score * 1.1;
                case SlotItemType.小型電探:
                    return score * 0.6;
                case SlotItemType.大型電探:
                    return score * 0.6;
                case SlotItemType.探照灯:
                    return score * 0.5;
            }
            return .0;
        }
    }
}
