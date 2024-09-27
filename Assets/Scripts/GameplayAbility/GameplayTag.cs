using System;
using System.Collections.Generic;
using System.Linq;
using SolidUtilities;

namespace GameplayAbility
{
    [Serializable]
    public class GameplayTag :  IComparable<GameplayTag>, IEquatable<GameplayTag>
    {
        public int[] hashList;
        public int singleHashCode;
        
        public bool IsEmpty => hashList == null || hashList.Length == 0;
        
        public static GameplayTag FromString(string tagString)
        {
            GameplayTag tag = default;

            int dotCount = tagString.CountChars('.');
            if (dotCount < 0)
                return default;
            
            tag.hashList = new int[dotCount + 1];

            int hashIndex = 0;
            int lastDotIndex = -1;
            for (int i = 0; i < tagString.Length; ++i)
            {
                if (tagString[i] != '.')
                    continue;

                int length = i - lastDotIndex - 1;

                if (length > 0)
                {
                    var s = tagString.Substring(lastDotIndex + 1, length);
                    tag.hashList[hashIndex++] = s.GetHashCode();
                }
                 
                lastDotIndex = i;
            }

            var lastLength = tagString.Length - lastDotIndex - 1;
            if (lastLength > 0)
            {
                var last = tagString.Substring(lastDotIndex + 1, lastLength);
                tag.hashList[hashIndex] = last.GetHashCode();
            }

            tag.CalculateSingleHashCode();
            
            return tag;
        }
        
        public void CalculateSingleHashCode()
        {
            singleHashCode = hashList.Length;
            foreach (int val in hashList)
            {
                singleHashCode = unchecked(singleHashCode * 314159 + val);
            }
        }
        
        public override string ToString()
        {
            return hashList.ToString();
        }
        
        private int Compare(int[] x, int[] y)
        {
            // I made this part up. I don't actually know how
            // you want to handle nulls. 
            if (x == null && y == null) return 0;
            if (x == null) return 1;
            if (y == null) return -1;

            // Always compare the longer one to the shorter.
            // if this one is shorter, do the reverse comparison
            // and reverse the result.
            if (y.Length < x.Length) return -Compare(y, x);
            if (x.SequenceEqual(y)) return 0;
            for (var index = 0; index < x.Length; index++)
            {
                var comparison = x[index].CompareTo(y[index]);
                if (comparison != 0) return comparison;
            }

            // If the other list is longer than this one, then assume
            // that the next element of this list is 0.  
            return -y[x.Length];
        }
        
        public int CompareTo(GameplayTag other)
        {
            return Compare(hashList, other.hashList);
        }

        public bool Equals(GameplayTag other)
        {
            if(hashList == null && other.hashList == null)
                return true;

            if (hashList == null || other.hashList == null)
                return false;
            
            if(hashList.Length != other.hashList.Length)
                return false;

            return hashList.SequenceEqual(other.hashList);
        }

        public override bool Equals(object obj)
        {
            return obj is GameplayTag other && Equals(other);
        }
           public override int GetHashCode() => singleHashCode;
        
        public static bool operator ==(GameplayTag left, GameplayTag right) => left.Equals(right);

        public static bool operator !=(GameplayTag left, GameplayTag right) => !(left == right);

        // Match("A.1", "A") == true
        // Match("A", "A.1") == false
        // Match("A.1", "B") == false
        public static bool MatchesTag(GameplayTag tag, GameplayTag tagToCheck)
        {
            if (tag.hashList.Length < tagToCheck.hashList.Length)
                return false;

            for (int i = 0; i < tagToCheck.hashList.Length; ++i)
            {
                if (tag.hashList[i] != tagToCheck.hashList[i])
                    return false;
            }

            return true;
        }

        public static bool MatchesTagExact(GameplayTag tag, GameplayTag tagToCheck)
        {
            if (tag.hashList.Length != tagToCheck.hashList.Length)
                return false;

            return MatchesTag(tag, tagToCheck);
        }

