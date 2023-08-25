using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeaviateNET
{
    public static class DistanceMetric
    {
        public const string Cosine = "cosine";
        public const string Dot = "dot";
        public const string L2Squared = "l2-squared";
        public const string Manhattan = "manhattan";
        public const string Hamming = "hamming";
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class VectorIndexConfigAttribute : Attribute
    {
        public string Distance { get; set; }

        public VectorIndexConfigAttribute(string distance = DistanceMetric.Cosine)
        {
            this.Distance = distance;
        }
    }


    [AttributeUsage(AttributeTargets.Class)]
    public class ReplicationConfigAttribute : Attribute
    {
        public int Factor { get; set; }

        public ReplicationConfigAttribute(int factor = 3)
        {
            this.Factor = factor;
        }
    }

    public static class PresetConfig
    {
        public const string En = "en";
        public const string None = "none";
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class IndexStopwordsAttribute : Attribute
    {
        public string Preset { get; set; }

        public string[] Additions { get; set; }

        public string[] Removals { get; set; }

        public IndexStopwordsAttribute(string preset = PresetConfig.En, string[]? additions = null, string[]? removals = null)
        {
            this.Preset = preset;

            if (additions != null) this.Additions = additions;
            else this.Additions = new string[0];

            if (removals != null) this.Removals = removals;
            else this.Removals = new string[0];
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class IndexTimestampsAttribute : Attribute
    {
        public bool Enabled { get; set; }

        public IndexTimestampsAttribute(bool enabled = true)
        {
            this.Enabled = enabled;
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class IndexNullStateAttribute : Attribute
    {
        public bool Enabled { get; set; }

        public IndexNullStateAttribute(bool enabled = true)
        {
            this.Enabled = enabled;
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class IndexPropertyLengthAttribute : Attribute
    {
        public bool Enabled { get; set; }

        public IndexPropertyLengthAttribute(bool enabled = true)
        {
            this.Enabled = enabled;
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class BM25IndexAttribute : Attribute
    {
        public float K1 { get; set; }
        public float B { get; set; }

        public BM25IndexAttribute(float k1 = 1.2f, float b = 0.75f)
        {
            this.K1 = k1;
            this.B = b;
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class MultiTenancyAttribute : Attribute
    {
        public bool Enabled { get; set; }

        public MultiTenancyAttribute(bool enabled = true)
        {
            this.Enabled = enabled;
        }
    }


    [AttributeUsage(AttributeTargets.Field)]
    public class TokenizationAttribute : Attribute
    {
        public PropertyTokenization Mode { get; set; }

        public TokenizationAttribute(PropertyTokenization mode = PropertyTokenization.Word)
        {
            this.Mode = mode;
        }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class IndexFilterableAttribute : Attribute
    {
        public bool Enabled { get; set; }

        public IndexFilterableAttribute(bool enabled = true)
        {
            this.Enabled = enabled;
        }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class IndexSearchableAttribute : Attribute
    {
        public bool Enabled { get; set; }

        public IndexSearchableAttribute(bool enabled = true)
        {
            this.Enabled = enabled;
        }
    }
}
