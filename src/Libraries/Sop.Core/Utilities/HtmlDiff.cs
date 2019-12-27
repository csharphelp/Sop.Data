﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Sop.Core.Utilities;

namespace Sop.Data.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    public class HtmlDiff
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldText"></param>
        /// <param name="newText"></param>
        /// <returns></returns>
        public static string Execute(string oldText, string newText)
        {
            return new HtmlDiff(oldText, newText).Build();
        }

        private StringBuilder content;
        private string oldText;
        private readonly string newText;
        private string[] oldWords, _newWords;
        private Dictionary<string, List<int>> wordIndices;
        private string[] specialCaseOpeningTags = new string[] { "<strong[\\>\\s]+", "<b[\\>\\s]+", "<i[\\>\\s]+", "<big[\\>\\s]+", "<small[\\>\\s]+", "<u[\\>\\s]+", "<sub[\\>\\s]+", "<sup[\\>\\s]+", "<strike[\\>\\s]+", "<s[\\>\\s]+" };
        private string[] specialCaseClosingTags = new string[] { "</strong>", "</b>", "</i>", "</big>", "</small>", "</u>", "</sub>", "</sup>", "</strike>", "</s>" };

        /// <summary>
        /// Initializes a new instance of the <see cref="Diff"/> class.
        /// </summary>
        /// <param name="oldText">The old text.</param>
        /// <param name="newText">The new text.</param>
        public HtmlDiff(string oldText, string newText)
        {
            this.oldText = oldText;
            this.newText = newText;

            this.content = new StringBuilder();
        }

        /// <summary>
        /// Builds the HTML diff output
        /// </summary>
        /// <returns>HTML diff markup</returns>
        public string Build()
        {
            this.SplitInputsToWords();

            this.IndexNewWords();

            var operations = this.Operations();

            foreach (var item in operations)
            {
                this.PerformOperation(item);
            }

            return this.content.ToString();
        }

        private void IndexNewWords()
        {
            this.wordIndices = new Dictionary<string, List<int>>();
            for (int i = 0; i < this._newWords.Length; i++)
            {
                string word = this._newWords[i];

                // if word is a tag, we should ignore attributes as attribute changes are not supported (yet)
                if (IsTag(word))
                {
                    word = StripTagAttributes(word);
                }

                if (this.wordIndices.ContainsKey(word))
                {
                    this.wordIndices[word].Add(i);
                }
                else
                {
                    this.wordIndices[word] = new List<int>();
                    this.wordIndices[word].Add(i);
                }
            }
        }

        private static string StripTagAttributes(string word)
        {
            var tag = Regex.Match(word, @"<[^\s>]+", RegexOptions.None).Value;
            word = tag + (word.EndsWith("/>") ? "/>" : ">");
            return word;
        }

        private void SplitInputsToWords()
        {
            this.oldWords = ConvertHtmlToListOfWords(Explode(this.oldText));
            this._newWords = ConvertHtmlToListOfWords(Explode(this.newText));
        }

        private string[] ConvertHtmlToListOfWords(string[] characterString)
        {
            HtmlDiffMode mode = HtmlDiffMode.character;
            string current_word = String.Empty;
            List<string> words = new List<string>();

            foreach (var character in characterString)
            {
                switch (mode)
                {
                    case HtmlDiffMode.character:

                        if (IsStartOfTag(character))
                        {
                            if (current_word != String.Empty)
                            {
                                words.Add(current_word);
                            }

                            current_word = "<";
                            mode = HtmlDiffMode.tag;
                        }
                        else if (Regex.IsMatch(character, @"\s", RegexOptions.ECMAScript))
                        {
                            if (current_word != String.Empty)
                            {
                                words.Add(current_word);
                            }
                            current_word = character;
                            mode = HtmlDiffMode.whitespace;
                        }
                        else if (Regex.IsMatch(character, @"[\w\#@]+", RegexOptions.IgnoreCase | RegexOptions.ECMAScript))
                        {
                            current_word += character;
                        }
                        else
                        {
                            if (current_word != String.Empty)
                            {
                                words.Add(current_word);
                            }
                            current_word = character;
                        }

                        break;

                    case HtmlDiffMode.tag:

                        if (IsEndOfTag(character))
                        {
                            current_word += ">";
                            words.Add(current_word);
                            current_word = "";

                            if (IsWhiteSpace(character))
                            {
                                mode = HtmlDiffMode.whitespace;
                            }
                            else
                            {
                                mode = HtmlDiffMode.character;
                            }
                        }
                        else
                        {
                            current_word += character;
                        }

                        break;

                    case HtmlDiffMode.whitespace:

                        if (IsStartOfTag(character))
                        {
                            if (current_word != String.Empty)
                            {
                                words.Add(current_word);
                            }
                            current_word = "<";
                            mode = HtmlDiffMode.tag;
                        }
                        else if (Regex.IsMatch(character, "\\s"))
                        {
                            current_word += character;
                        }
                        else
                        {
                            if (current_word != String.Empty)
                            {
                                words.Add(current_word);
                            }

                            current_word = character;
                            mode = HtmlDiffMode.character;
                        }

                        break;

                    default:
                        break;
                }
            }
            if (current_word != string.Empty)
            {
                words.Add(current_word);
            }

            return words.ToArray();
        }

        private void PerformOperation(HtmlDiffOperation operation)
        {
            switch (operation.Action)
            {
                case HtmlDiffAction.equal:
                    this.ProcessEqualOperation(operation);
                    break;

                case HtmlDiffAction.delete:
                    this.ProcessDeleteOperation(operation, "diffdel");
                    break;

                case HtmlDiffAction.insert:
                    this.ProcessInsertOperation(operation, "diffins");
                    break;

                case HtmlDiffAction.none:
                    break;

                case HtmlDiffAction.replace:
                    this.ProcessReplaceOperation(operation);
                    break;

                default:
                    break;
            }
        }

        private void ProcessReplaceOperation(HtmlDiffOperation operation)
        {
            this.ProcessDeleteOperation(operation, "diffmod");
            this.ProcessInsertOperation(operation, "diffmod");
        }

        private void ProcessInsertOperation(HtmlDiffOperation operation, string cssClass)
        {
            this.InsertTag("ins", cssClass, this._newWords.Where((s, pos) => pos >= operation.StartInNew && pos < operation.EndInNew).ToList());
        }

        private void ProcessDeleteOperation(HtmlDiffOperation operation, string cssClass)
        {
            var text = this.oldWords.Where((s, pos) => pos >= operation.StartInOld && pos < operation.EndInOld).ToList();
            this.InsertTag("del", cssClass, text);
        }

        private void ProcessEqualOperation(HtmlDiffOperation operation)
        {
            var result = this._newWords.Where((s, pos) => pos >= operation.StartInNew && pos < operation.EndInNew).ToArray();
            this.content.Append(String.Join("", result));
        }

        /// <summary>
        /// This method encloses words within a specified tag (ins or del), and adds this into "content",
        /// with a twist: if there are words contain tags, it actually creates multiple ins or del,
        /// so that they don't include any ins or del. This handles cases like
        /// old: '<p>a</p>'
        /// new: '<p>ab</p><p>c</b>'
        /// diff result: '<p>a<ins>b</ins></p><p><ins>c</ins></p>'
        /// this still doesn't guarantee valid HTML (hint: think about diffing a text containing ins or
        /// del tags), but handles correctly more cases than the earlier version.
        ///
        /// P.S.: Spare a thought for people who write HTML browsers. They live in this ... every day.
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="cssClass"></param>
        /// <param name="words"></param>
        private void InsertTag(string tag, string cssClass, List<string> words)
        {
            while (true)
            {
                if (words.Count == 0)
                {
                    break;
                }

                var nonTags = ExtractConsecutiveWords(words, x => !IsTag(x));

                string specialCaseTagInjection = string.Empty;
                bool specialCaseTagInjectionIsBefore = false;

                if (nonTags.Length != 0)
                {
                    string text = WrapText(string.Join("", nonTags), tag, cssClass);

                    this.content.Append(text);
                }
                else
                {
                    // Check if strong tag

                    if (this.specialCaseOpeningTags.FirstOrDefault(x => Regex.IsMatch(words[0], x)) != null)
                    {
                        specialCaseTagInjection = "<ins class='mod'>";
                        if (tag == "del")
                        {
                            words.RemoveAt(0);
                        }
                    }
                    else if (this.specialCaseClosingTags.Contains(words[0]))
                    {
                        specialCaseTagInjection = "</ins>";
                        specialCaseTagInjectionIsBefore = true;
                        if (tag == "del")
                        {
                            words.RemoveAt(0);
                        }
                    }
                }

                if (words.Count == 0 && specialCaseTagInjection.Length == 0)
                {
                    break;
                }

                if (specialCaseTagInjectionIsBefore)
                {
                    this.content.Append(specialCaseTagInjection + String.Join("", this.ExtractConsecutiveWords(words, x => IsTag(x))));
                }
                else
                {
                    this.content.Append(String.Join("", this.ExtractConsecutiveWords(words, x => IsTag(x))) + specialCaseTagInjection);
                }
            }
        }

        private string[] ExtractConsecutiveWords(List<string> words, Func<string, bool> condition)
        {
            int? indexOfFirstTag = null;

            for (int i = 0; i < words.Count; i++)
            {
                string word = words[i];

                if (!condition(word))
                {
                    indexOfFirstTag = i;
                    break;
                }
            }

            if (indexOfFirstTag != null)
            {
                var items = words.Where((s, pos) => pos >= 0 && pos < indexOfFirstTag).ToArray();
                if (indexOfFirstTag.Value > 0)
                {
                    words.RemoveRange(0, indexOfFirstTag.Value);
                }
                return items;
            }
            else
            {
                var items = words.Where((s, pos) => pos >= 0 && pos <= words.Count).ToArray();
                words.RemoveRange(0, words.Count);
                return items;
            }
        }

        private List<HtmlDiffOperation> Operations()
        {
            int positionInOld = 0, positionInNew = 0;
            List<HtmlDiffOperation> operations = new List<HtmlDiffOperation>();

            var matches = this.MatchingBlocks();

            matches.Add(new HtmlDiffMatch(this.oldWords.Length, this._newWords.Length, 0));

            for (int i = 0; i < matches.Count; i++)
            {
                var match = matches[i];

                bool matchStartsAtCurrentPositionInOld = (positionInOld == match.StartInOld);
                bool matchStartsAtCurrentPositionInNew = (positionInNew == match.StartInNew);

                HtmlDiffAction action = HtmlDiffAction.none;

                if (matchStartsAtCurrentPositionInOld == false
                    && matchStartsAtCurrentPositionInNew == false)
                {
                    action = HtmlDiffAction.replace;
                }
                else if (matchStartsAtCurrentPositionInOld == true
                    && matchStartsAtCurrentPositionInNew == false)
                {
                    action = HtmlDiffAction.insert;
                }
                else if (matchStartsAtCurrentPositionInOld == false
                    && matchStartsAtCurrentPositionInNew == true)
                {
                    action = HtmlDiffAction.delete;
                }
                else // This occurs if the first few words are the same in both versions
                {
                    action = HtmlDiffAction.none;
                }

                if (action != HtmlDiffAction.none)
                {
                    operations.Add(
                        new HtmlDiffOperation(action,
                            positionInOld,
                            match.StartInOld,
                            positionInNew,
                            match.StartInNew));
                }

                if (match.Size != 0)
                {
                    operations.Add(new HtmlDiffOperation(
                        HtmlDiffAction.equal,
                        match.StartInOld,
                        match.EndInOld,
                        match.StartInNew,
                        match.EndInNew));
                }

                positionInOld = match.EndInOld;
                positionInNew = match.EndInNew;
            }

            return operations;
        }

        private List<HtmlDiffMatch> MatchingBlocks()
        {
            List<HtmlDiffMatch> matchingBlocks = new List<HtmlDiffMatch>();
            this.FindMatchingBlocks(0, this.oldWords.Length, 0, this._newWords.Length, matchingBlocks);
            return matchingBlocks;
        }

        private void FindMatchingBlocks(int startInOld, int endInOld, int startInNew, int endInNew, List<HtmlDiffMatch> matchingBlocks)
        {
            var match = this.FindMatch(startInOld, endInOld, startInNew, endInNew);

            if (match != null)
            {
                if (startInOld < match.StartInOld && startInNew < match.StartInNew)
                {
                    this.FindMatchingBlocks(startInOld, match.StartInOld, startInNew, match.StartInNew, matchingBlocks);
                }

                matchingBlocks.Add(match);

                if (match.EndInOld < endInOld && match.EndInNew < endInNew)
                {
                    this.FindMatchingBlocks(match.EndInOld, endInOld, match.EndInNew, endInNew, matchingBlocks);
                }
            }
        }

        private HtmlDiffMatch FindMatch(int startInOld, int endInOld, int startInNew, int endInNew)
        {
            int bestMatchInOld = startInOld;
            int bestMatchInNew = startInNew;
            int bestMatchSize = 0;

            Dictionary<int, int> matchLengthAt = new Dictionary<int, int>();

            for (int indexInOld = startInOld; indexInOld < endInOld; indexInOld++)
            {
                var newMatchLengthAt = new Dictionary<int, int>();

                string index = this.oldWords[indexInOld];

                if (IsTag(index)) // strip attributes as this is not supported (yet)
                {
                    index = StripTagAttributes(index);
                }

                if (!this.wordIndices.ContainsKey(index))
                {
                    matchLengthAt = newMatchLengthAt;
                    continue;
                }

                foreach (var indexInNew in this.wordIndices[index])
                {
                    if (indexInNew < startInNew)
                    {
                        continue;
                    }

                    if (indexInNew >= endInNew)
                    {
                        break;
                    }

                    int newMatchLength = (matchLengthAt.ContainsKey(indexInNew - 1) ? matchLengthAt[indexInNew - 1] : 0) + 1;
                    newMatchLengthAt[indexInNew] = newMatchLength;

                    if (newMatchLength > bestMatchSize)
                    {
                        bestMatchInOld = indexInOld - newMatchLength + 1;
                        bestMatchInNew = indexInNew - newMatchLength + 1;
                        bestMatchSize = newMatchLength;
                    }
                }

                matchLengthAt = newMatchLengthAt;
            }

            return bestMatchSize != 0 ? new HtmlDiffMatch(bestMatchInOld, bestMatchInNew, bestMatchSize) : null;
        }

        private static string WrapText(string text, string tagName, string cssClass)
        {
            return string.Format("<{0} class='{1}'>{2}</{0}>", tagName, cssClass, text);
        }

        private static bool IsTag(string item)
        {
            bool isTag = IsOpeningTag(item) || IsClosingTag(item);
            return isTag;
        }

        private static bool IsOpeningTag(string item)
        {
            return Regex.IsMatch(item, "^\\s*<[^>]+>\\s*$");
        }

        private static bool IsClosingTag(string item)
        {
            return Regex.IsMatch(item, "^\\s*</[^>]+>\\s*$");
        }

        private static bool IsStartOfTag(string val)
        {
            return val == "<";
        }

        private static bool IsEndOfTag(string val)
        {
            return val == ">";
        }

        private static bool IsWhiteSpace(string value)
        {
            return Regex.IsMatch(value, "\\s");
        }

        private static string[] Explode(string value)
        {
            return Regex.Split(value, @"");
        }
    }
}