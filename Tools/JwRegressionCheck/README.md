# JwRegressionCheck（开发用回归对比工具）

用于确认**修改代码后，jww 的解析结果和 CSV 输出没有变化**。不随软件发布。

## 用法

```
JwRegressionCheck --db <jwdata.db> --project <工事名> --floor <階名>
                  [--jww <図面.jww>] [--csv <已知正确的 CSV>] [--appconfig <appconfig.json>] [--repeat <次数>]
```

例（项目根目录下执行）：

```
Tools\JwRegressionCheck\bin\x64\Debug\net6.0-windows\JwRegressionCheck.exe ^
  --db RGBJWMain\bin\Debug\net6.0-windows\jwdata.db --project A --floor 2FC3 ^
  --jww 2FC.jww --csv 2FC-3015-2.csv ^
  --appconfig RGBJWMain\bin\Debug\net6.0-windows\appconfig.json --repeat 5
```

- 数据库会先复制再打开，**不会修改原数据库**。
- 结束码：全部一致为 0，有差异为 1，参数错误为 2。

## 检查内容

| 项目 | 内容 |
|---|---|
| [1] | 数据库中的楼层数据 → CSV（与画面「梁製造CSV」相同的处理）与已知 CSV 按字节比较 |
| [2] | 用当前代码解析 jww（解析设置从数据库的客户设置和楼层数据中还原），与数据库中的楼层数据逐实体比较（梁、孔、梁接触信息、柱、B/BG、连结、下階柱、切断、追加穴、楼层汇总值） |
| [3] | 把 [2] 的解析结果在内存中组装成与数据库相同的结构并生成 CSV，与已知 CSV 比较（jww → CSV 端到端） |

比较时排除 Id、创建时间、外键，以及保存时由材料设置的字段（`BeamXHId`/`BeamXHName` 等）。

## 关于「梁号的英文后缀」

形状完全相同的一组梁，其 A/B/C… 后缀按梁列表的顺序分配，而梁的顺序由并行的矩形识别决定，
**原始代码（d42f1c0）也会每次运行不同**（已用本工具在原始代码上确认）。
因此先做完全一致的比较，不一致时再去掉梁号末尾的英文字母比较：

- `[一致]`：完全一致
- `[一致] …（梁番号の英字サフィックスを除く）`：只有同组内后缀的分配不同，其余全部一致
- `[差分]`：存在真正的差异 → 会按位置对齐梁并列出不同的字段

## 已确认的基准（2026-11-22）

样本：项目根目录的 `2FC.jww`、`2FC-3015-2.csv`，数据库中「A」工事的 `2FC3` 层（K 柱 900、退化矩形除外 = 开）。
当前代码与原始代码 d42f1c0 各运行 10 次，均为「全部一致（除后缀分配）」，[1] 为字节完全一致。

> 样本图纸、CSV、数据库属于业务数据，不要提交到仓库。
