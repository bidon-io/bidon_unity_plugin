using System;
using System.Diagnostics.CodeAnalysis;

// ReSharper disable once CheckNamespace
namespace Bidon.Mediation
{
    [Serializable]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "NotAccessedField.Global")]
    public class BidonAdUnitInfo
    {
        public string DemandId;

        public string Label;

        public double Price;

        public string Uid;

        public string BidType;

        public long FillStartTs;

        public long FillFinishTs;

        public long Timeout;

        public string Status;

        public string ErrorMessage;

        [NonSerialized]
        public string Ext;

        public string ToJsonString(bool isPretty = false) => UnityEngine.JsonUtility.ToJson(this, isPretty);
    }
}
