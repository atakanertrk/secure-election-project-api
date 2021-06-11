using System;
using Xunit;
using WebAPI.Helpers;

namespace WebPI_TestProject
{
    public class HashingHelperTests
    {
        [Theory]
        [InlineData("MyPlainText", "7b096209599f8009e4b36bbdefd1113806c974acf52c570df247c320591fe04a")]
        [InlineData("3453452", "1eabab79079dce16e3144bd2d564c3f2dd4c14bdd360d6ade529151328d6687b")]
        [InlineData(" ", "cipher text is null or empty")]
        [InlineData("", "cipher text is null or empty")]
        [InlineData("!şçöi,<", "838cb71d5bd6329fef90f64d06d082810d898dd6dc55f1d2ff5d0875c0b1dd78")]
        public void EncryptSHA256_MustCreateValidHashes(string plainText, string hashedText)
        {
            Assert.Equal(hashedText, HashingHelper.EncryptSHA256(plainText));
        }
    }
}
