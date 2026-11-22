using Microsoft.EntityFrameworkCore;

namespace JwServices
{
    /// <summary>工事の階数を超えて図面を取り込もうとしたとき（利用者向けの日本語メッセージを持つ）。</summary>
    public sealed class FloorLimitException : InvalidOperationException
    {
        public FloorLimitException(string message) : base(message) { }
    }

    /// <summary>
    /// 2026年11月22日 工事の「階数」と取り込み済みの階の数の照合。
    /// ・取り込み：取り込み済みの階数（実際の階データ件数）が工事の階数に達していたら取り込めない。
    /// ・階の削除：工事の階数は変えない（工事作成時に決める値）。取り込み済みの件数が減るので、その分また取り込める。
    /// ・階数の変更：取り込み済みの階数より小さくはできない。
    /// 件数は「解析数」(ParsedQuantity) ではなく実際の階データで数える（解析数は過去にずれていることがある）。
    /// </summary>
    public partial class JwProjectMainService
    {
        /// <summary>取り込み済みの階数と工事の階数。</summary>
        public async Task<(int Parsed, int Planned)> GetFloorCapacityAsync(long mainId)
        {
            int parsed = await context.JwProjectSubDatas.CountAsync(s => s.JwProjectMainDataId == mainId);
            var main = await context.JwProjectMainDatas.FindAsync(mainId);
            return (parsed, main?.FloorQuantity ?? 0);
        }

        /// <summary>取り込めない理由（取り込めるなら null）。</summary>
        public static string? ImportBlockedReason(int parsed, int planned)
        {
            if (planned <= 0)
                return "この工事の階数が 0 のため、図面を取り込めません。\n工事一覧の右クリック「階数を変更」で階数を設定してください。";
            if (parsed >= planned)
                return $"この工事は階数 {planned} 階分の図面がすべて取り込み済みです（取り込み済み {parsed} 階）。\n" +
                       "取り込み直す場合は不要な階を削除してください。階数そのものが違う場合は、工事一覧の右クリック「階数を変更」で修正できます。";
            return null;
        }

        /// <summary>取り込めない理由（取り込めるなら null）。</summary>
        public async Task<string?> CheckImportAsync(long mainId)
        {
            var (parsed, planned) = await GetFloorCapacityAsync(mainId);
            return ImportBlockedReason(parsed, planned);
        }

        /// <summary>取り込めなければ <see cref="FloorLimitException"/>。</summary>
        public async Task EnsureCanImportAsync(long mainId)
        {
            var reason = await CheckImportAsync(mainId);
            if (reason != null) throw new FloorLimitException(reason);
        }

        /// <summary>工事の階数を変更する（取り込み済みの階数未満、または 1 未満は不可）。</summary>
        public async Task ChangeFloorQuantityAsync(long mainId, int floorQuantity)
        {
            var (parsed, _) = await GetFloorCapacityAsync(mainId);
            int min = Math.Max(1, parsed);
            if (floorQuantity < min)
                throw new FloorLimitException($"階数は {min} 以上にしてください（取り込み済み {parsed} 階）。");
            await WriteAsync(async () =>
            {
                var main = await context.JwProjectMainDatas.FindAsync(mainId)
                           ?? throw new InvalidOperationException("工事が見つかりません。");
                main.FloorQuantity = floorQuantity;
                await context.SaveChangesAsync();
            });
        }
    }
}
