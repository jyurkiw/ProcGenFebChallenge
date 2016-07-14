using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformerLSystem.Models.Configuration
{
    public class Language2D
    {
        private IDictionary<Noun2D, IList<Noun2D>> _languageDict;
        private Random _rand;

        /// <summary>
        /// Filename constructor. Reads the file, verifies the integrity of the language, and loads the language.
        /// </summary>
        /// <param name="filename"></param>
        public Language2D(string filename)
        {
            if (System.IO.File.Exists(filename.ToString()))
            {
                Console.WriteLine("Reading configuration file @ " + filename);
                Language2DFile file = JsonConvert.DeserializeObject<Language2DFile>(System.IO.File.ReadAllText(filename));

                _languageDict = new Dictionary<Noun2D, IList<Noun2D>>(new Noun2DEqualityComparer());
                _rand = new Random();

                foreach (Rule rule in file.Rules)
                    _languageDict[new Noun2D(rule.InNoun, file.Dimensions)] = (from n in rule.OutNouns select new Noun2D(n, file.Dimensions)).ToList();

                Console.WriteLine("Added " + _languageDict.Count + " rules to the language.");
            }
            else
            {
                Console.WriteLine("No configuration file found.");
                throw new Exception("No configureation file found.");
            }
        }

        /// <summary>
        /// Get a random Noun2D associated with the passed Noun2D,
        /// or null if no such noun exists.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public Noun2D this[Noun2D i]
        {
            get
            {
                if (_languageDict.ContainsKey(i))
                    return _languageDict[i][_rand.Next(_languageDict[i].Count)];
                else return null;
            }
        }
    }

    /// <summary>
    /// Model of a 2D L-System Language configuration file.
    /// </summary>
    public class Language2DFile
    {
        [JsonProperty("noun_dimensions")]
        public NounDimension Dimensions { get; set; }

        [JsonProperty("language")]
        public IList<Rule> Rules { get; set; }
    }

    /// <summary>
    /// A Rule making up part of a 2D L-System Language.
    /// </summary>
    public class Rule
    {
        [JsonProperty("in")]
        public IList<string> InNoun { get; set; }

        [JsonProperty("out")]
        public IList<IList<string>> OutNouns { get; set; }
    }

    /// <summary>
    /// Dimensions of the given 2DLanguage nouns.
    /// </summary>
    public class NounDimension
    {
        [JsonProperty("height")]
        public int Height { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }

        public NounDimension() { }
    }

    public class Noun2D
    {
        public string[] Word2d { get; set; }

        /// <summary>
        /// Build a new Noun2D.
        /// Check the passed dimension to make sure the word is the correct dimensions.
        /// </summary>
        /// <param name="word2d"></param>
        /// <param name="dimension"></param>
        public Noun2D(IList<string> word2d, NounDimension dimension)
        {
            if (string.Join("", word2d).Length != dimension.Height * dimension.Width)
                throw new Exception("Noun2D block is incorrect dimension:\n" + string.Join("\n", word2d));

            Word2d = word2d.ToArray();
        }

        public Noun2D() { }

        public override string ToString()
        {
            return string.Join("\n", Word2d);
        }
    }

    /// <summary>
    /// Equality Comparer for Noun2D.
    /// Allows us to use Noun2D objects to key dictionaries.
    /// </summary>
    public class Noun2DEqualityComparer : IEqualityComparer<Noun2D>
    {
        public bool Equals(Noun2D x, Noun2D y)
        {
            return string.Join(string.Empty, x.Word2d).CompareTo(string.Join(string.Empty, y.Word2d)) == 0;
        }

        public int GetHashCode(Noun2D obj)
        {
            int hash = obj.Word2d[0].GetHashCode();

            for (int i = 1; i < obj.Word2d.Length; i++)
                hash ^= obj.Word2d[i].GetHashCode();

            return hash;
        }
    }
}
