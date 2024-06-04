using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QRCoder;

public static partial class PayloadGenerator
{
    /// <summary>
    /// Generates a ShadowSocks proxy config payload.
    /// </summary>
    public class ShadowSocksConfig : Payload
    {
        private readonly string hostname, password, methodStr;
        private readonly string? tag, parameter;
        private readonly Method method;
        private readonly int port;
        private Dictionary<string, string> encryptionTexts = new Dictionary<string, string>() {
            { "Chacha20IetfPoly1305", "chacha20-ietf-poly1305" },
            { "Aes128Gcm", "aes-128-gcm" },
            { "Aes192Gcm", "aes-192-gcm" },
            { "Aes256Gcm", "aes-256-gcm" },

            { "XChacha20IetfPoly1305", "xchacha20-ietf-poly1305" },

            { "Aes128Cfb", "aes-128-cfb" },
            { "Aes192Cfb", "aes-192-cfb" },
            { "Aes256Cfb", "aes-256-cfb" },
            { "Aes128Ctr", "aes-128-ctr" },
            { "Aes192Ctr", "aes-192-ctr" },
            { "Aes256Ctr", "aes-256-ctr" },
            { "Camellia128Cfb", "camellia-128-cfb" },
            { "Camellia192Cfb", "camellia-192-cfb" },
            { "Camellia256Cfb", "camellia-256-cfb" },
            { "Chacha20Ietf", "chacha20-ietf" },

            { "Aes256Cb", "aes-256-cfb" },

            { "Aes128Ofb", "aes-128-ofb" },
            { "Aes192Ofb", "aes-192-ofb" },
            { "Aes256Ofb", "aes-256-ofb" },
            { "Aes128Cfb1", "aes-128-cfb1" },
            { "Aes192Cfb1", "aes-192-cfb1" },
            { "Aes256Cfb1", "aes-256-cfb1" },
            { "Aes128Cfb8", "aes-128-cfb8" },
            { "Aes192Cfb8", "aes-192-cfb8" },
            { "Aes256Cfb8", "aes-256-cfb8" },

            { "Chacha20", "chacha20" },
            { "BfCfb", "bf-cfb" },
            { "Rc4Md5", "rc4-md5" },
            { "Salsa20", "salsa20" },

            { "DesCfb", "des-cfb" },
            { "IdeaCfb", "idea-cfb" },
            { "Rc2Cfb", "rc2-cfb" },
            { "Cast5Cfb", "cast5-cfb" },
            { "Salsa20Ctr", "salsa20-ctr" },
            { "Rc4", "rc4" },
            { "SeedCfb", "seed-cfb" },
            { "Table", "table" }
        };

        /// <summary>
        /// Generates a ShadowSocks proxy config payload.
        /// </summary>
        /// <param name="hostname">Hostname of the ShadowSocks proxy</param>
        /// <param name="port">Port of the ShadowSocks proxy</param>
        /// <param name="password">Password of the SS proxy</param>
        /// <param name="method">Encryption type</param>
        /// <param name="tag">Optional tag line</param>
        public ShadowSocksConfig(string hostname, int port, string password, Method method, string? tag = null) :
            this(hostname, port, password, method, null, tag)
        { }

        /// <summary>
        /// Generates a ShadowSocks proxy config payload with plugin options.
        /// </summary>
        /// <param name="hostname">Hostname of the ShadowSocks proxy</param>
        /// <param name="port">Port of the ShadowSocks proxy</param>
        /// <param name="password">Password of the SS proxy</param>
        /// <param name="method">Encryption type</param>
        /// <param name="plugin">Plugin name</param>
        /// <param name="pluginOption">Plugin option</param>
        /// <param name="tag">Optional tag line</param>
        public ShadowSocksConfig(string hostname, int port, string password, Method method, string plugin, string? pluginOption, string? tag = null) :
            this(hostname, port, password, method, new Dictionary<string, string>
            {
                ["plugin"] = plugin + (
                string.IsNullOrEmpty(pluginOption)
                ? ""
                : $";{pluginOption}"
            )
            }, tag)
        { }
        private Dictionary<string, string> UrlEncodeTable = new Dictionary<string, string>
        {
            [" "] = "+",
            ["\0"] = "%00",
            ["\t"] = "%09",
            ["\n"] = "%0a",
            ["\r"] = "%0d",
            ["\""] = "%22",
            ["#"] = "%23",
            ["$"] = "%24",
            ["%"] = "%25",
            ["&"] = "%26",
            ["'"] = "%27",
            ["+"] = "%2b",
            [","] = "%2c",
            ["/"] = "%2f",
            [":"] = "%3a",
            [";"] = "%3b",
            ["<"] = "%3c",
            ["="] = "%3d",
            [">"] = "%3e",
            ["?"] = "%3f",
            ["@"] = "%40",
            ["["] = "%5b",
            ["\\"] = "%5c",
            ["]"] = "%5d",
            ["^"] = "%5e",
            ["`"] = "%60",
            ["{"] = "%7b",
            ["|"] = "%7c",
            ["}"] = "%7d",
            ["~"] = "%7e",
        };

