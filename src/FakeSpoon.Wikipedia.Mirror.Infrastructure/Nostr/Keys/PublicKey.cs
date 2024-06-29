using FakeSpoon.Wikipedia.Mirror.Infrastructure.Nostr.Utils;
using NBitcoin.Secp256k1;

namespace FakeSpoon.Wikipedia.Mirror.Infrastructure.Nostr.Keys;

public class PublicKey : IEquatable<PublicKey>
    {
        private PublicKey(string hex, string bech32, ECXOnlyPubKey ec)
        {
            Hex = hex;
            Bech32 = bech32;
            Ec = ec;
        }

        public string Hex { get; }

        public string Bech32 { get; }

        public ECXOnlyPubKey Ec { get; }

        /// <summary>
        /// Validate signature of the given hex by the public key
        /// </summary>
        public bool IsHexSignatureValid(string? signatureHex, string? hex)
        {
            if (string.IsNullOrWhiteSpace(signatureHex))
                return false;
            if (!SecpSchnorrSignature.TryCreate(HexExtensions.ToByteArray(signatureHex), out var schnorr))
                return false;
            var result = Ec.SigVerifyBIP340(schnorr, HexExtensions.ToByteArray(hex ?? string.Empty));
            return result;
        }

        public bool Equals(PublicKey? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Hex, other.Hex, StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(Bech32, other.Bech32, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PublicKey)obj);
        }

        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            hashCode.Add(Hex, StringComparer.OrdinalIgnoreCase);
            hashCode.Add(Bech32, StringComparer.OrdinalIgnoreCase);
            return hashCode.ToHashCode();
        }

        public static PublicKey FromHex(string hex)
        {
            var ec = ECXOnlyPubKey.Create(HexExtensions.ToByteArray(hex));
            var bech32 = NostrConverter.ToNpub(hex) ?? string.Empty;
            return new PublicKey(hex, bech32, ec);
        }

        public static PublicKey FromBech32(string bech32)
        {
            var hex = NostrConverter.ToHex(bech32, out var hrp);
            if (!"npub".Equals(hrp, StringComparison.OrdinalIgnoreCase) || string.IsNullOrWhiteSpace(hex))
                throw new ArgumentException("Provided bech32 key is not 'npub'", nameof(bech32));
            return FromHex(hex);
        }

        public static PublicKey FromEc(ECXOnlyPubKey ec)
        {
            var hex = ec.ToBytes().ToHex();
            if (string.IsNullOrWhiteSpace(hex))
                throw new ArgumentException("Provided ec key is not correct", nameof(ec));
            return FromHex(hex);
        }

        public static PublicKey FromPrivateEc(ECPrivKey ec)
        {
            var publicEc = ec.CreateXOnlyPubKey();
            return FromEc(publicEc);
        }
}