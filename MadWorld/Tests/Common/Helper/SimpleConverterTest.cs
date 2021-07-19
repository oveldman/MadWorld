using System;
using Common.Helper;
using Tests.Setup;
using Xunit;

namespace Tests.Common.Helper
{
    public class SimpleConverterTest
    {
        [Theory]
        [AutoDomainInlineData("Hello, this is a test!", "SGVsbG8sIHRoaXMgaXMgYSB0ZXN0IQ==")]
        [AutoDomainInlineData("", "")]
        [AutoDomainInlineData(null, "")]
        public void ConvertToBase64_PlainText_CorrectBase64(string plaintext, string expectedBase64)
        {
            // No extra Test data

            // No Setup

            // Act
            string base64value = SimpleConverter.ConvertToBase64(plaintext);

            // Assert
            Assert.Equal(expectedBase64, base64value);

            // No Teardown
        }

        [Theory]
        [AutoDomainInlineData("SGVsbG8sIHRoaXMgaXMgYSB0ZXN0IQ==", "Hello, this is a test!")]
        [AutoDomainInlineData("", "")]
        [AutoDomainInlineData(null, "")]
        public void ConvertToString_Base64Value_CorrectPlainText(string base64, string expectedPlainText)
        {
            // No extra Test data

            // No Setup

            // Act
            string plainText = SimpleConverter.ConvertToString(base64);

            // Assert
            Assert.Equal(expectedPlainText, plainText);

            // No Teardown
        }
    }
}
