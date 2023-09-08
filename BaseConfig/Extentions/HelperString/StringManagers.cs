using Slugify;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace BaseConfig.Extentions.String
{
    public static class StringManagers
    {
        private static Random _randomTemp = new(DateTime.UtcNow.Millisecond);
        private static SlugHelper _slugHelper = new();
        public static string WithRegex(string text)
        {
            return Regex.Replace(text, @"\s+", " ");
        }
        public static long GenerateOtp()
        {
            HashSet<long> numberOtps = new();
            HashSet<long> fourDigitNumbers = new();

            for (long i = 0; i < 100; i++)
            {
                numberOtps.Add(_randomTemp.Next(9, 999));
            }

            foreach (long number in numberOtps)
            {
                if (number >= 10 && number <= 99)
                {
                    long digit1 = number / 10;
                    long digit2 = number % 10;

                    for (long i = 0; i < numberOtps.Count; i++)
                    {
                        for (long j = 0; j < numberOtps.Count; j++)
                        {
                            long fourDigitNumber = digit1 * 1000 + digit2 * 100 + i * 10 + j;
                            if (fourDigitNumber >= 1000 && fourDigitNumber <= 9999)
                            {
                                fourDigitNumbers.Add(fourDigitNumber);
                            }
                        }
                    }
                }
                else if (number >= 100 && number <= 999)
                {
                    long digit1 = number / 100;
                    long remainingDigits = number % 100;

                    for (long i = 0; i < numberOtps.Count; i++)
                    {
                        long fourDigitNumber = digit1 * 1000 + remainingDigits * 10 + i;
                        if (fourDigitNumber >= 1000 && fourDigitNumber <= 9999)
                        {
                            fourDigitNumbers.Add(fourDigitNumber);
                        }
                    }
                }
            }
            List<long> tempFourDigitNumber = fourDigitNumbers.ToList().Where(x => x >= 1000 && x.ToString()[0] != 0).Select(x => x).ToList();
            int indexRandom = _randomTemp.Next(0, tempFourDigitNumber.Count - 1);

            return tempFourDigitNumber.IndexOf(indexRandom);
        }
        static public string EncodeTo64(this string toEncode)
        {
            byte[] toEncodeAsBytes = Encoding.ASCII.GetBytes(toEncode);

            string returnValue = Convert.ToBase64String(toEncodeAsBytes);

            return returnValue;
        }
        static public string DecodeFrom64(this string encodedData)
        {
            byte[] encodedDataAsBytes = Convert.FromBase64String(encodedData);

            string returnValue = Encoding.ASCII.GetString(encodedDataAsBytes);

            return returnValue;
        }
        /// <summary>
        /// Nomarlize text after genarate text to slug
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static string NormalizeString(this string input)
        {
            input = input.ToLower().Replace('đ', 'd');
            var normalizedStringBuilder = new StringBuilder();
            foreach (char c in input.Normalize(NormalizationForm.FormD))
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    normalizedStringBuilder.Append(c);
                }
            }
            return normalizedStringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
        static public string GenerateSlug(this string textString)
       => _slugHelper.GenerateSlug(textString.NormalizeString());
    }

}
