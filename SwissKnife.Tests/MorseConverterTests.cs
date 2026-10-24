///ETML
///Auteur : Kyllian Gregoire
///Date   : 23.10.2026
///Description : Tests unitaires xUnit de la conversion texte <-> Morse

using SwissKnife;
using Xunit;

namespace SwissKnife.Tests
{
    /// <summary>
    /// Vérifie les critères d'acceptation du cahier des charges n°1 (Morse)
    /// </summary>
    public class MorseConverterTests
    {
        [Fact]
        public void ConvertTextToMorse_Sos_GivesExpectedCode()
        {
            Assert.Equal("... --- ...", MorseConverter.ConvertTextToMorse("SOS"));
        }

        [Fact]
        public void ConvertTextToMorse_Bienvenue_MatchesSpecificationExample()
        {
            Assert.Equal("-... .. . -. ...- . -. ..- .", MorseConverter.ConvertTextToMorse("Bienvenue"));
        }

        [Fact]
        public void ConvertTextToMorse_Space_BecomesSlash()
        {
            Assert.Equal("... / ---", MorseConverter.ConvertTextToMorse("S O"));
        }

        [Fact]
        public void ConvertTextToMorse_IgnoresCaseAndDigitsAreSupported()
        {
            Assert.Equal(".- .----", MorseConverter.ConvertTextToMorse("a1"));
        }

        [Fact]
        public void ConvertTextToMorse_UnsupportedCharacters_AreIgnoredAndReported()
        {
            string morse = MorseConverter.ConvertTextToMorse("é!A", out List<char> ignoredCharacters);

            Assert.Equal(".-", morse);
            Assert.Equal(new List<char> { 'é', '!' }, ignoredCharacters);
        }

        [Fact]
        public void ConvertMorseToText_RoundTrip_GivesUpperCaseText()
        {
            string morse = MorseConverter.ConvertTextToMorse("Hello World 42");

            Assert.Equal("HELLO WORLD 42", MorseConverter.ConvertMorseToText(morse));
        }

        [Fact]
        public void ConvertMorseToText_UnknownCode_GivesQuestionMark()
        {
            Assert.Equal("S?S", MorseConverter.ConvertMorseToText("... ........ ..."));
        }

        [Theory]
        [InlineData("... --- ...")]
        [InlineData("-... / .-")]
        [InlineData(".")]
        public void IsMorse_DetectsMorseInput(string input)
        {
            Assert.True(MorseConverter.IsMorse(input));
        }

        [Theory]
        [InlineData("SOS")]
        [InlineData("Bienvenue")]
        [InlineData("a.b")]
        [InlineData("/ /")]
        [InlineData("")]
        public void IsMorse_RejectsTextInput(string input)
        {
            Assert.False(MorseConverter.IsMorse(input));
        }
    }
}