        private string UrlEncode(string i)
        {
            string j = i;
            foreach (var kv in UrlEncodeTable)
            {
                j = j.Replace(kv.Key, kv.Value);
            }
            return j;
        }

        /// <summary>
        /// Generates a ShadowSocks proxy config payload with additional parameters.
        /// </summary>
        /// <param name="hostname">Hostname of the ShadowSocks proxy</param>
        /// <param name="port">Port of the ShadowSocks proxy</param>
        /// <param name="password">Password of the SS proxy</param>
        /// <param name="method">Encryption type</param>
        /// <param name="parameters">Additional parameters</param>
        /// <param name="tag">Optional tag line</param>
        public ShadowSocksConfig(string hostname, int port, string password, Method method, Dictionary<string, string>? parameters, string? tag = null)
        {
            this.hostname = Uri.CheckHostName(hostname) == UriHostNameType.IPv6
                ? $"[{hostname}]"
                : hostname;
            if (port < 1 || port > 65535)
                throw new ShadowSocksConfigException("Value of 'port' must be within 0 and 65535.");
            this.port = port;
            this.password = password;
            this.method = method;
            this.methodStr = encryptionTexts[method.ToString()];
            this.tag = tag;

            if (parameters != null)
                this.parameter =
                    string.Join("&",
                    parameters.Select(
                        kv => $"{UrlEncode(kv.Key)}={UrlEncode(kv.Value)}"
                    ).ToArray());
        }

        /// <summary>
        /// Converts the ShadowSocks config payload to a string.
        /// </summary>
        /// <returns>A string representation of the ShadowSocks config payload.</returns>
        public override string ToString()
        {
            if (string.IsNullOrEmpty(parameter))
            {
                var connectionString = $"{methodStr}:{password}@{hostname}:{port}";
                var connectionStringEncoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(connectionString));
                return $"ss://{connectionStringEncoded}{(!string.IsNullOrEmpty(tag) ? $"#{tag}" : string.Empty)}";
            }
            var authString = $"{methodStr}:{password}";
            var authStringEncoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(authString))
                .Replace('+', '-')
                .Replace('/', '_')
                .TrimEnd('=');
            return $"ss://{authStringEncoded}@{hostname}:{port}/?{parameter}{(!string.IsNullOrEmpty(tag) ? $"#{tag}" : string.Empty)}";
        }

        /// <summary>
        /// Specifies the encryption methods used by ShadowSocks.
        /// </summary>
        public enum Method
        {
            // AEAD
            Chacha20IetfPoly1305,
            Aes128Gcm,
            Aes192Gcm,
            Aes256Gcm,
            // AEAD, not standard
            XChacha20IetfPoly1305,
            // Stream cipher
            Aes128Cfb,
            Aes192Cfb,
            Aes256Cfb,
            Aes128Ctr,
            Aes192Ctr,
            Aes256Ctr,
            Camellia128Cfb,
            Camellia192Cfb,
            Camellia256Cfb,
            Chacha20Ietf,
            // alias of Aes256Cfb
            Aes256Cb,
            // Stream cipher, not standard
            Aes128Ofb,
            Aes192Ofb,
            Aes256Ofb,
            Aes128Cfb1,
            Aes192Cfb1,
            Aes256Cfb1,
            Aes128Cfb8,
            Aes192Cfb8,
            Aes256Cfb8,
            // Stream cipher, deprecated
            Chacha20,
            BfCfb,
            Rc4Md5,
            Salsa20,
            // Not standard and not in active use
            DesCfb,
            IdeaCfb,
            Rc2Cfb,
            Cast5Cfb,
            Salsa20Ctr,
            Rc4,
            SeedCfb,
            Table
        }

        /// <summary>
        /// Represents errors that occur during the generation of a ShadowSocksConfig payload.
        /// </summary>
        public class ShadowSocksConfigException : Exception
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ShadowSocksConfigException"/> class.
            /// </summary>
            public ShadowSocksConfigException()
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="ShadowSocksConfigException"/> class with a specified error message.
            /// </summary>
            /// <param name="message">The message that describes the error.</param>
            public ShadowSocksConfigException(string message)
                : base(message)
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="ShadowSocksConfigException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
            /// </summary>
            /// <param name="message">The message that describes the error.</param>
            /// <param name="inner">The exception that is the cause of the current exception.</param>
            public ShadowSocksConfigException(string message, Exception inner)
                : base(message, inner)
            {
            }
        }
    }
}
