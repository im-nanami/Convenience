using Convenience.Data;
using Convenience.Models.DataModels;
using Convenience.Models.Interfaces;
using Convenience.Models.Properties;
using Convenience.Models.Properties.Config;
using Convenience.Models.ViewModels.Chumon;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text.RegularExpressions;
using static Convenience.Models.Properties.Config.Message;
using static Microsoft.AspNetCore.Razor.Language.TagHelperMetadata;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Convenience.Models.Services
{
    /// <summary>
    /// 注文サービスクラス
    /// </summary>
    public partial class ChumonService : IChumonService, ISharedTools, IDisposable
    {

        /// <summary>
        /// 注文オブジェクト用
        /// </summary>
        private readonly IChumon _chumon;//実際のデータアクセス処理（DB操作など）は _chumon 

        ///// <summary>
        ///// コンストラクタ 注文オブジェクト用記述
        ///// </summary>
        ////ConvenienceContext を渡すと Chumon インスタンスを作れる
        ////今は使われてないが、DIを使わない場合の予備用
        //private readonly Func<ConvenienceContext, IChumon> CreateChumonInstance = context => new Chumon(context);

        /// <summary>
        /// 注文キービューモデル（１枚目の画面用）
        /// </summary>
        public ChumonKeysViewModel ChumonKeysViewModel { get; set; } = new ChumonKeysViewModel();

        /// <summary>
        /// 注文明細ビューモデル（２枚目の画面用） 
        /// </summary>
        public ChumonViewModel ChumonViewModel { get; set; } = new ChumonViewModel();

        private bool _disposed = false;//Dispose処理用のフラグ
        private bool _createdComposition = false;//_createdComposition はこのコードでは false のまま。リソースを自分で作ったかどうかのチェック用

        /// <summary>
        /// コンストラクター　通常用
        /// </summary>
        /// <param name="context">DBコンテキスト</param>
        /// <param name="chumon">注文クラスＤＩ注入用</param>
        public ChumonService(IChumon chumon)
        {
            _chumon = chumon;
            //chumon = CreateChumonInstance(_context);
        }
        /// <summary>
        /// デバッグ用
        /// </summary>
        public ChumonService()
        {
            _chumon = new Chumon();
        }

        /// <summary>
        ///デストラクタ（アンマネージドリソース開放用）
        /// </summary>
        /*
         * 明示的に Dispose() が呼ばれなかったとき用。アンマネージドリソース解放の最終手段
         * C# のクラスが メモリから削除されるときに最後に呼ばれる特別なメソッド
         * オブジェクトが ガベージコレクション（GC）によって破棄されるときに呼び出される
         * リソースの後片付けを自動的に行いたいときに使われる
         * C# ではあまり頻繁に使わない。代わりに Dispose() メソッド（IDisposable） を使い、明示的にリソースを解放することが多い
         */
        ~ChumonService()
        {
            Dispose(false);
        }

        /// <summary>
        /// ファイナライザ
        /// </summary>
        /*
         * 明示的なリソース解放用。using 文で呼ばれることを想定。ファイナライザをキャンセル
         * ファイナライザとは…
         * 　C# でクラスが GC によって削除される直前に呼ばれる特別な処理のこと
         * 　実体は デストラクタ（~ClassName()） と同じもの
         * 　開放し忘れたアンマネージドリソースの最後の片付け場所として使われます
         * 注意！
         * ファイナライザは いつ呼ばれるか保証されない。処理が遅くなることがあるため、必要最低限で使うべき
         */
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// ファイナライザ（オーバーライド可）
        /// </summary>
        /*
         * オーバーライド：親クラスのメソッドを、子クラスで上書きして独自の動きをさせること
         *                「基底クラス（親）」から継承したメソッドを「派生クラス（子）」で変更できるようにする
         * 
         * マネージド／アンマネージド両方に対応したリリース処理
         * マネージドリソース
         * →.NET が自動的に管理してくれるリソース
         * 　メモリ上の変数、クラス、リスト、文字列など
         * 　GC（ガベージコレクター）が不要になったら勝手に回収してくれる
         * 　
         * アンマネージドリソース
         * 　.NET が自動的に管理できないリソース
         * 　OS や外部に依存しているもの（例: ファイル、データベース接続、ネットワークソケット、画像やウィンドウハンドル）
         * 　明示的に解放しないと、**メモリリーク（無駄なメモリ消費）**の原因になる
         */
        protected virtual void Dispose(bool disposing)
        {
            if ((!_disposed) && _createdComposition)
            {
                if (disposing)
                {
                    //マネージドリソース解放を書く
                    _chumon?.Dispose();
                }
                //アンマネージドリソース解放を書く

                //複数回実行しないように
                _disposed = true;
            }
        }

        /// <summary>
        /// 注文キービューモデル初期設定
        /// </summary>
        /// <returns>ChumonKeysViewModel 注文キービューモデル</returns>
        public async Task<ChumonKeysViewModel> SetChumonKeysViewModel()//注文キー入力画面の初期表示に必要なデータ（仕入先リスト）を作成
        {
            //仕入先リストをDBから取得。SelectListItem（DropDownList用）に変換
            var list = await _chumon.ShiireSakiList(s => s.ShiireSakiId)
                .Select(s => new SelectListItem { Value = s.Value, Text = s.Value + ":" + s.Text }).ToListAsync();
            /*
             * SelectListItem のインスタンスを作成
             * 　Value: 仕入先ID（s.Value）を Value プロパティにセット
             * 　Text: 仕入先ID（s.Value）と仕入先名（s.Text）を組み合わせて表示用のテキストを作成
             * 　例えば、"123: 仕入先A"
             * SetChumonKeysViewModel メソッドで作成されたリストは、
             * ChumonKeysViewModel の ShiireSakiList プロパティに格納され、その後ビューで表示される
             */

            //今日の日付をセットして ViewModel を返す
            return ChumonKeysViewModel = new ChumonKeysViewModel()
            {
                ShiireSakiId = null,// 初期状態では「仕入先ID」をnullにセット
                ChumonDate = DateOnly.FromDateTime(DateTime.Today),// 今日の日付を「注文日付」にセット
                ShiireSakiList = list// 仕入先リストをViewModelにセット
            };
            /*
             * _chumon.ShiireSakiList(s => s.ShiireSakiId)：
             * 　_chumon = 注文に関連するサービス。ShiireSakiList = 仕入先（シイレサキ）のリストを取得するメソッド
             * 　s => s.ShiireSakiId = 仕入先ID（主キー）のリストを取得するための選択条件
             * 　おそらく、これはDBから仕入先のデータをフィルタリングして取得している部分
             * 
             * Select(s => new SelectListItem {...})：
             * 　Select = LINQ のメソッドで（リストの各アイテムを変換するために使われる）、仕入先データを SelectListItem 型に変換
             * 　SelectListItem は、ASP.NET MVC や Razor Pages の「ドロップダウンリスト（選択リスト）」で使用されるアイテム型
             * 　                    ドロップダウンリストで使われる項目を表すクラス
             * 　Value: ドロップダウンで選ばれたときに送信される値
             * 　Text: ドロップダウンリストに表示されるテキスト
             * 　
             * ToListAsync() は非同期でリストに変換するためのメソッド。このメソッドが呼ばれることで、非同期にリストを取得
             * 　DBへの問い合わせは非同期で行われ、結果がリストとして返される
             * 　
             * ShiireSakiId = null：
             * 　ユーザーが仕入先を選択する前の状態としてnull をセットすることで、「選択されていない状態」にするため
             * 　
             * DateOnly は C# で日付（年月日）だけを扱いたいときに使用する構造体
             * 　DateTime.Today は現在の日付（年、月、日）を取得しますが、
             * 　DateOnly.FromDateTime を使うことで、時刻（時間、分、秒）は無視され、日付だけが取り出される
             * 　
             * ChumonKeysViewModel を新しく作成し、それを返す
             * 　ShiireSakiId は仕入先ID（初期値は null）
             * 　ChumonDate は注文日付（今日の日付）
             * 　ShiireSakiList は、先程非同期で取得した仕入先のリスト（SelectListItem のリスト）
             * ChumonKeysViewModel はビューに渡され、注文キー入力画面の初期表示に使用される
             */
        }

        /// <summary>
        /// 注文セッティング
        /// </summary>
        /// <param name="inChumonKeysViewModel">注文キー入力画面仕入先コード（画面より）</param>
        /// <returns>注文明細ビューモデル</returns>
        //入力された仕入先＋日付をもとに、注文データを取得または作成
        public async Task<ChumonViewModel> ChumonSetting(ChumonKeysViewModel inChumonKeysViewModel)
        {
            //仕入先コード抽出(仕入先コードが必須)
            string shiireSakiId = inChumonKeysViewModel?.ShiireSakiId ?? throw new ArgumentException("仕入先がセットされていません");
            //注文日付抽出　無効日付（1/1/1）のときは今日を使う
            DateOnly chumonDate =
                inChumonKeysViewModel.ChumonDate == DateOnly.FromDateTime(new DateTime(1, 1, 1))
                    ? DateOnly.FromDateTime(DateTime.Now)
                    : inChumonKeysViewModel.ChumonDate;

            //注文実績モデル変数定義
            ChumonJisseki? createdChumonJisseki = default, existedChumonJisseki = default;
            //もし、引数の注文日付がない場合（画面入力の注文日付が入力なしだと、1年1月1日になる
            if (DateOnly.FromDateTime(new DateTime(1, 1, 1)) == chumonDate)
            {
                //注文作成
                createdChumonJisseki =
                    await _chumon.ChumonSakusei(shiireSakiId, DateOnly.FromDateTime(DateTime.Now));   //注文日付が指定なし→注文作成
            }
            else
            {
                //注文日付指定あり→注文問い合わせ
                existedChumonJisseki = await _chumon.ChumonToiawase(shiireSakiId, chumonDate);

                if (existedChumonJisseki == null)
                {
                    //注文問い合わせでデータがない場合は、注文作成
                    createdChumonJisseki = await _chumon.ChumonSakusei(shiireSakiId, chumonDate);
                }
            }

            //注文明細ビューモデルを設定し戻り値とする
            return ChumonViewModel = new ChumonViewModel()
            {
                ChumonJisseki = createdChumonJisseki
                ?? existedChumonJisseki
                ?? throw new
                NoDataFoundException("設定する注文実績データ")   //初期表示用の注文実績データ
            };
        }

        /// <summary>
        /// 注文データをDBに書き込む
        /// </summary>
        /// <param name="inChumonJisseki">Postされた注文実績</param>
        /// <returns>ChumonViewModel 注文明細ビューモデル</returns>
        /// <exception cref="Exception">排他制御の例外が起きたらスローする</exception>
        public async Task<ChumonViewModel> ChumonCommit(ChumonViewModel inChumonViewModel)
        {

            //注文実績抽出
            ChumonJisseki postedchumonJisseki = inChumonViewModel.ChumonJisseki;

            //注文実績＋明細を問い合わせる　同一の注文がDBに存在するか確認
            ChumonJisseki? existedChumonJisseki
                = await _chumon.ChumonToiawase(postedchumonJisseki.ShiireSakiId, postedchumonJisseki.ChumonDate, false);

            //Postされたデータで注文実績と注文実績明細の更新
            ChumonJisseki updatedChumonJisseki;
            if (ISharedTools.IsExistCheck(existedChumonJisseki))
            {
                //Postデータ上書き
                updatedChumonJisseki = await _chumon.ChumonUpdate(postedchumonJisseki, existedChumonJisseki);
            }
            else
            {
                //Postデータをそのまま追加
                updatedChumonJisseki = await _chumon.ChumonUpdate(postedchumonJisseki);
            }
            //Postされた注文実績のデータチェック
            (bool IsValid, ErrDef errCd) = ChumonJissekiIsValid(updatedChumonJisseki);

            if (IsValid)
            {

                //DB更新
                int entities = await _chumon.ChumonSaveChanges();

                //再表示用データセット
                updatedChumonJisseki = await _chumon.ChumonToiawase(postedchumonJisseki.ShiireSakiId, postedchumonJisseki.ChumonDate)
                    ?? throw new NoDataFoundException("DB更新後の注文実績");


                //注文ビューモデルセット(正常時）
                //更新処理が成功した場合、ErrDef.NormalUpdate のメッセージを Message クラスを使って設定
                ChumonViewModel = new ChumonViewModel
                {
                    ChumonJisseki = updatedChumonJisseki,
                    IsNormal = IsValid,
                    Remark = errCd == ErrDef.DataValid && entities > 0 || errCd != ErrDef.DataValid
                    ? new Message().SetMessage(ErrDef.NormalUpdate)?.MessageText
                    : null
                };
            }
            else
            {
                //注文ビューモデルセット(チェックエラー時）
                //エラーが発生した場合、エラーコード errCd に対応するメッセージを Message クラスを使って設定
                ChumonViewModel = new ChumonViewModel
                {
                    ChumonJisseki = updatedChumonJisseki,
                    IsNormal = IsValid,
                    Remark = new Message().SetMessage(errCd)?.MessageText
                };
            }
            //注文明細ビューモデルを返却
            return ChumonViewModel;

        }

        /// <summary>
        /// Postされた注文実績のデータチェック
        /// </summary>
        /// <param name="inChumonJisseki">postされた注文実績</param>
        /// <returns>正常=true、異常=false、エラーコード</returns>
        private static (bool, ErrDef) ChumonJissekiIsValid(ChumonJisseki inChumonJisseki)
        {
            //注文IDを取得
            var chumonId = inChumonJisseki.ChumonId;
            var chumonDate = inChumonJisseki.ChumonDate;

            //注文ID (ChumonId) が null または空文字の場合は、正規表現でチェック
            if (!ChumonRegex().IsMatch(chumonId ?? string.Empty))
            {
                return (false, ErrDef.ChumonIdError);
            }
            //注文日 (ChumonDate) が最小値または 1/1/1 以前の日付である場合、無効と見なす
            //この場合、エラーコード ErrDef.ChumonDateError を返す
            else if (chumonDate == DateOnly.MinValue || chumonDate <= new DateOnly(1, 1, 1))
            {
                return (false, ErrDef.ChumonDateError);
            }
            //注文データに関連する注文明細（ChumonJissekiMeisais）が null であれば、注文明細が存在しないというエラー
            //この場合、エラーコード ErrDef.NothingChumonJisseki を返す
            if (inChumonJisseki?.ChumonJissekiMeisais is null)
            {
                return (false, ErrDef.NothingChumonJisseki);
            }

            foreach (var i in inChumonJisseki.ChumonJissekiMeisais)
            {
                //注文IDの一致: 各注文明細の ChumonId が、最初に確認した注文ID（chumonId）と一致しているかをチェック
                //もし一致しない場合、ErrDef.ChumonIdRelationError（注文ID不一致）のエラーコードを返す
                if (i.ChumonId != chumonId)
                {
                    return (false, ErrDef.ChumonIdRelationError);
                }
                //数量の妥当性: ChumonSu（注文数量）が0以上であるかをチェック
                //もし負の値があれば、ErrDef.ChumonSuBadRange（数量範囲エラー）のエラーコードを返す
                else if (i.ChumonSu < 0)
                {
                    return (false, ErrDef.ChumonSuBadRange);
                }
                //在庫と注文数量の比較: ChumonSu（注文数量）が ChumonZan（在庫数量）よりも少なくなければならない
                //もし注文数量が在庫数量を超えていれば、ErrDef.SuErrorBetChumonSuAndZan（注文数と在庫数の不一致エラー）のエラーコードを返す
                else if (i.ChumonSu < i.ChumonZan)
                {
                    return (false, ErrDef.SuErrorBetChumonSuAndZan);
                }
            }
            //すべてのチェックを通過した場合、正常（true）とし、エラーコード ErrDef.DataValid を返す
            return (true, ErrDef.DataValid);
        }

        /*
         * ChumonId（注文ID）を検証するための正規表現
         * 正規表現は「8桁の数字」と「-」と「3桁の数字」の形式を要求　（例: 12345678-123
         * [GeneratedRegex] 属性を使うことで、コンパイル時に正規表現を最適化して生成
         */
        [GeneratedRegex("^[0-9]{8}-[0-9]{3}$")]
        private static partial Regex ChumonRegex();
        //ChumonRegex は、注文IDが「8桁-3桁」の形式（例えば 12345678-123）になっていることを確認する正規表現
        //この形式に合わない場合は、エラーコード ErrDef.ChumonIdError を返す

        public Task<IActionResult> InsertRow(int index)
        {
            throw new NotImplementedException();
        }
    }
    /*
     * 注文キー入力画面の初期設定（仕入先リストと日付設定）
     * 注文データの設定（仕入先コードと日付で注文データを取得または作成）
     * 注文データのコミット（注文データの保存とバリデーション）
     * バリデーション（注文ID、日付、明細の整合性チェック）
     * リソース管理（Dispose パターンによるリソースの解放）
     */
}