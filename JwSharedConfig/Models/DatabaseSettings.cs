using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;

namespace JwSharedConfig.Models
{
    /// <summary>
    /// 2026年11月24日 データの保存先（ローカル SQLite／リモート SQL Server）。appconfig.json に保存。
    /// 切替は再起動後に反映。
    /// </summary>
    public class DatabaseSettings
    {
        public const string ModeLocal = "Local";
        public const string ModeRemote = "Remote";

        /// <summary>Local = 本機の jwdata.db（従来どおり）、Remote = SQL Server。</summary>
        public string Mode { get; set; } = ModeLocal;

        /// <summary>サーバー（IP、「IP,ポート」、「IP\インスタンス名」のいずれも可）。</summary>
        public string Server { get; set; } = "";

        public string Database { get; set; } = "";

        public string UserName { get; set; } = "";

        /// <summary>パスワード（Windows DPAPI・現在のユーザーで暗号化し Base64 で保存。平文は保存しない）。</summary>
        public string PasswordProtected { get; set; } = "";

        /// <summary>サーバー証明書を検証しない（社内サーバーの自己署名証明書向け）。</summary>
        public bool TrustServerCertificate { get; set; } = true;

        /// <summary>接続タイムアウト（秒）。</summary>
        public int ConnectTimeout { get; set; } = 15;

        [JsonIgnore]
        public bool IsRemote => string.Equals(Mode, ModeRemote, StringComparison.OrdinalIgnoreCase);

        private static readonly byte[] Entropy = Encoding.UTF8.GetBytes("RGBJW.Database.v1");

        public string GetPassword()
        {
            if (string.IsNullOrEmpty(PasswordProtected)) return "";
            try
            {
                var data = ProtectedData.Unprotect(Convert.FromBase64String(PasswordProtected), Entropy, DataProtectionScope.CurrentUser);
                return Encoding.UTF8.GetString(data);
            }
            catch
            {
                // 別のユーザー・別の PC で暗号化されたものは復号できない → 再入力してもらう
                return "";
            }
        }

        public void SetPassword(string? password)
        {
            if (string.IsNullOrEmpty(password))
            {
                PasswordProtected = "";
                return;
            }
            var data = ProtectedData.Protect(Encoding.UTF8.GetBytes(password), Entropy, DataProtectionScope.CurrentUser);
            PasswordProtected = Convert.ToBase64String(data);
        }

        public DatabaseSettings Clone() => (DatabaseSettings)MemberwiseClone();

        /// <summary>接続先として同じかどうか（再起動が必要かの判定用）。</summary>
        public bool SameTarget(DatabaseSettings other) =>
            IsRemote == other.IsRemote
            && (!IsRemote
                || (Server == other.Server && Database == other.Database && UserName == other.UserName
                    && GetPassword() == other.GetPassword()
                    && TrustServerCertificate == other.TrustServerCertificate && ConnectTimeout == other.ConnectTimeout));
    }
}
