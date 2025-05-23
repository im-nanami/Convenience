﻿using Convenience.Models.ViewModels.Chumon;

namespace Convenience.Models.Interfaces
{

    /// <summary>
    /// 注文サービスクラス用インターフェース
    /// </summary>
    //IDisposable：リソース（例えばDB接続やファイル操作）を使い終わった後に解放するためのメソッドDispose()を実装することを求めるインターフェース
    //　→IChumonServiceを使用するクラス（例えば、ChumonController）が終了する際にリソースのクリーンアップを適切に行えるようになる
    public interface IChumonService : IDisposable
    {

        /// <summary>
        /// 注文キービューモデル初期設定
        /// </summary>
        /// <returns>ChumonKeysViewModel 注文キービューモデル</returns>
        public Task<ChumonKeysViewModel> SetChumonKeysViewModel();

        /// <summary>
        /// 注文セッティング
        /// </summary>
        /// <param name="inShiireSakiId">仕入先コード（画面より）</param>
        /// <param name="inChumonDate">注文日付（画面より）</param>
        /// <returns>ChumonKeysViewModel 注文明細ビューモデル</returns>
        public Task<ChumonViewModel> ChumonSetting(ChumonKeysViewModel inChumonKeysViewModel);

        /// <summary>
        /// 注文データをDBに書き込む
        /// </summary>
        /// <param name="inChumonJisseki">Postされた注文実績</param>
        /// <returns>ChumonViewModel 注文明細ビューモデル</returns>
        /// <exception cref="Exception">排他制御の例外が起きたらスローする</exception>
        public Task<ChumonViewModel> ChumonCommit(ChumonViewModel inChumonViewModel);
    }

}