        public static bool MatchAny(GameplayTag tag, GameplayTag[] tagsToCheck)
        {
            for (int j = 0; j < tagsToCheck.Length; ++j)
                if (GameplayTag.MatchesTag(tag, tagsToCheck[j]))
                    return true;

            return false;
        }

        public static bool MatchAnyExact(GameplayTag tag, GameplayTag[] tagsToCheck)
        {
            for (int j = 0; j < tagsToCheck.Length; ++j)
                if (GameplayTag.MatchesTagExact(tag, tagsToCheck[j]))
                    return true;

            return false;
        }

        public static bool HasTag(GameplayTag[] tags, GameplayTag tagToCheck)
        {
            for (int i = 0; i < tags.Length; ++i)
                if (MatchesTag(tags[i], tagToCheck))
                    return true;
            return false;
        }

        public static bool HasTagExact(GameplayTag[] tags,  GameplayTag tagToCheck)
        {
            for (int i = 0; i < tags.Length; ++i)
                if (MatchesTagExact(tags[i], tagToCheck))
                    return true;
            return false;
        }

        public static bool HasAny(GameplayTag[] tags, GameplayTag[] tagsToCheck)
        {
            for (int i = 0; i < tagsToCheck.Length; ++i)
                if (HasTag(tags, tagsToCheck[i]))
                    return true;
            return false;
        }

        public static bool HasAnyExact(GameplayTag[] tags, GameplayTag[] tagsToCheck)
        {
            for (int i = 0; i < tagsToCheck.Length; ++i)
                if (HasTagExact(tags, tagsToCheck[i]))
                    return true;
            return false;
        }

        public static bool HasAll(GameplayTag[] tags, GameplayTag[] tagsToCheck)
        {
            for (int i = 0; i < tagsToCheck.Length; ++i)
                if (!HasTag(tags, tagsToCheck[i]))
                    return false;
            return true;
        }

        public static bool HasAllExact(GameplayTag[] tags, GameplayTag[] tagsToCheck)
        {
            for (int i = 0; i < tagsToCheck.Length; ++i)
                if (!HasTagExact(tags, tagsToCheck[i]))
                    return false;
            return true;
        }
        
        public static bool HasTag(List<GameplayTagSO> tagRefs, GameplayTag tagToCheck)
        {
            for (int i = 0; i < tagRefs.Count; ++i)
                if (MatchesTag(tagRefs[i].tag, tagToCheck))
                    return true;
            return false;
        }

        public static bool HasTagExact(List<GameplayTagSO> tagRefs,  GameplayTag tagToCheck)
        {
            for (int i = 0; i < tagRefs.Count; ++i)
                if (MatchesTagExact(tagRefs[i].tag, tagToCheck))
                    return true;
            return false;
        }

        public static bool HasAny(List<GameplayTagSO> tagRefs, List<GameplayTagSO> tagRefsToCheck)
        {
            for (int i = 0; i < tagRefsToCheck.Count; ++i)
                if (HasTag(tagRefs, tagRefsToCheck[i].tag))
                    return true;
            return false;
        }

        public static bool HasAnyExact(List<GameplayTagSO> tagRefs, List<GameplayTagSO> tagRefsToCheck)
        {
            for (int i = 0; i < tagRefsToCheck.Count; ++i)
                if (HasTagExact(tagRefs, tagRefsToCheck[i].tag))
                    return true;
            return false;
        }

        public static bool HasAll(List<GameplayTagSO> tagRefs, List<GameplayTagSO> tagRefsToCheck)
        {
            for (int i = 0; i < tagRefsToCheck.Count; ++i)
                if (!HasTag(tagRefs, tagRefsToCheck[i].tag))
                    return false;
            return true;
        }

        public static bool HasAllExact(List<GameplayTagSO> tagRefs, List<GameplayTagSO> tagRefsToCheck)
        {
            for (int i = 0; i < tagRefsToCheck.Count; ++i)
                if (!HasTagExact(tagRefs, tagRefsToCheck[i].tag))
                    return false;
            return true;
        }
    }
}
